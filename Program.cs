using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using static System.Console;


namespace SoftwareGraphicsSandbox {
    static class Program {
        private static Image image;
        private static bool _running = true;
        static float angleRoboHand1 = 0;
        static float angleRoboHand2 = 0;
        static Point3D cameraPosition = new Point3D(0, 1, -2);

        static void myEvent(object sender, KeyEventArgs key) {
            if (key.KeyCode == Keys.D2) {
                angleRoboHand1 += 0.07f;
            } else if (key.KeyCode == Keys.D1) {
                angleRoboHand1 -= 0.07f;
            }
            if (key.KeyCode == Keys.D4) {
                angleRoboHand2 += 0.07f;
            } else if (key.KeyCode == Keys.D3) {
                angleRoboHand2 -= 0.07f;
            }


            float cameraSpeed = 0.05f;
            if (key.KeyCode == Keys.W) {
                cameraPosition.Z += cameraSpeed;
            }

            if (key.KeyCode == Keys.S) {
                cameraPosition.Z -= cameraSpeed;
            }

            if (key.KeyCode == Keys.A) {
                cameraPosition.X -= cameraSpeed;
            }

            if (key.KeyCode == Keys.D) {
                cameraPosition.X += cameraSpeed;
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

            float cameraAspect =  (float)image.Height / (float)image.Width;

            float cameraFov = DegreesToRadians(120.0f);

            var cameraProjectionMatrix = Matrix4x4.ProjectionMatrix(cameraFov, 70f, 100.0f, cameraAspect);


            var sphere = Mesh.Sphere(29, 29);
            var plane = Mesh.Plane(7, 7);

            void swap(int a, int b)
{
                int t;
                t = a;
                a = b;
                b = t;
            }

            void fillTriangle(Point2D first, Point2D second, Point2D third) {
                var firstY = (int)first.Y;
                var firstX = (int)first.X;
                var secondY = (int)second.Y;
                var secondX = (int)second.X;
                var thirdX = (int)third.X;
                var thirdY = (int)third.Y;

                float xLeft = Math.Min(firstX, secondX);
                float xRight = Math.Max(firstX, secondX);

 
                float leftK = (thirdX - firstX)/ (thirdY - firstY);
                float rightK = (thirdX - secondX)/ (thirdY - secondY);
                

                for (int y = secondY; y <= thirdY; y++) {

                    xLeft += leftK;
                    xRight += rightK;
                    

                    int x1 = (int)xLeft;
                    int x2 = (int)xRight;



                    for (int xLine = x1; xLine <= x2; xLine++) {
                        image.SetPixel(xLine, y, Color32.Red);
                        
                    }


                    Drawing.DrawTriangle(image, first, second, third, Color32.White);
                }

            }

            while (_running) {
                //Render image 
                //Process window messages
                Application.DoEvents();
                
                image.FillColor(Color32.Black);

                angle += DegreesToRadians(0.1f);

                var cameraViewMatrix = Matrix4x4.LookAt(cameraPosition, cameraTarget, Point3D.Up);
                
                Point3D toPixelCoordinates(Point3D ndcSpace) {
                    Point3D result = new Point3D();
                    result.X = ndcSpace.X * 0.5f * image.Width + (image.Width / 2.0f);
                    result.Y = -ndcSpace.Y * 0.5f * image.Height + (image.Height / 2.0f);
                    result.Z = ndcSpace.Z;
                    return result;
                }


                void drawMesh(Mesh mesh, Matrix4x4 model, Matrix4x4 view, Matrix4x4 projection) {
                    var mvp = projection * view * model;
                    // Method select works like the method map in java script
                    var mvpPoints = mesh.Vertices.Select(x => new Point4D(x) * mvp).ToArray();
                    var mvpScreenPoints = mvpPoints.Select(x => new Point3D(x.X / x.M, x.Y / x.M, x.Z / x.M)).ToArray();

                    for (int i = 0; i < mvpScreenPoints.Length; i += 3) {
                        var p0 = toPixelCoordinates(mvpScreenPoints[i]);
                        var p1 = toPixelCoordinates(mvpScreenPoints[i + 1]);
                        var p2 = toPixelCoordinates(mvpScreenPoints[i + 2]);
                        Drawing.DrawTriangle(image, new Point2D(p0.X, p0.Y), new Point2D(p1.X, p1.Y), new Point2D(p2.X, p2.Y), Color32.Red);
                    }
                }


                Matrix4x4 modelMatrix = Matrix4x4.RotateMatrix(angle);

                //drawMesh(plane, modelMatrix, cameraViewMatrix, cameraProjectionMatrix);

                //drawMesh(sphere, modelMatrix, cameraViewMatrix, cameraProjectionMatrix);

                Point2D Point1 = new Point2D(200, 300);
                Point2D Point2 = new Point2D(500, 300);
                Point2D Point3 = new Point2D(Cursor.Position.X - form.Location.X, Cursor.Position.Y - form.Location.Y);

                fillTriangle(Point1, Point2, Point3);

                //Show image in window
                renderer.Present();
            }
        }

        private static void Form_FormClosed(object sender, FormClosedEventArgs e) {
            _running = false;
        }
    }
}
