using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using P1_AP1_YudelkaTorres.DAL;
using P1_AP1_YudelkaTorres.Models;

namespace P1_AP1_YudelkaTorres.Service;
public class EntradasHuacalesServices(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<List<EntradasHuacales>> Listar (Expression<Func<EntradasHuacales, bool>> criterio)
    {
       await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.EntradasHuacales
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
     public async Task<bool> Guardar(EntradasHuacales entrada)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();

            bool existe = await Existe(entrada.EntradaId);

            if (!existe)
                contexto.EntradasHuacales.Add(entrada);
            else
                contexto.EntradasHuacales.Update(entrada);

            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Existe(int entradaId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.EntradasHuacales
                .AnyAsync(e => e.EntradaId == entradaId);
        }

        public async Task<EntradasHuacales?> Buscar(int entradaId)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.EntradasHuacales
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
        }
        private async Task<bool> Insertar(EntradasHuacales entrada)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.EntradasHuacales.Add(entrada);
            return await contexto.SaveChangesAsync() > 0;
        }   

        private async Task<bool> Modificar(EntradasHuacales entrada)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            contexto.EntradasHuacales.Update(entrada);
            return await contexto.SaveChangesAsync() > 0;
        }

        public async Task<bool> Eliminar(int idEntrada)
        {
            await using var contexto = await DbFactory.CreateDbContextAsync();
            return await contexto.EntradasHuacales
                .Where(e => e.EntradaId == idEntrada)
                .ExecuteDeleteAsync() > 0;
        }
}

