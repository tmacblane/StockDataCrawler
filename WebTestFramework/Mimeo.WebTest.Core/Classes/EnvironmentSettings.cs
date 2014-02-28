using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace WebTest.Core
{
	public class EnvironmentSettings
	{
		#region Fields

		private XElement configurationsNode;
		private XElement environmentSettingsNode;
		private XElement remoteSettingsNode;
		private XElement browsersNode;
		private XElement windowsAuthenticationNode;

		#endregion

		#region Constructors

		public EnvironmentSettings(string settingsDocumentPath)
		{
			var settingsDocument = XDocument.Load(settingsDocumentPath);

			this.configurationsNode = settingsDocument.Element("Configurations");
			this.environmentSettingsNode = this.configurationsNode.Element("EnvironmentSettings");
			this.remoteSettingsNode = this.configurationsNode.Element("RemoteSettings");
			this.browsersNode = this.configurationsNode.Element("Browsers");
			this.windowsAuthenticationNode = configurationsNode.Element("WindowsAuthentication");
			
			this.LoadSettings();
		}

		#endregion

		#region Helper methods

		private void LoadSettings()
		{			
			this.UseRemoteServer = bool.Parse(this.environmentSettingsNode.Element("RemoteServer").Value);
			this.RemoteHost = this.remoteSettingsNode.Element("Host").Value;
			this.RemotePort = int.Parse(this.remoteSettingsNode.Element("Port").Value);
			this.Browser = this.environmentSettingsNode.Element("Browser").Value;

			if(this.environmentSettingsNode.Element("Locale") != null)
			{
				this.Locale = this.environmentSettingsNode.Element("Locale").Value;
			}
			else
			{
				this.Locale = string.Empty;
			}

			if(this.environmentSettingsNode.Element("Environment").Value != null)
			{
				this.TestEnvironment = this.environmentSettingsNode.Element("Environment").Value;
			}
			else
			{
				this.TestEnvironment = string.Empty;
			}

			if(this.environmentSettingsNode.Element("BaseUrl") != null)
			{
				this.BaseUrl = this.environmentSettingsNode.Element("BaseUrl").Value;
			}
			else
			{
				this.BaseUrl = string.Empty;
			}

			this.Url = this.SetUrl();
			this.Timeout = this.remoteSettingsNode.Element("Timeout").Value;
			this.TestRunner = this.environmentSettingsNode.Element("TestRunner").Value;
			this.ChromeDriverLocation = this.browsersNode.Element("GoogleChrome").Value;
			this.IEDriverLocation = this.GetIEDriverLocation(this.Browser);

			// Used for IE windows authentication prompt
			if(this.windowsAuthenticationNode != null)
			{
				this.Username = this.windowsAuthenticationNode.Element("Username").Value;
				this.Password = this.windowsAuthenticationNode.Element("Password").Value;
			}
			else
			{
				this.Username = string.Empty;
				this.Password = string.Empty;
			}
		}

		private string SetUrl()
		{
			string url;

			if(this.BaseUrl != string.Empty)
			{
				url = this.BaseUrl;
			}
			else
			{
				// deprecate this
				string instance = this.environmentSettingsNode.Element("Instance").Value;
				string testEnvironment = this.environmentSettingsNode.Element("Environment").Value;
				string virtualDirectory = this.environmentSettingsNode.Element("VirtualDirectory").Value;

				if(!string.IsNullOrWhiteSpace(testEnvironment))
				{
					testEnvironment = "." + testEnvironment;
				}

				switch(this.Locale)
				{
					case "US":
						url = string.Format("http://{0}{1}.mimeo.com/{2}", instance, testEnvironment, virtualDirectory);
						break;
					case "UK":
						url = string.Format("https://{0}{1}.mimeo.co.uk/{2}", instance, testEnvironment, virtualDirectory);
						break;
					default:
						url = string.Format("http://{0}{1}.mimeo.com/{2}", instance, testEnvironment, virtualDirectory);
						break;
				}
			}

			return url;
		}

		private string GetIEDriverLocation(string browser)
		{
			return this.browsersNode.Element(browser).Value;
		}

		#endregion

		#region Type specific properties

		public string Locale
		{
			get;
			private set;
		}

		public string TestEnvironment
		{
			get;
			private set;
		}

		public string Browser
		{
			get;
			set;
		}

		public bool UseRemoteServer
		{
			get;
			set;
		}

		public string RemoteHost
		{
			get;
			set;
		}

		public int RemotePort
		{
			get;
			set;
		}

		public string BaseUrl
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

		public string Timeout
		{
			get;
			set;
		}

		public string TestRunner
		{
			get;
			set;
		}

		public string ChromeDriverLocation
		{
			get;
			set;
		}

		public string IEDriverLocation
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}
		 
		#endregion
	}
}