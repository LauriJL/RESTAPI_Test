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
    public class ProductsController : ControllerBase
    {
        //Hakee kaikki tuotteet
        [HttpGet]
        [Route("")]
        public List<Products> GetAllProducts()
        {
            northwindContext db = new northwindContext();
            List<Products> tuotteet = db.Products.ToList();
            return tuotteet;
        }

        [HttpGet]
        [Route("R")]
        public ActionResult GetSomeProducts(int page, int limit, string name, string supplierid)
        {
            if (name != null)
            {
                northwindContext db = new northwindContext();
                List<Products> tuotteet = db.Products.Where(x => x.ProductName == name).Take(limit).ToList();
                return Ok(tuotteet);
            }
            else if (supplierid != null)
            {
                northwindContext db = new northwindContext();
                List<Products> tuotteet = db.Products.Where(x => x.SupplierId.ToString() == supplierid).Take(limit).ToList();
                return Ok(tuotteet);
            }
            else 
            {
                northwindContext db = new northwindContext();
                List<Products> tuotteet = db.Products.Skip(page).Take(limit).ToList();
                return Ok(tuotteet);
            }
        }

        //Hakee tuotteen ID:n perusteella
        [HttpGet]
        [Route("id/{id}")]
        public List<Products> GetProductById(int id)
        {
            northwindContext db = new northwindContext();

            Products tuote = db.Products.Find(id);
            var productID = from p in db.Products
                            where p.ProductId == id
                            select p;
            return productID.ToList();
        }

        //Hakee tuotteen nimen perusteella
        //HUOM pitää olla koko nimi

        [HttpGet]
        [Route("name/{name}")]
        public List<Products> GetProductByName(string name)
        {
            northwindContext db = new northwindContext();
            var productName = from p in db.Products
                                where p.ProductName == name
                                select p;
            return productName.ToList();
        }

        //Uuden tuotteen lisäys
        [HttpPost]
        [Route("add/")]
        public ActionResult AddNewProduct([FromBody] Products uusituote)
        {
            northwindContext db = new northwindContext();

            try
            {
                db.Products.Add(uusituote);
                db.SaveChanges();
                return Ok(uusituote.ProductName + " lisätty.");
            }
            catch (Exception)
            {
                return BadRequest("Uuden tuotteen lisääminen ei onnistunut.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Tuotetetietojen päivitys kokonaan (put)
        [HttpPut]
        [Route("update/{id}")]
        public ActionResult UpdateProduct(int id, [FromBody] Products tuote)
        {
            northwindContext db = new northwindContext();

            try
            {
                Products updateTuote = db.Products.Find(id);
                if (updateTuote != null)
                {
                    updateTuote.ProductName = tuote.ProductName;
                    updateTuote.SupplierId = tuote.SupplierId;
                    updateTuote.CategoryId = tuote.CategoryId;
                    updateTuote.QuantityPerUnit = tuote.QuantityPerUnit;
                    updateTuote.UnitPrice = tuote.UnitPrice;
                    updateTuote.UnitsInStock = tuote.UnitsInStock;
                    updateTuote.UnitsOnOrder = tuote.UnitsOnOrder;
                    updateTuote.ReorderLevel = tuote.ReorderLevel;
                    updateTuote.Discontinued = tuote.Discontinued;
                    updateTuote.Category = tuote.Category;
                    updateTuote.Supplier = tuote.Supplier;

                    db.SaveChanges();
                    return Ok("Tuotteen tiedot päivitetty.");
                }
                else
                {
                    return NotFound("Tuotetta ei löytynyt.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen tuotetta päivitettäessä.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Tuotetetietojen päivitys osittain (patch)

        [HttpPatch]
        [Route("edit/{id}")]
        public ActionResult EditProduct(int id, [FromBody] Products tuote)
        {
            northwindContext db = new northwindContext();

            try
            {
                Products editTuote = db.Products.Find(id);
                if (editTuote != null)
                {
                    editTuote.ProductName = tuote.ProductName;
                    editTuote.UnitPrice = tuote.UnitPrice;
                    editTuote.UnitsInStock = tuote.UnitsInStock;

                    db.SaveChanges();
                    return Ok("Tuotteen " + tuote.ProductName + " tiedot päivitetty.");                    
                }
                else
                {
                    return NotFound("Hakemaasi tuotetta ei löytynyt.");
                };
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen tuotetta päivitettäessä.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Tuotteen poisto

        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult DeleteProduct(int id)
        {
            northwindContext db = new northwindContext();

            Products tuote = db.Products.Find(id);
            try
            {
                if (tuote != null)
                {
                    db.Products.Remove(tuote);
                    db.SaveChanges();
                    return Ok("Tuote " + tuote.ProductName + " poistettu.");
                }
                else
                {
                    return NotFound("Tuotetta ei löydy.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen tuotetta poistettaessa.");
            }
            finally
            {
                db.Dispose();
            }     
        }
    }
}
