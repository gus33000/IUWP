using System;
using System.Threading.Tasks;
using Windows.System.Profile;
using Windows.System.Threading;
using Windows.UI.Xaml.Controls;

namespace IUWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ROMInfoPage : Page
    {
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


        public ROMInfoPage()
        {
            InitializeComponent();
            Load();
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

                CabExtract.ExtractFile(bytes, "OEMInput.xml", out byte[] outdata, out int length);
                string oeminputstr = System.Text.Encoding.UTF8.GetString(outdata).Replace("﻿<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");

                OEMInput oeminput = XmlStringExtensions.XmlDeserializeFromString<OEMInput>(oeminputstr);

                CabExtract.ExtractFile(bytes, "OEMDevicePlatform.xml", out outdata, out length);
                string oemdeviceplatstr = System.Text.Encoding.UTF8.GetString(outdata).Replace("﻿<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");

                OEMDevicePlatform oemdeviceplat = XmlStringExtensions.XmlDeserializeFromString<OEMDevicePlatform>(oemdeviceplatstr);

                await RunInUIThread(() =>
                {
                    Platid.Text = oemdeviceplat.DevicePlatformID;

                    Desc.Text = oeminput.Description;
                    SV.Text = oeminput.SV;
                    SOC.Text = oeminput.SOC;
                    Device.Text = oeminput.Device;
                    RelType.Text = oeminput.ReleaseType;
                    BldType.Text = oeminput.BuildType;
                    BootUILp.Text = oeminput.BootUILanguage;
                    BootLoc.Text = oeminput.BootLocale;

                    UILpsLst.ItemsSource = oeminput.SupportedLanguages.UserInterface?.Language;
                    KbLpsLst.ItemsSource = oeminput.SupportedLanguages.Keyboard?.Language;
                    SpLpsLst.ItemsSource = oeminput.SupportedLanguages.Speech?.Language;

                    ResLst.ItemsSource = oeminput.Resolutions?.Resolution;

                    MSFtLst.ItemsSource = oeminput.Features.Microsoft?.Feature;
                    OEMFtLst.ItemsSource = oeminput.Features.OEM?.Feature;

                    ProgressRing.IsActive = false;
                    MainScroll.Visibility = Windows.UI.Xaml.Visibility.Visible;
                });
            });
        }
    }
}
