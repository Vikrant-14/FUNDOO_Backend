using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class NoteWithLabelDTO
    {
        public int NoteId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsTrashed { get; set; }
        public bool IsArchived { get; set; }
        public IList<LabelDTO> Labels { get; set; }
    }
}
