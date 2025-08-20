using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

using DevComponents.DotNetBar;

using OpetraViews.Views.MyContrloes;

namespace OpetraViews.Controls {
	/// <summary>Represents: MProgressBar</summary>

	public class MProgressBar : ProgressBar {
		private Timer animationTimer;
		private int displayedValue;
		private int animationTarget;
		private const int animationSpeed = 10;
		private Color mergedForeColor = Color.Empty;
		private bool useCheckOnComplete = true;
		private int cornerRadius = 8;
		private Color textColor = Color.Black;
		private Color foreColor1 = Color.DeepSkyBlue;
		private Color foreColor2 = Color.FromArgb(153, 153, 255);
		private Color borderColor = Color.Black;
		private TextFormatFlags FormatFlags = TextFormatFlags.Default | TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.WordBreak;
		private bool useRoundedCorners = true;

		private ProgressTextDisplayMode displayMode = ProgressTextDisplayMode.Percentage;
		private int borderWidth = 1;
		private Color backColor1 = Color.LightGray;


		/// <summary>Initializes a new instance of the MProgressBar class</summary>

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MProgressBar() {
			this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Transparent;

			this.TextColor = Color.Black;

			displayedValue = base.Value;
			animationTarget = base.Value;
		}
		/// <summary>Gets: DefaultMinimumSize</summary>

		protected override Size DefaultMinimumSize => new Size(0, 12);
		/// <summary>Gets or sets: MinimumSize</summary>

