using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApartmentManagement.Data;

namespace ApartmentManagement.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly MvcInvoiceContext _context;

        public InvoiceController(MvcInvoiceContext context)
        {
            _context = context;
        }

        // GET: Invoice
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Invoice == null)
            {
                return Problem("Entity set 'MvcInvoiceContext.Invoice' is null.");
            }

            var invoices = from i in _context.Invoice
                           select i;

            if (!string.IsNullOrEmpty(searchString))
            {
                if (Int32.TryParse(searchString, out int result))
                {
                    if (result > 0 & result < 13)
                    {
                        invoices = invoices.Where(s => s.Session.Month == result && s.Session.Year == DateTime.Now.Year);
                    }
                }
                else
                {
                    var splitted = searchString.Split("/");
                    if (splitted.Length == 2)
                    {
                        if (Int32.TryParse(splitted[0], out int month) && Int32.TryParse(splitted[1], out int year))
                        {
                            if (month >= 1 && month <= 12)
                            {
                                invoices = invoices.Where(s => s.Session.Month == month && s.Session.Year == year);
                            }
                            else
                            {
                                return Problem("You should provide a valid month value between 1 and 12.");
                            }
                        }
                        else
                        {
                            return Problem("You should give valid numeric month and year values.");
                        }
                    }
                    else
                    {
                        return Problem("You should provide a valid month/year value.");
                    }
                }
            }

            return View(await invoices.ToListAsync());
        }


        // GET: Invoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Invoice == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoice/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Session,Residence,Amount,Description")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        // GET: Invoice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Invoice == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Session,Residence,Amount,Description")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            return View(invoice);
        }

        // GET: Invoice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Invoice == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Invoice == null)
            {
                return Problem("Entity set 'MvcInvoiceContext.Invoice'  is null.");
            }
            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoice.Remove(invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.Id == id);
        }
    }
}
