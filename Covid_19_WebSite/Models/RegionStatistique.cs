//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Covid_19_WebSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class RegionStatistique
    {
        public string UID { get; set; }
        [Display(Name = "Cas Mort")]
        [Required]
        public Nullable<int> Cas_Mort { get; set; }
        [Display(Name = "Cas R�tablis")]
        [Required]
        public Nullable<int> Cas_Retablis { get; set; }
        [Display(Name = "Cas Confirmer")]
        [Required]
        public Nullable<int> Cas_Confirmer { get; set; }
        [Display(Name = "Date")]
        [Required]
        public Nullable<System.DateTime> Date_Ins { get; set; }
        public string UID_R { get; set; }
    
        public virtual Region Region { get; set; }
    }
}