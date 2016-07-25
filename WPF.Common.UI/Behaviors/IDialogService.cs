using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPF.Common.UI.Infrastracture;

namespace WPF.Common.UI.Behaviors
{
    public interface IDialogService
    {
        void NavigateTo(DialogUserControl i_Control, Action<object> i_Completed);

        void NavigateBackwards(object i_Result);
    }
}
