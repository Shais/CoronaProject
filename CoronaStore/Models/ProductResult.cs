using System.Collections.Generic;
using System.Linq;

namespace CoronaStore.Models
{
    public class ProductResult
    {

        public ProductResult(Product product, Category category)
        {
            this.Category = category;
            this.ProductID = product.ProductID;
            this.SupplierID = product.SupplierID;
            this.Price = product.Price;
            this.Description = product.Description;
            this.SupplierID = product.SupplierID;
            this.Name = product.Name;
            this.Image = product.Image;
        }

        public ProductResult(ProductResult product, IEnumerable<Inventory> inventory)
        {
            this.Category = product.Category;
            this.ProductID = product.ProductID;
            this.Price = product.Price;
            this.Description = product.Description;
            this.Name = product.Name;
            this.Image = product.Image;
            this.SupplierID = product.SupplierID;
            this.IsInStock = inventory.Sum(i => i.Quantity) > 0;
        }

        public int ProductID { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public Category Category { get; set; }

        public int SupplierID { get; set; }

        public string Name { get; set; }

        public bool IsInStock { get; set; }

        public string Image
        {
            get;
            set;
        }
    }
}
