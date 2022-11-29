using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Lecture11
{
    internal class Tests
    {
        private IWebDriver _driver;
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _driver = new ChromeDriver();
        }

        [Test]
        public void Test1()
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/text-box");
            _driver.Manage().Window.Maximize();

            var fullName = "Name Full";
            var fullNameTextBox = _driver.FindElement(By.Id("userName"));
            fullNameTextBox.SendKeys(fullName);

            var email = "email@gmail.com";
            var emailTextBox = _driver.FindElement(By.Id("userEmail"));
            emailTextBox.SendKeys(email);

            var currentAddress = "Svitrigailos";
            var currentAddressTextBox = _driver.FindElement(By.Id("currentAddress"));
            currentAddressTextBox.SendKeys(currentAddress);

            var permanentAddress = "Gostauto";
            var permanentAddressTextBox = _driver.FindElement(By.Id("permanentAddress"));
            permanentAddressTextBox.SendKeys(permanentAddress);

            var submitButton = _driver.FindElement(By.Id("submit"));
            submitButton.Click();

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _driver.Quit();
        }
    }
}
