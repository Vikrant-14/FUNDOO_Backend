using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Entity
{
    [Index(nameof(CollaboratorEmail), nameof(NoteId), IsUnique = true)]
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CollaboratorEmail { get; set; } = string.Empty;
        public int NoteId { get; set; }
        public Note? Note { get; set; }       
    }
}
