﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using P1_AP1_YudelkaTorres.DAL;
using P1_AP1_YudelkaTorres.Models;

namespace P1_AP1_YudelkaTorres.Services;
public class EntradasHuacalesService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<List<EntradasHuacales>> Listar(Expression<Func<EntradasHuacales, bool>> criterio)
    {
       await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.EntradasHuacales
            .Include(e => e.EntradasHuacalesDetalle)
            .ThenInclude(d => d.EntradaHuacalTipo)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<bool> Guardar(EntradasHuacales entrada)
    {
        if (!await Existe(entrada.EntradaId))
            return await Insertar(entrada);
        else
            return await Modificar(entrada);
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
            .Include(e => e.EntradasHuacalesDetalle)
            .ThenInclude(d => d.EntradaHuacalTipo)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);
    }
    private async Task AfectarExistencia(EntradasHuacales entrada, TipoOperacion tipoOperacion)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var entradashuacalesdetalle = await contexto.EntradasHuacalesDetalle
            .Where(d => d.EntradaId == entrada.EntradaId)
            .ToListAsync();

        foreach (var entradahuacaldetalle in entradashuacalesdetalle)
        {
            var entradashuacalestipo = await contexto.EntradasHuacalesTipos
                .SingleOrDefaultAsync(t => t.TipoId == entradahuacaldetalle.TipoId);

            if (entradashuacalestipo == null)
                continue;

            if (tipoOperacion == TipoOperacion.Suma)
                entradashuacalestipo.Existencia += entradahuacaldetalle.Cantidad;
            else
                entradashuacalestipo.Existencia -= entradahuacaldetalle.Cantidad;
        }
        await contexto.SaveChangesAsync();
    }
    private async Task<bool> Insertar(EntradasHuacales entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.EntradasHuacales.Add(entrada);
        await contexto.SaveChangesAsync();

        await AfectarExistencia(entrada, TipoOperacion.Suma);
        return true;
    }   
    private async Task<bool> Modificar(EntradasHuacales entrada)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var anterior = await contexto.EntradasHuacales
            .Include(e => e.EntradasHuacalesDetalle)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.EntradaId == entrada.EntradaId);

        if (anterior != null)
            await AfectarExistencia(anterior, TipoOperacion.Resta);

        contexto.Update(entrada);
        await contexto.SaveChangesAsync();

        await AfectarExistencia(entrada, TipoOperacion.Suma);
        return true;
    }
    public async Task<bool> Eliminar(int entradaId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var entrada = await contexto.EntradasHuacales
            .Include(e => e.EntradasHuacalesDetalle)
            .FirstOrDefaultAsync(e => e.EntradaId == entradaId);

        if (entrada == null)
            return false;

        await AfectarExistencia(entrada, TipoOperacion.Resta);

        contexto.EntradasHuacalesDetalle.RemoveRange(entrada.EntradasHuacalesDetalle);
        contexto.EntradasHuacales.Remove(entrada);

        await contexto.SaveChangesAsync();
        return true;
    }
    public async Task<List<EntradasHuacalesTipos>> ListarTipos()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.EntradasHuacalesTipos
            .AsNoTracking()
            .ToListAsync();
    }
}
public enum TipoOperacion
{
    Suma = 1,
    Resta = 2
}