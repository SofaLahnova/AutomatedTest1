using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;

/// <summary>
/// //////////////////////////////////respect
/// </summary>
namespace AutomatedTest1
{
    public class Tests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl("https://respect-shoes.ru/");
        }

        [Test]
        public void TestPriceFilt()
        {
            driver.FindElement(By.CssSelector(".slider-one")).Click();
            driver.FindElement(By.CssSelector(".in-left-catalog--price")).Click();
            driver.FindElement(By.CssSelector(".js-price-from")).SendKeys("1000");
            driver.FindElement(By.CssSelector(".js-price-to")).SendKeys("5000");

            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(x => driver.FindElements(By.XPath("//input[contains(@value,'ПРИМЕНИТЬ ВСЕ ФИЛЬТРЫ') and not(@disabled)]")).Any());

            driver.FindElement(By.XPath("//input[@value='ПРИМЕНИТЬ ВСЕ ФИЛЬТРЫ']")).Click();

            //
            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(x => driver.FindElements(By.XPath("//*[contains(@class,'lds-ring-container') and (@style='')]")).Any());
            new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(x => driver.FindElements(By.XPath("//*[contains(@class,'lds-ring-container') and (@style='')]")).Count == 0);


            int[] actualVlues = Array.ConvertAll(driver.FindElements(By.CssSelector(".card__price-num")).Select(webPrice => webPrice.Text.Replace(" ", "")).ToArray<string>(), s => int.Parse(s));
            actualVlues.ToList().ForEach(actualPrice => Assert.True(actualPrice <= 5000, "Error in Price filter"));
            Thread.Sleep(5000);
        }

        [Test]
        public void NegativeSignUpTest()
        {
            driver.FindElement(By.XPath("//*[@id='auth-button']/..")).Click();
            driver.FindElement(By.XPath("//*[@for='vkl20']")).Click();

            driver.FindElement(By.XPath("//input[@name='REGISTER[NAME]']")).SendKeys("Test");
            driver.FindElement(By.XPath("//input[@name='REGISTER[EMAIL]']")).SendKeys("fbsdhfkha@mail.ru");
            driver.FindElement(By.XPath("//input[@name='REGISTER[PASSWORD]']")).SendKeys("1234567");
            driver.FindElement(By.XPath("//input[@name='REGISTER[CONFIRM_PASSWORD]']")).SendKeys("1234567");
            driver.FindElement(By.XPath("//input[@name='REGISTER[PERSONAL_PHONE]']")).SendKeys("9271116622");

            driver.FindElement(By.XPath("//input[@name='register_submit_button']")).Click();


            Assert.IsTrue(driver.FindElement(By.CssSelector(".actual")).Displayed, "There was no error text for an empty mandatory field with a piccha");
            Thread.Sleep(5000);


        }

        [Test]
        public void TestTooltipText()
        {
            driver.FindElement(By.CssSelector(".slider-one")).Click();

            new Actions(driver).MoveToElement(driver.FindElement(By.XPath("//*[@class='card__delivery_icons']"))).Build().Perform();
            Assert.IsTrue(driver.FindElement(By.XPath("//*[@class='card__delivery_icons']//*[contains (@class, 'icon__tooltip')]")).Displayed, "Tooltip has not appeared");
            Assert.AreEqual("Возможна доставка домой, в офис или в пункт выдачи", driver.FindElement(By.XPath("//*[@class='card__delivery_icons']//*[contains (@class, 'icon__tooltip')]")).Text.Replace("\"", "").Trim(), "The text does not match");
            Thread.Sleep(5000);


        }
        [TearDown]
        public void CleanUp()
        {
            driver.Quit();
        }
    }
}