using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareGraphicsSandbox {
    struct Matrix2x2 {
        // row major matrix

        public float M0;
        public float M1;
        public float M2;
        public float M3;

        public static Matrix2x2 Identity {
            get {
                return new Matrix2x2(1, 0, 0, 1);
            }
        }
        public Matrix2x2(float m0, float m1, float m2, float m3) {
            M0 = m0;
            M1 = m1;
            M2 = m2;
            M3 = m3;
        }

        public void SetZero() {
            M0 = 0;
            M1 = 0;
            M2 = 0;
            M3 = 0;
        }

        public static Matrix2x2 Transposed(Matrix2x2 matrix) {
            float swap = matrix.M1;
            float m0 = matrix.M0;
            float m3 = matrix.M3;
            float m1 = matrix.M2;
            float m2 = swap;
            return new Matrix2x2(m0, m1, m2, m3);

        }

        public static Matrix2x2 RotateMatrix(float angleRadians) {
            float m0 = (float)Math.Cos(angleRadians);
            float m2 = -(float)Math.Sin(angleRadians);
            float m1 = (float)Math.Sin(angleRadians);
            float m3 = (float)Math.Cos(angleRadians);
            return new Matrix2x2(m0, m1, m2, m3);
        }

        public static Matrix2x2 ScaleMatrix(float scale) {
            float m0 = scale;
            float m1 = 0;
            float m2 = 0;
            float m3 = scale;
            return new Matrix2x2(m0, m1, m2, m3);
        }

        public static Matrix2x2 ScaleMatrix(float scaleX, float scaleY) {
            float m0 = scaleX;
            float m1 = 0;
            float m2 = 0;
            float m3 = scaleY;
            return new Matrix2x2(m0, m1, m2, m3);
        }

        public static Matrix2x2 operator *(Matrix2x2 Left, Matrix2x2 Right) {
            float m0 = Left.M0 * Right.M0 + Left.M1 * Right.M2;
            float m1 = Left.M0 * Right.M1 + Left.M1 * Right.M3;
            float m2 = Left.M2 * Right.M0 + Left.M3 * Right.M2;
            float m3 = Left.M2 * Right.M1 + Left.M3 * Right.M3;
            return new Matrix2x2(m0, m1, m2, m3);
        }

        public static Point2D operator *(Point2D Point, Matrix2x2 Matrix) {
            float x = Point.X * Matrix.M0 + Point.Y * Matrix.M1;
            float y = Point.X * Matrix.M2 + Point.Y * Matrix.M3;
            return new Point2D(x, y);

        }

    }
}
