using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using TARge25Shop.Models.ChuckNorris;
using TARge25Shop.Models.RealEstate;

namespace TARge25Shop.Controllers
{
    public class ChuckNorrisController : Controller
    {
        private readonly IChuckNorrisServices _chuckNorrisServices;
        private readonly TARge25ShopContext _context;

        public ChuckNorrisController
            (
                IChuckNorrisServices chuckNorrisServices,
                TARge25ShopContext context
                
            )
        {
            _chuckNorrisServices = chuckNorrisServices;
            _context = context;
            
        }
        public IActionResult Index()
        { 
            return View();
        }

        

        [HttpGet]
        public async Task<IActionResult> Joke()
        {
            var joke = await _chuckNorrisServices.ChuckNorrisResultHttpClient();

            ChuckNorrisViewModel vm = new();

            //vm.Categories = joke.Categories;
            vm.CreatedAt = joke.CreatedAt;
            vm.IconUrl = joke.IconUrl;
            vm.Id = joke.Id;
            vm.UpdatedAt = joke.UpdatedAt;
            vm.Url = joke.Url;
            vm.Value = joke.Value;

            return View("Joke", vm);

        }
    }
}
