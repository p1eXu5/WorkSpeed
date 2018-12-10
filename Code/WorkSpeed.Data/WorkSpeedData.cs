using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkSpeed.Data.DataContexts;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data
{
    public class WorkSpeedData : IWorkSpeedData, IDisposable
    {
        private bool _disposed;
        private readonly WorkSpeedDbContext _dbContext;

        public WorkSpeedData()
        {
            _dbContext = new WorkSpeedDbContext();
        }

        public IEnumerable<Employee> GetEmployees()
        {
            return _dbContext.Employees.OrderBy (e => e.Name);
        }

        public Employee GetEmployee (string id)
        {
            return _dbContext.Employees.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Document1C> GetDocuments()
        {
            return _dbContext.Documents.OrderBy (d => d.Date);
        }

        #region IDisposable

        private void Dispose (bool disposing)
        {
            if (!disposing && _disposed) {
                return;
            }

            _dbContext?.Dispose();

            _disposed = true;
        }
        
        public void Dispose()
        {
            Dispose (true);
        }

        #endregion


    }
}
