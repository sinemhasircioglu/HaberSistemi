using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSepeti.UI.Models.ViewModels
{
    public class SidebarViewModel
    {
        public IEnumerable<Haber> SonHaberler { get; set; }

        public IEnumerable<Yorum> SonYorumlar { get; set; }

        public IEnumerable<Haber> PopulerHaberler { get; set; }
    }
}