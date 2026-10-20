using Microsoft.AspNetCore.Mvc;
using TARge25Shop.ApplicationServices;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using TARge25Shop.Models.Cocktails;

namespace TARge25Shop.Controllers
{
    public class CocktailsController : Controller
    {
        private readonly ICocktailServices _cocktailServices;
        private readonly TARge25ShopContext _context;

        public CocktailsController
            (
                ICocktailServices cocktailServices,
                TARge25ShopContext context

            )
        {
            _cocktailServices = cocktailServices;
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public async Task<IActionResult> Drink()
        {
            var drink = await _cocktailServices.CocktailResultHttpClient();

            CocktailsViewModel vm = new();

            
            vm.Id = drink.Id;
            vm.Name = drink.Name;
            vm.Alternative = drink.Alternative;
            vm.Tags = drink.Tags;
            vm.Video = drink.Video;
            vm.Category = drink.Category;
            vm.IBA = drink.IBA;
            vm.Alcoholic = drink.Alcoholic;
            vm.Glass = drink.Glass;
            vm.Instructions = drink.Instructions;

            return View("Drink", vm);

        }
    }
}
