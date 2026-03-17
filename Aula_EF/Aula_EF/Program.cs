using Aula_EF.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options => {
   // options.ValidateScopes = false; // Ativa a validacao de escopo. False evita erro de BD inexistente
});

// Add services to the container.
builder.Services.AddControllersWithViews();

//ConfiguraCAOo da Entity Framework Core
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration["Data:Exemplo_EF_BD26:ConnectionString"],

    //evita que o BD n?o seja criado por problemas de timeout com o servidor
    sqlServerOptionsAction: sqlOptions => {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);
    }));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//SeedData.Initialize(app.Services);
SeedData.EnsurePopulated(app);

app.Run();