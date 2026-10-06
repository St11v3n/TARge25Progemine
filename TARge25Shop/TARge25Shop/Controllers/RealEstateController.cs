using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TARge25Shop.ApplicationServices.Services;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using TARge25Shop.Models.RealEstate;
using static System.Net.Mime.MediaTypeNames;

namespace TARge25Shop.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly IRealEstateServices _realestateServices;
        private readonly TARge25ShopContext _context;
        private readonly IFileServices _fileService;

        public RealEstateController
            (
                IRealEstateServices realestateServices,
                TARge25ShopContext context
            )

        {
            _realestateServices = realestateServices;
            _context = context;
            _fileService = fileSrvice;
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

        public async Task<IActionResult> 
            Create(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto
            {
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                //failide lisamine
                Files = vm.Files,
                Image = vm.Images
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        RealEstateId = x.RealEstateId

                    }).ToArray()
            };

            var result = await _realestateServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        public async Task<IActionResult> Update(Guid id)
        {
            var realestate = await _realestateServices.DetailAsync(id);
            if (realestate == null)
            {
                return NotFound();
            }

            RealEstateImageViewModel[] images = await FileFromDatabase(id);

            var vm = new RealEstateCreateUpdateViewModel();

                vm.Id = realestate.Id;
                vm.Area = realestate.Area;
                vm.Location = realestate.Location;
                vm.RoomNumber = realestate.RoomNumber;
                vm.BuildingType = realestate.BuildingType;
                vm.CreatedAt = realestate.CreatedAt;
                vm.ModifiedAt = realestate.ModifiedAt;
                vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]

        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel vm)
        {

            var dto = new RealEstateDto()
            {
                Id = vm.Id,
                Area = vm.Area,
                Location = vm.Location,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files,
                Image = vm.Image
                    .Select()(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        RealEstateId = x.RealEstateId
                    }).ToArray()

                
            };

            var result = await _realestateServices.Update(dto);
            var realEstateId = result.Id;
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
           
            return RedirectToAction(nameof(Details), new {id = realEstateId});

        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var realestate = await _realestateServices.DetailAsync(id);

            if (realestate == null)
            {
                return NotFound();
            }

            RealEstateImageViewModel[] images = await FileFromDatabase(id);

            var vm = new RealEstateDeleteViewModel();

                vm.Id = realestate.Id;
                vm.Area = realestate.Area;
                vm.Location = realestate.Location;
                vm.RoomNumber = realestate.RoomNumber;
                vm.BuildingType = realestate.BuildingType;
                vm.CreatedAt = realestate.CreatedAt;
                vm.ModifiedAt = realestate.ModifiedAt;
                vm.Image.AddRange(images);

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

            RealEstateImageViewModel[] images = await FileFromDatabase(id);

            var vm = new RealEstateDetailsViewModel();

                vm.Id = realestate.Id;
                vm.Area = realestate.Area;
                vm.Location = realestate.Location;
                vm.RoomNumber = realestate.RoomNumber;
                vm.BuildingType = realestate.BuildingType;
                vm.CreatedAt = realestate.CreatedAt;
                vm.ModifiedAt = realestate.ModifiedAt;
                vm.Images.AddRange(images);

            return View(vm);
        }

        public async Task<IActionResult> RemoveImage(RealEstateImageViewModel vm)
        {
            var dto = new FileToDatabaseDto()
            {
                Id = vm.ImageId
            };
            var image = await _fileService.RemoveImageFromDatabase(dto);
            var realEstateId = image.RealEstateId;

            if (image == null)
            {
                return RedirectToAction(nameof(Index));
            }
            //muuta see niimoodi, et pärast pildi kustutamist jääks kasutaja
            //samale kinnisvara detailide lehele, mitte ei suunataks tagasi index lehele
            return RedirectToAction(nameof(Update), new {id = realEstateId});
        }

        private async Task<RealEstateImageViewModel[]> FileFromDatabase(Guid id)
        {
            return await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageViewModel
                {
                    ImageId = y.Id,
                    ImageTitle = y.ImageTitle,
                    ImageData = y.ImageData,
                    RealEstateId = y.RealEstateId,
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();
        }
    }
}
