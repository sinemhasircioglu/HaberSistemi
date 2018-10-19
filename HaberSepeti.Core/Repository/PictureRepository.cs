using HaberSepeti.Core.Infrastructure;
using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using HaberSepeti.Data;

namespace HaberSepeti.Core.Repository
{
    public class PictureRepository : IPictureRepository
    {
        private readonly HaberSepetiDbContext _context = new HaberSepetiDbContext();

        public int Count()
        {
            return _context.Pictures.Count();
        }

        public void Delete(int id)
        {
            Picture picture = GetById(id);
            if (picture != null)
                _context.Pictures.Remove(picture);
        }

        public Picture Get(Expression<Func<Picture, bool>> expression)
        {
            return _context.Pictures.FirstOrDefault(expression); //Tek resim
        }

        public IEnumerable<Picture> GetAll()
        {
            return _context.Pictures.Select(x => x); //Tüm resimler dönecek
        }

        public Picture GetById(int id)
        {
            return _context.Pictures.FirstOrDefault(x => x.Id == id); //Tek resim
        }

        public IQueryable<Picture> GetMany(Expression<Func<Picture, bool>> expression)
        {
            return _context.Pictures.Where(expression); //Birden fazla dönecek
        }

        public void Insert(Picture obj)
        {
            _context.Pictures.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Picture obj)
        {
            //_context.Entry(obj).State= System.Data.Entity.EntityState.Modified;
            _context.Pictures.AddOrUpdate(obj);
        }
    }
}
