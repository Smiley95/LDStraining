using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesEventStore.SaleAggregate
{
    public class Product
    {
        public readonly Guid productId;
        public string Name;
        public double Price;
        public Product(string name, double price)
        {
            this.productId = Guid.NewGuid();
            this.Price = price;
            this.Name = name;
        }
    }
}
