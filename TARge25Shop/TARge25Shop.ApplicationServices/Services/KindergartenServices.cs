using Microsoft.EntityFrameworkCore;
using System.Xml;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using TARge25Shop.Data;
using static System.Net.Mime.MediaTypeNames;


namespace TARge25Shop.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly TARge25ShopContext _context;
        private readonly IFileServices _fileServices;

        public KindergartenServices
            (
                TARge25ShopContext context,
                IFileServices fileServices
            )
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            
            Kindergarten kinderGarten = new();

            kinderGarten.Id = Guid.NewGuid();
            kinderGarten.GroupName = dto.GroupName;
            kinderGarten.ChildrenCount = dto.ChildrenCount;
            kinderGarten.KindergartenName= dto.KindergartenName;
            kinderGarten.TeacherName = dto.TeacherName;
            kinderGarten.CreatedAt = DateTime.Now;
            kinderGarten.UpdatedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, kinderGarten);
            }


            _context.Kindergartens.Add(kinderGarten);
            await _context.SaveChangesAsync();

            return kinderGarten;
        }

        
        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            
            Kindergarten kinderGarten = new();

            kinderGarten.Id = dto.Id;
            kinderGarten.GroupName = dto.GroupName;
            kinderGarten.ChildrenCount = dto.ChildrenCount;
            kinderGarten.KindergartenName = dto.KindergartenName;
            kinderGarten.TeacherName = dto.TeacherName;
            kinderGarten.CreatedAt = dto.CreatedAt;
            kinderGarten.UpdatedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, kinderGarten);
            }


            _context.Kindergartens.Update(kinderGarten);
            await _context.SaveChangesAsync();

            return kinderGarten;
        }

        public async Task<Kindergarten> DetailAsync(Guid id)
        {
            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            return kindergarten;
        }

        public async Task<Kindergarten> Delete(Guid id)
        {
            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToDatabases
                .Where(x => x.KindergartenId == id)
                .Select(y => new FileToDatabaseDto
                {
                    Id = y.Id,
                }).ToArrayAsync();

            await _fileServices.RemoveImagesFromDatabase(images);
            _context.Kindergartens.Remove(kindergarten);
            await _context.SaveChangesAsync();

            return kindergarten;
        }
    }
}