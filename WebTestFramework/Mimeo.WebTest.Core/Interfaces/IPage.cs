using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebTest.Core
{
	public interface IPage
	{
		#region Type specific properties

		string PageTitle
		{
			get;
		}

		string PageUrl
		{
			get;
		}

		IBrowser Browser
		{
			get;
		}

		#endregion
	}
}