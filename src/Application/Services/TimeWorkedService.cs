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

        public async Task<List<TimeWorkedDto>> GetTimeWorkedByMonthWithPrevNextMonth(int monthNumber)
        {
            if(monthNumber < 0 || monthNumber > 12)
            {
                throw new ArgumentOutOfRangeException($"monthNumber: {monthNumber} is is not valid");
            }

            var timesWorked = await _timeWorkedRepository.FindAllByMonthAsync(monthNumber);

            var previousMonth = await _timeWorkedRepository.FindAllByMonthAsync(monthNumber > 1 ? monthNumber - 1 : 12);
            var nextMonth = await _timeWorkedRepository.FindAllByMonthAsync(monthNumber < 12 ? monthNumber + 1 : 1);

            previousMonth = previousMonth.Where(tw => tw.Date.Day >= 31-7).ToList();
            nextMonth = nextMonth.Where(tw => tw.Date.Day <= 7).ToList();

            timesWorked.AddRange(previousMonth);
            timesWorked.AddRange(nextMonth);

            return timesWorked.Select(tw => TimeWorkedDto.FromModel(tw)).ToList();
        }

        public async Task<TimeWorkedDto> SaveTimeWorked(TimeWorkedDto timeWorked)
        {
            var project = await _projectRepository.FindByIdAsync(timeWorked.ProjectId);

            if (timeWorked.Id == Guid.Empty)
            {
                var newTimeWorked = timeWorked with { Id = Guid.NewGuid() };
                

                var timeWorkedModel = new TimeWorked(newTimeWorked.Id, newTimeWorked.Date, project.Id, newTimeWorked.Hours, newTimeWorked.Notes);

                await _timeWorkedRepository.AddAsync(timeWorkedModel);

                return newTimeWorked;
            }
            else
            {
                var timeWorkedModel = new TimeWorked(timeWorked.Id, timeWorked.Date, project.Id, timeWorked.Hours, timeWorked.Notes);

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
