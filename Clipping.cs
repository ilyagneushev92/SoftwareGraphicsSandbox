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

            if (v0.Z < 0.01f &&
                v1.Z < 0.01f &&
                v2.Z < 0.01f) {
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

        public static List<Point3D> Clip1(Point3D v0, Point3D v1, Point3D v2, float edgeZ) {
            //float AlphaA = (-v0.Z) / (v1.Z - v0.Z);
            //float AlphaB = (-v0.Z) / (v2.Z - v0.Z);
            //// interpolate to get new vertices
            //var v0a = Interpolate(v0, v1, AlphaA);
            //var v0b = Interpolate(v0, v2, AlphaB);
            var v0v1 = v0 - v1;
            var AlphaA = (edgeZ - v0.Z) / (v1.Z - v0.Z);
            var v0v0a = v0v1 *AlphaA;
            var v0a = v0 + v0v0a;

            var v0v02 = v0 - v2;
            var AlphaB = (edgeZ - v0.Z) / (v2.Z - v0.Z);
            var v0v0b = v0v02 * AlphaB;
            var v0b = v0 + v0v0b;

            // 2 triangles 
            var result = new List<Point3D>();
            result.Add(v0);
            result.Add(v1);
            result.Add(v2);
            result.Add(v0a);
            result.Add(v0b);
            result.Add(v2);

            return result;
        }

        public static List<Point3D> Clip2(Point3D v0, Point3D v1, Point3D v2, float edgeZ) {
            //float Alpha0 = (-v0.Z) / (v2.Z - v0.Z);
            //float Alpha1 = (-v1.Z) / (v2.Z - v1.Z);
            //// interpolate to get new vertices
            //var v0New = v0 + (v2 - v0) * Alpha0;
            //var v1New = v1 + (v2 - v1) * Alpha1;
            var v2v1 = v2 - v1;
            var Alpha1 = (edgeZ - v2.Z) / (v1.Z - v2.Z);
            var v2v1New = v2v1 * Alpha1;
            var v1New = v2 + v2v1New;

            var v2v0 = v2 - v0;
            var Alpha0 = (edgeZ - v2.Z) / (v0.Z - v2.Z);
            var v2v0New = v2v0 * Alpha0;
            var v0New = v2 + v2v0New;



            // 1 triangle
            var result = new List<Point3D>();
            result.Add(v0New);
            result.Add(v1New);
            result.Add(v2);
            return result;
        }

        public static void Clip1for2D(Point2D v0, Point2D v1, Point2D v2, Image image, Color32 color, float edge) {
            //float AlphaA = (-v0.X) / (v1.X - v0.X);
            //float AlphaB = (-v0.X) / (v2.X - v0.X);
            // interpolate to get new vertices
            //var v0a = v0 + (v1 - v0) * AlphaA;
            //var v0b = v0 + (v2 - v0) * AlphaB;

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
