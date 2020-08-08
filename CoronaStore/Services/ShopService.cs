using System;
using System.Collections.Generic;
using System.Linq;
using Accord.MachineLearning.Rules;
using CoronaStore.Models;

namespace CoronaStore.Services
{
    public class ShopService
    {
        public IList<Product> GetAllProductsFromInventory()
        {
            using (var context = new CoronaPageContext())
            {
                return context.Products.ToList();
            }
        }

        public IList<CategoryResult> GetAllCategories()
        {
            using (var context = new CoronaPageContext())
            {
                var categories = context.Categories.ToList();

                var productsCount = context.Products.GroupBy(p => p.CategoryID)
                .Select(group => new
                {
                    categoryId = group.FirstOrDefault().CategoryID,
                    count = group.Count()
                }).ToList();

                return categories.Join(productsCount,
                                       c => c.CategoryID,
                                       pc => pc.categoryId,
                                       (c, pc) => new CategoryResult()
                                       {
                                           CategoryID = c.CategoryID,
                                           Name = c.Name,
                                           Count = pc.count
                                       })
                                 .ToList();
            }
        }

        public IList<SalesCategory> SalesPerCategory()
        {
            using (var context = new CoronaPageContext())
            {

                return context.Sales.Join(context.Products, s => s.ProductID, p => p.ProductID, (sale, product) => new
                {
                    Product = product,
                    Sale = sale
                })
                    .GroupBy(x => x.Product.CategoryID)
                    .Select(x => new SalesCategory()
                    {
                        CategoryName = GetCategoryName(x.Key),
                        SalesSum = (int)x.Sum(y => y.Product.Price)
                    })
                    .ToList();
            }
        }

        private string GetCategoryName(int key)
        {
            using (var context = new CoronaPageContext())
            {
                return context.Categories.Where(c => c.CategoryID == key).FirstOrDefault().Name;
            }

        }

        public IList<Category> GetAllAndNullCategories()
        {
            using (var context = new CoronaPageContext())
            {
                return (context.Categories.ToList());
            }
        }

        public IList<Product> SearchProducts(string term, int? price)
        {

            using (var context = new CoronaPageContext())
            {
                if (price != null)
                {
                    return context.Products
                        .Where(p => (p.Name.Contains(term) || p.Description.Contains(term)) && p.Price <= price).ToList();
                }
                else
                    return context.Products
                        .Where(p => p.Name.Contains(term) || p.Description.Contains(term)).ToList();
            }


        }

        public IList<Product> SearchProducts(string term, int? price, int categoryId)
        {
            using (var context = new CoronaPageContext())
            {
                var products = context.Products.Where(p => (p.Name.ToLower().Contains(term.ToLower()) || p.Description.ToLower().Contains(term.ToLower())));

                if (price != null)
                {
                    products = products.Where(p => p.Price <= price);
                }

                if (categoryId > 0)
                {
                    products = products.Where(p => p.CategoryID == categoryId);
                }

                return products.ToList();
            }
        }

