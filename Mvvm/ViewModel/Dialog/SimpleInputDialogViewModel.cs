using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfUtilV1.Mvvm;
using WpfUtilV1.Mvvm.ViewModel;

namespace NicoV3.Mvvm.ViewModel.Dialog
{
    public class SimpleInputDialogViewModel : ViewModelBase
    {
        public SimpleInputDialogViewModel(ICommand _OnAccept)
            : this(_OnAccept, new RelayCommand(_ => { }))
        {
            // ｷｬﾝｾﾙ時は何もしない
        }

        public SimpleInputDialogViewModel(ICommand _OnAccept, ICommand _OnCancel)
        {
            if (_OnAccept == null)
            {
                throw new ArgumentNullException("_OnAccept");
            }

            OnAccept = _OnAccept;
            OnCancel = _OnCancel;
        }

        /// <summary>
        /// ﾀｲﾄﾙ
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }
        private string _Title;

        /// <summary>
        /// 説明
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { SetProperty(ref _Description, value); }
        }
        private string _Description;

        /// <summary>
        /// 項目名
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        private string _Name;

        /// <summary>
        /// 項目入力値
        /// </summary>
        public string Input
        {
            get { return _Input; }
            set { SetProperty(ref _Input, value); }
        }
        private string _Input;

        /// <summary>
        /// OKﾎﾞﾀﾝ押下時処理
        /// </summary>
        public ICommand OnAccept
        {
            get { return _OnAccept; }
            set { SetProperty(ref _OnAccept, value); }
        }
        private ICommand _OnAccept;

        /// <summary>
        /// ｷｬﾝｾﾙﾎﾞﾀﾝ押下時処理
        /// </summary>
        public ICommand OnCancel
        {
            get { return _OnCancel; }
            set { SetProperty(ref _OnCancel, value); }
        }
        private ICommand _OnCancel;

    }
}
