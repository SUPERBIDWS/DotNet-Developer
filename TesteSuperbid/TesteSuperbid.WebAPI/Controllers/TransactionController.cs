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
    [RoutePrefix("api/Transaction")]
    public class TransactionController : ApiController
    {
        private Context db = new Context();

        // GET api/Transaction
        public IQueryable<Transaction> GetTransactions()
        {
            return db.Transactions
                .Include(c => c.AccountFrom)
                .Include(c => c.AccountTo);
        }

        [Route("Pending/{id}")]
        public IHttpActionResult GetPendingTransactions(int id)
        {
            Response response = new Response();

            Account acc = db.Accounts.Find(id);

            if (acc == null || acc.Id <= 0)
            {
                response.Error = true;
                response.Msg = "Não existe este Id de transferência";
                response.Obj = null;
                return Ok(response);
            }

            IQueryable<Transaction> transactions = db.Transactions.Where(c => c.AccountFrom.Id == id && c.Pending)
                .Include(c => c.AccountFrom)
                .Include(c => c.AccountTo);

            if (transactions == null || !transactions.Any())
            {
                response.Error = true;
                response.Msg = "Conta não contém transações";
                response.Obj = null;
            }

            response.Obj = transactions;

            return Ok(response);
        }

        // GET api/Transaction/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult GetTransaction(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [ResponseType(typeof(Response))]
        [Route("Confirm/{id}")]
        public IHttpActionResult ConfirmTransaction(int id)
        {
            Response response = new Response();
            Account account = new Account();
            try
            {
                Transaction transaction = db.Transactions
                    .Include(c => c.AccountFrom)
                    .Include(c => c.AccountTo)
                    .FirstOrDefault(c => c.Id == id);

                if (transaction == null || transaction.Id <= 0)
                {
                    response.Error = true;
                    response.Msg = "Não existe este Id de transferência";
                    response.Obj = null;
                    return Ok(response);
                }

                account = db.Accounts.Find(transaction.AccountFrom.Id);

                if (account.Balance - transaction.Ammount < 0)
                {
                    response.Error = true;
                    response.Msg = "Conta não possui saldo";
                    response.Obj = null;
                    return Ok(response);
                }

                transaction.Pending = false;
                db.Entry(transaction).State = EntityState.Modified;

                account.Balance -= transaction.Ammount;
                db.Entry(account).State = EntityState.Modified;

                account = db.Accounts.Find(transaction.AccountTo.Id);
                account.Balance += transaction.Ammount;
                db.Entry(account).State = EntityState.Modified;

                db.SaveChanges();

                response.Error = false;
                response.Obj = account;
            }
            catch
            {
                response.Error = true;
                return Ok(response);
            }
            return Ok(response);
        }

        // PUT api/Transaction/5
        public IHttpActionResult PutTransaction(int id, Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.Id)
            {
                return BadRequest();
            }

            db.Entry(transaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST api/Transaction
        [ResponseType(typeof(Response))]
        public IHttpActionResult PostTransaction(Transaction transaction)
        {
            Response response = new Response();
            if (!ModelState.IsValid)
            {
                response.Error = true;
                response.Msg = "Não foi possível completar a operação";
                return Ok(response);
            }

            if (transaction.AccountFrom == transaction.AccountTo)
            {
                response.Error = true;
                response.Msg = "As contas devem ser diferentes";
                return Ok(response);
            }

            Account acFrom = db.Accounts.FirstOrDefault(c => c.Id == transaction.AccountFrom.Id);
            Account acTo = db.Accounts.FirstOrDefault(c => c.Id == transaction.AccountTo.Id);

            if (acFrom == null || acFrom.Id <= 0)
            {
                response.Error = true;
                response.Msg = "Conta a ser debitada não existe";
                return Ok(response);
            }

            if (acTo == null || acTo.Id <= 0)
            {
                response.Error = true;
                response.Msg = "Conta a ser creditada não existe";
                return Ok(response);
            }

            Transaction tran = new Transaction()
            {
                Date = transaction.Date,
                AccountFrom = acFrom,
                AccountTo = acTo,
                Ammount = transaction.Ammount,
                Pending = true
            };

            db.Transactions.Add(tran);
            db.SaveChanges();

            response.Obj = tran;

            return CreatedAtRoute("DefaultApi", new { id = tran.Id }, response.Obj);
        }

        // DELETE api/Transaction/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult DeleteTransaction(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();

            return Ok(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(int id)
        {
            return db.Transactions.Count(e => e.Id == id) > 0;
        }
    }
}