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
    public class CoordinatorsController : Controller
    {
        private readonly DataContext _context;

        public CoordinatorsController(DataContext context)
        {
            _context = context;
        }

        // GET: Coordinators
        public async Task<IActionResult> Index()
        {
              return _context.Coordinators != null ? 
                          View(await _context.Coordinators.ToListAsync()) :
                          Problem("Entity set 'DataContext.Coordinators'  is null.");
        }

        // GET: Coordinators/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Coordinators == null)
            {
                return NotFound();
            }

            var coordinator = await _context.Coordinators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coordinator == null)
            {
                return NotFound();
            }

            return View(coordinator);
        }

        // GET: Coordinators/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coordinators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Coordinator coordinator)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coordinator);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coordinator);
        }

        // GET: Coordinators/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Coordinators == null)
            {
                return NotFound();
            }

            var coordinator = await _context.Coordinators.FindAsync(id);
            if (coordinator == null)
            {
                return NotFound();
            }
            return View(coordinator);
        }

        // POST: Coordinators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Coordinator coordinator)
        {
            if (id != coordinator.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coordinator);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoordinatorExists(coordinator.Id))
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
            return View(coordinator);
        }

        // GET: Coordinators/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Coordinators == null)
            {
                return NotFound();
            }

            var coordinator = await _context.Coordinators
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coordinator == null)
            {
                return NotFound();
            }

            return View(coordinator);
        }

        // POST: Coordinators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Coordinators == null)
            {
                return Problem("Entity set 'DataContext.Coordinators'  is null.");
            }
            var coordinator = await _context.Coordinators.FindAsync(id);
            if (coordinator != null)
            {
                _context.Coordinators.Remove(coordinator);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoordinatorExists(int id)
        {
          return (_context.Coordinators?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
