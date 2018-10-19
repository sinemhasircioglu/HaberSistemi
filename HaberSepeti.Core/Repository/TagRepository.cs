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
    public class TagRepository : ITagRepository
    {
        private readonly HaberSepetiDbContext _context = new HaberSepetiDbContext();

        public int Count()
        {
            return _context.Tags.Count();
        }

        public void Delete(int id)
        {
            Tag etiket = GetById(id);
            if (etiket != null)
                _context.Tags.Remove(etiket);
        }

        public IQueryable<Tag> Etiketler(string[] etiketler)
        {
            return _context.Tags.Where(x => etiketler.Contains(x.Name));
        }

        public Tag Get(Expression<Func<Tag, bool>> expression)
        {
            return _context.Tags.FirstOrDefault(expression);
        }

        public IEnumerable<Tag> GetAll()
        {
            return _context.Tags.Select(x => x);
        }

        public Tag GetById(int id)
        {
            return _context.Tags.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Tag> GetMany(Expression<Func<Tag, bool>> expression)
        {
            return _context.Tags.Where(expression);
        }

        public void EtiketEkle(int HaberId, string Etiket)
        {
            if (Etiket != null && Etiket != "")
            {
                string[] Etikets = Etiket.Split(',');
                foreach (var tag in Etikets)
                {
                    Tag etiket = this.Get(x => x.Name.ToLower() == tag.ToLower().Trim());
                    if (etiket == null)
                    {
                        etiket = new Tag();
                        etiket.Name = tag;
                        this.Insert(etiket);
                        this.Save();
                    }                    
                }
                this.HaberEtiketEkle(HaberId, Etikets);
            }
        }

        public void HaberEtiketEkle(int HaberId, string[] etiketler)
        {
            var haber = _context.News.FirstOrDefault(x => x.Id == HaberId);
            var gelenEtiket = this.Etiketler(etiketler);
            haber.Tags.Clear();
            gelenEtiket.ToList().ForEach(etiket => haber.Tags.Add(etiket));
            _context.SaveChanges();
        }

        public void Insert(Tag obj)
        {
            _context.Tags.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Tag obj)
        {
            _context.Tags.AddOrUpdate();
        }
    }
}
