using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Agbm.NpoiExcel;
using Agbm.NpoiExcel.Attributes;
using WorkSpeed.Data.Models;
using WorkSpeed;
using WorkSpeed.Data;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.Data.Models.Actions;
using WorkSpeed.FileModels;
using WorkSpeed.FileModels.Converters;
using WorkSpeed.Interfaces;
using WorkSpeed.ProductivityCalculator;
using WorkSpeed.Productivity;

namespace WorkSpeed
{
    public class Warehouse : IWarehouse
    {
        #region Fields

        private readonly IWorkSpeedBusinessContext _context;
        private readonly IDataImporter _dataImporter;

        private readonly ITypeRepository _typeRepository;
        private readonly ProductivityObservableCollection _productivities;

        #endregion


        #region Constructor

        public Warehouse( IWorkSpeedBusinessContext context, IDataImporter dataImporter, IFactoryEmployeeAction factoryEmployeeAction )
        {
            _context = context ?? throw new ArgumentNullException( nameof( context ) );
            _dataImporter = dataImporter ?? throw new ArgumentNullException( nameof( dataImporter ) );
            FactoryEmployeeAction = factoryEmployeeAction ?? throw new ArgumentNullException( nameof( factoryEmployeeAction ) );

            _typeRepository = new TypeRepository();
            AddTypesToRepository (_typeRepository);

            _productivities = new ProductivityObservableCollection();
        }


        #endregion


        #region Context Methods

        public IFactoryEmployeeAction FactoryEmployeeAction { get; }

        public IEnumerable< Product > GetProducts() => _context.GetProducts();
        public IEnumerable< Employee > GetEmployees() => _context.GetEmployees();
        public IEnumerable< Appointment > GetAppointments() => _context.GetAppointments();
        public IEnumerable< Position > GetPositions() => _context.GetPositions();
        public IEnumerable< Rank > GetRanks() => _context.GetRanks();
        public IEnumerable < Shift > GetShifts () => _context.GetShifts();
        public IEnumerable< ShortBreakSchedule > GetBreakList () => _context.GetBreakList();
        public IEnumerable< DoubleAddressAction > GetGatheringActions () => _context.GetGatheringActions();
        public IEnumerable< Category > GetCategories () => FactoryEmployeeAction.GetCategories ();

        public TimeSpan GetThreshold () => FactoryEmployeeAction.GetThreshold();


        public Task<bool> HasProductsAsync () => _context.HasProductsAsync();

        #endregion


        #region Methods

        public async Task<bool> ImportAsync< TImportModel > (string fileName, CancellationToken cancellationToken, IProgress<double> progress = null ) where TImportModel : ImportModel
        {
            if ( progress != null ) {
                _dataImporter.ProgressChangedEvent += OnProgressChanged;
            }

            var res = await Task.Run ( () => Import (fileName, typeof( TImportModel )), cancellationToken);

            if ( progress != null ) {
                _dataImporter.ProgressChangedEvent -= OnProgressChanged;
            }

            return res;

            void OnProgressChanged ( object sender, ProgressChangedEventArgs args )
            {
                progress.Report( args.Progress );
            }
        }

        public IWarehouseEntities NewData { get; }

        public async Task<bool> ImportAsync (string fileName)
        {
            return await Task<bool>.Factory.StartNew (() => Import (fileName), TaskCreationOptions.LongRunning);
        }

        public (DateTime, DateTime) GetActionsPeriod ()
        {
            throw new NotImplementedException();
        }

        public void UpdateShiftBreaks ()
        {
            var shifts = _context.GetShifts();

            foreach ( var shift in shifts ) {
                FactoryEmployeeAction.AddVariableBreak( shift );
            }
        }

        public void UpdateFixedBreaks ( )
        {
            var breakList = _context.GetBreakList().ToArray();

            var forNotSmokers = breakList.FirstOrDefault( b => b.IsForSmokers == false );

            if ( forNotSmokers != null ) {

                FactoryEmployeeAction.AddFixedBreaks( forNotSmokers );
            }

            var forSmokers = breakList.FirstOrDefault( b => b.IsForSmokers );

            if ( forNotSmokers != null )
            {
                FactoryEmployeeAction.AddFixedBreaks( forSmokers );
            }
        }


        private void AddTypesToRepository ( ITypeRepository repo )
        {
            repo.RegisterType< ProductImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            repo.RegisterType< EmployeeImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repo.RegisterType< EmployeeFullImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            repo.RegisterType< ShipmentImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            repo.RegisterType< GatheringImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repo.RegisterType< InventoryImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repo.RegisterType< ReceptionImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            repo.RegisterType< ProductivityImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
        }

