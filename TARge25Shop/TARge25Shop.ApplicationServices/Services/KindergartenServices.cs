
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.Domain;
using TARge25Shop.Data;
using TARge25Shop.Core.ServiceInterface;
using Microsoft.EntityFrameworkCore;

namespace TARge25Shop.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly KindergartenContext _context;
        public KindergartenServices
            (
                KindergartenContext context
            )
        {
            _context = context;
        }
        //see meetod on vaja controlleris esile kutsuda
        //peab lisama interface, et kutsuda see meetod välja
        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            //siin peab tegema vaheinstansi dto ja domain vahel,
            //et andmed liiguvad dto-st domain objekt
            Kindergarten kinderGarten = new();

            kinderGarten.Id = Guid.NewGuid();
            kinderGarten.GroupName = dto.GroupName;
            kinderGarten.ChildrenCount= dto.ChildrenCount;
            kinderGarten.KindergartenName = dto.KindergartenName;
            kinderGarten.TeacherName = dto.TeacherName;
            kinderGarten.CreatedAt = DateTime.Now;
            kinderGarten.UpdatedAt = DateTime.Now;

            //andmete salvestamine andmebaasi
            _context.Kindergartens.Add(kinderGarten);
            await _context.SaveChangesAsync();

            return kinderGarten;
        }

        //teha update meetod, mis võtab vastu dto ja uunedab olemasolevat kosmoselaeva

        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            //siin peab tegema vaheinstansi dto ja domain vahel,
            //et andmed liiguvad dto-st domain objekt
            Kindergarten kinderGarten = new();

            kinderGarten.Id = Guid.NewGuid();
            kinderGarten.GroupName = dto.GroupName;
            kinderGarten.ChildrenCount = dto.ChildrenCount;
            kinderGarten.KindergartenName = dto.KindergartenName;
            kinderGarten.TeacherName = dto.TeacherName;
            kinderGarten.CreatedAt = dto.CreatedAt;
            kinderGarten.UpdatedAt = DateTime.Now;

            //andmete uuendamine andmebaasis
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
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Kindergartens.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }



}
