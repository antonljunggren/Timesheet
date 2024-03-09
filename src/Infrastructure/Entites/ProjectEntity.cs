using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entites
{
    internal sealed class ProjectEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<TimeWorkedEntity>? TimeWorked { get; set; }
    }
}
