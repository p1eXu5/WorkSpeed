using Agbm.NpoiExcel;
using Agbm.NpoiExcel.Attributes;
using WorkSpeed.Business.FileModels;

namespace WorkSpeed.Business.Factories
{
    public static class TypeRepositoryExtensions
    {
        public static void Fill ( this ITypeRepository repo )
        {
            repo.RegisterType< ProductImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repo.RegisterType< EmployeeImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repo.RegisterType< EmployeeShortImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            repo.RegisterType< ShipmentImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            repo.RegisterType< GatheringImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repo.RegisterType< InventoryImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
            repo.RegisterType< ReceptionImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );

            repo.RegisterType< ProductivityImportModel >( typeof( HeaderAttribute ), typeof( HiddenAttribute ) );
        }
    }
}
