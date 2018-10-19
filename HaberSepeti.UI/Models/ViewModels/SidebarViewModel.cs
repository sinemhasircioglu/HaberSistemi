using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSepeti.UI.Models.ViewModels
{
    public class SidebarViewModel
    {
        public IEnumerable<News> SonHaberler { get; set; }

        public IEnumerable<Comment> SonYorumlar { get; set; }

        public IEnumerable<News> PopulerHaberler { get; set; }
    }
}