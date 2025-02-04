﻿using API.Models.DTOs;
using API.Models.DTOs.Employees;
using API.Models.Employees;
using API.Services.Employees;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Employees
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseApiController<Employee, EmployeeDto, EmployeeDto>
    {
        private readonly EmployeesService _employeesService;

        public EmployeesController(EmployeesService service) : base(service)
        {
            _employeesService = service;
        }

        protected override int GetEntityId(EmployeeDto entity)
        {
            return entity.Id;
        }
    }
}
