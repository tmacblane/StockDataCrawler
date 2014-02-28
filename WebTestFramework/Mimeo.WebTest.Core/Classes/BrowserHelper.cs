using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace WebTest.Core
{
	public class BrowserHelper
	{
		#region Type specific methods

		/// <summary>
		/// Returns an element found by either CSS, ID or XPath based on the locator file.
		/// </summary>
		/// <param name="driver">IWebDriver object</param>
		/// <param name="locator">element locator path</param>
		/// <param name="additionalLocatorPath">additional element locator path</param>
		/// <returns>IWebElement object</returns>
		public static IWebElement GetElement(IWebDriver driver, string locator, string additionalLocatorPath)
		{
			IWebElement element;

			try
			{
				if(locator.StartsWith("//"))
				{
					element = GetElementByXPath(driver, locator, additionalLocatorPath);
				}
				else if(locator.StartsWith("css"))
				{
					element = GetElementByCSS(driver, locator, additionalLocatorPath);
				}
				else
				{
					element = GetElementByID(driver, locator, additionalLocatorPath);
				}
			}
			catch(Exception)
			{
				return null;
			}

			return element;
		}

		/// <summary>
		/// Returns a list of elements found by either CSS, ID or XPath based on the locator file.
		/// </summary>
		public static IList<IWebElement> GetElements(IWebDriver driver, string locator, string additionalLocatorPath)
		{
			if(locator.StartsWith("//"))
			{
				return GetElementsByXPath(driver, locator, additionalLocatorPath);
			}
			else if(locator.StartsWith("css"))
			{
				return GetElementsByCSS(driver, locator, additionalLocatorPath);
			}
			else
			{
				return GetElementsByID(driver, locator, additionalLocatorPath);
			}
		}

		#endregion

		#region Helper methods

		/// <summary>
		/// Returns an element found by its cascading stylesheet (CSS) selector.
		/// </summary>
		private static IWebElement GetElementByCSS(IWebDriver driver, string locator, string additionalLocatorPath)
		{
			locator = locator.Replace("css=", string.Empty);
			return driver.FindElement(By.CssSelector(string.Format(locator, additionalLocatorPath)));
		}

		/// <summary>
		/// Returns a list of elements by their cascading stylesheet (CSS) selector.
		/// </summary>
		/// <returns>List of IWebElement objects</returns>
		private static IList<IWebElement> GetElementsByCSS(IWebDriver driver, string locator, string additionalLocatorPath)
		{
			locator = locator.Replace("css=", string.Empty);
			IList<IWebElement> elements = driver.FindElements(By.CssSelector(string.Format(locator, additionalLocatorPath)));

			return elements;
		}

		/// <summary>
		/// Returns an element found by its ID.
		/// </summary>
		private static IWebElement GetElementByID(IWebDriver driver, string locator, string additionalLocatorPath)
		{
			return driver.FindElement(By.Id(string.Format(locator, additionalLocatorPath)));
		}

		/// <summary>
		/// Returns a list of elements by their ID.
		/// </summary>
		/// <returns>List of IWebElement objects</returns>
		private static IList<IWebElement> GetElementsByID(IWebDriver driver, string locator, string additionalLocatorPath)
		{
			IList<IWebElement> elements = driver.FindElements(By.Id(string.Format(locator, additionalLocatorPath)));
			return elements;
		}

		/// <summary>
		/// Returns an element found by its XPath query.
		/// </summary>
		private static IWebElement GetElementByXPath(IWebDriver driver, string locator, string additionalLocatorPath)
		{
			return driver.FindElement(By.XPath(string.Format(locator, additionalLocatorPath)));
		}

		/// <summary>
		/// Returns a list of elements by their ID.
		/// </summary>
		/// <returns>List of IWebElement objects</returns>
		private static IList<IWebElement> GetElementsByXPath(IWebDriver driver, string locator, string additionalLocatorPath)
		{
			IList<IWebElement> elements = driver.FindElements(By.XPath(string.Format(locator, additionalLocatorPath)));

			return elements;
		}

		#endregion
	}
}