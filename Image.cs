using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SoftwareGraphicsSandbox {

    [StructLayout(LayoutKind.Sequential)]
    struct Color32 {
        public byte b;
        public byte g;
        public byte r;
        public byte a;

        public static Color32 Swamp {
            get { return new Color32 { a = 0xff, r = 125, g = 125, b = 0 }; }
        }
        public static Color32 Red {
            get { return new Color32 { a = 0xff, r = 0xff, g = 0, b = 0 }; }
        }
        public static Color32 Green {
            get { return new Color32 { a = 0xff, r = 0, g = 0xff, b = 0 }; }
        }
        public static Color32 Blue {
            get { return new Color32 { a = 0xff, r = 0, g = 0, b = 0xff }; }
        }
        public static Color32 Black {
            get { return new Color32 { a = 0xff, r = 0, g = 0, b = 0 }; }
        }
        public static Color32 White {
            get { return new Color32 { a = 0xff, r = 0xff, g = 0xff, b = 0xff }; }
        }
    }

    class Image {
        public Color32[] Data { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Image(int width, int height) {
            Width = width;
            Height = height;
            Data = new Color32[width * height];
        }


        public void FillColor(Color32 color) {
            var buffer = Data;
            for(int i = 0; i < buffer.Length; ++i) {
                buffer[i] = color;
            }
        }

        public bool SetPixel(int x, int y, Color32 color) {
            if(x < 0 || x >= Width) {
                return false;
            }
            if (y < 0 || y >= Height) {
                return false;
            }

            Data[y * Width + x] = color;
            return true;
        }
    }
}
