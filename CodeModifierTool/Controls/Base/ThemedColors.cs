using System.Runtime.CompilerServices;

/*
 * This code is provided under the Code Project Open Licence (CPOL)
 * See http://www.codeproject.com/info/cpol10.aspx for details
*/

using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace System.Drawing
{

    /// <summary>Represents: ThemedColors</summary>

    internal sealed class ThemedColors
    {

        #region "    Variables and Constants "

        private const string NormalColor = "NormalColor";
        private const string HomeStead = "HomeStead";
        private const string Metallic = "Metallic";
        private const string NoTheme = "NoTheme";

        private static Color[] _toolBorder;
        #endregion

        #region "    Properties "

        /// <summary>Gets: CurrentThemeIndex</summary>

        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static ColorScheme CurrentThemeIndex
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get { return ThemedColors.GetCurrentThemeIndex(); }
        }

        /// <summary>Gets: ToolBorder</summary>

        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public static Color ToolBorder
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get { return ThemedColors._toolBorder[(int)ThemedColors.CurrentThemeIndex]; }
        }

        #endregion

        #region "    Constructors "

        /// <summary>Initializes a new instance of the ThemedColors class</summary>

        [MethodImpl(MethodImplOptions.NoInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]

        static ThemedColors()
        {
            ThemedColors._toolBorder = new Color[] { Color.FromArgb(127, 157, 185), Color.FromArgb(164, 185, 127), Color.FromArgb(165, 172, 178), Color.FromArgb(132, 130, 132) };
        }


        /// <summary>Initializes a new instance of the ThemedColors class</summary>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ThemedColors() { }

        #endregion


        /// <summary>Gets current theme index</summary>
        /// <returns>The retrieved current theme index</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ColorScheme GetCurrentThemeIndex()
        {
            ColorScheme theme = ColorScheme.NoTheme;

            if (VisualStyleInformation.IsSupportedByOS && VisualStyleInformation.IsEnabledByUser && Application.RenderWithVisualStyles)
            {


                switch (VisualStyleInformation.ColorScheme)
                {
                    case NormalColor:
                        theme = ColorScheme.NormalColor;
                        break;
                    case HomeStead:
                        theme = ColorScheme.HomeStead;
                        break;
                    case Metallic:
                        theme = ColorScheme.Metallic;
                        break;
                    default:
                        theme = ColorScheme.NoTheme;
                        break;
                }
            }

            return theme;
        }

        /// <summary>Defines enumeration: ColorScheme</summary>

        public enum ColorScheme
        {
            NormalColor = 0,
            HomeStead = 1,
            Metallic = 2,
            NoTheme = 3
        }

    }

}
