using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesEventStore.SaleAggregate
{
    public class Sale
    {
        public readonly Guid saleId;
        public int Quantity;
        public Product Product;
        public double Price;
        
        public Sale(int quantity, string productName, double price)
        {
            this.saleId = Guid.NewGuid();
            this.Quantity = quantity;
            this.Price = price;
            this.Product = new Product(productName, price / quantity);

        }
    }
}
