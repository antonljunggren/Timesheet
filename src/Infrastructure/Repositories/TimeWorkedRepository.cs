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
    internal sealed class TimeWorkedRepository : ITimeWorkedRepository
    {
        private TimeSheetDbContext _dbContext;

        public TimeWorkedRepository(IDbContextFactory<TimeSheetDbContext> dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public async Task AddAsync(TimeWorked timeWorked)
        {
            var entity = new TimeWorkedEntity()
            {
                Id = timeWorked.Id,
                ProjectId = timeWorked.Project.Id,
                Date = timeWorked.Date,
                Hours = Convert.ToByte(timeWorked.Hours),
                Notes = timeWorked.Notes,
                IsActive = true,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
            };

            await _dbContext.TimeWorked.AddAsync(entity);
            var rows = await _dbContext.SaveChangesAsync();
            if(rows <= 0)
            {
                throw new Exception("TimeWorked was not added to database");
            }
        }

        public async Task UpdateAsync(TimeWorked timeWorked)
        {
            var entity = (await _dbContext.TimeWorked.SingleOrDefaultAsync(tw =>  tw.Id == timeWorked.Id))?
                .UpdateFromDomain(timeWorked);

            if(entity is null)
            {
                throw new NullReferenceException($"TimeWorked with id: {timeWorked.Id} does not exist in the database");
            }

            _dbContext.Update(entity);
            var rows = await _dbContext.SaveChangesAsync();
            if (rows <= 0)
            {
                throw new Exception("TimeWorked was not updated in database");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entityToDelete = await _dbContext.TimeWorked.SingleOrDefaultAsync(tw => tw.Id == id);

            if (entityToDelete is null)
            {
                throw new NullReferenceException($"TimeWorked with id: {id} does not exist in the database");
            }
            _dbContext.TimeWorked.Remove(entityToDelete);
            var rows = await _dbContext.SaveChangesAsync();

            if (rows <= 0)
            {
                throw new Exception($"TimeWorked {id} was not removed from database");
            }
        }

        public async Task<List<TimeWorked>> FindAllByMonthAsync(int month)
        {
            var result = await _dbContext.TimeWorked
                .Where(tw => tw.Date.Month == month)
                .Include(tw => tw.Project)
                .AsNoTracking()
                .Select(tw => new TimeWorked(tw.Id, tw.Date, new Project(tw.ProjectId, tw.Project!.Name), tw.Hours, tw.Notes))
                .ToListAsync();

            return result;
        }

        public async Task<TimeWorked> FindByIdAsync(Guid id)
        {
            var entity = await _dbContext.TimeWorked
                .Include(tw => tw.Project)
                .AsNoTracking()
                .SingleOrDefaultAsync(tw => tw.Id == id);
            if(entity is null)
            {
                throw new NullReferenceException($"TimeWorked with id: {id} does not exist in the database");
            }

            return new TimeWorked(entity.Id, entity.Date, new Project(entity.ProjectId, entity.Project!.Name), entity.Hours, entity.Notes);
        }
    }
}
