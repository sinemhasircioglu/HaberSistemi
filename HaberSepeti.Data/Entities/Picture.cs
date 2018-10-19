using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaberSepeti.Data.Entities
{
    public class Picture
    {
        [Key]
        public int Id { get; set; }
        public string PictureUrl { get; set; }
        public int NewsId { get; set; }

        public virtual News News { get; set; }
    }
}
