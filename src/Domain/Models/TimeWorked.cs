using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public sealed class TimeWorked
    {
        public Guid Id { get; private set; }
        public DateOnly Date { get; private set; }
        public Project Project { get; private set; } = default!;
        public int Hours { get; private set; }
        public string Notes { get; private set; } = default!;

        public TimeWorked(Guid id, DateOnly date, Project project, int hours, string notes)
        {
            Id = id;
            Date = date;
            ChangeProject(project);
            ChangeHours(hours);
            ChangeNotes(notes);
        }

        public void ChangeProject(Project project)
        {
            Project = project;
        }

        public void ChangeHours(int newHours)
        {
            if(newHours <= 0 || newHours > 24)
            {
                throw new ArgumentOutOfRangeException($"Work hours [{newHours}] not in allowed range");
            }

            Hours = newHours;
        }

        public void ChangeNotes(string newNotes)
        {
            Notes = newNotes;
        }
    }

}
