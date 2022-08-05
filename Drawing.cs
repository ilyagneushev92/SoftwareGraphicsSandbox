using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareGraphicsSandbox {
    static class Drawing {

        public static Point3D Interpolate(Point3D vertex1, Point3D vertex2, Point3D vertex3, float x, float y) {
            
            float d = (vertex2.Y - vertex3.Y)*(vertex1.X = vertex3.X) + (vertex3.X - vertex2.X)*(vertex1.Y - vertex3.Y);
            float t1 = ((vertex2.Y - vertex3.Y) * (x - vertex3.X) + (vertex3.X - vertex2.X) * (y - vertex3.Y)) / d;
            float t2 = ((vertex3.Y - vertex1.Y) * (x - vertex3.X) + (vertex1.X - vertex3.X) * (y - vertex3.Y)) / d;
            float t3 = 1 - t1 - t2;
            return new Point3D(t1, t2, t3);
        }

        static float CalculateZ(Point3D vertex1, Point3D vertex2, Point3D vertex3, float x, float y) {
            var weight = Interpolate(vertex1, vertex2, vertex3, x, y);
            var result = weight.X * vertex1.Z + weight.Y * vertex2.Z + weight.Z * vertex3.Z;
            return result;
        }


        public static void FillTriangle(DepthBuffer depth, Image image, Point3D first, Point3D second, Point3D third, Color32 color) {

            if (first.Y > second.Y) {
                var e1 = first;
                first = second;
                second = e1;
            }
            if (first.Y > third.Y) {
                var e1 = first;
                first = third;
                third = e1;
            }
            if (second.Y > third.Y) {
                var e1 = second;
                second = third;
                third = e1;
            }

            float xMid = first.X + (second.Y - first.Y) / (third.Y - first.Y) * (third.X - first.X);

            float xLeftDown = MathF.Min(xMid, second.X);
            float xRightDown = MathF.Max(xMid, second.X);
            float xLeftUp = xLeftDown;
            float xRightUp = xRightDown;

            float leftDownK = (third.X - xLeftDown) / (third.Y - second.Y);
            float rightDownK = (third.X - xRightDown) / (third.Y - second.Y);
            float rightUpK = (first.X - xRightUp) / (second.Y - first.Y);
            float leftUpK = (first.X - xLeftUp) / (second.Y - first.Y);

            float z;

            for (int y = (int)second.Y; y <= third.Y; y++) {

                xLeftDown += leftDownK;
                xRightDown += rightDownK;
                
                int xStart = (int)xLeftDown;
                int xEnd = (int)xRightDown;

                for (int xLine = xStart; xLine <= xEnd; xLine++) {
                    z = CalculateZ(first, second, third, xLine, y);
                    if (depth.Data[y * depth.Width + xLine] > z) {
                        image.SetPixel(xLine, y, color);
                        depth.SetPixel(xLine, y, z);
                    }
                }
            }

            for (int y = (int)second.Y; y >= first.Y; y--) {

                xLeftUp += leftUpK;
                xRightUp += rightUpK;

                int xStart = (int)xLeftUp;
                int xEnd = (int)xRightUp;

                for (int xLine = xStart; xLine <= xEnd; xLine++) {
                    z = CalculateZ(first, second, third, xLine, y);
                    if (depth.Data[y * depth.Width + xLine] > z) {
                        image.SetPixel(xLine, y, color);
                        depth.SetPixel(xLine, y, z);
                    }
                    }

            }
        }


        private static System.Collections.Generic.IEnumerable<int> range(int from, int to) {
            if (from <= to) {
                for (int i = from; i <= to; i++)
                    yield return i;
            } else {
                for (int i = from; i >= to; i--)
                    yield return i;
            }
        }

        public static void DrawTriangle(Image image, Point3D Point1, Point3D Point2, Point3D Point3, Color32 Color) {

            BresenhamLinePoint2D(image, Point1, Point2, Color);
            BresenhamLinePoint2D(image, Point2, Point3, Color);
            BresenhamLinePoint2D(image, Point3, Point1, Color);
        }

        public static void BresenhamLinePoint2D(Image image, Point3D Point1, Point3D Point2, Color32 color) {
            int x0 = (int)Point1.X;
            int x1 = (int)Point2.X;
            int y0 = (int)Point1.Y;
            int y1 = (int)Point2.Y;
            int z0 = (int)Point1.Z;
            int z1 = (int)Point2.Z;

            int a0;
            int a1;
            int b0;
            int b1;

            int da;
            int db;

            bool swap;

            if (Math.Abs(x1 - x0) > Math.Abs(y1 - y0)) {
                a0 = x0;
                a1 = x1;
                da = x1 - x0;
                b0 = y0;
                b1 = y1;
                db = y1 - y0;
                swap = false;
            } else {
                a0 = y0; 
                a1 = y1;
                da = y1 - y0;
                b0 = x0;
                b1 = x1;
                db = x1 - x0;
                swap = true;
            }


            double k = Math.Abs((double)db / da);

            int signB = b0 < b1 ? 1 : -1;

            double error = 0;

            int b = b0;

            foreach (int a in range(a0, a1)) {
                if (!swap) {
                    image.SetPixel(a, b, color);
                }
                else
                    image.SetPixel(b, a, color);

                error += k;

                if (error >= 1) {
                    b += signB;
                    error -= 1;
                }
            }
        }
    }
}
