using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using coreweb.Model;
using Newtonsoft.Json;
namespace coreweb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
        async public Task<IActionResult> GetContacts()
        {
            try
            {
                var url = $"http://{Environment.GetEnvironmentVariable("API_DOMAINNAME")}/api/getcontacts";
                var http = new HttpClient();
                var res = await http.GetAsync(url);
                var data = await res.Content.ReadAsStringAsync();
                return Json(new { status = true, result = data });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, result = "", message = ex.Message });
            }
        }
        [HttpPost]
        async public Task<IAsyncResult> SaveContact(ContactInfo contacts)
        {
            try
            {
                var url = $"http://{Environment.GetEnvironmentVariable("API_DOMAINNAME")}/api/addcontact";
                var http = new HttpClient();
                var contentData = new StringContent(JsonConvert.SerializeObject(contacts), System.Text.Encoding.UTF8, "application/json");
                await http.PostAsync(url, contentData);
                return GetContacts();

            }
            catch (Exception ex)
            {
                return Task.Run(() =>
                {
                    Json(new { status = false, result = "", message = ex.Message });
                }
                );
            }

        }
        public IActionResult GetUrl(string variable)
        {
            return Json(new { Url = Environment.GetEnvironmentVariable(variable) });
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
