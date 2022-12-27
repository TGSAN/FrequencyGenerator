using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Foundation.Metadata;
using Windows.UI.Core;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace FrequencyGenerator
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            // Hide default title bar.
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            UpdateTitleBarLayout(coreTitleBar);

            // 设置自定义标题栏作为可拖动区域
            Window.Current.SetTitleBar(AppTitleBar);

            // 处理如移动到不同 DPI 的屏幕上的情况
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            // 处理如切换到全屏模式下的情况
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;

            Window.Current.Activated += Current_Activated;

            ApplicationView.GetForCurrentView().Title = "Frequency Generator";
        }

        private string GetTitleName()
        {
            return ApplicationView.GetForCurrentView().Title;
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            // 确保自定义标题栏不与窗口标题控件重叠
            Thickness currMargin = AppTitleBar.Margin;
            AppTitleBar.Margin = new Thickness() { Left = currMargin.Left, Top = currMargin.Top, Right = coreTitleBar.SystemOverlayRightInset, Bottom = currMargin.Bottom };
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                AppTitleBar.Visibility = Visibility.Visible;
            }
            else
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        // 根据应用程序的非活动/活动状态更新标题栏
        private void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs args)
        {
            var resource = args.WindowActivationState == CoreWindowActivationState.Deactivated ? "TextFillColorDisabledBrush" : "TextFillColorPrimaryBrush";

            AppTitleText.Foreground = (SolidColorBrush)Application.Current.Resources[resource];
            PreviewTitleText.Foreground = (SolidColorBrush)Application.Current.Resources[resource];
        }

        // 根据 NavigationView 的 DisplayMode 更新 TitleBar 的内容布局。
        private void NavigationViewControl_DisplayModeChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewDisplayModeChangedEventArgs args)
        {
            double topIndent = 16.0;
            double expandedIndent = sender.CompactPaneLength;
            double minimalIndent = sender.CompactPaneLength * 2;

            // 如果后退按钮不可见，减少标题栏内容的缩进
            if (NavigationViewControl.IsBackButtonVisible.Equals(Microsoft.UI.Xaml.Controls.NavigationViewBackButtonVisible.Collapsed))
            {
                minimalIndent = expandedIndent;
            }

            Thickness currMargin = AppTitleBar.Margin;

            if (sender.PaneDisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Top)
            {
                NavigationViewControl.IsTitleBarAutoPaddingEnabled = true;
            }
            else
            {
                NavigationViewControl.IsTitleBarAutoPaddingEnabled = false;
            }

            // 根据 NavigationView 的 DisplayMode 来设置 TitleBar 的边距
            if (sender.PaneDisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewPaneDisplayMode.Top)
            {
                AppTitleBar.Margin = new Thickness(topIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
            }
            else if (sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
            {
                AppTitleBar.Margin = new Thickness(minimalIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
            }
            else
            {
                AppTitleBar.Margin = new Thickness(expandedIndent, currMargin.Top, currMargin.Right, currMargin.Bottom);
            }

            UpdateAppTitleMargin(sender);
        }

        private void UpdateAppTitleMargin(Microsoft.UI.Xaml.Controls.NavigationView sender)
        {
            const int smallLeftIndent = 4, largeLeftIndent = 24;

            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                AppTitleBar.TranslationTransition = new Vector3Transition();

                if ((sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
                         sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
                {
                    AppTitleBar.Translation = new System.Numerics.Vector3(smallLeftIndent, 0, 0);
                }
                else
                {
                    AppTitleBar.Translation = new System.Numerics.Vector3(largeLeftIndent, 0, 0);
                }
            }
            else
            {
                Thickness currMargin = AppTitleBar.Margin;

                if ((sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Expanded && sender.IsPaneOpen) ||
                         sender.DisplayMode == Microsoft.UI.Xaml.Controls.NavigationViewDisplayMode.Minimal)
                {
                    AppTitleBar.Margin = new Thickness() { Left = smallLeftIndent, Top = currMargin.Top, Right = currMargin.Right, Bottom = currMargin.Bottom };
                }
                else
                {
                    AppTitleBar.Margin = new Thickness() { Left = largeLeftIndent, Top = currMargin.Top, Right = currMargin.Right, Bottom = currMargin.Bottom };
                }
            }
        }

        private void NavigationViewControl_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(MainPageSubPages.WIPPage), null, new EntranceNavigationTransitionInfo());
            }
            else
            {
                var selectedItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.SelectedItem;
                sender.Header = (string)selectedItem.Content;
                string selectedItemTag = (string)selectedItem.Tag;
                Debug.WriteLine("Before hitting sample page " + selectedItemTag);
                // sender.Header = "Sample Page " + selectedItemTag.Substring(selectedItemTag.Length - 1);
                
                string pageName = $"{this.GetType().Namespace}.MainPageSubPages.{selectedItemTag}";
                Type pageType = Type.GetType(pageName);
                if (pageType != null)
                {
                    ContentFrame.Navigate(pageType, null, new EntranceNavigationTransitionInfo());
                }
                else
                {
                    ContentFrame.Navigate(typeof(MainPageSubPages.WIPPage), null, new EntranceNavigationTransitionInfo());
                }
            }
        }

        private void NavigationViewControl_PaneOpening(Microsoft.UI.Xaml.Controls.NavigationView sender, object args)
        {
            UpdateAppTitleMargin(sender);
        }

        private void NavigationViewControl_PaneClosing(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewPaneClosingEventArgs args)
        {
            UpdateAppTitleMargin(sender);
        }
    }
}
