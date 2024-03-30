using Application.Repositories;
using Domain.Models;
using Infrastructure.Entites;
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

        public async Task<Project> CreateProject(string projectName)
        {
            var project = new ProjectEntity()
            {
                Id = Guid.NewGuid(),
                Name = projectName,
                IsActive = true,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
            };

            var createdProject = await _dbContext.Projects.AddAsync(project);
            var rows = await _dbContext.SaveChangesAsync();

            if (rows <= 0)
            {
                throw new Exception("Project was not added to database");
            }

            return new Project(project.Id, project.Name);
        }

        public async Task DeleteProject(Guid id)
        {
            var entity = await _dbContext.Projects.AsNoTracking().SingleOrDefaultAsync(p => p.Id == id);

            if (entity is null)
            {
                throw new KeyNotFoundException($"Project [{id}] is not found");
            }

            _dbContext.Projects.Remove(entity);

            var rows = await _dbContext.SaveChangesAsync();

            if (rows <= 0)
            {
                throw new Exception("Project was not deleted");
            }
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
