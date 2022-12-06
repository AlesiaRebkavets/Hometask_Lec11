using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Lecture11
{
    internal class Tests
    {
        private IWebDriver _driver;
        private Actions _driverActions;
        private IJavaScriptExecutor _javascriptExecutor;
        private WebDriverWait _driverWait;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _driver = new ChromeDriver();
            //_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);   // wait: variant 3    комментарий для себя, эта строка взята как копипаста из лекции Ивана чтобы не забыть
            _driver.Manage().Window.Maximize();
            _driverActions = new Actions(_driver);
            _javascriptExecutor = (IJavaScriptExecutor)_driver;
            _driverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        }

        [Test]
        public void FirstSeleniumTest()  // ТЕСТ НЕ ЯВЛЯЕТСЯ ЧАСТЬЮ ДЗ. СКОПИПАСТИЛА ЕГО С 11-Й ЛЕКЦИИ ЧТОБЫ ИМЕТЬ УДОБНЫЙ НАГЛЯДНЫЙ ОРИЕНТИР
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/text-box");
            _driverActions.SendKeys(Keys.PageDown).Perform(); // for the chain of actions BUILD() is needed: _driverActions.SendKeys(Keys.PageDown).Click().MoveToElement().Build().Perform(); 

            var fullName = "Name Full";

            var fnTextBox = _driverWait.Until(drv => drv.FindElement(By.Id("userName")));  // wait: variant 1

            _driverWait.Until(drv =>                                                       // wait: variant 2
            {
                if (drv.FindElements(By.Id("userName")).Count > 0)
                {
                    return true;
                }
                return false;
            });
                    

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
            _javascriptExecutor.ExecuteScript("arguments[0].scrollIntoView()", submitButton);
            submitButton.Click();

            var currentAddressResult = _driver.FindElement(By.XPath("//p[@id='currentAddress']"));
            var currentAddressResultText = currentAddressResult.Text;
            Assert.IsTrue(currentAddressResultText.Contains(currentAddress));
        }

        [Test]
        public void IsCheckBoxElementSelected()     // test for Check Box
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/checkbox");  // opens the indicated url
            var homeCollapseButton = _driver.FindElement(By.XPath("//label[@for=\"tree-node-home\"]/../button"));   // finding homeCollapseButton
            homeCollapseButton.Click();   // clicking the button
            var documentsCollapseButton = _driver.FindElement(By.XPath("//label[@for=\"tree-node-documents\"]/../button")); // finding documentsCollapseButton
            documentsCollapseButton.Click(); // clicking the button
            var officeCollapseButton = _driver.FindElement(By.XPath("//label[@for=\"tree-node-office\"]/../button")); // finding officeCollapseButton
            officeCollapseButton.Click();   // clicking the button
            var privateCheckbox = _driver.FindElement(By.XPath("//label[@for=\"tree-node-private\"]/span[1]")); // finding privateCheckbox
            privateCheckbox.Click();   // selecting the element;
            var classifiedCheckbox = _driver.FindElement(By.XPath("//label[@for=\"tree-node-classified\"]/span"));  // finding classifiedCheckbox
            classifiedCheckbox.Click();  // selecting the element;
            var youHaveSelectedLabel = _driver.FindElement(By.XPath("//span[text()=\"private\"]")).Text + " " + _driver.FindElement(By.XPath("//span[text()=\"classified\"]")).Text;  // saving the displayed final text to a separate variable
            Assert.IsTrue(youHaveSelectedLabel.Equals("private classified"));  // verifying if the displayed text is equal to "private classified"
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _driver.Quit();
        }
    }
}
