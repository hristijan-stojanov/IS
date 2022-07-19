using KinoBileti.Data;
using KinoBileti.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KinoBileti.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = await _context.Users.Where(z => z.Id == userId)
                .Include(z => z.userCart)
                .Include(z=>z.userCart.biletInShoppingCarts)
                .Include("userCart.biletInShoppingCarts.bilet")
                .FirstOrDefaultAsync();

            var allProducts = loggedInUser.userCart.biletInShoppingCarts.ToList();
            int sum = 0;
            foreach(var item in allProducts)
            {
                sum += item.bilet.cena;
            }
            ShoppingDto shopping = new ShoppingDto { BiletInShoppingCarts = allProducts, cena = sum };

            return View(shopping);
        }
        [Authorize]
        public async Task<IActionResult> DeleteFromShoppingCart(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var loggedInUser = await _context.Users.Where(z => z.Id == userId)
                .Include(z => z.userCart)
                .Include(z => z.userCart.biletInShoppingCarts)
                .Include("userCart.biletInShoppingCarts.bilet")
                .FirstOrDefaultAsync();

            var item = loggedInUser.userCart.biletInShoppingCarts.Where(z => z.BiletId.Equals(id)).FirstOrDefault(); ;


            loggedInUser.userCart.biletInShoppingCarts.Remove(item);

                _context.Update(loggedInUser.userCart);
                await _context.SaveChangesAsync();
            

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
