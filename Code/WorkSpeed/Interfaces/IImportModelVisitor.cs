using WorkSpeed.Data.Models;
using WorkSpeed.FileModels;

namespace WorkSpeed.Interfaces
{
    public interface IImportModelVisitor
    {
        object GetDbModel ( ImportModel importModel );
        Product GetDbModel ( ProductImportModel productImportModel );
        Employee GetDbModel ( EmployeeImportModel employeeImportModel );
        GatheringAction GetDbModel ( GatheringImportModel gatheringImportModel );
        ReceptionAction GetDbModel ( ReceptionImportModel receptionImportModel );
        InventoryAction GetDbModel ( InventoryImportModel inventoryImportModel );
        ShipmentAction GetDbModel ( ShipmentImportModel shipmentImportModel );
        EmployeeAction GetDbModel ( ProductivityImportModel productivityImportModel );

    }
}