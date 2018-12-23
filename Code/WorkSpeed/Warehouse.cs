using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkSpeed.Data.Models;
using NpoiExcel;
using NpoiExcel.Attributes;
using WorkSpeed;
using WorkSpeed.Data;
using WorkSpeed.Data.BusinessContexts;
using WorkSpeed.FileModels;
using WorkSpeed.FileModels.Converters;
using WorkSpeed.Interfaces;
using WorkSpeed.ProductivityCalculator;

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

        public Warehouse( IWorkSpeedBusinessContext context, IDataImporter dataImporter )
        {
            _context = context ?? throw new ArgumentNullException( nameof( context ) );
            _dataImporter = dataImporter ?? throw new ArgumentNullException( nameof( dataImporter ) );

            _typeRepository = new TypeRepository();
            AddTypesToRepository (_typeRepository);

            _productivities = new ProductivityObservableCollection();
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

        #endregion


        #region Properties

        public IEnumerable< Product > Products => _context.GetProducts();
        
        /// <summary>
        /// Entities that don't contained in DB.
        /// </summary>
        public IWarehouseEntities NewData { get; }


        public Task<bool> HasProductsAsync () => _context.HasProductsAsync();

        #endregion


        #region Methods

        public async Task<bool> ImportAsync< TImportModel > (string fileName, CancellationToken cancellationToken, IProgress<double> progress = null ) where TImportModel : ImportModel
        {
            return await Task.Run ( () => Import (fileName, typeof( TImportModel )), cancellationToken);
        }

        public async Task<bool> ImportAsync (string fileName)
        {
            return await Task<bool>.Factory.StartNew (() => Import (fileName), TaskCreationOptions.LongRunning);
        }


        private bool Import ( string fileName,  Type type = null )
        {
            var sheetTable = _dataImporter.ImportData (fileName, 0);
            var mappedType = _typeRepository.GetTypeWithMap ( sheetTable );

            if ( type != null && mappedType.type != null && !mappedType.type.IsAssignableFrom( type ) ) {
                return false;
            }

            if ( typeof( ProductImportModel ) == mappedType.type  ) {

                ImportProducts(
                    _dataImporter.GetEnumerable( 
                        sheetTable,  
                        mappedType.propertyMap,  
                        new ImportModelConverter< ProductImportModel, Product >( new ImportModelVisitor() ) 
                    )
                );
            }
            else if ( typeof( EmployeeImportModel ) == mappedType.type ) {

                ImportEmployees(
                    _dataImporter.GetEnumerable( 
                        sheetTable,  
                        mappedType.propertyMap,  
                        new ImportModelConverter< EmployeeImportModel, Employee >( new ImportModelVisitor() ) 
                    )
                );
            }
            else if ( typeof( EmployeeFullImportModel ) == mappedType.type ) {

                ImportEmployees(
                    _dataImporter.GetEnumerable(
                        sheetTable,
                        mappedType.propertyMap,
                        new ImportModelConverter< EmployeeFullImportModel, Employee >( new ImportModelVisitor() )
                    )
                );
            }
            else if ( typeof( ShipmentImportModel ) == mappedType.type ) {

                ImportShipmentActions(
                    _dataImporter.GetEnumerable( 
                        sheetTable,  
                        mappedType.propertyMap,  
                        new ImportModelConverter< ShipmentImportModel, ShipmentAction >( new ImportModelVisitor() ) 
                    )
                );
            }
            else if ( typeof( GatheringImportModel ) == mappedType.type ) {

                ImportGatheringActions(
                    _dataImporter.GetEnumerable( 
                        sheetTable,  
                        mappedType.propertyMap,  
                        new ImportModelConverter< GatheringImportModel, GatheringAction >( new ImportModelVisitor() ) 
                    )
                );
            }
            else if ( typeof( ReceptionImportModel ) == mappedType.type ) {

                ImportWithProductAction(
                    _dataImporter.GetEnumerable( 
                        sheetTable,  
                        mappedType.propertyMap,  
                        new ImportModelConverter< ReceptionImportModel, ReceptionAction >( new ImportModelVisitor() ) 
                    )
                );
            }
            else if ( typeof( InventoryImportModel ) == mappedType.type ) {

                ImportWithProductAction(
                    _dataImporter.GetEnumerable( 
                        sheetTable,  
                        mappedType.propertyMap,  
                        new ImportModelConverter< InventoryImportModel, InventoryAction >( new ImportModelVisitor() ) 
                    )
                );
            }
            else if ( typeof( ProductivityImportModel ) == mappedType.type ) {

                ImportProductivity(
                    _dataImporter.GetEnumerable( 
                        sheetTable,  
                        mappedType.propertyMap,  
                        new ImportModelConverter< ProductivityImportModel, EmployeeAction >( new ImportModelVisitor() ) 
                    )
                );
            }

            return true;
        }

        public (DateTime, DateTime) GetActionsPeriod ()
        {
            throw new NotImplementedException();
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
        private void ImportProductivity ( IEnumerable< EmployeeAction > employeeActions )
        {
            ImportGatheringActions( employeeActions.Where( a => a is GatheringAction ).Cast< GatheringAction >() );
            ImportWithProductAction( employeeActions.Where( a => a is ReceptionAction ).Cast< ReceptionAction >() );
            ImportWithProductAction( employeeActions.Where( a => a is InventoryAction ).Cast< InventoryAction >() );
            ImportShipmentActions( employeeActions.Where( a => a is ShipmentAction ).Cast< ShipmentAction >() );
        }

        private void ImportWithProductAction ( IEnumerable< WithProductAction > withProductActions )
        {

        }

        private void ImportGatheringActions ( IEnumerable< GatheringAction > gatheringActions )
        {
            foreach ( var gatheringAction in gatheringActions ) {
                _context.AddGatheringAction( gatheringAction );
            }
        }

        private void ImportShipmentActions ( IEnumerable< ShipmentAction > shipmentActions )
        {

        }

        #endregion
    }
}
