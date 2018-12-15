using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkSpeed.Data.DbContexts;
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

        public Task<bool> HasProductsAsync ()
        {
            return _dbContext.Products.AnyAsync();
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
