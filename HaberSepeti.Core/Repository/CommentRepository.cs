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
    public class CommentRepository : ICommentRepository
    {
        private readonly HaberSepetiDbContext _context = new HaberSepetiDbContext();

        public int Count()
        {
            return _context.Comments.Count();
        }

        public void Delete(int id)
        {
            Comment yorum = GetById(id);
            if(yorum != null)
            {
                _context.Comments.Remove(yorum);
            }
        }

        public Comment Get(Expression<Func<Comment, bool>> expression)
        {
            return _context.Comments.FirstOrDefault(expression);
        }

        public IEnumerable<Comment> GetAll()
        {
            return _context.Comments.Select(x => x);
        }

        public Comment GetById(int id)
        {
            return _context.Comments.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Comment> GetMany(Expression<Func<Comment, bool>> expression)
        {
            return _context.Comments.Where(expression);
        }

        public void Insert(Comment obj)
        {
            _context.Comments.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Comment obj)
        {
            _context.Comments.AddOrUpdate();
        }
    }
}
