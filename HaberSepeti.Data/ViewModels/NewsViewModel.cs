using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSepeti.Data.ViewModels
{
    public class NewsViewModel
    {
        public List<News> FirstFour { get; set; }

        public List<News> SecondSix { get; set; }

    }
}
