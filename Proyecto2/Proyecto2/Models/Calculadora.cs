using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Proyecto2.Models
{
    public class Calculadora : DbContext
    {
        public Calculadora() : base("name=DefaultConnection")
        {
        }

        public DbSet<Calculos> Calculos { get; set; }
    }
}