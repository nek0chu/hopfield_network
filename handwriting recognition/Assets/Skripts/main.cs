using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using System.Linq;
using System;
using MathNet.Numerics;

public class main : MonoBehaviour
{
    private List<Vector<double>> vectors = new List<Vector<double>>();

    void Start()
    {
        Main();
    }
    public void AddVector(Vector<double> newVector)
    {
        vectors.Add(newVector);
    }
    public  Vector<double> GetLastVector()
    {
        return vectors[vectors.Count - 1];
    }
    // Функция для расчета матрицы W
    public Matrix<double> CalculateWeightMatrix(Vector<double>[] trainingVectors)
    {
        // Проверка наличия хотя бы одного вектора
        if (trainingVectors.Length == 0)
        {
            throw new ArgumentException("Необходимо предоставить хотя бы один вектор для обучения.");
        }

        // Проверка, что все вектора имеют одинаковую длину
        int vectorSize = trainingVectors[0].Count;
        if (trainingVectors.Any(v => v.Count != vectorSize))
        {
            throw new ArgumentException("Все вектора должны иметь одинаковую длину.");
        }

        // Создание матрицы W размерности vectorSize x vectorSize, заполненной нулями
        Matrix<double> weightMatrix = Matrix<double>.Build.Dense(vectorSize, vectorSize);

        // Расчет матрицы W
        foreach (var vector in trainingVectors)
        {
            // Преобразование вектора в матрицу-столбец
            var columnMatrix = vector.ToColumnMatrix();

            // Обновление матрицы W
            weightMatrix = weightMatrix.Add(columnMatrix * columnMatrix.Transpose());
        }

        // Деление на количество векторов для нормализации
        weightMatrix = ZeroOutDiagonal(weightMatrix);
        weightMatrix = MultiplyMatrixByScalar(weightMatrix, 1 / (double)vectorSize);

        return weightMatrix;
    }
    public Matrix<double> MultiplyMatrixByScalar(Matrix<double> matrix, double scalar)
    {
        // Копирование матрицы для избежания изменения оригинала
        Matrix<double> resultMatrix = matrix.Clone();

        // Умножение каждого элемента матрицы на число
        resultMatrix = resultMatrix.Multiply(scalar);

        return resultMatrix;
    }
    public Matrix<double> ZeroOutDiagonal(Matrix<double> matrix)
    {
        // Проверка, что матрица квадратная
        if (matrix.RowCount != matrix.ColumnCount)
        {
            throw new ArgumentException("Матрица должна быть квадратной для обнуления диагонали.");
        }

        // Копирование матрицы для избежания изменения оригинала
        Matrix<double> resultMatrix = matrix.Clone();

        // Обнуление элементов на диагонали
        for (int i = 0; i < matrix.RowCount; i++)
        {
            resultMatrix[i, i] = 0.0;
        }

        return resultMatrix;
    }
    public Matrix<double> MultiplyMatrixByVector(Matrix<double> matrix, Vector<double> vector)
    {
        // Умножение матрицы на транспонированный вектор-столбец
        Vector<double> resultVector = matrix.Multiply(vector.ToColumnMatrix().Column(0));

        // Создание матрицы из вектора-столбца
        Matrix<double> resultMatrix = Matrix<double>.Build.DenseOfColumnVectors(resultVector);

        return resultMatrix;
    }


    public Vector<double> MultiplyMatrixByVector2(Matrix<double> matrix, Vector<double> vector)
    {
        // Умножение матрицы на вектор-столбец
        Vector<double> resultVector = matrix.Multiply(vector);

        // Можно также использовать оператор умножения:
        // Vector<double> resultVector = matrix * vector;

        return resultVector;
    }
    public Vector<double> SignedVectorElements(Vector<double> inputVector)
    {
        // Создаем копию вектора, чтобы не изменять оригинал
        Vector<double> modifiedVector = inputVector.Clone();

        // Проходим по каждому элементу вектора и меняем его в соответствии с условием
        for (int i = 0; i < modifiedVector.Count; i++)
        {
            modifiedVector[i] = (modifiedVector[i] >= 0) ? 1 : -1;
        }

        return modifiedVector;
    }
    public int IdentifyRecognizedPattern(Vector<double> recognizedVector, Vector<double>[] trainingVectors)
    {
        for (int i = 0; i < trainingVectors.Length; i++)
        {
            // Сравнение распознанного вектора с обучающими
            if (recognizedVector.Equals(trainingVectors[i]))
            {
                return i; // Возвращаем индекс распознанного вектора
            }
        }

        return -1; // Возвращаем -1, если ни один вектор не совпал
    }
    // Функция для поиска индекса наилучшего соответствия
    private int FindBestMatchIndex(Vector<double> targetVector, Vector<double>[] candidates)
    {
        double minHammingDistance = double.MaxValue;
        int bestMatchIndex = -1;

        for (int i = 0; i < candidates.Length; i++)
        {
            double hammingDistance = HammingDistance(targetVector, candidates[i]);
            if (hammingDistance < minHammingDistance)
            {
                minHammingDistance = hammingDistance;
                bestMatchIndex = i;
            }
            //Debug.Log("hamming distance "+ hammingDistance + " index " + bestMatchIndex);
        }

        Debug.Log("min hamming distance " + minHammingDistance + " beast match index " + bestMatchIndex);
        return bestMatchIndex;
    }

    // Функция для вычисления расстояния Хэмминга между векторами
    private double HammingDistance(Vector<double> vector1, Vector<double> vector2)
    {
        int differingBits = 0;

        for (int i = 0; i < vector1.Count; i++)
        {
            if (vector1[i] != vector2[i])
            {
                differingBits++;
            }
        }

        return differingBits;
    }
    // Пример использования
    public void HopfieldNeuralNetWork(Vector<double> inputVectorToRecognize)
    {
        Vector<double>[] trainingVectors = vectors.ToArray();
        // Рассчет матрицы W с обнулёнными диагоналями и нормализовнный
        Matrix<double> weightMatrix = CalculateWeightMatrix(trainingVectors);
        // Умножаем на транспонировнный вектор который хотим расопзнать
        weightMatrix = MultiplyMatrixByVector(weightMatrix, inputVectorToRecognize);
        //Делаем из матрицы вектор
        double[] columnMajorArray = weightMatrix.ToColumnMajorArray();
        Vector<double> columnVector = Vector<double>.Build.Dense(columnMajorArray);
        //Наш вектор прошедший через функцию sign
        columnVector = SignedVectorElements(columnVector);
        //Сравнение с векторами на которых обучали 
        int numberOfRecognizedVector = FindBestMatchIndex(columnVector, trainingVectors);
        inputVectorToRecognize = columnVector;
        // Вывод матрицы W
        Debug.Log("Порядковый номер вектора " + numberOfRecognizedVector);
        Debug.Log(inputVectorToRecognize);
    }
    public void Main()
    {
        // Пример обучающих векторов
        Vector<double>[] trainingVectors = new[]
        {
            Vector<double>.Build.DenseOfArray(new double[] { 1, 1, 1, 1, -1, 1, 1, -1, 1}),
            Vector<double>.Build.DenseOfArray(new double[] { 1, 1, 1, 1, -1, -1, 1, -1, -1}),
            Vector<double>.Build.DenseOfArray(new double[] { 1, -1, -1, 1, -1, -1, 1, 1, 1}),
            // Добавьте другие векторы по мере необходимости
        };
        Vector<double> inputVectorToRecognize = Vector<double>.Build.DenseOfArray(new double[] { -1, -1, -1, 1, -1, -1, 1, -1, -1 });
    }
}
