using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebTest.Core
{
	public class Link : IPageElement
	{
		#region Constructors

		public Link(string locator, IPage page)
		{
			this.Locator = locator;
			this.Page = page;
		}

		#endregion

		#region IPageElement implementation

		public IPage Page
		{
			get;
			private set;
		}
		
		public string Locator
		{
			get;
			private set;
		}

		#endregion
	}
}