using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restful_Lopputehtava_LauriLeskinen.Models;


namespace Restful_Lopputehtava_LauriLeskinen.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("northwind/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {      
        [HttpGet]
        [Route("")]
        public List<Customers> GetAllCustomers()
        {
            northwindContext db = new northwindContext();
            List<Customers> asiakkaat = db.Customers.ToList();
            return asiakkaat;
        }
        //Hakee rivit: offset = ensimmäinen haettava rivi; limit = montako riviä haetaan; country = maan mukaan haku
        [HttpGet]
        [Route("R")]
        public ActionResult GetSomeCustomers(int page, int limit, string country)
        {
            if (country != null) //Jos HTTPGET-pyynnön mukaan tulee country, haetaan sen mukaan 
            {
                northwindContext db = new northwindContext();
                List<Customers> asiakkaat = db.Customers.Where(x => x.Country == country).Take(limit).ToList();
                return Ok(asiakkaat);
            }
            else //HTTPGET-pyynnössä ei countrya, haetaan vain määrän mukaan
            {
                northwindContext db = new northwindContext();
                List<Customers> asiakkaat = db.Customers.Skip(page).Take(limit).ToList();
                return Ok(asiakkaat);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("add/")]
        public ActionResult AddNewCustomer([FromBody] Customers uusiasiakas)
        {
            northwindContext db = new northwindContext();

            try
            {
                db.Customers.Add(uusiasiakas);
                db.SaveChanges();
                return Ok("Uusi asiakas lisätty tietokantaan.");
            }
            catch (Exception)
            {
                return BadRequest("Uuden asiakkaan lisääminen epäonnistui.");
            }
            finally
            {
                db.Dispose();
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        [Route("update/{id}")]
        public ActionResult UpdateCustomerData(string id, [FromBody] Customers customer)
        {
            northwindContext db = new northwindContext();

            try
            {
                Customers customerupdate = db.Customers.Find(id);
                if (customerupdate != null)
                {
                    customerupdate.CompanyName = customer.CompanyName;
                    customerupdate.ContactName = customer.ContactName;
                    customerupdate.ContactTitle = customer.ContactTitle;
                    customerupdate.Address = customer.Address;
                    customerupdate.City = customer.City;
                    customerupdate.Region = customer.Region;
                    customerupdate.PostalCode = customer.PostalCode;
                    customerupdate.Country = customer.Country;
                    customerupdate.Phone = customer.Phone;
                    customerupdate.Fax = customer.Fax;

                    db.SaveChanges();
                    return Ok("Asiakkaan " + customer.CustomerId +  " tiedot päivitetty.");
                }
                else
                {
                    return NotFound("ID:llä " + id.ToString() + " ei löytynyt yhtään asiakasta.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Asiakkaan tietojen päivitys epäonnistui.");
            }
            finally
            {
                db.Dispose();
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult DeleteCustomer(string id)
        {
            northwindContext db = new northwindContext();

            Customers customer = db.Customers.Find(id);

            try
            {
                if (customer != null)
                {
                    db.Customers.Remove(customer);
                    db.SaveChanges();
                    return Ok("Asiakas " + id + " poistettu.");
                }
                else
                {
                    return BadRequest("ID:llä " + id+ " ei löytynyt yhtään asiakasta.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen asiakkaan postamisessa.");
            }
            finally
            {
                db.Dispose();
            }
        }

    }
}
