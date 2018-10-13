using HaberSepeti.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSepeti.UI.Models.ViewModels
{
    public class DetayViewModel
    {
        public Haber Haber { get; set; }

        public Slider Slider { get; set; }

        public IEnumerable<Yorum> Yorumlar { get; set; }

        public IEnumerable<Haber> IliskiliHaberler { get; set; }
    }
}