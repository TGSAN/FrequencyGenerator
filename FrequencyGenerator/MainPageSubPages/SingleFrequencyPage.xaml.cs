using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Core;
using FrequencyGenerator.Controls;
using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using FrequencyGenerator.Utils;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FrequencyGenerator.MainPageSubPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SingleFrequencyPage : Page
    {
        private WasapiOutRT player;
        private readonly PcmWaveProvider sineProvider;

        public SingleFrequencyPage()
        {
            sineProvider = new PcmWaveProvider(48000);
            sineProvider.PortamentoTime = 0;
            player = new WasapiOutRT(NAudio.CoreAudioApi.AudioClientShareMode.Shared, 100);
            player.Init(new SampleToWaveProvider(sineProvider)).Wait();
            this.InitializeComponent();
            frequencyControl_FrequencyChanged(null, null);
            frequencyControl_WaveTypeChanged(null, null);
            volumeSlider_ValueChanged(null, null);
            balanceSlider_ValueChanged(null, null);
        }

        private string GetPlayButtonTextByState(bool? state)
        {
            if (state.HasValue)
            {
                return state.Value ? "Stop" : "Play";
            }
            else
            {
                return "Play";
            }
        }

        private string GetPlayButtonGlyphByState(bool? state)
        {
            return state.GetValueOrDefault() ? "\uE71A" : "\uE768";
        }

        private void playToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (playToggleButton.IsChecked.GetValueOrDefault())
            {
                player.Play();
            }
            else
            {
                player.Stop();
            }
        }

        private void frequencyControl_FrequencyChanged(object sender, EventArgs e)
        {
            sineProvider.Frequency = frequencyControl.Frequency;
        }

        private void frequencyControl_WaveTypeChanged(object sender, EventArgs e)
        {
            sineProvider.SetWaveType(frequencyControl.WaveType);
        }

        private void volumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            sineProvider.Volume = (float)volumeSlider.Value / 100;
        }
        
        private void balanceSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var balance = (float)balanceSlider.Value / 100;
            var left = (balance - 1) / 2 * -1;
            var right = (balance + 1) / 2;
            sineProvider.VolumeL = left;
            sineProvider.VolumeR = right;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            player.Stop();
        }
    }
}
