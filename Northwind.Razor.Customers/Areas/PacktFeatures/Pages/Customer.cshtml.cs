using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Packt.Shared;
namespace Northwind.Razor.Customers.Areas.PacktFeatures.Pages
{
    public class CustomerModel : PageModel
    {
        public Customer customer;
        public List<string> productNames;
        private NorthwindContext db;
        public CustomerModel(NorthwindContext injectContext)
        {
            db = injectContext;
        }

        public void OnGet(string id)
        {
            customer = db.Customers.Find(id);
            productNames = db.Products.Join(db.OrderDetails, products => products.ProductId, orderDetails => orderDetails.ProductId, (products, orderDetails) => new { products, orderDetails }).Join(db.Orders, combOrderDetails => combOrderDetails.orderDetails.OrderId, orders => orders.OrderId, (orders, combOrderDetails) => new { product = orders.products, custId = combOrderDetails.CustomerId }).Where(c => c.custId == id).Select(c => c.product.ProductName).ToList();
        }
    }
}
