using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareGraphicsSandbox {
    struct Point3D {
        public float X;
        public float Y;
        public float Z;



        public Point3D(Point4D p) {
            X = p.X/p.W;
            Y = p.Y / p.W;
            Z = p.Z / p.W;
        }
        public static Point3D Right {
            get {
                return new Point3D(1, 0, 0);
            }
        }

        public static Point3D Up {
            get {
                return new Point3D(0, 1, 0);
            }
        }

        public static Point3D Forward {
            get {
                return new Point3D(0, 0, 1);
            }
        }

        public static Point3D Zero {
            get {
                return new Point3D(0, 0, 0);
            }
        }
        public static Point3D One {
            get {
                return new Point3D(1, 1, 1);
            }
        }

        public float SquareLength {
            get {
                return X * X + Y * Y + Z * Z;    
            }
        }

        public float Length {
            get {
                return MathF.Sqrt(SquareLength);
            }
        }


        public Point3D Normalized {
            get {
                return this / Length;
            }
        }

        public static float Dot(Point3D l, Point3D r) {
            return l.X * r.X + l.Y * r.Y + l.Z * r.Z;
        }

        public static Point3D Cross(Point3D l, Point3D r) {
            return new Point3D(l.Y * r.Z - l.Z * r.Y,
                               l.Z * r.X - l.X * r.Z,
                               l.X * r.Y - l.Y * r.X);
        }

        public static Point3D operator /(Point3D l, float r) {
            return new Point3D(l.X / r, l.Y / r, l.Z / r);
        }

        public static Point3D operator *(Point3D l, float r) {
            return new Point3D(l.X * r, l.Y * r, l.Z * r);
        }

        public static Point3D operator -(Point3D l, Point3D r) {
            return new Point3D(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
        }

        public static Point3D operator +(Point3D l, Point3D r) {
            return new Point3D(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
        }

        public Point3D(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
