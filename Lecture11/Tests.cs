using System;
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
            var youHaveSelectedLabelText = $"{_driver.FindElement(By.XPath("//span[text()=\"private\"]")).Text} {_driver.FindElement(By.XPath("//span[text()='classified']")).Text}";  // saving the displayed final text to a separate variable
            Assert.IsTrue(youHaveSelectedLabelText.Equals("private classified"));  // verifying if the displayed text is equal to "private classified"
        }

        [Test]
        public void SelectRadioButton()      // test for Radio Button
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/radio-button");    // opens the indicated url
            var impressiveRadio = _driver.FindElement(By.XPath("//label[text()=\"Impressive\"]"));   // finding impressiveRadio
            impressiveRadio.Click();  // clicking the button
            var youHaveSelectedLabel = _driver.FindElement(By.XPath("//p[contains(text(), \"You have selected\")]/span"));  // saving locator of the displayed final text 
            Assert.That(youHaveSelectedLabel.Text.Equals("Impressive"), Is.True);  // verifying if the displayed text is equal to "Impressive"
            Assert.IsTrue(youHaveSelectedLabel.Displayed);
        }

        [Test]
        public void AddRowToAWebTable()     // test for Web Tables
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/webtables");         // opens the indicated url
            var AddButton = _driver.FindElement(By.Id("addNewRecordButton"));   // finding "Add" button element
            AddButton.Click();    // clicking the "Add" button
            var registrationForm = _driver.FindElement(By.Id("registration-form-modal"));  // finding registration form modal label

            Assert.IsTrue(registrationForm.Displayed);   // verifying if "Registration Form" modal opened

            var firstNameTextBox = _driver.FindElement(By.Id("firstName"));   // finding elements of registration form
            var lastNameTextBox = _driver.FindElement(By.Id("lastName"));
            var emailTextBox = _driver.FindElement(By.Id("userEmail"));
            var ageTextBox = _driver.FindElement(By.Id("age"));
            var salaryTextBox = _driver.FindElement(By.Id("salary"));
            var departmentTextBox = _driver.FindElement(By.Id("department"));
            var submitButton = _driver.FindElement(By.Id("submit"));

            firstNameTextBox.SendKeys("Walter");    // entering values to the registration form fields
            lastNameTextBox.SendKeys("White");
            emailTextBox.SendKeys("heisenberg@gmail.com");
            ageTextBox.SendKeys("51");
            salaryTextBox.SendKeys("11000000");
            departmentTextBox.SendKeys("Chemistry Department");
            submitButton.Click();

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

        [Test]
        public void ButtonsClickTest()     //test for Buttons
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/buttons");         // opens the indicated url

            var doubleClickButton = _driver.FindElement(By.Id("doubleClickBtn"));       // saving locators for the buttons
            var rightClickButton = _driver.FindElement(By.Id("rightClickBtn"));
            var clickButton = _driver.FindElement(By.XPath("//button[text()=\"Click Me\"]"));

            _driverActions.DoubleClick(doubleClickButton).Build().Perform();      // performing double click
            _driverActions.ContextClick(rightClickButton).Build().Perform();      // performing right click
            _driverActions.Click(clickButton).Build().Perform();                  // performing button click

            var doubleClickMessage = _driver.FindElement(By.Id("doubleClickMessage"));   // saving locators for the messages displayed
            var rightClickMessage = _driver.FindElement(By.Id("rightClickMessage"));
            var clickMessage = _driver.FindElement(By.Id("dynamicClickMessage"));   

            Assert.IsTrue(doubleClickMessage.Displayed);   // verifying that message appeared after double click 
            Assert.IsTrue(rightClickMessage.Displayed);    // verifying that message appeared after right click
            Assert.IsTrue(clickMessage.Displayed);         // verifying that message appeared after button click

            Assert.IsTrue(doubleClickMessage.Text.Equals("You have done a double click"));  // verifying that message appeared after double click is correct
            Assert.IsTrue(rightClickMessage.Text.Equals("You have done a right click"));    // verifying that message appeared after right click is correct
            Assert.IsTrue(clickMessage.Text.Equals("You have done a dynamic click"));       // verifying that message appeared after button click is correct               
        }

        [Test]
        public void LinksTest()     //test for Links
        {
            _driver.Navigate().GoToUrl("https://demoqa.com/links");         // opens the indicated url

            var homeLink = _driver.FindElement(By.Id("simpleLink"));          // locators for first 2 links (leading to the other webpage)
            var homeIrHZqLink = _driver.FindElement(By.Id("dynamicLink"));

            var createdLink = _driver.FindElement(By.Id("created"));          // locators for the other links (api)
            var noContentLink = _driver.FindElement(By.Id("no-content"));

            var toolsQaImage = _driver.FindElement(By.XPath("//img[@src='/images/Toolsqa.jpg']"));  // locator inicating that a new webpage was opened (for first 2 links)
            
            homeLink.Click();   // clicking Home link 
            Assert.IsTrue(toolsQaImage.Displayed);  // verifying that new page with toolsQaImage locator is displayed
            
            _driver.SwitchTo().Window(_driver.WindowHandles[0]);   // switching to the first browser tab
            homeIrHZqLink.Click();     // clicking 'HomemoqPw' link 
            Assert.IsTrue(toolsQaImage.Displayed);  // verifying that new page with toolsQaImage locator is displayed
            
            _driver.SwitchTo().Window(_driver.WindowHandles[0]);   // switching to the first browser tab
            createdLink.Click();    // clicking 'Created' link 

            var linkResponse1Locator = By.XPath("//p[@id = 'linkResponse']/b[1]");
            var fnTextBox = _driverWait.Until(drv => drv.FindElements(linkResponse1Locator).Count > 0);
            _javascriptExecutor.ExecuteScript("arguments[0].scrollIntoView()", _driver.FindElement(linkResponse1Locator));
            var StatusCode = _driver.FindElement(linkResponse1Locator);  // finding status code page element locator
            var StatusText = _driver.FindElement(By.XPath("//p[@id = \"linkResponse\"]/b[2]"));  // finding status text page element locator
            Assert.IsTrue(StatusCode.Text.Equals("201") && StatusText.Text.Equals("Created"));  // verifying that status code '201' and text 'Created' are displayed
        }
        
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _driver.Quit();
        }
    }
}
