using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IProjectRepository
    {
        Task<Project> FindByIdAsync(Guid id);
        Task<List<Project>> GetAllAsync();
        Task<Project> CreateProject(string projectName);
        Task DeleteProject(Guid id);
    }
}
