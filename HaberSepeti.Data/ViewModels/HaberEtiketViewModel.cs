using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSepeti.Data.ViewModels
{
    public class HaberEtiketViewModel
    {
        public News News { get; set; }

        public string[] GelenEtiketler { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public string TagName { get; set; }
    }
}
