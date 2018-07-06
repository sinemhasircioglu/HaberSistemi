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
    public class KategoriRepository : IKategoriRepository
    {
        private readonly HaberSepetiEntities _context = new HaberSepetiEntities();

        public int Count()
        {
            return _context.Kategoris.Count();
        }

        public void Delete(int id)
        {
            var Kategori = GetById(id);
            if (Kategori != null)
                _context.Kategoris.Remove(Kategori);
        }

        public Kategori Get(Expression<Func<Kategori, bool>> expression)
        {
            return _context.Kategoris.FirstOrDefault(expression);
        }

        public IEnumerable<Kategori> GetAll()
        {
            return _context.Kategoris.Select(x => x);
        }

        public Kategori GetById(int id)
        {
            return _context.Kategoris.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Kategori> GetMany(Expression<Func<Kategori, bool>> expression)
        {
            return _context.Kategoris.Where(expression);
        }

        public void Insert(Kategori obj)
        {
            _context.Kategoris.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Kategori obj)
        {
            _context.Kategoris.AddOrUpdate();
        }
    }
}
