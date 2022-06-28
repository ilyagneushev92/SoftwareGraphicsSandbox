using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareGraphicsSandbox {
    struct Point2D {
        public float X;
        public float Y;

        public Point2D(float x, float y) {
            X = x;
            Y = y;
        }

        public static Point2D operator -(Point2D l, Point2D r) {
            return new Point2D(l.X - r.X, l.Y - r.Y);
        }


        public static Point2D operator *(Point2D l, float r) {
            return new Point2D(l.X * r, l.Y * r);
        }

        public static Point2D operator +(Point2D l, Point2D r) {
            return new Point2D(l.X + r.X, l.Y + r.Y);
        }
    }
}
