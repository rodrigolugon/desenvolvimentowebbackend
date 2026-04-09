using Academia.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Academia.Areas.Identity.Pages.Account {
    public class RegisterAlunoModel : PageModel {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly Context _context;

        public RegisterAlunoModel(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, Context context) {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel {
            [Required(ErrorMessage = "Nome é obrigatório")]
            public string Nome { get; set; }

            [Required(ErrorMessage = "Data de nascimento é obrigatória")]
            public DateTime DataNascimento { get; set; }

            public string? Instagram { get; set; }
            public string? Telefone { get; set; }

            [Required, EmailAddress]
            public string Email { get; set; }

            [Required, DataType(DataType.Password)]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "As senhas não coincidem")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) return Page();

            var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded) {
                await _userManager.AddToRoleAsync(user, "Aluno");

                var aluno = new Aluno {
                    Nome = Input.Nome,
                    Data_Nascimento = Input.DataNascimento,
                    E_Mail = Input.Email,
                    Instagram = Input.Instagram,
                    Telefone = Input.Telefone
                };
                _context.Alunos.Add(aluno);
                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
    }
}