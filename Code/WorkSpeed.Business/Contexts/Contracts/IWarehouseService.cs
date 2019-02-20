using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkSpeed.Business.Contexts.Contracts 
{
    public interface IWarehouseService
    {
        Task ImportAsync ( string fileName );
    }
}