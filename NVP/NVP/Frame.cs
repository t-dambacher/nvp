using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NVP
{
    internal struct RawFrame : IDisposable
    {
        #region Properties

        public Image Image
        {
            get; private set;
        }

        public Byte[] Pixels
        {
            get { return this._pixels; }
        }
        private Byte[] _pixels;

        public IntPtr Address
        {
            get; private set;
        }

        public GCHandle Handle
        {
            get; private set;
        }

        #endregion

        public RawFrame(Size size)
            : this()
        {
            PixelFormat format = PixelFormat.Format32bppRgb;
            Int32 pixelFormatSize = Image.GetPixelFormatSize(format);
            Int32 stride = size.Width * pixelFormatSize;
            Int32 padding = 32 - (stride % 32);
            if (padding < 32)
                stride += padding;

            this._pixels = new Byte[((stride / 32) * size.Height * 4)];
            this.Handle = GCHandle.Alloc(this.Pixels, GCHandleType.Pinned);
            this.Address = Marshal.UnsafeAddrOfPinnedArrayElement(this._pixels, 0);

            this.Image = new Bitmap(size.Width, size.Height, stride / 8, format, this.Address);
        }

        #region IDisposable

        private Boolean _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this._isDisposed)
                return;

            // disposing unmanaged resources
            this.Address = IntPtr.Zero;
            if (this.Handle.IsAllocated)
            {
                this.Handle.Free();
            }

            if (disposing)
            {
                // disposing managed resources
                this.Image.Dispose();
                this.Image = null;
                this._pixels = null;
            }

            this._isDisposed = true;
        }

        #endregion
    }

    public sealed class Frame : IDisposable, IEquatable<Frame>
    {
        private readonly Guid _id;

        public Image Image
        {
            get { return this.Raw.Image; }
        }

        internal RawFrame Raw
        {
            get; private set;
        }

        internal Frame(Size size)
        {
            this.Raw = new RawFrame(size);
            this._isDisposed = false;
            this._id = Guid.NewGuid();
        }

        public override Boolean Equals(Object obj)
        {
            return Equals(obj as Frame);
        }

        public Boolean Equals(Frame other)
        {
            return other != null && other._id.Equals(this._id);
        }

        public override Int32 GetHashCode()
        {
            return this._id.GetHashCode();
        }

        public void SetColor(Byte backcolor)
        {
            NativeMethods.MemSet(this.Raw.Address, backcolor, this.Raw.Pixels.Length);
        }

        #region IDisposable

        private Boolean _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this._isDisposed)
                return;

            // disposing managed resources
            if (disposing)
            {
                // disposing managed resources
                this.Raw.Dispose();
            }
            
            this._isDisposed = true;
        }

        #endregion
    }
}
