using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WebTest.Core
{
	public static class PageElementExtensions
	{
		#region Type specific methods

		public static void Click(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.Click(element.Locator, additionalLocator);
		}

		public static void ClearText(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.ClearText(element.Locator, additionalLocator);
		}

		public static void CtrlAltDoubleClick(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.CtrlAltDoubleClick(element.Locator, additionalLocator);
		}

		public static void DoubleClick(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.DoubleClick(element.Locator, additionalLocator);
		}

		public static void DragAndDrop(this IPageElement element, int offsetX, int offsetY, string additionalLocator = "")
		{
			element.Page.Browser.DragAndDrop(element.Locator, offsetX, offsetY, additionalLocator);
		}

		public static bool Enabled(this IPageElement element, string additionalLocator = "")
		{
			return element.Page.Browser.Enabled(element.Locator, additionalLocator);
		}

		public static bool Exists(this IPageElement element, string additionalLocator = "")
		{
			return element.Page.Browser.Exists(element.Locator, additionalLocator);
		}

		public static void EnterText(this IPageElement element, string value, string additionalLocator = "")
		{
			element.Page.Browser.EnterText(element.Locator, value, additionalLocator);
		}

		public static string GetAttribute(this IPageElement element, string attribute, string additionalLocator = "")
		{
			return element.Page.Browser.GetAttribute(element.Locator, attribute, additionalLocator);
		}

		public static int GetCount(this IPageElement element, string addiitonalLocator = "")
		{
			return (int)element.Page.Browser.GetXPathCount(element.Locator, addiitonalLocator);
		}

		public static string GetElementErrorMessage(this IPageElement element)
		{
			return element.Page.Browser.GetElementErrorMessage(element.Locator);
		}

		public static Point GetLocation(this IPageElement element, string additionalLocator = "")
		{
			return element.Page.Browser.GetLocation(element.Locator, additionalLocator);
		}

		public static string GetSelectedItemText(this IPageElement element, string additionalLocator = "")
		{
			return element.Page.Browser.GetSelectedItemText(element.Locator, additionalLocator);
		}

		public static Size GetSize(this IPageElement element, string additionalLocator = "")
		{
			return element.Page.Browser.GetSize(element.Locator, additionalLocator);
		}

		public static string GetText(this IPageElement element, string additionalLocator = "")
		{
			if(element is TextBox)
			{
				return element.Page.Browser.GetValue(element.Locator, additionalLocator);
			}
			else
			{
				return element.Page.Browser.GetText(element.Locator, additionalLocator);
			}
		}

		public static bool IsSelected(this IPageElement element, string additionalLocator = "")
		{
			return element.Page.Browser.IsSelected(element.Locator, additionalLocator);
		}

		public static string GetValue(this IPageElement element, string additionalLocator = "")
		{
			return element.Page.Browser.GetValue(element.Locator, additionalLocator);
		}

		public static void MouseDown(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.MouseDown(element.Locator, additionalLocator);
		}

		public static void MouseOver(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.MouseOver(element.Locator, additionalLocator);
		}

		public static void ScrollIntoView(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.ScrollIntoView(element.Locator, additionalLocator);
		}

		public static void SendCtrlA(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.SendCtrlA(element.Locator, additionalLocator);
		}

		public static void Select(this IPageElement element, string value, string additionalLocator = "")
		{
			element.Page.Browser.SelectItem(element.Locator, value, additionalLocator);
		}

		public static void SelectFrame(this IPageElement element, string additionalLocator = "")
		{
			if(element is Frame)
			{
				element.Page.Browser.SelectFrame(element.Locator);
			}
		}

		public static bool Valid(this IPageElement element)
		{
			return element.Page.Browser.Valid(element.Locator);
		}

		public static bool Visible(this IPageElement element, string additionalLocator = "")
		{
			return element.Page.Browser.Visible(element.Locator, additionalLocator);
		}

		public static void WaitForElementEnabled(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.WaitForElementDisabled(element.Locator, additionalLocator);
		}

		public static void WaitForElementPresent(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.WaitForElementPresent(element.Locator, additionalLocator);
		}

		public static void WaitForElementVisible(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.WaitForElementVisible(element.Locator, additionalLocator);
		}

		public static void WaitForElementDisabled(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.WaitForElementDisabled(element.Locator, additionalLocator);
		}

		public static void WaitForElementNotPresent(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.WaitForElementNotPresent(element.Locator, additionalLocator);
		}

		public static void WaitForElementHidden(this IPageElement element, string additionalLocator = "")
		{
			element.Page.Browser.WaitForElementHidden(element.Locator, additionalLocator);
		}

		#endregion
	}
}