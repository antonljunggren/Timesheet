using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface ITimeWorkedRepository
    {
        Task<TimeWorked> FindByIdAsync(Guid id);
        Task<List<TimeWorked>> FindAllByMonthAsync(int month);
        Task AddAsync(TimeWorked timeWorked);
        Task UpdateAsync(TimeWorked timeWorked);
        Task DeleteAsync(Guid id);
    }
}
