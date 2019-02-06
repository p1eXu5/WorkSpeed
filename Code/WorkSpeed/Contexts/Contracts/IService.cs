using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Business.Contexts.Contracts
{
    public interface IService : IDisposable
    {
        WorkSpeedDbContext DbContext { get; }
    }
}
