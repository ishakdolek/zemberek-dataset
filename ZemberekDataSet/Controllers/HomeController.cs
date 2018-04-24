using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZemberekDataSet.Models;
using ZemberekDataSet.Zemberek;

namespace ZemberekDataSet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFindRootManager _findRootManager;

        public HomeController(IFindRootManager findRootManager)
        {
            _findRootManager = findRootManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Home")]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","dataset.txt");

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            var readFile = _findRootManager.ReadFile(filePath);

            return RedirectToAction("Index");
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
