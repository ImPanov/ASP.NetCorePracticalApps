using Microsoft.AspNetCore.Mvc.RazorPages; // PageModel
using Packt.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Northwind.Web.Pages;
public class SuppliersModel : PageModel
{
    public IEnumerable<Supplier>? Suppliers { get; set; }
    private NorthwindContext db;
    public SuppliersModel(NorthwindContext injectContext)
    {
        db = injectContext;
    }
    public void OnGet()
    {
        ViewData["Title"] = "Northwind B2B - Suppliers";
        Suppliers = db.Suppliers.OrderBy(c => c.Country).ThenBy(c => c.CompanyName);

    }
    [BindProperty]
    public Supplier? Supplier { get; set; }
    public IActionResult OnPost()
    {
        if ((Supplier is not null) && ModelState.IsValid)
        {
            db.Suppliers.Add(Supplier);
            db.SaveChanges();
            return RedirectToPage("""/suppliers""");
        }
        else
        {
            return Page(); // return to original page
        }
    }

}