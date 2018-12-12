using WorkSpeed.Data.Models;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public interface IQuantityIndicators
    {
        void AddQuantity ( EmployeeAction employeeAction );
    }
}