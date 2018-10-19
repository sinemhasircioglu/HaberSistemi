using HaberSepeti.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using HaberSepeti.Data.Entities;
using HaberSepeti.Data;

namespace HaberSepeti.Core.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly HaberSepetiDbContext _context = new HaberSepetiDbContext();

        public int Count()
        {
            return _context.Categories.Count();
        }

        public void Delete(int id)
        {
            Category category = GetById(id);
            if (category != null)
                _context.Categories.Remove(category);
        }

        public Category Get(Expression<Func<Category, bool>> expression)
        {
            return _context.Categories.FirstOrDefault(expression);
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.Select(x => x);
        }

        public Category GetById(int id)
        {
            return _context.Categories.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Category> GetMany(Expression<Func<Category, bool>> expression)
        {
            return _context.Categories.Where(expression);
        }

        public void Insert(Category obj)
        {
            _context.Categories.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Category obj)
        {
            _context.Categories.AddOrUpdate();
        }
    }
}
