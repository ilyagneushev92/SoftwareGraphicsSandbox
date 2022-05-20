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

            int a = 1;
            int time = 0;

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


                Matrix4x4 modelMatrix = Matrix4x4.Identity;
                //drawMesh(Mesh.Cube(), modelMatrix, cameraViewMatrix, cameraProjectionMatrix);

                
                drawMesh(Mesh.Plane(a, a), modelMatrix, cameraViewMatrix, cameraProjectionMatrix);
                time++;
                if (time % 250 == 0) a++;




                //Point2D Point1 = new Point2D(150, 0);
                //Point2D Point2 = new Point2D(0, 150);
                //Point2D Point3 = new Point2D(0, 0);
                //
                //
                //
                //var schoolCoordinates = Matrix3x3.Translate(-form.Width / 2, form.Height / 2);
                //
                //// Mouse drags circle 
                //var mouseMatrix1 = Matrix3x3.scaleMatrix(37);
                //var mouseMatrix2 = Matrix3x3.Translate(Cursor.Position.X - form.Location.X, - Cursor.Position.Y + form.Location.Y);
                //var dragCircle = schoolCoordinates * mouseMatrix2 *  mouseMatrix1;
                //Drawing.DrawCircle(dragCircle, 60, image, Color32.Red);
                //
                //
                //var triangleRotationMatrix = Matrix3x3.rotateMatrix(angle * 3);
                //var triangleOffsetMatrix = Matrix3x3.Translate(200, 0);
                //var worldRotationMatrix = Matrix3x3.rotateMatrix(angle);
                //var scaleMat = Matrix3x3.scaleMatrix(((MathF.Sin(angle * 3) * 0.5f + 0.5f) + 1) / 2);
                //var finalMatrix = worldRotationMatrix * triangleOffsetMatrix * triangleRotationMatrix * scaleMat;
                //
                //Point2D newPoint1 = finalMatrix * Point1; 
                //Point2D newPoint2 = finalMatrix * Point2;
                //Point2D newPoint3 = finalMatrix * Point3;
                //Drawing.DrawTriangle(image, newPoint1, newPoint2, newPoint3, Color32.Red);
                //
                ////
                //// robo-hand starts here
                ////
                //
                //var handLength = 70;
                //var pointZero = new Point2D(0, 0);
                //var jointPoint1 = new Point2D(0, handLength);
                //var jointPoint2 = new Point2D(0, handLength);
                //
                //
                //// Rotation for joint1
                //var jointRotation1 = Matrix3x3.rotateMatrix(angleRoboHand1);
                //// Rotation for joint2
                //var jointRotation2 = Matrix3x3.rotateMatrix(angleRoboHand2);
                //
                //
                //// Rotation of first joint around zero point
                //Point2D rotationJoint1 =  jointRotation1 * jointPoint1;
                //// Offset for joint2 relatively zero point
                //Matrix3x3 offsetJoint2 = Matrix3x3.Translate(jointPoint1.X, jointPoint2.Y);
                //// Rotation of second point around first point
                //Point2D rotationJoint2 =  jointRotation1 * offsetJoint2 * jointRotation2 *  jointPoint2;
                //
                //Drawing.BresenhamLinePoint2D(image, pointZero, rotationJoint1, Color32.Red);
                //Drawing.BresenhamLinePoint2D(image, rotationJoint1, rotationJoint2, Color32.Red);
                //
                //// Drawing circle for robo-hand
                //var jointMatrix = jointRotation1 * offsetJoint2 * Matrix3x3.scaleMatrix(4);
                //Drawing.DrawCircle(jointMatrix, 30, image, Color32.Red);

                //Show image in window
                renderer.Present();
            }
        }

        private static void Form_FormClosed(object sender, FormClosedEventArgs e) {
            _running = false;
        }
    }
}
