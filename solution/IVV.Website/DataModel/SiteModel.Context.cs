﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace IVV.Website.DataModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SiteEntity : DbContext
    {
        public SiteEntity()
            : base("name=SiteEntity")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<SiteInfo> SiteInfo { get; set; }
        public DbSet<SysUser> SysUser { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<FileInfo> FileInfo { get; set; }
        public DbSet<Video> Video { get; set; }
    }
}
