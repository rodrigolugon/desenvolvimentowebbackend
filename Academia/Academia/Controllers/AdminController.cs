using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Academia.Models;

namespace Academia.Controllers {
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller {
        private readonly Context _context;

        public AdminController(Context context) {
            _context = context;
        }

        // Gerenciar Alunos
        public async Task<IActionResult> GerenciarAlunos() {
            var alunos = await _context.Alunos.Include(a => a.Personal).ToListAsync();
            return View(alunos);
        }

        // Gerenciar Personais
        public async Task<IActionResult> GerenciarPersonais() {
            var personais = await _context.Personals.ToListAsync();
            return View(personais);
        }

        // Gerenciar Exercícios
        public async Task<IActionResult> GerenciarExercicios() {
            var exercicios = await _context.Exercicios.ToListAsync();
            return View(exercicios);
        }

        // Deletar Aluno
        [HttpPost]
        public async Task<IActionResult> DeletarAluno(int id) {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno != null) _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GerenciarAlunos));
        }

        // Deletar Personal
        [HttpPost]
        public async Task<IActionResult> DeletarPersonal(int id) {
            var personal = await _context.Personals.FindAsync(id);
            if (personal != null) _context.Personals.Remove(personal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GerenciarPersonais));
        }

        // Deletar Exercício
        [HttpPost]
        public async Task<IActionResult> DeletarExercicio(int id) {
            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio != null) _context.Exercicios.Remove(exercicio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(GerenciarExercicios));
        }
    }
}