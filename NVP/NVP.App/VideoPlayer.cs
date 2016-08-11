using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace NVP.App
{
    /// <summary>
    /// Main form
    /// </summary>
    public partial class VideoPlayer : Form
    {
        /// <summary>
        /// Watch used to refresh periodicaly the screen
        /// </summary>
        private readonly Watch _watch;

        /// <summary>
        /// The player associated with the current form
        /// </summary>
        private readonly VideoDecoder _player;

        /// <summary>
        /// Creates a new instance of the application's main form
        /// </summary>
        /// <param name="player">The player containing the video to dislay in the form</param>
        public VideoPlayer(VideoDecoder player)
        {
            InitializeComponent();

            this._player = player;
            Cursor.Hide();

            this._watch = new Watch(player.Metadata.FrameRate, this, Watch_Tick);
        }

        /// <summary>
        /// EventHandler called on each timer's tick
        /// </summary>
        private void Watch_Tick(Object sender, TickEventArgs e)
        {
            this.lblFps.Text = String.Format("{0} fps", e.ActualFrameRate);
            this.lblFps.ForeColor = e.ActualFrameRate >= e.RequestedFrameRate ? Color.Green : Color.Red;

            this._player.Enumerator.MoveNext();
            this.Invalidate();
        }

        /// <summary>
        /// Painting
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Frame currentFrame = this._player.Enumerator.Current;
            if (currentFrame != null)
            {
                Point startingPosition = GetStartingPosition(e.Graphics.VisibleClipBounds.Size, currentFrame.Image.Size);
                e.Graphics.DrawImage(currentFrame.Image, startingPosition.X, startingPosition.Y);
            }
        }

        /// <summary>
        /// Returns the starting position to use to draw the image
        /// </summary>
        /// <param name="screenSize">Screen size</param>
        /// <param name="imageSize">Image size</param>
        /// <returns>Starting position</returns>
        private Point GetStartingPosition(SizeF screenSize, Size imageSize)
        {
            Int32 x = (Int32)(screenSize.Width - imageSize.Width) / 2;
            Int32 y = (Int32)(screenSize.Height - imageSize.Height) / 2;
            return new Point(x, y);
        }
    }
}
