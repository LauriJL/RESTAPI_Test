using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restful_Lopputehtava_LauriLeskinen.Models;

namespace Restful_Lopputehtava_LauriLeskinen.Controllers
{
    [Route("northwind/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        //Hakee kaikki tilaukset
        //HUOM Orders.cs luokkamäärityksessä public Orders metodi siirretty tiedoston loppuun - muuten ei haku ei toimi.

        [HttpGet]
        [Route("")]
        public List<Orders> GetAllOrders()
        {
            northwindContext db = new northwindContext();
            List<Orders> tilaukset = db.Orders.ToList();
            return tilaukset;
        }

        //Hakee tilauksen id-numeron perusteella

        [HttpGet]
        [Route("id/{id}")]
        public List<Orders> GetOrderById(int id)
        {
            northwindContext db = new northwindContext();
            Orders tilaus = db.Orders.Find(id);
            var orderID = from o in db.Orders
                            where o.OrderId == id
                            select o;
            return orderID.ToList();
        }

        //Hakee tilauksen asiakas ID:n perusteella

        [HttpGet]
        [Route("customerID/{customerid}")]
        public List<Orders> GetOrderByCustomerId(string customerid)
        {
            northwindContext db = new northwindContext();
            Orders tilaus = db.Orders.Find(customerid);
            var customerId = from o in db.Orders
                          where o.CustomerId == customerid
                          select o;
            return customerId.ToList();
        }

        //Lisää uuden tilauksen (post)
        //HUOM CustomerID ja EmployeeID FK constraint: bodyn näissä kohdissa pitää olla arvot, jotka löytyvät jo tietokannasta ao. tauluista.

        [HttpPost]
        [Route("add/")]
        public ActionResult AddNewOrder([FromBody] Orders neworder)
        {
            northwindContext db = new northwindContext();

            try
            {
                db.Orders.Add(neworder);
                db.SaveChanges();
                return Ok("Uusi tilaus ID:llä " + neworder.OrderId + " lisätty.");
            }
            catch (Exception)
            {
                return BadRequest("Uuden tilauksen lisääminen epäonnistui.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Tilaustietojen päivitys kokonaan (put)

        [HttpPut]
        [Route("update/{id}")]
        public ActionResult UpdateOrderById(int id, [FromBody] Orders tilaus)
        {
            northwindContext db = new northwindContext();

            try
            {
                Orders updateOrder = db.Orders.Find(id);
                if (updateOrder != null)
                {
                    updateOrder.CustomerId = tilaus.CustomerId;
                    updateOrder.EmployeeId = tilaus.EmployeeId;
                    updateOrder.OrderDate = tilaus.OrderDate;
                    updateOrder.RequiredDate = tilaus.RequiredDate;
                    updateOrder.ShippedDate = tilaus.ShippedDate;
                    updateOrder.ShipVia = tilaus.ShipVia;
                    updateOrder.Freight = tilaus.Freight;
                    updateOrder.ShipName = tilaus.ShipName;
                    updateOrder.ShipAddress = tilaus.ShipAddress;
                    updateOrder.ShipCity = tilaus.ShipCity;
                    updateOrder.ShipRegion = tilaus.ShipRegion;
                    updateOrder.ShipPostalCode = tilaus.ShipPostalCode;
                    updateOrder.ShipCountry = tilaus.ShipCountry;

                    db.SaveChanges();
                    return Ok("Tilauksen " + updateOrder.OrderId + " tiedot päivitetty.");
                }
                else
                {
                    return NotFound("Tilausta ei löytynyt ID:llä " + id.ToString());
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen tilausta päivitettäessä.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Tilauksen osoitetietojen päivitys (patch)

        [HttpPatch]
        [Route("updateaddress/{id}")]
        public ActionResult UpdateAddress(int id, [FromBody] Orders tilaus)
        {
            northwindContext db = new northwindContext();

            try
            {
                Orders updateAddress = db.Orders.Find(id);
                if (updateAddress != null)
                {
                    updateAddress.ShipAddress = tilaus.ShipAddress;
                    updateAddress.ShipCity = tilaus.ShipCity;
                    updateAddress.ShipRegion = tilaus.ShipRegion;
                    updateAddress.ShipPostalCode = tilaus.ShipPostalCode;
                    updateAddress.ShipCountry = tilaus.ShipCountry;

                    db.SaveChanges();
                    return Ok("Tilauksen numero " + updateAddress.OrderId + " toimitusosoite päivitetty.");
                }
                else
                {
                    return NotFound("Tilausta ei löytynyt numerolla " + updateAddress.OrderId);
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen ythestietojen päivityksessä.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Tilauksen poistaminen

        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult DeleteOrder(int id)
        {
            northwindContext db = new northwindContext();

            Orders tilaus = db.Orders.Find(id);
            try
            {
                if (tilaus != null)
                {
                    db.Orders.Remove(tilaus);
                    db.SaveChanges();
                    return Ok("Tilaus numero " + tilaus.OrderId + " poistettu kannasta.");
                }
                else
                {
                    return NotFound("Tilausta numero " + tilaus.OrderId + " ei löytynyt");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen tilausta poistettaessa.");
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
