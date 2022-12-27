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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FrequencyGenerator.Controls
{
    public sealed partial class GroupItem : UserControl
    {
        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(
            "HeaderText",
            typeof(string),
            typeof(GroupItem),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty ContentTextProperty = DependencyProperty.Register(
            "ContentText",
            typeof(string),
            typeof(GroupItem),
            new PropertyMetadata(null)
        );

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public string ContentText
        {
            get { return (string)GetValue(ContentTextProperty); }
            set { SetValue(ContentTextProperty, value); }
        }

        public UIElementCollection Children
        {
            get { return subControl.Children; }
        }

        public GroupItem()
        {
            this.InitializeComponent();
            this.RegisterPropertyChangedCallback(HeaderTextProperty, (s, e) =>
            {
                headerTextBlock.Text = HeaderText;
                headerTextBlock.Visibility = TextToVisibility(HeaderText);
            });
            this.RegisterPropertyChangedCallback(ContentTextProperty, (s, e) =>
            {
                contentTextBlock.Text = ContentText;
                contentTextBlock.Visibility = TextToVisibility(ContentText);
            });
        }

        private Visibility TextToVisibility(string text)
        {
            return string.IsNullOrEmpty(text) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
