namespace Advent2020.Utils
{
    public static class Matrix
    {
        public static T[][] Transpose<T>(T[][] matrix)
        {
            int dim = matrix.Length;
            var result = new T[dim][];
            for (int row = 0; row < dim; row++)
            {
                result[row] = new T[dim];
                for (int col = 0; col < dim; col++)
                {
                    result[row][col] = matrix[col][row];
                }
            }

            return result;
        }

        public static void ReverseRows<T>(T[][] matrix)
        {
            int dim = matrix.Length;
            foreach (var row in matrix)
            {
                for (int col = 0; col < dim / 2; col++)
                {
                    var temp = row[col];
                    row[col] = row[dim - col - 1];
                    row[dim - col - 1] = temp;
                }
            }
        }
    }
}
