using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoAnWebBanGiay.Models
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base("MyCS") { }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductSize> productSizes { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserProF> UserProFs { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

    }
}