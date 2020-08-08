using System.Collections.Generic;


namespace CoronaStore.Models
{
    public class ProductsPageModel
    {

        public IEnumerable<Product> Products
        {
            get;
            set;
        }

        public IEnumerable<CategoryResult> Categories
        {
            get;
            set;
        }
        
        public IEnumerable<ProductResult> Recommended { get; set; }

        public ProductResult TopSale { get; set; }

        public ProductsPageModel()
        {
        }
    }
}