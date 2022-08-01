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

        // z position is -2 by default and y position is 1
        static Point3D cameraPosition = new Point3D(0.0f, 1.5f, -2f); //new Point3D(0.5f, 1f, -2.0f);

        static float zPos = 0.0f;

        static float zNear = 0.1f;

        static void myEvent(object sender, KeyEventArgs key) {

            float cameraSpeed = 0.05f;


            if (key.KeyCode == Keys.Q) {
                zNear -= 0.1f;
            }

            if (key.KeyCode == Keys.E) {
                zNear += 0.1f;
            }



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

            var depth = new DepthBuffer(form.Width, form.Height);

            float DegreesToRadians(float angleDegrees) {
                return (float)(angleDegrees * Math.PI / 180);
            }

            float angle = 0.5f;

            var cameraTarget = Point3D.Zero;

            float cameraAspect = (float)image.Height / (float)image.Width;

            float cameraFov = DegreesToRadians(120.0f);

            var sphere = Mesh.Sphere(29, 29);
            var plane = Mesh.Plane(2, 2);
            var cube = Mesh.Cube();

            while (_running) {
                //Render image 
                //Process window messages
                Application.DoEvents();

                var cameraProjectionMatrix = Matrix4x4.ProjectionMatrix(cameraFov, 1.0f, 100.0f, cameraAspect);

                var position = new Point3D(0f, 0f, zPos);

                image.FillColor(Color32.Black);

                depth.FillArray(1.0f);

                angle += DegreesToRadians(0.1f);

                var cameraViewMatrix = Matrix4x4.LookAt(cameraPosition, cameraTarget, Point3D.Up);




                void DrawMesh(Mesh mesh, Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection) {
                    var mvp = projection * view * model;
                    // Method select works like the method map in java script
                    var mvpPoints = mesh.Vertices.Select(x => new Point4D(x) * mvp).ToArray();
                    //var mvpScreenPoints = mvpPoints.Select(x => new Point3D(x.X / x.W, x.Y / x.W, x.Z / x.W)).ToArray();

                    Point3D[] toMvpScreenPoints(Point4D[] clippedVertices) {
                        var result = new Point3D[clippedVertices.Length];
                        for (int i = 0; i < clippedVertices.Length; i++) {
                            result[i] = new Point3D(clippedVertices[i]);
                        }
                        return result;
                    }

                    Point3D toPixelCoordinatesPoint(Point3D ndcSpace) {
                        Point3D result = new Point3D();
                        result.X = ndcSpace.X * 0.5f * image.Width + (image.Width / 2.0f);
                        result.Y = -ndcSpace.Y * 0.5f * image.Height + (image.Height / 2.0f);
                        result.Z = ndcSpace.Z;
                        return result;
                    }

                    Point3D[] toPixelCoordinates(Point3D[] ndcSpace) {
                        var result = new Point3D[ndcSpace.Length];
                        for (int i = 0; i < ndcSpace.Length; i++) {
                            result[i].X = ndcSpace[i].X * 0.5f * image.Width + (image.Width / 2.0f);
                            result[i].Y = -ndcSpace[i].Y * 0.5f * image.Height + (image.Height / 2.0f);
                            result[i].Z = ndcSpace[i].Z;
                        }
                        return result;
                    }

                    bool normalTest(Point3D v0, Point3D v1, Point3D v2) {
                        var p1p0 = new Point3D(v1.X - v0.X, v1.Y - v0.Y, v1.Z - v0.Z);
                        var p2p0 = new Point3D(v2.X - v0.X, v2.Y - v0.Y, v2.Z - v0.Z);
                        var cross = Point3D.Cross(p1p0, p2p0);
                        if (cross.Z > 0) {
                            return true;
                        } else {
                            return false;
                        }
                    }

                    for (int i = 0; i < mvpPoints.Length; i += 3) {
                        var p0 = mvpPoints[i];
                        var p1 = mvpPoints[i + 1];
                        var p2 = mvpPoints[i + 2];

                        // Discard whole triangles here
                        if(Clipping.DiscardTriangles(p0, p1, p2)) {
                            continue;
                        }

                        // near clipping tests and calculated adjusted vertices
                        if (p0.Z < 0) {
                            if (p1.Z < 0) {
                                var clippedVertices = Clipping.Clip2(p0, p1, p2);
                                var mvpScreenPoints = toMvpScreenPoints(clippedVertices);
                                var pixelCoordinates = toPixelCoordinates(mvpScreenPoints);
                                var v0 = pixelCoordinates[0];
                                var v1 = pixelCoordinates[1];
                                var v2 = pixelCoordinates[2];
                                if (normalTest(v0, v1, v2)) {
                                    Drawing.FillTriangle(depth, image, v0, v1, v2, Color32.Blue);
                                    //Drawing.DrawTriangle(image, v0, v1, v2, Color32.White);
                                }
                            } else if (p2.Z < 0) {
                                var clippedVertices = Clipping.Clip2(p0, p2, p1);
                                var mvpScreenPoints = toMvpScreenPoints(clippedVertices);
                                var pixelCoordinates = toPixelCoordinates(mvpScreenPoints);
                                var v0 = pixelCoordinates[0];
                                var v1 = pixelCoordinates[1];
                                var v2 = pixelCoordinates[2];
                                if (normalTest(v2, v1, v0)) {
                                    Drawing.FillTriangle(depth, image, v0, v1, v2, Color32.Blue);
                                    //Drawing.DrawTriangle(image, v0, v1, v2, Color32.White);
                                }
                            } else {
                                var clippedVertices = Clipping.Clip1(p0, p1, p2);
                                var mvpScreenPoints = toMvpScreenPoints(clippedVertices);
                                var pixelCoordinates = toPixelCoordinates(mvpScreenPoints);
                                var v0 = pixelCoordinates[0];
                                var v1 = pixelCoordinates[1];
                                var v2 = pixelCoordinates[2];
                                if (normalTest(v0, v1, v2)) {
                                    Drawing.FillTriangle(depth, image, v0, v1, v2, Color32.Blue);
                                    //Drawing.DrawTriangle(image, v0, v1, v2, Color32.White);
                                }
                                var v3 = pixelCoordinates[3];
                                var v4 = pixelCoordinates[4];
                                var v5 = pixelCoordinates[5];
                                if (normalTest(v3, v4, v5)) {
                                    Drawing.FillTriangle(depth, image, v3, v4, v5, Color32.Blue);
                                    //Drawing.DrawTriangle(image, v0, v1, v2, Color32.White);
                                }
                            }
                        } else if (p1.Z < 0) {
                            if (p2.Z < 0) {
                                var clippedVertices = Clipping.Clip2(p1, p2, p0);
                                var mvpScreenPoints = toMvpScreenPoints(clippedVertices);
                                var pixelCoordinates = toPixelCoordinates(mvpScreenPoints);
                                var v0 = pixelCoordinates[0];
                                var v1 = pixelCoordinates[1];
                                var v2 = pixelCoordinates[2];
                                if (normalTest(v0, v1, v2)) {
                                    Drawing.FillTriangle(depth, image, v0, v1, v2, Color32.Blue);
                                    //Drawing.DrawTriangle(image, v0, v1, v2, Color32.White);
                                }
                            } else {
                                var clippedVertices = Clipping.Clip1(p1, p0, p2);
                                var mvpScreenPoints = toMvpScreenPoints(clippedVertices);
                                var pixelCoordinates = toPixelCoordinates(mvpScreenPoints);

                                var v0 = pixelCoordinates[0];
                                var v1 = pixelCoordinates[1];
                                var v2 = pixelCoordinates[2];
                                if (normalTest(v2, v1, v0)) {
                                    Drawing.FillTriangle(depth, image, v0, v1, v2, Color32.Blue);
                                    //Drawing.DrawTriangle(image, v0, v1, v2, Color32.White);
                                }

                                var v3 = pixelCoordinates[3];
                                var v4 = pixelCoordinates[4];
                                var v5 = pixelCoordinates[5];
                                if (normalTest(v5, v4, v3)) {
                                    Drawing.FillTriangle(depth, image, v3, v4, v5, Color32.Blue);
                                    //Drawing.DrawTriangle(image, v0, v1, v2, Color32.White);
                                }
                            }
                        } else if (p2.Z < 0) {
                            var clippedVertices = Clipping.Clip1(p2, p0, p1);
                            var mvpScreenPoints = toMvpScreenPoints(clippedVertices);
                            var pixelCoordinates = toPixelCoordinates(mvpScreenPoints);

                            var v0 = pixelCoordinates[0];
                            var v1 = pixelCoordinates[1];
                            var v2 = pixelCoordinates[2];
                            if (normalTest(v0, v1, v2)) {
                                Drawing.FillTriangle(depth, image, v0, v1, v2, Color32.Blue);
                                //Drawing.DrawTriangle(image, v0, v1, v2, Color32.White);
                            }
                            var v3 = pixelCoordinates[3];
                            var v4 = pixelCoordinates[4];
                            var v5 = pixelCoordinates[5];
                            if (normalTest(v3, v4, v5)) {
                                Drawing.FillTriangle(depth, image, v3, v4, v5, Color32.Blue);
                                //Drawing.DrawTriangle(image, v0, v1, v2, Color32.White);
                            }
                        }
                              // no near clipping
                              else {
                            var p0new = new Point3D(p0);
                            var p1new = new Point3D(p1);
                            var p2new = new Point3D(p2);
                            var v0 = toPixelCoordinatesPoint(p0new);
                            var v1 = toPixelCoordinatesPoint(p1new);
                            var v2 = toPixelCoordinatesPoint(p2new);
                            if (normalTest(v0, v1, v2)) {
                                Drawing.FillTriangle(depth, image, v0, v1, v2, Color32.Blue);
                                Drawing.DrawTriangle(image, v0, v1, v2, Color32.White);

                            }
                        }
                    }
                }

                Matrix4x4 modelMatrix = Matrix4x4.Identity* Matrix4x4.RotateMatrix(angle);
                DrawMesh(sphere, modelMatrix, cameraViewMatrix, cameraProjectionMatrix);

                Matrix4x4 modelMatrix2 = Matrix4x4.Translate(new Point3D(0,0,3)) * Matrix4x4.RotateMatrix(angle);
                DrawMesh(cube, modelMatrix2, cameraViewMatrix, cameraProjectionMatrix);

                var p0 = new Point3D(250, 100, 0);
                var p1 = new Point3D(370, 100, 0);
                var p2 = new Point3D(120, 280, 0);

                //Drawing.DrawTriangle(image, p0, p1, p2, Color32.White);

                //Show image in window
                renderer.Present();

                form.Text = $"{cameraPosition.X};{cameraPosition.Y};{cameraPosition.Z}  ZNear={zNear}  Angle={angle}";
            }
        }

        private static void Form_FormClosed(object sender, FormClosedEventArgs e) {
            _running = false;
        }
    }
}
