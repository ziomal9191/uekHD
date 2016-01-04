using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    public class Product : ICollection
    {
        public Product() { Comments = new List<CommentDb>(); }
        public int ProductId { set; get; }
        public virtual ICollection<CommentDb> Comments { set; get; }
        public string Type { set; get; }
        public string Brand { set; get; }
        public string Model { set; get; }
        public string AdditionalComment { set; get;  }

        public int Count
        {
            get
            {
               return Comments.Count;
            }
        }
        public CommentDb this[int index]
        {
            get
            {
                return Comments.ToArray()[index];
                
            }
            set { /* Do Nothing */ }
        }
        public object SyncRoot
        {
            get
            {
                return this;

            }
        }

        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public void CopyTo(Array array, int index)
        {
            Comments.ToArray().CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return Comments.GetEnumerator();
        }
        public void Add(CommentDb newEmployee)
        {
            Comments.Add(newEmployee);
        }
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
        public bool? Recommend { set; get; }
        public int Usability { set; get; }
        public int UsabilityVotes { set; get; }
        public string PortalName { set; get; }
    }
    public class DatabaseContext : DbContext
    {
        public DbSet<Product> Product { set; get; }
        public DbSet<CommentDb> Comments { set; get; }
    }
}
