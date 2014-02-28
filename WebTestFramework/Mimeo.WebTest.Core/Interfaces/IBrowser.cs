using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;

using OpenQA.Selenium;

namespace WebTest.Core
{
	public interface IBrowser
	{
		#region Type specific methods

		void AcceptAlertBox();

		bool Checked(string locator, string additionalLocatorPath);

		void Click(string locator, string additionalLocatorPath);

		void ClearText(string locator, string additionalLocatorPath);

		void Close();

		void CtrlAltDoubleClick(string locator, string additionalLocatorPath);

		void DoubleClick(string locator, string additionalLocatorPath);

		void DragAndDrop(string locator, int offsetX, int offsetY, string additionalLocatorPath);

		bool Enabled(string locator, string additionalLocatorPath);

		void EnterText(string locator, string value, string additionalLocatorPath);

		bool Exists(string locator, string additionalLocatorPath);

		string GetAlertText();

		string GetAttribute(string locator, string attributeName, string additionalLocatorPath);

		string GetElementErrorMessage(string locatorId);

		Point GetLocation(string locator, string additionalLocatorPath);

		string GetPageSource();

		string GetPageTitle();

		string GetPageUrl();

		Size GetSize(string locator, string additionalLocatorPath);

		string GetSelectedItemText(string locator, string additionalLocatorPath);

		string GetText(string locator, string additionalLocatorPath);

		string GetValue(string locator, string additionalLocatorPath);

		ReadOnlyCollection<string> GetWindowHandles();

		decimal GetXPathCount(string locator, string additionalLocatorPath);

		void HandleAuthenticationDialogForIE(string username, string password);

		bool IsSelected(string locator, string additionalLocatorPath);

		void MouseDown(string locator, string additionalLocatorPath);

		void MouseOver(string locator, string additionalLocatorPath);

		void Open(bool ieAuthenticationDialog = false);

		void Open(string url, bool ieAuthenticationDialog = false);

		void ScrollIntoView(string locator, string additionalLocatorPath);

		void SendCtrlA(string locator, string additionalLocatorPath);

		void SelectItem(string locator, string item, string additionalLocatorPath);

		void SelectFrame(int frameIndex);

		void SelectFrame(string locator);

		void SelectMainPage();

		void SelectWindow(string windowName);

		void Quit();

		void TakeScreenShot(string fileInfo);

		bool Valid(string locatorId);

		bool Visible(string locator, string additionalLocatorPath);

		void WaitForElementEnabled(string locator, string additionalLocatorPath);

		void WaitForElementPresent(string locator, string additionalLocatorPath);

		void WaitForElementVisible(string locator, string additionalLocatorPath);

		void WaitForElementDisabled(string locator, string additionalLocatorPath);

		void WaitForElementNotPresent(string locator, string additionalLocatorPath);

		void WaitForElementHidden(string locator, string additionalLocatorPath);

		void WaitForPageRequestsToFinish();

		#endregion

		#region Type specific properties

		EnvironmentSettings Environment
		{
			get;
		}

		IWebDriver WebDriver
		{
			get;
		}

		string BaseUrl
		{
			get;
			set;
		}

		#endregion
	}
}