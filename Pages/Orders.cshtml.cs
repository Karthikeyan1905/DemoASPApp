using DemoASPApp.model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UserManagementModel.Model.Orders;

namespace DemoASPApp.Pages
{
    public class OrdersModel : BaseModel
    {
        private readonly HttpClientConnector _httpClientConnector;

        public OrdersModel(HttpClientConnector httpClientConnector)
        {
            _httpClientConnector = httpClientConnector;
        }

        public List<Category> Categories { get; set; }
        public List<SubCategory> Subcategories { get; set; }
        public List<Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = await _httpClientConnector.GetAsync<List<Category>>("http://localhost:5112/Category/Select");
            Subcategories = await _httpClientConnector.GetAsync<List<SubCategory>>("http://localhost:5112/Subcategory/Select");
            Products = await _httpClientConnector.GetAsync<List<Product>>("http://localhost:5112/Product/Select");

            return Page();
        }
    }
}
