using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Packt.Shared;

namespace PacktFearures.Pages
{
    public class EmployeesPageModel : PageModel
    {
        private NorthwindContext db;

        public EmployeesPageModel(NorthwindContext injectedContext)
        {
            db = injectedContext;
        }
        public Employee[]? Employees { get; set; }
        public void OnGet()
        {
            ViewData["Title"] = $"NorthwindContext B2B - Employees";
            Employees = db.Employees.OrderBy(c=>c.LastName).ThenBy(c=>c.FirstName).ToArray();
        }
    }
}