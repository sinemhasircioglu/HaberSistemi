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
    
    public partial class Haber
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Haber()
        {
            this.Resims = new HashSet<Resim>();
        }
    
        public int Id { get; set; }
        public string Baslik { get; set; }
        public string KisaAciklama { get; set; }
        public string Aciklama { get; set; }
        public bool AktifMi { get; set; }
        public System.DateTime EklenmeTarihi { get; set; }
        public int Okunma { get; set; }
        public string Resim { get; set; }
        public int KullaniciId { get; set; }
    
        public virtual Kullanici Kullanici { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Resim> Resims { get; set; }
    }
}