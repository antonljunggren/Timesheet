using Application.Dto;
using Application.Repositories;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public sealed class TimeWorkedService
    {
        private ITimeWorkedRepository _timeWorkedRepository;
        private IProjectRepository _projectRepository;

        public TimeWorkedService(ITimeWorkedRepository timeWorkedRepository, IProjectRepository projectRepository)
        {
            _timeWorkedRepository = timeWorkedRepository;
            _projectRepository = projectRepository;
        }

        public async Task<List<TimeWorkedDto>> GetTimeWorkedByMonth(int monthNumber)
        {
            if(monthNumber < 0 || monthNumber > 12)
            {
                throw new ArgumentOutOfRangeException($"monthNumber: {monthNumber} is is not valid");
            }

            var timesWorked = await _timeWorkedRepository.FindAllByMonthAsync(monthNumber);

            return timesWorked.Select(tw => TimeWorkedDto.FromModel(tw)).ToList();
        }

        public async Task<TimeWorkedDto> SaveTimeWorked(TimeWorkedDto timeWorked)
        {
            var project = await _projectRepository.FindByIdAsync(timeWorked.ProjectId);

            if (timeWorked.Id == Guid.Empty)
            {
                var newTimeWorked = timeWorked with { Id = Guid.NewGuid() };
                

                var timeWorkedModel = new TimeWorked(newTimeWorked.Id, newTimeWorked.Date, project, newTimeWorked.Hours, newTimeWorked.Notes);

                await _timeWorkedRepository.AddAsync(timeWorkedModel);

                return newTimeWorked;
            }
            else
            {
                var timeWorkedModel = new TimeWorked(timeWorked.Id, timeWorked.Date, project, timeWorked.Hours, timeWorked.Notes);

                await _timeWorkedRepository.UpdateAsync(timeWorkedModel);
                return timeWorked;
            }
        }

        public async Task DeleteTimeWorked(Guid timeWorkedId)
        {
            await _timeWorkedRepository.DeleteAsync(timeWorkedId);
        }
    }
}
