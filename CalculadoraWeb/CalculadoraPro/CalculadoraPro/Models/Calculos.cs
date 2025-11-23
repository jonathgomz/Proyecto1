using System;

namespace CalculadoraPro.Models
{
    public class Calculo
    {
        public int Id { get; set; }
        public string Operacion { get; set; }
        public decimal PrimerNumero { get; set; }
        public decimal SegundoNumero { get; set; }
        public decimal ResultadoFinal { get; set; }
        public DateTime FechaCalculo { get; set; }
    }
}