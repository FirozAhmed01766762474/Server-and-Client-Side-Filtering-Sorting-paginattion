using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pagination.Common;
using Pagination.Data;
using Pagination.Models;

namespace App14_EFCore.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly DataContext _context;

        public EmployeesController(DataContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(int page, string searchString)
        {

            if(page<=0) { page = 1; }

            List<Employee> emplist = await _context.Employee.ToListAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                emplist = emplist.Where(e =>
                                        e.EmployeeName.ToLower().Contains(searchString) ||
                                        e.EmployeeStatus.ToLower().Contains(searchString) ||
                                        e.Salary.ToString().Contains(searchString) || // Convert Salary to string for searching
                                        e.PayBasis.ToLower().Contains(searchString) ||
                                        e.PositionTitle.ToLower().Contains(searchString)).ToList();
            }

            int tot_records = emplist.Count;
            int pagesize = 10;
            int number_of_button = 4;
            

            Pager P = new Pager(tot_records, page, pagesize, number_of_button);
            ViewBag.pager = P;
            int skip_records = (page - 1) * pagesize;
            int take_records = pagesize;

            List<Employee> employees = emplist.Skip(skip_records).Take(take_records).ToList();



            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Test(int page, int rowperpage,string? searchString = null, string sortField = null, bool sortAscending = true)
        {

            if (page <= 0) { page = 1; }

            ViewBag.CurrentSortField = sortField;
            ViewBag.CurrentSortAscending = sortAscending;

            var emplist = await _context.Employee.ToListAsync();


            if (!string.IsNullOrEmpty(searchString))
            {
                emplist = emplist.Where(e =>
                                        e.EmployeeName.ToLower().Contains(searchString.ToLower()) ||
                                        e.EmployeeStatus.ToLower().Contains(searchString.ToLower()) ||
                                        e.Salary.ToString().Contains(searchString.ToLower()) || // Convert Salary to string for searching
                                        e.PayBasis.ToLower().Contains(searchString.ToLower()) ||
                                        e.PositionTitle.ToLower().Contains(searchString.ToLower())).ToList();
            }


            switch (sortField)
            {
                case "EmployeeName":
                    emplist = sortAscending ? emplist.OrderBy(e => e.EmployeeName).ToList() : emplist.OrderByDescending(e => e.EmployeeName).ToList();
                    break;
                case "EmployeeStatus":
                    emplist = sortAscending ? emplist.OrderBy(e => e.EmployeeStatus).ToList() : emplist.OrderByDescending(e => e.EmployeeStatus).ToList();
                    break;
                case "Salary":
                    emplist = sortAscending ? emplist.OrderBy(e => e.Salary).ToList() : emplist.OrderByDescending(e => e.Salary).ToList();
                    break;
                case "PositionTitle":
                    emplist = sortAscending ? emplist.OrderBy(e => e.PositionTitle).ToList() : emplist.OrderByDescending(e => e.PositionTitle).ToList();
                    break;
                default:
                    emplist = emplist.OrderBy(e => e.EmployeeName).ToList();
                    break;
            }


            int tot_records = emplist.Count;
            int pagesize = rowperpage > 0 ? rowperpage :10;
            int number_of_button = 4;


            Pager P = new Pager(tot_records, page, pagesize, number_of_button, searchString);
            ViewBag.pager = P;
            int skip_records = (page - 1) * pagesize;
            int take_records = pagesize;

            List<Employee> employees = emplist.Skip(skip_records).Take(take_records).ToList();



            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> ClientSide()
        {

            var employees = await _context.Employee.ToListAsync();



            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeName,EmployeeStatus,Salary,PayBasis,PositionTitle")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeName,EmployeeStatus,Salary,PayBasis,PositionTitle")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
