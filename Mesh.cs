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


        public static Mesh Plane(int rows, int columns) {
            int verticesNumber = (rows + 1) * (columns + 1);
            int triangleNumber = rows * columns * 2;
            int triangleVerticesNumber = triangleNumber * 3;
            // List is like a array
            var triangleVertices = new Point3D[triangleVerticesNumber];

            int i = 0;
            float cellRow = 1f / rows;
            float cellColumn = 1f / columns;
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
                    triangleVertices[i + 4] = RightBottom;
                    triangleVertices[i + 5] = LeftBottom;

                    i += 6;

                    
                }
            }

            return new Mesh(triangleVertices);

        }

        private static Point3D[] CubeVertices() {

            var result = new Point3D[] {
            new Point3D(-0.5f, 0.5f, -0.5f),
            new Point3D(0.5f, 0.5f, -0.5f),
            new Point3D(0.5f, -0.5f, -0.5f),
            new Point3D(-0.5f, -0.5f, -0.5f),
        
            new Point3D(-0.5f, 0.5f, 0.5f),
            new Point3D(0.5f, 0.5f, 0.5f),
            new Point3D(0.5f, -0.5f, 0.5f),
            new Point3D(-0.5f, -0.5f, 0.5f),
        };
            return result;

        }

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

                CubeVertices()[0],
                CubeVertices()[1],
                CubeVertices()[2],

                CubeVertices()[0],
                CubeVertices()[2],
                CubeVertices()[3],

                CubeVertices()[4],
                CubeVertices()[5],
                CubeVertices()[1],

                CubeVertices()[4],
                CubeVertices()[1],
                CubeVertices()[0],

                CubeVertices()[4],
                CubeVertices()[0],
                CubeVertices()[3],

                CubeVertices()[4],
                CubeVertices()[3],
                CubeVertices()[7],

                CubeVertices()[1],
                CubeVertices()[5],
                CubeVertices()[6],

                CubeVertices()[1],
                CubeVertices()[6],
                CubeVertices()[2],

                CubeVertices()[3],
                CubeVertices()[2],
                CubeVertices()[6],

                CubeVertices()[3],
                CubeVertices()[6],
                CubeVertices()[7],

                CubeVertices()[5],
                CubeVertices()[4],
                CubeVertices()[7],

                CubeVertices()[5],
                CubeVertices()[7],
                CubeVertices()[6],
        };
            return new Mesh(vertices);
        }

        public static Mesh Sphere(int rows, int columns) {
            int verticesNumber = (rows + 1) * (columns + 1);

            int triangleNumber = rows * columns * 2;

            var triangleVertices = new List<Point3D>();

            float cellRow = 2 * MathF.PI / rows;
            float cellColumn = MathF.PI / columns;

            float psi = 0f;
            float teta = 0f;
            

            for (int c = 0; c < columns; c++) {
                teta = c*cellColumn;
                for (int r = 0; r < rows; r++) {
                    psi = r*cellRow;

                    Point3D firstTop = new Point3D(MathF.Sin(teta) * MathF.Cos(psi), MathF.Sin(teta) * MathF.Sin(psi), MathF.Cos(teta));
                    Point3D secondTop = new Point3D(MathF.Sin(teta) * MathF.Cos(psi + cellRow), MathF.Sin(teta) * MathF.Sin(psi + cellRow), MathF.Cos(teta));
                    Point3D firstBottom = new Point3D(MathF.Sin(teta + cellColumn) * MathF.Cos(psi), MathF.Sin(teta + cellColumn) * MathF.Sin(psi), MathF.Cos(teta + cellColumn));
                    Point3D secondBottom = new Point3D(MathF.Sin(teta + cellColumn) * MathF.Cos(psi + cellRow), MathF.Sin(teta + cellColumn) * MathF.Sin(psi + cellRow), MathF.Cos(teta + cellColumn));

                    triangleVertices.Add(firstTop);
                    triangleVertices.Add(secondTop);
                    triangleVertices.Add(firstBottom);

                    triangleVertices.Add(secondTop);
                    triangleVertices.Add(secondBottom);
                    triangleVertices.Add(firstBottom);
                    

                }
            }

            return new Mesh(triangleVertices);


        }

        




    }
}

