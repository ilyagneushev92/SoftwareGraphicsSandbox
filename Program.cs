using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using static System.Console;
using System.Collections.Generic;


namespace SoftwareGraphicsSandbox {
    static class Program {
        private static Image image;
        private static bool _running = true;

        static Point3D cameraPosition = new Point3D(0, 1, -2);

        static float zPos = 0.0f;

        //
        static float m = 10;
        static Point2D manage = new Point2D(m, 200);
        //
        static void myEvent(object sender, KeyEventArgs key) {

            float cameraSpeed = 0.05f;

            

            if (key.KeyCode == Keys.W) {
                cameraPosition.Y += cameraSpeed;
            }

            if (key.KeyCode == Keys.S) {
                cameraPosition.Y -= cameraSpeed;
            }

            if (key.KeyCode == Keys.A) {
                cameraPosition.X -= cameraSpeed;
            }

            if (key.KeyCode == Keys.D) {
                cameraPosition.X += cameraSpeed;
            }

            if (key.KeyCode == Keys.Z) {
                cameraPosition.Z += cameraSpeed;
            }

            if (key.KeyCode == Keys.C) {
                cameraPosition.Z -= cameraSpeed;
            }

            if (key.KeyCode == Keys.H) {
                zPos += 0.03f;
            }

            if (key.KeyCode == Keys.G) {
                zPos -= 0.03f;
            }


        }



