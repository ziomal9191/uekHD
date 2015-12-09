using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    public class CommentDb
    {
        public int CommentDbID { set; get; }
        public string Comment { set; get; }
    }
    public class DatabaseContext : DbContext
    {
        public DbSet<CommentDb> Comments { set; get; }
    }
}
