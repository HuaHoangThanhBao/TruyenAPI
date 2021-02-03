using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.Models;
using Contracts;
using AutoMapper;
using CoreLibrary.DataTransferObjects;
using System.Net.Http;

namespace Core.Controllers
{
    public class HomeController : Controller
    {
        private IRepositoryWrapper _repository;
        private IMapper _mapper;

        public HomeController(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            //var owner = _repository.Owner.GetOwnerById(1);

            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync("http://localhost:50504/api/owner"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    ViewBag.list = apiResponse;
                }
            }

            return View();
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
