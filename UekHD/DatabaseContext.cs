using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    public class Product
    {
        public Product() { Comments = new List<CommentDb>(); }
        public int ProductId { set; get; }
        public virtual ICollection<CommentDb> Comments { set; get; }
        public string Type { set; get; }
        public string Brand { set; get; }
        public string Model { set; get; }
        public string AdditionalComment { set; get;  }

    }
    public class CommentDb
    {
        public int CommentDbID { set; get; }
        public string Comment { set; get; }
        public double Stars { set; get; }
        public string Advantages { set; get; }
        public string Disadvantages { set; get; }

    }
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Product { set; get; }
        public DbSet<CommentDb> Comments { set; get; }


    }
}
