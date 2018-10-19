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
    public class SliderRepository : ISliderRepository
    {
        private readonly HaberSepetiDbContext _context = new HaberSepetiDbContext();

        public int Count()
        {
            return _context.Sliders.Count();
        }

        public void Delete(int id)
        {
            Slider slider = GetById(id);
            if(slider != null)
                _context.Sliders.Remove(slider);
        }

        public Slider Get(Expression<Func<Slider, bool>> expression)
        {
            return _context.Sliders.FirstOrDefault(expression);
        }

        public IEnumerable<Slider> GetAll()
        {
            return _context.Sliders.Select(x => x);
        }

        public Slider GetById(int id)
        {
            return _context.Sliders.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Slider> GetMany(Expression<Func<Slider, bool>> expression)
        {
            return _context.Sliders.Where(expression);
        }

        public void Insert(Slider obj)
        {
            _context.Sliders.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Slider obj)
        {
            _context.Sliders.AddOrUpdate();
        }
    }
}
