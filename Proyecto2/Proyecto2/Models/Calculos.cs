using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto2.Models
{
    [Table("HistorialCalculos")]
    public class Calculos
    {
        [Key]
        public int Id { get; set; }
        public string Operacion { get; set; } 
        public double Resultado { get; set; }
        public DateTime FechaHora { get; set; }

        public Calculos() { }
    }
}