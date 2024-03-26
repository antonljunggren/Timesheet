using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entites
{
    internal sealed class TimeWorkedEntity : BaseEntity
    {
        public Guid ProjectId { get; set; }
        public ProjectEntity? Project { get; set; }
        public DateOnly Date { get; set; }
        public byte Hours { get; set; }
        public string Notes { get; set; } = string.Empty;

        public TimeWorkedEntity UpdateFromDomain(TimeWorked timeWorked)
        {
            ProjectId = timeWorked.ProjectId;
            Hours = Convert.ToByte(timeWorked.Hours);
            Notes = timeWorked.Notes;

            return this;
        }
    }
}
