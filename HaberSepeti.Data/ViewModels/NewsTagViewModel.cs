using HaberSepeti.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSepeti.Data.ViewModels
{
    public class NewsTagViewModel
    {
        public News News { get; set; }

        public string[] AddedTags { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public string TagName { get; set; }
    }
}
