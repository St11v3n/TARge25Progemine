using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TARge25Shop.SeleniumTesting
{
    public class SpaceshipFrontendTest
    {
        [Fact]
        public void Should_NavigateToCreate_AddSpaceShipWithCorrectData_ReturnToIndex()
        {
            //firefoxi käskiv ja jutiv draiver
            IWebDriver driver = new FirefoxDriver();
            //aadress millele draiver navigeerib
            driver.Url = "https://localhost:";
            //lehel otsitav element
            IWebElement createinIndex = driver.FindElement(By.LinkText("Spaceship"));
            //selle elemendiga tehtav tegevus
            navigateToSpaceship.Click();
            IWebElement createInIndex = driver.FindElement(By.Id("SpaceshipNavigate"));
            createInIndex.Click();
        }   
            
    }
}
