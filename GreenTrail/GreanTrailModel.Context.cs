﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GreenTrail
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GreanTrailEntities : DbContext
    {
        private static GreanTrailEntities _context;
        public static GreanTrailEntities GetContext()
        {
            if (_context == null) _context = new GreanTrailEntities();
            return _context;
        }

        public GreanTrailEntities()
            : base("name=GreanTrailEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Contemplation> Contemplation { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Event_Region> Event_Region { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Norm> Norm { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<Organization_Event> Organization_Event { get; set; }
        public virtual DbSet<Pollution> Pollution { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Region_Pollution> Region_Pollution { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Sample> Sample { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Type_organization> Type_organization { get; set; }
        public virtual DbSet<Users> Users { get; set; }
    }
}
