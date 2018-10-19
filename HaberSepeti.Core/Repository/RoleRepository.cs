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
    public class RoleRepository : IRoleRepository
    {
        private readonly HaberSepetiDbContext _context = new HaberSepetiDbContext();

        public int Count()
        {
            return _context.Roles.Count();
        }

        public void Delete(int id)
        {
            Role role = GetById(id);
            if (role != null)
                _context.Roles.Remove(role);
        }

        public Role Get(Expression<Func<Role, bool>> expression)
        {
            return _context.Roles.FirstOrDefault(expression);
        }

        public IEnumerable<Role> GetAll()
        {
            return _context.Roles.Select(x => x);
        }

        public Role GetById(int id)
        {
            return _context.Roles.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<Role> GetMany(Expression<Func<Role, bool>> expression)
        {
            return _context.Roles.Where(expression);
        }

        public void Insert(Role obj)
        {
            _context.Roles.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Role obj)
        {
            _context.Roles.AddOrUpdate();
        }
    }
}
