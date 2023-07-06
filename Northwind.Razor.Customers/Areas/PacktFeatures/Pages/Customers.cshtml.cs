using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Packt.Shared;

namespace PacktFeatures.Pages
{
    public class CustomersPageModel : PageModel
    {
        private NorthwindContext db;
        public CustomersPageModel(NorthwindContext injectContext)
        {
            db = injectContext;
        }
        public Dictionary<string?,List<Customer>>? CountryCustomers { get; set; }
        public void OnGet()
        {
            CountryCustomers = db.Customers.GroupBy(c=>c.Country).ToDictionary(c=>c.Key,c=>c.ToList());
        }
        
    }
}