using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;

public class main : MonoBehaviour
{
    void Start()
    {
        // Создаем вектор-столбец
        Vector<double> vectorColumn = Vector<double>.Build.Dense(new double[] { 1, 2, 3 });

        // Создаем вектор-строку
        Vector<double> vectorRow = Vector<double>.Build.Dense(new double[] { 1, 2, 3 });

        // Перемножаем вектор-столбец на вектор-строку
        Matrix<double> resultMatrix = vectorColumn.ToColumnMatrix() * vectorRow.ToRowMatrix();

        // Выводим результат в консоль Unity
        Debug.Log("Результат перемножения:");
        Debug.Log(resultMatrix); 
    }
}
