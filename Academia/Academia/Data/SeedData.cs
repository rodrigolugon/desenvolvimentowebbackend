using Microsoft.AspNetCore.Identity;

namespace Academia.Data {
    public static class SeedData {
        public static async Task CriarRoles(IServiceProvider serviceProvider) {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Personal", "Aluno" };

            foreach (var role in roles) {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}