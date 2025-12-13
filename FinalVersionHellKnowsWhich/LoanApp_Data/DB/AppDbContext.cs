using Microsoft.EntityFrameworkCore;
namespace FinalVersionHellKnowsWhich.LoanApp_Data.DB;
using FinalVersionHellKnowsWhich.LoanApp_Data.Entities;
using Microsoft.Identity.Client;
using System.Runtime.Intrinsics.Arm;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    
        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
   
    

}

