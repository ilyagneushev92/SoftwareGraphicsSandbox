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

        public static void DiscardTriangles(Point4D v0, Point4D v1, Point4D v2) {
            if (v0.X > v0.W &&
                v1.X > v1.W &&
                v2.X > v2.W) {
                return;
            }

            if (v0.X < -v0.W && 
                v1.X < -v1.W &&
                v2.X < -v2.W) {
                return;
            }

            if (v0.Y > v0.W &&
                v1.Y > v1.W &&
                v2.Y > v2.W) {
                return;
            }

            if (v0.Y < -v0.W &&
                v1.Y < -v1.W &&
                v2.Y < -v2.W) {
                return;
            }

            if (v0.Z > v0.W &&
                v1.Z > v1.W &&
                v2.Z > v2.W) {
                return;
            }

            if (v0.Z < 0.0f &&
                v1.Z < 0.0f &&
                v2.Z < 0.0f) {
                return;
            }
        }

        public static void ClipTrianglesFor2D(Point2D p0, Point2D p1, Point2D p2, float edge, Image image) {
            // discard whole triangles
                if (p0.X < edge &&
                    p1.X < edge &&
                    p2.X < edge) {
                return;
            }

            if (p0.X < edge) {
                if (p1.X < edge) {
                    Clip2for2D(p0, p1, p2, image, Color32.White, edge);

                } else if (p2.X < edge) {
                    Clip2for2D(p0, p2, p1, image, Color32.White, edge);
                } else {
                    Clip1for2D(p0, p1, p2, image, Color32.White, edge);
                }
            } else if (p1.X < edge) {
                if (p2.X < edge) {
                    Clip2for2D(p1, p2, p0, image, Color32.White, edge);
                } else {
                    Clip1for2D(p1, p0, p2, image, Color32.White, edge);
                }
            } else if (p2.X < edge) {
                Clip1for2D(p2, p0, p1, image, Color32.White, edge);
            }
                  // no near clipping
                  else {
                Drawing.FillTriangle(image, p0, p1, p2, Color32.White);
            }

        }

        public static Point4D[] Clip1(Point4D v0, Point4D v1, Point4D v2) {
            var result = new Point4D[6];

            var AlphaA = (-v0.Z)/(v1.Z - v0.Z);
            var AlphaB = (-v0.Z) / (v2.Z - v0.Z);
            //var v0a = new Point4D(v0.X + (v1.X - v0.X) * AlphaA, v0.Y + (v1.Y - v0.Y) * AlphaA, v0.Z + (v1.Z - v0.Z) * AlphaA, 1.0f);
            //var v0b = new Point4D(v0.X + (v2.X - v0.X) * AlphaB, v0.Y + (v2.Y - v0.Y) * AlphaB, v0.Z + (v2.Z - v0.Z) * AlphaB, 1.0f);

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
            //var v0New = new Point4D(v0.X + (v2.X - v0.X) * Alpha0, v0.Y + (v2.Y - v0.Y) * Alpha0, v0.Z + (v2.Z - v0.Z) * Alpha0, 1.0f);
            //var v1New = new Point4D(v1.X + (v2.X - v1.X) * Alpha1, v1.Y + (v2.Y - v1.Y) * Alpha1, v1.Z + (v2.Z - v1.Z) * Alpha1, 1.0f);

            var v0New = v0 + (v2 - v0) * Alpha0;
            var v1New = v1 + (v2 - v1) * Alpha1;


            // 1 triangle
            result[0] = v0New;
            result[1] = v1New;
            result[2] = v2;

            return result;
        }

        public static void Clip1for2D(Point2D v0, Point2D v1, Point2D v2, Image image, Color32 color, float edge) {

            var edgeYa = (v0.Y - (v0.X - edge) * (v0.Y - v1.Y) / (v0.X - v1.X));
            var v0a = new Point2D(edge, edgeYa);
            var edgeYb = (v0.Y - (v0.X - edge) * (v0.Y - v2.Y) / (v0.X - v2.X));
            var v0b = new Point2D(edge, edgeYb);


            // 2 triangles 
            Drawing.DrawTriangle(image, v0a, v1, v2, color);
            Drawing.DrawTriangle(image, v0a, v0b, v2, color);
            Drawing.FillTriangle(image, v0a, v1, v2, color);
            Drawing.FillTriangle(image, v0a, v0b, v2, color);


        }

        public static void Clip2for2D(Point2D v0, Point2D v1, Point2D v2, Image image, Color32 color, float edge) {
            //float Alpha0 = (-v0.X) / (v2.X - v0.X);
            //float Alpha1 = (-v1.X) / (v2.X - v1.X);
            //// interpolate to get new vertices
            //var v0New = v0 + (v2 - v0) * Alpha0;
            //var v1New = v1 + (v2 - v1) * Alpha1;

            var edgeY0 = (v2.Y - (v2.X - edge) * (v2.Y - v1.Y) / (v2.X - v1.X));
            var v0New = new Point2D(edge, edgeY0);
            var edgeY1 = (v2.Y - (v2.X - edge) * (v2.Y - v0.Y) / (v2.X - v0.X));
            var v1New = new Point2D(edge, edgeY1);


            // 1 triangle
            Drawing.DrawTriangle(image, v0New, v1New, v2, color);
            Drawing.FillTriangle(image, v0New, v1New, v2, color);
        }



    }
}
