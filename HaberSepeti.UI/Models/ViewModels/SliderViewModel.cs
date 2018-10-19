using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSepeti.UI.Models.ViewModels
{
    public class SliderViewModel
    {
        public IEnumerable<Slider> Slider { get; set; }

        public IEnumerable<News> Haber { get; set; }
    }
}