using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Linq;
using Microsoft.VisualBasic;

namespace SeleniumCsharp

{

    public class Tests

    {
        IWebDriver? driver;
        string URL = "https://wsxtest.atomwide.com/";

        [OneTimeSetUp]
        public void Setup()

        {
            //Below code is to get the drivers folder path dynamically.

            //You can also specify chromedriver.exe path dircly ex: C:/MyProject/Project/drivers

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            //Creates the ChomeDriver object, Executes tests on Google Chrome

            driver = new ChromeDriver(path + @"\drivers\");

            //If you want to Execute Tests on Firefox uncomment the below code

            //Specify Correct location of geckodriver.exe folder path. Ex: C:/Project/drivers

            //driver= new FirefoxDriver(path + @"\drivers\");
        }

        [Test]
        public void recatRequest()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("");
            driver.FindElement(By.Id("password")).SendKeys("");
            
            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Polcies | Recat request 
            wait.Until(driver => driver.FindElement(By.LinkText("Policies"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("Recategorisation Request"))).Click();

            //Enter in URL
            driver.FindElement(By.XPath("//*[@id=\"recattab\"]/tbody/tr[2]/td[1]/input")).SendKeys("https://google.com");
            //Choose 'Search Engine' from dropdown
            driver.FindElement(By.XPath("//*[@id=\"recattab\"]/tbody/tr[2]/td[3]/select")).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"recattab\"]/tbody/tr[2]/td[3]/select/option[4]")));
            driver.FindElement(By.XPath("//*[@id=\"recattab\"]/tbody/tr[2]/td[3]/select")).SendKeys("search" + Keys.Enter);
            //Enter reason
            driver.FindElement(By.XPath("//*[@id=\"recattab\"]/tbody/tr[2]/td[4]/textarea")).SendKeys("testing");

            //Submit button
            //driver.FindElement(By.XPath("//*[@id=\"recattab\"]/tbody/tr[12]/td[4]/button")).Click();

