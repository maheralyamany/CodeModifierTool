using System.Runtime.CompilerServices;

using System;
using System.ComponentModel;
using System.Drawing;

namespace OpetraViews.Views.MyContrloes
{
    /// <summary>Represents: MDevCoSerialize</summary>

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
    public class MDevCoSerialize : Attribute
    {
    }
    /// <summary>Represents: MElementStyle</summary>

    public class MElementStyle
    {


        private Size m_Size = Size.Empty;
        private int m_MarginLeft = 2;
        private int m_MarginRight = 2;
        private int m_MarginTop = 2;
        private int m_MarginBottom = 2;
        private int m_PaddingLeft = 2;
        private int m_PaddingRight = 2;
        private int m_PaddingTop = 2;
        private int m_PaddingBottom = 2;


        /// <summary>Initializes a new instance of the MElementStyle class</summary>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public MElementStyle()
        {
            m_MarginLeft = 0;
            m_MarginRight = m_MarginLeft;
            m_MarginTop = m_MarginLeft;
            m_MarginBottom = m_MarginLeft;
            m_PaddingLeft = m_MarginLeft;
            m_PaddingRight = m_MarginLeft;
            m_PaddingTop = m_MarginLeft;
            m_PaddingBottom = m_MarginLeft;
        }



        /// <summary>Gets: MarginHorizontal</summary>

        [Browsable(false)]
        public int MarginHorizontal => m_MarginLeft + m_MarginRight;
        /// <summary>Gets: MarginVertical</summary>

        [Browsable(false)]
        public int MarginVertical => m_MarginTop + m_MarginBottom;
        /// <summary>Gets or sets: MarginLeft</summary>

        [MDevCoSerialize]
        [Browsable(true)]
        [Category("Margins")]
        [DefaultValue(0)]
        [Description("Specifies left margin.")]
        public int MarginLeft
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return m_MarginLeft;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                m_MarginLeft = value;

            }
        }
        /// <summary>Gets or sets: MarginRight</summary>

        [MDevCoSerialize]
        [Browsable(true)]
        [Category("Margins")]
        [DefaultValue(0)]
        [Description("Specifies right margin.")]
        public int MarginRight
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return m_MarginRight;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                m_MarginRight = value;

            }
        }
        /// <summary>Gets or sets: MarginTop</summary>

        [MDevCoSerialize]
        [Browsable(true)]
        [Category("Margins")]
        [DefaultValue(0)]
        [Description("Specifies top margin.")]
        public int MarginTop
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return m_MarginTop;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                m_MarginTop = value;

            }
        }
        /// <summary>Gets or sets: MarginBottom</summary>

        [MDevCoSerialize]
        [Browsable(true)]
        [Category("Margins")]
        [DefaultValue(0)]
        [Description("Specifies bottom margin.")]
        public int MarginBottom
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return m_MarginBottom;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                m_MarginBottom = value;

            }
        }
        /// <summary>Gets: PaddingHorizontal</summary>

        [Browsable(false)]
        public int PaddingHorizontal => m_PaddingLeft + m_PaddingRight;
        /// <summary>Gets: PaddingVertical</summary>

        [Browsable(false)]
        public int PaddingVertical => m_PaddingTop + m_PaddingBottom;

        /// <summary>Gets or sets: PaddingTop</summary>

        [MDevCoSerialize]
        [Browsable(true)]
        [Category("Padding")]
        [DefaultValue(0)]
        [Description("Indicates the amount of space to insert between the top border of the element and the content.")]
        public int PaddingTop
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return m_PaddingTop;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                m_PaddingTop = value;

            }
        }
        /// <summary>Gets or sets: PaddingBottom</summary>

        [MDevCoSerialize]
        [Browsable(true)]
        [Category("Padding")]
        [DefaultValue(0)]
        [Description("Indicates the amount of space to insert between the bottom border of the element and the content.")]
        public int PaddingBottom
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return m_PaddingBottom;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                m_PaddingBottom = value;

            }
        }
        /// <summary>Gets or sets: PaddingLeft</summary>

        [MDevCoSerialize]
        [Browsable(true)]
        [Category("Padding")]
        [DefaultValue(0)]
        [Description("Indicates the amount of space to insert between the left border of the element and the content.")]
        public int PaddingLeft
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return m_PaddingLeft;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                m_PaddingLeft = value;

            }
        }
        /// <summary>Gets or sets: PaddingRight</summary>

        [MDevCoSerialize]
        [Browsable(true)]
        [Category("Padding")]
        [DefaultValue(0)]
        [Description("Indicates the amount of space to insert between the right border of the element and the content.")]
        public int PaddingRight
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return m_PaddingRight;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                m_PaddingRight = value;

            }
        }

        /// <summary>Gets: Size</summary>

        [Browsable(false)]
        public Size Size => m_Size;


        /// <summary>Performs copy</summary>
        /// <returns>The result of the copy</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public MElementStyle Copy()
        {
            /*	MElementStyle elementStyle = new MElementStyle();
				elementStyle.MarginBottom = MarginBottom;
				elementStyle.MarginLeft = MarginLeft;
				elementStyle.MarginRight = MarginRight;
				elementStyle.MarginTop = MarginTop;
				elementStyle.PaddingBottom = PaddingBottom;
				elementStyle.PaddingLeft = PaddingLeft;
				elementStyle.PaddingRight = PaddingRight;
				elementStyle.PaddingTop = PaddingTop;*/

            return this;
        }
    }
}
