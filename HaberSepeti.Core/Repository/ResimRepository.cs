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
    public class ResimRepository : IResimRepository
    {
        private readonly HaberSepetiEntities _context = new HaberSepetiEntities();

        public int Count()
        {
            return _context.Resims.Count();
        }

        public void Delete(int id)
        {
            var Resim = GetById(id);
            if (Resim != null)
                _context.Resims.Remove(Resim);
        }

        public Resim Get(Expression<Func<Resim, bool>> expression)
        {
            return _context.Resims.FirstOrDefault(expression); //Tek resim
        }

        public IEnumerable<Resim> GetAll()
        {
            return _context.Resims.Select(x => x); //Tüm resimler dönecek
        }

        public Resim GetById(int id)
        {
            return _context.Resims.FirstOrDefault(x => x.Id == id); //Tek resim
        }

        public IQueryable<Resim> GetMany(Expression<Func<Resim, bool>> expression)
        {
            return _context.Resims.Where(expression); //Birden fazla dönecek
        }

        public void Insert(Resim obj)
        {
            _context.Resims.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Resim obj)
        {
            //_context.Entry(obj).State= System.Data.Entity.EntityState.Modified;
            _context.Resims.AddOrUpdate(obj);
        }
    }
}
