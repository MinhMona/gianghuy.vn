//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NHST.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_OTP
    {
        public int ID { get; set; }
        public Nullable<int> UID { get; set; }
        public string Value { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Prefix { get; set; }
        public string UserPhone { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
