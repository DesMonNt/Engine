namespace Engine.MathExtensions;

public class Matrix3X3Functions
{
    public static double Determinant(Matrix3X3 matrix)
        => matrix[0, 0] * (matrix[1, 1] * matrix[2, 2] - matrix[2, 1] * matrix[1, 2])
               - matrix[0, 1] * (matrix[1, 0] * matrix[2, 2] - matrix[2, 0] * matrix[1, 2])
               + matrix[0, 2] * (matrix[1, 0] * matrix[2, 1] - matrix[2, 0] * matrix[1, 1]);
    
    public Matrix3X3? Inverse(Matrix3X3 matrix)
    {
        var determinant = Determinant(matrix);

        if (determinant == 0)
            return null;

        var result = new double[3, 3];

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                var minor = Minor(matrix,i, j);
                
                result[j, i] = minor / determinant;
            }
        }

        return new Matrix3X3(result);
    }
    
    public static double Minor(Matrix3X3 matrix, int row, int col)
    {
        var minor = new double[2, 2];
        var minorRow = 0; 
        var minorCol = 0;

        for (var i = 0; i < 3; i++)
        {
            if (i == row)
                continue;

            minorCol = 0;

            for (var j = 0; j < 3; j++)
            {
                if (j == col)
                    continue;

                minor[minorRow, minorCol] = matrix[i, j];
                minorCol++;
            }

            minorRow++;
        }

        return minor[0, 0] * minor[1, 1] - minor[0, 1] * minor[1, 0];
    }
}