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
        // Selles testis kontrollitakse et (2) Kosmoselaeva lisamisel
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
            Guid goodGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");

            // tegevus
            await Svc<IKindergartenServices>().DetailAsync(goodGuid);

            // kontroll
            Assert.NotEqual(wrongGuid, goodGuid);
        }

        // Seleta kodus lahti, nagu eelnevate testide laused, eesti keelde, selle testi oma ka.
        // Selles testis kontrollitakse et kosmoselaeva päringul andmebaasist peaks tagastama objekti siis kui ID on sama
        [Fact]
        public async Task Should_GetKindergartenByID_WhenGuidIsEqual()
        {
            // ülessezade
            Guid databaseGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");
            Guid seekGuid = Guid.Parse("ecbc059a-0bca-4df2-aae9-a3211e69185a");

            // tegevus
            await Svc<IKindergartenServices>().DetailAsync(seekGuid);

            // kontroll
            Assert.Equal(databaseGuid, seekGuid);
        }

        // Seleta kodus lahti, nagu eelnevate testide laused, eesti keelde, selle testi oma ka.
        // Selles testis kontrollitakse et kosmoselaeva kustutamisel andmebaasist peaks kustuma objekt kui tagastatav väärtus on sama
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

        // test mis kontrollib, et spaceshipi uuendatakse, uute andmete korral
        [Fact]
        public async Task Should_UpdateKindergartenByID_WhenUpdatingData()
        {
            //ülesseade
            var guid = new Guid("68eb8abd-086a-4c8b-9695-71234143f709");

            KindergartenDto dto = MockKindergartenData();

            KindergartenDto domain = new();

            domain.Id = Guid.Parse("68eb8abd-086a-4c8b-9695-71234143f709");
            domain.KindergartenName = "1 D0nT Kn0W Wh4T T0 D0";
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

        //kuna lastearv ei saa olla negatiivse arvuga, kontrollime et ei saaks
        //lisada negattivse arvu last.
        [Fact]
        public async Task ShouldNot_CreateKindergartenWithNegativeChildrenCount_WhenChildrenCountNegative()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData(true);
            dto.ChildrenCount -= (dto.ChildrenCount * 2);

            //tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //kontroll
            Assert.True(result.ChildrenCount > 0);
        }

        //test mis kontrollib, et lastearv on suurem kui 3,
        //service ei tohi lisada sellest vähema arvuga objekti, service
        //võib selle probleemi lahendada ükskõik kuidas
        [Fact]
        public async Task ShouldNot_CreateKindergarten_WhenChildrenCountIsThreeOrLess()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.ChildrenCount = 0;
            //tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);
            //kontroll
            Assert.True(result.ChildrenCount > 3);
        }

        [Fact]
        public async Task Should_RemoveKindergartenFromDatabase_WhenKindergartenIsDeleted()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();

            //tegevus
            var createdKindergarten = await Svc<IKindergartenServices>().Create(dto);
            var deletedKindergarten = await Svc<IKindergartenServices>().Delete((Guid)createdKindergarten.Id);
            var result = await Svc<IKindergartenServices>().DetailAsync((Guid)createdKindergarten.Id);

            //kontrollimine
            Assert.Equal(createdKindergarten.Id, deletedKindergarten.Id);
            Assert.Null(result);
        }

        [Fact]
        public async Task ShouldNot_RemoveKindergartenFromDatabase_WhenKindergartenIdIsDifferent()
        {
            var dto = MockKindergartenData();
            var createdKindergarten1 = await Svc<IKindergartenServices>().Create(dto);
            var createdKindergarten2 = await Svc<IKindergartenServices>().Create(dto);
            var deleteResult = await Svc<IKindergartenServices>().Delete((Guid)createdKindergarten2.Id);
            var spaceshipindb = await Svc<IKindergartenServices>().DetailAsync((Guid)createdKindergarten1.Id);

            //kontroll
            Assert.NotNull(spaceshipindb);
            Assert.NotEqual(deleteResult.Id, createdKindergarten1.Id);
            Assert.Equal(createdKindergarten1, spaceshipindb);
        }

        //------------------------------------------------------------------------------------------

        //Nüüd algavad lisa 15 teste, eelmised olid koos tehtud testid, mis olid mõeldud Spaceshipile, aga lihstalt ümbernimetatud, et sobiks Kindergartenile.

        [Fact]
        //Kontrollime, et kui string elemendis on olemas number, siis ei tohiks lasteaed tekkida.
        public async Task ShouldNot_CreateKindergarten_WhenAnyStringElementContainsANumber()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();

            dto.TeacherName = "Wh33Zi3 M4C ChIzz13"; //My creativity when it comes to names is....questionable.

            //tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);



            //kontroll
            Assert.Null(result);
        }

        [Fact]
        //Kontrollime, et kui ükski element on null, siis ei tohiks tekkida lasteaed.
        public async Task ShouldNot_CreateKindergarten_WhenAnyOfTheElementsAreNull()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.TeacherName = null;

            //tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //kontroll
            Assert.Null(result);


        }

        [Fact]
        //Kontrollime, et kui ükski element on null, siis ei tohiks uuendada lasteaeda.
        public async Task ShouldNot_UpdateKindergarten_WhenAnyOfTheElementsAreNull()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();


            //tegevus
            var createKindergarten = await Svc<IKindergartenServices>().Create(dto);

            KindergartenDto WhereIsMyStringElementDto = MockKindergartenDataWithAMissingStringElement(); // "Honey, where is my super suit?"
            var result = await Svc<IKindergartenServices>().Update(WhereIsMyStringElementDto);

            //kontroll
            Assert.Null(result);
        }

        [Fact]
        //Kontrollime, et kui string elemendid on kõik sama tulemusega, siis peaks lasteaed ikkagi uuendada saada.
        public async Task Should_UpdateKindergarten_WhenStringElementsAreTheSame()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();


            //tegevus
            var createKindergarten = await Svc<IKindergartenServices>().Create(dto);

            KindergartenDto EveryStringIsTheSameWhatIsGoingOnDto = MockKindergartenDataWithSameStringInEveryStringElement(); //Perfect for ppl like me who have terrible creativity when it comes to names.
            var result = await Svc<IKindergartenServices>().Update(EveryStringIsTheSameWhatIsGoingOnDto);

            //kontroll
            Assert.Equal(EveryStringIsTheSameWhatIsGoingOnDto.GroupName, result.GroupName);
            Assert.Equal(EveryStringIsTheSameWhatIsGoingOnDto.TeacherName, result.TeacherName);
            Assert.Equal(EveryStringIsTheSameWhatIsGoingOnDto.KindergartenName, result.KindergartenName);
        }

        [Fact]
        //Kontrollime, et kui laste loetlus on negatiivne, siis ei tohiks lasteaed uuendada saada.
        public async Task ShouldNot_UpdateKindergarten_WhenChildrenCountNegative()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();


            //tegevus
            var createKindergarten = await Svc<IKindergartenServices>().Create(dto);

            KindergartenDto NegativeChildrenCountDontMakeSenseHereDto = MockKindergartenDataWithNegativeChildrenCount();
            var result = await Svc<IKindergartenServices>().Update(NegativeChildrenCountDontMakeSenseHereDto);

            //kontroll
            Assert.Null(result);
        }


        [Fact]
        //Kontrollime, et kui laste loetlus on 3 või vähem, siis ei tohi uuendada lasteaeda.
        public async Task ShouldNot_UpdateKindergarten_WhenChildrenCountIsThreeOrLess()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();

            //tegevus
            var createKindergarten = await Svc<IKindergartenServices>().Create(dto);
            KindergartenDto updoot = new KindergartenDto()
            {
                Id = createKindergarten.Id,
                GroupName = "WAZZZAAAAA",
                ChildrenCount = 2,
                TeacherName = "Scatman",
                KindergartenName = "AYAYAYAYA"

            };
            var updoot_result = await Svc<IKindergartenServices>().Update(updoot);

            //kontroll
            Assert.Null(updoot_result);
        }

        [Fact]
        //Kontrollime, et kui ID on tühi, siis ei tohi kustutada lasteaeda.
        public async Task ShouldNot_RemoveKindergarten_WhenIdIsEmpty()
        {
            //ülesseade
            var guid = Guid.Empty;

            //tegevus
            var result = await Svc<IKindergartenServices>().Delete(guid);

            //kontroll
            Assert.Null(result);
        }

        [Fact]
        //Kontrollime, kui ükski string elemendis on olemas number, siis ei tohiks uuendada lasteaeda.
        public async Task ShouldNot_UpdateKindergarten_WhenAnyStringElementContainsANumber()
        {

            //ülesseade
            KindergartenDto dto = MockKindergartenData();



            //tegevus
            var createKindergarten = await Svc<IKindergartenServices>().Create(dto);
            KindergartenDto updoot = MockKindergartenData();
            updoot.Id = createKindergarten.Id;
            var updoot_result = await Svc<IKindergartenServices>().Update(updoot);


            //kontroll
            Assert.Null(updoot_result);
        }
        [Fact]
        //Kontrollime, et kui string elementidel on kõik sama tulemus, siis tohiks tekkitada lasteaeda.
        public async Task Should_CreateKindergarten_WhenStringElementsAreTheSame()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.TeacherName = "Hello";
            dto.GroupName = dto.TeacherName;
            dto.KindergartenName = dto.TeacherName;


            //tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);



            //kontroll
            Assert.Equal(dto.GroupName, result.GroupName);
            Assert.Equal(dto.TeacherName, result.TeacherName);
            Assert.Equal(dto.KindergartenName, result.KindergartenName);
        }

        [Fact]
        //Kontrollime, et kui gruppinimi on string, siis peaks lasteaed tekkima.
        public async Task Should_CreateKindergarten_WhenGroupNameIsAString()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.GroupName = "This test is literally pointless, but at the same time im basically just throwing stuff at a wall when it comes to ideas.";

            //tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);

            //kontroll
            Assert.NotNull(result);

        }

        [Fact]
        //Kontrollime, et kui gruppinimi on string, siis peaks lasteaed uuendama.
        public async Task Should_UpdateKindergarten_WhenGroupNameIsAString()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.GroupName = "This test is literally pointless, but at the same time im basically just throwing stuff at a wall when it comes to ideas.";

            //tegevus
            var createKindergarten = await Svc<IKindergartenServices>().Create(dto);
            KindergartenDto updoot = MockKindergartenData();
            updoot.Id = dto.Id;
            updoot.GroupName = "Same thing, but in update this time.";
            var result = await Svc<IKindergartenServices>().Update(updoot);

            //kontroll
            Assert.NotNull(result);

        }

        [Fact]
        //Kontrollime, et kui laste loetlus on täisarv, siis peaks tekkima lasteaed.
        public async Task Should_CreateKindergarten_WhenChildrenCountIsAnInterger()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.ChildrenCount = 5;

            //tegevus
            var result = await Svc<IKindergartenServices>().Create(dto);




            //kontroll
            Assert.NotNull(result);

        }

        [Fact]
        //Kontrollime, et kui laste loetlus on täisarv, siis peaks lastead uuendama.
        public async Task Should_UpdateKindergarten_WhenChildrenCountIsAnInterger()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();


            //tegevus
            var createKindergarten = await Svc<IKindergartenServices>().Create(dto);
            KindergartenDto updoot = MockKindergartenData();
            updoot.Id = dto.Id;
            updoot.ChildrenCount = 5;
            var result = await Svc<IKindergartenServices>().Update(updoot);




            //kontroll
            Assert.NotNull(result);

        }

        [Fact]
        //Kontrollime, et kui laste loetlus on million või suurem, siis ei tohiks tekkida lasteaed.
        public async Task ShouldNot_CreateKindergarten_WhenChildrenCountIsAtAMillionOrHigherRange()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();
            dto.ChildrenCount = 1000000;

            //tegevus


            var result = await Svc<IKindergartenServices>().Create(dto);




            //kontroll
            Assert.Null(result);

        }

        [Fact]
        //Kontrollime, et kui laste loetlus on million või suurem, siis ei tohiks uuendada lasteaeda.
        public async Task ShouldNot_UpdateKindergarten_WhenChildrenCountIsAtAMillionOrHigherRange()
        {
            //ülesseade
            KindergartenDto dto = MockKindergartenData();


            //tegevus


            var createKindergarten = await Svc<IKindergartenServices>().Create(dto);
            KindergartenDto updoot = MockKindergartenData();
            updoot.Id = dto.Id;
            dto.ChildrenCount = 1000000;
            var result = await Svc<IKindergartenServices>().Update(updoot);




            //kontroll
            Assert.Null(result);

        }

        /* üleval testid, all abimeetodid */

        private KindergartenDto MockKindergartenDataWithNegativeChildrenCount()
        {
            return new KindergartenDto
            {
                GroupName = "Something Something",
                ChildrenCount = -4,
                KindergartenName = "My Creativity With Names is Terrible",
                TeacherName = "Lamar Davis", //God help us if that were the case.
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue,
            };
        }

        private KindergartenDto MockKindergartenDataWithSameStringInEveryStringElement()
        {
            return new KindergartenDto
            {
                GroupName = "Hello",
                ChildrenCount = 5,
                KindergartenName = "Hello",
                TeacherName = "Hello",
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue,
            };
        }

        private KindergartenDto MockKindergartenDataWithAMissingStringElement()
        {
            return new KindergartenDto
            {
                GroupName = "Insert some name here",
                ChildrenCount = 5,
                KindergartenName = "Same as before",
                TeacherName = null, 
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue,
            };
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

        

        





    }
}