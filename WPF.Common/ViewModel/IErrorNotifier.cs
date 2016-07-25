using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF.Common.ViewModel
{
    public interface IErrorNotifier
    {
        event EventHandler ErrorNotice;
    }
}
