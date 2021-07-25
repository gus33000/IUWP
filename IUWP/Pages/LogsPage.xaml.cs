using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.System.Profile;
using Windows.System.Threading;
using Windows.UI.Xaml.Controls;

namespace IUWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogsPage : Page
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

        private string uastr = "";
        private string imgupdstr = "";
        private string imgupdcbsstr = "";

        private string etwstr = "";
        private string reportstr = "";
        private string resetstr = "";
        private string updtskstr = "";

        public LogsPage()
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

                CabExtract.ExtractFile(bytes, "ImgUpd.log", out byte[] outdata, out int length);
                if (outdata != null)
                {
                    imgupdstr = System.Text.Encoding.UTF8.GetString(outdata);
                }

                CabExtract.ExtractFile(bytes, "ImgUpd.log.cbs.log", out outdata, out length);
                if (outdata != null)
                {
                    imgupdcbsstr = System.Text.Encoding.UTF8.GetString(outdata);
                }

                CabExtract.ExtractFile(bytes, "UpdateAgent.log", out outdata, out length);
                if (outdata != null)
                {
                    uastr = System.Text.Encoding.UTF8.GetString(outdata);
                }

                CabExtract.ExtractFile(bytes, "FlushEtwSessions.log", out outdata, out length);
                if (outdata != null)
                {
                    etwstr = System.Text.Encoding.UTF8.GetString(outdata);
                }

                CabExtract.ExtractFile(bytes, "ReportingEvents.log", out outdata, out length);
                if (outdata != null)
                {
                    reportstr = System.Text.Encoding.Unicode.GetString(outdata);
                }

                CabExtract.ExtractFile(bytes, "ResetLog.txt", out outdata, out length);
                if (outdata != null)
                {
                    resetstr = System.Text.Encoding.Unicode.GetString(outdata);
                }

                CabExtract.ExtractFile(bytes, "UpdateTaskSchedules.txt", out outdata, out length);
                if (outdata != null)
                {
                    updtskstr = System.Text.Encoding.Unicode.GetString(outdata);
                }

                await RunInUIThread(() =>
                {
                    LogComboBox.SelectedIndex = 0;
                    SetLogText(imgupdstr);

                    ProgressRing.IsActive = false;
                    MainScroll.Visibility = Windows.UI.Xaml.Visibility.Visible;
                });
            });
        }

        public void SetLogText(string text)
        {
            List<string> splittext = text.Split(new char[] { '\n' }).ToList();
            LList.ItemsSource = splittext;
        }

        private void LogComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch ((sender as ComboBox).SelectedIndex)
                {
                    case 0:
                        {
                            SetLogText(imgupdstr);
                            break;
                        }
                    case 1:
                        {
                            SetLogText(imgupdcbsstr);
                            break;
                        }
                    case 2:
                        {
                            SetLogText(uastr);
                            break;
                        }
                    case 3:
                        {
                            SetLogText(etwstr);
                            break;
                        }
                    case 4:
                        {
                            SetLogText(reportstr);
                            break;
                        }
                    case 5:
                        {
                            SetLogText(resetstr);
                            break;
                        }
                    case 6:
                        {
                            SetLogText(updtskstr);
                            break;
                        }
                }
            }
            catch
            {

            }
        }
    }
}
