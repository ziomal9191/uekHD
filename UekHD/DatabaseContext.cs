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
        [System.ComponentModel.DataAnnotations.MaxLength]
        [System.ComponentModel.DataAnnotations.Schema.Column(TypeName = "ntext")]
        public string Comment { set; get; }
        public double Stars { set; get; }
        public string Advantages { set; get; }
        public string Disadvantages { set; get; }
        public string Author { set; get; }
        public DateTime Date { set; get; }
        public bool Recommend { set; get; }
        public int Usability { set; get; }
        public int UsabilityVotes { set; get; }
    }
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Product { set; get; }
        public DbSet<CommentDb> Comments { set; get; }
    }
}
