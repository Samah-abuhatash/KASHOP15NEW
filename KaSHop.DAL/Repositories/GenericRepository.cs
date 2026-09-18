using KaSHop.DAL.Data;
using System.Collections.Generic;
using System.Linq;

namespace KaSHop.DAL.Repositories
{
    public class GenericRepository<T> where T : class
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public void Create(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            context.Set<T>().Update(entity);
            context.SaveChanges();
        }

        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public List<T> GetAll()
        {
            return context.Set<T>().ToList();
        }

        public T? GetById(int id)
        {
            return context.Set<T>().Find(id);
        }
    }
}