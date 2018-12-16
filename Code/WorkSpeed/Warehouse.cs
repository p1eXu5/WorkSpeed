using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using WorkSpeed.Data.Models;
using NpoiExcel;
using NpoiExcel.Attributes;
using WorkSpeed;
using WorkSpeed.Data;
using WorkSpeed.FileModels;
using WorkSpeed.FileModels.Converters;
using WorkSpeed.Interfaces;
using WorkSpeed.ProductivityCalculator;

namespace WorkSpeed
{
    public class Warehouse : IWarehouse
    {
        private readonly IWorkSpeedData _context;
        private readonly IDataImporter _dataImporter;

        private readonly ITypeRepository _typeRepository;
        private readonly ProductivityObservableCollection _productivities;

        public Warehouse( IWorkSpeedData context, IDataImporter dataImporter )
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

            repo.RegisterType< ShipmentImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            repo.RegisterType< GatheringImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repo.RegisterType< InventoryImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repo.RegisterType< ReceptionImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            repo.RegisterType< ProductivityImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
        }

        /// <summary>
        /// Entities that don't contained in DB.
        /// </summary>
        public IWarehouseEntities NewData { get; }

        public async Task<bool> ImportAsync (string fileName)
        {
            return await Task<bool>.Factory.StartNew (() => Import (fileName), TaskCreationOptions.LongRunning);
        }

        public async Task<bool> ImportAsync< TImportModel > (string fileName) where TImportModel : ImportModel
        {
            return await Task<bool>.Factory.StartNew (() => Import (fileName, typeof( TImportModel )), TaskCreationOptions.LongRunning);
        }

        public Task<bool> HasProductsAsync () => _context.HasProductsAsync();

        private bool Import ( string fileName,  Type type = null )
        {
            var sheetTable = _dataImporter.ImportData (fileName, 0);
            var mappedType = _typeRepository.GetTypeWithMap ( sheetTable );

            if ( type != null && !mappedType.type.IsAssignableFrom( type ) ) {
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

                ImportWithProductAction(
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

        private void ImportEmployees ( IEnumerable< Employee > employees )
        {

        }

        private void ImportProducts ( IEnumerable< Product > products )
        {

        }

        // ReSharper disable PossibleMultipleEnumeration
        private void ImportProductivity ( IEnumerable< EmployeeAction > employeeActions )
        {
            ImportWithProductAction( employeeActions.Where( a => a is GatheringAction ).Cast< GatheringAction >() );
            ImportWithProductAction( employeeActions.Where( a => a is ReceptionAction ).Cast< ReceptionAction >() );
            ImportWithProductAction( employeeActions.Where( a => a is InventoryAction ).Cast< InventoryAction >() );
            ImportShipmentActions( employeeActions.Where( a => a is ShipmentAction ).Cast< ShipmentAction >() );
        }

        private void ImportWithProductAction ( IEnumerable< WithProductAction > withProductActions )
        {

        }

        private void ImportShipmentActions ( IEnumerable< ShipmentAction > shipmentActions )
        {

        }

    }
}
