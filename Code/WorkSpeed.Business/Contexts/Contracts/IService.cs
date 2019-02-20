using System;
using WorkSpeed.Data.Context;

namespace WorkSpeed.Business.Contexts.Contracts
{
    public interface IService : IDisposable
    {
        WorkSpeedDbContext DbContext { get; }
    }
}
