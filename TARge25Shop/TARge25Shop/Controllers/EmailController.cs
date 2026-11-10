using Microsoft.AspNetCore.Mvc;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Models.Email;

namespace TARge25Shop.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailServices _emailServices;

        public EmailController

            (
                IEmailServices emailServices
            )

        {
            _emailServices = emailServices;
        }
        public IActionResult Index()
        { 
            return View();
        }

        [HttpPost]
        //Vaja teha meetod, mis võtab vastu EmailDto objekti ja kutsub välja _emailServices.Sendmail meetodi
        //ning teha vahe instants, mis vahendab andmeid EmailDto ja ViewModeli vahel

        public IActionResult SendEmail(EmailViewModel vm)
        {
            var files = Request.Form.Files.Any() ? Request.Form.Files.ToList() : new List<IFormFile>();
            var emailDto = new EmailDto
            {
                To = vm.To,
                Subject = vm.Subject,
                Body = vm.Body,
                Attachments = files
            };
            _emailServices.SendEmail(emailDto);

            return RedirectToAction(nameof(Index));
        }
    }
}
