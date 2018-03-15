using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using TrainingSystem.Models;

namespace TrainingSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TrainingSystemContext>();
            context.Database.EnsureCreated();

            // Inicia perfis customizáveis 
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            string[] roleNames = { "Admin", "Employee" };

            foreach (var roleName in roleNames)
            {
                var roleExist = roleManager.RoleExistsAsync(roleName).Result;

                // Se o perfil não existir, ele é criado e adicionado no banco
                if (!roleExist)
                {
                    roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
                }
            }

            var user = userManager.FindByEmailAsync("admin@email.com").Result;

            // Se o admin não existir, cria um usuário com tais poderes
            if (user == null)
            {
                var poweruser = new AppUser
                {
                    UserName = "Admin",
                    Email = "admin@email.com",
                };
                string adminPassword = "Admin!23";

                var createPowerUser = userManager.CreateAsync(poweruser, adminPassword).Result;
                if (createPowerUser.Succeeded)
                {
                    userManager.AddToRoleAsync(poweruser, "Admin").Wait();
                }
            }
        }
    }
}
