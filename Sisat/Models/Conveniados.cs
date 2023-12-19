using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sisat.Models
{
    public partial class Conveniados
    {
        public Conveniados()
        {
            ConvenioProjeto = new HashSet<ConvenioProjeto>();
        }

        [Key]
        [Column("Id_Conveniado")]
        public int IdConveniado { get; set; }
        [Column("Nom_Conveniado")]
        [StringLength(100)]
        public string NomConveniado { get; set; } = null!;

        [InverseProperty("IdConNavigation")]
        public virtual ICollection<ConvenioProjeto> ConvenioProjeto { get; set; }
    }
}
