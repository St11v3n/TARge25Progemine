using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;

namespace TARge25Shop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly IHostEnvironment _webHost;
        private readonly TARge25ShopContext _context;
        
        public FileServices
            (
                IHostEnvironment webHost,
            TARge25ShopContext context
            )
        {
            _webHost = webHost;
            _context = context;
        }
               
        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                //kui Directoryt ei ole olemas, siis tee Directory
                //\\wwwroot\\multipleFileUpload\\
                //tuleb kasutada webHosti
                //if
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    //tuleb teha muutuja, kus on failide asukoht e kuhu hakatakse salvestama
                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    //tuleb kaks ülevalpool olevat muutujat kombineerida üheks
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        //tuleb Domaini teha class FileToApi
                        //kus on muutujad Id, ExistingFilePath ja SpaceshipId
                        FileToApi path = new FileToApi
                        {
                            Id = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName,
                            SpaceshipId = domain.Id
                        };

                        //tuleb lisada context konstruktorisse
                        _context.FileToApis.AddAsync(path);
                        
                    }

                }
            }

        }

        public async Task<FileToApi> RemoveImageFromApi(FileToApiDto dto)
        {
            //kui soovin kustutada faili, siis pean läbi Id pildi ülesse otsima
            var ImageId = await _context.FileToApis
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

            //teha muutuja filePath, mis näitab failide asukohta
            var filePath =  _webHost.ContentRootPath + "\\wwwroot\\mutlipleFileUpload\\"
                + ImageId.ExistingFilePath;
            //kui fail asub selles kaustas, siis kustuta
            if (File.Exists(filePath))

            {
                File.Delete(filePath);
            }

            _context.FileToApis.Remove(ImageId);
            await _context.SaveChangesAsync();

            return null;
        }
    }
}
