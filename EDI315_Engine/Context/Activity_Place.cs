//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EDI315_Engine.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class Activity_Place
    {
        public int activity_place_idnum { get; set; }
        public int container_idnum { get; set; }
        public string activity_place_location { get; set; }
        public string activity_place_portname { get; set; }
        public string activity_place_country { get; set; }
        public Nullable<System.DateTime> activity_place_datetime { get; set; }
        public System.DateTime created_date { get; set; }
    
        public virtual Container Container { get; set; }
    }
}
