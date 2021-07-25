using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.System.Profile;
using Windows.System.Threading;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace IUWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InstalledPackagesPage : Page
    {
        private readonly DataTemplate dtSmall;
        private readonly DataTemplate dtEnlarged;

        private readonly ObservableCollection<Package> InstalledPkgs = new();

        private async Task RunInUIThread(Action function)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                function();
            });
        }

        private async void RunInThreadPool(Action function)
        {
            await ThreadPool.RunAsync(x =>
            {
                function();
            });
        }

        private class Package
        {
            public string Partition { get; set; }
            public string PackageName { get; set; }
            public string Version { get; set; }
        }

        public InstalledPackagesPage()
        {
            InitializeComponent();
            dtSmall = (DataTemplate)Resources["dtSmall"];
            dtEnlarged = (DataTemplate)Resources["dtEnlarged"];
            Load();
        }

        public async void Load()
        {
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong build = (version & 0x00000000FFFF0000L) >> 16;

            DeviceUpdateUtils.DeviceUpdateKeys.IsDuaSessionInProgress(out bool isinprogress);

            if (isinprogress)
            {
                await new MessageDialog("An update session is currently in progress, this functionality will remain disabled until the completion of the current update session.").ShowAsync();
                Frame frame = (Frame)Window.Current.Content;
                if (frame.CanGoBack)
                {
                    frame.GoBack();
                }
                return;
            }

            RunInThreadPool(async () =>
            {
                FirstPartyUtils.DeviceUpdate svc = new();
                uint ret = svc.Initialize();
                if (ret != 0)
                {
                    return;
                }

                ret = svc.GetLogs(0, out string logPath);
                if (ret != 0)
                {
                    return;
                }

                byte[] bytes = System.IO.File.ReadAllBytes(logPath);

                CabExtract.ExtractFile(bytes, "InstalledPackages.csv", out byte[] outdata, out int length);

                string installedpackages = System.Text.Encoding.UTF8.GetString(outdata);
                System.Collections.Generic.IEnumerable<string> listpkgs = installedpackages.Split('\n').Skip(1);

                foreach (string pkg in listpkgs)
                {
                    if (pkg == "")
                    {
                        continue;
                    }

                    Package pkgclass = new()
                    {
                        Partition = pkg.Split(',').ElementAt(0),
                        PackageName = pkg.Split(',').ElementAt(1),
                        Version = pkg.Split(',').ElementAt(2)
                    };
                    InstalledPkgs.Add(pkgclass);
                }

                await RunInUIThread(() =>
                {
                    MainListView.ItemsSource = InstalledPkgs;
                    ProgressRing.IsActive = false;
                    MainScroll.Visibility = Visibility.Visible;
                });
            });
        }

        private void MainListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                foreach (object item in e.RemovedItems)
                {
                    ((ListViewItem)(sender as ListView).ContainerFromItem(item)).ContentTemplate = dtSmall;
                }

                foreach (object item in e.AddedItems)
                {
                    ((ListViewItem)(sender as ListView).ContainerFromItem(e.AddedItems[0])).ContentTemplate = dtEnlarged;
                }
            }
        }

        private void MainListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListView lv = (sender as ListView);

            if (lv.SelectedItem == null)
            {
                lv.SelectedItem = e.ClickedItem;
            }

            else
            {
                if (lv.SelectedItem.Equals(e.ClickedItem))
                {
                    lv.SelectedItem = null;
                }

                else
                {
                    lv.SelectedItem = e.ClickedItem;
                }
            }
        }
    }
}
