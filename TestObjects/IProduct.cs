using System.Collections.Generic;

namespace TestObjects
{
    public interface IProduct
    {
        string Name { get; }
        double Price { get; }
        List<ProductCategory> Categories { get; }
    }
}