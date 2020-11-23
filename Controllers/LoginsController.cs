using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restful_Lopputehtava_LauriLeskinen.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Restful_Lopputehtava_LauriLeskinen.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [Route("northwind/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public List<Logins> GetAllLogins()
        {
            northwindContext db = new northwindContext();
            List<Logins> logins = db.Logins.ToList();
            return logins;
        }

        [HttpGet]
        [Route("R")]
        public ActionResult GetSomeUsers(int page, int limit, string firstname, string lastname, string accesslevel)
        {
            if (firstname != null)
            {
                northwindContext db = new northwindContext();
                List<Logins> kayttaja = db.Logins.Where(x => x.Firstname == firstname).Take(limit).ToList();
                //List<Logins> kayttaja = db.Logins.Where(x => x.Firstname == firstname).ToList();
                return Ok(kayttaja);
            }
            else if (lastname != null)
            {
                northwindContext db = new northwindContext();
                List<Logins> kayttaja = db.Logins.Where(x => x.Lastname == lastname).Take(limit).ToList();
                //List<Logins> kayttaja = db.Logins.Where(x => x.Lastname == lastname).ToList();
                return Ok(kayttaja);
            }
            else if (accesslevel != null)
            {
                northwindContext db = new northwindContext();
                List<Logins> kayttaja = db.Logins.Where(x => x.AccesslevelId.ToString() == accesslevel).Take(limit).ToList();
                //List<Logins> kayttaja = db.Logins.Where(x => x.AccesslevelId.ToString() == accesslevel).ToList();
                return Ok(kayttaja);
            }
            else
            {
                northwindContext db = new northwindContext();
                List<Logins> kayttaja = db.Logins.Skip(page).Take(limit).ToList();
                //List<Logins> kayttaja = db.Logins.ToList();
                return Ok(kayttaja);
            }
        }

        //Uuden käyttäjän lisäys    
        
        [HttpPost]
        [Route("add/")]
        public ActionResult AddNewUser([FromBody] Logins uusikayttaja)
        {
            northwindContext db = new northwindContext();

            try
            {
                db.Logins.Add(uusikayttaja);
                db.SaveChanges();
                return Ok(uusikayttaja.Username + " lisätty.");
            }
            catch (Exception)
            {
                return BadRequest("Uuden käyttäjän lisääminen ei onnistunut.");
            }
            finally
            {
                db.Dispose();
            }
        }

        //Käyttäjätietojen päivitys kokonaan (put)
        [HttpPut]
        [Route("update/{id}")]
        public ActionResult UpdateUser(int id, [FromBody] Logins kayttaja)
        {
            northwindContext db = new northwindContext();

            try
            {
                Logins updateKayttaja = db.Logins.Find(id);
                if (updateKayttaja != null)
                {
                    updateKayttaja.Firstname = kayttaja.Firstname;
                    updateKayttaja.Lastname = kayttaja.Lastname;
                    updateKayttaja.Email = kayttaja.Email;
                    updateKayttaja.Username = kayttaja.Username;
                    updateKayttaja.Password = kayttaja.Password;
                    updateKayttaja.AccesslevelId = kayttaja.AccesslevelId;

                    db.SaveChanges();
                    return Ok("Käyttäjän tiedot päivitetty.");
                }
                else
                {
                    return NotFound("Käyttäjää ei löytynyt.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen käyttäjän tietoja päivitettäessä.");
            }
            finally
            {
                db.Dispose();
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult DeleteUser(int id)
        {
            northwindContext db = new northwindContext();

            Logins kayttaja = db.Logins.Find(id);
            try
            {
                if (kayttaja != null)
                {
                    db.Logins.Remove(kayttaja);
                    db.SaveChanges();
                    return Ok("Käyttäjä " + kayttaja.Username + " poistettu.");
                }
                else
                {
                    return NotFound("Käyttäjää ei löydy.");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen käyttäjää poistettaessa.");
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
