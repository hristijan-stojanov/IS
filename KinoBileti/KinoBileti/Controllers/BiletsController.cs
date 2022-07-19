using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinoBileti.Data;
using KinoBileti.Models;
using System.Security.Claims;
using ClosedXML.Excel;
using System.IO;

namespace KinoBileti.Controllers
{
    public class BiletsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BiletsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bilets
        public async Task<IActionResult> Index(DateTime? datum)
        {
            if (datum != null)
            {
                var data = await _context.Bilets.Where(z => z.datum > datum).ToListAsync();
                return View(data);
            }
            return View(await _context.Bilets.ToListAsync());
        }

        // GET: Bilets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilet = await _context.Bilets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bilet == null)
            {
                return NotFound();
            }

            return View(bilet);
        }

        // GET: Bilets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bilets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ime,datum,cena,zanr")] Bilet bilet)
        {
            if (ModelState.IsValid)
            {
                bilet.Id = Guid.NewGuid();
                _context.Add(bilet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bilet);
        }

        // GET: Bilets/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilet = await _context.Bilets.FindAsync(id);
            if (bilet == null)
            {
                return NotFound();
            }
            return View(bilet);
        }

        // POST: Bilets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ime,datum,cena,zanr")] Bilet bilet)
        {
            if (id != bilet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bilet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BiletExists(bilet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bilet);
        }

        // GET: Bilets/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bilet = await _context.Bilets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bilet == null)
            {
                return NotFound();
            }

            return View(bilet);
        }

        // POST: Bilets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var bilet = await _context.Bilets.FindAsync(id);
            _context.Bilets.Remove(bilet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BiletExists(Guid id)
        {
            return _context.Bilets.Any(e => e.Id == id);
        }
        public async Task<IActionResult> AddProductToCard(Guid id)
        {
            if (id != null)
            {
                var bilet = await _context.Bilets.Where(z => z.Id == id).FirstOrDefaultAsync();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var shoppingCart1 = await _context.ShoppingCards.Where(z => z.ownerId == userId).FirstOrDefaultAsync();
                BiletInShoppingCart biletInShoppingCart = new BiletInShoppingCart
                {
                    BiletId = bilet.Id,
                    ShoppingCartId = shoppingCart1.Id,
                    shoppingCart = shoppingCart1,
                    bilet = bilet

                };
                _context.Add(biletInShoppingCart);
                _context.SaveChanges();
                return RedirectToAction("Index", "Bilets");
            }
            return View();
        }




        public async Task<FileContentResult> ExportBilets(String? zanr)
        {
            var data= new List<Bilet>();
            if (zanr != null)
            {
                 data = await _context.Bilets.Where(z=>z.zanr.Equals(zanr)).ToListAsync();
            }
            else
            {
                 data = await _context.Bilets.ToListAsync();
            }
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workBook.Worksheets.Add("Bilets");

                worksheet.Cell(1, 1).Value = "Bilet Id";
                worksheet.Cell(1, 2).Value = "Ime";
                worksheet.Cell(1, 3).Value = "Datum i Vreme";
                worksheet.Cell(1, 4).Value = "Zanr";
                worksheet.Cell(1, 5).Value = "Cena";

                for (int i = 0; i < data.Count(); i++)
                {
                    var item = data[i];

                    worksheet.Cell(i + 2, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 2, 2).Value = item.ime;
                    worksheet.Cell(i + 2, 3).Value = item.datum.ToString();
                    worksheet.Cell(i + 2, 4).Value = item.zanr;
                    worksheet.Cell(i + 2, 5).Value = item.cena;



                }

                using (var stream = new MemoryStream())
                {
                    workBook.SaveAs(stream);

                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }



        }
    }
}
