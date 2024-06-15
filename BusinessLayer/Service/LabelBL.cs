using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.CustomExecption;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class LabelBL : ILabelBL
    {
        private readonly ILabelRL labelRL;

        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL; 
        }

        public Label CreateLabel(LabelML model)
        {
            try
            {
                return labelRL.CreateLabel(model);
            }
            catch (LabelException)
            {
                throw;
            }
        }

        public Label DeleteLabel(int id)
        {
            try
            {
                return labelRL.DeleteLabel(id);
            }
            catch (LabelException)
            {
                throw;
            }
        }

        public IList<Label> GetAllLabels()
        {
            try
            {
                return labelRL.GetAllLabels();
            }
            catch (LabelException)
            {
                throw;
            }
        }

        public Label GetLabelById(int id)
        {
            try
            {
                return labelRL.GetLabelById(id);
            }
            catch (LabelException)
            {
                throw;
            }
        }

        public Label UpdateLabel(string labelName, LabelML model)
        {
            try
            {
                return labelRL.UpdateLabel(labelName, model);
            }
            catch (LabelException)
            {
                throw;
            }
        }
    }
}
