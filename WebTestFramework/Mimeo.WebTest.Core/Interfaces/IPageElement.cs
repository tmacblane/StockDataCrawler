using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebTest.Core
{
	public interface IPageElement
	{
		#region Type specific properties

		string Locator
		{
			get;			
		}

		IPage Page
		{
			get;
		}

		#endregion
	}
}