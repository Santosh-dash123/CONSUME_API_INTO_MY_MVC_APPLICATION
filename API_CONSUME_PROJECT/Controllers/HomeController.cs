using API_CONSUME_PROJECT.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using static System.Net.WebRequestMethods;

namespace API_CONSUME_PROJECT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("https://localhost:7043/api/Home/GetStudent");
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<STUDENT>>(responseBody);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> AddStudent(int Id)
        {
            if(Id != 0)
            {
                var response = await _httpClient.GetAsync("https://localhost:7043/api/Home/GetAParticularStudent?Id="+Id);
                var responsebody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<STUDENT>(responsebody);
                return View(result);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(STUDENT student)
        {
            
            if(student == null)
            {
                return NotFound("Please provide student data !!!");
            }
            else
            {
                //This code is written for student coming as a object data but api gets the json data 
                //so, i will change the format from object to json using JsonSerializer.Serialize() method
                var json = System.Text.Json.JsonSerializer.Serialize(student);

                //Here, StringContent class is created which represents httpcontent as a string and it will take
                //three parameters which is take json format data, and second one is encoding the data from json, and
                //in third parameter i will write only type of json name mention that is "application/json"
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                //This line sends httppost request to the specified url and content as the request body.
                var response = await _httpClient.PostAsync("https://localhost:7043/api/Home/CreateAStudent", content);

                //This block of code checks if the HTTP response from the server indicates success(status code in the
                //range 200 - 299).If the response is successful, it redirects the user to the "Index" action of the
                //controller using RedirectToAction("Index"). If the response is not successful(e.g., status code in
                //the 400 or 500 range), it returns the current view.
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
        }
        public async Task<IActionResult> DeleteStudent(int Id)
        {
            if(Id == 0)
            {
                return NotFound("Please provide valid student !!!");
            }
            else
            {
                var response = await _httpClient.DeleteAsync("https://localhost:7043/api/Home/DeleteStudent?Id=" + Id);
                if(response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
