﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InvestingEntities : DbContext
    {
        public InvestingEntities()
            : base("name=InvestingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Exchange> Exchanges { get; set; }
        public virtual DbSet<Industry> Industries { get; set; }
        public virtual DbSet<Sector> Sectors { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
    }
}
