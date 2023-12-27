namespace Engine.MathExtensions;

public class Matrix3X3
{
    private readonly double[,] _data;
    
    public double this[int row, int col]
    {
        get => _data[row, col];
        set => _data[row, col] = value;
    }
    public Matrix3X3(double[,] data)
    {
        if (data.GetLength(0) != 3 || data.GetLength(1) != 3)
            throw new ArgumentException();

        _data = data;
    }
    
    public Matrix3X3(
        double a11, double a12, double a13, 
        double a21, double a22, double a23,
        double a31, double a32, double a33)
    {
        _data = new[,]
        {
            { a11, a12, a13 },
            { a21, a22, a23 },
            { a31, a32, a33 }
        };
    }

    public static Matrix3X3 operator +(Matrix3X3 left, Matrix3X3 right)
    {
        var result = new double[3, 3];

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
                result[i, j] = left[i, j] + right[i, j]; 
        }

        return new Matrix3X3(result);
    }
    
    public static Matrix3X3 operator -(Matrix3X3 matrix)
    {
        var result = new double[3, 3];

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
                result[i, j] = -matrix[i, j]; 
        }

        return new Matrix3X3(result);
    }

    public static Matrix3X3 operator *(Matrix3X3 left, Matrix3X3 right)
    {
        var result = new double[3, 3];

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                for (var k = 0; k < 3; k++)
                    result[i, j] += left[i, k] * right[k, j];
            }
        }

        return new Matrix3X3(result);
    }
    
    public static Matrix3X3 operator *(Matrix3X3 left, double right)
    {
        var result = new double[3, 3];
        
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
                result[i, j] = left[i, j] * right;
        }

        return new Matrix3X3(result);
    }

    public static Matrix3X3 operator *(double left, Matrix3X3 right) => right * left;
    
    public static Matrix3X3 operator -(Matrix3X3 left, Matrix3X3 right) => left + -right;

    public double Determinant() => Matrix3X3Functions.Determinant(this);
}