            //Wait to view case number
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void localListOverview()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Polcies | URLs & Keywords 
            wait.Until(driver => driver.FindElement(By.LinkText("Policies"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("URLs & Keywords"))).Click();

            //New URL/Keyword button
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[2]/div[2]/div[1]/div[2]/div/div/div[2]/div[4]/button"))).Click();

            //Enter "888.com" as the entry
            driver.FindElement(By.XPath("//*[@id=\"addListModal\"]/div/div/div[2]/div[1]/div[2]/div/input[1]")).SendKeys("888.com");
            //Move the top policy to "Allow"
            driver.FindElement(By.XPath("//*[@id=\"localtable\"]/tbody/tr/td[2]/div[2]/div[1]/a/i")).Click();
            //Click on the Add button
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[2]/div[2]/div[1]/div[3]/div/div/div[3]/button[4]"))).Click();
            //Wait the loading overlay to disappear then closing the pop up
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btnNewList_close\"]"))).Click();

            //Switch tab to "888.com" to see if it is allowed
            ((IJavaScriptExecutor)driver).ExecuteScript("window.open();");
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.Navigate().GoToUrl("https://www.888.com/");

            //Switch back to WebScreen X
            driver.SwitchTo().Window(driver.WindowHandles.First());

            //Enter 888.com into the search and editing the entry
            wait.Until(driver => driver.FindElement(By.LinkText("URLs & Keywords"))).Click();
            driver.FindElement(By.XPath("//*[@id=\"bodyContainer\"]/div[1]/div[2]/div/div/div[2]/div[2]/input")).SendKeys("888.com");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"tablemain\"]/tbody/tr[2]/td[1]/span/a"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"localtable\"]/tbody/tr/td[2]/div[2]/div[1]/a/i"))).Click();
            //Click update and wait for overlay
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btnNewList_Add\"]"))).Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            //Close the pop up
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btnNewList_close\"]"))).Click();

            //Enter 888.com into the search and removing the entry
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"addListModal\"]")));
            wait.Until(driver => driver.FindElement(By.LinkText("URLs & Keywords"))).Click();
            driver.FindElement(By.XPath("//*[@id=\"bodyContainer\"]/div[1]/div[2]/div/div/div[2]/div[2]/input")).SendKeys("888.com");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"tablemain\"]/tbody/tr[2]/td[9]/span/a/i"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btndeleteKeyword_Delete\"]"))).Click();

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void bundlesEstablishment()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Polcies | Bundles
            wait.Until(driver => driver.FindElement(By.LinkText("Policies"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("Bundles"))).Click();

            //Wait the loading overlay to disappear then closing the pop up
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[1]/div/div[1]/div[2]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[2]/div/div[1]/div[2]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[3]/div/div[1]/div[2]")));
            //Create new establishment bundle
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btnAddNewBundle\"]"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addListModal\"]/div/div/div[2]/div/div[1]/div/input"))).SendKeys("A Selenium Testing Bundle");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addListModal\"]/div/div/div[2]/div/div[2]/div/textarea"))).SendKeys("A Selenium Testing Bundle");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btnNewList_Add\"]"))).Click();

            //Wait for overlay to disappear and close pop up
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[1]/div/div[1]/div[2]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[2]/div/div[1]/div[2]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[3]/div/div[1]/div[2]")));

            //Going Refresh and edit the bundle
            driver.Navigate().Refresh();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"table\"]/tbody/tr[4]/td[1]/a"))).Click();

            //Add, enter URL, desc and click save
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"menu1\"]/div[1]/div/button[1]"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"TextValue\"]"))).SendKeys("google.com");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"DescValue\"]"))).SendKeys("Allow google.com");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btnListEntry_Save\"]"))).Click();

            //Click done and delete bundle
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btnListEntry_Cancel\"]"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"table\"]/tbody/tr[4]/td[5]/a/i"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"deleteSharedListModal\"]/div/div/div[3]/button[2]"))).Click();

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void bundlesSystem()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Polcies | Bundles
            wait.Until(driver => driver.FindElement(By.LinkText("Policies"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("Bundles"))).Click();

            //Wait the loading overlay to disappear then going to System Bundle
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[1]/div/div[1]/div[2]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[2]/div/div[1]/div[2]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[3]/div/div[1]/div[2]")));
            wait.Until(driver => driver.FindElement(By.LinkText("System Bundles"))).Click();

            //Click view on Facebook Bundle and close pop up
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[3]/div/div[1]/div[3]/div/div/div/table/tbody/tr[2]/td[1]/a"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"editSharedListModal\"]/div/div/div[1]/button"))).Click();

            //Move policy to deny
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"table\"]/tbody/tr[2]/td[3]/div[3]/a/i"))).Click();

            //Wait for overlay then move to allow
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[1]/div/div[1]/div[2]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[2]/div/div[1]/div[2]")));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[3]/div/div[1]/div[2]")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div/div[3]/div/div[1]/div[3]/div/div/div/table/tbody/tr[2]/td[4]/div[2]/a/i"))).Click();

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void groupResources()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Polcies | Bundles
            wait.Until(driver => driver.FindElement(By.LinkText("Group Resources"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("Group Bundles"))).Click();

            //Wait for overlay then click Create new bundle
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div/div/div/div/div[2]/div/button"))).Click();

            //Wait then enter detail and Add
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addBundleModal\"]/div/div/div[2]/div/div[1]/div/input"))).SendKeys("A Selenium Group Bundle");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addBundleModal\"]/div/div/div[2]/div/div[2]/div/input"))).SendKeys("A Selenium Group Bundle");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addBundleModal\"]/div/div/div[2]/div/div[5]/div[1]/input"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addBundleModal\"]/div/div/div[2]/div/div[5]/div[2]/input"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btnNewList_Add\"]"))).Click();

            //Click on edit, wait for overlay, add new entry, save 
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div/div/div/div/div[3]/table/tbody/tr/td[1]/a"))).Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"menu1\"]/div[1]/button[1]"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"TextValue\"]"))).SendKeys("google.com");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"DescValue\"]"))).SendKeys("Allow google.com");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btnListEntry_Save\"]"))).Click();

            //wait for overlay, done, then delete group
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"editBundleModal\"]/div/div/div[3]/button"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div/div/div/div/div[3]/table/tbody/tr/td[9]/a/i"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"deleteBundleModal\"]/div/div/div[3]/button[2]"))).Click();

            //Wait to view the end0
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void creatingIPReport()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Polcies | Create Reports 
            wait.Until(driver => driver.FindElement(By.LinkText("Reports"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("Create Reports"))).Click();

            //Wait for page to load and click on the 2 radio button
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"report\"]/div/div[1]/label[2]/input"))).Click();
            driver.FindElement(By.XPath("//*[@id=\"report\"]/div/div[2]/label[2]/input")).Click();
            //Choose 'IP Report' from dropdown
            driver.FindElement(By.XPath("//*[@id=\"report\"]/div/div[4]/div/div/select")).Click();
            driver.FindElement(By.XPath("//*[@id=\"report\"]/div/div[4]/div/div/select")).SendKeys("ip" + Keys.Enter);
            //Next button to step 2
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div[1]/div[1]/div/div[6]/a"))).Click();

            //Wait then select :05 on the end time *Only needed when start time and end time is not displayed as the last hour
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"reportFrequency\"]/div[1]/div[2]/select[2]"))).Click();
            driver.FindElement(By.XPath("//*[@id=\"reportFrequency\"]/div[1]/div[2]/select[2]")).SendKeys("05" + Keys.Enter);
            //Next button to step 3
            driver.FindElement(By.XPath("//*[@id=\"reportFrequency\"]/div[2]/button[2]")).Click();

            //Wait then put in IP address
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"startip\"]"))).SendKeys("10.253.141.1");
            driver.FindElement(By.XPath("//*[@id=\"endip\"]")).SendKeys("10.253.141.1");
            //Next button to step 4
            driver.FindElement(By.XPath("//*[@id=\"reportType\"]/div[2]/button[2]")).Click();

            //Wait then put in description
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"reportExtra\"]/div[1]/div/div/input"))).SendKeys("testing");
            //Run report
            driver.FindElement(By.XPath("//*[@id=\"reportExtra\"]/div[3]/button[2]")).Click();
            //Wait for pop up then go to View Reports
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addReportResultModal\"]/div/div/div[4]/a"))).Click();

            //Wait then select IP Report
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[2]/div/div[1]/select"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[2]/div/div[1]/select/option[10]")));
            driver.FindElement(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[2]/div/div[1]/select")).SendKeys("ip" + Keys.Enter);

            //Click on Delete and confirm the pop up.
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[4]/div/table/tbody/tr[1]/td[10]/a"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[3]/div/div/div[3]/button[2]"))).Click();

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void creatingUserReport()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Polcies | Create Reports 
            wait.Until(driver => driver.FindElement(By.LinkText("Reports"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("Create Reports"))).Click();

            //Wait for page to load and click on the 2 radio button
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"report\"]/div/div[1]/label[2]/input"))).Click();
            driver.FindElement(By.XPath("//*[@id=\"report\"]/div/div[2]/label[2]/input")).Click();
            //Choose 'User Report' from dropdown
            driver.FindElement(By.XPath("//*[@id=\"report\"]/div/div[4]/div/div/select")).Click();
            driver.FindElement(By.XPath("//*[@id=\"report\"]/div/div[4]/div/div/select")).SendKeys("user" + Keys.Enter);
            //Next button to step 2
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div[1]/div[1]/div/div[6]/a"))).Click();

            //Wait then select :05 on the end time *Only needed when start time and end time is not displayed as the last hour
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"reportFrequency\"]/div[1]/div[2]/select[2]"))).Click();
            driver.FindElement(By.XPath("//*[@id=\"reportFrequency\"]/div[1]/div[2]/select[2]")).SendKeys("05" + Keys.Enter);
            //Next button to step 3
            driver.FindElement(By.XPath("//*[@id=\"reportFrequency\"]/div[2]/button[2]")).Click();

            //Wait then put in username
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"reportType\"]/div[1]/div[1]/div[2]/div/div[1]/input"))).SendKeys("testm001.998");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"reportType\"]/div[1]/div[1]/div[2]/div/div[2]/button"))).Click();
            //Next button to step 4
            driver.FindElement(By.XPath("//*[@id=\"reportType\"]/div[3]/button[2]")).Click();

            //Wait then put in description
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"reportExtra\"]/div[1]/div/div/input"))).SendKeys("testing");
            //Run report
            driver.FindElement(By.XPath("//*[@id=\"reportExtra\"]/div[3]/button[2]")).Click();
            //Wait for pop up then go to View Reports
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addReportResultModal\"]/div/div/div[4]/a"))).Click();

            //Wait then select User Report
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[2]/div/div[1]/select"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[2]/div/div[1]/select/option[10]")));
            driver.FindElement(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[2]/div/div[1]/select")).SendKeys("user" + Keys.Enter);

            //Click on Delete and confirm the pop up.
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[4]/div/table/tbody/tr[1]/td[10]/a"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[3]/div/div/div[3]/button[2]"))).Click();

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void creatingURLReport()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Polcies | Create Reports 
            wait.Until(driver => driver.FindElement(By.LinkText("Reports"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("Create Reports"))).Click();

            //Wait for page to load and click on the 2 radio button
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"report\"]/div/div[1]/label[2]/input"))).Click();
            driver.FindElement(By.XPath("//*[@id=\"report\"]/div/div[2]/label[2]/input")).Click();
            //Choose 'URL Report' from dropdown
            driver.FindElement(By.XPath("//*[@id=\"report\"]/div/div[4]/div/div/select")).Click();
            driver.FindElement(By.XPath("//*[@id=\"report\"]/div/div[4]/div/div/select")).SendKeys("url" + Keys.Enter);
            //Next button to step 2
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[2]/div[2]/div/div/div[1]/div[1]/div/div[5]/a"))).Click();

            //Wait then select :05 on the end time *Only needed when start time and end time is not displayed as the last hour
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"reportFrequency\"]/div[1]/div[2]/select[2]"))).Click();
            driver.FindElement(By.XPath("//*[@id=\"reportFrequency\"]/div[1]/div[2]/select[2]")).SendKeys("05" + Keys.Enter);
            //Next button to step 3
            driver.FindElement(By.XPath("//*[@id=\"reportFrequency\"]/div[2]/button[2]")).Click();

            //Wait then put in URL
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"url\"]"))).SendKeys("google.com");
            //Next button to step 4
            driver.FindElement(By.XPath("//*[@id=\"reportType\"]/div[4]/button[2]")).Click();

            //Wait then put in description
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"reportExtra\"]/div[1]/div/div/input"))).SendKeys("testing");
            //Run report
            driver.FindElement(By.XPath("//*[@id=\"reportExtra\"]/div[3]/button[2]")).Click();
            //Wait for pop up then go to View Reports
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addReportResultModal\"]/div/div/div[4]/a"))).Click();

            //Wait then select URL Report
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[2]/div/div[1]/select"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[2]/div/div[1]/select/option[10]")));
            driver.FindElement(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[2]/div/div[1]/select")).SendKeys("url" + Keys.Enter);

            //Click on Delete and confirm the pop up.
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[4]/div/table/tbody/tr[1]/td[10]/a"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[3]/div/div/div[3]/button[2]"))).Click();

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void viewingReport()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Polcies | View Reports 
            wait.Until(driver => driver.FindElement(By.LinkText("Reports"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("View Reports"))).Click();

            //Wait then click on On-Demand Reports
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"demandtab\"]/b"))).Click();

            //Wait then select test policy
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[2]/div/div[1]/select"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[2]/div/div[1]/select/option[10]")));
            driver.FindElement(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[2]/div/div[1]/select")).SendKeys("ip" + Keys.Enter);

            //Select the bottom report then HTML
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"demand-report\"]/div[1]/div[4]/div/table/tbody/tr[last()]/td[9]/a"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"selectmodaltab\"]/tbody/tr/td[3]/a"))).Click();

            //Switch back to webscreen and close the pop up
            driver.SwitchTo().Window(driver.WindowHandles[0]);
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"DemandSelectModal\"]/div/div/div[1]/button"))).Click();

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }



        [Test]
        public void blockSettings()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Settings | Block Page Settings
            wait.Until(driver => driver.FindElement(By.LinkText("Settings"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("Block Page Settings"))).Click();

            //Click preview 
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[2]/div[2]/div[2]/div[3]/button[2]"))).Click();
            //Tab back to WebScreen
            driver.SwitchTo().Window(driver.WindowHandles[0]);

            //Click Save 
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[2]/div[2]/div[2]/div[3]/button[1]"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"dialogSuccess\"]/div/div/div[3]/button"))).Click();

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void createUserGroup()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Settings | URLs & Keywords 
            wait.Until(driver => driver.FindElement(By.LinkText("Settings"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("User Groups"))).Click();

            //Wait for overlay to disappear
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            //Wait then click on Create New
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div/div/div/div[1]/div/div[2]/button"))).Click();

            //Wait then enter Name, Description
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"newListModal\"]/div/div/div[2]/div[1]/div/input"))).SendKeys("zTest User Group");
            driver.FindElement(By.XPath("//*[@id=\"newListModal\"]/div/div/div[2]/div[2]/div/textarea")).SendKeys("User Group for testing with Selenium");
            //Wait then select test policy
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"newListModal\"]/div/div/div[2]/div[3]/div/select"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"newListModal\"]/div/div/div[2]/div[3]/div/select/option[1]")));
            driver.FindElement(By.XPath("//*[@id=\"newListModal\"]/div/div/div[2]/div[3]/div/select")).SendKeys("block" + Keys.Enter);
            //Click on Create
            driver.FindElement(By.XPath("//*[@id=\"newListModal\"]/div/div/div[3]/button[2]")).Click();

            //Refresh Page and wait for overlay
            wait.Until(driver => driver.FindElement(By.LinkText("User Groups"))).Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));

            //Edit Members, add ssatiarujikano.000
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div/div/div/div[2]/div/table/tbody/tr[last()]/td[5]/a"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"collapseOne\"]/div/div[5]/div[1]/input"))).SendKeys("ssatiarujikano.000");
            //Click add
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"collapseOne\"]/div/div[5]/div[2]/button"))).Click();

            //Refresh Page and wait for overlay
            wait.Until(driver => driver.FindElement(By.LinkText("User Groups"))).Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));

            //Delete the last entry in the group, and click confirm delete
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[2]/div[2]/div[2]/div/div/div/div[2]/div/table/tbody/tr[last()]/td[last()]/a/i"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"deleteListModal\"]/div/div/div[3]/button[2]"))).Click();

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void ipDefinitions()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Settings | URLs & Keywords 
            wait.Until(driver => driver.FindElement(By.LinkText("Settings"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("IP Definitions"))).Click();

            //Wait for overlay to disappear
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            //Wait then click on New IP Range
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"btnaddnewip\"]"))).Click();

            //Wait then enter Start IP, End IP, Description
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"IPRangeModal\"]/div/div/div[2]/div[2]/div[2]/div[1]/input"))).SendKeys("10.253.141.255");
            driver.FindElement(By.XPath("//*[@id=\"IPRangeModal\"]/div/div/div[2]/div[2]/div[2]/div[2]/input")).SendKeys("10.253.141.255");
            driver.FindElement(By.XPath("//*[@id=\"IPRangeModal\"]/div/div/div[2]/div[3]/div/input")).SendKeys("Test IP Range for Selenium");

            //Wait then select IP Range and click Save
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"ddliprangepolicy\"]"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"ddliprangepolicy\"]/option[15]")));
            driver.FindElement(By.XPath("//*[@id=\"ddliprangepolicy\"]")).SendKeys("test" + Keys.Enter);
            driver.FindElement(By.XPath("//*[@id=\"addnewtargetip\"]")).Click();

            //Confirm alert pop up
            driver.SwitchTo().Alert().Accept();

            //Wait for confirmation then Close pop up
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"dialogSuccess\"]/div/div/div[3]/button"))).Click();

            //Refresh IP Definitions and wait for overlay
            wait.Until(driver => driver.FindElement(By.LinkText("IP Definitions"))).Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));

            //Delete the last entry in the group, and click confirm delete
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div[2]/div[2]/table/tbody/tr[last()]/td[13]/div/a/i"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[3]/div/div/div[3]/button[2]"))).Click();

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void testTool()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            wait.Until(driver => driver.FindElement(By.LinkText("Policies"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("Test Tool"))).Click();

            //Wait then enter URL
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div/div/div/div[2]/div/div[1]/div[1]/div/input"))).SendKeys("google.com");
            //Wait then select IP Range
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"selectedOptions\"]"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"selectedOptions\"]/option[2]")));
            driver.FindElement(By.XPath("//*[@id=\"selectedOptions\"]")).SendKeys("ip" + Keys.Enter);
            //Click Test URL
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div/div/div/div[2]/div/div[1]/div[3]/div[3]/button"))).Click();

            //Wait for entry to load up
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div/div/div/div[2]/div/div[2]/div/table[2]/tbody/tr/td[4]")));

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void helpAndFeedback()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to Help & Feedback
            wait.Until(driver => driver.FindElement(By.LinkText("Help & Feedback"))).Click();

            //Wait then click on Webscreen Info Page
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div[1]/div/div/a/div"))).Click();
            //Tab back to WebScreen
            driver.SwitchTo().Window(driver.WindowHandles[0]);

            //Wait then click on View User Guide
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div[2]/div/div/a/div"))).Click();
            //Tab back to WebScreen
            driver.SwitchTo().Window(driver.WindowHandles[0]);

            //Wait then click on Raise a New Case
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div[3]/div/div/a/div"))).Click();
            //Tab back to WebScreen
            driver.SwitchTo().Window(driver.WindowHandles[0]);

            //Wait then click on Appropraite Filtering
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div[4]/div/div/a/div"))).Click();
            driver.SwitchTo().Window(driver.WindowHandles[0]);

            //Wait then click on Check Device
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div[5]/div/div/a/div"))).Click();
            driver.SwitchTo().Window(driver.WindowHandles[0]);

            //Wait then click on Test WebScreen
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div[6]/div/div/a/div"))).Click();
            driver.SwitchTo().Window(driver.WindowHandles[0]);

            //Wait then click on Submit Feedback
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[2]/div[7]/div/div/a/div"))).Click();
            driver.SwitchTo().Window(driver.WindowHandles[0]);

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void systemInfo()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to System Info | Category Definitions
            wait.Until(driver => driver.FindElement(By.LinkText("System Info"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("Category Definitions"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"bodyContainer\"]/div/div/div/div/div/div[1]/h3")));

            //Going to System Info | System URL/Keyword Bundles
            wait.Until(driver => driver.FindElement(By.LinkText("System Info"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("System URL/Keyword Bundles"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"bodyContainer\"]/div/div/div/div/div/div[1]/h3")));

            //Going to System Info | System URL List
            wait.Until(driver => driver.FindElement(By.LinkText("System Info"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("System URL List"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"bodyContainer\"]/div/div/div/div/div/div[1]/h3")));

            //Going to System Info | HTTPS Inspection
            wait.Until(driver => driver.FindElement(By.LinkText("System Info"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("HTTPS Inspection"))).Click();
            wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"bodyContainer\"]/div/div/div/div/div/div[1]/h3")));

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [Test]
        public void policyConfig()
        {
            //Going to WebScreen X URL
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Navigate().GoToUrl(URL);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            driver.Manage().Window.Maximize();

            //Setting up wait
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            //Sign in button
            driver.FindElement(By.ClassName("top")).Click();

            //Login Crendentials
            wait.Until(driver => driver.FindElement(By.Id("username")));
            driver.FindElement(By.Id("username")).SendKeys("testm001.998");
            driver.FindElement(By.Id("password")).SendKeys("purple133");

            //Login button
            driver.FindElement(By.Id("_eventId_proceed")).Click();

            //Wait for OTP pop up to show and click close
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//*[@id=\"NoOTPTagModal\"]/div/div/div[1]/button")).Click();

            //Going to System Info | Category Definitions
            wait.Until(driver => driver.FindElement(By.LinkText("Policies"))).Click();
            wait.Until(driver => driver.FindElement(By.LinkText("Policy Configuration"))).Click();

            //Wait the loading overlay to disappear then click on Create New
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div[1]/div[1]/div[1]/div/a"))).Click();
               
            //Enter new policy group details
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"myestab\"]"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"21050\"]"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"newgroupname\"]"))).SendKeys("A Selenium Testing Group");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"bodyContainer\"]/div/div[1]/div[9]/div/button"))).Click();

            //Wait the loading overlay to disappear then click on the polic group we just made
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[2]/div[2]/div[1]/div[2]/div[1]/div[6]/div/table/tbody/tr[1]/td[1]/a"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"targetipaddress\"]"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"targetIPAddressModal\"]/div/div/div[2]/div[3]/div[2]/div/div[1]/input"))).SendKeys("10.253.141.250");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"targetIPAddressModal\"]/div/div/div[2]/div[3]/div[2]/div/div[2]/input"))).SendKeys("10.253.141.250");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"targetIPAddressModal\"]/div/div/div[2]/div[4]/div[2]/input"))).SendKeys("Selenium Testing");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addnewtargetip\"]"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"dialogSuccess\"]/div/div/div[3]/button"))).Click();

            //View Calendar and Set up Scheduler
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(driver => driver.FindElement(By.ClassName("btn-info"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"viewCalendarModal\"]/div/div/div[1]/button"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"schedulePartialDiv\"]/div[2]/table/tbody/tr/td[1]/div"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"editScheduleModal\"]/div/div/div[4]/button[2]"))).Click();

            //Wait then edit filtering policy
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[3]/div/div/div[1]/button"))).Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"timepolicyedit\"]/tbody/tr/td[1]/div"))).Click();
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//*[@id=\"overlay\"]")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"localPartialDiv\"]/div[2]/div[1]/div[1]/button"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addLocalList\"]/div/div/div[2]/div[1]/div[2]/input"))).SendKeys("888.com");
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addLocalList\"]/div/div/div[3]/button[2]"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"addLocalList\"]/div/div/div[1]/button"))).Click();

            //Change tabs and link bunldes
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"sharedtab\"]"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"sharedlist\"]/div/div[2]/div[3]/div/div[2]/table/tbody/tr[1]/td[5]/a"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[3]/div/div/div[3]/button[2]"))).Click();

            //Go back to Policy Config and delete the policy group
            wait.Until(driver => driver.FindElement(By.LinkText("Policy Configuration"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"ippolicydelete\"]/i"))).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[3]/div/div/div[3]/button[2]"))).Click();

            //Wait to view the end
            //wait.Until(driver => driver.FindElement(By.LinkText("lpsum")));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            driver.Quit();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

    }

}
