using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareGraphicsSandbox { 
    class DepthBuffer {
            public float[] Data { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }

            public DepthBuffer(int width, int height) {
                Width = width;
                Height = height;
                Data = new float[width * height];
            }


            public void FillColor(float depth) {
                var buffer = Data;
                for (int i = 0; i < buffer.Length; ++i) {
                    buffer[i] = depth;
                }
            }

            public bool SetPixel(int x, int y, float depth) {
                if (x < 0 || x >= Width) {
                    return false;
                }
                if (y < 0 || y >= Height) {
                    return false;
                }

                Data[y * Width + x] = depth;
                return true;
            }
        }
    
}
