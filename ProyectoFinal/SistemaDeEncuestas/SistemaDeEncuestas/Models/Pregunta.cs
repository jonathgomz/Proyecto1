using SistemaDeEncuestas.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaDeEncuestas.Models
{
    public class Pregunta
{
    public int Id { get; set; }

    [Required]
    public int EncuestaId { get; set; }
        
    [Required]
    public string Texto { get; set; }

    [Required]
    [StringLength(20)]
    public string TipoPregunta { get; set; }

    public int Orden { get; set; }

    public virtual Encuesta Encuesta { get; set; }
    public virtual ICollection<OpcionPregunta> Opciones { get; set; }

    public Pregunta()
    {
        TipoPregunta = "Unica";
        Orden = 1;
        Opciones = new List<OpcionPregunta>();
    }
}
}