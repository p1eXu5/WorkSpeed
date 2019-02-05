using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.ProductivityIndicatorsModels
{
    public interface IQuantityIndicators
    {
        void AddQuantity ( EmployeeActionBase employeeAction );
    }
}