        [STAThread]
        static void Main() {

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new RenderWindow();
            form.Width = 1024;
            form.Height = 768;

            form.FormClosed += Form_FormClosed;
            form.Show();
            form.KeyDown += myEvent;

            Renderer renderer = new Renderer(form.Handle);
            image = renderer.BackBuffer;

            float DegreesToRadians(float angleDegrees) {
                return (float)(angleDegrees * Math.PI / 180);
            }

            float angle = 0;

            var cameraTarget = Point3D.Zero;

            float cameraAspect = (float)image.Height / (float)image.Width;

            float cameraFov = DegreesToRadians(120.0f);

            var cameraProjectionMatrix = Matrix4x4.ProjectionMatrix(cameraFov, 1f, 100.0f, cameraAspect);

            var sphere = Mesh.Sphere(29, 29);
            var plane = Mesh.Plane(7, 7);
            var cube = Mesh.Cube();
            var triangle3D = Mesh.Triangle3D();

            while (_running) {
                //Render image 
                //Process window messages
                Application.DoEvents();

                var position = new Point3D(0f, 0f, zPos);

                image.FillColor(Color32.Black);

                angle += DegreesToRadians(0.1f);

                var cameraViewMatrix = Matrix4x4.LookAt(cameraPosition, cameraTarget, Point3D.Up);
                
                
                 

                void DrawMesh(Mesh mesh, Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection) {
                    var mvp = projection * view * model;
                    // Method select works like the method map in java script
                    var mvpPoints = mesh.Vertices.Select(x => new Point4D(x) * mvp).ToArray();
                    //var mvpScreenPoints = mvpPoints.Select(x => new Point3D(x.X / x.W, x.Y / x.W, x.Z / x.W)).ToArray();

                    Point3D toPixelCoordinates(Point3D ndcSpace) {
                        Point3D result = new Point3D();
                        result.X = ndcSpace.X * 0.5f * image.Width + (image.Width / 2.0f);
                        result.Y = -ndcSpace.Y * 0.5f * image.Height + (image.Height / 2.0f);
                        result.Z = ndcSpace.Z;
                        return result;
                    }

                    //var mvpScreenPoints = new List<Point3D>();
                    
                    for (int i = 0; i < mvpPoints.Length; i += 3) {
                        var p0 = mvpPoints[i];
                        var p1 = mvpPoints[i + 1];
                        var p2 = mvpPoints[i + 2];

                        // Discard whole triangles here
                        Clipping.DiscardTriangles(p0, p1, p2);

                        // near clipping tests and calculated adjusted vertices
                        if (p0.Z < 0) {
                            if (p1.Z < 0) {
                                Clipping.Clip2(p0, p1, p2, image);
                                
                            } else if (p2.Z < 0) {
                                Clipping.Clip2(p0, p2, p1, image);
                                
                            } else {
                                Clipping.Clip1(p0, p1, p2, image);

                            }
                        } 
                        else if (p1.Z < 0) {
                             if (p2.Z < 0) {
                                Clipping.Clip2(p1, p2, p0, image);
                                
                            } 
                             else {
                                Clipping.Clip1(p1, p0, p2, image);
                                
                            }
                        } 
                        else if (p2.Z < 0) {
                                Clipping.Clip1(p2, p0, p1, image);
                                
                        }
                            // no near clipping
                            else {
                            var v0 = toPixelCoordinates(new Point3D(p0));
                            var v1 = toPixelCoordinates(new Point3D(p1));
                            var v2 = toPixelCoordinates(new Point3D(p2));
                            var p1p0 = new Point3D(v1.X - v0.X, v1.Y - v0.Y, v1.Z - v0.Z);
                            var p2p0 = new Point3D(v2.X - v0.X, v2.Y - v0.Y, v2.Z - v0.Z);
                            var cross = Point3D.Cross(p1p0, p2p0);
                            if (cross.Z > 0) { 
                            Drawing.FillTriangle(image, new Point2D(v0.X, v0.Y), new Point2D(v1.X, v1.Y), new Point2D(v2.X, v2.Y), Color32.Red);
                            Drawing.DrawTriangle(image, new Point2D(v0.X, v0.Y), new Point2D(v1.X, v1.Y), new Point2D(v2.X, v2.Y), Color32.White);
                            }
                        }


                    }
                    

                    //for (int i = 0; i < mvpScreenPoints.Count; i += 3) {
                    //    var p0 = toPixelCoordinates(mvpScreenPoints[i]);
                    //    var p1 = toPixelCoordinates(mvpScreenPoints[i + 1]);
                    //    var p2 = toPixelCoordinates(mvpScreenPoints[i + 2]);

                    //    // triangle normal direction test
                    //    var p1p0 = new Point3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
                    //    var p2p0 = new Point3D(p2.X - p0.X, p2.Y - p0.Y, p2.Z - p0.Z);
                    //    var cross = Point3D.Cross(p1p0, p2p0);
                    //    if (cross.Z > 0) {
                    //       var p02 = new Point2D(p0.X, p0.Y);
                    //       var p12 = new Point2D(p1.X, p1.Y);
                    //       var p22 = new Point2D(p2.X, p2.Y);
                    //        Clipping.ClipTrianglesFor2D(p02, p12, p22, 100, image);
                    //        //Drawing.FillTriangle(image, new Point2D(p0.X, p0.Y), new Point2D(p1.X, p1.Y), new Point2D(p2.X, p2.Y), Color32.Red);
                    //        //Drawing.DrawTriangle(image, new Point2D(p0.X, p0.Y), new Point2D(p1.X, p1.Y), new Point2D(p2.X, p2.Y), Color32.White);
                    //    }
                    //}
                }

                Matrix4x4 modelMatrix = Matrix4x4.Translate(position);
                
                DrawMesh(plane, modelMatrix, cameraViewMatrix, cameraProjectionMatrix);

                //DrawMesh(sphere, modelMatrix, cameraViewMatrix, cameraProjectionMatrix); 

                //DrawMesh(triangle3D, modelMatrix, cameraViewMatrix, cameraProjectionMatrix);

                var p0 = new Point2D(250, 100);
                var p1 = new Point2D(370, 100);
                var p2 = new Point2D(120, 280);

                //Utils.ClipTrianglesFor2D(p0, p1, p2, manage.X, image);

                //Show image in window
                renderer.Present();
            }
        }

        private static void Form_FormClosed(object sender, FormClosedEventArgs e) {
        _running = false;
}
    }
}
