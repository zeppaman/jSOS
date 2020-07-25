using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using JsOS.APP.Model;

namespace JsOS.APP.Core
{
    public class AppPermissionToEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as BindingList<Need>;


            var allOn = item.Count(x => x.Enabled == true);
            var allOff = item.Count(x => x.Enabled == false);
            var all = item.Count;
            if (all == allOff) return false;
            else if (all == allOn) return true;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
