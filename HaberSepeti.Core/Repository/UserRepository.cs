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
    public class UserRepository : IUserRepository
    {
        private readonly HaberSepetiDbContext _context = new HaberSepetiDbContext();

        public int Count()
        {
            return _context.Users.Count();
        }

        public void Delete(int id)
        {
            User user = GetById(id);
            if (user != null)
                _context.Users.Remove(user);
        }

        public User Get(Expression<Func<User, bool>> expression)
        {
            return _context.Users.FirstOrDefault(expression);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.Select(x => x);
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<User> GetMany(Expression<Func<User, bool>> expression)
        {
            return _context.Users.Where(expression);
        }

        public void Insert(User obj)
        {
            _context.Users.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(User obj)
        {
            _context.Users.AddOrUpdate();
        }
    }
}
