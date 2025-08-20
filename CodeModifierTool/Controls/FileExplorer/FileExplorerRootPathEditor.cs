using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace OpetraViews.Controls
{
    /// <summary>Represents: file explorer root path editor</summary>
    public class FileExplorerRootPathEditor : UITypeEditor
    {
        #region Constructor
        private System.Windows.Forms.TextBox _TextBox = null;
        /// <summary>Initializes a new instance of the <see cref = "FileExplorerRootPathEditor"/>  class</summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public FileExplorerRootPathEditor()
        {
        }

        #endregion
        /// <summary>Gets valid path</summary>
        /// <param name = "value">The value</param>
        /// <returns>The retrieved valid path</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static string GetValidPath(string value)
        {
            if (value != null && value != "")
            {
                var list = value.Split("\\", StringSplitOptions.RemoveEmptyEntries);
                return string.Join("\\\\", list);
            }

            return value;
        }

        /// <summary>Gets: is drop down resizable</summary>
        public override bool IsDropDownResizable => true;

        /// <summary>Performs edit value</summary>
        /// <param name = "context">The context</param>
        /// <param name = "provider">The provider</param>
        /// <param name = "value">The value</param>
        /// <returns>The result of the edit value</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value is string path)
            {
                value = GetValidPath(path);
            }

            if (provider != null)
            {
                System.Windows.Forms.Design.IWindowsFormsEditorService service = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
                if (service != null)
                {
                    if (_TextBox == null)
                        _TextBox = new System.Windows.Forms.TextBox
                        {
                            Multiline = true,
                            MinimumSize = new System.Drawing.Size(250, 100)
                        };
                    _TextBox.Text = (string)value;
                    service.DropDownControl(_TextBox);
                    value = _TextBox.Text;
                }
            }

            return value;
        }

        /// <summary>Gets edit style</summary>
        /// <param name = "context">The context</param>
        /// <returns>The retrieved edit style</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }

    /// <summary>Represents: root path converter</summary>
    public class RootPathConverter : TypeConverter
    {
        /// <summary>Performs can convert from</summary>
        /// <param name = "context">The context</param>
        /// <param name = "sourceType">The sourceType</param>
        /// <returns>The result of the can convert from</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>Performs can convert to</summary>
        /// <param name = "context">The context</param>
        /// <param name = "destinationType">The destinationType</param>
        /// <returns>The result of the can convert to</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>Performs convert from</summary>
        /// <param name = "context">The context</param>
        /// <param name = "culture">The culture</param>
        /// <param name = "value">The value</param>
        /// <returns>The result of the convert from</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string text = value as string;
            if (text != null)
            {
                return GetValidPath(text);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>Gets valid path</summary>
        /// <param name = "value">The value</param>
        /// <returns>The retrieved valid path</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static string GetValidPath(string value)
        {
            if (value.IsNotEmpty())
            {
                var list = value.SplitList("\\", StringSplitOptions.RemoveEmptyEntries);
                return string.Join("\\\\", list);
            }

            return value;
        }

        /// <summary>Performs convert to</summary>
        /// <param name = "context">The context</param>
        /// <param name = "culture">The culture</param>
        /// <param name = "value">The value</param>
        /// <param name = "destinationType">The destinationType</param>
        /// <returns>The result of the convert to</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            string text = value as string;
            if (text != null)
            {
                return GetValidPath(text);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>Gets standard values supported</summary>
        /// <param name = "context">The context</param>
        /// <returns>The retrieved standard values supported</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}