using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.DataContexts;

namespace WorkSpeed.Data.BusinessContexts.Contracts
{
    public interface IService : IDisposable
    {
        WorkSpeedDbContext DdContext { get; }
    }
}
