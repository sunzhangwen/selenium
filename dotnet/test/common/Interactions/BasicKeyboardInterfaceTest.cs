﻿using NUnit.Framework;

namespace OpenQA.Selenium.Interactions
{
    [TestFixture]
    public class BasicKeyboardInterfaceTest : DriverTestFixture
    {
        [SetUp]
        public void Setup()
        {
            new Actions(driver).SendKeys(Keys.Null).Perform();
        }

        [TearDown]
        public void ReleaseModifierKeys()
        {
            new Actions(driver).SendKeys(Keys.Null).Perform();
        }

        [Test]
        [IgnoreBrowser(Browser.IPhone, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Remote, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Android, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Safari, "API not implemented in driver")]
        public void ShouldAllowBasicKeyboardInput()
        {
            driver.Url = javascriptPage;

            IWebElement keyReporter = driver.FindElement(By.Id("keyReporter"));

            Actions actionProvider = new Actions(driver);
            IAction sendLowercase = actionProvider.SendKeys(keyReporter, "abc def").Build();

            sendLowercase.Perform();

            Assert.AreEqual("abc def", keyReporter.GetAttribute("value"));

        }

        [Test]
        [IgnoreBrowser(Browser.Firefox, "API not implemented in driver")]
        [IgnoreBrowser(Browser.IPhone, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Remote, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Android, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Safari, "API not implemented in driver")]
        public void ShouldAllowSendingKeyDownOnly()
        {
            driver.Url = javascriptPage;

            IWebElement keysEventInput = driver.FindElement(By.Id("theworks"));

            Actions actionProvider = new Actions(driver);

            IAction pressShift = actionProvider.KeyDown(keysEventInput, Keys.Shift).Build();
            pressShift.Perform();

            IWebElement keyLoggingElement = driver.FindElement(By.Id("result"));
            string logText = keyLoggingElement.Text;

            IAction releaseShift = actionProvider.KeyDown(keysEventInput, Keys.Shift).Build();
            releaseShift.Perform();

            Assert.IsTrue(logText.EndsWith("keydown"), "Key down event not isolated. Log text should end with 'keydown', got: " + logText);
        }

        [Test]
        [IgnoreBrowser(Browser.Firefox, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Chrome, "API not implemented in driver")]
        [IgnoreBrowser(Browser.IPhone, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Remote, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Android, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Safari, "API not implemented in driver")]
        public void ShouldAllowSendingKeyUp()
        {
            driver.Url = javascriptPage;
            IWebElement keysEventInput = driver.FindElement(By.Id("theworks"));

            IAction pressShift = new Actions(driver).KeyDown(keysEventInput, Keys.Shift).Build();
            pressShift.Perform();

            IWebElement keyLoggingElement = driver.FindElement(By.Id("result"));

            string eventsText = keyLoggingElement.Text;
            Assert.IsTrue(keyLoggingElement.Text.EndsWith("keydown"), "Key down should be isolated for this test to be meaningful. Event text should end with 'keydown', got events: " + eventsText);

            IAction releaseShift = new Actions(driver).KeyUp(keysEventInput, Keys.Shift).Build();

            releaseShift.Perform();

            eventsText = keyLoggingElement.Text;
            Assert.IsTrue(keyLoggingElement.Text.EndsWith("keyup"), "Key up event not isolated. Event text should end with 'keyup', got: " + eventsText);
        }

        [Test]
        [IgnoreBrowser(Browser.Firefox, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Chrome, "API not implemented in driver")]
        [IgnoreBrowser(Browser.IPhone, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Remote, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Android, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Safari, "API not implemented in driver")]
        public void ShouldAllowSendingKeysWithShiftPressed()
        {
            driver.Url = javascriptPage;

            IWebElement keysEventInput = driver.FindElement(By.Id("theworks"));

            keysEventInput.Click();

            IAction pressShift = new Actions(driver).KeyDown(Keys.Shift).Build();
            pressShift.Perform();

            IAction sendLowercase = new Actions(driver).SendKeys("ab").Build();
            sendLowercase.Perform();

            IAction releaseShift = new Actions(driver).KeyUp(Keys.Shift).Build();
            releaseShift.Perform();

            AssertThatFormEventsFiredAreExactly("focus keydown keydown keypress keyup keydown keypress keyup keyup"); 

            Assert.AreEqual("AB", keysEventInput.GetAttribute("value"));
        }

        [Test]
        [IgnoreBrowser(Browser.Firefox, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Chrome, "API not implemented in driver")]
        [IgnoreBrowser(Browser.IPhone, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Remote, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Android, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Safari, "API not implemented in driver")]
        public void ShouldAllowSendingKeysWithLeftShiftPressed()
        {
            driver.Url = javascriptPage;

            IWebElement keysEventInput = driver.FindElement(By.Id("theworks"));

            keysEventInput.Click();

            IAction pressShift = new Actions(driver).KeyDown(Keys.LeftShift).Build();
            pressShift.Perform();

            IAction sendLowercase = new Actions(driver).SendKeys("ab").Build();
            sendLowercase.Perform();

            IAction releaseShift = new Actions(driver).KeyUp(Keys.LeftShift).Build();
            releaseShift.Perform();

            AssertThatFormEventsFiredAreExactly("focus keydown keydown keypress keyup keydown keypress keyup keyup"); 

            Assert.AreEqual("AB", keysEventInput.GetAttribute("value"));
        }

        [Test]
        [IgnoreBrowser(Browser.IPhone, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Remote, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Android, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Safari, "API not implemented in driver")]
        public void ShouldAllowSendingKeysToActiveElement()
        {
            driver.Url = bodyTypingPage;

            Actions actionProvider = new Actions(driver);
            IAction someKeys = actionProvider.SendKeys("ab").Build();
            someKeys.Perform();

            AssertThatBodyEventsFiredAreExactly("keypress keypress");
            IWebElement formLoggingElement = driver.FindElement(By.Id("result"));
            AssertThatFormEventsFiredAreExactly(string.Empty); 
        }

        [Test]
        [IgnoreBrowser(Browser.IPhone, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Remote, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Android, "API not implemented in driver")]
        [IgnoreBrowser(Browser.Safari, "API not implemented in driver")]
        public void ShouldAllowBasicKeyboardInputOnActiveElement()
        {
            driver.Url = javascriptPage;

            IWebElement keyReporter = driver.FindElement(By.Id("keyReporter"));

            keyReporter.Click();

            Actions actionProvider = new Actions(driver);
            IAction sendLowercase = actionProvider.SendKeys("abc def").Build();

            sendLowercase.Perform();

            Assert.AreEqual("abc def", keyReporter.GetAttribute("value"));
        }

        private void AssertThatFormEventsFiredAreExactly(string message, string expected)
        {
            Assert.AreEqual(expected, driver.FindElement(By.Id("result")).Text.Trim(), message);
        }

        private void AssertThatFormEventsFiredAreExactly(string expected)
        {
            AssertThatFormEventsFiredAreExactly(string.Empty, expected);
        }

        private void AssertThatBodyEventsFiredAreExactly(string expected)
        {
            Assert.AreEqual(expected, driver.FindElement(By.Id("body_result")).Text.Trim());
        }
    }
}
