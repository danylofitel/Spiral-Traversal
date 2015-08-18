using System;

namespace SpiralTraversal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            for (int m = 1; m <= 10; ++m)
            {
                for (int n = 1; n <= 10; ++n)
                {
                    RectangularDemo(m, n);
                    if (m == n)
                    {
                        SquareDemo(m);
                    }
                }
            }
        }

        private static void RectangularDemo(int m, int n)
        {
            int mn = m * n;

            RectangularSpiralTraversal rst = new RectangularSpiralTraversal(m, n);
            int[,] rectangularMatrix = new int[m, n];

            for (int i = 1; i <= mn; ++i)
            {
                Tuple<int, int> coordinates = rst.Coordinates(i);
                Console.Write($"[{coordinates.Item1}; {coordinates.Item2}] -> ");

                int index = rst.Index(coordinates.Item1, coordinates.Item2);
                if (index != i)
                {
                    throw new Exception($"Retrieved index {index} is not equal to actual index {i}.");
                }

                rectangularMatrix[coordinates.Item1 - 1, coordinates.Item2 - 1] = index;
            }

            Console.WriteLine("\n");

            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    Console.Write($"{rectangularMatrix[i, j]}".PadLeft(mn.ToString().Length + 3, ' '));
                }

                Console.WriteLine("\n");
            }

            Console.WriteLine("\n");
        }

        private static void SquareDemo(int m)
        {
            int m2 = m * m;

            SquareSpiralTraversal sst = new SquareSpiralTraversal(m);
            int[,] squareMatrix = new int[m, m];

            for (int i = 1; i <= m2; ++i)
            {
                Tuple<int, int> coordinates = sst.Coordinates(i);
                Console.Write($"[{coordinates.Item1}; {coordinates.Item2}] -> ");

                int index = sst.Index(coordinates.Item1, coordinates.Item2);
                if (index != i)
                {
                    throw new Exception($"Retrieved index {index} is not equal to actual index {i}.");
                }

                squareMatrix[coordinates.Item1 - 1, coordinates.Item2 - 1] = index;
            }

            Console.WriteLine("\n");

            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    Console.Write($"{squareMatrix[i, j]}".PadLeft(m2.ToString().Length + 3, ' '));
                }

                Console.WriteLine("\n");
            }
        }
    }
}
