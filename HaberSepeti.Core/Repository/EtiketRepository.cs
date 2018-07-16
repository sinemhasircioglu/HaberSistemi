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
    public class EtiketRepository : IEtiketRepository
    {
        private readonly HaberSepetiEntities _context = new HaberSepetiEntities();

        public int Count()
        {
            return _context.Etikets.Count();
        }

        public void Delete(int id)
        {
            Etiket etiket = GetById(id);
            if (etiket != null)
                _context.Etikets.Remove(etiket);
        }

        public IQueryable<Etiket> Etiketler(string[] etiketler)
        {
            return _context.Etikets.Where(x => etiketler.Contains(x.EtiketAdi));
        }

        public Etiket Get(Expression<Func<Etiket, bool>> expression)
        {
            return _context.Etikets.FirstOrDefault(expression);
        }

        public IEnumerable<Etiket> GetAll()
        {
            return _context.Etikets.Select(x => x);
        }

        public Etiket GetById(int id)
        {
            return _context.Etikets.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Etiket> GetMany(Expression<Func<Etiket, bool>> expression)
        {
            return _context.Etikets.Where(expression);
        }

        public void EtiketEkle(int HaberId, string Etiket)
        {
            if (Etiket != null && Etiket != "")
            {
                string[] Etikets = Etiket.Split(',');
                foreach (var tag in Etikets)
                {
                    Etiket etiket = this.Get(x => x.EtiketAdi.ToLower() == tag.ToLower().Trim());
                    if (etiket == null)
                    {
                        etiket = new Etiket();
                        etiket.EtiketAdi = tag;
                        this.Insert(etiket);
                        this.Save();
                    }                    
                }
                this.HaberEtiketEkle(HaberId, Etikets);
            }
        }

        public void HaberEtiketEkle(int HaberId, string[] etiketler)
        {
            var haber = _context.Habers.FirstOrDefault(x => x.Id == HaberId);
            var gelenEtiket = this.Etiketler(etiketler);
            haber.Etikets.Clear();
            gelenEtiket.ToList().ForEach(etiket => haber.Etikets.Add(etiket));
            _context.SaveChanges();
        }

        public void Insert(Etiket obj)
        {
            _context.Etikets.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Etiket obj)
        {
            _context.Etikets.AddOrUpdate();
        }
    }
}
