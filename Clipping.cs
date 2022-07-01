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

        static Point3D toPixelCoordinates(Point3D ndcSpace, Image image) {
            Point3D result = new Point3D();
            result.X = ndcSpace.X * 0.5f * image.Width + (image.Width / 2.0f);
            result.Y = -ndcSpace.Y * 0.5f * image.Height + (image.Height / 2.0f);
            result.Z = ndcSpace.Z;
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

        public static void Clip1(Point4D v0, Point4D v1, Point4D v2, Image image) {
            var AlphaA = (-v0.Z)/(v1.Z - v0.Z);
            var v0a = v0 + (v1 - v0) * AlphaA;

            var AlphaB = (-v0.Z) / (v2.Z - v0.Z);
            var v0b = v0 + (v2 - v0) * AlphaB;

            // 2 triangles 

            var p0 = toPixelCoordinates(new Point3D(v0a), image);
            var p1 = toPixelCoordinates(new Point3D(v1), image);
            var p2 = toPixelCoordinates(new Point3D(v2), image);

                // triangle normal direction test
                var p1p0 = new Point3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
                var p2p0 = new Point3D(p2.X - p0.X, p2.Y - p0.Y, p2.Z - p0.Z);
                var cross = Point3D.Cross(p1p0, p2p0);
            if (cross.Z > 0) {
                var p02 = new Point2D(p0.X, p0.Y);
                var p12 = new Point2D(p1.X, p1.Y);
                var p22 = new Point2D(p2.X, p2.Y);
                Drawing.FillTriangle(image, new Point2D(p0.X, p0.Y), new Point2D(p1.X, p1.Y), new Point2D(p2.X, p2.Y), Color32.Red);
                Drawing.DrawTriangle(image, new Point2D(p0.X, p0.Y), new Point2D(p1.X, p1.Y), new Point2D(p2.X, p2.Y), Color32.White);
                {

                    p0 = toPixelCoordinates(new Point3D(v0b), image);
                    p1 = toPixelCoordinates(new Point3D(v0a), image);
                    p2 = toPixelCoordinates(new Point3D(v2), image);

                    // triangle normal direction test
                    p1p0 = new Point3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
                    p2p0 = new Point3D(p2.X - p0.X, p2.Y - p0.Y, p2.Z - p0.Z);
                    cross = Point3D.Cross(p1p0, p2p0);
                    if (cross.Z > 0) {
                    p02 = new Point2D(p0.X, p0.Y);
                    p12 = new Point2D(p1.X, p1.Y);
                    p22 = new Point2D(p2.X, p2.Y);
                    Drawing.FillTriangle(image, new Point2D(p0.X, p0.Y), new Point2D(p1.X, p1.Y), new Point2D(p2.X, p2.Y), Color32.Red);
                    Drawing.DrawTriangle(image, new Point2D(p0.X, p0.Y), new Point2D(p1.X, p1.Y), new Point2D(p2.X, p2.Y), Color32.White);
                    }

                }
            }
        }

            
        

        public static void Clip2(Point4D v0, Point4D v1, Point4D v2, Image image) {
            var Alpha1 = (-v1.Z) / (v2.Z - v1.Z);
            var v1New = v1 + (v2 - v1) * Alpha1;

            var Alpha0 = (- v0.Z) / (v2.Z - v0.Z);
            var v0New = v0 + (v2 - v0) * Alpha0;


            var p0 = toPixelCoordinates(new Point3D(v0New), image);
            var p1 = toPixelCoordinates(new Point3D(v2), image);
            var p2 = toPixelCoordinates(new Point3D(v1New), image);

            Drawing.FillTriangle(image, new Point2D(p0.X, p0.Y), new Point2D(p1.X, p1.Y), new Point2D(p2.X, p2.Y), Color32.Red);
            Drawing.DrawTriangle(image, new Point2D(p0.X, p0.Y), new Point2D(p1.X, p1.Y), new Point2D(p2.X, p2.Y), Color32.White);

            // 1 triangle

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
