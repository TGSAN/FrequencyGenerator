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

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace FrequencyGenerator.MainPageSubPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class PlayerDemoPage : Page
    {
        MediaSource mediaSource;

        public PlayerDemoPage()
        {
            this.InitializeComponent();
        }
        
        private async Task<string> ReadAllText(string url, Windows.Storage.Streams.UnicodeEncoding encoding = Windows.Storage.Streams.UnicodeEncoding.Utf8)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(url));
            var text = await FileIO.ReadTextAsync(file, encoding);
            return text;
        }

        private async void execJSBtn_Click(object sender, RoutedEventArgs e)
        {
            //_ = webView1.InvokeScriptAsync("eval", new string[]
            //{
            //    "var testdata = new new Uint8Array(256); window.JSPlayerBridge.nativeDataMethod(testdata);"
            //});

            var nativeMseProxy = await ReadAllText("ms-appx:///PlayerAssets/iLoliPlayerCore/NativeMseProxy.js");
            var msePlugin = await ReadAllText("ms-appx:///PlayerAssets/iLoliPlayerCore/MsePlugin.js");
            Debug.WriteLine(nativeMseProxy);
            Debug.WriteLine(msePlugin);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Debug.WriteLine("Exec");
                _ = webView1.InvokeScriptAsync("eval", new string[]
                {
                    "console.log(\"hello\");"
                });

                //Debug.WriteLine("Exec NativeMseProxy");
                //_ = webView1.InvokeScriptAsync("eval", new string[]
                //{
                //    nativeMseProxy
                //});

                //Debug.WriteLine("Exec MsePlugin");
                //_ = webView1.InvokeScriptAsync("eval", new string[]
                //{
                //    msePlugin
                //});
            });
        }

        private void testBtn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(MseStreamSource.IsContentTypeSupported("video/mp4; codecs=\"avc1.42E01E\""));
            Debug.WriteLine(MseStreamSource.IsContentTypeSupported("video/mp4; codecs=\"hvc1.1.6.L93\""));
            Debug.WriteLine(MseStreamSource.IsContentTypeSupported("video/mp4; codecs=\"hev1.1.6.L93\""));
            Debug.WriteLine(MseStreamSource.IsContentTypeSupported("video/mp4; codecs=\"av01.0.05M.08\""));
            Debug.WriteLine(MseStreamSource.IsContentTypeSupported("video/mp4; codecs=\"ec-3\""));
            Debug.WriteLine(MseStreamSource.IsContentTypeSupported("video/mp4; codecs=\"vp9\""));
            var mse = new MseStreamSource();
            mse.Opened += (MseStreamSource s, object args) =>
            {
                Debug.WriteLine("Opened");
            };
            mse.Closed += (MseStreamSource s, object args) =>
            {
                Debug.WriteLine("Closed");
            };
            mse.Ended += (MseStreamSource s, object args) =>
            {
                Debug.WriteLine("Ended");
            };
            var mediaSource = MediaSource.CreateFromMseStreamSource(mse);
            mediaPlayerElement1.Source = mediaSource;
        }
    }



}
