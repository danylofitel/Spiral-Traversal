using System;

namespace SpiralTraversal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const int n = 9;
            const int n2 = n * n;

            int[,] matrix = new int[n, n];

            SpiralTraversal spiral = new SpiralTraversal(n);

            for (int i = 1; i <= n2; ++i)
            {
                Tuple<int, int> coordinates = spiral.Coordinates(i);
                int index = spiral.Index(coordinates.Item1, coordinates.Item2);
                matrix[coordinates.Item1 - 1, coordinates.Item2 - 1] = index;
            }

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    Console.Write($"{matrix[i, j]}".PadLeft(n2.ToString().Length + 3, ' '));
                }

                Console.WriteLine("\n");
            }
        }
    }
}
