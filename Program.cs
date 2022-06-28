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

        static float z = 0.1f;

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
                manage.X += 5;
            }

            if (key.KeyCode == Keys.G) {
                manage.X -= 5;
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

            var cameraProjectionMatrix = Matrix4x4.ProjectionMatrix(cameraFov, 0.1f, 100.0f, cameraAspect);

            var sphere = Mesh.Sphere(29, 29);
            var plane = Mesh.Plane(7, 7);

            while (_running) {
                //Render image 
                //Process window messages
                Application.DoEvents();

                var position = new Point3D(0f, 0f, z);

                image.FillColor(Color32.Black);

                angle += DegreesToRadians(0.1f);

                var cameraViewMatrix = Matrix4x4.LookAt(cameraPosition, cameraTarget, Point3D.Up);
                
                
                 

                void DrawMesh(Mesh mesh, Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection) {
                    var mvp = projection * view * model;
                    // Method select works like the method map in java script
                    var mvpPoints = mesh.Vertices.Select(x => new Point4D(x) * mvp).ToArray();

                    Point3D toPixelCoordinates(Point3D ndcSpace) {
                        Point3D result = new Point3D();
                        result.X = ndcSpace.X * 0.5f * image.Width + (image.Width / 2.0f);
                        result.Y = -ndcSpace.Y * 0.5f * image.Height + (image.Height / 2.0f);
                        result.Z = ndcSpace.Z;
                        return result;
                    }

                    var mvpScreenPoints = new List<Point3D>();
                    
                    for (int i = 0; i < mvpPoints.Length; i += 3) {
                        var p0 = mvpPoints[i];
                        var p1 = mvpPoints[i + 1];
                        var p2 = mvpPoints[i + 2];

                        // Discard whole triangles here
                        Utils.DiscardTriangles(p0, p1, p2);

                        // Initialize new points which appear after divide by w
                        var p03D = new Point3D(p0.X / p0.W, p0.Y / p0.W, p0.Z / p0.W);
                        var p13D = new Point3D(p1.X / p1.W, p1.Y / p1.W, p1.Z / p1.W);
                        var p23D = new Point3D(p2.X / p2.W, p2.Y / p2.W, p2.Z / p2.W);

                        var PostClip = new List<Point3D>();

                        // near clipping tests and calculated adjusted vertices
                        if (p0.Z < 0) {
                            if (p1.Z < 0) {
                                var Clip = Utils.Clip2(p03D, p13D, p23D);
                                PostClip.Add(Clip[0]);
                                PostClip.Add(Clip[1]);
                                PostClip.Add(Clip[2]);
                            } else if (p2.Z < 0) {
                                var Clip = Utils.Clip2(p03D, p13D, p23D);
                                PostClip.Add(Clip[0]);
                                PostClip.Add(Clip[1]);
                                PostClip.Add(Clip[2]);
                            } else {
                                var Clip = Utils.Clip1(p03D, p13D, p23D);
                                PostClip.Add(Clip[0]);
                                PostClip.Add(Clip[1]);
                                PostClip.Add(Clip[2]);
                                PostClip.Add(Clip[3]);
                                PostClip.Add(Clip[4]);
                                PostClip.Add(Clip[5]);
                            }
                        } 
                        else if (p1.Z < 0) {
                             if (p2.Z < 0) {
                                var Clip = Utils.Clip2(p03D, p13D, p23D);
                                PostClip.Add(Clip[0]);
                                PostClip.Add(Clip[1]);
                                PostClip.Add(Clip[2]);
                            } 
                             else {
                                var Clip = Utils.Clip1(p03D, p13D, p23D);
                                PostClip.Add(Clip[0]);
                                PostClip.Add(Clip[1]);
                                PostClip.Add(Clip[2]);
                                PostClip.Add(Clip[3]);
                                PostClip.Add(Clip[4]);
                                PostClip.Add(Clip[5]);
                            }
                        } 
                        else if (p2.Z < 0) {
                                var Clip = Utils.Clip1(p03D, p13D, p23D);
                                PostClip.Add(Clip[0]);
                                PostClip.Add(Clip[1]);
                                PostClip.Add(Clip[2]);
                                PostClip.Add(Clip[3]);
                                PostClip.Add(Clip[4]);
                                PostClip.Add(Clip[5]);
                        }
                            // no near clipping
                            else {
                            PostClip.Add(p03D);
                            PostClip.Add(p13D);
                            PostClip.Add(p23D);
                        }

                        for (int j = 0; j < PostClip.Count; j++) {
                            
                            mvpScreenPoints.Add(PostClip[j]);
                        }

                    }
                    

                    for (int i = 0; i < mvpScreenPoints.Count; i += 3) {
                        var p0 = toPixelCoordinates(mvpScreenPoints[i]);
                        var p1 = toPixelCoordinates(mvpScreenPoints[i + 1]);
                        var p2 = toPixelCoordinates(mvpScreenPoints[i + 2]);

                        // triangle normal direction test
                        var p1p0 = new Point3D(p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
                        var p2p0 = new Point3D(p2.X - p0.X, p2.Y - p0.Y, p2.Z - p0.Z);
                        var cross = Point3D.Cross(p1p0, p2p0);
                        if (cross.Z > 0) {
                            Drawing.FillTriangle(image, new Point2D(p0.X, p0.Y), new Point2D(p1.X, p1.Y), new Point2D(p2.X, p2.Y), Color32.Red);
                            Drawing.DrawTriangle(image, new Point2D(p0.X, p0.Y), new Point2D(p1.X, p1.Y), new Point2D(p2.X, p2.Y), Color32.White);
                        }
                    }
                }

                

                Matrix4x4 modelMatrix = Matrix4x4.Translate(position);

                //DrawMesh(plane, modelMatrix, cameraViewMatrix, cameraProjectionMatrix);

                //DrawMesh(sphere, modelMatrix, cameraViewMatrix, cameraProjectionMatrix); 

                var p0 = new Point2D(250, 100);
                var p1 = new Point2D(370, 100);
                var p2 = new Point2D(120, 280);
                Drawing.BresenhamLinePoint2D(image, new Point2D(manage.X, 50), new Point2D(manage.X, 400), Color32.White);

                if (p0.X < manage.X &&
                    p1.X < manage.X &&
                    p2.X < manage.X) {
                    return;
                }

                if (p0.X < manage.X) {
                    if (p1.X < manage.X) {
                        Utils.Clip2for2D(p0, p1, p2, image, Color32.White);
                        
                    } else if (p2.X < manage.X) {
                        Utils.Clip2for2D(p0, p2, p1, image, Color32.White);
                    } else {
                        Utils.Clip1for2D(p0, p1, p2, image, Color32.White, manage.X);
                    }
                } else if (p1.X < manage.X) {
                    if (p2.X < manage.X) {
                        Utils.Clip2for2D(p1, p2, p0, image, Color32.White);
                    } else {
                        Utils.Clip1for2D(p1, p0, p2, image, Color32.White, manage.X);
                    }
                } else if (p2.X < manage.X) {
                        Utils.Clip1for2D(p2, p0, p1, image, Color32.White, manage.X);
                }
                      // no near clipping
                      else {
                    Drawing.FillTriangle(image, p0, p1, p2, Color32.White);
                }

                



                //Show image in window
                renderer.Present();
            }
        }

        private static void Form_FormClosed(object sender, FormClosedEventArgs e) {
        _running = false;
}
    }
}
