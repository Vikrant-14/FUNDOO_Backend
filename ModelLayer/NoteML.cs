using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class NoteML
    {
        public string Title { get; set; }

        public string Description { get; set; }

        [DefaultValue(false)]
        public bool IsArchived { get; set; } 

        [DefaultValue(false)]
        public bool IsTrashed { get; set; } 
    }
}
