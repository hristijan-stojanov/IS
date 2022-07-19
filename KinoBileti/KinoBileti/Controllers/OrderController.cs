using GemBox.Document;
using KinoBileti.Data;
using KinoBileti.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KinoBileti.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

               var loggedInUser = await _context.Users.Where(z => z.Id == userId)
              .Include(z => z.Orders)
              .Include("Orders.Bilets")
              .Include("Orders.Bilets.BIlet")
              .FirstOrDefaultAsync();

            var data = loggedInUser.Orders;
            return View(data);
        }
        public async Task<IActionResult> Pay(string stripeEmail, string stripeToken)

        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            

            var loggedInUser = await _context.Users.Where(z => z.Id == userId)
                .Include(z => z.userCart)
                .Include(z => z.userCart.biletInShoppingCarts)
                .Include("userCart.biletInShoppingCarts.bilet")
                .FirstOrDefaultAsync();

            var allProducts = loggedInUser.userCart.biletInShoppingCarts.ToList();
            int sum = 0;
            foreach (var item in allProducts)
            {
                sum += item.bilet.cena;
            }


            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (sum * 100),
                Description = "Kino Bileti Payment",
                Currency = "usd",
                Customer = customer.Id
            });
            if (charge.Status == "succeeded")
            {
                AddToOrder();
                return RedirectToAction("AddToOrder", "Order");
            }
            return RedirectToAction("AddToOrder", "Order");
        }
        public async Task<IActionResult> AddToOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = await _context.Users.Where(z => z.Id == userId)
                .Include(z => z.userCart)
                .Include(z => z.userCart.biletInShoppingCarts)
                .Include("userCart.biletInShoppingCarts.bilet")
                .FirstOrDefaultAsync();
            Models.Order item = new Models.Order { Id = Guid.NewGuid(), user = loggedInUser, userId = userId };
            _context.Add(item);
            await _context.SaveChangesAsync();
            List<BiletInOrder> InOrder = new List<BiletInOrder>();
            
            InOrder = loggedInUser.userCart.biletInShoppingCarts.Select(z=> new BiletInOrder {
                OrderId=item.Id,
                BiletId=z.bilet.Id,
                BIlet=z.bilet,
                Order=item}).ToList();
            foreach(var items in InOrder)
            {
                _context.Add(items);
                await _context.SaveChangesAsync();
            }
            loggedInUser.userCart.biletInShoppingCarts.Clear();
            _context.Update(loggedInUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "ShoppingCart");
        }
        public async Task<IActionResult> GetFaktura(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = await _context.Users.Where(z => z.Id == userId).FirstOrDefaultAsync();

           var order= await _context.Orders.Where(z => z.Id == id)
                .Include(z=>z.Bilets)
                .Include("Bilets.BIlet").FirstOrDefaultAsync();




            var pateka = Path.Combine(Directory.GetCurrentDirectory(), "FakturaKorisnik.docx");
            var document = DocumentModel.Load(pateka);
            document.Content.Replace("[broj]", id.ToString());
            document.Content.Replace("[Korisnik]", loggedInUser.ToString());
            int suma = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var item in order.Bilets)
            {
                suma += item.BIlet.cena;
                sb.AppendLine(item.BIlet.ime+" "+ item.BIlet.cena.ToString());
            }
            document.Content.Replace("[bileti]", sb.ToString());
            document.Content.Replace("[cena]", suma.ToString());
            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());


            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "Fakrura.pdf");
        }
    }
}
