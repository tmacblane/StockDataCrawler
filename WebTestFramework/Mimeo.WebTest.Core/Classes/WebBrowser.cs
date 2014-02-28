using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;

namespace WebTest.Core
{
	public class WebBrowser : IBrowser
	{
		#region Fields

		private IWebDriver driver;
		private WebDriverWait wait;

		#endregion

		#region Constructors

		public WebBrowser(string browser, bool useRemoteServer, string host, string port, string ieDriverLocation = "", string chromeDriverLocation = "")
		{
			this.driver = this.GetWebDriver(browser, useRemoteServer, host, port, ieDriverLocation, chromeDriverLocation);
			this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(2));
			this.driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

			if(this.Environment.Browser != "GoogleChrome")
			{
				this.driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(45));
			}
		}

		public WebBrowser(string environmentSettings)
		{
			this.Environment = new EnvironmentSettings(environmentSettings);
			this.driver = this.GetWebDriver();
			this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(2));
			this.driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));

			if(this.Environment.Browser != "GoogleChrome")
			{
				this.driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(45));
			}
		}

		#endregion

		#region IBrowser implementation

		public IWebDriver WebDriver
		{
			get
			{
				return this.driver;
			}
		}

		public EnvironmentSettings Environment
		{
			get;
			private set;
		}

		public string BaseUrl
		{
			get;
			set;
		}

		public void AcceptAlertBox()
		{
			for(int i = 0; i < 5; i++)
			{
				try
				{
					IAlert alert = this.driver.SwitchTo().Alert();
					alert.Accept();
				}
				catch
				{
					Thread.Sleep(100);
					i++;
				}
			}
		}

		public bool Checked(string locator, string additionalLocatorPath)
		{
			return BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Selected;
		}

		public void Click(string locator, string additionalLocatorPath)
		{
			try
			{
				BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Click();
			}
			catch(OpenQA.Selenium.WebDriverException e)
			{
				if(e.Message.StartsWith("The HTTP request to the remote WebDriver Server for URL"))
				{
					Trace.WriteLine("Clicking the element " + string.Format(locator, additionalLocatorPath) + " caused a webdriver exception error");
					Trace.WriteLine(e.ToString());
				}
			}
		}

		public void ClearText(string locator, string additionalLocatorPath)
		{
			BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Clear();
		}

		public void Close()
		{
			this.driver.Close();
		}

		public void CtrlAltDoubleClick(string locator, string additionalLocatorPath)
		{
			string jsScript = "var doubleClickElement = document.getElementById('" + locator + "'); if(document.createEventObject){var evt = document.createEventObject();evt.altKey = true;evt.ctrlKey = true;doubleClickElement.fireEvent('ondblclick', evt);}else{var evt = document.createEvent('MouseEvents');evt.initMouseEvent('dblclick', true, true, window, null, 1, 1, 1, 1, true, true, false, false, null, doubleClickElement);doubleClickElement.dispatchEvent(evt);}";

			((IJavaScriptExecutor)this.driver).ExecuteScript(jsScript);
		}

		public void DoubleClick(string locator, string additionalLocatorPath)
		{
			Actions builder = new Actions(this.driver);
			builder.DoubleClick(BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath));

			IAction mouseDoubleClick = builder.Build();

			mouseDoubleClick.Perform();
		}

		public void DragAndDrop(string locator, int offsetX, int offsetY, string additionalLocatorPath)
		{
			var elementToDrag = BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath);

			Actions builder = new Actions(this.driver);
			builder.ClickAndHold(elementToDrag)
				.DragAndDropToOffset(elementToDrag, offsetX, offsetY)
				.Release(elementToDrag);

			IAction dragAndDrop = builder.Build();

			dragAndDrop.Perform();
		}

		public bool Enabled(string locator, string additionalLocatorPath)
		{
			return BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Enabled;
		}

		public void EnterText(string locator, string value, string additionalLocatorPath)
		{
			BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).SendKeys(value);
		}

		public bool Exists(string locator, string additionalLocatorPath)
		{
			bool exists = false;

			IWebElement element = BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath);

			if(element != null)
			{
				exists = true;
			}

			return exists;
		}

		public string GetAlertText()
		{
			IAlert alert = this.driver.SwitchTo().Alert();
			return alert.Text;
		}

		public string GetAttribute(string locator, string attributeName, string additionalLocatorPath)
		{
			return BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).GetAttribute(attributeName);
		}

		public string GetElementErrorMessage(string locatorId)
		{
			string jsScript = "return document.getElementById('" + locatorId + "').errormessage";

			return ((IJavaScriptExecutor)this.driver).ExecuteScript(jsScript).ToString();
		}

		public Point GetLocation(string locator, string additionalLocatorPath)
		{
			return BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Location;
		}

		public string GetPageSource()
		{
			return driver.PageSource;
		}

		public string GetPageTitle()
		{
			if(this.driver.Title == string.Empty)
			{
				Thread.Sleep(1000);
			}

			return this.driver.Title;
		}

		public string GetPageUrl()
		{
			return this.driver.Url;
		}

		public string GetSelectedItemText(string locator, string additionalLocatorPath)
		{
			SelectElement select = new SelectElement(BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath));

			return select.SelectedOption.Text;
		}

		public Size GetSize(string locator, string additionalLocatorPath)
		{
			return BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Size;
		}

		public string GetText(string locator, string additionalLocatorPath)
		{
			return BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Text;
		}

		public ReadOnlyCollection<string> GetWindowHandles()
		{
			return this.driver.WindowHandles;
		}

		public string GetValue(string locator, string additionalLocatorPath)
		{
			return this.GetAttribute(locator, "value", additionalLocatorPath);
		}

		public decimal GetXPathCount(string locator, string additionalLocatorPath)
		{
			return BrowserHelper.GetElements(this.driver, locator, additionalLocatorPath).Count();
		}

		public void HandleAuthenticationDialogForIE(string username, string password)
		{
			if(String.IsNullOrWhiteSpace(username))
			{
				throw new ArgumentNullException(username, "Must contain a value");
			}

			if(String.IsNullOrWhiteSpace(password))
			{
				throw new ArgumentNullException(password, "Must contain a value");
			}

			System.OperatingSystem osInfo = System.Environment.OSVersion;

			// Check to make sure this is run on Windows 7 or Windows 8 only. I didn't test this code on any other OS.
			if((osInfo.Version.Major != 6) || ((osInfo.Version.Major == 6) && (osInfo.Version.Minor == 0)))
			{
				throw new NotSupportedException("This code has only been tested on Windows 7 and Windows 8.");
			}

			// If the minor version is "2" then it's Windows 8. Else, it's Windows 7. See http://msdn.microsoft.com/en-us/library/windows/desktop/ms724832(v=vs.85).aspx
			bool isWindows8 = osInfo.Version.Minor == 2;

			// Condition for finding all "pane" elements
			Condition paneCondition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Pane);

			// Conditions for finding windows with a class of type dialog that's labeled Windows Security
			Condition windowsSecurityCondition = new AndCondition(
								new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
								new PropertyCondition(AutomationElement.ClassNameProperty, "#32770"),
								new PropertyCondition(AutomationElement.NameProperty, "Windows Security"));

			// Conditions for finding list elements with an AutomationId of "UserList"
			Condition userListCondition = new AndCondition(
							new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.List),
							new PropertyCondition(AutomationElement.AutomationIdProperty, "UserList"));

			// Conditions for finding the account listitem element
			Condition userTileCondition;

			if(isWindows8)
			{
				userTileCondition = new AndCondition(
							   new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem),
							   new PropertyCondition(AutomationElement.ClassNameProperty, "CredProvUserTile"),
							   new PropertyCondition(AutomationElement.NameProperty, "Use another account"));
			}
			else
			{
				userTileCondition = new AndCondition(
							   new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem),
							   new PropertyCondition(AutomationElement.ClassNameProperty, "UserTile"),
							   new PropertyCondition(AutomationElement.NameProperty, "Use another account"));
			}


			// Conditions for finding the OK button
			Condition submitButtonCondition = new AndCondition(
								new PropertyCondition(AutomationElement.IsEnabledProperty, true),
								new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Button),
								new PropertyCondition(AutomationElement.AutomationIdProperty, "SubmitButton"));

			// Conditions for finding the edit controls
			Condition editCondition = new AndCondition(
							new PropertyCondition(AutomationElement.IsEnabledProperty, true),
							new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));

			Console.Write("Looking for credentials dialog...");

			// Find all "pane" elements that are children of the desktop
			AutomationElementCollection panes = AutomationElement.RootElement.FindAll(TreeScope.Children, paneCondition);

			bool foundSecurityDialog = false;

			// Iterate through the collection of "panes"
			foreach(AutomationElement pane in panes)
			{
				// Check to see if the current pane is labeled as IE
				if(pane.Current.Name.Contains("Windows Internet Explorer"))
				{
					// Ok, we found IE. Now find all children of the IE pane that meets the windowSecurityCondition defined above
					AutomationElement windowsSecurityDialog = pane.FindFirst(TreeScope.Children, windowsSecurityCondition);

					if(windowsSecurityDialog != null)
					{
						// Great, we found the dialog
						Console.WriteLine("found security dialog");

						foundSecurityDialog = true;

						AutomationElement userTile;

						if(isWindows8)
						{
							// Grab the first child of the dialog that is a UserList
							AutomationElement userList = windowsSecurityDialog.FindFirst(TreeScope.Children, userListCondition);

							// Grab the first child of the UserList that is a UserTile
							userTile = userList.FindFirst(TreeScope.Children, userTileCondition);
						}
						else
						{
							// Grab the first child of the dialog that is a UserTile
							userTile = windowsSecurityDialog.FindFirst(TreeScope.Children, userTileCondition);
						}

						// Make sure the UserTile has focus so that we can see the UserName and Password edit boxes
						userTile.SetFocus();

						// Get all children of the UserTile that are edit controls
						AutomationElementCollection edits = userTile.FindAll(TreeScope.Children, editCondition);

						// Iterate thru the edit controls
						foreach(AutomationElement edit in edits)
						{
							if(edit.Current.Name == "User name")
							{
								// We found the username edit control. Let's set the contents of the box to the username.
								Console.WriteLine("Entering username");
								ValuePattern userNamePattern = (ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern);
								userNamePattern.SetValue(username);
							}
							if(edit.Current.Name == "Password")
							{
								// We found the password edit control. Let's set the contents of the box to the password.
								Console.WriteLine("Entering password");
								ValuePattern userNamePattern = (ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern);
								userNamePattern.SetValue(password);
							}
						}

						// Find the first child of the security dialog that meets the submitButtonCondition defined above
						AutomationElement submitButton = windowsSecurityDialog.FindFirst(TreeScope.Children, submitButtonCondition);

						// Now press the button
						InvokePattern buttonPattern = (InvokePattern)submitButton.GetCurrentPattern(InvokePattern.Pattern);
						buttonPattern.Invoke();

						break;
					}
				}
			}

			if(!foundSecurityDialog)
			{
				Console.WriteLine("no security dialogs found.");
			}
		}

		public bool IsSelected(string locator, string additionalLocatorPath = "")
		{
			return BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Selected;
		}

		public void MouseDown(string locator, string additionalLocatorPath)
		{
			Actions builder = new Actions(this.driver);
			builder.Click(BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath));

			IAction mouseClick = builder.Build();

			mouseClick.Perform();
		}

		public void MouseOver(string locator, string additionalLocatorPath)
		{
			Actions builder = new Actions(this.driver);
			builder.MoveToElement(BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath));

			IAction moveMouse = builder.Build();

			moveMouse.Perform();
			Thread.Sleep(500);

			try
			{
				string jsScript = "var showDiv = document.getElementById('" + locator + "'); showDiv.style.visibility=\"visible\";}";

				((IJavaScriptExecutor)this.driver).ExecuteScript(jsScript);
			}
			catch(Exception e)
			{
				System.Diagnostics.Trace.WriteLine(e.ToString());
			}
		}

		public void Open(bool ieAuthenticationDialog = false)
		{
			if(this.BaseUrl != null && this.BaseUrl != string.Empty)
			{
				this.driver.Navigate().GoToUrl(this.BaseUrl);
			}
			else if(this.Environment.Url != null && this.Environment.Url != string.Empty)
			{
				this.driver.Navigate().GoToUrl(this.Environment.Url);
			}
			else
			{
				throw new Exception("Why you no enter a Url");
			}

			if(ieAuthenticationDialog == true)
			{
				this.HandleAuthenticationDialogForIE(this.Environment.Username, this.Environment.Password);
			}
		}

		public void Open(string url, bool ieAuthenticationDialog = false)
		{
			if(url.StartsWith("http"))
			{
				this.driver.Navigate().GoToUrl(url);
			}
			else
			{
				this.driver.Navigate().GoToUrl(this.BaseUrl + @"\" + url);
			}

			if(ieAuthenticationDialog == true)
			{
				this.HandleAuthenticationDialogForIE(this.Environment.Username, this.Environment.Password);
			}
		}

		public void ScrollIntoView(string locator, string additionalLocator = "")
		{
			var jsScript = string.Empty;

			if(locator.StartsWith("//"))
			{
				var element = string.Format(locator.Replace("'", "\\'"), additionalLocator);
				jsScript = "var element = document.evaluate('" + element + "' ,document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null ).singleNodeValue; if(element != null) {element.scrollIntoView();};";
			}
			else if(locator.StartsWith("css"))
			{
				// TO DO
			}
			else
			{
				jsScript = "var element = document.getElementById('" + locator + "'); if(element != null) {element.scrollIntoView();};";
			}

			((OpenQA.Selenium.IJavaScriptExecutor)this.driver).ExecuteScript(jsScript);
		}

		public void SelectItem(string locator, string item, string additionalLocatorPath)
		{
			SelectElement selectElement = new SelectElement(BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath));

			int i = 0;

			foreach(IWebElement webElement in selectElement.Options)
			{
				if(webElement.Text.Contains(item))
				{
					selectElement.SelectByIndex(i);
					break;
				}

				i++;
			}
		}

		public void SelectFrame(int frameIndex)
		{
			this.driver.SwitchTo().Frame(frameIndex);
		}

		public void SelectFrame(string locator)
		{
			this.driver.SwitchTo().Frame(BrowserHelper.GetElement(this.driver, locator, string.Empty));
		}

		public void SelectMainPage()
		{
			this.driver.SwitchTo().DefaultContent();
		}

		public void SelectWindow(string windowName)
		{
			this.driver.SwitchTo().Window(windowName);
		}

		public void SendCtrlA(string locator, string additionalLocatorPath)
		{
			Actions builder = new Actions(this.driver);
			builder.SendKeys(BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath), OpenQA.Selenium.Keys.Control + "a");

			IAction ctrlA = builder.Build();
			ctrlA.Perform();
		}

		public void Quit()
		{
			this.driver.Quit();
		}

		public void TakeScreenShot(string fileInfo)
		{
			ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
			Screenshot screenshot = screenshotDriver.GetScreenshot();
			screenshot.SaveAsFile(fileInfo, ImageFormat.Png);
		}

		public bool Valid(string locatorId)
		{
			string jsScript = "return document.getElementById('" + locatorId + "').isvalid";

			return bool.Parse(((IJavaScriptExecutor)this.driver).ExecuteScript(jsScript).ToString());
		}

		public bool Visible(string locator, string additionalLocatorPath = "")
		{
			bool visible = false;

			if(this.Exists(locator, additionalLocatorPath))
			{
				visible = BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Displayed;
			}

			return visible;
		}

		public void WaitForElementEnabled(string locator, string additionalLocatorPath)
		{
			this.wait.Until(x => BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Enabled);
		}

		public void WaitForElementPresent(string locator, string additionalLocatorPath)
		{
			this.wait.Until(x => BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath));
		}

		public void WaitForElementVisible(string locator, string additionalLocatorPath)
		{
			this.wait.Until(x => BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Displayed);
		}

		public void WaitForElementDisabled(string locator, string additionalLocatorPath)
		{
			this.wait.Until(x => BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Enabled.Equals(false));
		}

		public void WaitForElementNotPresent(string locator, string additionalLocatorPath)
		{
			this.wait.Until(x => BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Enabled.Equals(false));
		}

		public void WaitForElementHidden(string locator, string additionalLocatorPath)
		{
			this.wait.Until(x => BrowserHelper.GetElement(this.driver, locator, additionalLocatorPath).Displayed.Equals(false));
		}

		public void WaitForPageRequestsToFinish()
		{
			this.wait.Until(x => (bool)(this.driver as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0"));
		}

		#endregion

		#region Helper methods

		private IWebDriver GetWebDriver()
		{
			DesiredCapabilities capabilities = null;
			ICapabilities safariCapabilities = null;
			IWebDriver webDriver;

			if(this.Environment.UseRemoteServer == true)
			{
				var remoteAddress = new Uri(string.Format("http://{0}:{1}/wd/hub", this.Environment.RemoteHost, this.Environment.RemotePort));

				switch(this.Environment.Browser)
				{
					case "Firefox":
						var ffp = new FirefoxProfile();
						ffp.AcceptUntrustedCertificates = true;
						ffp.SetPreference("app.update.auto", false);
						ffp.SetPreference("app.update.enabled", false);
						ffp.SetPreference("browser.search.update", false);
						ffp.SetPreference("browser.shell.checkDefaultBrowser", false);
						ffp.SetPreference("browser.startup.page", 0);
						ffp.SetPreference("browser.tabs.warnOnClose", false);
						ffp.SetPreference("browser.tabs.warnOnOpen;false", false);
						ffp.SetPreference("extensions.checkCompatibility", false);
						ffp.SetPreference("layout.spellcheckDefault", 0);
						ffp.AlwaysLoadNoFocusLibrary = true;
						ffp.EnableNativeEvents = false;

						capabilities = DesiredCapabilities.Firefox();
						capabilities.SetCapability(FirefoxDriver.ProfileCapabilityName, ffp);
						webDriver = new RemoteWebDriver(remoteAddress, capabilities);
						break;
					case "GoogleChrome":
						ChromeOptions chromeOptions = new ChromeOptions();
						chromeOptions.AddArgument("start-maximized");
						capabilities = DesiredCapabilities.Chrome();
						capabilities.SetCapability(ChromeOptions.Capability, chromeOptions);
						webDriver = new RemoteWebDriver(remoteAddress, capabilities);
						break;
					case "IE7":
					case "IE8":
					case "IE9":
					case "IE10":
						capabilities = DesiredCapabilities.InternetExplorer();
						capabilities.SetCapability("ignoreProtectedModeSettings", true);
						webDriver = new RemoteWebDriver(remoteAddress, capabilities);
						break;
					case "Safari":
						safariCapabilities = DesiredCapabilities.Safari();
						webDriver = new RemoteWebDriver(remoteAddress, safariCapabilities);
						break;
					default:
						capabilities = DesiredCapabilities.Firefox();
						webDriver = new RemoteWebDriver(remoteAddress, capabilities);
						break;
				}
			}
			else
			{
				switch(this.Environment.Browser)
				{
					case "Firefox":
						FirefoxProfileManager ffpm = new FirefoxProfileManager();
						var ffp = new FirefoxProfile();

						if(ffpm.GetProfile("WebDriver") != null)
						{
							ffp = ffpm.GetProfile("WebDriver");
							ffp.SetPreference("app.update.auto", false);
							ffp.SetPreference("app.update.enabled", false);
							ffp.SetPreference("browser.search.update", false);
							ffp.SetPreference("browser.shell.checkDefaultBrowser", false);
							ffp.SetPreference("browser.startup.page", 0);
							ffp.SetPreference("browser.tabs.warnOnClose", false);
							ffp.SetPreference("browser.tabs.warnOnOpen;false", false);
							ffp.SetPreference("layout.spellcheckDefault", 0);
							ffp.SetPreference("extensions.checkCompatibility", false);
							ffp.AlwaysLoadNoFocusLibrary = true;
							ffp.EnableNativeEvents = false;
						}

						ffp.AcceptUntrustedCertificates = true;
						webDriver = new FirefoxDriver(ffp);
						break;
					case "GoogleChrome":
						ChromeOptions chromeOptions = new ChromeOptions();
						chromeOptions.AddArgument("start-maximized");
						capabilities = DesiredCapabilities.Chrome();
						capabilities.SetCapability(ChromeOptions.Capability, chromeOptions);
						webDriver = new ChromeDriver(this.Environment.ChromeDriverLocation, chromeOptions);
						break;
					case "IE7":
					case "IE8":
					case "IE9":
					case "IE10":
						InternetExplorerOptions internetExplorerOptions = new InternetExplorerOptions();
						internetExplorerOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
						webDriver = new InternetExplorerDriver(this.Environment.IEDriverLocation, internetExplorerOptions);
						break;
					case "Safari":
						webDriver = new SafariDriver();
						break;
					default:
						var dffp = new FirefoxProfile();
						dffp.AlwaysLoadNoFocusLibrary = true;
						dffp.EnableNativeEvents = false;
						dffp.AcceptUntrustedCertificates = true;
						dffp.SetPreference("app.update.auto", false);
						dffp.SetPreference("app.update.enabled", false);
						dffp.SetPreference("browser.search.update", false);
						dffp.SetPreference("browser.shell.checkDefaultBrowser", false);
						dffp.SetPreference("browser.startup.page", 0);
						dffp.SetPreference("browser.tabs.warnOnClose", false);
						dffp.SetPreference("browser.tabs.warnOnOpen;false", false);
						dffp.SetPreference("layout.spellcheckDefault", 0);
						dffp.SetPreference("extensions.checkCompatibility", false);
						webDriver = new FirefoxDriver(dffp);
						break;
				}
			}

			return webDriver;
		}

		private IWebDriver GetWebDriver(string browser, bool useRemoteServer, string host, string port, string ieDriverLocation, string chromeDriverLocation)
		{
			DesiredCapabilities capabilities = null;
			ICapabilities safariCapabilities = null;
			IWebDriver webDriver;

			if(useRemoteServer == true)
			{
				Uri remoteAddress = null;

				if(port == string.Empty)
				{
					remoteAddress = new Uri(string.Format("http://{0}/wd/hub", host));
				}
				else
				{
					remoteAddress = new Uri(string.Format("http://{0}:{1}/wd/hub", host, port));
				}

				switch(browser)
				{
					case "Firefox":
						var ffp = new FirefoxProfile();
						ffp.AcceptUntrustedCertificates = true;
						ffp.SetPreference("app.update.auto", false);
						ffp.SetPreference("app.update.enabled", false);
						ffp.SetPreference("browser.search.update", false);
						ffp.SetPreference("browser.shell.checkDefaultBrowser", false);
						ffp.SetPreference("browser.startup.page", 0);
						ffp.SetPreference("browser.tabs.warnOnClose", false);
						ffp.SetPreference("browser.tabs.warnOnOpen;false", false);
						ffp.SetPreference("extensions.checkCompatibility", false);
						ffp.SetPreference("layout.spellcheckDefault", 0);
						ffp.AlwaysLoadNoFocusLibrary = true;
						ffp.EnableNativeEvents = false;

						capabilities = DesiredCapabilities.Firefox();
						capabilities.SetCapability(FirefoxDriver.ProfileCapabilityName, ffp);
						webDriver = new RemoteWebDriver(remoteAddress, capabilities);
						break;
					case "GoogleChrome":
						ChromeOptions chromeOptions = new ChromeOptions();
						chromeOptions.AddArgument("start-maximized");
						capabilities = DesiredCapabilities.Chrome();
						capabilities.SetCapability(ChromeOptions.Capability, chromeOptions);
						webDriver = new RemoteWebDriver(remoteAddress, capabilities);
						break;
					case "IE7":
					case "IE8":
					case "IE9":
					case "IE10":
						capabilities = DesiredCapabilities.InternetExplorer();
						capabilities.SetCapability("ignoreProtectedModeSettings", true);
						webDriver = new RemoteWebDriver(remoteAddress, capabilities);
						break;
					case "Safari":
						safariCapabilities = DesiredCapabilities.Safari();
						webDriver = new RemoteWebDriver(remoteAddress, safariCapabilities);
						break;
					default:
						capabilities = DesiredCapabilities.Firefox();
						webDriver = new RemoteWebDriver(remoteAddress, capabilities);
						break;
				}
			}
			else
			{
				switch(browser)
				{
					case "Firefox":
						FirefoxProfileManager ffpm = new FirefoxProfileManager();
						var ffp = new FirefoxProfile();

						if(ffpm.GetProfile("WebDriver") != null)
						{
							ffp = ffpm.GetProfile("WebDriver");
							ffp.SetPreference("app.update.auto", false);
							ffp.SetPreference("app.update.enabled", false);
							ffp.SetPreference("browser.search.update", false);
							ffp.SetPreference("browser.shell.checkDefaultBrowser", false);
							ffp.SetPreference("browser.startup.page", 0);
							ffp.SetPreference("browser.tabs.warnOnClose", false);
							ffp.SetPreference("browser.tabs.warnOnOpen;false", false);
							ffp.SetPreference("layout.spellcheckDefault", 0);
							ffp.SetPreference("extensions.checkCompatibility", false);
							ffp.AlwaysLoadNoFocusLibrary = true;
							ffp.EnableNativeEvents = false;
						}

						ffp.AcceptUntrustedCertificates = true;
						webDriver = new FirefoxDriver(ffp);
						break;
					case "GoogleChrome":
						ChromeOptions chromeOptions = new ChromeOptions();
						chromeOptions.AddArgument("start-maximized");
						capabilities = DesiredCapabilities.Chrome();
						capabilities.SetCapability(ChromeOptions.Capability, chromeOptions);
						webDriver = new ChromeDriver(chromeDriverLocation, chromeOptions);
						break;
					case "IE7":
					case "IE8":
					case "IE9":
					case "IE10":
						InternetExplorerOptions internetExplorerOptions = new InternetExplorerOptions();
						internetExplorerOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;
						webDriver = new InternetExplorerDriver(ieDriverLocation, internetExplorerOptions);
						break;
					case "Safari":
						webDriver = new SafariDriver();
						break;
					default:
						var dffp = new FirefoxProfile();
						dffp.AlwaysLoadNoFocusLibrary = true;
						dffp.EnableNativeEvents = false;
						dffp.AcceptUntrustedCertificates = true;
						dffp.SetPreference("app.update.auto", false);
						dffp.SetPreference("app.update.enabled", false);
						dffp.SetPreference("browser.search.update", false);
						dffp.SetPreference("browser.shell.checkDefaultBrowser", false);
						dffp.SetPreference("browser.startup.page", 0);
						dffp.SetPreference("browser.tabs.warnOnClose", false);
						dffp.SetPreference("browser.tabs.warnOnOpen;false", false);
						dffp.SetPreference("layout.spellcheckDefault", 0);
						dffp.SetPreference("extensions.checkCompatibility", false);
						webDriver = new FirefoxDriver(dffp);
						break;
				}
			}

			return webDriver;
		}

		#endregion
	}
}