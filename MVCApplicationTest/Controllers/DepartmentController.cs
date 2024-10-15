using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVCApplicationTest.BLL.Interfaces;
using MVCApplicationTest.DAL.Models;
using MVCApplicationTest.BLL.Interfaces;
using MVCApplicationTestPL.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVCApplicationTestPL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Department> departments;
            if (string.IsNullOrEmpty(SearchValue))
                departments = await _unitOfWork.DepartmentRepository.GetAll();
            else
                departments = _unitOfWork.DepartmentRepository.GetDepartmentByName(SearchValue);
            
            var departmentsVM = _mapper.Map<IEnumerable<Department>,IEnumerable<DepartmentViewModel>>(departments);
            return View(departmentsVM);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                /// Manual Mapping 
                ///Department mappedDepartment = new Department()
                ///{
                ///    Id = department.Id,
                ///    Name = department.Name,
                ///    Code = department.Code,
                ///    DateOfCreation = department.DateOfCreation,
                ///    Employees = department.Employees,
                ///};
                ///

                Department mappedDepartment = _mapper.
                    Map<DepartmentViewModel, Department>(departmentVM);
               
                await _unitOfWork.DepartmentRepository.Add(mappedDepartment);
                int insertedRowCount = await _unitOfWork.Complete();
                if (insertedRowCount > 0)
                    TempData["Message"] = "Department Created Successfully!!";
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }

        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var department = await _unitOfWork.DepartmentRepository.Get(id.Value);

            if(department is null)
                return NotFound();
            var departmentVM = _mapper.Map<Department,DepartmentViewModel>(department);
            return View(viewName,departmentVM);
        }

        public async Task<IActionResult> Edit(int?id)
        {
            //if(id is null ) return BadRequest();
            //var department = _departmentRepository.Get(id.Value);
            //if(department is null) return NotFound();
            //return View(department);

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit([FromRoute]int id, DepartmentViewModel departmentVM)
        {
            if(id != departmentVM.Id)
                return BadRequest();
            if(ModelState.IsValid)
            {
                try
                {
                    Department mappedDepartment = 
                        _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Update(mappedDepartment);
                    await _unitOfWork.Complete();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //restore at log file
                    //Show friendly message
                     ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null) return BadRequest();
            //var department = _departmentRepository.Get(id.Value);
            //if(department is null) return NotFound();
            //return View(department);
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if(id != departmentVM.Id)
                return BadRequest();
            if(ModelState.IsValid)
            {
                try
                {
                    Department mappedDepartment =
                        _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    _unitOfWork.DepartmentRepository.Delete(mappedDepartment);
                    await _unitOfWork.Complete();

                    return RedirectToAction(nameof(Index));
                }catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }
    }
}
