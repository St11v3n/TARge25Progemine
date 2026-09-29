using Microsoft.AspNetCore.Mvc;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using TARge25Shop.Models.RealEstate;

namespace TARge25Shop.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly IRealEstateServices _realestateServices;
        private readonly RealEstateContext _context;

        public RealEstateController
            (
                IRealEstateServices realestateServices,
                RealEstateContext context
            )

        {
            _realestateServices = realestateServices;
            _context = context;
        }

        public IActionResult Index()
        {
            var result = _context.RealEstates
                .Select(x => new RealEstateIndexViewModel
                {
                    Id = x.Id,
                    Area = x.Area,
                    Location = x.Location,
                    RoomNumber = x.RoomNumber,
                    BuildingType = x.BuildingType
                });
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel result = new();
            return View("CreateUpdate", result);
        }

        [HttpPost]

        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto
            {
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType
            };

            var result = await _realestateServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        public async Task<IActionResult> Modify(Guid id)
        {
            var realestate = await _realestateServices.DetailAsync(id);
            if (realestate == null)
            {
                return NotFound();
            }

            var vm = new RealEstateCreateUpdateViewModel();

                vm.Id = realestate.Id;
                vm.Area = realestate.Area;
                vm.Location = realestate.Location;
                vm.RoomNumber = realestate.RoomNumber;
                vm.BuildingType = realestate.BuildingType;
                vm.CreatedAt = realestate.CreatedAt;
                vm.ModifiedAt = realestate.ModifiedAt;

            return View("CreateUpdate", vm);
        }

        [HttpPost]

        public async Task<IActionResult> Modify(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto()
            {
                Id = vm.Id,
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt
            };

            var result = await _realestateServices.Modify(dto);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var realestate = await _realestateServices.DetailAsync(id);

            if (realestate == null)
            {
                return NotFound();
            }

            var vm = new RealEstateDeleteViewModel();

                vm.Id = realestate.Id;
                vm.Area = realestate.Area;
                vm.Location = realestate.Location;
                vm.RoomNumber = realestate.RoomNumber;
                vm.BuildingType = realestate.BuildingType;
                vm.CreatedAt = realestate.CreatedAt;
                vm.ModifiedAt = realestate.ModifiedAt;

            return View(vm);
        }

        [HttpPost]

        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var realestate = await _realestateServices.Delete(id);
            
            if (realestate == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var realestate = await _realestateServices.DetailAsync(id);

            if (realestate == null)
            {
                return NotFound();
            }

            var vm = new RealEstateDetailsViewModel();

                vm.Id = realestate.Id;
                vm.Area = realestate.Area;
                vm.Location = realestate.Location;
                vm.RoomNumber = realestate.RoomNumber;
                vm.BuildingType = realestate.BuildingType;
                vm.CreatedAt = realestate.CreatedAt;
                vm.ModifiedAt = realestate.ModifiedAt;

            return View(vm);
        }

    }
}
