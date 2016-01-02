using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UekHD
{
    class CeneoWebCrawler : IWebCrawler
    {
        public CeneoWebCrawler(string downladString)
        {
            m_downloadString = downladString;
        }
        public Product getCommentList(IStatisctics statistic)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = Encoding.UTF8;
            CommentList comments = new CommentList();
            ILinkProvider provider = new LinkProvider(m_downloadString);
            using (var db = new DatabaseContext())
            {
                Product product = new Product(); // getProductFromDb(client ,db);
                string link = "";
                while ((link = provider.getLink()) != "")
                {
                    statistic.addDowlodedPage(link);
                    string pageContent = client.DownloadString(link);
                    comments.AddRange(ceneoParser.getCommentsContentFromPage(pageContent, product));
                }
                //addProductToDatabaseWithContext(product, db);
                return product;
            }
            
          //  addProductToDatabase(product);
        }
        private Product getProductFromDb(System.Net.WebClient client, DatabaseContext db)
        {
            string pageContent = client.DownloadString(m_downloadString);
            string productName = ceneoParser.getModelName(pageContent);
            string brandName = ceneoParser.getBrandName(pageContent);
             try
            {

                IQueryable<Product> tmp = from p in db.Product where p.Brand.Equals(brandName) && p.Model.Equals(productName) select p;// select db.Product;//and p.Model.E;
                foreach (Product p in tmp)
                {
                    return p;
                }
//                Product product = new Product();
//                db.Product.Add(product);
//                db.SaveChanges();
                return null;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }



            ;

        }

        public void addProductToDatabaseWithContext(Product product, DatabaseContext db)
        {
            try
            {
       //         using (var db = new DatabaseContext())
          //      {
                    //db.Product.Add(product);
                    db.SaveChanges();
       //         }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }


        }


        public void addProductToDatabase(Product product)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    //db.Product.Add(product);
                    IQueryable<Product> productsInDb = from p in db.Product where 
                                              p.Brand.Equals(product.Brand) && 
                                              p.Model.Equals(product.Model) &&
                                              p.Type.Equals(product.Type) select p;// select db.Product;//and p.Model.E;

                    //       var productInDb = db.Product.Find(product.ProductId);
                    if (productsInDb != null)
                    {
                        //productInDb.Comments.Clear();
                        //db.SaveChanges();
                        //db.Entry(productInDb).CurrentValues.SetValues(product);
                        foreach (Product productInDb in productsInDb)
                        {
                            foreach (CommentDb dowloadedProd in product.Comments)
                            {
                                bool contains = productInDb.Comments.Any(x => {
                                    bool returnValue = true;
                                    if (dowloadedProd.Advantages != null && x.Advantages != null)
                                        returnValue &=  dowloadedProd.Advantages.Equals(x.Advantages);
                                    if (dowloadedProd.Disadvantages != null && x.Disadvantages != null)
                                        returnValue &= dowloadedProd.Disadvantages.Equals(x.Disadvantages);
                                    if (dowloadedProd.Comment != null && x.Comment != null)
                                        returnValue &= dowloadedProd.Comment.Equals(x.Comment);
                                    if (dowloadedProd.Date != null && x.Date != null)
                                        returnValue &= dowloadedProd.Date.Equals(x.Date);
                                    returnValue &= dowloadedProd.Recommend.Equals(x.Recommend);
                                    returnValue &= dowloadedProd.Stars.Equals(x.Stars);
                                    returnValue &= dowloadedProd.Usability.Equals(x.Usability);
                                    returnValue &= dowloadedProd.UsabilityVotes.Equals(x.UsabilityVotes);
                                    returnValue &= dowloadedProd.Author.Equals(x.Author);
                                    return returnValue;
                                });
                                if (/*productInDb.Comments.Contains(dowloadedProd)*/contains) { }
                                else
                                {
                                    productInDb.Comments.Add(dowloadedProd);
                                }
                            }
                        }
                        //   productInDb.Comments = product.Comments ;
                        if (productsInDb.Count()==0)
                        {
                            db.Product.Add(product);
                        }
                        
                    }
                    else
                    {
                        db.Product.Add(product);
                    }
                    db.SaveChanges();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }


        }
        private ICommentParser ceneoParser = new CeneoCommentParser();
        private string m_downloadString;

    }
}
