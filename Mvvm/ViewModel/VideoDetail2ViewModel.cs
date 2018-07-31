using NicoV3.Common;
using NicoV3.Mvvm.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfUtilV1.Mvvm;
using WpfUtilV1.Mvvm.Service;

namespace NicoV3.Mvvm.ViewModel
{
    public class VideoDetail2ViewModel : WorkSpaceViewModel
    {
        public VideoModel Source { get; set; }

        public VideoDetail2ViewModel(VideoModel source)
        {
            Source = source;

            Tags = Source.Tags;
            JumpUrl = Source.VideoUrl;
        }

        /// <summary>
        /// Flvﾌｧｲﾙ
        /// </summary>
        public Uri FlvFile
        {
            get { return _FlvFile; }
            set { SetProperty(ref _FlvFile, value); }
        }
        private Uri _FlvFile = null;

        /// <summary>
        /// ﾀｸﾞ (空白区切り)
        /// </summary>
        public string Tags
        {
            get { return _Tags; }
            set { SetProperty(ref _Tags, value); }
        }
        private string _Tags = null;

        /// <summary>
        /// ﾀｸﾞ (空白区切り)
        /// </summary>
        public string JumpUrl
        {
            get { return _JumpUrl; }
            set { SetProperty(ref _JumpUrl, value); }
        }
        private string _JumpUrl = null;

        /// <summary>
        /// 画面初期化時ｲﾍﾞﾝﾄ
        /// </summary>
        public ICommand OnLoaded
        {
            get
            {
                return _OnLoaded = _OnLoaded 
                    ?? new RelayCommand<string>(StartListening);
            }
        }
        public ICommand _OnLoaded;

        /// <summary>
        /// ｼﾞｬﾝﾌﾟ処理
        /// </summary>
        public ICommand OnJump
        {
            get
            {
                return _OnJump = _OnJump ?? new RelayCommand(_ =>
                {
                    OnLoaded.Execute(JumpUrl);
                });
            }
        }
        public ICommand _OnJump;

        /// <summary>
        /// Urlｺﾋﾟｰ処理
        /// </summary>
        public ICommand OnCopyUrl
        {
            get
            {
                return _OnCopyUrl = _OnCopyUrl ?? new RelayCommand(_ =>
                {
                    Clipboard.SetText(Source.VideoUrl);
                });
            }
        }
        public ICommand _OnCopyUrl;

        /// <summary>
        /// ﾀﾞｳﾝﾛｰﾄﾞ処理
        /// </summary>
        public ICommand OnDownload
        {
            get
            {
                return _OnDownload = _OnDownload ?? new RelayCommand(_ =>
                {
                    ServiceFactory.MessageService.Debug(FlvFile.AbsolutePath);
                });
            }
        }
        public ICommand _OnDownload;

        /// <summary>
        /// MP3変換処理
        /// </summary>
        public ICommand OnMp3Convert
        {
            get
            {
                return _OnMp3Convert = _OnMp3Convert ?? new RelayCommand(_ =>
                {
                    ServiceFactory.MessageService.Debug("TODO OnMp3Convert");
                });
            }
        }
        public ICommand _OnMp3Convert;

        #region Socket

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        private async void StartListening(string url)
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 81);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(100);

            StateObject state = new StateObject();
            state.WorkSocket = listener;
            state.ConnectUri = await VideoStatusModel.Instance
                .GetVideo(NicoDataConverter.ToId(url))
                .GetMovieUriAsync();

            allDone.Reset();

            // Start an asynchronous socket to listen for connections.  
            Console.WriteLine("Waiting for a connection...");
            listener.BeginAccept(new AsyncCallback(AcceptCallback), state);

            FlvFile = new Uri("127.0.0.1:81", UriKind.Absolute);
            
            // Wait until a connection is made before continuing.  
            allDone.WaitOne();
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            StateObject state = ar.AsyncState as StateObject;

            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = state.WorkSocket;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            state.WorkSocket = handler;
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }

        public static void ReceiveCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = ar.AsyncState as StateObject;
            Socket handler = state.WorkSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            state.Request = WebRequest.CreateHttp(state.ConnectUri);
            state.Request.CookieContainer = LoginModel.Instance.Cookie;
            state.Request.AllowReadStreamBuffering = false;
            state.Request.BeginGetResponse(new AsyncCallback(GetResponseCallback), state);
        }

        private static async void GetResponseCallback(IAsyncResult ar)
        {
            StateObject state = ar.AsyncState as StateObject;
            Socket handler = state.WorkSocket;
            WebRequest req = state.Request as WebRequest;

            using (var res = req.EndGetResponse(ar) as HttpWebResponse)
            {
                if (res == null || res.StatusCode != HttpStatusCode.OK) return;

                long read = 0, tick = res.ContentLength / 200;

                using (var stz = res.GetResponseStream())
                {
                    int c;
                    byte[] buffer = new byte[StateObject.BufferSize];
                    while (0 < (c = await stz.ReadAsync(buffer, 0, buffer.Length)))
                    {
                        handler.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(SendCallback), handler);

                        read += c;
                        await Dispatcher.CurrentDispatcher.InvokeAsync(() =>//UIのスレッドを待たない
                        {
                            //slider.ProgressValue = read / tick;
                        });
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    handler.Dispose();
                }
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = ar.AsyncState as Socket;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion

    }

    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Client  socket.  
        public Socket WorkSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] Buffer = new byte[BufferSize];
        // Uri
        public Uri ConnectUri = null;
        // Request
        public HttpWebRequest Request = null;
    }
}
