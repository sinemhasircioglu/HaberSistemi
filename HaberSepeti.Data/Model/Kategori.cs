//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HaberSepeti.Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Kategori
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kategori()
        {
            this.Habers = new HashSet<Haber>();
        }
    
        public int Id { get; set; }
        public string Ad { get; set; }
        public int ParentId { get; set; }
        public bool AktifMi { get; set; }
        public string URL { get; set; }
        public Nullable<int> KullaniciId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Haber> Habers { get; set; }
        public virtual Kullanici Kullanici { get; set; }
    }
}
