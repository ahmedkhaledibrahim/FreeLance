//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FreeLance.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FreeLanceSystemEntities : DbContext
    {
        public FreeLanceSystemEntities()
            : base("name=FreeLanceSystemEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Proposal> Proposals { get; set; }
        public virtual DbSet<SavedPost> SavedPosts { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