        public bool DeleteLocation(int id)
        {
            using (var context = new CoronaPageContext())
            {
                var targetLocation = context.Locations.Where(c => c.LocationID == id).FirstOrDefault();

                try
                {
                    context.Locations.Remove(targetLocation);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        public bool UpdateInventory(Inventory inventory)
        {
            using (var context = new CoronaPageContext())
            {
                try
                {
                    context.Inventory.Add(inventory);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }

        public Product AddProduct(Product pProduct)
        {
            using (var context = new CoronaPageContext())
            {
                Product newProd;
                try
                {
                    pProduct.SupplierID = 2;
                    context.Products.Add(pProduct);
                    context.SaveChanges();
                    newProd = context.Products.Where(p => p == pProduct).FirstOrDefault();
                    //Inventory newProdInventory = new Inventory()
                    //{
                    //    ProductID = newProd.ProductID,
                    //    Quantity = 100
                    //};
                    //context.Inventory.Add(newProdInventory);
                    //context.SaveChanges();
                }
                catch (Exception e)
                {
                    return null;
                }

                return newProd;
            }
        }

        public object UpdateLocation(Location location)
        {
            using (var context = new CoronaPageContext())
            {
                var targetLocation = context.Locations.Where(c => c.LocationID == location.LocationID).FirstOrDefault();
                
                try
                {
                    targetLocation.Population = location.Population;
                    context.Locations.Update(targetLocation);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }

            }
            return true;
        }

        public bool BuyProduct(int UserId, int id)
        {
            using (var context = new CoronaPageContext())
            {

                var targetInventory = context.Inventory.Where(p => p.ProductID == id).FirstOrDefault();
                if (targetInventory is null)
                {
                    return false;
                }
                targetInventory.Quantity--;

                Sale saleToAdd = new Sale()
                {
                    ProductID = id,
                    UserID = UserId
                };

                try
                {
                    context.Inventory.Update(targetInventory);
                    context.Sales.Add(saleToAdd);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;
        }

        public Product UpdateProduct(Product product)
        {
            using (var context = new CoronaPageContext())
            {
                var targetProduct = context.Products.Where(p => p.ProductID == product.ProductID).FirstOrDefault();

                targetProduct.Name = product.Name;
                targetProduct.Description = product.Description;
                targetProduct.Price = product.Price;
                targetProduct.CategoryID = product.CategoryID;

                if (product.Image != null)
                    targetProduct.Image = product.Image;

                try
                {
                    context.Products.Update(targetProduct);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    return null;
                }

                return targetProduct;
            }
        }

        public bool DeleteProduct(int id)
        {
            using (var context = new CoronaPageContext())
            {
                var targetProduct = context.Products.Where(p => p.ProductID == id).FirstOrDefault();
                context.Products.Remove(targetProduct);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        public ProductResult GetProduct(int id)
        {
            using (var context = new CoronaPageContext())
            {
                return context.Products
                    .Join(context.Categories, p => p.CategoryID, c => c.CategoryID, (product, category) => new ProductResult(product, category))
                    .GroupJoin(context.Inventory, p => p.ProductID, i => i.ProductID, (product, inventory) => new ProductResult(product, inventory))
                    .FirstOrDefault(p => p.ProductID == id);

            }
        }

        public ProductStockResult[] GetProductsStock()
        {
            using (var context = new CoronaPageContext())
            {
                return context.Inventory.Join(context.Products, i => i.ProductID, p => p.ProductID, (inventory, product) => new
                {
                    Product = product,
                    Inventory = inventory
                })
                .GroupBy(x => x.Product.Name)
                .Select(x => new ProductStockResult()
                {
                    ProductName = x.Key,
                    Count = x.Sum(y => y.Inventory.Quantity)
                })
                .ToArray();
            }
        }

        public IList<Product> GetProductsByCategory(int id)
        {
            using (var context = new CoronaPageContext())
            {
                return context.Products.Where(p => p.CategoryID == id).ToList();
            }
        }

        public IList<Sale> GetAllSales()
        {
            using (var ctx = new CoronaPageContext())
            {
                return ctx.Sales.ToList();
            }
        }

        public dynamic GetAllSalesByUser()
        {
            using (var ctx = new CoronaPageContext())
            {
                return ctx.Sales.GroupBy(s => s.UserID,
                    s => s.ProductID,
                    (key, g) => new { UserID = key, Products = g.ToList() }).ToList();
            }
        }

        public IList<ProductResult> RecommendProducts(int userId)
        {
            var salesByUser = this.GetAllSalesByUser();

            List<int[]> tempDataset = new List<int[]>();
            int[] currUserSales = null;

            foreach (var userSales in salesByUser)
            {
                if (userSales.UserID == userId)
                {
                    currUserSales = userSales.Products.ToArray();
                }

                tempDataset.Add(userSales.Products.ToArray());
            }

            if (currUserSales == null || currUserSales.Length == 0)
            {
                return new List<ProductResult>();
            }

            int[][] dataset = tempDataset.ToArray();

            // We will use Apriori to determine the frequent item sets of this database.
            // To do this, we will say that an item set is frequent if it appears in at 
            // least 3 transactions of the database: the value 3 is the support threshold.

            // Create a new a-priori learning algorithm with support 3
            Apriori apriori = new Apriori(threshold: 1, confidence: 0.5);

            //// Use the algorithm to learn a set matcher
            AssociationRuleMatcher<int> classifier = apriori.Learn(dataset);

            // Use the classifier to find orders that are similar to 
            // orders where clients have bought items 1 and 2 together:
            int[][] matches = classifier.Decide(currUserSales);

            List<ProductResult> recommededProducts = new List<ProductResult>();

            if (matches.Length > 0)
            {
                int[] tmpRecommendedProducts = matches[0];
                foreach (var product in tmpRecommendedProducts)
                {
                    recommededProducts.Add(this.GetProduct(product));
                }
            }

            return recommededProducts;
        }
          
        public IList<ProductResult> getUserProducts(int UserId)
        {
            IList<ProductResult> listToReturn = new List<ProductResult>();
            using (var context = new CoronaPageContext())
            {
                var sales = context.Sales.Where(u => u.UserID == UserId);
                foreach (var sale in sales)
                {
                    listToReturn.Add(GetProduct(sale.ProductID));
                }
            }
            return listToReturn;
        }

        public ProductResult getTopSaleProduct()
        {
            var sales = this.GetAllSales();

            var grouped = sales.GroupBy(s => s.ProductID, (key, g) => new { ProductID = key, Count = g.Count() });
            grouped = grouped.OrderByDescending(s => s.Count);

            return this.GetProduct(grouped.First().ProductID);
        }
    }
}
