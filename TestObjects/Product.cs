﻿using System.Collections.Generic;

namespace TestObjects
{
    public class Product
    {
        public string Manufacturer { get; }
        public string Name { get; }
        public double Price { get; }
        public List<ProductCategory> Categories { get; }
    }
}
