using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareGraphicsSandbox {
    static class Drawing {

        public static void DrawCircle(Matrix3x3 Matrix, int verticesAmount, Image image, Color32 color) {
            float step = 2 * MathF.PI / verticesAmount;
            // constStep is for second point of drawing line

            var sin = MathF.Sin(step);
            var cos = MathF.Cos(step);
            var prev = new Point2D(1, 0);

            for (int i = 1; i <= verticesAmount + 1; i++) {
                var cur = new Point2D(cos*prev.X - sin*prev.Y, sin*prev.X + cos*prev.Y);
                BresenhamLinePoint2D(image, Matrix * prev, Matrix * cur, color);
                prev = cur;
            }
        }
        public static void setPixel2(Image image, int x, int y, Color32 color) {
            x = x + image.Width / 2;
            y = -y + image.Height / 2;
            image.SetPixel(x, y, color);
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

        public static void DrawTriangle(Image image, Point2D Point1, Point2D Point2, Point2D Point3, Color32 Color) {
            BresenhamLinePoint2D(image, Point1, Point2, Color);
            BresenhamLinePoint2D(image, Point2, Point3, Color);
            BresenhamLinePoint2D(image, Point3, Point1, Color);
        }

        public static void BresenhamLinePoint2D(Image image, Point2D Point1, Point2D Point2, Color32 color) {
            int x0 = (int)Point1.X;
            int x1 = (int)Point2.X;
            int y0 = (int)Point1.Y;
            int y1 = (int)Point2.Y;

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
                if (!swap)
                    // setPixel or setPixel2 ???
                    setPixel2(image, a, b, color);
                else
                    setPixel2(image, b, a, color);

                error += k;

                if (error >= 1) {
                    b += signB;
                    error -= 1;
                }
            }
        }
    }
}
