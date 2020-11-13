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
    public class EmployeesController : ControllerBase
    {
        //Hakee kaikki työntekijät
        //HUOM Internal server error 505 - sisäinen loop/circular reference.
        //Error ratkaistu lisäämällä Employees luokkamäärityksessä [JsonIgnore] ReportsToNavigation ja InverseReportsToNavigation sisäisen PK-FK olion eteen.
        //[JsonIgnore] lisätty myös Photo ja PhotPath määrityksiin, koska muuten tulostuu älyttömän pitkät URLit.

        [HttpGet]
        [Route("")]
        public List<Employees> GetAllEmployees()
        {
            northwindContext db = new northwindContext();
            List<Employees> duunarit = db.Employees.ToList();
            return duunarit;
        }

        //Hakee työntekijän ID:n perusteella

        [HttpGet]
        [Route("id/{id}")]
        public List<Employees> GetEmployeeById(int id)
        {
            northwindContext db = new northwindContext();

            var duunariId = from e in db.Employees
                            where e.EmployeeId == id
                            select e;
            return duunariId.ToList();            
        }

        //Hakee työntekijän sukunimen perusteella
        //HUOM pitää olla koko sukunimi

        [HttpGet]
        [Route("name/{nimi}")]

        public List<Employees> GetEmployeeByName(string nimi)
        {
            northwindContext db = new northwindContext();

            var duunariNimi = from e in db.Employees
                              where e.LastName == nimi
                              select e;

            return duunariNimi.ToList();
        }

        //Uuden työnetkijän lisääminen

        [HttpPost]
        [Route("add/")]

        public ActionResult AddNewEmployee([FromBody] Employees uusiduunari)
        {
            northwindContext db = new northwindContext();

            try
            {
                db.Employees.Add(uusiduunari);
                db.SaveChanges();
                return Ok("Uusi työntekijä " + uusiduunari.FirstName + " " + uusiduunari.LastName + " lisätty tietokantaan.");
            }
            catch (Exception)
            {
                return BadRequest("Uuden työntekijän lisääminen epäonnistui.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Työntekijän tietojen päivitys kokonaan (put)

        [HttpPut]
        [Route("update/{id}")]

        public ActionResult UpdateEmployeeData(int id, [FromBody] Employees duunari)
        {
            northwindContext db = new northwindContext();

            try
            {
                Employees duunaripaivitys = db.Employees.Find(id);
                if (duunaripaivitys != null)
                {
                    duunaripaivitys.LastName = duunari.LastName;
                    duunaripaivitys.FirstName = duunari.FirstName;
                    duunaripaivitys.Title = duunari.Title;
                    duunari.TitleOfCourtesy = duunari.TitleOfCourtesy;
                    duunaripaivitys.BirthDate = duunari.BirthDate;
                    duunaripaivitys.HireDate = duunari.HireDate;
                    duunaripaivitys.Address = duunari.Address;
                    duunaripaivitys.City = duunari.City;
                    duunaripaivitys.Region = duunari.Region;
                    duunaripaivitys.PostalCode = duunari.PostalCode;
                    duunaripaivitys.Country = duunari.Country;
                    duunaripaivitys.HomePhone = duunari.HomePhone;
                    duunaripaivitys.Extension = duunari.Extension;
                    duunaripaivitys.Notes = duunari.Notes;
                    duunaripaivitys.ReportsTo = duunari.ReportsTo;

                    db.SaveChanges();
                    return Ok("Työntekijän " + duunari.FirstName + " " + duunari.LastName + " tiedot päivitetty.");
                }
                else
                {
                    return NotFound("ID:llä " + id.ToString() +  " ei löytynyt yhtään työntekijää ei löytynyt ID:llä.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Työntekijän tietojen päivitys epäonnistui.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Yhteystietojen päivitys (patch)

        [HttpPatch]
        [Route("editcontactinfo/{id}")]

        public ActionResult EditEmployeeContactInfo(int id, [FromBody] Employees duunari)
        {
            northwindContext db = new northwindContext();

            try
            {
                Employees duunariPatch = db.Employees.Find(id);
                if (duunariPatch != null)
                {
                    duunariPatch.Address = duunari.Address;
                    duunariPatch.City = duunari.City;
                    duunariPatch.Region = duunari.Region;
                    duunariPatch.PostalCode = duunari.PostalCode;
                    duunariPatch.Country = duunari.Country;
                    duunariPatch.HomePhone = duunari.HomePhone;
                    duunariPatch.Extension = duunari.Extension;

                    db.SaveChanges();
                    return Ok("Työntekijän " + id.ToString() + " yhteystiedot päivitetty.");
                }
                else
                {
                    return NotFound("ID:llä " + id.ToString() + " ei löytynyt yhtään työntekijää.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Yhteystietojen päivitys epäonnistui.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Työntekijän poistaminen

        [HttpDelete]
        [Route("delete/{id}")]

        public ActionResult DeleteEmployee(int id)
        {
            northwindContext db = new northwindContext();

            Employees duunari = db.Employees.Find(id);

            try
            {
                if (duunari != null)
                {
                    db.Employees.Remove(duunari);
                    db.SaveChanges();
                    return Ok("Työntekijä " + id.ToString() + " poistettu.");
                }
                else
                {
                    return BadRequest("ID:llä " + id.ToString() + " ei löytynyt yhtään työntekijää.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen työntekijän postamisessa.");
            }
            finally
            {
                db.Dispose();
            }

        }
    }
}
