using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSepeti.Data.ViewModels
{
    public class HaberEtiketViewModel
    {
        public Haber Haber { get; set; }

        public string[] GelenEtiketler { get; set; }

        public IEnumerable<Etiket> Etiketler { get; set; }

        public string EtiketAdi { get; set; }
    }
}
