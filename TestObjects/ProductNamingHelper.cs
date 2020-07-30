using System;

namespace TestObjects
{
    public class ProductNamingHelper
    {
        public string GenerateName()
        {
            return Guid.NewGuid().ToString();
        }
    }
}