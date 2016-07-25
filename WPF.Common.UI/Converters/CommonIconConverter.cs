using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using WPF.Common.UI.Resources;
using System.Windows.Media.Imaging;

namespace WPF.Common.UI.Converters
{
    public class CommonIconConverter : IValueConverter
    {
        private const string m_ApplicationPath = @"pack://application:,,,/WPF.Common.UI;component/Icons/";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string converted = null;

            if (value is eMessageIcon)
            {
                switch ((eMessageIcon)value)
                {
                    case eMessageIcon.Close:
                        converted = CommonIcons.Close;
                        break;
                    case eMessageIcon.Exclamation:
                        converted = CommonIcons.Exclamation;
                        break;
                    case eMessageIcon.Info:
                        converted = CommonIcons.Info;
                        break;
                    case eMessageIcon.Warning:
                        converted = CommonIcons.Warning;
                        break;

                    case eMessageIcon.OK:
                        converted = CommonIcons.OK;
                        break;
                }
            }
            else if (value is string)
            {
                converted = value as string;
            }

            return new BitmapImage(new Uri(m_ApplicationPath + converted));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
