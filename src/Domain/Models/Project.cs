using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public sealed class Project
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;

        public Project(Guid id, string name)
        {
            Id = id;
            ChangeName(name);
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                throw new ArgumentNullException(nameof(newName));
            }

            Name = newName;
        }
    }
}
