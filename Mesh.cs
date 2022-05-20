using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SoftwareGraphicsSandbox {
    class Mesh {

        // Field of class
        public Point3D[] Vertices;

        // Constructor
        public Mesh(IEnumerable<Point3D> vertices) {
            Vertices = vertices.ToArray();
        }
        
        // rows, columns - number of CELLS in a grid. NOT vertices, blyat.
        public static Mesh Plane(int rows, int columns) {
            int verticesNumber = (rows + 1) * (columns + 1);
            int triangleNumber = rows * columns * 2;
            int triangleVerticesNumber = triangleNumber * 3;

            // List is like a array
            Point3D[] triangleVertices = new Point3D[triangleVerticesNumber];

            float cellRow = 1f / rows;
            float cellColumn = 1f / columns;
            int i = 0;
            for (int c = 0; c < columns; c++) {
                float startZ = 0.5f - c * cellColumn;
                for (int r = 0; r < rows; r++) {
                    float startX = -0.5f + r * cellRow;
                    

                    Point3D LeftTop = new Point3D(startX, 0, startZ);
                    Point3D RightTop = new Point3D(startX + cellRow, 0, startZ);
                    Point3D LeftBottom = new Point3D(startX, 0, startZ - cellColumn);
                    Point3D RightBottom = new Point3D(startX + cellRow, 0, startZ - cellColumn);

                    triangleVertices[i] = LeftTop;
                    triangleVertices[i + 1] = RightTop;
                    triangleVertices[i + 2] = LeftBottom;

                    triangleVertices[i + 3] = RightTop;
                    triangleVertices[i + 4] = LeftBottom;
                    triangleVertices[i + 5] = RightBottom;

                    i += 6;



                }
            }


            

            
            return new Mesh(triangleVertices);

        }

        public static Mesh Cube() {
            var vertices = new Point3D[] {



                new Point3D(-0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, 0.5f, -0.5f),

                new Point3D(-0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, 0.5f, -0.5f),
                new Point3D(-0.5f, 0.5f, -0.5f),

                new Point3D(-0.5f, -0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, -0.5f),

                new Point3D(-0.5f, -0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, -0.5f),
                new Point3D(-0.5f, -0.5f, -0.5f),

                new Point3D(-0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, 0.5f),

                new Point3D(-0.5f, 0.5f, 0.5f),
                new Point3D(-0.5f, -0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, 0.5f),

                new Point3D(0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, 0.5f),
                new Point3D(0.5f, -0.5f, -0.5f),

                new Point3D(0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, 0.5f, -0.5f),
                new Point3D(0.5f, -0.5f, -0.5f),

                new Point3D(0.5f, 0.5f, 0.5f),
                new Point3D(0.5f, 0.5f, -0.5f),
                new Point3D(0.5f, -0.5f, -0.5f),

                new Point3D(-0.5f, 0.5f, -0.5f),
                new Point3D(0.5f, -0.5f, -0.5f),
                new Point3D(-0.5f, -0.5f, -0.5f),

                new Point3D(-0.5f, 0.5f, -0.5f),
                new Point3D(-0.5f, -0.5f, 0.5f),
                new Point3D(-0.5f, -0.5f, -0.5f),

                new Point3D(-0.5f, 0.5f, 0.5f),
                new Point3D(-0.5f, 0.5f, -0.5f),
                new Point3D(-0.5f, -0.5f, 0.5f),

                };
            return new Mesh(vertices);
        }

        

        public static Mesh Sphere(int rows, int columns) {
            // rows = parallels; columns = meridians

            var horizontalStep = (float)(2 * MathF.PI / columns);
            var verticalStep = (float)(1/2f * MathF.PI / rows);

            var triangleNumber = rows * columns * 2;
            var triangleVerticesNumber = triangleNumber * 3;
            var vertices = new Point3D[triangleVerticesNumber];

            var horCos = MathF.Cos(horizontalStep);
            var horSin = MathF.Sin(horizontalStep);

            var vertCos = MathF.Cos(verticalStep);
            var vertSin = MathF.Sin(verticalStep);
            

            var c = 0;
            // start is the initial point
            var start = new Point3D(-0.5f, 0, 0.5f);

            for (int i = 0; i < columns; i++) {
                var firstBottom = start;
                var secondBottom = new Point3D(horCos * start.X - horSin * start.Z, start.Y, horSin * start.X + horCos * start.Z);
                var firstTop = new Point3D(vertCos * start.X - vertSin * start.Y, vertSin * start.X + vertCos * start.Y, start.Z);
                var secondTop = new Point3D(horCos * secondBottom.X - horSin * secondBottom.Z, secondBottom.Y, horSin * secondBottom.X + horCos * secondBottom.Z);

                vertices[c] = firstBottom;
                vertices[c + 1] = secondBottom;
                vertices[c + 2] = firstTop;

                vertices[c + 3] = secondBottom;
                vertices[c + 4] = firstTop;
                vertices[c + 5] = secondTop;

                c += 6;

                start = secondBottom;
            }

            return new Mesh(vertices);

        }
        
       
    }
}

