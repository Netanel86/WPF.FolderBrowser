using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPF.Common.UI.Infrastracture;

namespace WPF.Common.UI.Behaviors
{
    /// <summary>
    /// Defines methods for navigating threw views
    /// </summary>
    /// <remarks>
    /// Interface should be implemented by a behavior that handles the presentation of new view or windows.
    /// </remarks>
    public interface IDialogService
    {
        void NavigateTo(DialogUserControl i_Control, Action<object> i_Completed);

        void NavigateBackwards(object i_Result);
    }
}
