﻿using NicoV3.Common;
using NicoV3.Mvvm.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfUtilV1.Mvvm;
using WpfUtilV1.Mvvm.Service;

namespace NicoV3.Mvvm.ViewModel
{
    public class VideoDetailViewModel : WorkSpaceViewModel
    {
        public VideoModel Source { get; set; }

        public VideoDetailViewModel(VideoModel source)
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
                return _OnLoaded = _OnLoaded ?? new RelayCommand<string>(async url =>
                {
                    var path = Path.Combine(Variables.MovieTemporaryPath, NicoDataConverter.ToId(url));

                    if (!File.Exists(path))
                    {
                        // TODO ある条件でﾃﾝﾎﾟﾗﾘをｸﾘｰﾝｱｯﾌﾟする。

                        int read;
                        byte[] buffer = new byte[1024];
                        using (var ms = await Source.GetMovieStreamAsync())
                        using (var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
                        {
                            while ((read = ms.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fs.Write(buffer, 0, read);
                            }
                        }
                    }

                    // Uri変更
                    FlvFile = new Uri(path);

                    // ｼﾞｬﾝﾌﾟ先初期化
                    JumpUrl = string.Empty;
                });
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

    }
}
