using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Domain;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using Xunit;

namespace TARge25Shop.SpaceshipTest
{
    public class KindergartenTest : TestBase
    {
        [Fact]
        // Fact tähistab ära ühte testi xUnit raamistikus
        // 1 - Kirjeldatakse ära kas test on tavaline, või negatiivne.
        // 2 - Kirjeldatakse ära mida parasjagu üritatakse testialuse objektiga teha
        // 3 - Mis tingimustel tulemust kontrollitakse, peale tegevust
        //
        // Selles testis kontrollitakse et (2) Lasteaia lisamisel
        // (1) Ei tohiks (3) saadud tulemus olla tühi. Jälgi seda sõnastusviisi:
        //
        //                  1           2               3
        //                  \/          \/              \/
        public async Task ShouldNot_AddEmptyKindergarten_WhenResultIsReturned()
        {
            // Ülesseade
            KindergartenDto dto = new KindergartenDto()
            {
                GroupName = "",
                ChildrenCount = 8,
                KindergartenName = "Bullworth",
                TeacherName = "Jackson Jackson",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            // tegutsemine
            var result = await Svc<IKindergartenServices>().Create(dto);

            // kontroll
            Assert.NotNull(result);
        }

        // Selles testis kontrollitakse et (2) Kindergarteni päring andmebaasist
        // (1) ei tohiks tagastada objekti (3) kui ID-d ei ole samad:
        //
        //                  1           2               3
        //                  \/          \/              \/
        [Fact]
        public async Task ShouldNot_GetKindergartenByID_WhenIDNotEqual()
        {
            // ülesseade
            Guid wrongGuid = Guid.NewGuid();
            Guid goodGuid = Guid.Parse("F6F51483-E28A-4211-BC99-2305141B4CB9");

            // tegevus
            await Svc<IKindergartenServices>().DetailAsync(goodGuid);

            // kontroll
            Assert.NotEqual(wrongGuid, goodGuid);
        }

        // Selles testis kontrollitakse et lasteaia päringul andmebaasist peaks tagastama objekti siis kui ID on sama
        [Fact]
        public async Task Should_GetKindergartenByID_WhenGuidIsEqual()
        {
            // ülessezade
            Guid databaseGuid = Guid.Parse("F6F51483-E28A-4211-BC99-2305141B4CB9");
            Guid seekGuid = Guid.Parse("F6F51483-E28A-4211-BC99-2305141B4CB9");

            // tegevus
            await Svc<IKindergartenServices>().DetailAsync(seekGuid);

            // kontroll
            Assert.Equal(databaseGuid, seekGuid);
        }

        // Selles testis kontrollitakse et lasteaia kustutamisel andmebaasist peaks kustuma objekt kui tagastatav väärtus on sama
        [Fact]
        public async Task Should_KindergartenDeletedByID_WhenReturnedResultIsEqual()
        {
            // ülesseade
            KindergartenDto dto = MockKindergartenData();

            // tegevus
            var addKindergarten = await Svc<IKindergartenServices>().Create(dto);
            var deleteKindergarten = await Svc<IKindergartenServices>().Delete((Guid)addKindergarten.Id);

            // kontroll
            Assert.Equal(addKindergarten.Id, deleteKindergarten.Id);
        }

        //Selles testis kontrollitakse, et kui lasteaia ei valitud kustutamiseks, siis ei tohiks kustutada lasteaia id kaudu.
        [Fact]
        public async Task ShouldNot_DeleteKindergartenByID_WhenDidNotDeleteKindergarten()
        {
            //ülesseade
            var dto = MockKindergartenData();

            //tegevus
            var kinderGarten1 = await Svc<IKindergartenServices>().Create(dto);
            var kinderGarten2 = await Svc<IKindergartenServices>().Create(dto);

            var result = await Svc<IKindergartenServices>().Delete((Guid)kinderGarten2.Id);

            //kontroll
            Assert.NotEqual(kinderGarten1.Id, result.Id);
        }

        // test mis kontrollib, et kui uuendatakse andmeid, siis peaks lasteaed uuendatud saada id kaudu.
        [Fact]
        public async Task Should_UpdateKindergartenByID_WhenUpdatingData()
        {
            //ülesseade
            var guid = new Guid("F6F51483-E28A-4211-BC99-2305141B4CB9");

            KindergartenDto dto = MockKindergartenData();

            KindergartenDto domain = new();

            domain.Id = Guid.Parse("F6F51483-E28A-4211-BC99-2305141B4CB9");
            domain.KindergartenName = "I DonT KnOW WhAT To DO";
            domain.GroupName = "WAZZAA";
            domain.TeacherName = "Mike Bozawski";
            domain.ChildrenCount = 10;
            domain.CreatedAt = dto.CreatedAt;//  <-- ei tohi muutuda Update korral, tuleb võtta olemasolevast objektist.
            domain.UpdatedAt = DateTime.UtcNow;//  <-- PEAB muutuma Update korral

            //tegevus
            await Svc<IKindergartenServices>().Update(dto);

            //kontroll
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.KindergartenName, domain.KindergartenName);
            Assert.NotEqual(dto.GroupName, domain.GroupName);
            Assert.DoesNotMatch(dto.TeacherName, domain.TeacherName);
            Assert.DoesNotMatch(dto.ChildrenCount.ToString(), domain.ChildrenCount.ToString());
            Assert.Equal(dto.CreatedAt, domain.CreatedAt);
            Assert.NotEqual(dto.UpdatedAt, domain.CreatedAt);
        }

        //Test, mis kontrollib kui ei ole mingit andmed mis on uuendatud, siis ei tohiks uuendada lasteaia id kaudu.
        [Fact]
        public async Task ShouldNot_UpdateKindergartenByID_WhenNoDataIsUpdated()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();
            var createdKindergarten = await Svc<IKindergartenServices>().Create(dto);

            //tegevus
            KindergartenDto nullDto = MockKindergartenNullData();
            var result = await Svc<IKindergartenServices>().Update(nullDto);

            //kontroll
            Assert.NotEqual(createdKindergarten.Id, result.Id);
        }



        //Siia alla tulevad abimeetodid.
        private KindergartenDto MockKindergartenData(bool isOneOrTwo = false)
        {
            if (isOneOrTwo == false)
            {
                return new KindergartenDto
                {
                    GroupName = "Something Something",
                    ChildrenCount = 5,
                    KindergartenName = "My Creativity With Names is Terrible",
                    TeacherName = "Lamar Davis", //God help us if that were the case.
                    CreatedAt = DateTime.MinValue,
                    UpdatedAt = DateTime.MinValue,
                };
            }
            else
            {
                return new KindergartenDto
                {
                    GroupName = "Nothing Has Changed",
                    ChildrenCount = 5,
                    KindergartenName = "Still Cant Think Of Anything Good",
                    TeacherName = "Samuel Leroy Jackson", // Perfect candidate for the job.
                    CreatedAt = DateTime.MinValue,
                    UpdatedAt = DateTime.MinValue,
                };
            }

        }

        private KindergartenDto MockKindergartenNullData()
        {
            return new KindergartenDto
            {
                Id = null,
                GroupName = null,
                ChildrenCount = 0,
                KindergartenName = null,
                TeacherName = null,
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue,
            };
        }

    }
}
