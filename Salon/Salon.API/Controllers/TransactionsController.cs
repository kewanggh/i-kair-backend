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
using Salon.API.Infrastructure;
using Salon.API.Models;
using Stripe;
using System.Diagnostics;
using Salon.API.Requests;
using Salon.API.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions;
//using System.Web.Mvc;

namespace Salon.API.Controllers
{
   [Authorize]
    public class TransactionsController : ApiController
    {
        private SalonDataContext db = new SalonDataContext();

        // GET: api/Transactions
        public IQueryable<TransactionDTO> GetTransactions()
        {
            //return db.Transactions;
            return db.Transactions.ProjectTo<TransactionDTO>();
        }

        // GET: api/Transactions/5
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

        // PUT: api/Transactions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransaction(int id, Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.TransactionId)
            {
                return BadRequest();
            }
            // 1. Get the appointment itself from the database
            var transactionInDb = db.Transactions.Find(id);

            // 2. Update appointmentInDb with the VALUES of the appointment object
            db.Entry(transactionInDb).CurrentValues.SetValues(transaction);

            // 3. Mark appointmentInDb as modified (instead of appointments)
            db.Entry(transactionInDb).State = EntityState.Modified;

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

        // POST: api/Transactions
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult PostTransaction(TransactionDTO transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Get the username printed on the incoming token
            string username = User.Identity.Name;

            // Get the actual user from the database (may return null if not found!)
            var user = db.Users.FirstOrDefault(u => u.UserName == username);
 
            if (user == null) { return Unauthorized(); }

            var dbTransaction = new Transaction(transaction);
            dbTransaction.UserId = user.Id;
            dbTransaction.Date = DateTime.Now;


            db.Transactions.Add(dbTransaction);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = transaction.TransactionId }, Mapper.Map<TransactionDTO>(dbTransaction));
        }

        //[Route("api/transactions/{transactionId}/product/{productId}")]
        //[HttpPost]
        //public IHttpActionResult AddProductToTransaction(int transactionId, int productId)
        //{
        //    var transaction = db.Transactions.Find(transactionId);
        //    var product = db.Products.Find(productId);

        //    transaction.Products.Add(product);

        //    db.Entry(transaction).State = EntityState.Modified;

        //    db.SaveChanges();

        //    return Ok();
        //}

        //[Route("api/transactions/{transactionId}/product/{productId}/{quantity}")]
        //[HttpPost]
        //public IHttpActionResult AddProductToTransactionWithQuantity(int transactionId, int productId, int quantity)
        //{
        //    var transaction = db.Transactions.Find(transactionId);
        //    var product = db.Products.Find(productId);

        //    for(var i = 0; i < quantity; i++)
        //    {
        //        transaction.Products.Add(product);
        //    }

        //    db.Entry(transaction).State = EntityState.Modified;

        //    db.SaveChanges();

        //    return Ok();
        //}
        [Route("api/transactions/{transactionId}/product/{productId}/{quantity}")]
        [HttpPost]
        public IHttpActionResult AddProductToTransactionWithQuantity(int transactionId, int productId, int quantity)
        {
            var transaction = db.Transactions.Find(transactionId);
            var product = db.Products.Find(productId);

            
                transaction.TransactionProducts.Add(new TransactionProduct
                {
                    Product = product,
                    Quantity = quantity
                   
                });
            

            db.Entry(transaction).State = EntityState.Modified;

            db.SaveChanges();

            return Ok();
        }
        // PUT: api/Transactions/5
        [Route("api/transactions/{transactionId}/product/{productId}/{quantity}")]
        [HttpPut]
        public IHttpActionResult UpdateQuantityForOneProduct(int transactionId, int productId, int quantity)
        {
            var transactionProduct = db.TransactionProducts.Find(transactionId, productId);

            transactionProduct.Quantity = quantity;
            
            db.Entry(transactionProduct).State = EntityState.Modified;

            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }


        [Route("api/transactions/{transactionId}/service/{serviceId}/{quantity}")]
        [HttpPost]
        public IHttpActionResult AddServiceToTransactionWithQuantity(int transactionId, int serviceId, int quantity)
        {
            var transaction = db.Transactions.Find(transactionId);
            var service = db.Services.Find(serviceId);


            transaction.TransactionServices.Add(new TransactionService
            {
                Service = service,
                Quanitity = quantity

            });


            db.Entry(transaction).State = EntityState.Modified;

            db.SaveChanges();

            return Ok();
        }













        [Route("api/stripe")]
        [HttpPost]
        //[ValidateAntiForgeryToken()]
        public IHttpActionResult UsingTokenToCharge(StripeChargeRequest request)
        {
            
            var transaction = db.Transactions.Find(request.TransactionId);

            var chargeOptions = new StripeChargeCreateOptions()
            {
                // required
                Amount = (int)(transaction.TotalAmount.GetValueOrDefault() * 100),
                Currency = "usd",
                ReceiptEmail = "shawn_k03@hotmail.com",
                Source = new StripeSourceOptions() { TokenId = request.StripeToken }
                //Description = string.Format("something," stripemail,
                //ReceiptEmail = customViewModel.StripeEmail
            };
            var chargeService = new StripeChargeService();
            try
            {
                var stripeCharge = chargeService.Create(chargeOptions);
                transaction.ChargeId = stripeCharge.Id;
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(stripeCharge);
            }
            catch (StripeException stripeException)
            {
                Debug.WriteLine(stripeException.Message);
                ModelState.AddModelError(string.Empty, stripeException.Message);
                return BadRequest(ModelState);
            }
            
            
            
        }


        // DELETE: api/Transactions/5
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
            return db.Transactions.Count(e => e.TransactionId == id) > 0;
        }
    }
}