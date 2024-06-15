using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class Label
    {
        public int Id { get; set; } 
        public string LabelName { get; set; }

        [JsonIgnore]
        public ICollection<NoteLabelEntity> NoteLabel { get; set; }
    }
}
