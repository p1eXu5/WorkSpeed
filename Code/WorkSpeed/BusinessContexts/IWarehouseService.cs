using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkSpeed.Data.BusinessContexts 
{
    public interface IWarehouseService
    {
        Task ImportAsync ( string fileName );
    }
}