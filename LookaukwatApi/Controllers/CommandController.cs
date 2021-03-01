using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using LookaukwatApi.Models;
using LookaukwatApi.Services;
using LookaukwatApi.ViewModel;
using Microsoft.AspNet.Identity;

namespace LookaukwatApi.Controllers
{
    public class CommandController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Command
        public IQueryable<CommandModel> GetCommands()
        {
            return db.Commands;
        }

        [Authorize]
        [Route("api/Command/GetUserCommand")]
        public List<CommandForMobile> GetUserCommand()
        {
            string UserId = User.Identity.GetUserId();

            var commands = db.Commands.Where(m => m.user.Id == UserId).Select(s => new CommandForMobile
            {
                Id = s.Id,
                CommandDate = s.CommandDate,
                DeliveredDate = s.DeliveredDate,
                IsDelivered = s.IsDelivered,
                CommandId = s.CommandId,
                IsHomeDelivered = s.IsHomeDelivered
                
            }).OrderByDescending(o => o.Id).ToList();

          
            return commands;


        }


        //Solution passed through purchase
        //[Authorize]
        //[Route("api/Command/GetProviderSold")]
        //public List<CommandForMobile> GetProviderSold()
        //{
        //    string UserId = User.Identity.GetUserId();

        //    var commands = db.Commands.Select(p =>p.Purchases).Select(s => new CommandForMobile
        //    {
        //        Id = s.Id,
        //        CommandDate = s.CommandDate,
        //        DeliveredDate = s.DeliveredDate,
        //        IsDelivered = s.IsDelivered,
        //        CommandId = s.CommandId,
        //        IsHomeDelivered = s.IsHomeDelivered

        //    }).OrderByDescending(o => o.Id).ToList();


        //    return commands;


        //}


        // GET: api/Command/5
        [ResponseType(typeof(CommandModel))]
        public async Task<IHttpActionResult> GetCommandModel(int id)
        {
            CommandModel commandModel = await db.Commands.FirstOrDefaultAsync(model => model.CommandId == id);
            if (commandModel == null)
            {
                return NotFound();
            }

            return Ok(commandModel);
        }


        [HttpGet]
        [Route("api/Command/DeliveredCommand")]
        public async Task<string> DeliveredCommand(int id)
        {
            CommandModel commandModel = await db.Commands.FirstOrDefaultAsync(model => model.CommandId == id);
            if (commandModel == null)
            {
                return "La Commande n'existe pas !";
            }
            else
            {
                if (commandModel.IsDelivered)
                {
                    return "Commande a déja été livrée";
                }
                else
                {
                    commandModel.IsDelivered = true;
                    commandModel.DeliveredDate = DateTime.UtcNow;

                    await db.SaveChangesAsync();
                    return "Livraison validée avec succès !";
                }
            }
           

        }

        // PUT: api/Command/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCommandModel(int id, CommandModel commandModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != commandModel.Id)
            {
                return BadRequest();
            }

            db.Entry(commandModel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandModelExists(id))
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

        // POST: api/Command
        [ResponseType(typeof(CommandModel))]
        [Authorize]
        public async Task<IHttpActionResult> PostCommandModel(CommandModel commandModel)
        {
         
           foreach(var purchase in commandModel.Purchases.ToList())
            {
                var product = await db.Products.FirstOrDefaultAsync(model => model.id == purchase.product.id);
                if (product != null)
                {
                    if (product.Stock >= purchase.Quantities)
                    {
                        product.Stock = product.Stock - purchase.Quantities;

                        if(product.Stock == 0)
                        {
                            product.IsActive = false;
                        }
                        purchase.product = product;
                    }

                }
                else
                {
                    return BadRequest();
                }
                    
            }
           // add user
            string UserId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(m => m.Id == UserId);

            commandModel.user = user;

            // add time
            commandModel.CommandDate = DateTime.UtcNow;

            db.Commands.Add(commandModel);
            await db.SaveChangesAsync();
            var command = DateTime.Now.Year.ToString() + commandModel.Id.ToString();
            commandModel.CommandId = Convert.ToInt32(command); 

            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { CommandId = commandModel.CommandId }, commandModel);
        }


        //For TrackingComand
        [HttpPost]
        [Route("api/TrackingCommand")]
       
        [Authorize]
        public async Task<bool> PostTrackingCommand(TrackingCommandModel trakingCommand)
        {
            //add command
            CommandModel commandModel = await db.Commands.FirstOrDefaultAsync(model => model.CommandId == trakingCommand.Command.CommandId);
            if (commandModel != null)
            {
                trakingCommand.Command = commandModel;
            }
            else
            {
                return false;
            }
           
            // add user
            string UserId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(m => m.Id == UserId);

            trakingCommand.UserAgent = user;

            var location = await CoordonateService.GetLocationAsync(trakingCommand.Lat, trakingCommand.Lon);

            trakingCommand.Town = location.Town;
            trakingCommand.Street = location.Street;
            trakingCommand.Road = location.Road;
            // add time
            trakingCommand.Date = DateTime.UtcNow;

            db.TrackingCommands.Add(trakingCommand);
            await db.SaveChangesAsync();
            
            return true;
        }

        //tracking
        [HttpGet]
        [Route("api/TrackingCommand")]
        public List<TrackingCommandViewModel> GetCommands(int id)
        {
            return db.TrackingCommands.Where(model =>model.Command.CommandId == id).Select(s =>new TrackingCommandViewModel 
            { 
                Date = s.Date,
                Town = s.Town,
                Road =s.Road,
                Street = s.Street,
                Lat = s.Lat,
                Lon = s.Lon
            }).ToList();
        }


        // DELETE: api/Command/5
        [ResponseType(typeof(CommandModel))]
        public async Task<IHttpActionResult> DeleteCommandModel(int id)
        {
            CommandModel commandModel = await db.Commands.FindAsync(id);
            if (commandModel == null)
            {
                return NotFound();
            }

            db.Commands.Remove(commandModel);
            await db.SaveChangesAsync();

            return Ok(commandModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommandModelExists(int id)
        {
            return db.Commands.Count(e => e.Id == id) > 0;
        }
    }
}