using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSepeti.UI.Models.ViewModels
{
    public class DetayViewModel
    {
        public News Haber { get; set; }

        public Slider Slider { get; set; }

        public IEnumerable<Comment> Yorumlar { get; set; }

        public IEnumerable<News> IliskiliHaberler { get; set; }
    }
}