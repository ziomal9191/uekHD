using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UekHD
{

    class HttpCommentGeter
    {
        /// <summary>
        /// Pobieranie kontentów strony komentarzy dla danego produkru z serwisów ceneo.pl i skapice.pl
        /// </summary>
        public HttpCommentGeter(string productId, IStatisctics statistic)
        {
            product = new Product();
            string pageName = "http://www.ceneo.pl/" + productId + "#tab=reviews";
            fillProductPropertis(product, pageName);
            m_webCrawlerCeneo = new CeneoWebCrawler(pageName);
            m_webCrawlerCeneo.getPagesContent( statistic, product);
            ILinkToProductFinder productFinder = new SkapiecLinkToProductFinder();
            string foundProduct = productFinder.getLinkToProduct(product);
            if (foundProduct != null)
            {
                m_webCrawlerSkapiec = new SkapiecWebCrawler("http://www.skapiec.pl" + productFinder.getLinkToProduct(product) + "#opinie");
                m_webCrawlerSkapiec.getPagesContent(statistic, product);
            }

        }
        /// <summary>
        /// Tłumaczenie nazwy id na nazwę produktu
        /// </summary>
        /// <param name="product"></param>
        /// <param name="pageName"></param>
        private void fillProductPropertis(Product product, string pageName )
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = Encoding.UTF8;
            string pageContent = client.DownloadString(pageName);
            htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(pageContent);
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                throw new System.ExecutionEngineException();
                // Handle any parse errors as required
            }
            else
            {
                if (htmlDoc.DocumentNode != null)
                {
                    fillBrandAndModel(product);
                    fillType(product);
                }
            }
        }

        /// <summary>
        /// Wypełnienie poszczególnych struktu danymi z pobranych stron przy pomocy parsowania html
        /// </summary>
        /// <param name="statistic"></param>
        public void translateProduct(IStatisctics statistic)
        {
            m_webCrawlerCeneo.fillProduct(product);
            m_webCrawlerSkapiec.fillProduct(product);
        }


        /// <summary>
        /// Wypełnianie bazy danych danymi uzyskanymi z procesu Transform
        /// </summary>
        /// <param name="statistic"></param>
        public void loadProductToDataBase(IStatisctics statistic)
        {
            addProductToDatabase(product, statistic);
        }

        /// <summary>
        /// Funcja dodająca proddukt do bd, wraz ze sprawdzaniem czy dany komentarz dla danego produktu istnieje
        /// </summary>
        /// <param name="product"></param>
        /// <param name="statistic"></param>
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
                            DateTime time = DateTime.Now;

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
                                    dowloadedProd.LoadDate = time;
                                    statistic.addAddedComment(dowloadedProd.Comment);

                                    productInDb.Comments.Add(dowloadedProd);
                                }
                            }
                        }
                        if (productsInDb.Count() == 0)
                        {
                            DateTime time = DateTime.Now;
                            foreach (CommentDb com in product.Comments)
                            {
                                com.LoadDate = time;
                                statistic.addAddedComment(com.Comment);
                            }

                            db.Product.Add(product);
                        }

                    }
                    else
                    {
                        DateTime time = DateTime.Now;
                        foreach (CommentDb com in product.Comments)
                        {
                                com.LoadDate = time;
                                statistic.addAddedComment(com.Comment);
                        }
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
        /// <summary>
        /// Pobiera model i producenta z wpisanego id produktu
        /// </summary>
        /// <param name="product"></param>
        private void fillBrandAndModel(Product product)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//nav[@class=\"breadcrumbs\"]//dl//strong");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode nodeType in bodyNodes)
                {
                    string[] brand = nodeType.InnerHtml.Split(' ');
                    product.Brand = brand[0];
                    //get model
                    string model = "";
                    for (int i = 1; i < brand.Length; i++)
                    {
                        model += brand[i] + " ";
                    }
                    product.Model = model;
                }
            }


        }
        /// <summary>
        /// Pobiera typ z wpisanego id produktu
        /// </summary>
        /// <param name="product"></param>
        private void fillType(Product product)
        {
            HtmlAgilityPack.HtmlNodeCollection bodyNodes = htmlDoc.DocumentNode.SelectNodes("//nav[@class=\"breadcrumbs\"]//dd//span[last()]//span");
            if (bodyNodes != null)
            {
                foreach (HtmlAgilityPack.HtmlNode nodeType in bodyNodes)
                {
                    product.Type = nodeType.InnerText;
                }
            }

        }
        HtmlAgilityPack.HtmlDocument htmlDoc;
        IWebCrawler m_webCrawlerCeneo;
        IWebCrawler m_webCrawlerSkapiec;
        private Product product;
    }
}
