using EventStore.ClientAPI;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesEventStore.SaleAggregate;

namespace SalesEventStore
{
    public class MyEventStore
    {
        private string _streamName;
        public IEventStoreConnection ConnectToEventStore()
        {
            var conn = EventStoreConnection.Create(new Uri("tcp://admin:changeit@127.0.0.1:1113"));
            conn.ConnectAsync().Wait();
            return conn;
        }
        public void SaveEvents(Sale sale, DateTime date)
        {
            _streamName = "MySalesStream";
            var eventType = "sold-product";
            var data = "{\"id\": \"" + sale.saleId + "\"," +
                    "\"quantity\":" + sale.Quantity + "," +
                    "\"product\": {" +
                        "\"id\": \"" + sale.Product.productId + "\"," +
                        "\"name\": \"" + sale.Product.Name + "\"," +
                        "\"price\":" + sale.Product.Price + "}," +
                    "\"price\":" + sale.Price +
                "}";
            var metadata = "{" + date + "}";
            var eventPayload = new EventData(Guid.NewGuid(), eventType, true, Encoding.UTF8.GetBytes(data), Encoding.UTF8.GetBytes(metadata));
            ConnectToEventStore().AppendToStreamAsync(_streamName, ExpectedVersion.Any, eventPayload).Wait();
        }
        public IEnumerable<JObject> ReadAllEvents()
        {
            //var readEvents = ConnectToEventStore().ReadStreamEventsForwardAsync("MySalesStream", StreamPosition.Start, 200, true);
            //List<JObject> sales = new List<JObject>();
            //foreach (var evt in readEvents.Result.Events)
            //{
            //    JObject sale = JObject.Parse(Encoding.UTF8.GetString(evt.Event.Data));
            //    sales.Add(sale);
            //}
            return new List<JObject>(); //sales;
        }
        public double GetTotalAmountOfSales()
        {
            List<JObject> sales = ReadAllEvents().ToList();
            double totalAmount = 0;
            foreach (var sale in sales)
            {
                totalAmount += (double)sale["price"];
            }
            return totalAmount;
        }
    }
}
