using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SalesEventStore.Repositories;
using SalesEventStore.SaleAggregate;

namespace SalesEventStore.Controllers
{
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly EventRepository _repository = null;
        public SaleController()
        {
            this._repository = new EventRepository();
        }

        [HttpGet]
        [Route("director/getAllSales")]
        public ActionResult<IEnumerable<JObject>> GetAllSales()
        {
            return Ok(_repository.GetSales());
        }

        [HttpGet]
        [Route("director/getToTalAmount")]
        public ActionResult<double> GetSalesTotalPrice()
        {
            return Ok(_repository.GetSalesTotalPrice());
        }

        [HttpGet]
        [Route("InventoryManager/getAll")]
        public ActionResult<IEnumerable<JToken>> GetInventory()
        {
            return Ok(_repository.GetInventory());
        }

        [HttpPost]
        [Route("saleman/addSale")]
        public OkObjectResult AddSale([FromBody] Sale sale)
        {
            var savedSale = new Sale(sale.Quantity, sale.Product.Name, sale.Price);
            _repository.Save(sale);
            return Ok(sale);
        }
    }
}
