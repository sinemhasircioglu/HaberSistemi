using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSepeti.UI.Models.ViewModels
{
    public class DetailViewModel
    {
        public News News { get; set; }

        public Slider Slider { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public IEnumerable<News> RelatedNews { get; set; }
    }
}