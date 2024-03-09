using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public sealed record ProjectDto(Guid Id, string ProjectName)
    {
        public static ProjectDto FromDomain(Project project)
        {
            var dto = new ProjectDto(
                project.Id,
                project.Name
                );

            return dto;
        }
    }
}
