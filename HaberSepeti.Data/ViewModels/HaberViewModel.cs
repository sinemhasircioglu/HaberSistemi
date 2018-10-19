using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSepeti.Data.ViewModels
{
    public class HaberViewModel
    {
        public List<News> IlkDortlu { get; set; }

        public List<News> IkinciAltili { get; set; }

    }
}
