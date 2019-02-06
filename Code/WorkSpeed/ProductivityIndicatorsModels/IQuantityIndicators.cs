using WorkSpeed.Data.Models;
using WorkSpeed.Data.Models.Actions;

namespace WorkSpeed.Business.ProductivityIndicatorsModels
{
    public interface IQuantityIndicators
    {
        void AddQuantity ( EmployeeActionBase employeeAction );
    }
}