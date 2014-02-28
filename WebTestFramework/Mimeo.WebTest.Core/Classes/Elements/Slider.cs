using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebTest.Core
{
	public class Slider : IPageElement
	{
		#region Constructors

		public Slider(string locator, IPage page)
		{
			this.Locator = locator;
			this.Page = page;
		}

		#endregion

		#region IPageElement implementation

		public string Locator
		{
			get; private set;			
		}

		public IPage Page
		{
			get;
			private set;
		}
		
		#endregion
	}
}
