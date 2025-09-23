using P1_AP1_YudelkaTorres.Models;
using Microsoft.EntityFrameworkCore;

namespace P1_AP1_YudelkaTorres.DAL;
public class Contexto : DbContext
{
	public Contexto(DbContextOptions<Contexto> options) : base(options) { }
	public DbSet<Registro> Registro { get; set; }
}
