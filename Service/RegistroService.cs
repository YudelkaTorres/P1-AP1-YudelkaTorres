using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using P1_AP1_YudelkaTorres.DAL;
using P1_AP1_YudelkaTorres.Models;
public class RegistroService
{
    private readonly IDbContextFactory<Contexto> _dbFactory;

    public RegistroService(IDbContextFactory<Contexto> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Registro>> Listar (Expression<Func<Registro, bool>> criterio)
    {
       await using var contexto = await _dbFactory.CreateDbContextAsync();
        return await contexto.Registro
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}
