using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareGraphicsSandbox {
    struct Matrix4x4 {
        public float M0;
        public float M1;
        public float M2;
        public float M3;
        public float M4;
        public float M5;
        public float M6;
        public float M7;
        public float M8;
        public float M9;
        public float M10;
        public float M11;
        public float M12;
        public float M13;
        public float M14;
        public float M15;

        // Constructor
        public Matrix4x4(float m0, float m1, float m2, float m3,
                         float m4, float m5, float m6, float m7,
                         float m8, float m9, float m10, float m11,
                         float m12, float m13, float m14, float m15) {
            M0 = m0;
            M1 = m1;
            M2 = m2;
            M3 = m3;
            M4 = m4;
            M5 = m5;
            M6 = m6;
            M7 = m7;
            M8 = m8;
            M9 = m9;
            M10 = m10;
            M11 = m11;
            M12 = m12;
            M13 = m13;
            M14 = m14;
            M15 = m15;
        }

        public static Matrix4x4 Identity {
            get {
                return new Matrix4x4(
                    1, 0, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1);
            }
        }

        public static Matrix4x4 transposed(Matrix4x4 matrix) {
            float m0 = matrix.M0;
            float m1 = matrix.M4;
            float m2 = matrix.M8;
            float m3 = matrix.M12;
            float m4 = matrix.M1;
            float m5 = matrix.M5;
            float m6 = matrix.M9;
            float m7 = matrix.M13;
            float m8 = matrix.M2;
            float m9 = matrix.M6;
            float m10 = matrix.M10;
            float m11 = matrix.M14;
            float m12 = matrix.M3;
            float m13 = matrix.M7;
            float m14 = matrix.M11;
            float m15 = matrix.M15;
            return new Matrix4x4(m0, m1, m2, m3,
                                 m4, m5, m6, m7,
                                 m8, m9, m10, m11,
                                 m12, m13, m14, m15);
        }

        public static Matrix4x4 rotateMatrix(float angleRadians) {
            float m0 = MathF.Cos(angleRadians);
            float m1 = MathF.Sin(angleRadians);
            float m2 = 0;
            float m3 = 0;
            float m4 = -MathF.Sin(angleRadians);
            float m5 = MathF.Cos(angleRadians);
            float m6 = 0;
            float m7 = 0;
            float m8 = 0;
            float m9 = 0;
            float m10 = 1;
            float m11 = 0;
            float m12 = 0;
            float m13 = 0;
            float m14 = 0;
            float m15 = 1;
            return new Matrix4x4(m0, m1, m2, m3,
                                 m4, m5, m6, m7,
                                 m8, m9, m10, m11,
                                 m12, m13, m14, m15);
        }

        public static Matrix4x4 Translate(float x, float y, float z) {
            var resultMatrix = Identity;
            resultMatrix.M3 = x;
            resultMatrix.M7 = y;
            resultMatrix.M11 = z;
            return resultMatrix;
        }

        public static Matrix4x4 scaleMatrix(float scale) {
            float m0 = scale;
            float m1 = 0;
            float m2 = 0;
            float m3 = 0;
            float m4 = 0;
            float m5 = scale;
            float m6 = 0;
            float m7 = 0;
            float m8 = 0;
            float m9 = 0;
            float m10 = scale;
            float m11 = 0;
            float m12 = 0;
            float m13 = 0;
            float m14 = 0;
            float m15 = 1;

            return new Matrix4x4(m0, m1, m2, m3,
                                 m4, m5, m6, m7,
                                 m8, m9, m10, m11,
                                 m12, m13, m14, m15);
        }

        public static Matrix4x4 scaleMatrix(float scaleX, float scaleY, float scaleZ) {
            float m0 = scaleX;
            float m1 = 0;
            float m2 = 0;
            float m3 = 0;
            float m4 = 0;
            float m5 = scaleY;
            float m6 = 0;
            float m7 = 0;
            float m8 = 0;
            float m9 = 0;
            float m10 = scaleZ;
            float m11 = 0;
            float m12 = 0;
            float m13 = 0;
            float m14 = 0;
            float m15 = 1;

            return new Matrix4x4(m0, m1, m2, m3,
                                 m4, m5, m6, m7,
                                 m8, m9, m10, m11,
                                 m12, m13, m14, m15);
        }

        public static Matrix4x4 projectionMatrix(float viewAngle, float zNear, float zFar) {
            float m0 = (1024/768) * MathF.Tan(2 / viewAngle);
            float m1 = 0;
            float m2 = 0;
            float m3 = 0;
            float m4 = 0;
            float m5 = MathF.Tan(2 / viewAngle);
            float m6 = 0;
            float m7 = 0;
            float m8 = 0;
            float m9 = 0;
            float m10 = zFar  / (zFar - zNear);
            float m11 = -zFar * zNear/ (zFar - zNear);
            float m12 = 0;
            float m13 = 0;
            float m14 = 1;
            float m15 = 0;

            return new Matrix4x4(m0, m1, m2, m3,
                                 m4, m5, m6, m7,
                                 m8, m9, m10, m11,
                                 m12, m13, m14, m15);

        }

        public static Matrix4x4 inverseMatrix(Matrix4x4 matrix) {
            float m11 = matrix.M0;
            float m12 = matrix.M1;
            float m13 = matrix.M2;
            float m14 = matrix.M3;
            float m21 = matrix.M4;
            float m22 = matrix.M5;
            float m23 = matrix.M6;
            float m24 = matrix.M7;
            float m31 = matrix.M8;
            float m32 = matrix.M9;
            float m33 = matrix.M10;
            float m34 = matrix.M11;
            float m41 = matrix.M12;
            float m42 = matrix.M13;
            float m43 = matrix.M14;
            float m44 = matrix.M15;

            // find the determinant
            float det = m11 * (m22 * m33 * m44 + m23 * m34 * m42 + m24 * m32 * m43 - m24 * m33 * m42 - m23 * m32 * m44 - m22 * m34 * m43)
                      - m21 * (m12 * m33 * m44 + m13 * m34 * m42 + m14 * m32 * m43 - m14 * m33 * m42 - m13 * m32 * m44 - m12 * m34 * m43)
                      + m31 * (m12 * m23 * m44 + m13 * m24 * m42 + m14 * m22 * m43 - m14 * m23 * m42 - m13 * m22 * m44 - m12 * m24 * m43)
                      - m41 * (m12 * m23 * m34 + m13 * m24 * m32 + m14 * m22 * m33 - m14 * m23 * m32 - m13 * m22 * m34 - m12 * m24 * m33);

            // Find the components of adjugated matrix
            float a11 = m22 * m33 * m44 + m23 * m34 * m42 + m24 * m32 * m43 
                        - m24 * m33 * m42 - m23 * m32 * m44 - m22 * m34 * m43;
            float a12 = m21 * m33 * m44 + m23 * m34 * m41 + m24 * m31 * m43 
                        - m24 * m33 * m41 - m23 * m31 * m44 - m21 * m34 * m43;
            float a13 = m21 * m32 * m44 + m22 * m34 * m41 + m24 * m31 * m42 
                        - m24 * m32 * m41 - m22 * m31 * m44 - m21 * m34 * m42;
            float a14 = m21 * m32 * m43 + m22 * m33 * m41 + m23 * m31 * m42
                        - m23 * m32 * m41 - m22 * m31 * m43 - m21 * m33 * m42;

            float a21 = m12 * m33 * m44 + m13 * m34 * m42 + m14 * m32 * m43 
                      - m14 * m33 * m42 - m13 * m32 * m44 - m12 * m34 * m43;
            float a22 = m11 * m33 * m44 + m13 * m34 * m41 + m14 * m31 * m43 
                      - m14 * m33 * m41 - m13 * m31 * m44 - m11 * m34 * m43;
            float a23 = m11 * m32 * m44 + m12 * m34 * m41 + m14 * m31 * m42
                      - m14 * m32 * m41 - m12 * m31 * m44 - m11 * m34 * m42;
            float a24 = m11 * m32 * m43 + m12 * m33 * m41 + m13 * m31 * m42 
                      - m13 * m32 * m41 - m12 * m31 * m43 - m11 * m33 * m42;

            float a31 = m12 * m23 * m44 + m13 * m24 * m42 + m14 * m22 * m43 
                      - m14 * m23 * m42 - m13 * m22 * m44 - m12 * m24 * m43;
            float a32 = m11 * m23 * m44 + m13 * m24 * m41 + m14 * m21 * m43
                      - m14 * m23 * m41 - m13 * m21 * m44 - m11 * m24 * m43;
            float a33 = m11 * m22 * m44 + m12 * m24 * m41 + m14 * m21 * m42 
                      - m14 * m22 * m41 - m12 * m21 * m44 - m11 * m24 * m42;
            float a34 = m11 * m22 * m43 + m12 * m23 * m41 + m13 * m21 * m42
                      - m13 * m22 * m41 - m12 * m21 * m43 - m11 * m23 * m42;

            float a41 = m12 * m23 * m34 + m13 * m24 * m32 + m14 * m22 * m33 
                      - m14 * m23 * m32 - m13 * m22 * m34 - m12 * m24 * m33;
            float a42 = m11 * m23 * m34 + m13 * m24 * m31 + m14 * m21 * m33
                      - m14 * m23 * m31 - m13 * m21 * m34 - m11 * m24 * m33;
            float a43 = m11 * m22 * m34 + m12 * m24 * m31 + m14 * m21 * m32 
                      - m14 * m22 * m31 - m12 * m21 * m34 - m11 * m24 * m32;
            float a44 = m11 * m22 * m33 + m12 * m23 * m31 + m13 * m21 * m32 
                      - m13 * m22 * m31 - m12 * m21 * m33 - m11 * m23 * m32;
            // Inverted matrix return [;
            var result = new Matrix4x4(a11/det, - a21/det, a31/det, - a41/det,
                                       - a12/det, a22/det, -a32/det, a42/det,
                                       a13/det, -a23/det, a33/det, - a43/det,
                                       - a14/det, a24/det, - a34/det, a44/det);
            return result;

        }

        // Matrix4x4 multiplies Matrix4x4
        public static Matrix4x4 operator *(Matrix4x4 Left, Matrix4x4 Right) {
            float m0 = Left.M0 * Right.M0 + Left.M1 * Right.M4 + Left.M2 * Right.M8 + Left.M3 * Right.M12;
            float m1 = Left.M0 * Right.M1 + Left.M1 * Right.M5 + Left.M2 * Right.M9 + Left.M3 * Right.M13;
            float m2 = Left.M0 * Right.M2 + Left.M1 * Right.M6 + Left.M2 * Right.M10 + Left.M3 * Right.M14;
            float m3 = Left.M0 * Right.M3 + Left.M1 * Right.M7 + Left.M2 * Right.M11 + Left.M3 * Right.M15;

            float m4 = Left.M4 * Right.M0 + Left.M5 * Right.M4 + Left.M6 * Right.M8 + Left.M7 * Right.M12;
            float m5 = Left.M4 * Right.M1 + Left.M5 * Right.M5 + Left.M6 * Right.M9 + Left.M7 * Right.M13;
            float m6 = Left.M4 * Right.M2 + Left.M5 * Right.M6 + Left.M6 * Right.M10 + Left.M7 * Right.M14;
            float m7 = Left.M4 * Right.M3 + Left.M5 * Right.M7 + Left.M6 * Right.M11 + Left.M7 * Right.M15;

            float m8 = Left.M8 * Right.M0 + Left.M9 * Right.M4 + Left.M10 * Right.M8 + Left.M11 * Right.M12;
            float m9 = Left.M8 * Right.M1 + Left.M9 * Right.M5 + Left.M10 * Right.M9 + Left.M11 * Right.M13;
            float m10 = Left.M8 * Right.M2 + Left.M9 * Right.M6 + Left.M10 * Right.M10 + Left.M11 * Right.M14;
            float m11 = Left.M8 * Right.M3 + Left.M9 * Right.M7 + Left.M10 * Right.M11 + Left.M11 * Right.M15;

            float m12 = Left.M12 * Right.M0 + Left.M13 * Right.M4 + Left.M14 * Right.M8 + Left.M15 * Right.M12;
            float m13 = Left.M12 * Right.M1 + Left.M13 * Right.M5 + Left.M14 * Right.M9 + Left.M15 * Right.M13;
            float m14 = Left.M12 * Right.M2 + Left.M13 * Right.M6 + Left.M14 * Right.M10 + Left.M15 * Right.M14;
            float m15 = Left.M12 * Right.M3 + Left.M13 * Right.M7 + Left.M14 * Right.M11 + Left.M15 * Right.M15;

            return new Matrix4x4(m0, m1, m2, m3,
                                 m4, m5, m6, m7,
                                 m8, m9, m10, m11,
                                 m12, m13, m14, m15);

        }

        // Matrix4x4 multiplies Point4D
        public static Point4D operator *(Point4D Point, Matrix4x4 Matrix) {
            float x = Matrix.M0 * Point.X + Matrix.M1 * Point.Y + Matrix.M2 * Point.Z + Matrix.M3 * Point.M;
            float y = Matrix.M4 * Point.X + Matrix.M5 * Point.Y + Matrix.M6 * Point.Z + Matrix.M7 * Point.M;
            float z = Matrix.M8 * Point.X + Matrix.M9 * Point.Y + Matrix.M10 * Point.Z + Matrix.M11 * Point.M;
            float m = Matrix.M12 * Point.X + Matrix.M13 * Point.Y + Matrix.M14 * Point.Z + Matrix.M15 * Point.M;
            return new Point4D(x, y, z, m);

        }

        // Matrix4x4 multiplies Point3D
        public static Point3D operator *(Point3D Point, Matrix4x4 Matrix) {
            float x = Matrix.M0 * Point.X + Matrix.M1 * Point.Y + Matrix.M2 * Point.Z + Matrix.M3;
            float y = Matrix.M4 * Point.X + Matrix.M5 * Point.Y + Matrix.M6 * Point.Z + Matrix.M7;
            float z = Matrix.M8 * Point.X + Matrix.M9 * Point.Y + Matrix.M10 * Point.Z + Matrix.M11;
            float m = Matrix.M12 * Point.X + Matrix.M13 * Point.Y + Matrix.M14 * Point.Z + Matrix.M15;
            return new Point3D(x, y, z);

        }

    }
}
