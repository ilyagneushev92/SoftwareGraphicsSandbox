using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareGraphicsSandbox {
    struct Matrix3x3 {
        public float M0;
        public float M1;
        public float M2;
        public float M3;
        public float M4;
        public float M5;
        public float M6;
        public float M7;
        public float M8;

        public static Matrix3x3 Identity {
            get {
                return new Matrix3x3(
                    1, 0, 0, 
                    0, 1, 0, 
                    0, 0, 1);
            }
        }

        // Constructor
        public Matrix3x3(float m0, float m1, float m2,
                         float m3, float m4, float m5,
                         float m6, float m7, float m8) {
            M0 = m0;
            M1 = m1;
            M2 = m2;
            M3 = m3;
            M4 = m4;
            M5 = m5;
            M6 = m6;
            M7 = m7;
            M8 = m8;

        }
        public Matrix3x3 transposed(Matrix3x3 matrix) {
            float m0 = matrix.M0;
            float m4 = matrix.M4;
            float m8 = matrix.M8;
            float swap = matrix.M1;
            float m3 = swap;
            float m1 = matrix.M3;
            swap = matrix.M2;
            float m2 = matrix.M6;
            float m6 = swap;
            swap = matrix.M5;
            float m5 = matrix.M7;
            float m7 = swap;
            return new Matrix3x3(m0, m1, m2,
                                  m3, m4, m5,
                                  m6, m7, m8);

        }

        public static Matrix3x3 rotateMatrix(float angleRadians) {
            float m0 = (float)Math.Cos(angleRadians);
            float m3 = -(float)Math.Sin(angleRadians);
            float m2 = 0;
            float m1 = (float)Math.Sin(angleRadians);
            float m4 = (float)Math.Cos(angleRadians);
            float m5 = 0;
            float m6 = 0;
            float m7 = 0;
            float m8 = 1;
            return new Matrix3x3(m0, m1, m2,
                                 m3, m4, m5,
                                 m6, m7, m8);
        }

        public static Matrix3x3 Translate(float x, float y) {
            var result = Identity;
            result.M2 = x;
            result.M5 = y;
            return result;
        }


        public static Matrix3x3 scaleMatrix(float scale) {
            float m0 = scale;
            float m1 = 0;
            float m2 = 0;
            float m3 = 0;
            float m4 = scale;
            float m5 = 0;
            float m6 = 0;
            float m7 = 0;
            float m8 = 1;
            return new Matrix3x3(m0, m1, m2,
                                 m3, m4, m5,
                                 m6, m7, m8);
        }


        public static Matrix3x3 scaleMatrix(float scaleX, float scaleY) {
            float m0 = scaleX;
            float m1 = 0;
            float m2 = 0;
            float m3 = 0;
            float m4 = scaleY;
            float m5 = 0;
            float m6 = 0;
            float m7 = 0;
            float m8 = 1;
            return new Matrix3x3(m0, m1, m2,
                                 m3, m4, m5,
                                 m6, m7, m8);
        }

        public static Matrix3x3 inverted(Matrix3x3 matrix) {
            float m11 = matrix.M0;
            float m12 = matrix.M1;
            float m13 = matrix.M2;
            float m21 = matrix.M3;
            float m22 = matrix.M4;
            float m23 = matrix.M5;
            float m31 = matrix.M6;
            float m32 = matrix.M7;
            float m33 = matrix.M8;

            var det = m11 * (m22 * m33 - m23 * m32) - m12 * (m21 * m33 - m23 * m31) + m13 * (m21 * m32 - m22 * m31);

            var a11 = m22 * m33 - m23 * m32;
            var a12 = m21 * m33 - m23 * m31; 
            var a13 = m21 * m32 - m22 * m31; 
            var a21 = m12 * m33 - m32 * m13;
            var a22 = m11 * m33 - m13 * m31; 
            var a23 = m11 * m32 - m12 * m31;
            var a31 = m12 * m23 - m22 * m13;
            var a32 = m11 * m23 - m21 * m13;
            var a33 = m11 * m22 - m12 * m21; 

            var result = new  Matrix3x3(a11 / det, -a21 / det,  a31 / det,
                                        -a12 / det, a22 / det, -a32 / det,
                                         a13 / det, -a23 / det, a33 / det);

            return result;
        }
        // Matrix3x3 multiplies Matrix3x3
        public static Matrix3x3 operator *(Matrix3x3 Left, Matrix3x3 Right) {
            float m0 = Left.M0 * Right.M0 + Left.M1 * Right.M3 + Left.M2 * Right.M6;
            float m1 = Left.M0 * Right.M1 + Left.M1 * Right.M4 + Left.M2 * Right.M7;
            float m2 = Left.M0 * Right.M2 + Left.M1 * Right.M5 + Left.M2 * Right.M8;

            float m3 = Left.M3 * Right.M0 + Left.M4 * Right.M3 + Left.M5 * Right.M6;
            float m4 = Left.M3 * Right.M1 + Left.M4 * Right.M4 + Left.M5 * Right.M7;
            float m5 = Left.M3 * Right.M2 + Left.M4 * Right.M5 + Left.M5 * Right.M8;

            float m6 = Left.M6 * Right.M0 + Left.M7 * Right.M3 + Left.M8 * Right.M6;
            float m7 = Left.M6 * Right.M1 + Left.M7 * Right.M4 + Left.M8 * Right.M7;
            float m8 = Left.M6 * Right.M2 + Left.M7 * Right.M5 + Left.M8 * Right.M8;

            return new Matrix3x3(m0, m1, m2,
                                 m3, m4, m5,
                                 m6, m7, m8);

        }
        // Matrix3x3 multiplies Point3D
        public static Point3D operator *(Matrix3x3 Matrix, Point3D Point) {
            float x = Matrix.M0 * Point.X + Matrix.M1 * Point.Y + Matrix.M2 * Point.Z;
            float y = Matrix.M3 * Point.X + Matrix.M4 * Point.Y + Matrix.M5 * Point.Z;
            float z = Matrix.M6 * Point.X + Matrix.M7 * Point.Y + Matrix.M8 * Point.Z;
            return new Point3D(x, y, z);
        }

        // Matrix3x3 multiplies Point2D
        public static Point2D operator *(Matrix3x3 Matrix, Point2D Point) {
            float x = Matrix.M0 * Point.X + Matrix.M1 * Point.Y + Matrix.M2;
            float y = Matrix.M3 * Point.X + Matrix.M4 * Point.Y + Matrix.M5;
            return new Point2D(x, y);
        }
    }
}
