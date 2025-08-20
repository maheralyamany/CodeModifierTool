using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace OpetraViews.Views.MyContrloes
{

    /// <summary>Represents: CorPaintHelper</summary>

    public class CorPaintHelper
    {

        /// <summary>Gets text bounds</summary>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="text">The text</param>
        /// <param name="flags">The flags</param>
        /// <param name="font">The font</param>
        /// <param name="textAlign">The textAlign</param>
        /// <returns>The retrieved text bounds</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static Rectangle GetTextBounds(Rectangle rectangle, string text, TextFormatFlags flags, Font font, ContentAlignment textAlign)
        {
            if ((flags & TextFormatFlags.SingleLine) != 0)
            {
                Size sizeRequired = TextRenderer.MeasureText(text, font, new Size(int.MaxValue, int.MaxValue), flags);
                if (sizeRequired.Width > rectangle.Width)
                {
                    flags |= TextFormatFlags.EndEllipsis;
                }
            }

            Size sizeCell = new Size(rectangle.Width, rectangle.Height);
            Size sizeConstraint = TextRenderer.MeasureText(text, font, sizeCell, flags);
            if (sizeConstraint.Width > sizeCell.Width)
            {
                sizeConstraint.Width = sizeCell.Width;
            }
            if (sizeConstraint.Height > sizeCell.Height)
            {
                sizeConstraint.Height = sizeCell.Height;
            }
            if (sizeConstraint == sizeCell)
            {
                return rectangle;
            }
            return new Rectangle(GetTextLocation(rectangle, sizeConstraint, flags, textAlign), sizeConstraint);
        }

        /// <summary>Gets text location</summary>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="sizeText">The sizeText</param>
        /// <param name="flags">The flags</param>
        /// <param name="textAlign">The textAlign</param>
        /// <returns>The retrieved text location</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static Point GetTextLocation(Rectangle rectangle, Size sizeText, TextFormatFlags flags, ContentAlignment textAlign)
        {
            Point ptTextLocation = new Point(0, 0);

            // now use the alignment on the cellStyle to determine the final text location
            ContentAlignment alignment = textAlign;
            if ((flags & TextFormatFlags.RightToLeft) != 0)
            {
                switch (alignment)
                {
                    case ContentAlignment.TopLeft:
                        alignment = ContentAlignment.TopRight;
                        break;

                    case ContentAlignment.TopRight:
                        alignment = ContentAlignment.TopLeft;
                        break;

                    case ContentAlignment.MiddleLeft:
                        alignment = ContentAlignment.MiddleRight;
                        break;

                    case ContentAlignment.MiddleRight:
                        alignment = ContentAlignment.MiddleLeft;
                        break;

                    case ContentAlignment.BottomLeft:
                        alignment = ContentAlignment.BottomRight;
                        break;

                    case ContentAlignment.BottomRight:
                        alignment = ContentAlignment.BottomLeft;
                        break;
                }
            }

            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                    ptTextLocation.X = rectangle.X;
                    ptTextLocation.Y = rectangle.Y;
                    break;

                case ContentAlignment.TopCenter:
                    ptTextLocation.X = rectangle.X + (rectangle.Width - sizeText.Width) / 2;
                    ptTextLocation.Y = rectangle.Y;
                    break;

                case ContentAlignment.TopRight:
                    ptTextLocation.X = rectangle.Right - sizeText.Width;
                    ptTextLocation.Y = rectangle.Y;
                    break;

                case ContentAlignment.MiddleLeft:
                    ptTextLocation.X = rectangle.X;
                    ptTextLocation.Y = rectangle.Y + (rectangle.Height - sizeText.Height) / 2;
                    break;

                case ContentAlignment.MiddleCenter:
                    ptTextLocation.X = rectangle.X + (rectangle.Width - sizeText.Width) / 2;
                    ptTextLocation.Y = rectangle.Y + (rectangle.Height - sizeText.Height) / 2;
                    break;

                case ContentAlignment.MiddleRight:
                    ptTextLocation.X = rectangle.Right - sizeText.Width;
                    ptTextLocation.Y = rectangle.Y + (rectangle.Height - sizeText.Height) / 2;
                    break;

                case ContentAlignment.BottomLeft:
                    ptTextLocation.X = rectangle.X;
                    ptTextLocation.Y = rectangle.Bottom - sizeText.Height;
                    break;

                case ContentAlignment.BottomCenter:
                    ptTextLocation.X = rectangle.X + (rectangle.Width - sizeText.Width) / 2;
                    ptTextLocation.Y = rectangle.Bottom - sizeText.Height;
                    break;

                case ContentAlignment.BottomRight:
                    ptTextLocation.X = rectangle.Right - sizeText.Width;
                    ptTextLocation.Y = rectangle.Bottom - sizeText.Height;
                    break;

                default:

                    break;
            }
            return ptTextLocation;
        }

        /// <summary>Performs compute text format flags for content alignment</summary>
        /// <param name="rightToLeft">The rightToLeft</param>
        /// <param name="alignment">The alignment</param>
        /// <param name="wordBreak">The wordBreak</param>
        /// <returns>The result of the compute text format flags for content alignment</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when wordBreak is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static TextFormatFlags ComputeTextFormatFlagsForContentAlignment(RightToLeft rightToLeft, ContentAlignment alignment, bool wordBreak)
        {
            bool isRightToLeft = rightToLeft == RightToLeft.Yes;
            TextFormatFlags tff;
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                    tff = TextFormatFlags.Top;
                    if (isRightToLeft)
                    {
                        tff |= TextFormatFlags.Right;
                    }
                    else
                    {
                        tff |= TextFormatFlags.Left;
                    }
                    break;
                case ContentAlignment.TopCenter:
                    tff = TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopRight:
                    tff = TextFormatFlags.Top;
                    if (isRightToLeft)
                    {
                        tff |= TextFormatFlags.Left;
                    }
                    else
                    {
                        tff |= TextFormatFlags.Right;
                    }
                    break;
                case ContentAlignment.MiddleLeft:
                    tff = TextFormatFlags.VerticalCenter;
                    if (isRightToLeft)
                    {
                        tff |= TextFormatFlags.Right;
                    }
                    else
                    {
                        tff |= TextFormatFlags.Left;
                    }
                    break;
                case ContentAlignment.MiddleCenter:
                    tff = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.MiddleRight:
                    tff = TextFormatFlags.VerticalCenter;
                    if (isRightToLeft)
                    {
                        tff |= TextFormatFlags.Left;
                    }
                    else
                    {
                        tff |= TextFormatFlags.Right;
                    }
                    break;
                case ContentAlignment.BottomLeft:
                    tff = TextFormatFlags.Bottom;
                    if (isRightToLeft)
                    {
                        tff |= TextFormatFlags.Right;
                    }
                    else
                    {
                        tff |= TextFormatFlags.Left;
                    }
                    break;
                case ContentAlignment.BottomCenter:
                    tff = TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomRight:
                    tff = TextFormatFlags.Bottom;
                    if (isRightToLeft)
                    {
                        tff |= TextFormatFlags.Left;
                    }
                    else
                    {
                        tff |= TextFormatFlags.Right;
                    }
                    break;
                default:
                    tff = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                    break;
            }
            if (!wordBreak)
            {
                tff |= TextFormatFlags.SingleLine;
            }
            else
            {
                //tff |= TextFormatFlags.NoFullWidthCharacterBreak;  VSWhidbey 518422
                tff |= TextFormatFlags.WordBreak;
            }
            tff |= TextFormatFlags.NoPrefix;
            tff |= TextFormatFlags.PreserveGraphicsClipping;
            if (isRightToLeft)
            {
                tff |= TextFormatFlags.RightToLeft;
            }
            return tff;
        }

        /// <summary>Performs apply region</summary>
        /// <param name="ctrl">The ctrl</param>
        /// <param name="radius">The radius</param>
        /// <returns>The result of the apply region</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath ApplyRegion(Control ctrl, int radius)
        {
            GraphicsPath graphicsPath = CreateRoundRectangle(ctrl.Width - 1, ctrl.Height - 1, radius);
            ctrl.Region = new Region(graphicsPath);
            return graphicsPath;
        }

        /// <summary>Creates a new create round rectangle</summary>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="radius">The radius</param>
        /// <returns>The newly created create round rectangle</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when width is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when height is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath CreateRoundRectangle(int width, int height, int radius)
        {
            int d = radius << 1;
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, d, d), 180, 90);
            path.AddLine(radius, 0, width - radius, 0);
            path.AddArc(new Rectangle(width - d, 0, d, d), 270, 90);
            path.AddLine(width + 1, radius, width + 1, height - radius);
            path.AddArc(new Rectangle(width - d, height - d, d, d), 0, 90);
            path.AddLine(width - radius, height + 1, radius, height + 1);
            path.AddArc(new Rectangle(0, height - d, d, d), 90, 90);
            path.AddLine(0, height - radius, 0, radius);
            path.CloseFigure();
            return path;
        }

        /// <summary>Gets figure path</summary>
        /// <param name="rect">The rect</param>
        /// <param name="radius">The radius</param>
        /// <returns>The retrieved figure path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        /// <summary>Performs draw round rect</summary>
        /// <param name="g">The g</param>
        /// <param name="p">The p</param>
        /// <param name="X">The x</param>
        /// <param name="Y">The y</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="radius">The radius</param>
        /// <exception cref="System.ArgumentNullException">Thrown when X is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when Y is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when width is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when height is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DrawRoundRect(Graphics g, Pen p, float X, float Y, float width, float height, float radius)
        {
            GraphicsPath gp = new GraphicsPath();
            //Upper-right arc:
            gp.AddArc(X + width - radius * 2, Y, radius * 2, radius * 2, 270, 90);
            //Lower-right arc:
            gp.AddArc(X + width - radius * 2, Y + height - radius * 2, radius * 2, radius * 2, 0, 90);
            //Lower-left arc:
            gp.AddArc(X, Y + height - radius * 2, radius * 2, radius * 2, 90, 90);
            //Upper-left arc:
            gp.AddArc(X, Y, radius * 2, radius * 2, 180, 90);
            gp.CloseFigure();
            g.DrawPath(p, gp);
            gp.Dispose();
        }

        /// <summary>Gets round path</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="radius">The radius</param>
        /// <returns>The retrieved round path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath GetRoundPath(RectangleF bounds, float radius)
        {
            GraphicsPath gp = new GraphicsPath();

            float X = bounds.X;
            float Y = bounds.Y;
            float width = bounds.Width;
            float height = bounds.Height;
            //Upper-right arc:
            gp.AddArc(X + width - radius * 2, Y, radius * 2, radius * 2, 270, 90);
            //Lower-right arc:
            gp.AddArc(X + width - radius * 2, Y + height - radius * 2, radius * 2, radius * 2, 0, 90);
            //Lower-left arc:
            gp.AddArc(X, Y + height - radius * 2, radius * 2, radius * 2, 90, 90);
            //Upper-left arc:
            gp.AddArc(X, Y, radius * 2, radius * 2, 180, 90);
            gp.CloseFigure();
            return gp;
        }

        /// <summary>Gets round path</summary>
        /// <param name="X">The x</param>
        /// <param name="Y">The y</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="radius">The radius</param>
        /// <returns>The retrieved round path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when X is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when Y is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when width is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when height is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath GetRoundPath(float X, float Y, float width, float height, float radius)
        {
            GraphicsPath gp = new GraphicsPath();
            //Upper-right arc:
            gp.AddArc(X + width - radius * 2, Y, radius * 2, radius * 2, 270, 90);
            //Lower-right arc:
            gp.AddArc(X + width - radius * 2, Y + height - radius * 2, radius * 2, radius * 2, 0, 90);
            //Lower-left arc:
            gp.AddArc(X, Y + height - radius * 2, radius * 2, radius * 2, 90, 90);
            //Upper-left arc:
            gp.AddArc(X, Y, radius * 2, radius * 2, 180, 90);
            gp.CloseFigure();
            return gp;
        }

        /// <summary>Gets top round path</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="radius">The radius</param>
        /// <returns>The retrieved top round path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath GetTopRoundPath(Rectangle bounds, float radius)
        {
            float X = bounds.X;
            float Y = bounds.Y;
            float width = bounds.Width;
            float height = bounds.Height;
            GraphicsPath gp = new GraphicsPath();
            //Upper-right arc:
            gp.AddArc(X + width - radius * 2, Y, radius * 2, radius * 2, 270, 90);
            //Lower-right arc:
            gp.AddArc(X + width - radius * 2, Y + height - radius * 2, radius * 2, radius * 2, 0, 90);
            //Lower-left arc:
            gp.AddArc(X, Y + height - radius * 2, radius * 2, radius * 2, 0, 90);
            //Upper-left arc:
            gp.AddArc(X, Y, radius * 2, radius * 2, 180, 90);
            gp.CloseFigure();
            return gp;
        }

        /// <summary>Gets top round region</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="radius">The radius</param>
        /// <returns>The retrieved top round region</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Region GetTopRoundRegion(Rectangle bounds, int radius)
        {
            bounds.X++;
            bounds.Y++;
            bounds.Width = bounds.Width - 2;
            bounds.Height = bounds.Height - 2;
            Region region = new Region();
            /*using (GraphicsPath graphicsPath = CreatePath(bounds, cornerDiameter, false)) {
                region.MakeEmpty();
                region.Union(graphicsPath);
                return region;
            }*/
            using (GraphicsPath graphicsPath = GetTopRoundPath(bounds, radius))
            {
                region.MakeEmpty();
                region.Union(graphicsPath);
                return region;
            }
        }

        /// <summary>Creates a new create path</summary>
        /// <param name="rect">The rect</param>
        /// <param name="nRadius">The nRadius</param>
        /// <returns>The newly created create path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when nRadius is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath CreatePath(Rectangle rect, int nRadius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, nRadius, nRadius, 180f, 90f);
            path.AddArc(rect.Right - nRadius, rect.Y, nRadius, nRadius, 270f, 90f);
            path.AddArc(rect.Right - nRadius, rect.Bottom - nRadius, nRadius, nRadius, 0f, 90f);
            path.AddArc(rect.X, rect.Bottom - nRadius, nRadius, nRadius, 90f, 90f);
            path.CloseFigure();
            return path;
        }

        /// <summary>Creates a new create path</summary>
        /// <param name="rect">The rect</param>
        /// <param name="nRadius">The nRadius</param>
        /// <param name="bOutline">The bOutline</param>
        /// <returns>The newly created create path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when nRadius is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when bOutline is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath CreatePath(Rectangle rect, int nRadius, bool bOutline)
        {
            int nShift = bOutline ? 1 : 0;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X + nShift, rect.Y, nRadius, nRadius, 180f, 90f);
            path.AddArc(rect.Right - nRadius - nShift, rect.Y, nRadius, nRadius, 270f, 90f);
            path.AddArc(rect.Right - nRadius - nShift, rect.Bottom - nRadius - nShift, nRadius, nRadius, 0f, 90f);
            path.AddArc(rect.X + nShift, rect.Bottom - nRadius - nShift, nRadius, nRadius, 90f, 90f);
            path.CloseFigure();
            return path;
        }

        /// <summary>Gets style region</summary>
        /// <param name="cornerDiameter">The cornerDiameter</param>
        /// <param name="bounds">The bounds</param>
        /// <returns>The retrieved style region</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when cornerDiameter is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Region GetStyleRegion(int cornerDiameter, Rectangle bounds)
        {
            bounds.X++;
            bounds.Y++;
            bounds.Width = bounds.Width - 2;
            bounds.Height = bounds.Height - 2;
            Region region = new Region();

            /*using (GraphicsPath graphicsPath = CreatePath(bounds, cornerDiameter, false)) {
                region.MakeEmpty();
                region.Union(graphicsPath);
                return region;
            }*/
            using (GraphicsPath graphicsPath = GetBackgroundPath(cornerDiameter, bounds))
            {
                region.MakeEmpty();
                region.Union(graphicsPath);
                return region;
            }
        }

        /// <summary>Gets border rectangle</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="BorderWidth">The borderWidth</param>
        /// <returns>The retrieved border rectangle</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when BorderWidth is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Rectangle GetBorderRectangle(Rectangle bounds, int BorderWidth)
        {
            if (BorderWidth <= 1)
            {
                bounds.Width -= BorderWidth;
            }
            else
            {
                bounds.Width -= BorderWidth / 2;
            }
            if (BorderWidth <= 1)
            {
                bounds.Height -= BorderWidth;
            }
            else
            {
                bounds.Height -= BorderWidth / 2;
            }
            if (BorderWidth > 1)
            {
                bounds.X += BorderWidth / 2;
                bounds.Width -= BorderWidth / 2;
            }
            if (BorderWidth > 1)
            {
                bounds.Y += BorderWidth / 2;
                bounds.Height -= BorderWidth / 2;
            }
            return bounds;
        }

        /// <summary>Gets background rectangle</summary>
        /// <param name="style">The style</param>
        /// <param name="bounds">The bounds</param>
        /// <returns>The retrieved background rectangle</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Rectangle GetBackgroundRectangle(MElementStyle style, Rectangle bounds)
        {
            if (style == null)
            {
                style = new MElementStyle();
            }
            bounds.X += style.MarginLeft;
            bounds.Width -= style.MarginLeft + style.MarginRight;
            bounds.Y += style.MarginTop;
            bounds.Height -= style.MarginTop + style.MarginBottom;
            return bounds;
        }

        /// <summary>Performs paint border</summary>
        /// <param name="e">The e</param>
        /// <returns>The result of the paint border</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Rectangle PaintBorder(MElementStyleDisplayInfo e)
        {
            MElementStyle elementStyle = e.Style.Copy();
            var backgroundRectangle = GetBackgroundRectangle(elementStyle, e.Bounds);
            Rectangle borderRectangle = GetBorderRectangle(backgroundRectangle, e.BorderWidth);
            if (borderRectangle.Width >= 2 && borderRectangle.Height >= 2)
            {
                if (e.BorderWidth == 1)
                {
                    borderRectangle.Width++;
                    borderRectangle.Height++;
                }
                e.BackgroundRectangle = backgroundRectangle;
                return DrawRoundGradientRectangle(e, borderRectangle);
            }
            return borderRectangle;
        }

        /// <summary>Performs border rectangle</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="BorderWidth">The borderWidth</param>
        /// <param name="elementStyle">The elementStyle</param>
        /// <returns>The result of the border rectangle</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when BorderWidth is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Rectangle BorderRectangle(Rectangle bounds, int BorderWidth, MElementStyle elementStyle = null)
        {
            if (elementStyle == null)
                elementStyle = new MElementStyle();
            var backgroundRectangle = GetBackgroundRectangle(elementStyle, bounds);
            Rectangle borderRectangle = GetBorderRectangle(backgroundRectangle, BorderWidth);
            if (borderRectangle.Width >= 2 && borderRectangle.Height >= 2)
            {
                borderRectangle.Width--;
                borderRectangle.Height--;
            }
            return borderRectangle;
        }

        /// <summary>Performs border rectangle</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="BorderWidth">The borderWidth</param>
        /// <returns>The result of the border rectangle</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when BorderWidth is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Rectangle BorderRectangle(Rectangle bounds, int BorderWidth)
        {
            MElementStyle elementStyle = new MElementStyle() { MarginBottom = 0, MarginLeft = 0, MarginRight = 0, MarginTop = 0 };
            Rectangle backgroundRectangle = GetBackgroundRectangle(elementStyle, bounds);
            Rectangle borderRectangle = GetBorderRectangle(backgroundRectangle, BorderWidth);
            if (borderRectangle.Width >= 2 && borderRectangle.Height >= 2)
            {
                if (BorderWidth == 1)
                {
                    borderRectangle.Width++;
                    borderRectangle.Height++;
                }
                //CorPaintHelper.DrawRoundGradientRectangle(e, borderRectangle);
            }
            return borderRectangle;
        }

        /// <summary>Gets region figure path</summary>
        /// <param name="radius">The radius</param>
        /// <param name="bounds">The bounds</param>
        /// <returns>The retrieved region figure path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static GraphicsPath GetRegionFigurePath(int radius, Rectangle bounds)
        {
            float curveSize = radius * 2F;
            float curveSize1 = curveSize;
            GraphicsPath path = new GraphicsPath();
            /*int X = bounds.X;
            int Y = bounds.Y;
            int width = bounds.Width;
            int height = bounds.Height;
            //Upper-right arc:
            path.AddArc(X + width - curveSize1, Y, curveSize1, curveSize1, 270, 90);
            //Lower-right arc:
            path.AddArc(X + width - curveSize1, Y + height - curveSize1, curveSize1, curveSize1, 0, 90);
            //Lower-left arc:
            path.AddArc(X, Y + height - curveSize, curveSize, curveSize, 90, 90);
            //Upper-left arc:
            path.AddArc(X, Y, curveSize, curveSize, 180, 90);
            path.CloseFigure();*/





            path.StartFigure();
            path.AddArc(bounds.X, bounds.Y, curveSize, curveSize, 180, 90);
            path.AddArc(bounds.Right - curveSize, bounds.Y, curveSize, curveSize, 270, 90);
            path.AddArc(bounds.Right - curveSize, bounds.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }


        /// <summary>Performs draw round rectangle</summary>
        /// <param name="g">The g</param>
        /// <param name="bounds">The bounds</param>
        /// <param name="BorderWidth">The borderWidth</param>
        /// <param name="BorderColor">The borderColor</param>
        /// <param name="BackColor">The backColor</param>
        /// <param name="CornerDiameter">The cornerDiameter</param>
        /// <exception cref="System.ArgumentNullException">Thrown when BorderWidth is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when CornerDiameter is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DrawRoundRectangle(Graphics g, Rectangle bounds, int BorderWidth, Color BorderColor, Color BackColor, int CornerDiameter)
        {
            Rectangle borderRectangle = GetBorderRectangle(bounds, BorderWidth);
            if (borderRectangle.Width >= 2 && borderRectangle.Height >= 2)
            {
                if (BorderWidth == 1)
                {
                    borderRectangle.Width++;
                    borderRectangle.Height++;
                }
                if (!BorderColor.IsEmpty && bounds.Width > 0 && bounds.Height > 0 && CornerDiameter > 0 && BorderWidth > 0)
                {
                    Rectangle r = bounds;
                    r.Width--;
                    r.Height--;
                    using (GraphicsPath graphicsPath = GetBackgroundPath(CornerDiameter, r))
                    {
                        LinearGradientBrush linGrBrush = new LinearGradientBrush(graphicsPath.GetBounds(), BackColor, BackColor, 90);
                        g.FillPath(linGrBrush, graphicsPath);
                        using (Pen pen2 = new Pen(BorderColor, BorderWidth))
                        {
                            graphicsPath.Widen(pen2);
                        }
                        using (LinearGradientBrush brush = new LinearGradientBrush(bounds, BorderColor, BorderColor, 90))
                        {
                            g.FillPath(brush, graphicsPath);
                        }
                    }
                }
            }
        }

        /// <summary>Performs draw rectangle border</summary>
        /// <param name="g">The g</param>
        /// <param name="bounds">The bounds</param>
        /// <param name="BorderWidth">The borderWidth</param>
        /// <param name="BorderColor">The borderColor</param>
        /// <param name="BackColor">The backColor</param>
        /// <exception cref="System.ArgumentNullException">Thrown when BorderWidth is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DrawRectangleBorder(Graphics g, Rectangle bounds, int BorderWidth, Color BorderColor, Color BackColor)
        {
            Rectangle borderRectangle = GetBorderRectangle(bounds, BorderWidth);
            if (borderRectangle.Width >= 2 && borderRectangle.Height >= 2)
            {
                if (BorderWidth == 1)
                {
                    borderRectangle.Width++;
                    borderRectangle.Height++;
                }
                if (!BorderColor.IsEmpty && bounds.Width > 0 && bounds.Height > 0 && BorderWidth > 0)
                {
                    Rectangle r = bounds;
                    r.Width--;
                    r.Height--;
                    using (GraphicsPath graphicsPath = GetBackgroundPath(0, r))
                    {
                        LinearGradientBrush linGrBrush = new LinearGradientBrush(graphicsPath.GetBounds(), BackColor, BackColor, 90);
                        g.FillPath(linGrBrush, graphicsPath);
                        using (Pen pen2 = new Pen(BorderColor, BorderWidth))
                        {
                            graphicsPath.Widen(pen2);
                        }
                        using (LinearGradientBrush brush = new LinearGradientBrush(bounds, BorderColor, BorderColor, 90))
                        {
                            g.FillPath(brush, graphicsPath);
                        }
                    }
                }
            }
        }

        /// <summary>Gets region path</summary>
        /// <param name="cornerDiameter">The cornerDiameter</param>
        /// <param name="bounds">The bounds</param>
        /// <returns>The retrieved region path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when cornerDiameter is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath GetRegionPath(int cornerDiameter, Rectangle bounds)
        {
            bounds.Width--;
            bounds.Height--;
            return GetBackgroundPath(cornerDiameter, bounds);
        }


        /// <summary>Gets corner arc</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="cornerDiameter">The cornerDiameter</param>
        /// <param name="corner">The corner</param>
        /// <returns>The retrieved corner arc</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when cornerDiameter is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when corner is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static ArcData GetCornerArc(Rectangle bounds, int cornerDiameter, CornerArc corner)
        {
            ArcData a;
            if (cornerDiameter == 0) cornerDiameter = 1;
            int diameter = cornerDiameter * 2;
            switch (corner)
            {
                case CornerArc.TopLeft:
                    a = new ArcData(bounds.X, bounds.Y, diameter, diameter, 180, 90);
                    break;
                case CornerArc.TopRight:
                    a = new ArcData(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
                    break;
                case CornerArc.BottomLeft:
                    a = new ArcData(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
                    break;
                default: // eCornerArc.BottomRight:
                    a = new ArcData(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
                    break;
            }
            return a;
        }

        /// <summary>Performs add corner arc</summary>
        /// <param name="path">The path</param>
        /// <param name="bounds">The bounds</param>
        /// <param name="cornerDiameter">The cornerDiameter</param>
        /// <param name="corner">The corner</param>
        /// <exception cref="System.ArgumentNullException">Thrown when cornerDiameter is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when corner is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void AddCornerArc(GraphicsPath path, Rectangle bounds, int cornerDiameter, CornerArc corner)
        {
            if (cornerDiameter > 0)
            {
                ArcData a = GetCornerArc(bounds, cornerDiameter, corner);
                path.AddArc(a.X, a.Y, a.Width, a.Height, a.StartAngle, a.SweepAngle);
            }
            else
            {
                if (corner == CornerArc.TopLeft)
                {
                    path.AddLine(bounds.X, bounds.Y + 2, bounds.X, bounds.Y);
                }
                else if (corner == CornerArc.BottomLeft)
                {
                    path.AddLine(bounds.X + 2, bounds.Bottom, bounds.X, bounds.Bottom);
                }
                else if (corner == CornerArc.TopRight)
                {
                    path.AddLine(bounds.Right - 2, bounds.Y, bounds.Right, bounds.Y);
                }
                else if (corner == CornerArc.BottomRight)
                {
                    path.AddLine(bounds.Right, bounds.Bottom - 2, bounds.Right, bounds.Bottom);
                }
            }
        }

        /// <summary>Gets rounded rectangle path</summary>
        /// <param name="r">The r</param>
        /// <param name="cornerSize">The cornerSize</param>
        /// <returns>The retrieved rounded rectangle path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when cornerSize is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath GetRoundedRectanglePath(Rectangle r, int cornerSize)
        {
            GraphicsPath path = new GraphicsPath();
            if (cornerSize == 0)
                path.AddRectangle(r);
            else
            {
                AddCornerArc(path, r, cornerSize, CornerArc.TopLeft);
                AddCornerArc(path, r, cornerSize, CornerArc.TopRight);
                AddCornerArc(path, r, cornerSize, CornerArc.BottomRight);
                AddCornerArc(path, r, cornerSize, CornerArc.BottomLeft);
                path.CloseAllFigures();
            }
            return path;
        }

        /// <summary>Gets background path</summary>
        /// <param name="cornerDiameter">The cornerDiameter</param>
        /// <param name="bounds">The bounds</param>
        /// <returns>The retrieved background path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when cornerDiameter is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath GetBackgroundPath(int cornerDiameter, Rectangle bounds)
        {
            return GetBackgroundPath(cornerDiameter, cornerDiameter, cornerDiameter, cornerDiameter, bounds);
        }

        /// <summary>Gets background path</summary>
        /// <param name="e">The e</param>
        /// <param name="bounds">The bounds</param>
        /// <returns>The retrieved background path</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath GetBackgroundPath(MElementStyleDisplayInfo e, Rectangle bounds)
        {
            return GetBackgroundPath(e.TopLeftCorner, e.TopRightCorner, e.BottomRightCorner, e.BottomLeftCorner, bounds);
        }

        /// <summary>Gets background path</summary>
        /// <param name="TopLeftCorner">The topLeftCorner</param>
        /// <param name="TopRightCorner">The topRightCorner</param>
        /// <param name="BottomRightCorner">The bottomRightCorner</param>
        /// <param name="BottomLeftCorner">The bottomLeftCorner</param>
        /// <param name="bounds">The bounds</param>
        /// <returns>The retrieved background path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when TopLeftCorner is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when TopRightCorner is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when BottomRightCorner is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when BottomLeftCorner is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static GraphicsPath GetBackgroundPath(int TopLeftCorner, int TopRightCorner, int BottomRightCorner, int BottomLeftCorner, Rectangle bounds)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            ArcData cornerArc = GetCornerArc(bounds, TopLeftCorner, CornerArc.TopLeft);
            graphicsPath.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
            cornerArc = GetCornerArc(bounds, TopRightCorner, CornerArc.TopRight);
            graphicsPath.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
            cornerArc = GetCornerArc(bounds, BottomRightCorner, CornerArc.BottomRight);
            graphicsPath.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
            cornerArc = GetCornerArc(bounds, BottomLeftCorner, CornerArc.BottomLeft);
            graphicsPath.AddArc(cornerArc.X, cornerArc.Y, cornerArc.Width, cornerArc.Height, cornerArc.StartAngle, cornerArc.SweepAngle);
            graphicsPath.CloseAllFigures();

            return graphicsPath;
        }
        private float opacity = 1f;

        /// <summary>Performs office arrow rect from bounds</summary>
        /// <param name="bounds">The bounds</param>
        /// <returns>The result of the office arrow rect from bounds</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Rectangle OfficeArrowRectFromBounds(Rectangle bounds)
        {
            Rectangle rectangle = new Rectangle()
            {
                Height = 3,
                Width = 5
            };
            rectangle.X = bounds.X + (bounds.Width - rectangle.Width) / 2 + 1;
            rectangle.Y = bounds.Y + (bounds.Height - rectangle.Height) / 2 + 1;
            return rectangle;
        }

        /// <summary>Sets opacity to image</summary>
        /// <param name="bitmap">The bitmap</param>
        /// <param name="value">The value</param>
        /// <exception cref="System.ArgumentNullException">Thrown when value is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SetOpacityToImage(Bitmap bitmap, byte value)
        {
            for (int y = 0; y < bitmap.Height; ++y)
            {
                for (int x = 0; x < bitmap.Width; ++x)
                {
                    int num = bitmap.GetPixel(x, y).A - value;
                    if (num < 0)
                        num = 0;
                    bitmap.SetPixel(x, y, Color.FromArgb((byte)num, bitmap.GetPixel(x, y)));
                }
            }
        }

        /// <summary>Gets button rect</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="flat">The flat</param>
        /// <returns>The retrieved button rect</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when flat is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Rectangle GetButtonRect(Rectangle bounds, bool flat) => GetButtonRect(bounds, flat, SystemInformation.HorizontalScrollBarArrowWidth - 1);

        /// <summary>Gets button rect</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="flat">The flat</param>
        /// <param name="buttonWidth">The buttonWidth</param>
        /// <returns>The retrieved button rect</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when flat is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when buttonWidth is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Rectangle GetButtonRect(Rectangle bounds, bool flat, int buttonWidth)
        {
            int num1 = SystemInformation.Border3DSize.Width;
            int num2 = SystemInformation.Border3DSize.Height;
            if (flat)
                num1 = num2 = 1;
            return new Rectangle(bounds.Right - buttonWidth - num1, bounds.Top + num2, buttonWidth, bounds.Height - num2 * 2);
        }

        /// <summary>Performs draw item</summary>
        /// <param name="graphics">The graphics</param>
        /// <param name="bounds">The bounds</param>
        /// <param name="state">The state</param>
        /// <param name="backColor">The backColor</param>
        /// <param name="foreColor">The foreColor</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DrawItem(
                  Graphics graphics,
                  Rectangle bounds,
                  DrawItemState state,
                  Color backColor,
                  Color foreColor)
        {
            if ((state & DrawItemState.Focus) != DrawItemState.None)
                graphics.DrawRectangle(Pens.Black, bounds.X, bounds.Y, bounds.Width, bounds.Height - 1);
            else if ((state & DrawItemState.Selected) != DrawItemState.None)
            {
                graphics.DrawRectangle(Pens.Black, bounds.X, bounds.Y, bounds.Width, bounds.Height - 1);
            }
            else
            {
                using (SolidBrush solidBrush = new SolidBrush(backColor))
                    graphics.FillRectangle(solidBrush, bounds);
            }
        }

        /// <summary>Gets content rect</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="flat">The flat</param>
        /// <returns>The retrieved content rect</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when flat is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Rectangle GetContentRect(Rectangle bounds, bool flat)
        {
            Rectangle buttonRect = GetButtonRect(bounds, flat);
            int num1 = SystemInformation.Border3DSize.Width;
            int num2 = SystemInformation.Border3DSize.Height;
            if (flat)
                num1 = num2 = 1;
            return new Rectangle(bounds.Left + num1, bounds.Top + num2, bounds.Width - num1 * 2 - buttonRect.Width, bounds.Height - num2 * 2);
        }

        /// <summary>Performs draw win7 glow</summary>
        /// <param name="g">The g</param>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="offsetX">The offsetX</param>
        /// <param name="offsetY">The offsetY</param>
        /// <param name="glowColor">The glowColor</param>
        /// <exception cref="System.ArgumentNullException">Thrown when offsetX is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when offsetY is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void DrawWin7Glow(
                  Graphics g,
                  Rectangle rectangle,
                  int offsetX,
                  int offsetY,
                  Color glowColor)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(offsetX - 5, offsetY, rectangle.Width + 11, rectangle.Height * 2);
                using (PathGradientBrush pathGradientBrush = new PathGradientBrush(path))
                {
                    pathGradientBrush.CenterColor = glowColor;
                    pathGradientBrush.SurroundColors = new Color[2]
                    {
            Color.FromArgb( byte.MaxValue, glowColor),
            Color.FromArgb(0, glowColor)
                    };
                    g.FillPath(pathGradientBrush, path);
                }
            }
        }

        /// <summary>Performs draw win7 glow</summary>
        /// <param name="g">The g</param>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="offsetX">The offsetX</param>
        /// <param name="offsetY">The offsetY</param>
        /// <param name="glowColor">The glowColor</param>
        /// <param name="surroundColors">The surroundColors</param>
        /// <exception cref="System.ArgumentNullException">Thrown when offsetX is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when offsetY is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void DrawWin7Glow(
                  Graphics g,
                  Rectangle rectangle,
                  int offsetX,
                  int offsetY,
                  Color glowColor,
                  Color[] surroundColors)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(offsetX - 5, offsetY, rectangle.Width + 11, rectangle.Height * 2);
                using (PathGradientBrush pathGradientBrush = new PathGradientBrush(path))
                {
                    pathGradientBrush.CenterColor = glowColor;
                    pathGradientBrush.SurroundColors = surroundColors;
                    g.FillPath(pathGradientBrush, path);
                }
            }
        }

        /// <summary>Performs draw glow</summary>
        /// <param name="g">The g</param>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="glowColor">The glowColor</param>
        /// <param name="centerColor">The centerColor</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void DrawGlow(Graphics g, Rectangle rectangle, Color glowColor, Color centerColor)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(-5, rectangle.Height / 2 - 10, rectangle.Width + 11, rectangle.Height + 11);
                using (PathGradientBrush pathGradientBrush = new PathGradientBrush(path))
                {
                    pathGradientBrush.CenterColor = centerColor;
                    pathGradientBrush.SurroundColors = new Color[1]
                    {
            Color.FromArgb(0, glowColor)
                    };
                    g.FillPath(pathGradientBrush, path);
                }
            }
        }

        /// <summary>Performs draw glow</summary>
        /// <param name="g">The g</param>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="glowColor">The glowColor</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void DrawGlow(Graphics g, Rectangle rectangle, Color glowColor)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(-5, rectangle.Height / 2 - 10, rectangle.Width + 11, rectangle.Height + 11);
                using (PathGradientBrush pathGradientBrush = new PathGradientBrush(path))
                {
                    pathGradientBrush.CenterColor = Color.FromArgb(128, glowColor);
                    pathGradientBrush.SurroundColors = new Color[1]
                    {
            Color.FromArgb(0, glowColor)
                    };
                    g.FillPath(pathGradientBrush, path);
                }
            }
        }

        /// <summary>Performs draw glow ellipse</summary>
        /// <param name="g">The g</param>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="colors">The colors</param>
        /// <param name="centerColor">The centerColor</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void DrawGlowEllipse(
                  Graphics g,
                  Rectangle rectangle,
                  Color[] colors,
                  Color centerColor)
        {
            rectangle.Inflate(-1, -1);
            DrawCircleButton(g, rectangle, colors);
        }

        /// <summary>Performs draw circle button</summary>
        /// <param name="g">The g</param>
        /// <param name="r">The r</param>
        /// <param name="colors">The colors</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void DrawCircleButton(Graphics g, Rectangle r, Color[] colors)
        {
            Rectangle rect1 = r;
            rect1.Inflate(-1, -1);
            Rectangle rect2 = r;
            rect2.Offset(1, 1);
            rect2.Inflate(2, 2);
            Color color1 = colors[0];
            Color color2 = colors[1];
            Color color3 = colors.Length <= 2 ? colors[1] : colors[2];
            Color whiteSmoke = Color.WhiteSmoke;
            GraphicsPath path1;
            using (path1 = new GraphicsPath())
            {
                path1.AddEllipse(rect2);
                PathGradientBrush pathGradientBrush;
                using (pathGradientBrush = new PathGradientBrush(path1))
                {
                    pathGradientBrush.WrapMode = WrapMode.Clamp;
                    pathGradientBrush.CenterPoint = new PointF(rect2.Left + rect2.Width / 2, rect2.Top + rect2.Height / 2);
                    pathGradientBrush.CenterColor = Color.FromArgb(180, Color.Black);
                    Color[] colorArray = new Color[1]
                    {
            Color.Transparent
                    };
                    pathGradientBrush.SurroundColors = colorArray;
                    pathGradientBrush.Blend = new Blend(3)
                    {
                        Factors = new float[3] { 0.0f, 1f, 1f },
                        Positions = new float[3] { 0.0f, 0.2f, 1f }
                    };
                    g.FillPath(pathGradientBrush, path1);
                }
            }
            using (Pen pen = new Pen(color1, 1f))
                g.DrawEllipse(pen, r);
            GraphicsPath path2;
            using (path2 = new GraphicsPath())
            {
                path2.AddEllipse(r);
                PathGradientBrush pathGradientBrush;
                using (pathGradientBrush = new PathGradientBrush(path2))
                {
                    pathGradientBrush.WrapMode = WrapMode.Clamp;
                    pathGradientBrush.CenterPoint = new PointF(Convert.ToSingle(r.Left + r.Width / 2), Convert.ToSingle(r.Bottom));
                    pathGradientBrush.CenterColor = color3;
                    Color[] colorArray = new Color[1] { color2 };
                    pathGradientBrush.SurroundColors = colorArray;
                    pathGradientBrush.Blend = new Blend(3)
                    {
                        Factors = new float[3] { 0.0f, 0.8f, 1f },
                        Positions = new float[3] { 0.0f, 0.5f, 1f }
                    };
                    g.FillPath(pathGradientBrush, path2);
                }
            }
            Rectangle rect3 = new Rectangle(0, 0, r.Width / 2, r.Height / 2);
            rect3 = new Rectangle(r.X + (r.Width - rect3.Width) / 2, r.Y + r.Height / 2, rect3.Width, rect3.Height);
            GraphicsPath path3;
            using (path3 = new GraphicsPath())
            {
                path3.AddEllipse(rect3);
                PathGradientBrush pathGradientBrush;
                using (pathGradientBrush = new PathGradientBrush(path3))
                {
                    pathGradientBrush.WrapMode = WrapMode.Clamp;
                    pathGradientBrush.CenterPoint = new PointF(Convert.ToSingle(r.Left + r.Width / 2), Convert.ToSingle(r.Bottom));
                    pathGradientBrush.CenterColor = Color.White;
                    Color[] colorArray = new Color[1]
                    {
            Color.Transparent
                    };
                    pathGradientBrush.SurroundColors = colorArray;
                    g.FillPath(pathGradientBrush, path3);
                }
            }
            GraphicsPath path4;
            using (path4 = new GraphicsPath())
            {
                int num1 = 160;
                int num2 = 180 + (180 - num1) / 2;
                path4.AddArc(rect1, num2, num1);
                Point point1 = Point.Round(path4.PathData.Points[0]);
                Point point2 = Point.Round(path4.PathData.Points[path4.PathData.Points.Length - 1]);
                Point point3 = new Point(rect1.Left + rect1.Width / 2, point2.Y - 3);
                Point[] points = new Point[3]
                {
          point2,
          point3,
          point1
                };
                path4.AddCurve(points);
                PathGradientBrush pathGradientBrush;
                using (pathGradientBrush = new PathGradientBrush(path4))
                {
                    pathGradientBrush.WrapMode = WrapMode.Clamp;
                    pathGradientBrush.CenterPoint = point3;
                    pathGradientBrush.CenterColor = Color.Transparent;
                    Color[] colorArray = new Color[1]
                    {
            whiteSmoke
                    };
                    pathGradientBrush.SurroundColors = colorArray;
                    Blend blend = new Blend(3)
                    {
                        Factors = new float[3] { 0.3f, 0.8f, 1f }
                    };
                    blend.Positions = new float[3] { 0.0f, 0.5f, 1f };
                    pathGradientBrush.Blend = blend;
                    g.FillPath(pathGradientBrush, path4);
                }
                LinearGradientBrush linearGradientBrush;
                using (linearGradientBrush = new LinearGradientBrush(new Point(r.Left, r.Top), new Point(r.Left, point1.Y), Color.White, Color.Transparent))
                {
                    Blend blend = new Blend(4)
                    {
                        Factors = new float[4] { 0.0f, 0.4f, 0.8f, 1f },
                        Positions = new float[4] { 0.0f, 0.3f, 0.4f, 1f }
                    };
                    linearGradientBrush.Blend = blend;
                    g.FillPath(linearGradientBrush, path4);
                }
            }
            GraphicsPath path5;
            using (path5 = new GraphicsPath())
            {
                int num1 = 160;
                int num2 = 180 + (180 - num1) / 2;
                path5.AddArc(rect1, num2, num1);
                using (Pen pen = new Pen(Color.White))
                    g.DrawPath(pen, path5);
            }
            GraphicsPath path6;
            using (path6 = new GraphicsPath())
            {
                int num1 = 160;
                int num2 = (180 - num1) / 2;
                path6.AddArc(rect1, num2, num1);
                Point point = Point.Round(path6.PathData.Points[0]);
                Rectangle rect4 = rect1;
                rect4.Inflate(-1, -1);
                int num3 = 160;
                int num4 = (180 - num3) / 2;
                path6.AddArc(rect4, num4, num3);
                LinearGradientBrush linearGradientBrush;
                using (linearGradientBrush = new LinearGradientBrush(new Point(rect1.Left, rect1.Bottom), new Point(rect1.Left, point.Y - 1), whiteSmoke, Color.FromArgb(50, whiteSmoke)))
                    g.FillPath(linearGradientBrush, path6);
            }
        }

        /// <summary>Performs draw ellipses border</summary>
        /// <param name="g">The g</param>
        /// <param name="colors">The colors</param>
        /// <param name="smoothingMode">The smoothingMode</param>
        /// <param name="rect">The rect</param>
        /// <param name="path">The path</param>
        /// <param name="brush">The brush</param>
        /// <param name="colorArray">The colorArray</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void DrawEllipsesBorder(
                  Graphics g,
                  Color[] colors,
                  SmoothingMode smoothingMode,
                  ref Rectangle rect,
                  GraphicsPath path,
                  ref PathGradientBrush brush,
                  ref Color[] colorArray)
        {
            path.Reset();
            path.AddEllipse(rect);
            ++rect.Width;
            DrawFillGlowGradientRectangleBackground(rect, path, colors, 1f, g);
            --rect.Width;
            GraphicsPath path1 = new GraphicsPath();
            rect.Offset(-1, rect.Height / 2);
            path1.AddEllipse(rect);
            brush = new PathGradientBrush(path1);
            ColorBlend colorBlend = new ColorBlend(3);
            colorBlend.Positions = new float[3] { 0.0f, 0.6f, 1f };
            colorArray = new Color[3]
            {
        Color.Transparent,
        Color.Transparent,
        Color.White
            };
            colorBlend.Colors = colorArray;
            brush.InterpolationColors = colorBlend;
            brush.Dispose();
            brush.Dispose();
            path.Dispose();
            g.SmoothingMode = smoothingMode;
        }

        /// <summary>Performs draw ellipses fill</summary>
        /// <param name="g">The g</param>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="colors">The colors</param>
        /// <param name="centerColor">The centerColor</param>
        /// <param name="smoothingMode">The smoothingMode</param>
        /// <param name="rect">The rect</param>
        /// <param name="path">The path</param>
        /// <param name="brush">The brush</param>
        /// <param name="colorArray">The colorArray</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal void DrawEllipsesFill(
                  Graphics g,
                  ref Rectangle rectangle,
                  Color[] colors,
                  ref Color centerColor,
                  out SmoothingMode smoothingMode,
                  out Rectangle rect,
                  out GraphicsPath path,
                  out PathGradientBrush brush,
                  out Color[] colorArray)
        {
            Rectangle rectangle1 = rectangle;
            rectangle1.X += 2;
            rectangle1.Y += 2;
            rectangle1.Width -= 2;
            rectangle1.Height -= 2;
            smoothingMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            rect = new Rectangle(rectangle1.X + 2, rectangle1.Y + 2, rectangle1.Width - 2, rectangle1.Height - 2);
            path = new GraphicsPath();
            path.AddEllipse(rect);
            brush = new PathGradientBrush(path);
            brush.CenterColor = centerColor;
            colorArray = new Color[1] { Color.Empty };
            brush.SurroundColors = colorArray;
            brush.FocusScales = new PointF(0.85f, 0.85f);
            brush.Dispose();
            rect = new Rectangle(rectangle1.X, rectangle1.Y, rectangle1.Width, rectangle1.Height);
            path.Reset();
            path.AddEllipse(rect);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.SmoothingMode = smoothingMode;
        }

        /// <summary>Performs fill glow path foreground</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="path">The path</param>
        /// <param name="colors">The colors</param>
        /// <param name="borderWidth">The borderWidth</param>
        /// <param name="graphics">The graphics</param>
        /// <returns>The result of the fill glow path foreground</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when borderWidth is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual object FillGlowPathForeground(
                  Rectangle bounds,
                  GraphicsPath path,
                  Color[] colors,
                  float borderWidth,
                  Graphics graphics)
        {
            if (bounds.Width > 0 && bounds.Height > 0 && borderWidth > 0.0)
            {
                if (colors[0] == Color.Empty)
                    return null;
                SmoothingMode smoothingMode = graphics.SmoothingMode;
                Pen pen = new Pen(colors[1], borderWidth);
                graphics.DrawPath(pen, path);
                pen.Dispose();
                graphics.SmoothingMode = smoothingMode;
            }
            return null;
        }

        /// <summary>Performs draw fill glow gradient rectangle background</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="backPath">The backPath</param>
        /// <param name="colors">The colors</param>
        /// <param name="borderWidth">The borderWidth</param>
        /// <param name="graphics">The graphics</param>
        /// <exception cref="System.ArgumentNullException">Thrown when borderWidth is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void DrawFillGlowGradientRectangleBackground(
                  Rectangle bounds,
                  GraphicsPath backPath,
                  Color[] colors,
                  float borderWidth,
                  Graphics graphics)
        {
            Orientation orientation = Orientation.Horizontal;
            float num1 = 0.5f;
            float num2 = 1f;
            int offset = 15;
            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;
            Rectangle boundsToUseForBrushes = bounds;
            Color color1 = colors[0];
            Color color2 = colors[1];
            float gradientOffsetFactor = 10f;
            int num3;
            Rectangle rectangle2;
            Rectangle rectangle4;
            CalculateEllipses(orientation, offset, ref boundsToUseForBrushes, ref gradientOffsetFactor, out num3, out int _, out rectangle2, out Rectangle _, out rectangle4, out int _);
            float num4 = num1 > 0.0 ? num1 / 100f : 0.0f;
            float num5 = num2 > 0.0 ? num2 / 100f : 0.0f;
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush(new Rectangle(boundsToUseForBrushes.Left - 1, boundsToUseForBrushes.Top - 1, boundsToUseForBrushes.Width + 2, boundsToUseForBrushes.Height + 2), color1, color2, num3);
            linearGradientBrush.Blend = new Blend()
            {
                Factors = new float[3] { 0.0f, num4, num5 },
                Positions = new float[3]
              {
          0.0f,
          gradientOffsetFactor,
          1f
              }
            };
            DrawGradientPathFigure(graphics, backPath, bounds, colors, new float[4]
            {
        0.0f,
        0.5f,
        0.5f,
        1f
            }, GradientStyles.Linear, 90.0, 0.5, 0.5);
            Blend blend = new Blend();
            blend.Factors = new float[3] { 1f, 1f, num5 };
            float[] numArray = new float[3] { 0.0f, num5, 1f };
            blend.Positions = numArray;
            linearGradientBrush.Blend = blend;
            Region savedRegion = SetClipRegion(graphics, new Region(rectangle2), CombineMode.Exclude);
            DrawGradientPathFigure(graphics, backPath, bounds, colors, new float[4]
            {
        0.0f,
        0.5f,
        0.5f,
        1f
            }, GradientStyles.Linear, 90.0, 0.5, 0.5);
            DrawCenterEllipse(backPath, colors, graphics, rectangle4);
            RestoreClipRegion(graphics, savedRegion);
            if (borderWidth > 0.0)
                graphics.DrawPath(new Pen(color1), backPath);
            linearGradientBrush.Dispose();
        }

        /// <summary>Calculates calculate ellipses</summary>
        /// <param name="orientation">The orientation</param>
        /// <param name="offset">The offset</param>
        /// <param name="boundsToUseForBrushes">The boundsToUseForBrushes</param>
        /// <param name="gradientOffsetFactor">The gradientOffsetFactor</param>
        /// <param name="num">The num</param>
        /// <param name="gradientOffset">The gradientOffset</param>
        /// <param name="rectangle2">The rectangle2</param>
        /// <param name="rectangle3">The rectangle3</param>
        /// <param name="rectangle4">The rectangle4</param>
        /// <param name="num3">The num3</param>
        /// <exception cref="System.ArgumentNullException">Thrown when offset is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when gradientOffsetFactor is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when num is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when gradientOffset is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when num3 is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void CalculateEllipses(
                  Orientation orientation,
                  int offset,
                  ref Rectangle boundsToUseForBrushes,
                  ref float gradientOffsetFactor,
                  out int num,
                  out int gradientOffset,
                  out Rectangle rectangle2,
                  out Rectangle rectangle3,
                  out Rectangle rectangle4,
                  out int num3)
        {
            if (orientation == Orientation.Horizontal)
            {
                gradientOffsetFactor = offset / boundsToUseForBrushes.Height;
                gradientOffset = offset;
                rectangle2 = new Rectangle(boundsToUseForBrushes.Left, boundsToUseForBrushes.Top, boundsToUseForBrushes.Width, gradientOffset);
                rectangle3 = new Rectangle(boundsToUseForBrushes.Left, boundsToUseForBrushes.Top + gradientOffset - 1, boundsToUseForBrushes.Width, boundsToUseForBrushes.Height - gradientOffset + 1);
                num3 = (int)(0.100000001490116 * rectangle3.Width);
                rectangle4 = new Rectangle(rectangle3.Left - num3, rectangle3.Top, rectangle3.Width + 2 * num3, rectangle3.Height * 2);
                num = 90;
            }
            else
            {
                gradientOffsetFactor = offset / boundsToUseForBrushes.Width;
                gradientOffset = offset;
                rectangle2 = new Rectangle(boundsToUseForBrushes.Left, boundsToUseForBrushes.Top, gradientOffset, boundsToUseForBrushes.Height);
                rectangle3 = new Rectangle(boundsToUseForBrushes.Left + gradientOffset - 1, boundsToUseForBrushes.Top, boundsToUseForBrushes.Width - gradientOffset + 1, boundsToUseForBrushes.Height);
                num3 = (int)(0.100000001490116 * rectangle3.Height);
                rectangle4 = new Rectangle(rectangle3.Left, rectangle3.Top - num3, rectangle3.Width * 2, rectangle3.Height + 2 * num3);
                num = 0;
            }
        }

        /// <summary>Performs draw center ellipse</summary>
        /// <param name="backPath">The backPath</param>
        /// <param name="colors">The colors</param>
        /// <param name="graphics">The graphics</param>
        /// <param name="rectangle4">The rectangle4</param>
        /// <returns>The result of the draw center ellipse</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Rectangle DrawCenterEllipse(
                  GraphicsPath backPath,
                  Color[] colors,
                  Graphics graphics,
                  Rectangle rectangle4)
        {
            if (rectangle4.Width > 0 && rectangle4.Height > 0)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(rectangle4);
                PathGradientBrush pathGradientBrush = new PathGradientBrush(path);
                pathGradientBrush.CenterColor = ControlPaint.LightLight(colors[0]);
                pathGradientBrush.SurroundColors = new Color[1]
                {
          Color.Transparent
                };
                graphics.FillPath(pathGradientBrush, backPath);
                pathGradientBrush.Dispose();
                path.Dispose();
            }
            return rectangle4;
        }

        /// <summary>Performs fill glow path background</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="path">The path</param>
        /// <param name="colors">The colors</param>
        /// <param name="borderWidth">The borderWidth</param>
        /// <param name="graphics">The graphics</param>
        /// <returns>The result of the fill glow path background</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when borderWidth is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private object FillGlowPathBackground(
                  Rectangle bounds,
                  GraphicsPath path,
                  Color[] colors,
                  float borderWidth,
                  Graphics graphics)
        {
            if (bounds.Width > 0 && bounds.Height > 0 && colors[0] != Color.Empty)
            {
                SmoothingMode smoothingMode = graphics.SmoothingMode;
                Brush brush = new SolidBrush(colors[0]);
                graphics.FillPath(brush, path);
                if (borderWidth > 0.0)
                {
                    Pen pen = new Pen(brush, borderWidth);
                    graphics.DrawPath(pen, path);
                    pen.Dispose();
                }
                brush.Dispose();
                graphics.SmoothingMode = smoothingMode;
            }
            return null;
        }

        /// <summary>Sets clip region</summary>
        /// <param name="graphics">The graphics</param>
        /// <param name="region">The region</param>
        /// <param name="combineMode">The combineMode</param>
        /// <returns>The result of the set clip region</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Region SetClipRegion(
                  Graphics graphics,
                  Region region,
                  CombineMode combineMode)
        {
            Region region1 = graphics.Clip.Clone();
            graphics.SetClip(region, combineMode);
            return region1;
        }

        /// <summary>Performs restore clip region</summary>
        /// <param name="graphics">The graphics</param>
        /// <param name="savedRegion">The savedRegion</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void RestoreClipRegion(Graphics graphics, Region savedRegion)
        {
            Region clip = graphics.Clip;
            graphics.SetClip(savedRegion, CombineMode.Replace);
            clip.Dispose();
        }

        /// <summary>Performs draw item</summary>
        /// <param name="graphics">The graphics</param>
        /// <param name="bounds">The bounds</param>
        /// <param name="state">The state</param>
        /// <param name="text">The text</param>
        /// <param name="imageList">The imageList</param>
        /// <param name="imageIndex">The imageIndex</param>
        /// <param name="font">The font</param>
        /// <param name="backColor">The backColor</param>
        /// <param name="foreColor">The foreColor</param>
        /// <param name="rightToLeft">The rightToLeft</param>
        /// <exception cref="System.ArgumentNullException">Thrown when imageIndex is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DrawItem(
                  Graphics graphics,
                  Rectangle bounds,
                  DrawItemState state,
                  string text,
                  ImageList imageList,
                  int imageIndex,
                  Font font,
                  Color backColor,
                  Color foreColor,
                  RightToLeft rightToLeft)
        {
            DrawItem(graphics, bounds, state, text, imageList, imageIndex, font, backColor, foreColor, 0, rightToLeft);
        }

        /// <summary>Performs draw item</summary>
        /// <param name="graphics">The graphics</param>
        /// <param name="bounds">The bounds</param>
        /// <param name="state">The state</param>
        /// <param name="text">The text</param>
        /// <param name="imageList">The imageList</param>
        /// <param name="imageIndex">The imageIndex</param>
        /// <param name="font">The font</param>
        /// <param name="backColor">The backColor</param>
        /// <param name="foreColor">The foreColor</param>
        /// <param name="textStartPos">The textStartPos</param>
        /// <param name="rightToLeft">The rightToLeft</param>
        /// <exception cref="System.ArgumentNullException">Thrown when imageIndex is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when textStartPos is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void DrawItem(
                  Graphics graphics,
                  Rectangle bounds,
                  DrawItemState state,
                  string text,
                  ImageList imageList,
                  int imageIndex,
                  Font font,
                  Color backColor,
                  Color foreColor,
                  int textStartPos,
                  RightToLeft rightToLeft)
        {
            DrawItem(graphics, bounds, state, backColor, foreColor);
            int num = 0;
            if (imageList != null && imageIndex >= 0 && imageIndex < imageList.Images.Count)
            {
                Rectangle rectangle = new Rectangle(bounds.X + 1, bounds.Y + (bounds.Height - imageList.ImageSize.Height) / 2, imageList.ImageSize.Width, imageList.ImageSize.Height);
                imageList.Draw(graphics, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, imageIndex);
                num = imageList.ImageSize.Width + 2;
            }
            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Center;
                format.FormatFlags = StringFormatFlags.NoWrap;
                format.Trimming = StringTrimming.EllipsisCharacter;
                if (rightToLeft == RightToLeft.Yes)
                    format.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                bounds.X += num + textStartPos;
                bounds.Width -= num + textStartPos;
                if (text == null)
                    return;
                using (SolidBrush solidBrush = new SolidBrush(foreColor))
                    graphics.DrawString(text, font, solidBrush, bounds, format);
            }
        }

        /// <summary>Performs draw path</summary>
        /// <param name="g">The g</param>
        /// <param name="path">The path</param>
        /// <param name="color">The color</param>
        /// <param name="penAlignment">The penAlignment</param>
        /// <param name="penWidth">The penWidth</param>
        /// <exception cref="System.ArgumentNullException">Thrown when penWidth is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void DrawPath(
                  Graphics g,
                  GraphicsPath path,
                  Color color,
                  PenAlignment penAlignment,
                  float penWidth)
        {
            DrawPath(g, path, color, penAlignment, penWidth, null);
        }

        /// <summary>Performs draw path</summary>
        /// <param name="g">The g</param>
        /// <param name="path">The path</param>
        /// <param name="color">The color</param>
        /// <param name="penAlignment">The penAlignment</param>
        /// <param name="penWidth">The penWidth</param>
        /// <param name="brush">The brush</param>
        /// <exception cref="System.ArgumentNullException">Thrown when penWidth is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void DrawPath(
                  Graphics g,
                  GraphicsPath path,
                  Color color,
                  PenAlignment penAlignment,
                  float penWidth,
                  Brush brush)
        {
            using (Pen pen = new Pen(color))
            {
                pen.Width = penWidth;
                pen.Alignment = penAlignment;
                if (brush != null)
                    pen.Brush = brush;
                g.DrawPath(pen, path);
            }
        }

        /// <summary>Performs draw path arrow figure</summary>
        /// <param name="g">The g</param>
        /// <param name="color">The color</param>
        /// <param name="bounds">The bounds</param>
        /// <param name="direction">The direction</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void DrawPathArrowFigure(
                  Graphics g,
                  Color color,
                  Rectangle bounds,
                  ArrowDirection direction)
        {
            SmoothingMode smoothingMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Point[] pointArray = new Point[3];
            int x1 = bounds.X + bounds.Width / 2;
            int y1 = bounds.Y + bounds.Height / 2;
            int x2 = bounds.X;
            int x3 = bounds.X + bounds.Width - 1;
            int y2 = bounds.Y;
            int y3 = bounds.Y + bounds.Height - 1;
            if (bounds.Width % 2 == 0)
                ++x3;
            if (bounds.Height % 2 == 0)
                ++y3;
            using (new SolidBrush(color))
            {
                using (Pen pen = new Pen(color))
                {
                    switch (direction)
                    {
                        case ArrowDirection.Left:
                            pointArray = new Point[3]
                            {
                new Point(x3, y2),
                new Point(x2, y1),
                new Point(x3, y3)
                            };
                            break;
                        case ArrowDirection.Up:
                            pointArray = new Point[3]
                            {
                new Point(x2, y3),
                new Point(x1, y2),
                new Point(x3, y3)
                            };
                            break;
                        case ArrowDirection.Right:
                            pointArray = new Point[3]
                            {
                new Point(x2, y2),
                new Point(x3, y1),
                new Point(x2, y3)
                            };
                            break;
                        case ArrowDirection.Down:
                            pointArray = new Point[3]
                            {
                new Point(x2, y2),
                new Point(x1, y3),
                new Point(x3, y2)
                            };
                            break;
                    }
                    g.DrawLine(pen, pointArray[0], pointArray[1]);
                    g.DrawLine(pen, pointArray[1], pointArray[2]);
                }
            }
            g.SmoothingMode = smoothingMode;
        }

        /// <summary>Performs draw arrow figure</summary>
        /// <param name="g">The g</param>
        /// <param name="color">The color</param>
        /// <param name="bounds">The bounds</param>
        /// <param name="direction">The direction</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void DrawArrowFigure(
                  Graphics g,
                  Color color,
                  Rectangle bounds,
                  ArrowDirection direction)
        {
            SmoothingMode smoothingMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.None;
            Point[] points = new Point[3];
            int x1 = bounds.X + bounds.Width / 2;
            int y1 = bounds.Y + bounds.Height / 2;
            int x2 = bounds.X;
            int x3 = bounds.X + bounds.Width - 1;
            int y2 = bounds.Y;
            int y3 = bounds.Y + bounds.Height - 1;
            if (bounds.Width % 2 == 0)
                ++x3;
            if (bounds.Height % 2 == 0)
                ++y3;
            using (SolidBrush solidBrush = new SolidBrush(color))
            {
                using (Pen pen = new Pen(color))
                {
                    switch (direction)
                    {
                        case ArrowDirection.Left:
                            points = new Point[3]
                            {
                new Point(x3, y2),
                new Point(x2, y1),
                new Point(x3, y3)
                            };
                            break;
                        case ArrowDirection.Up:
                            points = new Point[3]
                            {
                new Point(x2, y3),
                new Point(x1, y2),
                new Point(x3, y3)
                            };
                            break;
                        case ArrowDirection.Right:
                            points = new Point[3]
                            {
                new Point(x2, y2),
                new Point(x3, y1),
                new Point(x2, y3)
                            };
                            break;
                        case ArrowDirection.Down:
                            points = new Point[3]
                            {
                new Point(x2, y2),
                new Point(x3, y2),
                new Point(x1, y3)
                            };
                            break;
                    }
                    g.FillPolygon(solidBrush, points);
                    g.DrawPolygon(pen, points);
                }
            }
            g.SmoothingMode = smoothingMode;
        }

        /// <summary>Performs draw image and text rectangle</summary>
        /// <param name="g">The g</param>
        /// <param name="wrapText">The wrapText</param>
        /// <param name="textImageOffset">The textImageOffset</param>
        /// <param name="rect">The rect</param>
        /// <param name="draw">The draw</param>
        /// <param name="font">The font</param>
        /// <param name="textBrush">The textBrush</param>
        /// <param name="imageAlign">The imageAlign</param>
        /// <param name="textAlign">The textAlign</param>
        /// <param name="text">The text</param>
        /// <param name="image">The image</param>
        /// <param name="relation">The relation</param>
        /// <returns>The result of the draw image and text rectangle</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when wrapText is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when textImageOffset is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when draw is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public Rectangle DrawImageAndTextRectangle(
                  Graphics g,
                  bool wrapText,
                  int textImageOffset,
                  Rectangle rect,
                  bool draw,
                  Font font,
                  SolidBrush textBrush,
                  ContentAlignment imageAlign,
                  ContentAlignment textAlign,
                  string text,
                  Image image,
                  TextImageRelation relation)
        {
            SizeF sizeF = g.MeasureString(text, font);
            int width = Size.Ceiling(sizeF).Width;
            int height = Size.Ceiling(sizeF).Height;
            int x = rect.X;
            int y1 = rect.Y;
            Rectangle Bounds = Rectangle.Empty;
            int top = y1;
            int bottom = y1 + rect.Height - image.Height;
            int middle = y1 + rect.Height / 2 - image.Height / 2;
            int left = x;
            int center = x + rect.Width / 2 - image.Width / 2;
            int right = x + rect.Width - image.Width;
            switch (relation)
            {
                case TextImageRelation.Overlay:
                    Bounds = rect;
                    if (draw)
                    {
                        DrawImageAndTextOverlay(g, imageAlign, image, top, bottom, middle, left, center, right);
                        break;
                    }
                    break;
                case TextImageRelation.ImageAboveText:
                    Bounds = new Rectangle(x, y1 + textImageOffset + image.Height, rect.Width, rect.Height - image.Height);
                    if (draw)
                    {
                        DrawImageAboveText(g, imageAlign, image, top, left, center, right);
                        break;
                    }
                    break;
                case TextImageRelation.TextAboveImage:
                    Bounds = new Rectangle(x, y1, rect.Width, rect.Height - image.Height);
                    if (draw)
                    {
                        int y2 = y1;
                        switch (textAlign)
                        {
                            case ContentAlignment.TopLeft:
                            case ContentAlignment.TopCenter:
                            case ContentAlignment.TopRight:
                                y2 = top + height + textImageOffset;
                                break;
                            case ContentAlignment.MiddleLeft:
                            case ContentAlignment.MiddleCenter:
                            case ContentAlignment.MiddleRight:
                                y2 = Bounds.Bottom + textImageOffset;
                                break;
                            case ContentAlignment.BottomLeft:
                            case ContentAlignment.BottomCenter:
                            case ContentAlignment.BottomRight:
                                y2 = bottom;
                                break;
                        }
                        DrawTextAboveImage(g, textImageOffset, imageAlign, image, height, y2, left, center, right);
                        break;
                    }
                    break;
                case TextImageRelation.ImageBeforeText:
                    Bounds = new Rectangle(x + textImageOffset + image.Width, y1, rect.Width - textImageOffset - image.Width, rect.Height);
                    if (draw)
                    {
                        DrawImageBeforeText(g, imageAlign, image, top, bottom, middle, left);
                        break;
                    }
                    break;
                case TextImageRelation.TextBeforeImage:
                    Bounds = new Rectangle(x + textImageOffset, y1, rect.Width - textImageOffset - image.Width, rect.Height);
                    if (draw)
                    {
                        rect = DrawTextBeforeImage(g, textImageOffset, rect, imageAlign, image, x, top, bottom, middle);
                        break;
                    }
                    break;
            }
            if (draw)
                DrawText(g, Bounds, wrapText, textBrush.Color, text, font, textAlign);
            return rect;
        }

        /// <summary>Creates a new create partially rounded path</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="radius">The radius</param>
        /// <param name="roundedCornersBitmask">The roundedCornersBitmask</param>
        /// <returns>The newly created create partially rounded path</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when roundedCornersBitmask is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public GraphicsPath CreatePartiallyRoundedPath(Rectangle bounds, int radius, byte roundedCornersBitmask)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            if (bounds.Height <= 0 || bounds.Width <= 0)
                return graphicsPath;
            if (radius <= 0.0)
            {
                graphicsPath.AddRectangle(bounds);
                graphicsPath.CloseFigure();
                return graphicsPath;
            }
            float num = radius * 2f;
            SizeF size = new SizeF(num, num);
            RectangleF rect = new RectangleF(bounds.Location, size);
            if ((roundedCornersBitmask & 1) == 1)
                graphicsPath.AddArc(rect, 180f, 90f);
            else
                graphicsPath.AddLine(bounds.Left, bounds.Top, bounds.Left + 1, bounds.Top);
            rect.X = bounds.Right - num;
            if ((roundedCornersBitmask & 2) == 2)
                graphicsPath.AddArc(rect, 270f, 90f);
            else
                graphicsPath.AddLine(bounds.Right, bounds.Top, bounds.Right, bounds.Top + 1);
            rect.Y = bounds.Bottom - num;
            if ((roundedCornersBitmask & 8) == 8)
                graphicsPath.AddArc(rect, 0.0f, 90f);
            else
                graphicsPath.AddLine(bounds.Right, bounds.Bottom, bounds.Right - 1, bounds.Bottom);
            rect.X = bounds.Left;
            if ((roundedCornersBitmask & 4) == 4)
                graphicsPath.AddArc(rect, 90f, 90f);
            else
                graphicsPath.AddLine(bounds.Left + 1, bounds.Bottom, bounds.Left, bounds.Bottom);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }

        /// <summary>Gets horizontal alignment</summary>
        /// <param name="TextAlignment">The textAlignment</param>
        /// <returns>The retrieved horizontal alignment</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public StringAlignment GetHorizontalAlignment(ContentAlignment TextAlignment)
        {
            switch (TextAlignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    return StringAlignment.Near;
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    return StringAlignment.Center;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    return StringAlignment.Far;
                default:
                    return StringAlignment.Near;
            }
        }

        /// <summary>Gets vertical alignment</summary>
        /// <param name="TextAlignment">The textAlignment</param>
        /// <returns>The retrieved vertical alignment</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public StringAlignment GetVerticalAlignment(ContentAlignment TextAlignment)
        {
            switch (TextAlignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    return StringAlignment.Near;
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    return StringAlignment.Center;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    return StringAlignment.Far;
                default:
                    return StringAlignment.Near;
            }
        }
        // public GraphicsPath GetRoundedPathRect(Rectangle bounds,int radius) => this.CreatePartiallyRoundedPath(bounds,radius,byte.MaxValue);

        /// <summary>Gets rounded path rect</summary>
        /// <param name="clientBounds">The clientBounds</param>
        /// <param name="radius">The radius</param>
        /// <returns>The retrieved rounded path rect</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public GraphicsPath GetRoundedPathRect(Rectangle clientBounds, int radius)
        {
            clientBounds.Height -= 2;
            float offset = 2f;
            RectangleF rect = new RectangleF(clientBounds.Left + offset, clientBounds.Y + offset, clientBounds.Width - offset * 2, clientBounds.Height - offset * 2);
            float x = rect.Left;
            float y = rect.Top;
            PointF origin = new PointF(x + radius, y);
            GraphicsPath lpath = new GraphicsPath();
            float opr = radius;
            lpath.AddLine(origin.X - 5, y, x + rect.Width - opr, y);
            //Creates a U shapped border, with radiused corners. Each Arc joins to the next.
            lpath.AddArc(x + rect.Width - opr, y, opr, opr, 270, 90);
            lpath.AddArc(x + rect.Width - opr, y + rect.Height - opr, opr, opr, 0, 90);
            lpath.AddArc(x, y + rect.Height - opr, opr, opr, 90, 90);
            lpath.AddArc(x, y, opr, opr, 180, 90);
            //Completes line to start of text
            lpath.AddLine(x + radius, y, x + radius + 10, y);
            return lpath;
        }

        /// <summary>Performs draw text before image</summary>
        /// <param name="g">The g</param>
        /// <param name="textImageOffset">The textImageOffset</param>
        /// <param name="rect">The rect</param>
        /// <param name="imageAlign">The imageAlign</param>
        /// <param name="image">The image</param>
        /// <param name="x">The x</param>
        /// <param name="top">The top</param>
        /// <param name="bottom">The bottom</param>
        /// <param name="middle">The middle</param>
        /// <returns>The result of the draw text before image</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when textImageOffset is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when x is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when top is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when bottom is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when middle is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Rectangle DrawTextBeforeImage(
                  Graphics g,
                  int textImageOffset,
                  Rectangle rect,
                  ContentAlignment imageAlign,
                  Image image,
                  int x,
                  int top,
                  int bottom,
                  int middle)
        {
            switch (imageAlign)
            {
                case ContentAlignment.TopLeft:
                    g.DrawImage(image, x + rect.Width - textImageOffset - image.Width, top);
                    break;
                case ContentAlignment.TopCenter:
                    g.DrawImage(image, x + rect.Width - textImageOffset - image.Width, top);
                    break;
                case ContentAlignment.TopRight:
                    g.DrawImage(image, x + rect.Width - textImageOffset - image.Width, top);
                    break;
                case ContentAlignment.MiddleLeft:
                    g.DrawImage(image, x + rect.Width - textImageOffset - image.Width, middle);
                    break;
                case ContentAlignment.MiddleCenter:
                    g.DrawImage(image, x + rect.Width - textImageOffset - image.Width, middle);
                    break;
                case ContentAlignment.MiddleRight:
                    g.DrawImage(image, x + rect.Width - textImageOffset - image.Width, middle);
                    break;
                case ContentAlignment.BottomLeft:
                    g.DrawImage(image, x + rect.Width - textImageOffset - image.Width, bottom);
                    break;
                case ContentAlignment.BottomCenter:
                    g.DrawImage(image, x + rect.Width - textImageOffset - image.Width, bottom);
                    break;
                case ContentAlignment.BottomRight:
                    g.DrawImage(image, x + rect.Width - textImageOffset - image.Width, bottom);
                    break;
            }
            return rect;
        }

        /// <summary>Performs draw text above image</summary>
        /// <param name="g">The g</param>
        /// <param name="textImageOffset">The textImageOffset</param>
        /// <param name="imageAlign">The imageAlign</param>
        /// <param name="image">The image</param>
        /// <param name="height">The height</param>
        /// <param name="y">The y</param>
        /// <param name="left">The left</param>
        /// <param name="center">The center</param>
        /// <param name="right">The right</param>
        /// <exception cref="System.ArgumentNullException">Thrown when textImageOffset is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when height is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when y is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when left is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when center is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when right is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DrawTextAboveImage(
                  Graphics g,
                  int textImageOffset,
                  ContentAlignment imageAlign,
                  Image image,
                  int height,
                  int y,
                  int left,
                  int center,
                  int right)
        {
            switch (imageAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    g.DrawImage(image, left, y + textImageOffset + height - image.Height);
                    break;
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    g.DrawImage(image, center, y + textImageOffset + height - image.Height);
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    g.DrawImage(image, right, y + textImageOffset + height - image.Height);
                    break;
            }
        }

        /// <summary>Performs draw image and text overlay</summary>
        /// <param name="g">The g</param>
        /// <param name="imageAlign">The imageAlign</param>
        /// <param name="image">The image</param>
        /// <param name="top">The top</param>
        /// <param name="bottom">The bottom</param>
        /// <param name="middle">The middle</param>
        /// <param name="left">The left</param>
        /// <param name="center">The center</param>
        /// <param name="right">The right</param>
        /// <exception cref="System.ArgumentNullException">Thrown when top is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when bottom is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when middle is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when left is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when center is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when right is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DrawImageAndTextOverlay(
                  Graphics g,
                  ContentAlignment imageAlign,
                  Image image,
                  int top,
                  int bottom,
                  int middle,
                  int left,
                  int center,
                  int right)
        {
            switch (imageAlign)
            {
                case ContentAlignment.TopLeft:
                    g.DrawImage(image, left, top);
                    break;
                case ContentAlignment.TopCenter:
                    g.DrawImage(image, center, top);
                    break;
                case ContentAlignment.TopRight:
                    g.DrawImage(image, right, top);
                    break;
                case ContentAlignment.MiddleLeft:
                    g.DrawImage(image, left, middle);
                    break;
                case ContentAlignment.MiddleCenter:
                    g.DrawImage(image, center, middle);
                    break;
                case ContentAlignment.MiddleRight:
                    g.DrawImage(image, right, middle);
                    break;
                case ContentAlignment.BottomLeft:
                    g.DrawImage(image, left, bottom);
                    break;
                case ContentAlignment.BottomCenter:
                    g.DrawImage(image, center, bottom);
                    break;
                case ContentAlignment.BottomRight:
                    g.DrawImage(image, right, bottom);
                    break;
            }
        }

        /// <summary>Performs draw image above text</summary>
        /// <param name="g">The g</param>
        /// <param name="imageAlign">The imageAlign</param>
        /// <param name="image">The image</param>
        /// <param name="top">The top</param>
        /// <param name="left">The left</param>
        /// <param name="center">The center</param>
        /// <param name="right">The right</param>
        /// <exception cref="System.ArgumentNullException">Thrown when top is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when left is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when center is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when right is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DrawImageAboveText(
                  Graphics g,
                  ContentAlignment imageAlign,
                  Image image,
                  int top,
                  int left,
                  int center,
                  int right)
        {
            switch (imageAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    g.DrawImage(image, left, top);
                    break;
                case ContentAlignment.TopCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.BottomCenter:
                    g.DrawImage(image, center, top);
                    break;
                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    g.DrawImage(image, right, top);
                    break;
            }
        }

        /// <summary>Performs draw image before text</summary>
        /// <param name="g">The g</param>
        /// <param name="imageAlign">The imageAlign</param>
        /// <param name="image">The image</param>
        /// <param name="top">The top</param>
        /// <param name="bottom">The bottom</param>
        /// <param name="middle">The middle</param>
        /// <param name="left">The left</param>
        /// <exception cref="System.ArgumentNullException">Thrown when top is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when bottom is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when middle is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when left is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void DrawImageBeforeText(
                  Graphics g,
                  ContentAlignment imageAlign,
                  Image image,
                  int top,
                  int bottom,
                  int middle,
                  int left)
        {
            switch (imageAlign)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    g.DrawImage(image, left, top);
                    break;
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    g.DrawImage(image, left, middle);
                    break;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    g.DrawImage(image, left, bottom);
                    break;
            }
        }

        /// <summary>Performs draw text</summary>
        /// <param name="g">The g</param>
        /// <param name="Bounds">The bounds</param>
        /// <param name="wrap">The wrap</param>
        /// <param name="foreColor">The foreColor</param>
        /// <param name="text">The text</param>
        /// <param name="font">The font</param>
        /// <param name="textAlign">The textAlign</param>
        /// <exception cref="System.ArgumentNullException">Thrown when wrap is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void DrawText(
                  Graphics g,
                  Rectangle Bounds,
                  bool wrap,
                  Color foreColor,
                  string text,
                  Font font,
                  ContentAlignment textAlign)
        {
            using (Brush brush = new SolidBrush(foreColor))
            {
                StringFormat stringFormat = GetStringFormat(textAlign);
                stringFormat.FormatFlags = StringFormatFlags.FitBlackBox;
                if (!wrap)
                    stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
                stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
                stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
                g.DrawString(text, font, brush, Bounds, stringFormat);
            }
        }

        /// <summary>Performs draw gradient rect figure</summary>
        /// <param name="g">The g</param>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="colorStops">The colorStops</param>
        /// <param name="colorOffsets">The colorOffsets</param>
        /// <param name="style">The style</param>
        /// <param name="angle">The angle</param>
        /// <param name="GradientOffset">The gradientOffset</param>
        /// <param name="GradientOffset2">The gradientOffset2</param>
        /// <exception cref="System.ArgumentNullException">Thrown when style is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when angle is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when GradientOffset is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when GradientOffset2 is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void DrawGradientRectFigure(
                  Graphics g,
                  Rectangle rectangle,
                  Color[] colorStops,
                  float[] colorOffsets,
                  GradientStyles style,
                  double angle,
                  double GradientOffset,
                  double GradientOffset2)
        {
            if (rectangle.Width == 0 || rectangle.Height == 0)
                return;
            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, (float)angle))
            {
                linearGradientBrush.WrapMode = WrapMode.TileFlipXY;
                ColorBlend colorBlend = new ColorBlend();
                Color[] colorArray = new Color[colorStops.Length];
                for (int index = 0; index < colorStops.Length; ++index)
                    colorArray[index] = Color.FromArgb(Math.Min(byte.MaxValue, Math.Max(0, (int)(colorStops[index].A * (double)opacity))), colorStops[index]);
                colorBlend.Colors = colorArray;
                colorBlend.Positions = colorOffsets;
                linearGradientBrush.InterpolationColors = colorBlend;
                g.FillRectangle(linearGradientBrush, rectangle);
                linearGradientBrush.Dispose();
            }
        }

        /// <summary>Performs draw gradient rect border</summary>
        /// <param name="g">The g</param>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="colorStops">The colorStops</param>
        /// <param name="colorOffsets">The colorOffsets</param>
        /// <param name="style">The style</param>
        /// <param name="angle">The angle</param>
        /// <param name="GradientOffset">The gradientOffset</param>
        /// <param name="GradientOffset2">The gradientOffset2</param>
        /// <exception cref="System.ArgumentNullException">Thrown when style is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when angle is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when GradientOffset is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when GradientOffset2 is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void DrawGradientRectBorder(
                  Graphics g,
                  Rectangle rectangle,
                  Color[] colorStops,
                  float[] colorOffsets,
                  GradientStyles style,
                  double angle,
                  double GradientOffset,
                  double GradientOffset2)
        {
            if (rectangle.Width == 0 || rectangle.Height == 0)
                return;
            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, (float)angle))
            {
                linearGradientBrush.WrapMode = WrapMode.TileFlipXY;
                ColorBlend colorBlend = new ColorBlend();
                Color[] colorArray = new Color[colorStops.Length];
                for (int index = 0; index < colorStops.Length; ++index)
                    colorArray[index] = Color.FromArgb(Math.Min(byte.MaxValue, Math.Max(0, (int)(colorStops[index].A * (double)opacity))), colorStops[index]);
                colorBlend.Colors = colorArray;
                colorBlend.Positions = colorOffsets;
                linearGradientBrush.InterpolationColors = colorBlend;
                using (Pen pen = new Pen(linearGradientBrush))
                    g.DrawRectangle(pen, rectangle);
                linearGradientBrush.Dispose();
            }
        }

        /// <summary>Gets rotated size</summary>
        /// <param name="bounds">The bounds</param>
        /// <param name="angle">The angle</param>
        /// <returns>The retrieved rotated size</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when angle is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public Size GetRotatedSize(SizeF bounds, double angle)
        {
            double width = bounds.Width;
            double height = bounds.Height;
            double num1 = angle * Math.PI / 180.0;
            while (num1 < 0.0)
                num1 += 2.0 * Math.PI;
            double num2;
            double num3;
            double num4;
            double num5;
            if (num1 >= 0.0 && num1 < Math.PI / 2.0 || num1 >= Math.PI && num1 < 3.0 * Math.PI / 2.0)
            {
                num2 = Math.Abs(Math.Cos(num1)) * width;
                num3 = Math.Abs(Math.Sin(num1)) * width;
                num4 = Math.Abs(Math.Cos(num1)) * height;
                num5 = Math.Abs(Math.Sin(num1)) * height;
            }
            else
            {
                num2 = Math.Abs(Math.Sin(num1)) * height;
                num3 = Math.Abs(Math.Cos(num1)) * height;
                num4 = Math.Abs(Math.Sin(num1)) * width;
                num5 = Math.Abs(Math.Cos(num1)) * width;
            }
            double a1 = num2 + num5;
            double a2 = num4 + num3;
            return new Size((int)Math.Ceiling(a1), (int)Math.Ceiling(a2));
        }

        /// <summary>Performs draw rotated text</summary>
        /// <param name="g">The g</param>
        /// <param name="textColor">The textColor</param>
        /// <param name="RotationAngle">The rotationAngle</param>
        /// <param name="text">The text</param>
        /// <param name="font">The font</param>
        /// <param name="textAlignment">The textAlignment</param>
        /// <param name="multiline">The multiline</param>
        /// <param name="ellipsis">The ellipsis</param>
        /// <param name="clientRectangle">The clientRectangle</param>
        /// <exception cref="System.ArgumentNullException">Thrown when RotationAngle is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when multiline is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when ellipsis is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void DrawRotatedText(
                  Graphics g,
                  Color textColor,
                  float RotationAngle,
                  string text,
                  Font font,
                  ContentAlignment textAlignment,
                  bool multiline,
                  bool ellipsis,
                  Rectangle clientRectangle)
        {
            if (string.IsNullOrEmpty(text))
                return;
            StringFormat format = new StringFormat();
            if (!multiline)
                format.FormatFlags = StringFormatFlags.NoWrap;
            if (ellipsis)
                format.Trimming = StringTrimming.EllipsisCharacter;
            SizeF sizeF = g.MeasureString(text, font);
            float width1 = sizeF.Width;
            float height1 = sizeF.Height;
            Rectangle rectangle = clientRectangle;
            double num = RotationAngle / 180.0;
            SizeF bounds = new SizeF(width1, height1);
            float width2 = GetRotatedSize(bounds, RotationAngle).Width;
            float height2 = GetRotatedSize(bounds, RotationAngle).Height;
            Bitmap bitmap1 = new Bitmap((int)width1, (int)height1);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            Graphics graphics = Graphics.FromImage(bitmap1);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            using (SolidBrush solidBrush = new SolidBrush(textColor))
                graphics.DrawString(text, font, solidBrush, rectangle, format);
            Bitmap bitmap2 = RotateBitmap(bitmap1, RotationAngle);
            PointF point = PointF.Empty;
            float x1 = 0.0f;
            float x2 = rectangle.Width / 2 - width2 / 2f;
            float x3 = rectangle.Width - width2;
            float y1 = 0.0f;
            float y2 = rectangle.Height / 2 - height2 / 2f;
            float y3 = rectangle.Height - height2;
            switch (textAlignment)
            {
                case ContentAlignment.TopLeft:
                    point = PointF.Empty;
                    break;
                case ContentAlignment.TopCenter:
                    point = new PointF(x2, y1);
                    break;
                case ContentAlignment.TopRight:
                    point = new PointF(x3, y1);
                    break;
                case ContentAlignment.MiddleLeft:
                    point = new PointF(point.X, y2);
                    break;
                case ContentAlignment.MiddleCenter:
                    point = new PointF(x2, y2);
                    break;
                case ContentAlignment.MiddleRight:
                    point = new PointF(x3, y2);
                    break;
                case ContentAlignment.BottomLeft:
                    point = new PointF(x1, y3);
                    break;
                case ContentAlignment.BottomCenter:
                    point = new PointF(x2, y3);
                    break;
                case ContentAlignment.BottomRight:
                    point = new PointF(x3, y3);
                    break;
            }
            g.DrawImage(bitmap2, point);
        }

        /// <summary>Performs rotate bitmap</summary>
        /// <param name="image">The image</param>
        /// <param name="angle">The angle</param>
        /// <returns>The result of the rotate bitmap</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when angle is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public Bitmap RotateBitmap(Image image, double angle)
        {
            double num1 = image != null ? image.Width : throw new ArgumentNullException(nameof(image));
            double height = image.Height;
            double num2 = angle * Math.PI / 180.0;
            while (num2 < 0.0)
                num2 += 2.0 * Math.PI;
            double num3;
            double num4;
            double num5;
            double num6;
            if (num2 >= 0.0 && num2 < Math.PI / 2.0 || num2 >= Math.PI && num2 < 3.0 * Math.PI / 2.0)
            {
                num3 = Math.Abs(Math.Cos(num2)) * num1;
                num4 = Math.Abs(Math.Sin(num2)) * num1;
                num5 = Math.Abs(Math.Cos(num2)) * height;
                num6 = Math.Abs(Math.Sin(num2)) * height;
            }
            else
            {
                num3 = Math.Abs(Math.Sin(num2)) * height;
                num4 = Math.Abs(Math.Cos(num2)) * height;
                num5 = Math.Abs(Math.Sin(num2)) * num1;
                num6 = Math.Abs(Math.Cos(num2)) * num1;
            }
            double num7 = (int)num3 + (int)num6;
            double num8 = (int)num5 + (int)num4;
            int num9 = (int)num7;
            int num10 = (int)num8;
            Bitmap bitmap = new Bitmap(num9, num10);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Point[] destPoints;
                if (num2 >= 0.0 && num2 < Math.PI / 2.0)
                    destPoints = new Point[3]
                    {
            new Point((int) num6, 0),
            new Point(num9, (int) num4),
            new Point(0, (int) num5)
                    };
                else if (num2 >= Math.PI / 2.0 && num2 < Math.PI)
                    destPoints = new Point[3]
                    {
            new Point(num9, (int) num4),
            new Point((int) num3, num10),
            new Point((int) num6, 0)
                    };
                else if (num2 >= Math.PI && num2 < 3.0 * Math.PI / 2.0)
                    destPoints = new Point[3]
                    {
            new Point((int) num3, num10),
            new Point(0, (int) num5),
            new Point(num9, (int) num4)
                    };
                else
                    destPoints = new Point[3]
                    {
            new Point(0, (int) num5),
            new Point((int) num6, 0),
            new Point((int) num3, num10)
                    };
                graphics.DrawImage(image, destPoints);
            }
            return bitmap;
        }

        /// <summary>Performs draw ellipse</summary>
        /// <param name="g">The g</param>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="colorStops">The colorStops</param>
        /// <param name="colorOffsets">The colorOffsets</param>
        /// <param name="style">The style</param>
        /// <param name="angle">The angle</param>
        /// <param name="GradientOffset">The gradientOffset</param>
        /// <param name="GradientOffset2">The gradientOffset2</param>
        /// <exception cref="System.ArgumentNullException">Thrown when style is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when angle is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when GradientOffset is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when GradientOffset2 is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void DrawEllipse(
                  Graphics g,
                  Rectangle rectangle,
                  Color[] colorStops,
                  float[] colorOffsets,
                  GradientStyles style,
                  double angle,
                  double GradientOffset,
                  double GradientOffset2)
        {
            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, (float)angle))
            {
                ColorBlend colorBlend = new ColorBlend();
                Color[] colorArray = new Color[colorStops.Length];
                for (int index = 0; index < colorStops.Length; ++index)
                    colorArray[index] = Color.FromArgb(Math.Min(byte.MaxValue, Math.Max(0, (int)(colorStops[index].A * (double)opacity))), colorStops[index]);
                colorBlend.Colors = colorArray;
                colorBlend.Positions = colorOffsets;
                linearGradientBrush.InterpolationColors = colorBlend;
                g.FillEllipse(linearGradientBrush, rectangle);
                linearGradientBrush.Dispose();
            }
        }

        /// <summary>Performs draw gradient path figure</summary>
        /// <param name="g">The g</param>
        /// <param name="path">The path</param>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="colorStops">The colorStops</param>
        /// <param name="colorOffsets">The colorOffsets</param>
        /// <param name="style">The style</param>
        /// <param name="angle">The angle</param>
        /// <param name="GradientOffset">The gradientOffset</param>
        /// <param name="GradientOffset2">The gradientOffset2</param>
        /// <exception cref="System.ArgumentNullException">Thrown when style is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when angle is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when GradientOffset is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when GradientOffset2 is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void DrawGradientPathFigure(
                  Graphics g,
                  GraphicsPath path,
                  Rectangle rectangle,
                  Color[] colorStops,
                  float[] colorOffsets,
                  GradientStyles style,
                  double angle,
                  double GradientOffset,
                  double GradientOffset2)
        {
            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, (float)angle))
            {
                ColorBlend colorBlend = new ColorBlend();
                Color[] colorArray = new Color[colorStops.Length];
                for (int index = 0; index < colorStops.Length; ++index)
                    colorArray[index] = Color.FromArgb(Math.Min(byte.MaxValue, Math.Max(0, (int)(colorStops[index].A * (double)opacity))), colorStops[index]);
                colorBlend.Colors = colorArray;
                if (colorOffsets.Length > colorArray.Length)
                {
                    if (colorArray.Length == 2)
                        colorOffsets = new float[2] { 0.0f, 1f };
                    else if (colorArray.Length == 3)
                        colorOffsets = new float[3] { 0.0f, 0.5f, 1f };
                }
                colorBlend.Positions = colorOffsets;
                linearGradientBrush.InterpolationColors = colorBlend;
                g.FillPath(linearGradientBrush, path);
                linearGradientBrush.Dispose();
            }
        }

        /// <summary>Performs draw gradient path figure</summary>
        /// <param name="g">The g</param>
        /// <param name="radius">The radius</param>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="colorStops">The colorStops</param>
        /// <param name="colorOffsets">The colorOffsets</param>
        /// <param name="style">The style</param>
        /// <param name="angle">The angle</param>
        /// <param name="GradientOffset">The gradientOffset</param>
        /// <param name="GradientOffset2">The gradientOffset2</param>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when style is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when angle is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when GradientOffset is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when GradientOffset2 is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void DrawGradientPathFigure(
                  Graphics g,
                  int radius,
                  Rectangle rectangle,
                  Color[] colorStops,
                  float[] colorOffsets,
                  GradientStyles style,
                  double angle,
                  double GradientOffset,
                  double GradientOffset2)
        {
            if (rectangle.Width == 0 || rectangle.Height == 0)
                return;
            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, (float)angle))
            {
                using (GraphicsPath roundedPathRect = GetRoundedPathRect(rectangle, radius))
                {
                    ColorBlend colorBlend = new ColorBlend();
                    Color[] colorArray = new Color[colorStops.Length];
                    for (int index = 0; index < colorStops.Length; ++index)
                        colorArray[index] = Color.FromArgb(Math.Min(byte.MaxValue, Math.Max(0, (int)(colorStops[index].A * (double)opacity))), colorStops[index]);
                    colorBlend.Colors = colorArray;
                    colorBlend.Positions = colorOffsets;
                    linearGradientBrush.InterpolationColors = colorBlend;
                    g.FillPath(linearGradientBrush, roundedPathRect);
                    linearGradientBrush.Dispose();
                }
            }
        }

        /// <summary>Performs draw gradient partially rounded rect figure</summary>
        /// <param name="g">The g</param>
        /// <param name="radius">The radius</param>
        /// <param name="rectangle">The rectangle</param>
        /// <param name="roundedCornerBitmask">The roundedCornerBitmask</param>
        /// <param name="colorStops">The colorStops</param>
        /// <param name="colorOffsets">The colorOffsets</param>
        /// <param name="style">The style</param>
        /// <param name="angle">The angle</param>
        /// <param name="GradientOffset">The gradientOffset</param>
        /// <param name="GradientOffset2">The gradientOffset2</param>
        /// <exception cref="System.ArgumentNullException">Thrown when radius is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when roundedCornerBitmask is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when style is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when angle is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when GradientOffset is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when GradientOffset2 is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void DrawGradientPartiallyRoundedRectFigure(
                  Graphics g,
                  int radius,
                  Rectangle rectangle,
                  byte roundedCornerBitmask,
                  Color[] colorStops,
                  float[] colorOffsets,
                  GradientStyles style,
                  double angle,
                  double GradientOffset,
                  double GradientOffset2)
        {
            if (radius == 0)
                DrawGradientRectFigure(g, rectangle, colorStops, colorOffsets, style, angle, GradientOffset, GradientOffset2);
            else if (roundedCornerBitmask == 15)
            {
                DrawGradientPathFigure(g, radius, rectangle, colorStops, colorOffsets, style, angle, GradientOffset, GradientOffset2);
            }
            else
            {
                using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangle, Color.Transparent, Color.Transparent, (float)angle))
                {
                    using (GraphicsPath partiallyRoundedPath = CreatePartiallyRoundedPath(rectangle, radius, roundedCornerBitmask))
                    {
                        ColorBlend colorBlend = new ColorBlend();
                        Color[] colorArray = new Color[colorStops.Length];
                        for (int index = 0; index < colorStops.Length; ++index)
                            colorArray[index] = Color.FromArgb(Math.Min(byte.MaxValue, Math.Max(0, (int)(colorStops[index].A * (double)opacity))), colorStops[index]);
                        colorBlend.Colors = colorArray;
                        colorBlend.Positions = colorOffsets;
                        linearGradientBrush.InterpolationColors = colorBlend;
                        g.FillPath(linearGradientBrush, partiallyRoundedPath);
                    }
                }
            }
        }

        /// <summary>Gets string format</summary>
        /// <param name="contentAlignment">The contentAlignment</param>
        /// <returns>The retrieved string format</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public StringFormat GetStringFormat(ContentAlignment contentAlignment)
        {
            StringFormat stringFormat = new StringFormat();
            switch (contentAlignment)
            {
                case ContentAlignment.TopLeft:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    stringFormat.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    stringFormat.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.TopRight:
                    stringFormat.LineAlignment = StringAlignment.Near;
                    stringFormat.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.MiddleLeft:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleCenter:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomLeft:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    stringFormat.Alignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomCenter:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    stringFormat.Alignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomRight:
                    stringFormat.LineAlignment = StringAlignment.Far;
                    stringFormat.Alignment = StringAlignment.Far;
                    break;
            }
            return stringFormat;
        }

        /// <summary>Performs draw bitmap</summary>
        /// <param name="g">The g</param>
        /// <param name="image">The image</param>
        /// <param name="x">The x</param>
        /// <param name="y">The y</param>
        /// <exception cref="System.ArgumentNullException">Thrown when x is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when y is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void DrawBitmap(Graphics g, Image image, int x, int y) => g.DrawImage(image, new Rectangle(x, y, image.Size.Width, image.Size.Height));

        /// <summary>Performs draw bitmap</summary>
        /// <param name="g">The g</param>
        /// <param name="image">The image</param>
        /// <param name="x">The x</param>
        /// <param name="y">The y</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <exception cref="System.ArgumentNullException">Thrown when x is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when y is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when width is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when height is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void DrawBitmap(Graphics g, Image image, int x, int y, int width, int height) => DrawBitmap(g, image, x, y, width, height, 1.0);

        /// <summary>Gets image color array</summary>
        /// <param name="length">The length</param>
        /// <param name="opacity">The opacity</param>
        /// <returns>The retrieved image color array</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when length is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when opacity is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private float[][] GetImageColorArray(int length, double opacity)
        {
            float[][] numArray1 = new float[length][];
            for (int index = 0; index < length; ++index)
            {
                float[] numArray2 = new float[length];
                numArray2[index] = 1f;
                numArray1[index] = numArray2;
            }
            numArray1[3][3] = (float)opacity;
            return numArray1;
        }

        /// <summary>Performs draw bitmap</summary>
        /// <param name="g">The g</param>
        /// <param name="image">The image</param>
        /// <param name="x">The x</param>
        /// <param name="y">The y</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="opacity">The opacity</param>
        /// <exception cref="System.ArgumentNullException">Thrown when x is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when y is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when width is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when height is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when opacity is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void DrawBitmap(
                  Graphics g,
                  Image image,
                  int x,
                  int y,
                  int width,
                  int height,
                  double opacity)
        {
            if (opacity == 1.0)
                g.DrawImage(image, new Rectangle(x, y, width, height));
            else
                DrawTransparentImage(g, image, x, y, width, height, opacity);
        }

        /// <summary>Performs draw transparent image</summary>
        /// <param name="g">The g</param>
        /// <param name="image">The image</param>
        /// <param name="x">The x</param>
        /// <param name="y">The y</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="opacity">The opacity</param>
        /// <exception cref="System.ArgumentNullException">Thrown when x is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when y is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when width is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when height is null</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when opacity is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void DrawTransparentImage(
                  Graphics g,
                  Image image,
                  int x,
                  int y,
                  int width,
                  int height,
                  double opacity)
        {
            ColorMatrix newColorMatrix = new ColorMatrix(GetImageColorArray(5, opacity));
            using (ImageAttributes imageAttr = new ImageAttributes())
            {
                imageAttr.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                Rectangle destRect = new Rectangle(x, y, width, height);
                g.DrawImage(image, destRect, 0, 0, image.Size.Width, image.Size.Height, GraphicsUnit.Pixel, imageAttr);
            }
        }

        /// <summary>Performs draw round gradient rectangle</summary>
        /// <param name="e">The e</param>
        /// <param name="bounds">The bounds</param>
        /// <returns>The result of the draw round gradient rectangle</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Rectangle DrawRoundGradientRectangle(MElementStyleDisplayInfo e, Rectangle bounds)
        {
            if ((!e.BorderColor.IsEmpty || !e.BorderColor2.IsEmpty) && bounds.Width > 0 && bounds.Height > 0 && e.CornerDiameter > 0)
            {

                if (e.HasRegion && e.Control != null)
                {
                    using (GraphicsPath path = e.GetRegionPath(bounds))
                    {
                        e.Control.Region = new Region(path);
                    }
                }
                bounds.Width--;
                bounds.Height--;
                using (var graphicsPath = e.GetBackgroundPath(bounds))
                {
                    using (var linGrBrush = e.GetBackgroundGradientBrush(graphicsPath.GetBounds()))
                    {
                        e.Graphics.FillPath(linGrBrush, graphicsPath);
                    }
                    if (e.BorderWidth > 0)
                    {
                        e.WidenBorderWidth(graphicsPath);
                        using (var brush = e.GetBorderGradientBrush(graphicsPath.GetBounds()))
                        {
                            /*brush.Blend = new Blend {
                                Factors = new float[] { 0f, 0.5f, 1f, 1f },
                                Positions = new float[] { 0f, 0.5f, 0.51f, 1f }
                            };*/
                            e.Graphics.FillPath(brush, graphicsPath);
                        }
                    }
                }
            }
            return bounds;
        }
    }
    /// <summary>Defines enumeration: GradientStyles</summary>

    public enum GradientStyles
    {
        Linear
    }

    /// <summary>Represents struct: ArcData</summary>

    internal struct ArcData
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public float StartAngle;
        public float SweepAngle;

        /// <summary>Initializes a new instance of the ArcData class</summary>
        /// <param name="x">The x</param>
        /// <param name="y">The y</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="startAngle">The startAngle</param>
        /// <param name="sweepAngle">The sweepAngle</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public ArcData(int x, int y, int width, int height, float startAngle, float sweepAngle)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            StartAngle = startAngle;
            SweepAngle = sweepAngle;
        }
    }
    /// <summary>Represents: MElementStyleDisplayInfo</summary>

    public class MElementStyleDisplayInfo
    {
        private Rectangle backgroundRectangle = Rectangle.Empty;
        /// <summary>Gets or sets: Control</summary>

        public Control Control { get; set; }
        /// <summary>Gets or sets: BackgroundColor1</summary>

        public Color BackgroundColor1 { get; set; }
        /// <summary>Gets or sets: BackgroundColor2</summary>

        public Color BackgroundColor2 { get; set; }
        /// <summary>Gets or sets: BorderColor</summary>

        public Color BorderColor { get; set; }
        /// <summary>Gets or sets: BorderColor2</summary>

        public Color BorderColor2 { get; set; }
        /// <summary>Gets or sets: BackgroundGradientAngle</summary>

        public int BackgroundGradientAngle
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => backgroundGradientAngle <= 0 ? BorderGradientAngle : backgroundGradientAngle;
            [MethodImpl(MethodImplOptions.NoInlining)]
            set => backgroundGradientAngle = value;
        }
        /// <summary>Gets or sets: BorderGradientAngle</summary>

        public int BorderGradientAngle { get; set; }
        /// <summary>Gets or sets: BackgroundGradientMode</summary>

        public LinearGradientMode BackgroundGradientMode { get; set; } = LinearGradientMode.Vertical;
        /// <summary>Gets or sets: BorderGradientMode</summary>

        public LinearGradientMode BorderGradientMode { get; set; } = LinearGradientMode.BackwardDiagonal;
        /// <summary>Gets or sets: CornerDiameter</summary>

        public int CornerDiameter
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return cornerDiameter;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                cornerDiameter = value;
                if (TopLeftCorner == 0)
                    TopLeftCorner = value;
                if (TopRightCorner == 0)
                    TopRightCorner = value;
                if (BottomLeftCorner == 0)
                    BottomLeftCorner = value;
                if (BottomRightCorner == 0)
                    BottomRightCorner = value;
            }
        }
        /// <summary>Gets or sets: TopLeftCorner</summary>

        public int TopLeftCorner { get; set; } = 0;
        /// <summary>Gets or sets: TopRightCorner</summary>

        public int TopRightCorner { get; set; } = 0;
        /// <summary>Gets or sets: BottomLeftCorner</summary>

        public int BottomLeftCorner { get; set; } = 0;
        /// <summary>Gets or sets: BottomRightCorner</summary>

        public int BottomRightCorner { get; set; } = 0;

        /// <summary>Gets or sets: Style</summary>

        public MElementStyle Style { get; set; }
        /// <summary>Gets or sets: Graphics</summary>

        public Graphics Graphics { get; set; }
        public Rectangle Bounds = Rectangle.Empty;
        private int cornerDiameter;
        private int backgroundGradientAngle;

        /// <summary>Gets or sets: BackgroundPathCallBack</summary>

        public Func<Rectangle, GraphicsPath> BackgroundPathCallBack { get; set; }


        /// <summary>Gets or sets: BackgroundRectangle</summary>

        public Rectangle BackgroundRectangle
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (backgroundRectangle == Rectangle.Empty && Bounds != Rectangle.Empty)
                    backgroundRectangle = Bounds;
                return backgroundRectangle;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                backgroundRectangle = value;
            }
        }
        /// <summary>Gets or sets: RightToLeft</summary>

        public bool RightToLeft { get; set; }
        /// <summary>Gets or sets: BorderWidth</summary>

        public int BorderWidth { get; set; }
        /// <summary>Gets or sets: HasRegion</summary>

        public bool HasRegion { get; set; } = false;
        /// <summary>Gets or sets: IsTwistedDoubleBorder</summary>

        public bool IsTwistedDoubleBorder { get; set; }

        /// <summary>Initializes a new instance of the MElementStyleDisplayInfo class</summary>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public MElementStyleDisplayInfo()
        {
        }


        /// <summary>Gets background path</summary>
        /// <param name="bounds">The bounds</param>
        /// <returns>The retrieved background path</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public GraphicsPath GetBackgroundPath(Rectangle bounds)
        {
            if (BackgroundPathCallBack != null)
            {
                var path = BackgroundPathCallBack.Invoke(bounds);
                if (path != null && path.PointCount > 0)
                    return path;
            }
            return CorPaintHelper.GetBackgroundPath(this.TopLeftCorner, this.TopRightCorner, this.BottomRightCorner, this.BottomLeftCorner, bounds);
        }

        /// <summary>Gets region path</summary>
        /// <param name="bounds">The bounds</param>
        /// <returns>The retrieved region path</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public GraphicsPath GetRegionPath(Rectangle bounds = default)
        {
            if (bounds == null || bounds.IsEmpty)
                bounds = new Rectangle(0, 0, this.Control.Width - 1, this.Control.Height - 1);

            return CorPaintHelper.GetBackgroundPath(this.TopLeftCorner, this.TopRightCorner, this.BottomRightCorner, this.BottomLeftCorner, bounds);
        }

        /// <summary>Gets background gradient brush</summary>
        /// <param name="bounds">The bounds</param>
        /// <returns>The retrieved background gradient brush</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual LinearGradientBrush GetBackgroundGradientBrush(RectangleF bounds)
        {
            if (BackgroundGradientAngle <= 0)
                return CreateLinearGradientBrush(bounds, BackgroundColor1, BackgroundColor2, BackgroundGradientMode);

            return CreateLinearGradientBrush(bounds, BackgroundColor1, BackgroundColor2, BackgroundGradientAngle);
        }

        /// <summary>Creates a new create linear gradient brush</summary>
        /// <param name="r">The r</param>
        /// <param name="color1">The color1</param>
        /// <param name="color2">The color2</param>
        /// <param name="gradientAngle">The gradientAngle</param>
        /// <returns>The newly created create linear gradient brush</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when gradientAngle is null</exception>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static LinearGradientBrush CreateLinearGradientBrush(RectangleF r, Color color1, Color color2, float gradientAngle)
        {
            if (r.Width <= 0)
                r.Width = 1;
            if (r.Height <= 0)
                r.Height = 1;
            return new LinearGradientBrush(new RectangleF(r.X, r.Y - 1, r.Width, r.Height + 1), color1, color2, gradientAngle);
        }


        /// <summary>Creates a new create linear gradient brush</summary>
        /// <param name="r">The r</param>
        /// <param name="color1">The color1</param>
        /// <param name="color2">The color2</param>
        /// <param name="mode">The mode</param>
        /// <returns>The newly created create linear gradient brush</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static LinearGradientBrush CreateLinearGradientBrush(RectangleF r, Color color1, Color color2, LinearGradientMode mode)
        {
            if (r.Width <= 0)
                r.Width = 1;
            if (r.Height <= 0)
                r.Height = 1;
            return new LinearGradientBrush(new RectangleF(r.X, r.Y - 1, r.Width, r.Height + 1), color1, color2, mode);
        }

        /// <summary>Gets border gradient brush</summary>
        /// <param name="bounds">The bounds</param>
        /// <returns>The retrieved border gradient brush</returns>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual LinearGradientBrush GetBorderGradientBrush(RectangleF bounds)
        {
            if (BorderGradientAngle <= 0)
                return new LinearGradientBrush(bounds, BorderColor, BorderColor2, BorderGradientMode);
            return new LinearGradientBrush(bounds, BorderColor, BorderColor2, BorderGradientAngle);
        }

        /// <summary>Performs widen border width</summary>
        /// <param name="graphicsPath">The graphicsPath</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void WidenBorderWidth(GraphicsPath graphicsPath)
        {
            var borderWidth = BorderWidth;
            if (!BorderColor.Equals(BorderColor2))
            {
                borderWidth *= 2;
            }
            using (var pen2 = new Pen(Color.Transparent, borderWidth))
            {
                graphicsPath.Widen(pen2);
            }

        }
    }
    /// <summary>Defines enumeration: CornerArc</summary>

    public enum CornerArc
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
