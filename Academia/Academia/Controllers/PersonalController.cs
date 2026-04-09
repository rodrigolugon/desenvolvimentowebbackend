using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Academia.Models;
using Microsoft.AspNetCore.Authorization; // ALTERAÇÃO 1: Importar para segurança

namespace Academia.Controllers {
    public class PersonalController : Controller {
        private readonly Context _context;

        public PersonalController(Context context) {
            _context = context;
        }

        // GET: Personal
        public async Task<IActionResult> Index() {
            return View(await _context.Personals.ToListAsync());
        }

        // Personal vê apenas seus próprios alunos
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> MeusAlunos() {
            var email = User.Identity!.Name;
            var personal = await _context.Personals.FirstOrDefaultAsync(p => p.Nome == email);
            if (personal == null) return NotFound();

            var alunos = await _context.Alunos
                .Where(a => a.PersonalID == personal.PersonalID)
                .ToListAsync();

            return View(alunos);
        }

        // GET: Personal/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) return NotFound();

            var personal = await _context.Personals
                .FirstOrDefaultAsync(m => m.PersonalID == id);

            if (personal == null) return NotFound();

            return View(personal);
        }

        // GET: Personal/Create
        [Authorize(Roles = "Personal")]
        public IActionResult Create() {
            return View();
        }

        // POST: Personal/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Create([Bind("PersonalID,Nome,Especialidade")] Personal personal) {
            // ALTERAÇÃO 2: Limpamos as validações de listas (Alunos e Treinos)
            // Isso impede que o ModelState fique 'false' por causa de campos que não estão no formulário
            ModelState.Remove("Alunos");
            ModelState.Remove("Treinos");

            if (ModelState.IsValid) {
                _context.Add(personal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personal);
        }

        // GET: Personal/Edit/5
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return NotFound();

            var personal = await _context.Personals.FindAsync(id);
            if (personal == null) return NotFound();

            return View(personal);
        }

        // POST: Personal/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Edit(int id, [Bind("PersonalID,Nome,Especialidade")] Personal personal) {
            if (id != personal.PersonalID) return NotFound();

            // ALTERAÇÃO 3: Mesma limpeza aqui para o Edit funcionar sempre
            ModelState.Remove("Alunos");
            ModelState.Remove("Treinos");

            if (ModelState.IsValid) {
                try {
                    _context.Update(personal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!PersonalExists(personal.PersonalID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(personal);
        }

        // GET: Personal/Delete/5
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return NotFound();

            var personal = await _context.Personals
                .FirstOrDefaultAsync(m => m.PersonalID == id);

            if (personal == null) return NotFound();

            return View(personal);
        }

        // POST: Personal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var personal = await _context.Personals.FindAsync(id);
            if (personal != null) _context.Personals.Remove(personal);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalExists(int id) {
            return _context.Personals.Any(e => e.PersonalID == id);
        }
    }
}