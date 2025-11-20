namespace Lab5_Selenium;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System;

public class Tests
{
    private const string BaseUrl = "https://the-internet.herokuapp.com";
    IWebDriver driver;
    private WebDriverWait wait;

    [SetUp]
    public void Setup()
    {
        driver = new EdgeDriver();
        driver.Manage().Window.Maximize();
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    [Test]
    public void ForgotPassword_ShouldShowServerError_WhenEmailIsSubmitted()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/forgot_password");

        driver.FindElement(By.Id("email")).SendKeys("testemail@gmail.com");

        driver.FindElement(By.Id("form_submit")).Click();

        bool isErrorDisplayed = wait.Until(d => d.PageSource.Contains("Internal Server Error"));
        Assert.That(isErrorDisplayed, Is.True);
    }

    [Test]
    public void HorizontalSlider_ShouldIncreaseValueToHalf_WhenArrowRightIsPressed()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/horizontal_slider");

        var slider = driver.FindElement(By.CssSelector("input[type='range']"));
        var valueSpan = driver.FindElement(By.Id("range"));

        slider.SendKeys(Keys.ArrowRight);

        Assert.That(valueSpan.Text, Is.EqualTo("0.5"));
    }

    [Test]
    public void Dropdown_ShouldUpdateSelection_WhenOptionIsChanged()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/dropdown");

        var selectElement = driver.FindElement(By.Id("dropdown"));
        var select = new SelectElement(selectElement);

        select.SelectByText("Option 1");

        Assert.That(select.SelectedOption.Text, Is.EqualTo("Option 1"));
    }

    [Test]
    public void Typos_ShouldDisplayExpectedText_WhenPageIsLoaded()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/typos");
        var textElement = driver.FindElement(By.XPath("//div[@class='example']/p[2]"));
        string content = textElement.Text;

        bool containsExpectedText = content.Contains("won't") || content.Contains("won,t");
        Assert.That(containsExpectedText, Is.True);
    }

    [Test]
    public void EntryAd_ShouldCloseModal_WhenCloseButtonIsClicked()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/entry_ad");

        wait.Until(d => d.FindElement(By.XPath("//div[@class='modal']")).Displayed);

        var closeButton = wait.Until(d =>
        {
            var btn = d.FindElement(By.XPath("//div[@class='modal-footer']//p"));
            return (btn.Displayed && btn.Enabled) ? btn : null;
        });

        Thread.Sleep(500);
        closeButton.Click();

        wait.Until(d => !d.FindElement(By.XPath("//div[@class='modal']")).Displayed);
        Assert.That(driver.FindElement(By.XPath("//div[@class='modal']")).Displayed, Is.False);
    }

    [Test]
    public void FileDownload_ShouldDisplayDownloadOptions_WhenPageIsLoaded()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/download");
        var downloadLinks = driver.FindElements(By.XPath("//div[@class='example']//a"));

        Assert.That(downloadLinks.Count, Is.GreaterThan(0));
        Assert.That(downloadLinks[0].GetAttribute("href"), Is.Not.Empty);
    }

    [Test]
    public void BasicAuth_ShouldLoginSuccessfully_WhenValidCredentialsAreProvided()
    {
        string authUrl = "https://admin:admin@the-internet.herokuapp.com/basic_auth";
        driver.Navigate().GoToUrl(authUrl);

        var successMessage = driver.FindElement(By.XPath("//p")).Text;
        Assert.That(successMessage, Is.EqualTo("Congratulations! You must have the proper credentials."));
    }

    [Test]
    public void DynamicLoading_ShouldShowHelloWorld_WhenStartButtonIsClicked()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/dynamic_loading/1");
        driver.FindElement(By.XPath("//div[@id='start']/button")).Click();

        var finishedTextElement = wait.Until(d =>
        {
            var el = d.FindElement(By.Id("finish"));
            return el.Displayed ? el : null;
        });

        Assert.That(finishedTextElement.Text, Is.EqualTo("Hello World!"));
    }

    [Test]
    public void ContextMenu_ShouldShowAlert_WhenRightClickIsPerformed()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/context_menu");
        var hotSpot = driver.FindElement(By.Id("hot-spot"));

        Actions actions = new Actions(driver);
        actions.ContextClick(hotSpot).Perform();

        IAlert alert = driver.SwitchTo().Alert();
        Assert.That(alert.Text, Is.EqualTo("You selected a context menu"));
        alert.Accept();
    }

    [Test]
    public void RedirectLink_ShouldRedirectToStatusCodes_WhenLinkIsClicked()
    {
        driver.Navigate().GoToUrl($"{BaseUrl}/redirector");
        driver.FindElement(By.Id("redirect")).Click();

        Assert.That(driver.Url, Does.Contain("status_codes"));
        var header = driver.FindElement(By.XPath("//h3")).Text;
        Assert.That(header, Is.EqualTo("Status Codes"));
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }
}