using System;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace IUWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DUSvcPage : Page
    {
        private const int SERVICE_CONTINUE_PENDING = 0x00000005;
        private const int SERVICE_PAUSE_PENDING = 0x00000006;
        private const int SERVICE_PAUSED = 0x00000007;
        private const int SERVICE_RUNNING = 0x00000004;
        private const int SERVICE_START_PENDING = 0x00000002;
        private const int SERVICE_STOP_PENDING = 0x00000003;
        private const int SERVICE_STOPPED = 0x00000001;

        public DUSvcPage()
        {
            InitializeComponent();
            Check();
        }

        private void Check()
        {
            WinPRTUtils.RegistryKey dukey = DeviceUpdateUtils.DeviceUpdateKeys.EnumDUKeys();

            foreach (WinPRTUtils.RegistryKeyValue val in dukey.Values)
            {
                Debug.WriteLine(val.Name + "," + val.ValueType.ToString() + "," + val.Value);
            }

            foreach (WinPRTUtils.RegistryKey key in dukey.SubKeys)
            {
                Debug.WriteLine(key.Name);
                foreach (WinPRTUtils.RegistryKeyValue val in key.Values)
                {
                    Debug.WriteLine(val.Name + "," + val.ValueType.ToString() + "," + val.Value);
                }

                foreach (WinPRTUtils.RegistryKey key2 in key.SubKeys)
                {
                    Debug.WriteLine(key2.Name);
                    foreach (WinPRTUtils.RegistryKeyValue val in key2.Values)
                    {
                        Debug.WriteLine(val.Name + "," + val.ValueType.ToString() + "," + val.Value);
                    }
                }
            }

            DeviceUpdateUtils.DeviceUpdateKeys.IsDuaServiceRunning(out int svcstatus);

            if (svcstatus == SERVICE_RUNNING)
            {
                DUSvcToggle.IsOn = true;
            }

            switch (svcstatus)
            {
                case SERVICE_CONTINUE_PENDING:
                    {
                        DUSvcToggle.Header = "Device Update service state: Continue pending";
                        break;
                    }
                case SERVICE_PAUSED:
                    {
                        DUSvcToggle.Header = "Device Update service state: Paused";
                        break;
                    }
                case SERVICE_PAUSE_PENDING:
                    {
                        DUSvcToggle.Header = "Device Update service state: Pause pending";
                        break;
                    }
                case SERVICE_RUNNING:
                    {
                        DUSvcToggle.Header = "Device Update service state: Running";
                        break;
                    }
                case SERVICE_START_PENDING:
                    {
                        DUSvcToggle.Header = "Device Update service state: Start pending";
                        break;
                    }
                case SERVICE_STOPPED:
                    {
                        DUSvcToggle.Header = "Device Update service state: Stopped";
                        break;
                    }
                case SERVICE_STOP_PENDING:
                    {
                        DUSvcToggle.Header = "Device Update service state: Stop pending";
                        break;
                    }
            }

            DeviceUpdateUtils.DeviceUpdateKeys.IsDuaSessionInProgress(out bool sessionstatus);

            if (sessionstatus)
            {
                SessionToggle.IsOn = true;
            }

            DeviceUpdateUtils.DeviceUpdateKeys.IsThDuaSessionInProgress(out bool sessionthstatus);

            if (sessionstatus)
            {
                SessionThToggle.IsOn = true;
            }
        }

        private async void StopDuaService()
        {
            int ret = DeviceUpdateUtils.DeviceUpdateKeys.StopDuaService();
            if (ret != 0)
            {
                await new MessageDialog("StopDuaService returned status " + ret).ShowAsync();
            }

            Check();
        }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            StopDuaService();
        }
    }
}
