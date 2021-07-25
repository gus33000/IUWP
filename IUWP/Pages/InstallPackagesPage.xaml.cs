using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class InstallPackagesPage : Page
    {
        private readonly DataTemplate dtSmall;
        private readonly DataTemplate dtEnlarged;

        public InstallPackagesPage()
        {
            InitializeComponent();
            dtSmall = (DataTemplate)Resources["dtSmall"];
            dtEnlarged = (DataTemplate)Resources["dtEnlarged"];
            packagestoinstall.CollectionChanged += Packagestoinstall_CollectionChanged;
            MainListView.ItemsSource = packagestoinstall;
            FList.ItemsSource = packagestoinstall;
            Load();
        }

        private void Packagestoinstall_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if ((sender as ObservableCollection<StorageFile>).Count != 0)
            {
                InstallCabsButton.IsEnabled = true;
            }
        }

        public async void Load()
        {
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

            ProgressRing.IsActive = false;
            MainScroll.Visibility = Visibility.Visible;
        }

        private readonly ObservableCollection<StorageFile> packagestoinstall = new();

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

        private int currentfile = 0;

        private async void BrowseCabsButton_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker openPicker = new()
            {
                ViewMode = PickerViewMode.List,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            openPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await openPicker.PickSingleFolderAsync();//IReadOnlyList<StorageFile> files = await (await StorageFolder.GetFolderFromPathAsync(@"C:\Data\Users\Public\Documents\newnewcshell\")).GetFilesAsync();//await openPicker.PickMultipleFilesAsync();
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            if (files.Count > 0)
            {
                foreach (StorageFile file in files)
                {
                    packagestoinstall.Add(file);
                }
            }
        }

        private void InstallCabsButton_Click(object sender, RoutedEventArgs e)
        {
            int totalpackages = packagestoinstall.Count;

            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong build = (version & 0x00000000FFFF0000L) >> 16;

            FirstPartyUtils.DeviceUpdate svc = new();
            uint ret = svc.Initialize();
            if (ret != 0)
            {
                return;
            }

            ret = svc.GetSharedDataFolder(out string sharedDataFolder);
            if (ret != 0)
            {
                return;
            }

            PackageHeader.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            InstallCabsButton.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            UpdateHeader.Visibility = Windows.UI.Xaml.Visibility.Visible;

            ProgressText.Text = "Initializing updates...";
            CopyProgress.IsIndeterminate = true;

            RunInThreadPool(async () =>
            {
                IEnumerable<string> files2 = Directory.EnumerateFiles(sharedDataFolder);

                foreach (string file in files2)
                {
                    System.IO.File.Delete(file);
                }

                if (totalpackages > 0)
                {
                    await RunInUIThread(() =>
                    {
                        CopyProgress.IsIndeterminate = false;
                        CopyProgress.Maximum = totalpackages * 100;
                    });

                    List<StorageFile> packagestoinstallcopy = new();

                    foreach (StorageFile file in packagestoinstall)
                    {
                        packagestoinstallcopy.Add(file);
                    }

                    foreach (StorageFile file in packagestoinstallcopy)
                    {
                        currentfile++;

                        FileInfo _source = new(file.Path);
                        FileInfo _destination = new(sharedDataFolder + @"\" + file.Name);

                        if (_destination.Exists)
                        {
                            _destination.Delete();
                        }

                        _source.CopyTo(_destination, async (x) =>
                        {
                            await RunInUIThread(() =>
                            {
                                CopyProgress.Value = x + (currentfile - 1) * 100;
                                ProgressText.Text = "Copying updates " + Math.Round(CopyProgress.Value / totalpackages, 0) + "%";
                            });
                        });

                        await RunInUIThread(() =>
                        {
                            CopyProgress.Value = currentfile * 100;
                            ProgressText.Text = "Copying updates " + Math.Round(CopyProgress.Value / totalpackages, 0) + "%";
                            packagestoinstall.Remove(file);
                        });

                        if (currentfile == totalpackages)
                        {
                            ret = svc.StartInstall();
                            if (ret != 0)
                            {
                                return;
                            }

                            await RunInUIThread(async () =>
                            {
                                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:windowsupdate"));
                                Frame frame = (Frame)Window.Current.Content;
                                if (frame.CanGoBack)
                                {
                                    frame.GoBack();
                                }
                                return;
                            });
                        }
                    }
                }
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