        private bool Import ( string fileName,  Type type = null )
        {
            //var sheetTable = _dataImporter.GetSheetTable (fileName, 0);
            //var mappedType = _typeRepository.GetTypeWithMap ( sheetTable );

            //if ( type != null && mappedType.type != null && !mappedType.type.IsAssignableFrom( type ) ) {
            //    return false;
            //}


            //if ( typeof( ProductImportModel ) == mappedType.type  ) {

            //    ImportProducts(
            //        _dataImporter.GetEnumerable( 
            //            sheetTable,  
            //            mappedType.propertyMap,  
            //            new ImportModelConverter< ProductImportModel, Product >( new ImportModelVisitor() ) 
            //        ).ToArray()
            //    );
            //}
            //else if ( typeof( EmployeeImportModel ) == mappedType.type ) {

            //    ImportEmployees(
            //        _dataImporter.GetEnumerable( 
            //            sheetTable,  
            //            mappedType.propertyMap,  
            //            new ImportModelConverter< EmployeeImportModel, Employee >( new ImportModelVisitor() ) 
            //        ).ToArray()
            //    );
            //}
            //else if ( typeof( EmployeeFullImportModel ) == mappedType.type ) {

            //    ImportEmployees(
            //        _dataImporter.GetEnumerable(
            //            sheetTable,
            //            mappedType.propertyMap,
            //            new ImportModelConverter< EmployeeFullImportModel, Employee >( new ImportModelVisitor() )
            //        ).ToArray()
            //    );
            //}
            //else if ( typeof( ShipmentImportModel ) == mappedType.type ) {

            //    ImportShipmentActions(
            //        _dataImporter.GetEnumerable( 
            //            sheetTable,  
            //            mappedType.propertyMap,  
            //            new ImportModelConverter< ShipmentImportModel, ShipmentAction >( new ImportModelVisitor() ) 
            //        ).ToArray()
            //    );
            //}
            //else if ( typeof( GatheringImportModel ) == mappedType.type ) {

            //    ImportGatheringActions(
            //        _dataImporter.GetEnumerable( 
            //            sheetTable,  
            //            mappedType.propertyMap,  
            //            new ImportModelConverter< GatheringImportModel, GatheringAction >( new ImportModelVisitor() ) 
            //        ).ToArray()
            //    );
            //}
            //else if ( typeof( ReceptionImportModel ) == mappedType.type ) {

            //    ImportSingleAction(
            //        _dataImporter.GetEnumerable( 
            //            sheetTable,  
            //            mappedType.propertyMap,  
            //            new ImportModelConverter< ReceptionImportModel, ReceptionAction >( new ImportModelVisitor() ) 
            //        ).ToArray()
            //    );
            //}
            //else if ( typeof( InventoryImportModel ) == mappedType.type ) {

            //    ImportSingleAction(
            //        _dataImporter.GetEnumerable( 
            //            sheetTable,  
            //            mappedType.propertyMap,  
            //            new ImportModelConverter< InventoryImportModel, InventoryAction >( new ImportModelVisitor() ) 
            //        ).ToArray()
            //    );
            //}
            //else if ( typeof( ProductivityImportModel ) == mappedType.type ) {

            //    ImportProductivity(
            //        _dataImporter.GetEnumerable( 
            //            sheetTable,  
            //            mappedType.propertyMap,  
            //            new ImportModelConverter< ProductivityImportModel, EmployeeAction >( new ImportModelVisitor() ) 
            //        ).ToArray()
            //    );
            //}

            return true;
        }

        private void ImportEmployees ( IEnumerable< Employee > employees )
        {
            foreach ( var employee in employees ) {
                _context.AddEmployee( employee );
            }
        }

        private void ImportProducts ( IEnumerable< Product > products )
        {
            foreach ( var product in products ) {
                _context.AddProduct( product );
            }
        }

        // ReSharper disable PossibleMultipleEnumeration
        private void ImportProductivity ( IEnumerable< EmployeeActionBase > employeeActions )
        {
            ImportGatheringActions( employeeActions.Where( a => a is DoubleAddressAction ).Cast< DoubleAddressAction >() );
            ImportSingleAction( employeeActions.Where( a => a is ReceptionAction ).Cast< ReceptionAction >() );
            ImportSingleAction( employeeActions.Where( a => a is InventoryAction ).Cast< InventoryAction >() );
            ImportShipmentActions( employeeActions.Where( a => a is ShipmentAction ).Cast< ShipmentAction >() );
        }

        private void ImportSingleAction ( IEnumerable< ReceptionAction > withProductActions )
        {

        }

        private void ImportSingleAction ( IEnumerable< InventoryAction > withProductActions )
        {

        }

        private void ImportGatheringActions ( IEnumerable< DoubleAddressAction > gatheringActions )
        {
            foreach ( var gatheringAction in gatheringActions ) {
                _context.AddGatheringAction( gatheringAction );
            }
        }

        private void ImportShipmentActions ( IEnumerable< ShipmentAction > shipmentActions )
        {

        }

        IEnumerable<DoubleAddressAction> IWarehouse.GetGatheringActions()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
