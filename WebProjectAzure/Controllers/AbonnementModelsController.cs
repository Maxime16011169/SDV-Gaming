using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebProjectAzure.Data;
using WebProjectAzure.Models;

namespace WebProjectAzure.Controllers
{
    public class AbonnementModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AbonnementModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AbonnementModels
        //public async Task<IActionResult> Index()
        //{
        //    if (!_context.ListeAbonnement.Any())
        //    {
        //        return RedirectToAction("Create");
        //    }
        //    else
        //    {
        //        return _context.ListeAbonnement != null ?
        //                View(await _context.ListeAbonnement.ToListAsync()) :
        //                Problem("Entity set 'ApplicationDbContext.ListeAbonnement'  is null.");
        //    }
        //}

        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["MailSortParm"] = String.IsNullOrEmpty(sortOrder) ? "mail_desc" : "";
            var abonnement = from a in _context.ListeAbonnement
                           select a;
            switch (sortOrder)
            {
                case "mail_desc":
                    abonnement = abonnement.OrderByDescending(a => a.Mail);
                    break;
            }
            return View(await abonnement.AsNoTracking().ToListAsync());
        }

        // GET: AbonnementModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ListeAbonnement == null)
            {
                return NotFound();
            }

            var abonnementModel = await _context.ListeAbonnement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (abonnementModel == null)
            {
                return NotFound();
            }

            return View(abonnementModel);
        }

        // GET: AbonnementModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AbonnementModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateDebut,Duree,TarifMensuel,Mail")] AbonnementModel abonnementModel)
        {
            abonnementModel.Mail = HttpContext.User.Identity.Name;
            _context.Add(abonnementModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: AbonnementModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ListeAbonnement == null)
            {
                return NotFound();
            }

            var abonnementModel = await _context.ListeAbonnement.FindAsync(id);
            if (abonnementModel == null)
            {
                return NotFound();
            }
            return View(abonnementModel);
        }

        // POST: AbonnementModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateDebut,Duree,TarifMensuel,Mail")] AbonnementModel abonnementModel)
        {
            if (id != abonnementModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(abonnementModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbonnementModelExists(abonnementModel.Id))
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
            return View(abonnementModel);
        }

        // GET: AbonnementModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ListeAbonnement == null)
            {
                return NotFound();
            }

            var abonnementModel = await _context.ListeAbonnement
                .FirstOrDefaultAsync(m => m.Id == id);
            if (abonnementModel == null)
            {
                return NotFound();
            }

            return View(abonnementModel);
        }

        // POST: AbonnementModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ListeAbonnement == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ListeAbonnement'  is null.");
            }
            var abonnementModel = await _context.ListeAbonnement.FindAsync(id);
            if (abonnementModel != null)
            {
                _context.ListeAbonnement.Remove(abonnementModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbonnementModelExists(int id)
        {
            return (_context.ListeAbonnement?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
