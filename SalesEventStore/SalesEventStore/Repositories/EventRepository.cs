using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SalesEventStore.SaleAggregate;

namespace SalesEventStore.Repositories
{
    public class EventRepository
    {
        static readonly MyEventStore _storage = new MyEventStore();
        public void Save(Sale sale)
        {
            _storage.SaveEvents(sale, DateTime.Now);
        }
        public IEnumerable<JObject> GetSales()
        {
            return _storage.ReadAllEvents();
        }
        public double GetSalesTotalPrice()
        {
            return _storage.GetTotalAmountOfSales();
        }
        public IEnumerable<JToken> GetInventory()
        {
            JArray sales = JArray.FromObject(_storage.ReadAllEvents());
            var products = sales.Select(e => new { name = e["product"]["name"], soldQuantity = e["quantity"] }).GroupBy(prod => prod.name).Select(prod => new { product = prod.Key, totalSoldAmount = prod.Sum(x => (int)x.soldQuantity) });

            return JArray.FromObject(products);
        }
    }
}
