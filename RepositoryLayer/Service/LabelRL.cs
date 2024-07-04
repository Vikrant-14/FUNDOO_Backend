using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExecption;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class LabelRL : ILabelRL
    {
        private readonly ApplicationDbContext _context;

        public LabelRL(ApplicationDbContext context)
        {
            _context = context;
        }

        public Label CreateLabel(LabelML model)
        {
            Label label = new Label() 
            {
                LabelName = model.LabelName
            };

            _context.Labels.Add(label);
            _context.SaveChanges();

            return label;
        }

        public Label UpdateLabel(int id, LabelML model)
        {
            var label = _context.Labels.FirstOrDefault(l => l.Id == id);

            if (label == null)
            {
                throw new LabelException("No such label exists");
            }

            label.LabelName = model.LabelName;

            _context.Labels.Update(label);
            _context.SaveChanges();

            return label;
        }
        
        public Label GetLabelById(int id)
        {
            var label = _context.Labels.FirstOrDefault(l => l.Id == id);

            if (label == null)
            {
                throw new LabelException("No such label exists");
            }

            return label;
        }

        public IList<Label> GetAllLabels()
        {
            var lables = _context.Labels.ToList();

            if(lables.Count == 0)
            {
                throw new LabelException("Label List is empty.");
            }

            return lables;
        }

        public Label DeleteLabel(int id)
        {
            var label = _context.Labels.FirstOrDefault(l => l.Id == id);

            if (label == null)
            {
                throw new LabelException("No such label exists");
            }

            _context.Labels.Remove(label);
            _context.SaveChanges();

            return label;
        }
    }
}
