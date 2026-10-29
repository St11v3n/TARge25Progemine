using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace TARge25Shop.SeleniumTesting
{
    //järjesti klass, mis omab omadusi mille järgi teste saab järjestada
    public class OrderAttribute : Attribute
    {
        public int Value { get; } //väärtus
        public OrderAttribute(int value) //lihtne omistuskonstruktor
        {
            Value = value;
        }
    }

    //Xunit järjesti mis implementeerib ITestCaseOrdereri liidesest "OrderTestCases" meetodi, mis on see mis testide käivitamisel
    //järjestab nad vastavalt "Order" omadusele
    public class PriorityOrderer : ITestCaseOrderer
    {
        //meetod võtab sisse kõik test cased
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            //meetod tagastab kõik test cased järjestatuna arendaja defineeritud järjesti järgi.
            return testCases.OrderBy(test =>
            {
                //leitakse omadus                                               omaduse tüüp
                var method = test.TestMethod.Method.GetCustomAttributes(typeof(OrderAttribute)).FirstOrDefault();
                //kui ei leita
                if (method == null)
                {
                    //antakse maksimaalne väärtus, ehk test teostatakse kohe, ega ei
                    //panda teiste pontensiaalselt järjekorras olevate testide vahele
                    return int.MaxValue;
                }
                //kui leitakse
                else
                {
                    //siis tagastatakse omaduse "Order" väärtus
                    return method.GetNamedArgument<int>(nameof(OrderAttribute.Value));
                }
            }).ToList(); //pannakse loendisse, ToList on linq meetod.
        }
    }

    //enne testi klassi kirjeldatakse ära mille järgi testid sorteeritakse
    //tunnuse "TestCaseOrderer"i järgi. Sulgude vahel esimene väärtus on klassi asukoht ise, teine väärtus on nimeruum kus klass asub.
    [TestCaseOrderer("TARge25Shop.SeleniumTesting.PriorityOrderer", "TARge25Shop.SeleniumTesting")]
    public class KindergartenFrontendTest
    {
        public static string thisRunGuid { get; set; }

        public static string thisRunGuid1 { get; set; }

        public static string thisRunGuid2 { get; set; }



        //See test testib, et navigeerides koostmise lehele ning lisades unikaalse lasteaia koos õige andmetega, minnakse tagasi indeksi vaatele.
        [Fact]
        [Order(1)]
        public void Should_NavigateToCreate_AddUniqueKindergarten_WithCorrectData_ReturnToIndex()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSIndex();

            //tegevus
            IWebElement CreateInIndex = driver.FindElement(By.Id("CreateInIndex"));
            CreateInIndex.Click();

            //sisestatavad andmed, siin tagastub ka kasutatud nimi
            string concreteName = InsertKindergartenData_ReturnName(driver, false);
            thisRunGuid = concreteName;

            IWebElement cu_CreateKindergarten = driver.FindElement(By.Id("CU_CreateKindergarten"));
            cu_CreateKindergarten.Click();

            Thread.Sleep(1000);



            //andmete kogumine pärast reloadi
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);

            string kindergartenNameData = "";
            string kindergartenGroupNameData = "";
            string kindergartenTeacherNameData = "";
            string kindergartenChildrenCountData = "";
            string kindergartenBuiltAtData = "";

            bool doesItExist = false;
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexNameKindergarten")).Text == thisRunGuid)
                {
                    doesItExist = true;
                    kindergartenNameData = thisRunGuid;
                    kindergartenGroupNameData = row.FindElement(By.Id("IndexGroupNameKindergarten")).Text;
                    kindergartenTeacherNameData = row.FindElement(By.Id("IndexTeacherNameKindergarten")).Text;
                    kindergartenChildrenCountData = row.FindElement(By.Id("IndexChildrenCountKindergarten")).Text;
                    kindergartenBuiltAtData = row.FindElement(By.Id("IndexBuiltDateKindergarten")).Text;
                    break;
                }
            }
            driver.Quit();

            //kontroll

            Assert.True(doesItExist == true);
            Assert.Equal(kindergartenNameData, thisRunGuid);
            Assert.True(kindergartenGroupNameData == "Something something");
            Assert.Equal(kindergartenChildrenCountData, "9");
            Assert.Equal(kindergartenTeacherNameData, "Yusuf Onion");
            Assert.True((thisRunGuid == kindergartenNameData)
                && ("Something something" == kindergartenGroupNameData)
                && ("9" == kindergartenChildrenCountData)
                && ("Yusuf Onion" == kindergartenTeacherNameData));
        }

        //See test testib, et navigeerides detailide lehele spetsiifilise valitud lasteaiale näidatakse varem näidatud õiged andmeid, mis oli juba nähtav indeksi vaates.
         [Fact]
         [Order(2)]
         public void Should_NavigateToDetails_OfThisKindergarten_WithPreviouslyCorrectData()
         {
             //ülesseade
             IWebDriver driver = SetupAndNavigateSIndex();

             //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
             //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
             var table = driver.FindElement(By.Id("IndexTable"));
             List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
             rows.RemoveAt(0);
             bool isObjectFound = false;

             //tsükkel käib kõik read läbi            
             foreach (var row in rows)
             {
                
                var locatedelement = row.FindElement(By.Id("IndexNameKindergarten"));
                 if (locatedelement.Text == thisRunGuid)
                 {
                    isObjectFound = true;
                    //siis vajuta selles reas asuvat details nuppu

                    var rowelement = row.FindElement(By.Id("IndexActionsKindergarten"));
                    var button = rowelement.FindElement(By.Id("IndexKindergartenDetails"));
                    button.Click();
                    //pärast õiget vajutust, tsükkel katkestatakse
                    //arvuti tudub
                    //Thread.Sleep(500);
                    break;
                 }
             }
             //andmete kogumine pärast reloadi
             IWebElement details_KindergartenId = driver.FindElement(By.Id("Details_KindergartenId"));
             var details_kindergartenid = details_KindergartenId.Text;

             IWebElement details_KindergartenName = driver.FindElement(By.Id("Details_KindergartenName"));
             var details_kindergartenname = details_KindergartenName.Text;

             IWebElement details_KindergartenGroupName = driver.FindElement(By.Id("Details_KindergartenGroupName"));
             var details_kindergartengroupname = details_KindergartenGroupName.Text;

            IWebElement details_KindergartenTeacherName = driver.FindElement(By.Id("Details_KindergartenTeacherName"));
            var details_kindergartenteachername = details_KindergartenTeacherName.Text;

            IWebElement details_KindergartenChildrenCount = driver.FindElement(By.Id("Details_KindergartenChildrenCount"));
             var details_kindergartenchildrencount = details_KindergartenChildrenCount.Text;

            

             driver.Quit();
             //kontroll
             Assert.True(isObjectFound);
             Assert.NotNull(details_kindergartenid);
             // et ei oleks tühi kontrollid kõikidele andmetele
             Assert.NotEmpty(details_kindergartenname);
             Assert.NotNull(details_kindergartengroupname);
             Assert.True(!string.IsNullOrEmpty(details_kindergartenchildrencount));
             Assert.Equal(thisRunGuid, details_kindergartenname);
             Assert.Equal("Something something", details_kindergartengroupname);
             Assert.Equal("9", details_kindergartenchildrencount);
             Assert.Equal("Yusuf Onion", details_kindergartenteachername);
             Assert.True(int.Parse(details_kindergartenchildrencount).GetType() == typeof(int));

         }
         //Test, mis testib, et uuendatakse detailid spetsiifilise valitud lasteaia jaoks uue andmetega.
         [Fact]
         [Order(3)]
         public void Should_UpdateDetails_OfThisKindergarten_WithNewData()
         {
             //ülesseade
             IWebDriver driver = SetupAndNavigateSIndex();

             //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
             //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
             var table = driver.FindElement(By.Id("IndexTable"));
             List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
             rows.RemoveAt(0);
             bool doesItExist = false;
             //tsükkel käib kõik read läbi

             foreach (var row in rows)
             {

                 var locatedelement = row.FindElement(By.Id("IndexNameKindergarten"));
                 if (locatedelement.Text == thisRunGuid)
                 {
                     doesItExist = true;
                     //siis vajuta selles reas asuvat details nuppu
                     var rowelement = row.FindElement(By.Id("IndexActionsKindergarten"));
                     var button = rowelement.FindElement(By.Id("IndexKindergartenUpdate"));
                     button.Click();
                     //pärast õiget vajutust, tsükkel katkestatakse
                     //arvuti tudub
                     Thread.Sleep(500);
                     break;
                 }
             }
             //sisestatavad andmed
             InsertKindergartenData(driver, thisRunGuid, true);

             IWebElement cu_CreateKindergarten = driver.FindElement(By.Id("CU_UpdateKindergarten"));
             cu_CreateKindergarten.Click();

             //andmete kogumine pärast reloadi
             var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
             List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
             rowsOfTable.RemoveAt(0);
             string kindergartenNameData = "";
             string kindergartenGroupNameData = "";
             string kindergartenTeacherNameData = "";
             string kindergartenChildrenCountData = "";
             string kindergartenBuiltAtData = "";
             doesItExist = false;
             foreach (var row in rowsOfTable)
             {
                 if (row.FindElement(By.Id("IndexNameKindergarten")).Text == thisRunGuid)
                 {
                     doesItExist = true;
                     kindergartenNameData = thisRunGuid;
                     kindergartenGroupNameData = row.FindElement(By.Id("IndexGroupNameKindergarten")).Text;
                     kindergartenTeacherNameData = row.FindElement(By.Id("IndexTeacherNameKindergarten")).Text;
                     kindergartenChildrenCountData = row.FindElement(By.Id("IndexChildrenCountKindergarten")).Text;
                     kindergartenBuiltAtData = row.FindElement(By.Id("IndexBuiltDateKindergarten")).Text;
                     break;
                 }
             }
             driver.Quit();

             ////arvuti tudub
             //Thread.Sleep(1000);
             Assert.True(doesItExist);
             Assert.NotNull(kindergartenNameData);
             Assert.Equal(thisRunGuid, kindergartenNameData);
             Assert.NotEqual(kindergartenGroupNameData, "Something something");
             Assert.True(kindergartenGroupNameData == "Group with no name.");
             Assert.False(string.IsNullOrEmpty(kindergartenGroupNameData));
             Assert.NotEqual(kindergartenTeacherNameData, "Yusuf Onion");
             Assert.True(kindergartenTeacherNameData == "Rip Van Winkle");
             Assert.False(string.IsNullOrEmpty(kindergartenTeacherNameData));
             Assert.NotEqual(kindergartenChildrenCountData, "9");
             Assert.True(kindergartenChildrenCountData == "7");
             Assert.False(string.IsNullOrEmpty(kindergartenChildrenCountData));
             //kontrolli kõik teised andmed ka üle põhjalikult.

        }

         //Test testib, et kustutatakse spetsiifilise valitud lasteaia rakendusest kustutamise vaate kaudu.
         [Fact]
         [Order(4)]
         public void Should_DeleteThisKindergarten_FromAppVIADeleteView()
         {
             //ülesseade
             IWebDriver driver = SetupAndNavigateSIndex();

             //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
             //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
             var table = driver.FindElement(By.Id("IndexTable"));
             List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
             rows.RemoveAt(0);
             bool doesItExist = false;
             //tsükkel käib kõik read läbi

             foreach (var row in rows)
             {

                 var locatedelement = row.FindElement(By.Id("IndexNameKindergarten"));
                 if (locatedelement.Text == thisRunGuid)
                 {
                     doesItExist = true;
                     //siis vajuta selles reas asuvat details nuppu
                     var rowelement = row.FindElement(By.Id("IndexActionsKindergarten"));
                     var button = rowelement.FindElement(By.Id("IndexKindergartenDelete"));
                     button.Click();
                     //pärast õiget vajutust, tsükkel katkestatakse
                     //arvuti tudub
                     Thread.Sleep(500);
                     break;
                 }
             }
             var clickthisbutton = driver.FindElement(By.Id("Delete_Delete"));
             clickthisbutton.Click();
             bool isDeleted = false;
             table = driver.FindElement(By.Id("IndexTable"));
             rows = table.FindElements(By.TagName("tr")).ToList();
             rows.RemoveAt(0);
             foreach (var row in rows)
             {
                 var nameElement = row.FindElement(By.Id("IndexNameKindergarten")).Text;
                 if (nameElement == thisRunGuid)
                 {
                     isDeleted = false;
                     break;
                 }
                 else
                 {
                     isDeleted = true;
                 }
             }

             Assert.False(isDeleted);
             Assert.True(!isDeleted);
         }
         //See test testib loomise vaate navigeerimisele, siis luuakse üks lasteaed, siis minnakse tagasi ineksi vaatele ja pärast
         //navigeeritakse uuesti loomise vaatele et luua teist lasteaeda, ning minnakse tagasi jälle indeksi vaatele, kontrollitakse et mülematel oleks õiged andmed.
         [Fact]
         [Order(5)]

         public void Should_NavigateToCreate_AddMultipleKindergartens_WithCorrectData_ReturnToIndex()
         {
             //ülesseade
             IWebDriver driver = SetupAndNavigateSIndex();

             //tegevus
             IWebElement createInIndex1 = driver.FindElement(By.Id("CreateInIndex"));
             createInIndex1.Click();

             //sisestatavad andmed, siin tagastub ka kasutatud nimi
             string concreteName1 = InsertKindergartenData_ReturnName(driver, false);
             thisRunGuid1 = concreteName1;

             IWebElement cu_CreateKindergarten1 = driver.FindElement(By.Id("CU_CreateKindergarten"));
             cu_CreateKindergarten1.Click();



             IWebElement createInIndex2 = driver.FindElement(By.Id("CreateInIndex"));
             createInIndex2.Click();

             string concreteName2 = InsertKindergartenData_ReturnName(driver, true);

             thisRunGuid2 = concreteName2;

             IWebElement cu_CreateKindergarten2 = driver.FindElement(By.Id("CU_CreateKindergarten"));
             cu_CreateKindergarten2.Click();

             //andmete kogumine pärast reloadi
             var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
             List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
             rowsOfTable.RemoveAt(0);
             string kindergartenGroupNameData1 = "";
             string kindergartenNameData1 = "";
             string kindergartenTeacherNameData1 = "";
             string kindergartenChildrenCountData1 = "";
             string kindergartenBuiltAtData1 = "";

             string kindergartenGroupNameData2 = "";
             string kindergartenNameData2 = "";
             string kindergartenTeacherNameData2 = "";
             string kindergartenChildrenCountData2 = "";
             string kindergartenBuiltAtData2 = "";

             bool doesItExist1 = false;
             bool doesItExist2 = false;
             foreach (var row in rowsOfTable)
             {
                 if (row.FindElement(By.Id("IndexNameKindergarten")).Text == thisRunGuid1)
                 {
                     doesItExist1 = true;
                     kindergartenNameData1 = thisRunGuid1;
                     kindergartenGroupNameData1 = row.FindElement(By.Id("IndexGroupNameKindergarten")).Text;
                     kindergartenTeacherNameData1 = row.FindElement(By.Id("IndexTeacherNameKindergarten")).Text;
                     kindergartenChildrenCountData1 = row.FindElement(By.Id("IndexChildrenCountKindergarten")).Text;
                     kindergartenBuiltAtData1 = row.FindElement(By.Id("IndexBuiltDateKindergarten")).Text;

                 }

                 if (row.FindElement(By.Id("IndexNameKindergarten")).Text == thisRunGuid2)
                 {
                     doesItExist2 = true;
                     kindergartenNameData2 = thisRunGuid2;
                     kindergartenGroupNameData2 = row.FindElement(By.Id("IndexGroupNameKindergarten")).Text;
                     kindergartenTeacherNameData2 = row.FindElement(By.Id("IndexTeacherNameKindergarten")).Text;
                     kindergartenChildrenCountData2 = row.FindElement(By.Id("IndexChildrenCountKindergarten")).Text;
                     kindergartenBuiltAtData2 = row.FindElement(By.Id("IndexBuiltDateKindergarten")).Text;



                 }
             }
             driver.Quit();

             //kontroll

            Assert.True(true == doesItExist1);
            Assert.True(true == doesItExist2);
            Assert.Equal(kindergartenNameData1, thisRunGuid1);
            Assert.Equal(kindergartenNameData2, thisRunGuid2);
            Assert.True(kindergartenGroupNameData1 == "Group with no name.");
            Assert.True(kindergartenGroupNameData2 == "Something something");
            Assert.Equal(kindergartenChildrenCountData1, "7");
            Assert.Equal(kindergartenChildrenCountData2, "9");
            Assert.True((thisRunGuid1 == kindergartenNameData1)
                && ("Group with no name." == kindergartenGroupNameData1)
                && ("7" == kindergartenChildrenCountData1));
            Assert.True((thisRunGuid2 == kindergartenNameData2)
                && ("Something something" == kindergartenGroupNameData2)
                && ("9" == kindergartenChildrenCountData2));


        }
        //See test testib, et minnakse detailide vaatele ühele lasteaiale ning kontrollitakse et selle andmed on sama mis on nähtav indeks vaates, siis see läheb tagasi indeks
        //vaatele ja uuesti läheb detailide vaatele teise lasteaia jaoks ning ka kontrollitakse et selle andmed on sama mis indeksi vaates näha on.

        [Fact]
        [Order(6)]

        public void Should_NavigateToDetails_ForMultipleKindergartens_WithPreviouslyCorrectData()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSIndex();

            //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
            //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool isObjectFound = false;

            //tsükkel käib kõik read läbi            
            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameKindergarten"));
                if (locatedelement.Text == thisRunGuid1)
                {
                    //siis vajuta selles reas asuvat details nuppu
                    var rowelement = row.FindElement(By.Id("IndexActionsKindergarten"));
                    var button = rowelement.FindElement(By.Id("IndexKindergartenDetails"));
                    button.Click();
                    //pärast õiget vajutust, tsükkel katkestatakse
                    //arvuti tudub
                    Thread.Sleep(500);
                    break;

                }



            }

            //andmete kogumine pärast reloadi
            IWebElement details_KindergartenId1 = driver.FindElement(By.Id("Details_KindergartenId"));
            var details_kindergartenid1 = details_KindergartenId1.Text;

            IWebElement details_KindergartenName1 = driver.FindElement(By.Id("Details_KindergartenName"));
            var details_kindergartenname1 = details_KindergartenName1.Text;

            IWebElement details_KindergartenGroupName1 = driver.FindElement(By.Id("Details_KindergartenGroupName"));
            var details_kindergartengroupname1 = details_KindergartenGroupName1.Text;

            IWebElement details_KindergartenChildrenCount1 = driver.FindElement(By.Id("Details_KindergartenChildrenCount"));
            var details_kindergartenchildrencount1 = details_KindergartenChildrenCount1.Text;

            IWebElement details_KindergartenTeacherName1 = driver.FindElement(By.Id("Details_KindergartenTeacherName"));
            var details_kindergartenteachername1 = details_KindergartenTeacherName1.Text;


            IWebElement navigateToIndexViewUsingTheBackButton = driver.FindElement(By.Id("Details_Back"));
            navigateToIndexViewUsingTheBackButton.Click();

            table = driver.FindElement(By.Id("IndexTable"));

            rows = table.FindElements(By.TagName("tr")).ToList();

            rows.RemoveAt(0);

            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameKindergarten"));
                if (locatedelement.Text == thisRunGuid2)
                {
                    //siis vajuta selles reas asuvat details nuppu
                    var rowelement = row.FindElement(By.Id("IndexActionsKindergarten"));
                    var button = rowelement.FindElement(By.Id("IndexKindergartenDetails"));
                    button.Click();
                    //pärast õiget vajutust, tsükkel katkestatakse
                    //arvuti tudub
                    Thread.Sleep(500);
                    break;

                }


            }

            //andmete kogumine pärast reloadi
            IWebElement details_KindergartenId2 = driver.FindElement(By.Id("Details_KindergartenId"));
            var details_kindergartenid2 = details_KindergartenId2.Text;

            IWebElement details_KindergartenName2 = driver.FindElement(By.Id("Details_KindergartenName"));
            var details_kindergartenname2 = details_KindergartenName2.Text;

            IWebElement details_KindergartenGroupName2 = driver.FindElement(By.Id("Details_KindergartenGroupName"));
            var details_kindergartengroupname2 = details_KindergartenGroupName2.Text;

            IWebElement details_KindergartenChildrenCount2 = driver.FindElement(By.Id("Details_KindergartenChildrenCount"));
            var details_kindergartenchildrencount2 = details_KindergartenChildrenCount2.Text;

            IWebElement details_KindergartenTeacherName2 = driver.FindElement(By.Id("Details_KindergartenTeacherName"));
            var details_kindergartenteachername2 = details_KindergartenTeacherName2.Text;

            driver.Quit();
            //kontroll
            Assert.True(isObjectFound);
            Assert.NotNull(details_kindergartenid1);
            Assert.NotNull(details_kindergartenid2);
            // et ei oleks tühi kontrollid kõikidele andmetele
            Assert.NotEmpty(details_kindergartenname1);
            Assert.NotEmpty(details_kindergartenname2);
            Assert.NotNull(details_kindergartengroupname1);
            Assert.NotNull(details_kindergartengroupname2);
            Assert.True(!string.IsNullOrEmpty(details_kindergartenchildrencount1));
            Assert.True(!string.IsNullOrEmpty(details_kindergartenchildrencount2));
            Assert.Equal(thisRunGuid1, details_kindergartenname1);
            Assert.Equal(thisRunGuid2, details_kindergartenname2);
            Assert.Equal("Group with no name.", details_kindergartengroupname1);
            Assert.Equal("Something something", details_kindergartengroupname2);
            Assert.Equal("7", details_kindergartenchildrencount1);
            Assert.Equal("9", details_kindergartenchildrencount2);
            Assert.Equal("Rip Van Winkle", details_kindergartenteachername1);
            Assert.Equal("Yusuf Onion", details_kindergartenteachername2);
            Assert.True(int.Parse(details_kindergartenchildrencount1).GetType() == typeof(int));
            Assert.True(int.Parse(details_kindergartenchildrencount2).GetType() == typeof(int));
        }

        //See test testib mitme lasteaiade andmete uuendamist, need uued andmed on sama mõlematele lasteaiadele.
        [Fact]
        [Order(7)]

        public void Should_UpdateDetails_ForBothKindergartens_WithNewData()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSIndex();

            //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
            //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;
            //tsükkel käib kõik read läbi

            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameKindergarten"));
                if (locatedelement.Text == thisRunGuid1)
                {
                    doesItExist = true;
                    //siis vajuta selles reas asuvat details nuppu
                    var rowelement = row.FindElement(By.Id("IndexActionsKindergarten"));
                    var button = rowelement.FindElement(By.Id("IndexKindergartenUpdate"));
                    button.Click();
                    //pärast õiget vajutust, tsükkel katkestatakse
                    //arvuti tudub
                    Thread.Sleep(500);
                    break;
                }
            }
            //sisestatavad andmed
            InsertKindergartenDataForTwo(driver, thisRunGuid1);

            IWebElement cu_CreateKindergarten = driver.FindElement(By.Id("CU_UpdateKindergarten"));
            cu_CreateKindergarten.Click();

            //andmete kogumine pärast reloadi
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            string kindergartenNameData1 = "";
            string kindergartenGroupNameData1 = "";
            string kindergartenChildrenCountData1 = "";
            string kindergartenTeacherNameData1 = "";
            string kindergartenBuiltAtData1 = "";
            doesItExist = false;

            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexNameKindergarten")).Text == thisRunGuid1)
                {
                    doesItExist = true;
                    kindergartenNameData1 = thisRunGuid1;
                    kindergartenGroupNameData1 = row.FindElement(By.Id("IndexGroupNameKindergarten")).Text;
                    kindergartenChildrenCountData1 = row.FindElement(By.Id("IndexChildrenCountKindergarten")).Text;
                    kindergartenBuiltAtData1 = row.FindElement(By.Id("IndexBuiltDateKindergarten")).Text;
                    break;
                }
            }

            table = driver.FindElement(By.Id("IndexTable"));
            rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            doesItExist = false;
            //tsükkel käib kõik read läbi
            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameKindergarten"));
                if (locatedelement.Text == thisRunGuid2)
                {
                    doesItExist = true;
                    //siis vajuta selles reas asuvat details nuppu
                    var rowelement = row.FindElement(By.Id("IndexActionsKindergarten"));
                    var button = rowelement.FindElement(By.Id("IndexKindergartenUpdate"));
                    button.Click();
                    //pärast õiget vajutust, tsükkel katkestatakse
                    //arvuti tudub
                    Thread.Sleep(500);
                    break;
                }
            }

            //sisestatavad andmed
            InsertKindergartenDataForTwo(driver, thisRunGuid2);

            cu_CreateKindergarten = driver.FindElement(By.Id("CU_UpdateKindergarten"));
            cu_CreateKindergarten.Click();

            //andmete kogumine pärast reloadi
            tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            string kindergartenNameData2 = "";
            string kindergartenGroupNameData2 = "";
            string kindergartenChildrenCountData2 = "";
            string kindergartenTeacherNameData2 = "";
            string kindergartenBuiltAtData2 = "";
            doesItExist = false;

            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexNameKindergarten")).Text == thisRunGuid2)
                {
                    doesItExist = true;
                    kindergartenNameData2 = thisRunGuid2;
                    kindergartenGroupNameData2 = row.FindElement(By.Id("IndexGroupNameKindergarten")).Text;
                    kindergartenChildrenCountData2 = row.FindElement(By.Id("IndexChildrenCountKindergarten")).Text;
                    kindergartenBuiltAtData2 = row.FindElement(By.Id("IndexBuiltDateKindergarten")).Text;
                    break;
                }
            }

            driver.Quit();

            
            Assert.True(doesItExist);
            Assert.NotNull(kindergartenNameData1);
            Assert.NotNull(kindergartenNameData2);
            Assert.Equal(thisRunGuid1, kindergartenNameData1);
            Assert.Equal(thisRunGuid2, kindergartenNameData2);
            Assert.False(string.IsNullOrEmpty(kindergartenGroupNameData1));
            Assert.False(string.IsNullOrEmpty(kindergartenGroupNameData2));
            Assert.NotEqual(kindergartenGroupNameData1, "Group with no name.");
            Assert.NotEqual(kindergartenGroupNameData2, "Something something");
            Assert.True(kindergartenGroupNameData1 == "idk");
            Assert.True(kindergartenGroupNameData2 == "idk");
            Assert.False(string.IsNullOrEmpty(kindergartenGroupNameData1));
            Assert.False(string.IsNullOrEmpty(kindergartenGroupNameData2));
            Assert.True(kindergartenChildrenCountData1 == "12");
            Assert.True(kindergartenChildrenCountData2 == "12");
            
            //kontrolli kõik teised andmed ka üle põhjalikult.



        }
        //Test, mis testib, et mitte valitud lasteaed ei kustutatakse pärast valitud lasteaia kustutamist läbi kustutamise vaatest.
        [Fact]
        [Order(8)]

        public void ShouldNot_DeleteANonSelectedKindergarten_FromAppVIADeleteView()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSIndex();

            //otsime üles kõik tabeli read, rida htmli tabelis tähistatakse "tr"iga
            //ICollection<IWebElement> table = driver.FindElements(By.TagName("tr"));
            var table = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            bool doesItExist = false;
            //tsükkel käib kõik read läbi

            foreach (var row in rows)
            {

                var locatedelement = row.FindElement(By.Id("IndexNameKindergarten"));
                if (locatedelement.Text == thisRunGuid1)
                {
                    doesItExist = true;
                    //siis vajuta selles reas asuvat details nuppu
                    var rowelement = row.FindElement(By.Id("IndexActionsKindergarten"));
                    var button = rowelement.FindElement(By.Id("IndexKindergartenDelete"));
                    button.Click();
                    //pärast õiget vajutust, tsükkel katkestatakse
                    //arvuti tudub
                    Thread.Sleep(500);
                    break;
                }
            }
            var clickthisbutton = driver.FindElement(By.Id("Delete_Delete"));
            clickthisbutton.Click();
            bool isAvailable = false;
            table = driver.FindElement(By.Id("IndexTable"));
            rows = table.FindElements(By.TagName("tr")).ToList();
            rows.RemoveAt(0);
            foreach (var row in rows)
            {
                var nameElement = row.FindElement(By.Id("IndexNameKindergarten")).Text;
                if (nameElement == thisRunGuid2)
                {
                    isAvailable = true;
                    break;
                }
                
            }

            Assert.False(!isAvailable);
            Assert.True(isAvailable);
        }

        //abimeetodid käivad siia alla.
        private static IWebDriver SetupAndNavigateSIndex()
        {
            //firefoxi käskiv ja juhtiv draiver
            IWebDriver driver = new FirefoxDriver();
            //aadress millele draiver navigeerib
            driver.Url = "https://localhost:7227/";
            //lehelt otsitav element
            IWebElement navigateToKindergarten = driver.FindElement(By.LinkText("Kindergarten"));
            //selle elemendiga tehtav tegevus
            navigateToKindergarten.Click();
            return driver;
        }





        private string InsertKindergartenData_ReturnName(IWebDriver driver, bool isOneOrTwo = false)
        {
            Guid returnGuid = Guid.NewGuid();
            if (isOneOrTwo)
            {
                IWebElement cu_NameEntryKindergarten = driver.FindElement(By.Id("CU_NameEntryKindergarten"));
                cu_NameEntryKindergarten.Clear();
                cu_NameEntryKindergarten.SendKeys(returnGuid.ToString());
                IWebElement cu_GroupNameEntryKindergarten = driver.FindElement(By.Id("CU_GroupNameEntryKindergarten"));
                cu_GroupNameEntryKindergarten.Clear();
                cu_GroupNameEntryKindergarten.SendKeys("Group with no name.");
                IWebElement cu_ChildrenCountEntryKindergarten = driver.FindElement(By.Id("CU_ChildrenCountEntryKindergarten"));
                cu_ChildrenCountEntryKindergarten.Clear();
                cu_ChildrenCountEntryKindergarten.SendKeys("7");
                IWebElement cu_TeacherNameEntryKindergarten = driver.FindElement(By.Id("CU_TeacherNameEntryKindergarten"));
                cu_TeacherNameEntryKindergarten.Clear();
                cu_TeacherNameEntryKindergarten.SendKeys("Rip Van Winkle");
            }
            else
            {
                IWebElement cu_NameEntryKindergarten = driver.FindElement(By.Id("CU_NameEntryKindergarten"));
                cu_NameEntryKindergarten.Clear();
                cu_NameEntryKindergarten.SendKeys(returnGuid.ToString());
                IWebElement cu_GroupNameEntryKindergarten = driver.FindElement(By.Id("CU_GroupNameEntryKindergarten"));
                cu_GroupNameEntryKindergarten.Clear();
                cu_GroupNameEntryKindergarten.SendKeys("Something something");
                IWebElement cu_ChildrenCountEntryKindergarten = driver.FindElement(By.Id("CU_ChildrenCountEntryKindergarten"));
                cu_ChildrenCountEntryKindergarten.Clear();
                cu_ChildrenCountEntryKindergarten.SendKeys("9");
                IWebElement cu_TeacherNameEntryKindergarten = driver.FindElement(By.Id("CU_TeacherNameEntryKindergarten"));
                cu_TeacherNameEntryKindergarten.Clear();
                cu_TeacherNameEntryKindergarten.SendKeys("Yusuf Onion");
            }
            return returnGuid.ToString();
        }

        private void InsertKindergartenData(IWebDriver driver, string name = null, bool isOneOrTwo = false)
        {

            if (isOneOrTwo)
            {
                IWebElement cu_NameEntryKindergarten = driver.FindElement(By.Id("CU_NameEntryKindergarten"));
                cu_NameEntryKindergarten.Clear();
                if (name != null)
                {
                    cu_NameEntryKindergarten.SendKeys(name);
                }
                else
                {
                    cu_NameEntryKindergarten.SendKeys("Bullworth");
                }
                IWebElement cu_GroupNameEntryKindergarten = driver.FindElement(By.Id("CU_GroupNameEntryKindergarten"));
                cu_GroupNameEntryKindergarten.Clear();
                cu_GroupNameEntryKindergarten.SendKeys("Group with no name.");
                IWebElement cu_ChildrenCountEntryKindergarten = driver.FindElement(By.Id("CU_ChildrenCountEntryKindergarten"));
                cu_ChildrenCountEntryKindergarten.Clear();
                cu_ChildrenCountEntryKindergarten.SendKeys("7");
                IWebElement cu_TeacherNameEntryKindergarten = driver.FindElement(By.Id("CU_TeacherNameEntryKindergarten"));
                cu_TeacherNameEntryKindergarten.Clear();
                cu_TeacherNameEntryKindergarten.SendKeys("Rip Van Winkle");
            }
            else
            {
                IWebElement cu_NameEntryKindergarten = driver.FindElement(By.Id("CU_NameEntryKindergarten"));
                cu_NameEntryKindergarten.Clear();
                if (name != null)
                {
                    cu_NameEntryKindergarten.SendKeys(name);
                }
                else
                {
                    cu_NameEntryKindergarten.SendKeys("Salad pizza is just salad on bread.");
                }
                IWebElement cu_GroupNameEntryKindergarten = driver.FindElement(By.Id("CU_GroupNameEntryKindergarten"));
                cu_GroupNameEntryKindergarten.Clear();
                cu_GroupNameEntryKindergarten.SendKeys("Something something");
                IWebElement cu_ChildrenCountEntryKindergarten = driver.FindElement(By.Id("CU_ChildrenCountEntryKindergarten"));
                cu_ChildrenCountEntryKindergarten.Clear();
                cu_ChildrenCountEntryKindergarten.SendKeys("9");
                IWebElement cu_TeacherNameEntryKindergarten = driver.FindElement(By.Id("CU_TeacherNameEntryKindergarten"));
                cu_TeacherNameEntryKindergarten.Clear();
                cu_TeacherNameEntryKindergarten.SendKeys("Yusuf Onion");
            }
        }

        private void InsertKindergartenDataForTwo(IWebDriver driver, string name = null)
        {

            IWebElement cu_NameEntryKindergarten = driver.FindElement(By.Id("CU_NameEntryKindergarten"));
            cu_NameEntryKindergarten.Clear();
            if (name != null)
            {
                cu_NameEntryKindergarten.SendKeys(name);
            }
            else
            {
                cu_NameEntryKindergarten.SendKeys("Shovels are spoons.");
            }
            IWebElement cu_GroupNameEntryKindergarten = driver.FindElement(By.Id("CU_GroupNameEntryKindergarten"));
            cu_GroupNameEntryKindergarten.Clear();
            cu_GroupNameEntryKindergarten.SendKeys("idk");
            IWebElement cu_ChildrenCountEntryKindergarten = driver.FindElement(By.Id("CU_ChildrenCountEntryKindergarten"));
            cu_ChildrenCountEntryKindergarten.Clear();
            cu_ChildrenCountEntryKindergarten.SendKeys("12");
            IWebElement cu_TeacherNameEntryKindergarten = driver.FindElement(By.Id("CU_TeacherNameEntryKindergarten"));
            cu_TeacherNameEntryKindergarten.Clear();
            cu_TeacherNameEntryKindergarten.SendKeys("Lance Wilson AKA Ryder"); //Everyone will turn into geniuses overnight....for all the wrong reasons that is.


        }


    }

}
