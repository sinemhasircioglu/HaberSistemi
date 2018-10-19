using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations; //AddOrUpdate için gerekli
using System.Data.Entity.Validation;
using HaberSepeti.Data;

namespace HaberSepeti.Core.Repository
{
    public class NewsRepository : INewsRepository
    {

        private readonly HaberSepetiDbContext _context = new HaberSepetiDbContext();

        public int Count()
        {
            return _context.News.Count();
        }

        public void Delete(int id)
        {
            News news = GetById(id);
            if(news != null)
                _context.News.Remove(news);
        }

        public News Get(Expression<Func<News, bool>> expression)
        {
            return _context.News.FirstOrDefault(expression); //Tek haber
        }

        public IEnumerable<News> GetAll()
        {
            return _context.News.Select(x => x); //Tüm haberler dönecek
        }

        public News GetById(int id)
        {
            return _context.News.FirstOrDefault(x => x.Id == id); //Tek haber
        }

        public IQueryable<News> GetMany(Expression<Func<News, bool>> expression)
        {
            return _context.News.Where(expression); //Birden fazla dönecek
        }

        public void Insert(News obj)
        {
            _context.News.Add(obj);
        }

        public void Save()
        {           
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
        }

        public void Update(News obj)
        {
            //_context.Entry(obj).State= System.Data.Entity.EntityState.Modified;
            _context.News.AddOrUpdate(obj);
        }
    }
}
