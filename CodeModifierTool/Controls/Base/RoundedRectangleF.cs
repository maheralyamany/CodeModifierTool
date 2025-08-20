using System.Runtime.CompilerServices;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace OpetraViews.Views.MyContrloes
{
    /// <summary>Represents: RoundedRectangleF</summary>

    public class RoundedRectangleF
    {

        Point location;
        float radius;
        GraphicsPath grPath;
        float x, y;
        float width, height;



        /// <summary>Initializes a new instance of the RoundedRectangleF class</summary>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="radius">The radius</param>
        /// <param name="x">The x</param>
        /// <param name="y">The y</param>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public RoundedRectangleF(float width, float height, float radius, float x = 0, float y = 0)
        {

            location = new Point(0, 0);
            this.radius = radius;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            grPath = new GraphicsPath();
            if (radius <= 0)
            {
                grPath.AddRectangle(new RectangleF(x, y, width, height));
                return;
            }
            RectangleF upperLeftRect = new RectangleF(x, y, 2 * radius, 2 * radius);
            RectangleF upperRightRect = new RectangleF(width - 2 * radius - 1, x, 2 * radius, 2 * radius);
            RectangleF lowerLeftRect = new RectangleF(x, height - 2 * radius - 1, 2 * radius, 2 * radius);
            RectangleF lowerRightRect = new RectangleF(width - 2 * radius - 1, height - 2 * radius - 1, 2 * radius, 2 * radius);

            grPath.AddArc(upperLeftRect, 180, 90);
            grPath.AddArc(upperRightRect, 270, 90);
            grPath.AddArc(lowerRightRect, 0, 90);
            grPath.AddArc(lowerLeftRect, 90, 90);
            grPath.CloseAllFigures();

        }

        /// <summary>Initializes a new instance of the RoundedRectangleF class</summary>

        [MethodImpl(MethodImplOptions.NoInlining)]
        public RoundedRectangleF()
        {
        }
        /// <summary>Gets: Path</summary>

        public GraphicsPath Path
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return grPath;
            }
        }
        /// <summary>Gets: Rect</summary>

        public RectangleF Rect
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return new RectangleF(x, y, width, height);
            }
        }
        /// <summary>Gets or sets: Radius</summary>

        public float Radius
        {

            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                return radius;
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            set
            {
                radius = value;
            }
        }

    }
}
