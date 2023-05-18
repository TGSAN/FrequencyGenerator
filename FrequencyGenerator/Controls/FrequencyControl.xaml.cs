using FrequencyGenerator.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FrequencyGenerator.Controls
{
    public sealed partial class FrequencyControl : UserControl
    {
        public static readonly DependencyProperty FrequencyProperty = DependencyProperty.Register(
            "Frequency",
            typeof(int),
            typeof(FrequencyControl),
            new PropertyMetadata(0)
        );

        public static readonly DependencyProperty WaveTypeProperty = DependencyProperty.Register(
            "WaveType",
            typeof(int),
            typeof(WaveType),
            new PropertyMetadata(Utils.WaveType.Sine)
        );

        public int Frequency
        {
            get { return (int)GetValue(FrequencyProperty); }
            set {
                if (Frequency != value)
                {
                    SetValue(FrequencyProperty, value);
                    FrequencyChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public Utils.WaveType WaveType
        {
            get { return (Utils.WaveType)GetValue(WaveTypeProperty); }
            set
            {
                if (WaveType != value)
                {
                    SetValue(WaveTypeProperty, value);
                    WaveTypeChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public event EventHandler FrequencyChanged;
        public event EventHandler WaveTypeChanged;

        public FrequencyControl()
        {
            this.InitializeComponent();
            waveTypeComboBox.ItemsSource = Enum.GetValues(typeof(Utils.WaveType)).Cast<Utils.WaveType>();
        }

        private Geometry GetWaveIconData(Utils.WaveType type)
        {
            switch (type)
            {
                case Utils.WaveType.Sine:
                    return (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M22 2V4C20.26 4 19 8.58 17.96 12.27C16.57 17.27 15.26 22 12 22C8.74 22 7.43 17.27 6.04 12.27C5 8.58 3.74 4 2 4V2C5.26 2 6.57 6.73 7.96 11.73C9 15.42 10.26 20 12 20C13.74 20 15 15.42 16.04 11.73C17.43 6.73 18.74 2 22 2Z");
                case Utils.WaveType.Square:
                    return (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M2 2V12H4V4H11V22H22V12H20V20H13V2H2Z");
                case Utils.WaveType.Triangle:
                    return (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M22 12L17 22L7.1 6.04L4.24 12H2L7 2L16.9 17.96L19.76 12H22Z");
                case Utils.WaveType.Sawtooth:
                    return (Geometry)XamlBindingHelper.ConvertValue(typeof(Geometry), "M11 22V6.83L2 16V13.17L13 2V17.17L22 8V10.83L11 22Z");
                default:
                    return null;
            }
        }
    }
}
