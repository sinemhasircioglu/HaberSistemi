using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations; //AddOrUpdate için gerekli

namespace HaberSepeti.Core.Repository
{
    public class HaberRepository : IHaberRepository
    {

        private readonly HaberSepetiEntities _context = new HaberSepetiEntities();

        public int Count()
        {
            return _context.Habers.Count();
        }

        public void Delete(int id)
        {
            var Haber = GetById(id);
            if(Haber != null)
                _context.Habers.Remove(Haber);
        }

        public Haber Get(Expression<Func<Haber, bool>> expression)
        {
            return _context.Habers.FirstOrDefault(expression); //Tek haber
        }

        public IEnumerable<Haber> GetAll()
        {
            return _context.Habers.Select(x => x); //Tüm haberler dönecek
        }

        public Haber GetById(int id)
        {
            return _context.Habers.FirstOrDefault(x => x.Id == id); //Tek haber
        }

        public IQueryable<Haber> GetMany(Expression<Func<Haber, bool>> expression)
        {
            return _context.Habers.Where(expression); //Birden fazla dönecek
        }

        public void Insert(Haber obj)
        {
            _context.Habers.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Haber obj)
        {
            //_context.Entry(obj).State= System.Data.Entity.EntityState.Modified;
            _context.Habers.AddOrUpdate(obj);
        }
    }
}
