﻿using Application.Dto;
using Application.Repositories;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public sealed class ProjectService
    {
        private IProjectRepository _repository;

        public ProjectService(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProjectDto>> GetAllProjects()
        {
            var projects = await _repository.GetAllAsync();
            return projects.Select(p => ProjectDto.FromDomain(p)).ToList();
        }

        public async Task<ProjectDto> CreateProject(string projectName)
        {
            var project = await _repository.CreateProject(projectName);
            return ProjectDto.FromDomain(project);
        }

        public async Task DeleteProject(ProjectDto project)
        {
            await _repository.DeleteProject(project.Id);
        }
    }
}
