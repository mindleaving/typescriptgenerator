using System.Collections.Generic;

namespace TestObjects
{
    public class Product : IProduct
    {
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public ProductType Type { get; set; }
        public List<ProductCategory> Categories { get; set; }
    }
}
