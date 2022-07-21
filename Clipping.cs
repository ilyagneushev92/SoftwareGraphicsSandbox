using System;
using System.Collections.Generic;
using System.Text;

namespace SoftwareGraphicsSandbox {
    class Clipping {
        
        // Interpolate needs to find new vertices after clipping by frustum planes 
        public static Point3D Interpolate(Point3D point1, Point3D point2, float Alpha) {
            var result = point1 + (point2 - point1) * Alpha;
            return result;
        }

        public static bool DiscardTriangles(Point4D v0, Point4D v1, Point4D v2) {
            if (v0.X > v0.W &&
                v1.X > v1.W &&
                v2.X > v2.W) {
                return true;
            }

            if (v0.X < -v0.W && 
                v1.X < -v1.W &&
                v2.X < -v2.W) {
                return true;
            }

            if (v0.Y > v0.W &&
                v1.Y > v1.W &&
                v2.Y > v2.W) {
                return true;
            }

            if (v0.Y < -v0.W &&
                v1.Y < -v1.W &&
                v2.Y < -v2.W) {
                return true;
            }

            if (v0.Z > v0.W &&
                v1.Z > v1.W &&
                v2.Z > v2.W) {
                return true;
            }

            if (v0.Z < 0.0f &&
                v1.Z < 0.0f &&
                v2.Z < 0.0f) {
                return true;
            }

            return false;
        }

        

        public static Point4D[] Clip1(Point4D v0, Point4D v1, Point4D v2) {
            var result = new Point4D[6];

            var AlphaA = (-v0.Z) / (v1.Z - v0.Z);
            var AlphaB = (-v0.Z) / (v2.Z - v0.Z);

            var v0a = v0 + (v1 - v0) * AlphaA;
            var v0b = v0 + (v2 - v0) * AlphaB;

            // 2 triangles 
            result[0] = v0a;
            result[1] = v1;
            result[2] = v2;
                   
            result[3] = v0a;
            result[4] = v2;
            result[5] = v0b;

            return result;
        }

            
        

        public static Point4D[] Clip2(Point4D v0, Point4D v1, Point4D v2) {
            var result = new Point4D[3];

            var Alpha0 = (-v0.Z) / (v2.Z - v0.Z);
            var Alpha1 = (-v1.Z) / (v2.Z - v1.Z);
            var v0New = v0 + (v2 - v0) * Alpha0;
            var v1New = v1 + (v2 - v1) * Alpha1;


            // 1 triangle
            result[0] = v1New;
            result[1] = v2;
            result[2] = v0New;

            return result;
        }


    }
}
