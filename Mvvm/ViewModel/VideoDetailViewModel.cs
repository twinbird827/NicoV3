using NicoV3.Mvvm.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NicoV3.Mvvm.ViewModel
{
    public class VideoDetailViewModel : WorkSpaceViewModel
    {
        public VideoDetailModel Source { get; set; }

        public VideoDetailViewModel(string id)
            : this(new VideoDetailModel(id))
        {

        }

        public VideoDetailViewModel(VideoDetailModel source)
        {

        }

        /// <summary>
        /// Flvﾌｧｲﾙ
        /// </summary>
        public MemoryStream FlvFile
        {
            get { return _FlvFile; }
            set { SetProperty(ref _FlvFile, value); }
        }
        private MemoryStream _FlvFile = null;

        /// <summary>
        /// ﾀｸﾞ (空白区切り)
        /// </summary>
        public string Tags
        {
            get { return _Tags; }
            set { SetProperty(ref _Tags, value); }
        }
        private string _Tags = null;

    }
}
