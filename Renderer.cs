using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SoftwareGraphicsSandbox {
    class Renderer {
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern int SetDIBitsToDevice(
            IntPtr hDC,
            int x,
            int y,
            int dx,
            int dy,
            int SrcX,
            int SrcY,
            int Scan,
            int NumScans,
            IntPtr Bits,
            ref BITMAPINFO BitsInfo,
            uint wUsage);

        public struct BITMAPINFOHEADER {
            public int biSize;
            public int biWidth;
            public int biHeight;
            public short biPlanes;
            public short biBitCount;
            public int biCompression;
            public int biSizeImage;
            public double biXPelsPerMeter;
            public double biClrUsed;
        }

        private IntPtr _hdc;
        private BITMAPINFO _bminfo32;
        private int _screenWidth;
        private int _screenHeight;
        private System.Drawing.Graphics _graphics;

        public Image BackBuffer { get; private set; }

        public DepthBuffer _DepthBuffer { get; private set; }

        public struct BITMAPINFO {
            public BITMAPINFOHEADER bmiHeader;
            public int bmiColors;
        }

        public Renderer(IntPtr windowHandle) {
            var control = Control.FromHandle(windowHandle);

            _screenWidth = control.ClientRectangle.Width;
            _screenHeight = control.ClientRectangle.Height;

            _graphics = control.CreateGraphics();
            _graphics.CompositingMode = CompositingMode.SourceCopy;
            _graphics.CompositingQuality = CompositingQuality.HighSpeed;
            _graphics.PixelOffsetMode = PixelOffsetMode.None;
            _graphics.SmoothingMode = SmoothingMode.None;

            _hdc = _graphics.GetHdc();

            _bminfo32 = new BITMAPINFO();
            _bminfo32.bmiHeader.biBitCount = (short)32;
            _bminfo32.bmiHeader.biPlanes = (short)1;
            _bminfo32.bmiHeader.biSize = 40;
            _bminfo32.bmiHeader.biWidth = _screenWidth;
            _bminfo32.bmiHeader.biHeight = -_screenHeight;
            _bminfo32.bmiHeader.biSizeImage = 4 * (_screenWidth * _screenHeight);

            BackBuffer = new Image(_screenWidth, _screenHeight);
        }



        public void Present() {
            var handle = GCHandle.Alloc(BackBuffer.Data, GCHandleType.Pinned);
            SetDIBitsToDevice(_hdc, 0, 0, _screenWidth, _screenHeight, 0, 0, 0, _screenHeight, handle.AddrOfPinnedObject(), ref _bminfo32, 0U);
            handle.Free();
        }
    }
}
