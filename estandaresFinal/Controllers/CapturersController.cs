using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using estandaresFinal.Data;
using estandaresFinal.Data.Entities;

namespace estandaresFinal.Controllers
{
    public class CapturersController : Controller
    {
        private readonly DataContext _context;

        public CapturersController(DataContext context)
        {
            _context = context;
        }

        // GET: Capturers
        public async Task<IActionResult> Index()
        {
              return _context.Capturers != null ? 
                          View(await _context.Capturers.ToListAsync()) :
                          Problem("Entity set 'DataContext.Capturers'  is null.");
        }

        // GET: Capturers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Capturers == null)
            {
                return NotFound();
            }

            var capturer = await _context.Capturers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (capturer == null)
            {
                return NotFound();
            }

            return View(capturer);
        }

        // GET: Capturers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Capturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Capturer capturer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(capturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(capturer);
        }

        // GET: Capturers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Capturers == null)
            {
                return NotFound();
            }

            var capturer = await _context.Capturers.FindAsync(id);
            if (capturer == null)
            {
                return NotFound();
            }
            return View(capturer);
        }

        // POST: Capturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Capturer capturer)
        {
            if (id != capturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(capturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CapturerExists(capturer.Id))
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
            return View(capturer);
        }

        // GET: Capturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Capturers == null)
            {
                return NotFound();
            }

            var capturer = await _context.Capturers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (capturer == null)
            {
                return NotFound();
            }

            return View(capturer);
        }

        // POST: Capturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Capturers == null)
            {
                return Problem("Entity set 'DataContext.Capturers'  is null.");
            }
            var capturer = await _context.Capturers.FindAsync(id);
            if (capturer != null)
            {
                _context.Capturers.Remove(capturer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CapturerExists(int id)
        {
          return (_context.Capturers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
