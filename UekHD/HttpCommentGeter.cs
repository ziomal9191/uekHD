using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UekHD
{

    class HttpCommentGeter
    {
        public HttpCommentGeter(string productId, IStatisctics statistic)
        {
            product = new Product();
            m_webCrawler = new CeneoWebCrawler("http://www.ceneo.pl/" + productId + "#tab=reviews");
            product = m_webCrawler.getCommentList( statistic, product);
            ILinkToProductFinder productFinder = new SkapiecLinkToProductFinder();
            string foundProduct = productFinder.getLinkToProduct(product);
            if (foundProduct != null)
            {
                m_webCrawler = new SkapiecWebCrawler("http://www.skapiec.pl" + productFinder.getLinkToProduct(product) + "#opinie");
                product = m_webCrawler.getCommentList(statistic, product);
            }

        }
        public void loadProductToDataBase(IStatisctics statistic)
        {
            addProductToDatabase(product, statistic);
        }
        private void addProductToDatabase(Product product, IStatisctics statistic)
        {
            try
            {
                using (var db = new DatabaseContext())
                {
                    IQueryable<Product> productsInDb = from p in db.Product
                                                       where
                         p.Brand.Equals(product.Brand) &&
                         p.Model.Equals(product.Model) &&
                         p.Type.Equals(product.Type)
                                                       select p;// select db.Product;//and p.Model.E;

                    if (productsInDb != null)
                    {
                        foreach (Product productInDb in productsInDb)
                        {
                            foreach (CommentDb dowloadedProd in product.Comments)
                            {
                                bool contains = productInDb.Comments.Any(x => {
                                    bool returnValue = true;
                                    if (dowloadedProd.Advantages != null && x.Advantages != null)
                                        returnValue &= dowloadedProd.Advantages.Equals(x.Advantages);
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
                                if (contains) { }
                                else
                                {
                                    statistic.addAddedComment(dowloadedProd.Comment);
                                    productInDb.Comments.Add(dowloadedProd);
                                }
                            }
                        }
                        if (productsInDb.Count() == 0)
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
 
        IWebCrawler m_webCrawler;
        private Product product;
    }
}