		[DefaultValue(typeof(Size), "0,12")]
		public override Size MinimumSize {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.MinimumSize;
			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (value.Height < 12)
					value.Height = 12;
				base.MinimumSize = value;
			}
		}
		/// <summary>Gets or sets: MarqueeAnimationSpeed</summary>

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int MarqueeAnimationSpeed {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.MarqueeAnimationSpeed;
			[MethodImpl(MethodImplOptions.NoInlining)]
			set => base.MarqueeAnimationSpeed = value;
		}
		/// <summary>Gets or sets: Font</summary>

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Font Font {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return base.Font;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				base.Font = value;
			}
		}
		/// <summary>Gets or sets: ForeColor</summary>

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color ForeColor {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.ForeColor;
			[MethodImpl(MethodImplOptions.NoInlining)]
			set => base.ForeColor = value;
		}
		/// <summary>Gets or sets: BackColor</summary>

		[DefaultValue(typeof(Color), "Transparent")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color BackColor {
			[MethodImpl(MethodImplOptions.NoInlining)]
			get => base.BackColor;
			[MethodImpl(MethodImplOptions.NoInlining)]
			set => base.BackColor = value;
		}
		/// <summary>Gets or sets: BackColor1</summary>

		[DefaultValue(typeof(Color), "LightGray")]
		[Category("Appearance")]
		public Color BackColor1 {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return backColor1;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (backColor1 == value)
					return;
				backColor1 = value;
				InternalInvalidate();
			}
		}



		/// <summary>Gets or sets: BorderColor</summary>

		[Category("Appearance")]
		[DefaultValue(typeof(Color), "Black")]

		public virtual Color BorderColor {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return borderColor;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (borderColor == value)
					return;
				borderColor = value;
				InternalInvalidate();
			}
		}
		/// <summary>Gets or sets: BorderWidth</summary>

		[Category("Appearance")]
		[DefaultValue(1)]
		public int BorderWidth {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get { return borderWidth; }

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (borderWidth == value)
					return;
				borderWidth = value;
				InternalInvalidate();
			}
		}
		/// <summary>Gets or sets: ForeColor1</summary>

		[Category("Appearance")]
		[DefaultValue(typeof(Color), "DeepSkyBlue")]
		public Color ForeColor1 {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return foreColor1;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (foreColor1 == value)
					return;
				foreColor1 = value;
				RMergedForeColor();
				InternalInvalidate();
			}
		}
		/// <summary>Gets or sets: ForeColor2</summary>

		[Category("Appearance")]
		[DefaultValue(typeof(Color), "153,153,255")]
		public Color ForeColor2 {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				return foreColor2;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (foreColor2 == value)
					return;
				foreColor2 = value;

				RMergedForeColor();
				InternalInvalidate();
			}
		}


		/// <summary>Gets or sets: CornerRadius</summary>

		[Category("Appearance")]
		[DefaultValue(8)]
		public int CornerRadius {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get {
				if (cornerRadius == 0)
					return 8;
				return cornerRadius;
			}

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (cornerRadius == value) {
					return;
				}
				validRadius = null;
				cornerRadius = value;
				InternalInvalidate();
			}
		}

		/// <summary>Gets or sets: DisplayMode</summary>

		[Category("Appearance")]
		[DefaultValue(ProgressTextDisplayMode.Percentage)]

		public ProgressTextDisplayMode DisplayMode {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get { return displayMode; }

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (displayMode.Equals(value))
					return;
				displayMode = value;
				InternalInvalidate();
			}
		}

		/// <summary>Gets or sets: UseRoundedCorners</summary>

		[Category("Appearance")]
		[DefaultValue(true)]

		public bool UseRoundedCorners {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get { return useRoundedCorners; }

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (useRoundedCorners == value)
					return;
				useRoundedCorners = value;
				InternalInvalidate();
			}
		}
		/// <summary>Gets or sets: UseCheckOnComplete</summary>

		[Category("Appearance")]
		[DefaultValue(true)]

		public bool UseCheckOnComplete {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get { return useCheckOnComplete; }

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (useCheckOnComplete == value)
					return;
				useCheckOnComplete = value;
				InternalInvalidate();
			}
		}
		/// <summary>Gets or sets: TextColor</summary>

		[Category("Appearance")]
		[DefaultValue(typeof(Color), "Black")]

		public Color TextColor {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get { return textColor; }

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (textColor.Equals(value))
					return;
				textColor = value;
				InternalInvalidate();
			}
		}
		private int value = 0;
		/// <summary>Gets or sets: Value</summary>

		[DefaultValue(0)]
		public new int Value {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get => this.value;

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (this.value == value)
					return;
				SetValue(value);

			}
		}




		/// <summary>Performs r merged fore color</summary>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void RMergedForeColor() {
			mergedForeColor = Color.Empty;
			if (this.IsHandleCreated)
				this.ForeColor = GetMergedForeColor();
		}

		/// <summary>Gets merged fore color</summary>
		/// <returns>The retrieved merged fore color</returns>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Color GetMergedForeColor() {
			if (mergedForeColor.IsEmpty)
				mergedForeColor = ColorScheme.MergeColors(foreColor1, foreColor2);
			return mergedForeColor;
		}

		/// <summary>Performs internal invalidate</summary>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void InternalInvalidate() {
			this.Invalidate();
		}

		/// <summary>Performs on paint</summary>
		/// <param name="e">The e</param>

		[MethodImpl(MethodImplOptions.NoInlining)]
		protected override void OnPaint(PaintEventArgs e) {
			Graphics g = e.Graphics;

			var anon = new {
				g.SmoothingMode,
				g.CompositingQuality,
				g.PixelOffsetMode,
				g.InterpolationMode
			};
			void ReSetGraphicsMode() {
				g.SmoothingMode = anon.SmoothingMode;
				g.CompositingQuality = anon.CompositingQuality;
				g.PixelOffsetMode = anon.PixelOffsetMode;
				g.InterpolationMode = anon.InterpolationMode;
			}
			void SetGraphicsMode() {
				g.SmoothingMode = SmoothingMode.HighQuality;
				//g.CompositingQuality = CompositingQuality.HighQuality;
				/*g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;*/
			}
			try {
				Rectangle rect = this.ClientRectangle;

				var bounds = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);

				using (Brush backBrush = new SolidBrush(Color.Transparent))
					g.FillRectangle(backBrush, rect);

				SetGraphicsMode();





				//DrawRectangle DrawProgress
				if (UseRoundedCorners) {
					/* bounds = CorPaintHelper.BorderRectangle(rect, borderWidth);
                     using (var path = GetRoundedRect(bounds)) {
                         using (Brush backBrush = new SolidBrush(this.backColor1))
                             g.FillPath(backBrush, path);
                         using (Pen pen = new Pen(borderColor, borderWidth)) {
                             g.DrawPath(pen, path);
                         }
                     }*/
					bounds = CorPaintHelper.PaintBorder(GetElementStyleDisplayInfo(rect, g));
				} else {
					using (Brush backBrush = new SolidBrush(this.backColor1)) {
						g.FillRectangle(backBrush, bounds);
					}
					using (Pen pen = new Pen(borderColor, borderWidth))
						g.DrawRectangle(pen, bounds);
					bounds.Inflate(-1, -1);
				}

				if (displayedValue > 0) {

					DrawProgress(bounds, g);
				}
				ReSetGraphicsMode();


			} catch (Exception ex) {
				ReSetGraphicsMode();

				base.OnPaint(e);
			}
		}


		/// <summary>Performs draw progress</summary>
		/// <param name="bounds">The bounds</param>
		/// <param name="g">The g</param>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void DrawProgress(Rectangle bounds, Graphics g) {

			float percent = (float)displayedValue / this.Maximum;
			Rectangle progressRect = new Rectangle(bounds.X, bounds.Y, (int)(bounds.Width * percent), bounds.Height);
			if (progressRect.Width > 0 && progressRect.Height > 0) {
				// Progress fill

				if (UseRoundedCorners) {

					using (GraphicsPath path = GetRoundedRect(progressRect)) {
						using (LinearGradientBrush brush = new LinearGradientBrush(path.GetBounds(), this.foreColor1, this.foreColor2, LinearGradientMode.Horizontal)) {
							g.FillPath(brush, path);
						}
					}
				} else {
					using (LinearGradientBrush brush = new LinearGradientBrush(progressRect, this.foreColor1, this.foreColor2, LinearGradientMode.Horizontal)) {
						g.FillRectangle(brush, progressRect);
					}
				}



				// Draw check or text
				if (displayedValue == this.Maximum && useCheckOnComplete) {
					DrawCheckMark(g, bounds);
				} else {
					string text = DisplayMode == ProgressTextDisplayMode.Percentage
						? $"%{(int)(percent * 100)}"
						: $"{displayedValue} / {this.Maximum}";

					TextRenderer.DrawText(g, text, this.GetTextFont(bounds.Height), bounds, TextColor, FormatFlags);
				}
			}
		}

		/// <summary>Gets rounded rect</summary>
		/// <param name="rect">The rect</param>
		/// <returns>The retrieved rounded rect</returns>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private GraphicsPath GetRoundedRect(Rectangle rect) {
			var radius = GetValidRadius();
			int TopLeftCorner = radius;
			int BottomLeftCorner = radius;
			int TopRightCorner = radius;
			int BottomRightCorner = radius;
			var percent = ((double)rect.Width / this.Width) * 100;
			if (percent < 98) {
				BottomRightCorner = TopRightCorner = 0;
			}
			return CorPaintHelper.GetBackgroundPath(TopLeftCorner, TopRightCorner, BottomRightCorner, BottomLeftCorner, rect);
		}
		private int? validRadius;

		/// <summary>Gets valid radius</summary>
		/// <returns>The retrieved valid radius</returns>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private int GetValidRadius() {
			if (!validRadius.HasValue) {
				int radius = CornerRadius;
				if (radius > 0) {
					int height = this.Height - 2;
					int heR = height - radius;
					if (radius > heR && height <= 18) {
						//m_CornerDiameter = heR;
						radius = heR;
					}
				}
				validRadius = radius;
			}
			return validRadius.Value;
		}

		/// <summary>Performs on resize</summary>
		/// <param name="e">The e</param>

		[MethodImpl(MethodImplOptions.NoInlining)]
		protected override void OnResize(EventArgs e) {
			validRadius = null;
			base.OnResize(e);

		}

		/// <summary>Performs on back color changed</summary>
		/// <param name="e">The e</param>

		[MethodImpl(MethodImplOptions.NoInlining)]
		protected override void OnBackColorChanged(EventArgs e) {
			base.OnBackColorChanged(e);
			if (!base.BackColor.Equals(Color.Transparent))
				base.BackColor = Color.Transparent;
		}

		/// <summary>Gets element style display info</summary>
		/// <param name="clientBounds">The clientBounds</param>
		/// <param name="g">The g</param>
		/// <returns>The retrieved element style display info</returns>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private MElementStyleDisplayInfo GetElementStyleDisplayInfo(Rectangle clientBounds, Graphics g) {
			return new MElementStyleDisplayInfo {
				BackgroundColor1 = this.backColor1,
				BackgroundColor2 = this.backColor1,
				BorderColor = borderColor,
				BorderColor2 = borderColor,
				BorderWidth = this.BorderWidth,
				Bounds = clientBounds,
				CornerDiameter = GetValidRadius(),
				Graphics = g,
				Style = new MElementStyle(),
			};
		}



		/// <summary>Gets client rectangle</summary>
		/// <returns>The retrieved client rectangle</returns>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Rectangle GetClientRectangle() {
			Rectangle rect = this.ClientRectangle;
			if ((rect.Width == 0 && this.Width == 0) || (rect.Height == 0 && this.Height == 0)) {
				return Rectangle.Empty;
			}
			if (rect.Width == 0)
				rect.Width = this.Width;
			if (rect.Height == 0)
				rect.Height = this.Height;
			return rect;
		}

		/// <summary>Sets value</summary>
		/// <param name="value">The value</param>
		/// <exception cref="System.ArgumentNullException">Thrown when value is null</exception>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void SetValue(int value) {
			if (value < this.Minimum) value = this.Minimum;
			if (value > this.Maximum) value = this.Maximum;
			this.value = value; // internal logic

			if (this.value == 0) {
				displayedValue = this.value;
				this.Invalidate();
			} else {
				animationTarget = value;
				if (!GetAnimationTimer().Enabled)
					GetAnimationTimer().Start();
			}



		}

		/// <summary>Gets animation timer</summary>
		/// <returns>The retrieved animation timer</returns>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Timer GetAnimationTimer() {
			if (animationTimer == null) {
				animationTimer = new Timer {
					Interval = 15,
					Enabled = false
				};
				animationTimer.Tick += AnimateProgress;
			}
			return animationTimer;
		}

		/// <summary>Performs animate progress</summary>
		/// <param name="sender">The sender</param>
		/// <param name="e">The e</param>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void AnimateProgress(object sender, EventArgs e) {
			if (displayedValue == animationTarget) {
				animationTimer?.Stop();
				return;
			}
			int diff = animationTarget - displayedValue;
			int step = Math.Sign(diff) * Math.Max(1, Math.Abs(diff) / animationSpeed);
			displayedValue += step;
			if (Math.Abs(displayedValue - animationTarget) < 2)
				displayedValue = animationTarget;
			this.Invalidate();
		}
		private float checkMarkScale = 0.9F;
		/// <summary>Gets or sets: CheckMarkScale</summary>

		[DefaultValue(0.9F)]
		public float CheckMarkScale {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get { return checkMarkScale; }

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (checkMarkScale == value)
					return;
				checkMarkScale = value;
				if (displayedValue == this.Maximum && useCheckOnComplete)
					InternalInvalidate();
			}
		}
		private Color checkMarkColor = Color.White;
		/// <summary>Gets or sets: CheckMarkColor</summary>

		[DefaultValue(typeof(Color), "White")]
		public Color CheckMarkColor {

			[MethodImpl(MethodImplOptions.NoInlining)]
			get { return checkMarkColor; }

			[MethodImpl(MethodImplOptions.NoInlining)]
			set {
				if (checkMarkColor == value)
					return;
				checkMarkColor = value;
				if (displayedValue == this.Maximum && useCheckOnComplete)
					InternalInvalidate();
			}
		}

		/// <summary>Performs draw check mark</summary>
		/// <param name="g">The g</param>
		/// <param name="rect">The rect</param>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void DrawCheckMark(Graphics g, Rectangle rect) {
			using (GraphicsPath path = CreateCheckMarkPath(rect))
				DisplayHelp.FillPath(g, path, checkMarkColor, checkMarkColor);
		}





		/// <summary>Gets valid check mark font size</summary>
		/// <param name="Height">The height</param>
		/// <param name="scale">The scale</param>
		/// <returns>The retrieved valid check mark font size</returns>
		/// <exception cref="System.ArgumentNullException">Thrown when Height is null</exception>
		/// <exception cref="System.ArgumentNullException">Thrown when scale is null</exception>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private float GetValidCheckMarkFontSize(int Height, float scale) {
			if (scale == 0)
				scale = 0.1F;

			var fontSize = Height * scale;
			if (fontSize <= 0)
				fontSize = 12F;
			if (fontSize > 100)
				fontSize = 100F;
			return fontSize;
		}

		/// <summary>Creates a new create check mark path</summary>
		/// <param name="outterRect">The outterRect</param>
		/// <returns>The newly created create check mark path</returns>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private GraphicsPath CreateCheckMarkPath(Rectangle outterRect) {
			Rectangle bounds = outterRect;
			bounds.Inflate(-1, 1);
			bounds.Y -= 1;


			var fontSize = GetValidCheckMarkFontSize(outterRect.Height, checkMarkScale);

			GraphicsPath path = new GraphicsPath();
			string checkmark = "✓";
			StringFormat format = GetCheckMarkPathFormat();
			using (Font font = FontExtensions.GetSymbolFont(fontSize)) {
				path.AddString(
					checkmark,
					font.FontFamily,
					(int)font.Style,
					fontSize,
					bounds,
					format
				);
			}
			return path;
		}

		private Font textFont;
		private int oldHeight = 0;

		/// <summary>Gets text font</summary>
		/// <param name="Height">The height</param>
		/// <returns>The retrieved text font</returns>
		/// <exception cref="System.ArgumentNullException">Thrown when Height is null</exception>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Font GetTextFont(int Height) {
			if (textFont == null || oldHeight != Height) {
				oldHeight = Height;
				float fontSize = GetValidCheckMarkFontSize(Height, 0.7F);
				textFont = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
			}
			return textFont;
		}

		private StringFormat checkMarkFormat;



		/// <summary>Gets check mark path format</summary>
		/// <returns>The retrieved check mark path format</returns>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private StringFormat GetCheckMarkPathFormat() {
			if (checkMarkFormat == null) {
				checkMarkFormat = StringFormat.GenericDefault;
				checkMarkFormat.Alignment = StringAlignment.Center;
				checkMarkFormat.LineAlignment = StringAlignment.Center;
				checkMarkFormat.FormatFlags = StringFormatFlags.NoWrap;
				checkMarkFormat.Trimming = StringTrimming.None;
				checkMarkFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

			}
			return checkMarkFormat;
		}


		/// <summary>Gets center position</summary>
		/// <param name="rect">The rect</param>
		/// <param name="size">The size</param>
		/// <returns>The retrieved center position</returns>

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Point GetCenterPosition(Rectangle rect, SizeF size) {
			if (size.Height >= rect.Height)
				size.Height -= 2;
			return new Point(
								(int)((rect.Width - size.Width) / 2),
								(int)((rect.Height - size.Height) / 2));
		}



	}
	/// <summary>Defines enumeration: ProgressTextDisplayMode</summary>

	public enum ProgressTextDisplayMode {
		ValueOverMaximum,
		Percentage
	}
}
