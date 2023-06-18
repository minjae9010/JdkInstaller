using JavaInstaller.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace JavaInstaller
{
    public class AddWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<string> _margerVersionList;
        public List<string> MargerVersionList
        {
            get { return _margerVersionList; }
            set
            {
                _margerVersionList = value;
                NotifyPropertyChanged(nameof(MargerVersionList));
            }
        }

        private List<string> _distributionVersions;
        public List<string> DistributionVersions
        {
            get { return _distributionVersions; }
            set
            {
                _distributionVersions = value;
                NotifyPropertyChanged(nameof(DistributionVersions));
            }
        }

        private List<string> _productList;
        public List<string> ProductList
        {
            get { return _productList; }
            set
            {
                _productList = value;
                NotifyPropertyChanged(nameof(ProductList));
            }
        }

        public AddWindowViewModel()
        {
            LoadMajorVersions();
        }

        private async void LoadMajorVersions()
        {
            DiscoAPI api = new DiscoAPI();
            List<string> majorVersions = await api.GetMajorVersions();
            MargerVersionList = majorVersions;
        }
    }

    public partial class AddWindow : Window
    {
        private AddWindowViewModel viewModel;
        private bool isInstallationInProgress = false;

        public AddWindow()
        {
            InitializeComponent();

            viewModel = new AddWindowViewModel();
            DataContext = viewModel;
        }

        Dictionary<string, string> productList = new Dictionary<string, string>();
        Dictionary<string, string> versionList = new Dictionary<string, string>();

        private async void ChangeJdkVer(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectedVersion = comboBox.SelectedValue as string;

            if (!string.IsNullOrEmpty(selectedVersion))
            {
                DiscoAPI api = new DiscoAPI();
                productList = await api.GetDistributionVersions(selectedVersion);

                viewModel.ProductList = productList.Keys.ToList();
            }
        }

        private void CloseBtn(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void JdkInstallBtn(object sender, RoutedEventArgs e)
        {
            string ver = version.SelectedValue as string;
            string jdkVer = JdkVer.SelectedValue as string;
            string selectProduct = product.SelectedValue as string;

            if (!String.IsNullOrEmpty(ver))
            {
                DiscoAPI discoAPI = new DiscoAPI();
                if (versionList.TryGetValue(ver, out string url))
                {
                    Console.Out.WriteLine(url);
                    string path = await InstallJDK(url); // Replace this with your JDK installation logic

                    jdkInfo jdk = new jdkInfo();
                    jdk.path = path;
                    jdk.version = ver;
                    jdk.jdkver = jdkVer;
                    jdk.product = selectProduct;

                    DbDAO dbDAO = new DbDAO();
                    dbDAO.InsertDataIntoTable(jdk);

                    this.Close();
                }
            }
        }

        private async Task<string> InstallJDK(string url)
        {
            // Show the progress bar and set its visibility to Visible
            Dispatcher.Invoke(() =>
            {
                progressBar.Visibility = Visibility.Visible;
                isInstallationInProgress = true;
            });

            // Download the JDK zip file
            string zipFilePath = Path.Combine(Path.GetTempPath(), "jdk.zip");
            using (WebClient webClient = new WebClient())
            {
                // Subscribe to the download progress changed event
                webClient.DownloadProgressChanged += (sender, e) =>
                {
                    // Update the progress bar value
                    Dispatcher.Invoke(() => progressBar.Value = e.ProgressPercentage);
                };

                // Download the JDK zip file asynchronously
                await webClient.DownloadFileTaskAsync(new Uri(url), zipFilePath);
            }

            // Unzip the JDK zip file to the specified subfolder
            string extractFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Jdk");
            if (!Directory.Exists(extractFolderPath))
            {
                Directory.CreateDirectory(extractFolderPath);
            }

            string extractFolderPathAndName = "";

            using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string entryDestinationPath = Path.Combine(extractFolderPath, entry.FullName);
                    extractFolderPathAndName = Path.GetDirectoryName(entryDestinationPath);
                    if (!string.IsNullOrEmpty(entry.Name))
                    {
                        entry.ExtractToFile(entryDestinationPath, true);
                    }
                    else
                    {
                        Directory.CreateDirectory(entryDestinationPath);
                    }
                }
            }

            // Delete the downloaded Zip file
            File.Delete(zipFilePath);

            // Hide the progress bar after the installation is complete
            Dispatcher.Invoke(() =>
            {
                progressBar.Visibility = Visibility.Collapsed;
                isInstallationInProgress = false;
            });

            return extractFolderPathAndName;
        }


        private async void ChangedProduct(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string selectProduct = comboBox.SelectedValue as string;
            string selectJdkVer = JdkVer.SelectedValue as string;

            if (!string.IsNullOrEmpty(selectProduct))
            {
                DiscoAPI api = new DiscoAPI();

                if (productList.TryGetValue(selectProduct, out string distribution))
                {
                    versionList = await api.GetPackageVersionAndDownload(selectJdkVer, distribution);
                    viewModel.DistributionVersions = versionList.Keys.ToList();
                }
            }
        }
    }
}
