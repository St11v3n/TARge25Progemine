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

        [Fact]
        [Order(1)]
        public void Should_NavigateToCreate_AddUniqueKindergarten_WithCorrectData_ReturnToIndex()
        {
            //ülesseade
            IWebDriver driver = SetupAndNavigateSIndex();

            //tegevus
            IWebElement createInIndex = driver.FindElement(By.Id("CreateInIndex"));
            createInIndex.Click();

            //sisestatavad andmed, siin tagastub ka kasutatud nimi
            string concreteName = InsertKindergartenData_ReturnName(driver, false);
            thisRunGuid = concreteName;

            IWebElement cu_CreateKindergarten = driver.FindElement(By.Id("CU_CreateKindergarten"));
            cu_CreateKindergarten.Click();

            

            //andmete kogumine pärast reloadi
            var tableOfIndex = driver.FindElement(By.Id("IndexTable"));
            List<IWebElement> rowsOfTable = tableOfIndex.FindElements(By.TagName("tr")).ToList();
            rowsOfTable.RemoveAt(0);
            string kindergartenGroupNameData = "";
            string kindergartenNameData = "";
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
            
            Assert.True(true == doesItExist);
            Assert.Equal(kindergartenNameData, thisRunGuid);
            Assert.True(kindergartenGroupNameData == "Group with no name.");
            Assert.Equal(kindergartenChildrenCountData, "7");
            Assert.True((thisRunGuid == kindergartenNameData)
                && ("Group with no name." == kindergartenGroupNameData)
                && ("7" == kindergartenChildrenCountData));
        }

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
            IWebElement details_KindergartenId = driver.FindElement(By.Id("Details_KindergartenId"));
            var details_kindergartenid = details_KindergartenId.Text;

            IWebElement details_KindergartenName = driver.FindElement(By.Id("Details_KindergartenName"));
            var details_kindergartenname = details_KindergartenName.Text;

            IWebElement details_KindergartenGroupName = driver.FindElement(By.Id("Details_KindergartenGroupName"));
            var details_kindergartengroupname = details_KindergartenGroupName.Text;

            IWebElement details_KindergartenChildrenCount = driver.FindElement(By.Id("Details_KindergartenChildrenCount"));
            var details_kindergartenchildrencount = details_KindergartenChildrenCount.Text;

            IWebElement details_KindergartenTeacherName = driver.FindElement(By.Id("Details_KindergartenTeacherName"));
            var details_kindergartenteachername = details_KindergartenTeacherName.Text;

            driver.Quit();
            //kontroll
            Assert.True(isObjectFound);
            Assert.NotNull(details_kindergartenid);
            // et ei oleks tühi kontrollid kõikidele andmetele
            Assert.NotEmpty(details_kindergartenname);
            Assert.NotNull(details_kindergartengroupname);
            Assert.True(!string.IsNullOrEmpty(details_kindergartenchildrencount));
            Assert.Equal(thisRunGuid, details_kindergartenname);
            Assert.Equal("Group with no name.", details_kindergartengroupname);
            Assert.Equal("7", details_kindergartenchildrencount);
            Assert.Equal("Rip Van Winkle", details_kindergartenteachername);
            Assert.True(int.Parse(details_kindergartenchildrencount).GetType() == typeof(int));
   
        }

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
            string kindergartenChildrenCountData = "";
            string kindergartenTeacherNameData = "";
            string kindergartenBuiltAtData = "";
            doesItExist = false;
            foreach (var row in rowsOfTable)
            {
                if (row.FindElement(By.Id("IndexNameKindergarten")).Text == thisRunGuid)
                {
                    doesItExist = true;
                    kindergartenNameData = thisRunGuid;
                    kindergartenGroupNameData = row.FindElement(By.Id("IndexGroupNameKindergarten")).Text;
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
            Assert.False(string.IsNullOrEmpty(kindergartenGroupNameData));
            Assert.NotEqual(kindergartenGroupNameData, "Group with no name.");
            Assert.True(kindergartenGroupNameData == "completely unheard of haunebu");
            Assert.False(string.IsNullOrEmpty(kindergartenGroupNameData));
            //kontrolli kõik teised andmed ka üle põhjalikult.
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
                cu_GroupNameEntryKindergarten.SendKeys("Something something");
                IWebElement cu_ChildrenCountEntryKindergarten = driver.FindElement(By.Id("CU_ChildrenCountEntryKindergarten"));
                cu_ChildrenCountEntryKindergarten.Clear();
                cu_ChildrenCountEntryKindergarten.SendKeys("10");
                IWebElement cu_TeacherNameEntryKindergarten = driver.FindElement(By.Id("CU_TeacherNameEntrySpaceship"));
                cu_TeacherNameEntryKindergarten.Clear();
                cu_TeacherNameEntryKindergarten.SendKeys("Rodriguez Rodriguez");
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
                    cu_NameEntryKindergarten.SendKeys("What is a good name for a kindergarten?");
                }
                IWebElement cu_GroupNameEntryKindergarten = driver.FindElement(By.Id("CU_GroupNameEntryKindergarten"));
                cu_GroupNameEntryKindergarten.Clear();
                cu_GroupNameEntryKindergarten.SendKeys("i add groupname for kindergarten");
                IWebElement cu_ChildrenCountEntryKindergarten = driver.FindElement(By.Id("CU_ChildrenCountEntryKindergarten"));
                cu_ChildrenCountEntryKindergarten.Clear();
                cu_ChildrenCountEntryKindergarten.SendKeys("13");
                IWebElement cu_TeacherNameEntryKindergarten = driver.FindElement(By.Id("CU_TeacherNameEntryKindergarten"));
                cu_TeacherNameEntryKindergarten.Clear();
                cu_TeacherNameEntryKindergarten.SendKeys("Joshua Graham");
            }
        }


    }


}
