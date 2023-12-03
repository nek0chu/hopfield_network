using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Material basedMaterial;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private main mainSkript;
    [SerializeField] private GameObject GridLayout; 
    [SerializeField] private GameObject imagesToTrainOn;
    [Header("Buttons")]
    [SerializeField] private Button openImagesToTrainButton;
    [SerializeField] private Button closeImagesToTrainButton;
    [SerializeField] private Button addNewTrainVectorButton;
    [SerializeField] private Button clearButton;
    [SerializeField] private Button checkCurrentImageButton;
    [SerializeField] private Button showOnQuadButton;
    void Start()
    {
        openImagesToTrainButton.onClick.AddListener(() => imagesToTrainOn.SetActive(true)); 
        closeImagesToTrainButton.onClick.AddListener(() => imagesToTrainOn.SetActive(false));
        addNewTrainVectorButton.onClick.AddListener(AddNewTrainVector);
        clearButton.onClick.AddListener(playerController.FillTextureWithWhite);
        checkCurrentImageButton.onClick.AddListener(CheckCurrentImage);
        showOnQuadButton.onClick.AddListener(ShowLastVector);
        playerController.FillTextureWithWhite();
    }
    public void ShowLastVector()
    {
        Texture2D newTexture = playerController.GetTexture();
        int size = playerController.GetTextureSize();
        newTexture = VectorToTexture(mainSkript.GetLastVector(), size, size);
        playerController.SetTexture(newTexture);
    }
    public void CheckCurrentImage()
    {
        // Получаем текущую текстуру
        Texture2D newTexture = playerController.GetTexture();

        // Ваши последующие действия
        Vector<double> newVector = TextureToVector(newTexture);
        mainSkript.HopfieldNeuralNetWork(newVector);
    }
    public void AddNewTrainVector()
    {
        // Получаем текущую текстуру
        Texture2D newTexture = playerController.GetTexture();

        // Создаем новый RawImage
        GameObject newRawImageObject = new GameObject("RawImage");
        RawImage rawImage = newRawImageObject.AddComponent<RawImage>();

        // Копируем текстуру, чтобы изменения не затрагивали оригинал
        Texture2D copiedTexture = new Texture2D(newTexture.width, newTexture.height);
        copiedTexture.SetPixels(newTexture.GetPixels());
        copiedTexture.filterMode = playerController.GetFilterMode();
        copiedTexture.wrapMode = playerController.GetWrapMode();
        copiedTexture.Apply();
        rawImage.material = basedMaterial;
        // Присваиваем текстуру RawImage
        rawImage.texture = copiedTexture;

        // Делаем новый RawImage дочерним для GridLayout
        newRawImageObject.transform.SetParent(GridLayout.transform, false);

        // Ваши последующие действия
        Vector<double> newVector = TextureToVector(newTexture);
        mainSkript.AddVector(newVector);
        playerController.FillTextureWithWhite();
    }

    public static Vector<double> TextureToVector(Texture2D texture)
    {
        // Получаем массив цветов пикселей
        Color[] pixels = texture.GetPixels();

        // Создаем вектор нужной длины
        Vector<double> resultVector = Vector<double>.Build.Dense(pixels.Length);

        // Проходим по каждому пикселю
        for (int i = 0; i < pixels.Length; i++)
        {
            // Если цвет пикселя белая  (или близок к чёрному)
            if (pixels[i].r > 0.95f && pixels[i].g > 0.95f && pixels[i].b > 0.95f)
            {
                // Присваиваем 1
                resultVector[i] = -1.0;
            }
            else
            {
                // Иначе присваиваем 0
                resultVector[i] = 1.0;
            }
        }

        return resultVector;
    }
    public static Texture2D VectorToTexture(Vector<double> vector, int width, int height)
    {
        // Создаем новую текстуру с указанными шириной и высотой
        Texture2D texture = new Texture2D(width, height);

        // Преобразуем вектор в массив значений
        double[] values = vector.ToArray();

        // Проходим по каждому пикселю текстуры
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Получаем индекс элемента вектора, соответствующего текущему пикселю
                int index = y * width + x;

                // Если значение вектора равно 1, устанавливаем черный цвет, иначе — белый
                Color pixelColor = (values[index] == 1.0) ? Color.black : Color.white;

                // Устанавливаем цвет пикселя в текстуре
                texture.SetPixel(x, y, pixelColor);
            }
        }

        // Применяем изменения к текстуре
        texture.Apply();

        return texture;
    }
}
