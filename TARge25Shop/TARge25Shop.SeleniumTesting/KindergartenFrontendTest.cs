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
                    kindergartenBuiltAtData = row.FindElement(By.Id("IndexBuiltDateSpaceship")).Text;
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

    }
}
