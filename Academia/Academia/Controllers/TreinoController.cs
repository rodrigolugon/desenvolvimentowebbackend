using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Academia.Models;
using Microsoft.AspNetCore.Authorization;

namespace Academia.Controllers {
    public class TreinoController : Controller {
        private readonly Context _context;

        public TreinoController(Context context) {
            _context = context;
        }

        // GET: Treino
        public async Task<IActionResult> Index() {
            var treinos = _context.Treinos
                .Include(t => t.Aluno)
                .Include(t => t.Personal);
            return View(await treinos.ToListAsync());
        }

        // GET: Treino/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) return NotFound();

            var treino = await _context.Treinos
                .Include(t => t.Aluno)
                .Include(t => t.Personal)
                .Include(t => t.TreinoExercicios)
                    .ThenInclude(te => te.Exercicio)
                .FirstOrDefaultAsync(m => m.TreinoID == id);

            if (treino == null) return NotFound();

            return View(treino);
        }

        // GET: Treino/Create
        [Authorize(Roles = "Personal")]
        public IActionResult Create() {
            ViewBag.Personals = _context.Personals.ToList();
            ViewBag.Alunos = _context.Alunos.ToList();
            ViewBag.Exercicios = _context.Exercicios.ToList();
            return View();
        }

        // POST: Treino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Create([Bind("TreinoID,PersonalID,AlunoID,Data,Hora")] Treino treino, int[] exerciciosIds) {
            ModelState.Remove("Personal");
            ModelState.Remove("Aluno");
            ModelState.Remove("TreinoExercicios");

            if (ModelState.IsValid) {
                _context.Add(treino);
                await _context.SaveChangesAsync();

                if (exerciciosIds != null) {
                    foreach (var exercicioId in exerciciosIds.Take(4)) {
                        _context.TreinoExercicios.Add(new TreinoExercicio {
                            TreinoID = treino.TreinoID,
                            ExercicioID = exercicioId
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Personals = _context.Personals.ToList();
            ViewBag.Alunos = _context.Alunos.ToList();
            ViewBag.Exercicios = _context.Exercicios.ToList();
            return View(treino);
        }

        // GET: Treino/Edit/5
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) return NotFound();

            var treino = await _context.Treinos
                .Include(t => t.TreinoExercicios)
                .FirstOrDefaultAsync(t => t.TreinoID == id);

            if (treino == null) return NotFound();

            ViewBag.Personals = new SelectList(_context.Personals, "PersonalID", "Nome", treino.PersonalID);
            ViewBag.Alunos = new SelectList(_context.Alunos, "AlunoID", "Nome", treino.AlunoID);
            ViewBag.Exercicios = _context.Exercicios.ToList();

            return View(treino);
        }

        // POST: Treino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Edit(int id, [Bind("TreinoID,PersonalID,AlunoID,Data,Hora")] Treino treino, int[] exerciciosSelecionados) {
            if (id != treino.TreinoID) return NotFound();

            ModelState.Remove("Personal");
            ModelState.Remove("Aluno");
            ModelState.Remove("TreinoExercicios");

            if (ModelState.IsValid) {
                try {
                    _context.Update(treino);

                    var exerciciosAntigos = _context.TreinoExercicios
                        .Where(te => te.TreinoID == id);
                    _context.TreinoExercicios.RemoveRange(exerciciosAntigos);

                    if (exerciciosSelecionados != null) {
                        foreach (var exercicioId in exerciciosSelecionados.Take(4)) {
                            _context.TreinoExercicios.Add(new TreinoExercicio {
                                TreinoID = treino.TreinoID,
                                ExercicioID = exercicioId
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!TreinoExists(treino.TreinoID)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Personals = new SelectList(_context.Personals, "PersonalID", "Nome", treino.PersonalID);
            ViewBag.Alunos = new SelectList(_context.Alunos, "AlunoID", "Nome", treino.AlunoID);
            ViewBag.Exercicios = _context.Exercicios.ToList();
            return View(treino);
        }

        // GET: Treino/Delete/5
        [Authorize(Roles = "Personal")]
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
            var treino = await _context.Treinos
                .Include(t => t.TreinoExercicios)
                .FirstOrDefaultAsync(t => t.TreinoID == id);

            if (treino != null) {
                _context.TreinoExercicios.RemoveRange(treino.TreinoExercicios);
                _context.Treinos.Remove(treino);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreinoExists(int id) {
            return _context.Treinos.Any(e => e.TreinoID == id);
        }
    }
}