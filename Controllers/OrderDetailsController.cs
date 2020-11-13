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
    public class OrderDetailsController : ControllerBase
    {
        //Hakee kaikki tilausdetaljit

        [HttpGet]
        [Route("")]
        public List<OrderDetails> GetAllOrderDetails()
        {
            northwindContext db = new northwindContext();
            List<OrderDetails> tilausdetaljit = db.OrderDetails.ToList();
            return tilausdetaljit;
        }

        //Hakee tilausdetaljit OrderId:n perusteella
        //Haku pelkällä orderId:llä tuottaa errorin. Ratkaistu lisäämällä toinen parametri (int id2), joka viittaa toiseen PK:hon (ProductID): where o.OrderId == id || o.ProductId == id2

        [HttpGet]
        [Route("orderid/{id}")]
        public List<OrderDetails> GetOrderDetailById(int id, int id2)
        {
            northwindContext db = new northwindContext();
            OrderDetails tilausdetaljit = db.OrderDetails.Find(id, id2);
            var orderDetailId = from o in db.OrderDetails
                                where o.OrderId == id || o.ProductId == id2
                                select o;
            return orderDetailId.ToList();
        }

        //Hakee tilausdetaljit ProductId:n perusteella

        [HttpGet]
        [Route("productid/{id}")]
        public List<OrderDetails> GetOrderDetailByProductId(int id, int id2)
        {
            northwindContext db = new northwindContext();
            OrderDetails tilausdetaljit = db.OrderDetails.Find(id, id2);
            var orderDetailId = from o in db.OrderDetails
                                where o.ProductId == id || o.OrderId == id2
                                select o;
            return orderDetailId.ToList();
        }

        //Hakee tilausdetaljit OrderId+ProductId yhdistelmän perusteella

        [HttpGet]
        [Route("orderandproductid/{orderid}/{productid}")]
        public List<OrderDetails> GetOrderDetailById2(int orderid, int productid)
        {
            northwindContext db = new northwindContext();
            OrderDetails tilausdetaljit = db.OrderDetails.Find(orderid, productid);
            var orderDetailId2 = from o in db.OrderDetails
                                where o.OrderId == orderid & o.ProductId == productid
                                select o;
            return orderDetailId2.ToList();
        }

        //Lisää uuden OrderDetailin
        //HUOM! Bodyssa pitää olla olemassaoleva orderId ja productId eikä tällaista yhdistelmää saa vielä löytyä OrderDetails taulusta

        [HttpPost]
        [Route("add/")]
        public ActionResult AddNewOrderDetail([FromBody] OrderDetails newdetail)
        {
            northwindContext db = new northwindContext();

            try
            {
                db.OrderDetails.Add(newdetail);
                db.SaveChanges();
                return Ok("Uusi tilausdetalji lisätty.");
            }
            catch (Exception)
            {
                return BadRequest("Uuden tilausdetaljin lisääminen epäonnistui.");
                //return BadRequest(e);
            }
            finally
            {
                db.Dispose();
            }
        }

        //Päivittää tilausdetaljin (put)
        //HUOM Tähän tarvitaan parametreiksi sekä orderId että productId koska molemmat ovat PK OrderDetails taulussa

        [HttpPut]
        [Route("update/{orderid}/{productid}")]
        public ActionResult UpdateOrderDetailById(int orderid, int productid, [FromBody] OrderDetails detalji)
        {
            northwindContext db = new northwindContext();

            try
            {
                OrderDetails updateOrderDetail = db.OrderDetails.Find(orderid, productid);
                if (updateOrderDetail != null)
                {
                    updateOrderDetail.UnitPrice = detalji.UnitPrice;
                    updateOrderDetail.Quantity = detalji.Quantity;
                    updateOrderDetail.Discount = detalji.Discount;

                    db.SaveChanges();
                    return Ok("Tilausdetaljin (orderId: " + orderid.ToString() + " + productId: " + productid.ToString() + ") tiedot päivitetty.");
                }
                else
                {
                    return NotFound("Tilausdetaljia (orderId: " + orderid.ToString() + " + productId: " + productid.ToString() + ") ei löydy kannasta.");
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

        //Tilausdetaljin poistaminen

        [HttpDelete]
        [Route("delete/{orderid}/{productid}")]
        public ActionResult DeleteOrderDetail(int orderid, int productid)
        {
            northwindContext db = new northwindContext();

            OrderDetails detalji = db.OrderDetails.Find(orderid, productid);
            try
            {
                if (detalji != null)
                {
                    db.OrderDetails.Remove(detalji);
                    db.SaveChanges();
                    return Ok("Tilausdetalji yhdistelmällä orderId: " + detalji.OrderId + " + productId: " + detalji.ProductId + " poistettu kannasta.");
                }
                else
                {
                    return NotFound("Tilausdetaljia yhdistelmällä orderId: " + orderid.ToString() + " + productId: " + productid.ToString() + " ei löytynyt kannasta.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen tilausdetaljia poistettaessa.");
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
