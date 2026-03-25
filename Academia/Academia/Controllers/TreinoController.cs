using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Academia.Models;
using Microsoft.AspNetCore.Authorization; // 1. ADICIONE ESTA LINHA

namespace Academia.Controllers {
    // Adicionando [Authorize] aqui, todas as páginas de treino exigirão login
    //[Authorize]
    public class TreinoController : Controller {
        private readonly Context _context;

        public TreinoController(Context context) {
            _context = context;
        }

        // GET: Treino
        // Já está protegido pelo [Authorize] da classe
        public async Task<IActionResult> Index() {
            var context = _context.Treinos.Include(t => t.Aluno).Include(t => t.Personal);
            return View(await context.ToListAsync());
        }

        // GET: Treino/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) return NotFound();

            var treino = await _context.Treinos
                .Include(t => t.Aluno)
                .Include(t => t.Personal)
                .FirstOrDefaultAsync(m => m.TreinoID == id);

            if (treino == null) return NotFound();

            return View(treino);
        }

        // GET: Treino/Create
        //  [Authorize(Roles = "Personal")] // 2. SOMENTE PERSONAL ACESSA
        // GET: Treino/Create
        public IActionResult Create() {
            ViewBag.Personals = _context.Personals.ToList();
            ViewBag.Alunos = _context.Alunos.ToList();
            ViewBag.Exercicios = _context.Exercicios.ToList();
            return View();
        }

        // POST: Treino/Create
        // POST: Treino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Removi o Authorize temporariamente caso você não tenha login pronto, 
        // mas pode manter se já tiver usuários configurados.
        public async Task<IActionResult> Create([Bind("TreinoID,PersonalID,AlunoID,Data,Hora")] Treino treino, int[] exerciciosIds) {
            // 1. Limpa as validações que o .NET 9 cria para objetos de navegação
            ModelState.Remove("Personal");
            ModelState.Remove("Aluno");
            ModelState.Remove("TreinoExercicios");

            if (ModelState.IsValid) {
                // 2. Salva o Treino primeiro
                _context.Add(treino);
                await _context.SaveChangesAsync();

                // 3. Salva os Exercícios (Limitando a 4 no servidor por segurança)
                if (exerciciosIds != null) {
                    // O .Take(4) ignora qualquer coisa acima de 4
                    foreach (var exercicioId in exerciciosIds.Take(4)) {
                        var link = new TreinoExercicio {
                            TreinoID = treino.TreinoID,
                            ExercicioID = exercicioId
                        };
                        _context.TreinoExercicios.Add(link);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            // Se der erro, recarrega as listas para a View
            ViewBag.Personals = _context.Personals.ToList();
            ViewBag.Alunos = _context.Alunos.ToList();
            ViewBag.Exercicios = _context.Exercicios.ToList();
            return View(treino);
        }

        // GET: Treino/Edit/5
        [Authorize(Roles = "Personal")] // 4. SOMENTE PERSONAL EDITA
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return NotFound();

            var treino = await _context.Treinos.FindAsync(id);
            if (treino == null) return NotFound();

            ViewData["AlunoID"] = new SelectList(_context.Alunos, "AlunoID", "Nome", treino.AlunoID);
            ViewData["PersonalID"] = new SelectList(_context.Personals, "PersonalID", "Nome", treino.PersonalID);
            return View(treino);
        }

        // POST: Treino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Edit(int id, [Bind("TreinoID,PersonalID,AlunoID,Data,Hora")] Treino treino) {
            if (id != treino.TreinoID) return NotFound();

            if (ModelState.IsValid) {
                try {
                    _context.Update(treino);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!TreinoExists(treino.TreinoID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(treino);
        }

        // GET: Treino/Delete/5
        [Authorize(Roles = "Personal")] // 5. RECOMENDADO: PROTEGE A EXCLUSÃO TAMBÉM
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) return NotFound();

            var treino = await _context.Treinos
                .Include(t => t.Aluno)
                .Include(t => t.Personal)
                .FirstOrDefaultAsync(m => m.TreinoID == id);

            if (treino == null) return NotFound();

            return View(treino);
        }

        // POST: Treino/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var treino = await _context.Treinos.FindAsync(id);
            if (treino != null) _context.Treinos.Remove(treino);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreinoExists(int id) {
            return _context.Treinos.Any(e => e.TreinoID == id);
        }
    }
}