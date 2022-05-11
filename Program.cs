using System;
using System.Windows.Forms;
using System.Drawing;
using static System.Console;


namespace SoftwareGraphicsSandbox {
    static class Program {
        private static Image image;
        private static bool _running = true;
        static float angleRoboHand1 = 0;
        static float angleRoboHand2 = 0;

        //delegate int ololo(object sender, KeyEventArgs args);

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

            // array of vertices
            var vertices = new Point2D[] {
                new Point2D(0, 100),
                new Point2D(0, 0),
                new Point2D(100, 0),
                new Point2D(50, -100)
            };

            // array of indices of vertices
            var indices = new float[] {
                1, 0, 2, 2, 3, 1
            };
            
            float angle = 0;

            

            while (_running) {
                //Render image 
                //Process window messages     
                Application.DoEvents();
                
                image.FillColor(Color32.Black);

                Point2D Point1 = new Point2D(150, 0);
                Point2D Point2 = new Point2D(0, 150);
                Point2D Point3 = new Point2D(0, 0);

                angle += DegreesToRadians(0.1f);

                var schoolCoordinates = Matrix3x3.Translate(-form.Width / 2, form.Height / 2);

                // Mouse drags circle 
                var mouseMatrix1 = Matrix3x3.scaleMatrix(37);
                var mouseMatrix2 = Matrix3x3.Translate(Cursor.Position.X - form.Location.X, - Cursor.Position.Y + form.Location.Y);
                var dragCircle = schoolCoordinates * mouseMatrix2 *  mouseMatrix1;
                Drawing.DrawCircle(dragCircle, 5, image, Color32.Red);


                var triangleRotationMatrix = Matrix3x3.rotateMatrix(angle * 3);
                var triangleOffsetMatrix = Matrix3x3.Translate(200, 0);
                var worldRotationMatrix = Matrix3x3.rotateMatrix(angle);
                var scaleMat = Matrix3x3.scaleMatrix(((MathF.Sin(angle * 3) * 0.5f + 0.5f) + 1) / 2);
                var finalMatrix = worldRotationMatrix * triangleOffsetMatrix * triangleRotationMatrix * scaleMat;

                Point2D newPoint1 = finalMatrix * Point1; 
                Point2D newPoint2 = finalMatrix * Point2;
                Point2D newPoint3 = finalMatrix * Point3;
                Drawing.DrawTriangle(image, newPoint1, newPoint2, newPoint3, Color32.Red);

                //
                // robo-hand starts here
                //

                var handLength = 70;
                var pointZero = new Point2D(0, 0);
                var jointPoint1 = new Point2D(0, handLength);
                var jointPoint2 = new Point2D(0, handLength);
                

                // Rotation for joint1
                var jointRotation1 = Matrix3x3.rotateMatrix(angleRoboHand1);
                // Rotation for joint2
                var jointRotation2 = Matrix3x3.rotateMatrix(angleRoboHand2);


                // Rotation of first joint around zero point
                Point2D rotationJoint1 =  jointRotation1 * jointPoint1;
                // Offset for joint2 relatively zero point
                Matrix3x3 offsetJoint2 = Matrix3x3.Translate(jointPoint1.X, jointPoint2.Y);
                // Rotation of second point around first point
                Point2D rotationJoint2 =  jointRotation1 * offsetJoint2 * jointRotation2 *  jointPoint2;
                
                Drawing.BresenhamLinePoint2D(image, pointZero, rotationJoint1, Color32.Red);
                Drawing.BresenhamLinePoint2D(image, rotationJoint1, rotationJoint2, Color32.Red);

                // Drawing circle for robo-hand
                var jointMatrix = jointRotation1 * offsetJoint2 * Matrix3x3.scaleMatrix(4);
                Drawing.DrawCircle(jointMatrix, 30, image, Color32.Red);

                // Examine invert matrix

                var startMatrix = new Matrix4x4 (10, 12, 17, 39,
                                                 12, 24, 40, 43,
                                                 9, 14, 22, 39,
                                                 10, 59, 27, 25);
                var invertedStartMatrix = Matrix4x4.inverseMatrix(startMatrix);
                var matrixShouldBeIdentity = startMatrix * invertedStartMatrix;
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M0);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M1);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M2);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M3);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M4);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M5);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M6);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M7);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M8);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M9);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M10);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M11);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M12);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M13);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M14);
                System.Diagnostics.Debug.WriteLine(matrixShouldBeIdentity.M15);

                





                //Show image in window
                renderer.Present();
            }
        }

        private static void Form_FormClosed(object sender, FormClosedEventArgs e) {
            _running = false;
        }
    }
}
