using Academia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Academia.Controllers {
    public class AccountController : Controller {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly Context _context;

        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, Context context) {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // GET: RegisterPersonal
        public IActionResult RegisterPersonal() => View();

        // POST: RegisterPersonal
        [HttpPost]
        public async Task<IActionResult> RegisterPersonal(string nome, string especialidade, string email, string senha) {
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, senha);

            if (result.Succeeded) {
                await _userManager.AddToRoleAsync(user, "Personal");
                _context.Personals.Add(new Personal { Nome = nome, Especialidade = especialidade });
                await _context.SaveChangesAsync();
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View();
        }

        // GET: RegisterAluno
        public IActionResult RegisterAluno() => View();

        // POST: RegisterAluno
        [HttpPost]
        public async Task<IActionResult> RegisterAluno(string nome, string email, string senha, int personalId) {
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, senha);

            if (result.Succeeded) {
                await _userManager.AddToRoleAsync(user, "Aluno");
                _context.Alunos.Add(new Aluno { Nome = nome, E_Mail = email, PersonalID = personalId });
                await _context.SaveChangesAsync();
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            ViewBag.Personals = _context.Personals.ToList();
            return View();
        }
    }
}