using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSepeti.UI.Models.ViewModels
{
    public class SidebarViewModel
    {
        public IEnumerable<News> LatestNews { get; set; }

        public IEnumerable<Comment> LatestComments { get; set; }

        public IEnumerable<News> PopularNews { get; set; }
    }
}