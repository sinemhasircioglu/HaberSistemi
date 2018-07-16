using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSepeti.Core.Infrastructure
{
    public interface IEtiketRepository : IRepository<Etiket>
    {
        IQueryable<Etiket> Etiketler(string[] etiketler);

        void EtiketEkle(int HaberId, string Etiket);

        void HaberEtiketEkle(int HaberId, string[] etiketler);
    }
}
