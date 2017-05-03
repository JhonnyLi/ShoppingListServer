using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShoppingWeb.Models;
using System.Web.Http.Cors;
using ShoppingWeb.Interface;

namespace ShoppingWeb.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        private readonly DbOperations _dbOps;
        public ValuesController()
        {
            _dbOps = new DbOperations();
        }
        // GET api/values
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Get()
        {
            //var model = GetShoppingLists();
            var model = _dbOps.GetAllShoppingLists();
            if(model == null)
            {
                model = new List<ShoppingList>()
                {
                    new ShoppingList
                    {
                        Name="Inga listor existerar"
                    }
                };
            }
            return Json(model);
        }

        // GET api/values/5
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IHttpActionResult Get(int id)
        {
            //var model = GetShoppingListByIndex(id);
            var model = _dbOps.GetShoppingListByIndex(id);
            if (model == null)
            {
                model = new ShoppingList()
                {
                    Name = "Inga listor existerar"
                };
            }
            return Json(model);
        }

        // POST api/values
        public void Post([FromBody]ShoppingList model)
        {
            var test = model;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        private static List<ShoppingList> GetShoppingLists()
        {
            var model = new List<ShoppingList>()
            {
                new ShoppingList()
                {
                    ShoppingListId = Guid.NewGuid(),
                    Name = "Min shoppinglista"
                },
                new ShoppingList()
                {
                    ShoppingListId = Guid.NewGuid(),
                    Name = "En annan lista"
                }
            };
            model[0].Items.AddRange(new List<Item>()
            {
                new Item()
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Lök"
                },
                new Item()
                {
                    ItemId = Guid.NewGuid(),
                    Name =  "Smör"
                },
                new Item()
                {
                    ItemId=Guid.NewGuid(),
                    Name = "Bröd"
                }
            });
            model[1].Items.AddRange(new List<Item>
            {
                new Item()
                {
                    ItemId = Guid.NewGuid(),
                    Name = "Gröt"
                },
                new Item()
                {
                    ItemId = Guid.NewGuid(),
                    Name =  "Coca Cola"
                },
                new Item()
                {
                    ItemId=Guid.NewGuid(),
                    Name = "Kaviar"
                },
                new Item()
                {
                    ItemId=Guid.NewGuid(),
                    Name = "Champinjoner"
                },
                new Item()
                {
                    ItemId=Guid.NewGuid(),
                    Name = "Tomat"
                }


                //"Gröt", "Coca Cola", "Kaviar", "Champinjoner", "Tomat"


            });

            return model;
        }
        private static ShoppingList GetShoppingListByIndex(int index)
        {
            var lists = GetShoppingLists();
            if (index <= lists.Count)
            {
                return lists[index];
            }
            return new ShoppingList();
        }
    }

}
