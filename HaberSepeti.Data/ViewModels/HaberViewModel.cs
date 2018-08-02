using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSepeti.Data.ViewModels
{
    public class HaberViewModel
    {
        public List<Haber> IlkDortlu { get; set; }

        public List<Haber> IkinciAltili { get; set; }

    }
}
