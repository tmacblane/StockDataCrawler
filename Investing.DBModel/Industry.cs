//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Investing.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Industry
    {
        public Industry()
        {
            this.Stocks = new HashSet<Stock>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
