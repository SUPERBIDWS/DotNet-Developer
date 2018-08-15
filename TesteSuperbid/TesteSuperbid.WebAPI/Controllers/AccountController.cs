using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TesteSuperbid.WebAPI.Models;

namespace TesteSuperbid.WebAPI.Controllers
{
    public class AccountController : ApiController
    {
        private Context db = new Context();

        // GET api/Account
        public IQueryable<Account> GetAccounts()
        {
            return db.Accounts;
        }

        // GET api/Account/5
        [ResponseType(typeof(Response))]
        public IHttpActionResult GetAccount(int id)
        {
            Response response = new Response();
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                response.Error = true;
                response.Msg = "Conta não encontrada";
                return Ok(response);
            }

            response.Obj = account;
            return Ok(response);
        }

        // PUT api/Account/5
        public IHttpActionResult PutAccount(int id, Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != account.Id)
            {
                return BadRequest();
            }

            db.Entry(account).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Account
        [ResponseType(typeof(Account))]
        public IHttpActionResult PostAccount(Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Accounts.Add(account);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = account.Id }, account);
        }

        // DELETE api/Account/5
        [ResponseType(typeof(Account))]
        public IHttpActionResult DeleteAccount(int id)
        {
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }

            db.Accounts.Remove(account);
            db.SaveChanges();

            return Ok(account);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccountExists(int id)
        {
            return db.Accounts.Count(e => e.Id == id) > 0;
        }
    }
}