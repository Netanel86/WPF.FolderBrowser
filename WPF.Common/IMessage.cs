using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPF.Common.ViewModel;
using System.Windows.Input;

namespace WPF.Common
{
    public enum eMessageIcon
    {
        Exclamation,
        Warning,
        Close,
        Info,
        OK
    }

    public interface IMessageModel : IClosableElement
    {
        string Title { get; }
        string Text { get; }
        string Content { get; }
        eMessageIcon Icon { get; }
    }

    public class MessageModel : ViewModelBase, IMessageModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Content { get; set; }
        public eMessageIcon Icon { get; set; }

        public MessageModel()
        {
            DefaultButtonCommand = new RelayCommand(x => OnCloseRequest(true));
        }
        public event EventHandler CloseRequest;

        protected void OnCloseRequest(bool i_DialogCanceled)
        {
            if (this.CloseRequest != null)
            {
                this.CloseRequest(this, new NotificationEventArgs<bool>(i_DialogCanceled));
            }
        }

        public ICommand DefaultButtonCommand { set; get; }
    }

    public class ErrorMessage : MessageModel
    {

    }
}
