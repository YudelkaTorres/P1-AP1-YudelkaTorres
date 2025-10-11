using P1_AP1_YudelkaTorres.Components;
using P1_AP1_YudelkaTorres.DAL;
using Microsoft.EntityFrameworkCore;
using P1_AP1_YudelkaTorres.Services;

namespace P1_AP1_YudelkaTorres;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        var ConnectionString = builder.Configuration.GetConnectionString("SqlConstr");

        builder.Services.AddDbContextFactory<Contexto>(options => options.UseSqlite(ConnectionString));

        builder.Services.AddScoped<EntradasHuacalesService>();

        //Inyeccion del Bootstrap
        builder.Services.AddBlazorBootstrap();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
