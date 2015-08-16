using System;
using System.Diagnostics;

namespace SpiralTraversal
{
    public class RectangularSpiralTraversal
    {
        // Indicates whether size of the matrix is odd.
        private readonly bool Odd;

        // Length of the matrix.
        private readonly int M;

        // Width of the matrix.
        private readonly int N;

        // Delta between length and width.
        private readonly int D;

        // Number of cells in the matrix, used for performance reasons.
        private readonly int MN;

        // Number of loops in the traversal.
        private readonly int L;

        // Constructor from matrix size, size has to be a natural number.
        public RectangularSpiralTraversal(int m, int n)
        {
            if (m < 1 || n < 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            Odd = Math.Min(m, n) % 2 == 1;
            M = m;
            N = n;
            D = Math.Abs(m - n);
            MN = m * n;
            L = LoopCount(m, n);
        }

        // Returns index of the cell with given coordinates in the traversal.
        public int Index(int i, int j)
        {
            if (i < 1 || i > M || j < 1 || j > N)
            {
                throw new ArgumentOutOfRangeException();
            }

            int currentLoopNumber = LoopNumber(i, j);

            Tuple<int, int> blockSize = LoopBlockSize(currentLoopNumber);

            int cellsInPreviousLoops = CellsInFirstLoops(currentLoopNumber - 1);

            int result = cellsInPreviousLoops;
            if (i == currentLoopNumber)
            {
                result += j - currentLoopNumber + 1;
            }
            else if (j == N - currentLoopNumber + 1)
            {
                result += blockSize.Item2 + i - currentLoopNumber + 1;
            }
            else if (i == M - currentLoopNumber + 1)
            {
                result += blockSize.Item2 + blockSize.Item1 + N - j - currentLoopNumber + 2;
            }
            else
            {
                result += 2 * blockSize.Item2 + blockSize.Item1 + M - i - currentLoopNumber + 2;
            }

            Debug.Assert(result >= 1 && result <= MN);

            return result;
        }

        // Returns coordinates of cell of given order in the traversal.
        public Tuple<int, int> Coordinates(int k)
        {
            if (k < 1 || k > MN)
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
                        M - currentLoopNumber + 1,
                        N - currentLoopNumber - blockPosition.Item2 + 2);
                case 4:
                default:
                    return new Tuple<int, int>(
                        M - currentLoopNumber - blockPosition.Item2 + 2,
                        currentLoopNumber);
            }
        }

        // Number of loops in the spiral of given size.
        private static int LoopCount(int m, int n)
        {
            int minDimension = Math.Min(m, n);
            return minDimension / 2 + minDimension % 2;
        }

        // Loop number for given cell number.
        public int LoopNumber(int k)
        {
            Debug.Assert(k >= 1 && k <= MN);

            int result = L - (int)Math.Floor(
                0.5 * Math.Sqrt(MN - k + D * D / 4.0) - (M + N - 4 * L) / 4.0);

            Debug.Assert(result > 0);

            return result;
        }

        // Loop number for given cell coordinates.
        private int LoopNumber(int i, int j)
        {
            Debug.Assert(
                i >= 1 && i <= M &&
                j >= 1 && j <= N);

            int result = Math.Min(i, Math.Min(j, Math.Min(M - i + 1, N - j + 1)));

            Debug.Assert(result > 0 && result <= L);

            return result;
        }

        // The number of cells in the first given loops.
        private int CellsInFirstLoops(int loops)
        {
            Debug.Assert(loops >= 0 && loops <= L);

            if (loops == L)
            {
                return MN;
            }

            int l = M - 2 * loops;
            int w = N - 2 * loops;

            return MN - l * w;
        }

        // Size (length and width resp.) of the block of the loop.
        // I.e. Item1 is vertical size, Item2 is horizontal size.
        private Tuple<int, int> LoopBlockSize(int loop)
        {
            Debug.Assert(loop >= 1 && loop <= L);

            return new Tuple<int, int>(M - 2 * loop + 1, N - 2 * loop + 1);
        }

        // Index of the loop block and position in the block.
        private Tuple<int, int> PositionInCurrentLoop(int loop, int cellInLoop)
        {
            Tuple<int, int> blockSize = LoopBlockSize(loop);

            if (blockSize.Item1 + blockSize.Item2 == 0)
            {
                // It can only happen for the last loop of spiral of square matrix of odd size
                // containing one cell.
                Debug.Assert(Odd);
                Debug.Assert(loop == L);
                Debug.Assert(cellInLoop == 1);

                return new Tuple<int, int>(1, cellInLoop);
            }
            else if (blockSize.Item1 == 0) // blockSize.Item2 != 0
            {
                // It can only happen for the last horizontal loop of spiral of odd size.
                Debug.Assert(Odd);
                Debug.Assert(loop == L);

                return new Tuple<int, int>(1, cellInLoop);
            }
            else if (blockSize.Item2 == 0) // blockSize.Item1 != 0
            {
                // It can only happen for the last vertical loop of spiral of odd size.
                Debug.Assert(Odd);
                Debug.Assert(loop == L);

                return new Tuple<int, int>(2, cellInLoop);
            }

            Debug.Assert(cellInLoop >= 1 && cellInLoop <= 2 * (blockSize.Item1 + blockSize.Item2));

            Tuple<int, int> result;
            if (cellInLoop <= blockSize.Item2)
            {
                result = new Tuple<int, int>(1, cellInLoop);
            }
            else if (cellInLoop <= blockSize.Item2 + blockSize.Item1)
            {
                result = new Tuple<int, int>(2, cellInLoop - blockSize.Item2);
            }
            else if (cellInLoop <= 2 * blockSize.Item2 + blockSize.Item1)
            {
                result = new Tuple<int, int>(3, cellInLoop - blockSize.Item2 - blockSize.Item1);
            }
            else
            {
                result = new Tuple<int, int>(4, cellInLoop - 2 * blockSize.Item2 - blockSize.Item1);
            }

            Debug.Assert(
                result.Item1 >= 1 && result.Item1 <= 4 &&
                result.Item2 >= 1 && result.Item2 <= Math.Max(blockSize.Item1, blockSize.Item2));

            return result;
        }
    }
}
