using System;
using System.Collections.Generic;
using System.Text;
using TARge25Shop.Core.Dto;
using TARge25Shop.Core.ServiceInterface;
using Xunit;

namespace TARge25Shop.SpaceshipTest
{
    public class SpaceshipTest : TestBase
    {

        [Fact] //Fact tähistab ära ühte testi xUnit raamistikus
        //  1 - Kirjeldakse ära, kas test on tavaline või negatiivne. 
        //  2 - Kirjeldatakse ära mida parasjagu üritatakse testialuse objektiga teha
        // 3 - Mis tingimustel tulemust kontrollitakse, peale tegevust
        //
        // Selles testis kontrollitakse et (2) Kosmoselaeva lisamisel
        // (1) Ei tohiks (3) saadud tulemus olla tühi. Jälgi seda sõnastusviisi:
        //
        //            1       2             3
        //           \/       \/           \/
        public async Task ShouldNot_AddEmptySpaceShip_WhenResultIsReturned()
        {
            //Ülesseade
            SpaceshipDto dto = new SpaceshipDto()
            {
                Name = "Nimi",
                ShipType = "Lendav taldrik",
                Crew = 67,
                EnginePower = 69, //hobujõudu siis
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            //tegutsemine
            var result = await Svc<ISpaceshipServices>().Create(dto);

            //kontroll
            Assert.NotNull(result);
        }

       
        //  1 - Kirjeldakse ära, kas test on tavaline või negatiivne. 
        //  2 - Kirjeldatakse ära mida parasjagu üritatakse testialuse objektiga teha
        // 3 - Mis tingimustel tulemust kontrollitakse, peale tegevust
        //
        // Selles testis kontrollitakse et (2) Spaceshipi päring andmebaasist
        // (1) ei tohiks tagastada objektid (3) kui ID-d ei ole samad:

        
        [Fact]
        public async Task ShouldNot_GetSpaceShipById_WhenIdNotEqual()
        {
            //ülesseade
            Guid wrongGuid = Guid.NewGuid();
            Guid goodGuid = Guid.Parse("");

            //tegevus
            await Svc<ISpaceshipServices>().DetailAsync(goodGuid);

            //kontroll
            Assert.NotEqual(wrongGuid, goodGuid);
        }
        //Seleta kodus lahti, nagu eelnevate testide laused, eesti keelde, selle testi oma ka....
        [Fact]
        public async Task Should_GetSpaceshipById_WhenGuidIsEqual()
        {
            //ülesseade
            Guid databaseGuid = Guid.Parse("");
            Guid seekGuid = Guid.Parse("");

            //tegevus
            await Svc<ISpaceshipServices>().DetailAsync(seekGuid);

            //kontroll
            Assert.Equal(databaseGuid, seekGuid);
        }
        //Seleta kodus lahti, nagu eelnevate testide laused, eesti keelde, selle testi oma ka....
        [Fact]
        public async Task Should_SpaceshipDeletedById_WhenReturnedResultIsEqual()
        {
            //ülesseade
            SpaceshipDto dto = MockSpaceShipData();

            //tegevus
            var addSpaceship = await Svc<ISpaceshipServices>().Create(dto);
            var deleteSpaceship = await Svc<ISpaceshipServices>().Delete((Guid)addSpaceship.Id);

            //kontroll
            Assert.Equal(addSpaceship.Id, deleteSpaceship.Id);
        }

        private SpaceshipDto MockSpaceShipData(bool isOneOrTwo = false)
        {
            if (isOneOrTwo == false)
            {
                return new SpaceshipDto
                {
                    Name = "Nimi",
                    ShipType = "Lendav taldrik",
                    Crew = 67,
                    EnginePower = 69, //hobujõudu siis
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
            else
            {
                return new SpaceshipDto
                {
                    Name = "Wow",
                    ShipType = "Bowling Ball",
                    Crew = 111,
                    EnginePower = 632, //hobujõudu siis
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };
            }
        }   
    }
}
