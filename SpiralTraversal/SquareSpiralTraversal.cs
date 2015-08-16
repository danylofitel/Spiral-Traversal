using System;
using System.Diagnostics;

namespace SpiralTraversal
{
    public class SquareSpiralTraversal
    {
        // Indicates whether size of the matrix is odd.
        private readonly bool Odd;

        // Size of the square matrix.
        private readonly int N;

        // Number of cells in the matrix, used for performance reasons.
        private readonly int N2;

        // Number of loops in the traversal.
        private readonly int L;

        // Constructor from matrix size, size has to be a natural number.
        public SquareSpiralTraversal(int n)
        {
            if (n < 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            Odd = n % 2 == 1;
            N = n;
            N2 = n * n;
            L = LoopCount(n);
        }

        // Returns index of the cell with given coordinates in the traversal.
        public int Index(int i, int j)
        {
            if (i < 1 || i > N || j < 1 || j > N)
            {
                throw new ArgumentOutOfRangeException();
            }

            int currentLoopNumber = LoopNumber(i, j);

            int blockSize = LoopBlockSize(currentLoopNumber);

            int cellsInPreviousLoops = CellsInFirstLoops(currentLoopNumber - 1);

            int result;
            if (i == currentLoopNumber)
            {
                result = cellsInPreviousLoops + j - currentLoopNumber + 1;
            }
            else if (j == N - currentLoopNumber + 1)
            {
                result = cellsInPreviousLoops + blockSize + i - currentLoopNumber + 1;
            }
            else if (i == N - currentLoopNumber + 1)
            {
                result = cellsInPreviousLoops + 2 * blockSize + N - j - currentLoopNumber + 2;
            }
            else
            {
                result = cellsInPreviousLoops + 3 * blockSize + N - i - currentLoopNumber + 2;
            }

            Debug.Assert(result >= 1 && result <= N2);

            return result;
        }

        // Returns coordinates of cell of given order in the traversal.
        public Tuple<int, int> Coordinates(int k)
        {
            if (k < 1 || k > N2)
            {
                throw new ArgumentException();
            }

            int currentLoopNumber = LoopNumber(k);

            int cellInCurrentLoop = k - CellsInFirstLoops(currentLoopNumber - 1);

            Tuple<int, int> blockPosition = PositionInCurrentLoop(currentLoopNumber, cellInCurrentLoop);

            switch (blockPosition.Item1)
            {
                case 1:
                    return new Tuple<int, int>(
                        currentLoopNumber,
                        currentLoopNumber + blockPosition.Item2 - 1);
                case 2:
                    return new Tuple<int, int>(
                        currentLoopNumber + blockPosition.Item2 - 1,
                        N - currentLoopNumber + 1);
                case 3:
                    return new Tuple<int, int>(
                        N - currentLoopNumber + 1,
                        N - currentLoopNumber - blockPosition.Item2 + 2);
                case 4:
                default:
                    return new Tuple<int, int>(
                        N - currentLoopNumber - blockPosition.Item2 + 2,
                        currentLoopNumber);
            }
        }

        // Number of loops in the spiral of given size.
        private static int LoopCount(int n)
        {
            return n / 2 + n % 2;
        }

        // Loop number for given cell number.
        private int LoopNumber(int k)
        {
            Debug.Assert(k >= 1 && k <= N2);

            int result = L - ((int)Math.Floor(Math.Sqrt(N2 - k)) + (Odd ? 1 : 0)) / 2;

            Debug.Assert(result > 0);

            return result;
        }

        // Loop number for given cell coordinates.
        private int LoopNumber(int i, int j)
        {
            Debug.Assert(
                i >= 1 && i <= N &&
                j >= 1 && j <= N);

            return Math.Min(i, Math.Min(j, Math.Min(N - i + 1, N - j + 1)));
        }

        // The number of cells in the first given loops.
        private int CellsInFirstLoops(int loops)
        {
            Debug.Assert(loops >= 0 && loops <= L);

            if (Odd && loops == L)
            {
                return N2;
            }

            return 4 * loops * (N - loops);
        }

        // Size of the block equal to a quarter of the loop.
        private int LoopBlockSize(int loop)
        {
            Debug.Assert(loop >= 1 && loop <= L);

            return N - 2 * loop + 1;
        }

        // Index of the loop block and position in the block.
        private Tuple<int, int> PositionInCurrentLoop(int loop, int cellInLoop)
        {
            int blockSize = LoopBlockSize(loop);

            if (blockSize == 0)
            {
                // It can only happen for the last loop of spiral of odd size
                // containing 1 cell.
                Debug.Assert(Odd);
                Debug.Assert(loop == L);
                Debug.Assert(cellInLoop == 1);

                return new Tuple<int, int>(1, 1);
            }

            Debug.Assert(cellInLoop >= 1 && cellInLoop <= 4 * blockSize);

            Tuple<int, int> result =  new Tuple<int, int>(
                (cellInLoop - 1) / blockSize + 1, (cellInLoop - 1) % blockSize + 1);

            Debug.Assert(
                result.Item1 >= 1 && result.Item1 <= 4 &&
                result.Item2 >= 1 && result.Item2 <= blockSize);

            return result;
        }
    }
}
