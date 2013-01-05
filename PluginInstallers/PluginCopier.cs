using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Reflection;

using Microsoft.Win32;

namespace PluginInstallers
{
	[RunInstaller(true)]
	public class PluginCopier : Installer
	{

		/*
		 * Once the actual installer project places the files specified in the FileSystem directory
		 * on the local machine, the Custom Actions defined in the inslaaer project then call to 
		 * this class to complete the install.
		 */ 

		public override void Commit(IDictionary savedState)
		{
			// No need to do anything here.
		}


		public override void Install(IDictionary stateSaver)
		{
			// Confirm that WLW is installed on the local machine, and get the 
			// plugins folder path:

			string LIVE_WRITER_NOT_PRESENT_MESSAGE = ""
				+ "Windows Live Writer does not appear to be present on this computer. This installer "
				+ "Can  only run if Windows Live Writer is present on the current machine. Please "
				+ "install Windows Live Writer and try again";

			string PLUG_IN_DIRECTORY_NOT_FOUND_MESSAGE = ""
				+ "The Pulins directory used by Windows Live Writer cannot be found. Try reinstalling "
				+ "Windows Live Writer, and then run Setup again.";

			string targetPath = this.GetWLWPluginsFolder();

			if (string.IsNullOrEmpty(targetPath))
			{
				throw new InvalidOperationException(LIVE_WRITER_NOT_PRESENT_MESSAGE);
			}

			if (!Directory.Exists(targetPath))
			{
				throw new DirectoryNotFoundException(PLUG_IN_DIRECTORY_NOT_FOUND_MESSAGE);
			}

			this.InstallPlugins(this.GetPluginsFolder(), targetPath);
		}


		public override void Rollback(IDictionary savedState)
		{
			// No need to do anything here.
		}


		public override void Uninstall(IDictionary savedState)
		{

			string targetPath = this.GetWLWPluginsFolder();

			if (null != targetPath && Directory.Exists(targetPath))
			{
				this.UninstallPlugins(this.GetPluginsFolder(), targetPath);
			}

		}


		private string GetAssemblyFolder()
		{
			return new FileInfo(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName).Directory.FullName;
		}


		private string GetPluginsFolder()
		{
			return string.Format(@"{0}\Plugins", this.GetAssemblyFolder());
		}


		private string GetWLWPluginsFolder()
		{
			RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows Live Writer");
			if (null != key)
			{
				string installDir = key.GetValue("InstallDir") as string;
				return string.Format(@"{0}\Plugins", installDir);
			}

			return null;
		}


		private void InstallPlugins(string sourcePath, string pluginsPath)
		{
			// The Setup project itself placed the output from Plugin project and the PluginsInstaller
			// project in the original install directory, either the default, or one specified by 
			// the user:
			foreach (string file in Directory.GetFiles(sourcePath))
			{
				FileInfo sourceFile = new FileInfo(file);

				FileInfo targetFile = new FileInfo(string.Format(@"{0}\{1}", pluginsPath, sourceFile.Name));
				File.Copy(sourceFile.FullName, targetFile.FullName, true);
			}
		}


		private void UninstallPlugins(string sourcePath, string pluginsPath)
		{
			foreach (string file in Directory.GetFiles(sourcePath))
			{
				FileInfo sourceFile = new FileInfo(file);
				FileInfo targetFile = new FileInfo(string.Format(@"{0}\{1}", pluginsPath, sourceFile.Name));

				if (File.Exists(targetFile.FullName))
				{
					File.Delete(targetFile.FullName);
				}
			}
		}

	}
}
