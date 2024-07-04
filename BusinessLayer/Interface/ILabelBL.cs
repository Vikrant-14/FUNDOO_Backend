using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ILabelBL
    {
        public Label CreateLabel(LabelML model);
        public Label UpdateLabel(int id, LabelML model);
        public Label GetLabelById(int id);
        public IList<Label> GetAllLabels();
        public Label DeleteLabel(int id);
    }
}
