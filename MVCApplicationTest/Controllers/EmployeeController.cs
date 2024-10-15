using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVCApplicationTest.BLL.Interfaces;
using MVCApplicationTest.BLL.Specifications;
using MVCApplicationTest.DAL.Models;
using MVCApplicationTestPL.Helpers;
using MVCApplicationTestPL.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVCApplicationTestPL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string SearchValue)
        {
            //ViewData["Message"] = "Hello Badr View Data";
            //ViewBag.Message = "Hello Badr View Bag";

            IEnumerable<Employee> employees;
            var spec = new EmployeeWithDepartmentSpecification();
            if (string.IsNullOrEmpty(SearchValue))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllWithSpecAsync(spec);
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.GetEmployeeByName(SearchValue);
            }
            var mappedEmployees = _mapper
                    .Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(mappedEmployees);

        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var spec = new EmployeeWithDepartmentSpecification(id);
            var employee = await _unitOfWork.EmployeeRepository.GetWithSpecAsync(spec);
            if (employee is null)
                return NotFound();

            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);
            return View(viewName, mappedEmployee);
        }

        public IActionResult Create()
        {
            //ViewBag.Departments = _departmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                string fileName = await DocumentSettings.UploadFile(employeeVM.Image, "images");
                employeeVM.ImageName = fileName;

                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                await _unitOfWork.EmployeeRepository.Add(mappedEmployee);
                int rowInsertedCount = await _unitOfWork.Complete();

                if (rowInsertedCount > 0)
                    TempData["Message"] = "Employee Created Successfully!!";
                return RedirectToAction(nameof(Index));

            }
            return View(employeeVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(mappedEmployee);
                    await _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                    int deletedRows = await _unitOfWork.Complete();
                    if (deletedRows > 0)
                        DocumentSettings.DeleteFiel("images", employeeVM.ImageName);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }

    }
}
