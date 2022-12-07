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
            var youHaveSelectedLabelText = _driver.FindElement(By.XPath("//span[text()=\"private\"]")).Text + " " + _driver.FindElement(By.XPath("//span[text()=\"classified\"]")).Text;  // saving the displayed final text to a separate variable
            Assert.IsTrue(youHaveSelectedLabelText.Equals("private classified"));  // verifying if the displayed text is equal to "private classified"
        }

        [Test]
        public void SelectRadioButton()      // test for Radio Button
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/radio-button");    // opens the indicated url
            var impressiveRadio = _driver.FindElement(By.XPath("//label[text()=\"Impressive\"]"));   // finding impressiveRadio
            impressiveRadio.Click();  // clicking the button
            var youHaveSelectedLabelText = _driver.FindElement(By.XPath("//p[contains(text(), \"You have selected\")]/span")).Text;  // saving the displayed final text to a separate variable
            Assert.That(youHaveSelectedLabelText.Equals("Impressive"), Is.True);  // verifying if the displayed text is equal to "Impressive"
        }

        [Test]
        public void AddRowToAWebTable()     // test for Web Tables
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/webtables");         // opens the indicated url
            var AddButton = _driver.FindElement(By.Id("addNewRecordButton"));   // finding "Add" button element
            AddButton.Click();    // clicking the "Add" button
            var registrationForm = _driver.FindElement(By.Id("registration-form-modal"));  // finding registration form modal label

            Assert.IsTrue(registrationForm.Displayed);   // verifying if "Registration Form" modal opened

            var FirstNameTextBox = _driver.FindElement(By.Id("firstName"));   // finding elements of registration form
            var LastNameTextBox = _driver.FindElement(By.Id("lastName"));
            var EmailTextBox = _driver.FindElement(By.Id("userEmail"));
            var AgeTextBox = _driver.FindElement(By.Id("age"));
            var SalaryTextBox = _driver.FindElement(By.Id("salary"));
            var DepartmentTextBox = _driver.FindElement(By.Id("department"));
            var SubmitButton = _driver.FindElement(By.Id("submit"));

            FirstNameTextBox.SendKeys("Walter");    // entering values to the registration form fields
            LastNameTextBox.SendKeys("White");
            EmailTextBox.SendKeys("heisenberg@gmail.com");
            AgeTextBox.SendKeys("51");
            SalaryTextBox.SendKeys("11000000");
            DepartmentTextBox.SendKeys("Chemistry Department");
            SubmitButton.Click();

            var FirstNameTableColumn = _driver.FindElement(By.XPath("//div[@role=\"rowgroup\"][4]//div[@role=\"gridcell\"][1]"));   // finding columns of the fourth table row
            var LastNameTableColumn = _driver.FindElement(By.XPath("//div[@role=\"rowgroup\"][4]//div[@role=\"gridcell\"][2]"));
            var AgeTableColumn = _driver.FindElement(By.XPath("//div[@role=\"rowgroup\"][4]//div[@role=\"gridcell\"][3]"));
            var EmailTableColumn = _driver.FindElement(By.XPath("//div[@role=\"rowgroup\"][4]//div[@role=\"gridcell\"][4]"));
            var SalaryTableColumn = _driver.FindElement(By.XPath("//div[@role=\"rowgroup\"][4]//div[@role=\"gridcell\"][5]"));
            var DepartmentTableColumn = _driver.FindElement(By.XPath("//div[@role=\"rowgroup\"][4]//div[@role=\"gridcell\"][6]"));

            Assert.IsTrue(FirstNameTableColumn.Text.Equals("Walter"));         // asserts to verify that the entred data correctly displayed in the table
            Assert.IsTrue(LastNameTableColumn.Text.Equals("White"));
            Assert.IsTrue(AgeTableColumn.Text.Equals("51"));
            Assert.IsTrue(EmailTableColumn.Text.Equals("heisenberg@gmail.com"));
            Assert.IsTrue(SalaryTableColumn.Text.Equals("11000000"));
            Assert.IsTrue(DepartmentTableColumn.Text.Equals("Chemistry Department"));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _driver.Quit();
        }
    }
}
