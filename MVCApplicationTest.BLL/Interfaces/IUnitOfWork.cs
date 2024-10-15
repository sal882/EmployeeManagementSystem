using MVCApplicationTest.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCApplicationTest.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Represent All Repository in this DB
        public IEmployeeRepository EmployeeRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; set; }

        Task<int> Complete();
    }
}