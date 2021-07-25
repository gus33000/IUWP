using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.System.Profile;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static IUWP.UpdateHistoryClass;

namespace IUWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UpdateHistoryPage : Page
    {
        private readonly DataTemplate dtSmall;
        private readonly DataTemplate dtEnlarged;

        private readonly ObservableCollection<Package> InstalledPkgs = new();

        private class Package
        {
            public string Title { get; set; }
            public string State { get; set; }
        }

        public UpdateHistoryPage()
        {
            InitializeComponent();
            dtSmall = (DataTemplate)Resources["dtSmall"];
            dtEnlarged = (DataTemplate)Resources["dtEnlarged"];
            Load();
        }

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

        public void Load()
        {
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong build = (version & 0x00000000FFFF0000L) >> 16;

            RunInThreadPool(async () =>
            {
                FirstPartyUtils.DeviceUpdate svc = new();
                uint ret = svc.Initialize();
                if (ret != 0)
                {
                    return;
                }

                ret = svc.GetLogs(514, out string logPath);
                if (ret != 0)
                {
                    return;
                }

                byte[] bytes = System.IO.File.ReadAllBytes(logPath);

                CabExtract.ExtractFile(bytes, "UpdateHistory.xml", out byte[] outdata, out int length);


                string historystr = System.Text.Encoding.UTF8.GetString(outdata);
                UpdateHistory updatehistory = XmlStringExtensions.XmlDeserializeFromString<UpdateHistory>(historystr);

                System.Collections.Generic.List<UpdateEvent> updlist = updatehistory.UpdateEvents.UpdateEvent;

                foreach (UpdateEvent update in updlist)
                {
                    Package Package = new()
                    {
                        Title = update.UpdateOSOutput.Description == null ? DateTime.Parse(update.DateTime.Replace(":: ", "")).ToString() : update.UpdateOSOutput.Description + " (" + DateTime.Parse(update.DateTime.Replace(":: ", "")).ToString() + ")",
                        State = update.UpdateOSOutput.UpdateState
                    };
                    InstalledPkgs.Add(Package);
                }

                await RunInUIThread(() =>
                {
                    MainListView.ItemsSource = InstalledPkgs;
                    ProgressRing.IsActive = false;
                    MainScroll.Visibility = Windows.UI.Xaml.Visibility.Visible;
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
