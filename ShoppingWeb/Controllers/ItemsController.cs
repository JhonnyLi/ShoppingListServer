using ShoppingWeb.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace ShoppingWeb.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ItemsController : ApiController
    {
        private readonly DbOperations _dbOps;
        public ItemsController()
        {
            _dbOps = new DbOperations();
        }
        // GET: Items
        public IHttpActionResult Get()
        {
            var model = _dbOps.GetAllItems();

            return Json(model);
        }

        // GET api/items/5
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Get(int id)
        {
            //var model = _dbOps.GetShoppingListByIndex(id);
            return Json("Get + value");
        }

        // POST api/items
        public void Post([FromBody]string value)
        {
        }

        // PUT api/items/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/items/5
        public void Delete(int id)
        {
        }
    }
}