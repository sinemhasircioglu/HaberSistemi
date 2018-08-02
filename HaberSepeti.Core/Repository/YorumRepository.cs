using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;

namespace HaberSepeti.Core.Repository
{
    class YorumRepository : IYorumRepository
    {
        private readonly HaberSepetiEntities _context = new HaberSepetiEntities();

        public int Count()
        {
            return _context.Yorums.Count();
        }

        public void Delete(int id)
        {
            Yorum yorum = GetById(id);
            if(yorum != null)
            {
                _context.Yorums.Remove(yorum);
            }
        }

        public Yorum Get(Expression<Func<Yorum, bool>> expression)
        {
            return _context.Yorums.FirstOrDefault(expression);
        }

        public IEnumerable<Yorum> GetAll()
        {
            return _context.Yorums.Select(x => x);
        }

        public Yorum GetById(int id)
        {
            return _context.Yorums.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Yorum> GetMany(Expression<Func<Yorum, bool>> expression)
        {
            return _context.Yorums.Where(expression);
        }

        public void Insert(Yorum obj)
        {
            _context.Yorums.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Yorum obj)
        {
            _context.Yorums.AddOrUpdate();
        }
    }
}
