using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPF.Common.ViewModel;

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

    public interface IMessageModel
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
    }

    public class ErrorMessage : MessageModel
    {

    }
}
