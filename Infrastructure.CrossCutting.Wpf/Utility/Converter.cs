using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using Infrastructure.Crosscutting.Utility.CustomAttribute;
using Infrastructure.CrossCutting.Wpf.Resources;

namespace Infrastructure.CrossCutting.Wpf.Utility
{
    [ValueConversion(typeof(byte), typeof(string))]
    public class Enum2StringConverter : IValueConverter
    {
        public object Convert(object value, Type notused1, object notused2, CultureInfo notused3)
        {
            string result = null;
            if (value != null)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                // To get around the stupid WPF designer bug
                if (fi != null)
                {
                    var attributes = (LocalizableDescriptionAttribute[])fi.GetCustomAttributes(typeof(LocalizableDescriptionAttribute), false);
                    result = ((attributes.Length > 0) && (!String.IsNullOrEmpty(attributes[0].Description))) ? attributes[0].Description : value.ToString();
                    return result;
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type notused1, object notused2, CultureInfo notused3)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(object), typeof(bool))]
    public class Null2EnableConverter : IValueConverter
    {
        public object Convert(object value, Type notused1, object invert, CultureInfo notused3)
        {
            bool invertLocal = invert != null ? bool.Parse(invert.ToString()) : false;
            if (!invertLocal)
                return value != null;
            else
                return value == null;
        }

        public object ConvertBack(object value, Type notused1, object notused2, CultureInfo notused3)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(double), typeof(string))]
    public class Double2PercentConverter : IValueConverter
    {
        public object Convert(object dblValue, Type notused1, object notused, CultureInfo notused3)
        {
            return string.Format(Messages.Format_Percent, ((double)dblValue) * 100);
        }

        public object ConvertBack(object strValue, Type notused1, object notused2, CultureInfo notused3)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(object[]), typeof(bool))]
    public class Equal2EnableConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type notused1, object invert, CultureInfo notused3)
        {
            bool invertLocal = invert != null ? bool.Parse(invert.ToString()) : false;

            if (value != null && value.Length == 2 && value[0] != null && value[1] != null)
            {
                if (!invertLocal)
                    return value[0].ToString() == value[1].ToString();
                else
                    return value[0].ToString() != value[1].ToString();
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(bool), typeof(bool))]
    public class ReverseBooleanConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.GetType() != typeof(bool)) return null;
            return !System.Convert.ToBoolean(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
