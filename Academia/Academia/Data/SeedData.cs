using Microsoft.AspNetCore.Identity;

namespace Academia.Models {
    public static class SeedData {
        public static async Task InicializarAsync(IServiceProvider serviceProvider) {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Cria as roles
            string[] roles = { "Personal", "Aluno", "Admin" };
            foreach (var role in roles) {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Cria o Admin padrão
            var adminEmail = "admin@academia.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null) {
                var novoAdmin = new IdentityUser {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var resultado = await userManager.CreateAsync(novoAdmin, "Admin@123");
                if (resultado.Succeeded)
                    await userManager.AddToRoleAsync(novoAdmin, "Admin");
            }
        }
    }
}