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

        public static Mesh SimpleTriangle() {
            var result = new Point3D[] {
            new Point3D(-1.0f, 0, -0.7f),
            new Point3D(-1.0f, 0, 0.7f),
            new Point3D(0.8f, 0, 0.7f),

        };
            return new Mesh(result);
        }


        public static Mesh Plane(int rows, int columns) {
            int triangleNumber = rows * columns * 2;
            int triangleVerticesNumber = triangleNumber * 3;
            var triangleVertices = new Point3D[triangleVerticesNumber];

            int i = 0;
            float cellRow = 1f / rows;
            float cellColumn = 1f / columns;
            for (int c = 0; c < columns; c++) {
                var startZ = 0.5f - c * cellColumn;
                for (int r = 0; r < rows; r++) {
                    var startX = -0.5f + r * cellRow;

                    Point3D LeftTop = new Point3D(startX, 0, startZ);
                    Point3D RightTop = new Point3D(startX + cellRow, 0, startZ);
                    Point3D LeftBottom = new Point3D(startX, 0, startZ - cellColumn);
                    Point3D RightBottom = new Point3D(startX + cellRow, 0, startZ - cellColumn);

                    triangleVertices[i] = LeftTop;
                    triangleVertices[i + 1] = RightTop;
                    triangleVertices[i + 2] = LeftBottom;
                    triangleVertices[i + 3] = LeftBottom;
                    triangleVertices[i + 4] = RightTop;
                    triangleVertices[i + 5] = RightBottom;

                    i += 6;


                }
            }
            return new Mesh(triangleVertices);
        }

        private static Point3D[] Plane2x2Vertices() {
            var result = new Point3D[] {
            new Point3D(-0.5f, 0, 0.5f),
            new Point3D(0, 0, 0.5f),
            new Point3D(0.5f, 0, 0.5f),

            new Point3D(-0.5f, 0, 0),
            new Point3D(0, 0, 0),
            new Point3D(0.5f, 0, 0),

            new Point3D(-0.5f, 0, -0.5f),
            new Point3D(0, -0, -0.5f),
            new Point3D(0.5f, 0, -0.5f),


        };
            return result;
        }

        public static Mesh Plane2x2() {
            var vertices = new Point3D[] {
            Plane2x2Vertices()[0],
            Plane2x2Vertices()[1],
            Plane2x2Vertices()[3],

            Plane2x2Vertices()[1],
            Plane2x2Vertices()[4],
            Plane2x2Vertices()[3],

            Plane2x2Vertices()[1],
            Plane2x2Vertices()[2],
            Plane2x2Vertices()[4],

            Plane2x2Vertices()[2],
            Plane2x2Vertices()[5],
            Plane2x2Vertices()[4],

            Plane2x2Vertices()[3],
            Plane2x2Vertices()[4],
            Plane2x2Vertices()[6],

            Plane2x2Vertices()[4],
            Plane2x2Vertices()[7],
            Plane2x2Vertices()[6],

            Plane2x2Vertices()[4],
            Plane2x2Vertices()[5],
            Plane2x2Vertices()[7],

            Plane2x2Vertices()[5],
            Plane2x2Vertices()[8],
            Plane2x2Vertices()[7],
            };

            return new Mesh(vertices);
        }




        private static Point3D[] _cubeVertices = new Point3D[] {
            new Point3D(-0.5f, 0.5f, -0.5f),
            new Point3D(0.5f, 0.5f, -0.5f),
            new Point3D(0.5f, -0.5f, -0.5f),
            new Point3D(-0.5f, -0.5f, -0.5f),
            new Point3D(-0.5f, 0.5f, 0.5f),
            new Point3D(0.5f, 0.5f, 0.5f),
            new Point3D(0.5f, -0.5f, 0.5f),
            new Point3D(-0.5f, -0.5f, 0.5f),
        };
           

        public static Mesh Triangle3D() {

            var result = new Point3D[] {

            new Point3D(0f, 0.5f, 1.5f),
            new Point3D(0.5f, 0f, -0.5f),
            new Point3D(-0.5f, 0f, -0.5f),
        };
            return new Mesh(result);
        }

        public static Mesh Cube() {
            var vertices = new Point3D[] {

                _cubeVertices[0],
                _cubeVertices[1],
                _cubeVertices[2],

                _cubeVertices[0],
                _cubeVertices[2],
                _cubeVertices[3],

                _cubeVertices[4],
                _cubeVertices[5],
                _cubeVertices[1],

                _cubeVertices[4],
                _cubeVertices[1],
                _cubeVertices[0],

                _cubeVertices[4],
                _cubeVertices[0],
                _cubeVertices[3],

                _cubeVertices[4],
                _cubeVertices[3],
                _cubeVertices[7],

                _cubeVertices[1],
                _cubeVertices[5],
                _cubeVertices[6],

                _cubeVertices[1],
                _cubeVertices[6],
                _cubeVertices[2],

                _cubeVertices[3],
                _cubeVertices[2],
                _cubeVertices[6],

                _cubeVertices[3],
                _cubeVertices[6],
                _cubeVertices[7],

                _cubeVertices[5],
                _cubeVertices[4],
                _cubeVertices[7],

                _cubeVertices[5],
                _cubeVertices[7],
                _cubeVertices[6],
        };
            return new Mesh(vertices);
        }

        public static Mesh Sphere(int rows, int columns) {
            int verticesNumber = (rows + 1) * (columns + 1);

            int triangleNumber = rows * columns * 2;

            var triangleVertices = new List<Point3D>();

            float cellRow = 2 * MathF.PI / rows;
            float cellColumn = MathF.PI / columns;

            for (int c = 0; c < columns; c++) {
                var teta = c * cellColumn;
                for (int r = 0; r < rows; r++) {
                    var psi = r * cellRow;

                    Point3D firstTop = new Point3D(MathF.Sin(teta) * MathF.Cos(psi), MathF.Sin(teta) * MathF.Sin(psi), MathF.Cos(teta));
                    Point3D secondTop = new Point3D(MathF.Sin(teta) * MathF.Cos(psi + cellRow), MathF.Sin(teta) * MathF.Sin(psi + cellRow), MathF.Cos(teta));
                    Point3D firstBottom = new Point3D(MathF.Sin(teta + cellColumn) * MathF.Cos(psi), MathF.Sin(teta + cellColumn) * MathF.Sin(psi), MathF.Cos(teta + cellColumn));
                    Point3D secondBottom = new Point3D(MathF.Sin(teta + cellColumn) * MathF.Cos(psi + cellRow), MathF.Sin(teta + cellColumn) * MathF.Sin(psi + cellRow), MathF.Cos(teta + cellColumn));

                    triangleVertices.Add(secondTop);
                    triangleVertices.Add(firstTop);
                    triangleVertices.Add(firstBottom);

                    triangleVertices.Add(secondBottom);
                    triangleVertices.Add(secondTop);
                    triangleVertices.Add(firstBottom);


                }
            }

            return new Mesh(triangleVertices);


        }






    }
}

