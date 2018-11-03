using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HaberSepeti.Admin.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int ActiveComments { get; set; }

        public int PassiveComments { get; set; }

        public int ActiveUsers { get; set; }

        public int PassiveUsers { get; set; }

        public int TotalNews { get; set; }

        public int TotalReads { get; set; }
    }
}