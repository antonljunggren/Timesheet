using Application.Repositories;
using Domain.Models;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal sealed class ProjectRepository : IProjectRepository
    {
        private TimeSheetDbContext _dbContext;

        public ProjectRepository(IDbContextFactory<TimeSheetDbContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public async Task<Project> FindByIdAsync(Guid id)
        {
            var entity = await _dbContext.Projects.AsNoTracking().SingleOrDefaultAsync(p => p.Id == id);

            if(entity is null)
            {
                throw new KeyNotFoundException($"Project [{id}] is not found");
            }

            return new Project(entity.Id, entity.Name);
        }

        public async Task<List<Project>> GetAllAsync()
        {
            return await _dbContext.Projects.AsNoTracking().Select(e => new Project(e.Id, e.Name)).ToListAsync();
        }
    }
}
