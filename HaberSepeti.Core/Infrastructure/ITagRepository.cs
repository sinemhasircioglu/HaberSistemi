using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSepeti.Core.Infrastructure
{
    public interface ITagRepository : IRepository<Tag>
    {
        IQueryable<Tag> Tags(string[] tags);

        void AddTag(int NewsId, string Tag);

        void AddNewsTag(int NewsId, string[] tags);
    }
}
