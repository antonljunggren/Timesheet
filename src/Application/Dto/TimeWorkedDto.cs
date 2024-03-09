using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public sealed record TimeWorkedDto(Guid Id, Guid ProjectId, int Hours, DateOnly Date, string Notes)
    {
        public static TimeWorkedDto FromModel(TimeWorked timeWorked)
        {
            var dto = new TimeWorkedDto(
                timeWorked.Id,
                timeWorked.Project.Id,
                timeWorked.Hours,
                timeWorked.Date,
                timeWorked.Notes
                );

            return dto;
        }
    }
}
