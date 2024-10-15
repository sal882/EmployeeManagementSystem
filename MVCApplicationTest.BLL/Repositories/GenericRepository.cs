using Microsoft.EntityFrameworkCore;
using MVCApplicationTest.BLL.Interfaces;
using MVCApplicationTest.BLL.Specifications;
using MVCApplicationTest.DAL.Contexts;
using MVCApplicationTest.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCApplicationTest.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MVCApplicationDbContext _dbContext;
        public GenericRepository(MVCApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(T item)
            => await _dbContext.Set<T>().AddAsync(item);

        public void Delete(T item)
            => _dbContext.Set<T>().Remove(item);

        public async Task<T> Get(int id)
            => await _dbContext.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetAll()
        {
            //if (typeof(T) == typeof(Employee))
            //{
            //    return (IEnumerable<T>)await _dbContext.Employees.Include(e => e.Department).ToListAsync();
            //}
            

                return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec)
            => await ApplayQuery(spec).ToListAsync();
        

        public async Task<T> GetWithSpecAsync(ISpecification<T> spec)
            => await ApplayQuery(spec).FirstOrDefaultAsync();

        private IQueryable<T> ApplayQuery(ISpecification<T> spec)
            => SpecificationEvaluation<T>.GetQuery(_dbContext.Set<T>(),spec);

        public void Update(T item)
            =>  _dbContext.Set<T>().Update(item);
    }
}
