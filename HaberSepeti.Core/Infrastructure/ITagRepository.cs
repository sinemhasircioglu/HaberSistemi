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
        IQueryable<Tag> Etiketler(string[] etiketler);

        void EtiketEkle(int HaberId, string Etiket);

        void HaberEtiketEkle(int HaberId, string[] etiketler);
    }
}
