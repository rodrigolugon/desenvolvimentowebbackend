using Academia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Academia.Controllers {
    public class AlunoController : Controller {
        private readonly Context _context;

        public AlunoController(Context context) {
            _context = context;
        }

        // GET: Aluno
        public async Task<IActionResult> Index() {
            var context = _context.Alunos.Include(a => a.Personal);
            return View(await context.ToListAsync());
        }

        // Aluno vê apenas seus próprios treinos
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> MeuTreino() {
            var email = User.Identity!.Name;
            var aluno = await _context.Alunos
                .FirstOrDefaultAsync(a => a.E_Mail == email);

            if (aluno == null) return NotFound();

            var treinos = await _context.Treinos
                .Include(t => t.Personal)
                .Include(t => t.TreinoExercicios)
                    .ThenInclude(te => te.Exercicio)
                .Where(t => t.AlunoID == aluno.AlunoID)
                .ToListAsync();

            return View(treinos);
        }

        // Aluno vê o seu Personal
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> MeuPersonal() {
            var email = User.Identity!.Name;
            var aluno = await _context.Alunos
                .Include(a => a.Personal)
                .FirstOrDefaultAsync(a => a.E_Mail == email);

            if (aluno == null) return NotFound();
            return View(aluno.Personal);
        }

        // GET: Aluno/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var aluno = await _context.Alunos
                .Include(a => a.Personal)
                .FirstOrDefaultAsync(m => m.AlunoID == id);

            if (aluno == null) {
                return NotFound();
            }

            return View(aluno);
        }

        // GET: Aluno/Create
        public IActionResult Create() {
            ViewBag.Personals = new SelectList(_context.Personals, "PersonalID", "Nome");
            return View();
        }

        // POST: Aluno/Create
        [Authorize(Roles = "Personal")] // <-------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlunoID,Nome,Data_Nascimento,E_Mail,Instagram,Telefone,PersonalID,Observacoes")] Aluno aluno) {
            if (ModelState.IsValid) {
                _context.Add(aluno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Recarrega o dropdown se houver erro
            ViewBag.Personals = new SelectList(_context.Personals, "PersonalID", "Nome", aluno.PersonalID);
            return View(aluno);
        }

        // GET: Aluno/Edit/5
        [Authorize(Roles = "Personal")] // <-------------
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) {
                return NotFound();
            }

            ViewData["PersonalID"] = new SelectList(_context.Personals, "PersonalID", "Nome", aluno.PersonalID);
            return View(aluno);
        }

        // POST: Aluno/Edit/5
        [Authorize(Roles = "Personal")] // <-------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlunoID,Nome,Data_Nascimento,E_Mail,Instagram,Telefone,PersonalID,Observacoes")] Aluno aluno) {
            if (id != aluno.AlunoID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!AlunoExists(aluno.AlunoID)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["PersonalID"] = new SelectList(_context.Personals, "PersonalID", "Nome", aluno.PersonalID);
            return View(aluno);
        }

        // GET: Aluno/Delete/5
        [Authorize(Roles = "Personal")] // <-------------
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var aluno = await _context.Alunos
                .Include(a => a.Personal)
                .FirstOrDefaultAsync(m => m.AlunoID == id);

            if (aluno == null) {
                return NotFound();
            }

            return View(aluno);
        }

        // POST: Aluno/Delete/5
        [Authorize(Roles = "Personal")] // <-------------
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var aluno = await _context.Alunos.FindAsync(id);

            if (aluno != null) {
                _context.Alunos.Remove(aluno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Editar Meus Dados
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> EditarMeusDados() {
            var email = User.Identity!.Name;
            var aluno = await _context.Alunos
                .FirstOrDefaultAsync(a => a.E_Mail == email);

            if (aluno == null) return NotFound();
            return View(aluno);
        }

        // POST: Editar Meus Dados
        [HttpPost]
        [Authorize(Roles = "Aluno")]
        public async Task<IActionResult> EditarMeusDados([Bind("AlunoID,Nome,E_Mail,Instagram,Telefone,Observacoes,PersonalID")] Aluno aluno) {
            ModelState.Remove("Personal");
            ModelState.Remove("Data_Nascimento");

            if (ModelState.IsValid) {
                _context.Update(aluno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MeuTreino));
            }
            return View(aluno);
        }

        private bool AlunoExists(int id) {
            return _context.Alunos.Any(e => e.AlunoID == id);
        }
    }
}