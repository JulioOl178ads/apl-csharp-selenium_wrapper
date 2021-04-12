using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace SeleniumManager
{
    public class SeleniumUpdater
    {
        internal static void DownloadChromeDriver()
        {
            KillChromeDriverProcesses();
            string path = GetChromeVersion();
            var version = GetProductVersion(path);
            var urlToDownload = GetUrlToDownload(version);
            DownloadChromeDriver(urlToDownload);
        }

        private static string GetChromeVersion()
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\chrome.exe"))
            {
                if (key != null)
                {
                    Object o = key.GetValue("");
                    if (!String.IsNullOrEmpty(o.ToString()))
                    {
                        return o.ToString();
                    }
                    else
                    {
                        throw new ArgumentException("Unable to get version because chrome registry value was null");
                    }
                }
                else
                {
                    throw new ArgumentException("Unable to get version because chrome registry path was null");
                }
            }
        }

        private static string GetProductVersion(string productVersionPath)
        {
            if (String.IsNullOrEmpty(productVersionPath))
            {
                throw new ArgumentException("Unable to get version because path is empty");
            }

            if (!File.Exists(productVersionPath))
            {
                throw new FileNotFoundException("Unable to get version because path specifies a file that does not exists");
            }

            var versionInfo = FileVersionInfo.GetVersionInfo(productVersionPath);
            if (versionInfo != null && !String.IsNullOrEmpty(versionInfo.FileVersion))
            {
                return versionInfo.FileVersion;
            }
            else
            {
                throw new ArgumentException("Unable to get version from path because the version is either null or empty: " + productVersionPath);
            }
        }

        private static string GetUrlToDownload(string version)
        {
            if (String.IsNullOrEmpty(version))
            {
                throw new ArgumentException("Unable to get url because version is empty");
            }

            string html = string.Empty;
            string urlToPathLocation = @"https://chromedriver.storage.googleapis.com/LATEST_RELEASE_" + String.Join(".", version.Split('.').Take(3));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPathLocation);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
                html = reader.ReadToEnd();


            if (String.IsNullOrEmpty(html))
                throw new WebException("Unable to get version path from website");

            return "https://chromedriver.storage.googleapis.com/" + html + "/chromedriver_win32.zip";
        }

        private static void KillChromeDriverProcesses()
        {
            var processes = Process.GetProcessesByName("chromedriver");
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch
                {

                }
            }
        }

        private static void DownloadChromeDriver(string urlToDownload)
        {
            var chromeZipUrl = Environment.CurrentDirectory + "\\chromedriver.zip";
            var chromeExeUrl = Environment.CurrentDirectory + "\\chromedriver.exe";
            var currentDir = Environment.CurrentDirectory;

            if (String.IsNullOrEmpty(urlToDownload))
                throw new ArgumentException("Unable to get url because UrlToDownload is empty");


            using (var client = new WebClient())
            {
                if (File.Exists(chromeZipUrl))
                    FolderManagement.DeleteFile(chromeZipUrl);

                client.DownloadFile(urlToDownload, "chromedriver.zip");

                if (File.Exists(chromeZipUrl) && File.Exists(chromeExeUrl))
                    FolderManagement.DeleteFile(chromeExeUrl);

                if (File.Exists(chromeZipUrl))
                    ZipFile.ExtractToDirectory(chromeZipUrl, currentDir);
            }
        }

       
    }
}
