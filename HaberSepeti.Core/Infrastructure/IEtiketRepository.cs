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
        IQueryable<Etiket> Etiketler(int[] etiketler);

        void HaberEtiketEkle(int HaberId, int[] etiketler);
    }
}
