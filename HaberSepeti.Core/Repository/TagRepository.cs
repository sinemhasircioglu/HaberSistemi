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
            Tag tag = GetById(id);
            if (tag != null)
                _context.Tags.Remove(tag);
        }

        public IQueryable<Tag> Tags(string[] tags)
        {
            return _context.Tags.Where(x => tags.Contains(x.Name));
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

        public void AddTag(int NewsId, string Tag)
        {
            if (Tag != null && Tag != "")
            {
                string[] Tags = Tag.Split(',');
                foreach (var t in Tags)
                {
                    Tag tag = this.Get(x => x.Name.ToLower() == t.ToLower().Trim());
                    if (tag == null)
                    {
                        tag = new Tag();
                        tag.Name = t;
                        this.Insert(tag);
                        this.Save();
                    }                    
                }
                this.AddNewsTag(NewsId, Tags);
            }
        }

        public void AddNewsTag(int NewsId, string[] tags)
        {
            var news = _context.News.FirstOrDefault(x => x.Id == NewsId);
            var pTag = this.Tags(tags);
            news.Tags.Clear();
            pTag.ToList().ForEach(t => news.Tags.Add(t));
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
