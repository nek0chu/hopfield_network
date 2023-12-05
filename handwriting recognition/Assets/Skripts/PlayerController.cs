using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Controls controls;
    private InputAction press;
    private InputAction pressPosition;
    private bool isPressed = false;
    [Range(2, 512)]
    [SerializeField] private int _textureSize = 28;
    [SerializeField] private TextureWrapMode _textureWrapMode;
    [SerializeField] private FilterMode _filterMode;
    [SerializeField] private Texture2D _texture;
    [SerializeField] private Material _material;

    [SerializeField] private Camera _camera;
    [SerializeField] private Collider _collider;
    [SerializeField] private Color _color;
    [SerializeField] private int _brushSize = 8;

    private int _oldRayX, _oldRayY;
    private void Awake() 
    {
        controls = new Controls();
        press = controls.painting.Press;
        pressPosition = controls.painting.Position;
        EnableControls();
    }
    public void EnableControls()
    {
        press.started += Press_started;
        press.canceled += Press_canceled;
    }
    public void DisableControls()
    {
        press.started -= Press_started;
        press.canceled -= Press_canceled;
    }
    private void Press_started(InputAction.CallbackContext obj)
    {
        isPressed = true;
        //StartCoroutine(onPressing());
        //Debug.Log("Press started");
    }
    private void Press_canceled(InputAction.CallbackContext obj)
    {
        isPressed = false;
        //StopCoroutine(onPressing());
        //Debug.Log("Press canceled");
    }
    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    private void Update()
    {
        if (isPressed)
        {
            Ray ray = _camera.ScreenPointToRay(pressPosition.ReadValue<Vector2>());

            RaycastHit hit;
            if (_collider.Raycast(ray, out hit, 100f))
            {

                int rayX = (int)(hit.textureCoord.x * _textureSize);
                int rayY = (int)(hit.textureCoord.y * _textureSize);

                if (_oldRayX != rayX || _oldRayY != rayY)
                {
                    DrawQuad(rayX, rayY);
                    //DrawCircle(rayX, rayY);
                    _oldRayX = rayX;
                    _oldRayY = rayY;
                }
                _texture.Apply();
            }
        }
    }
    IEnumerator onPressing()
    {
        while (isPressed)
        {
            Ray ray = _camera.ScreenPointToRay(pressPosition.ReadValue<Vector2>());

            RaycastHit hit;
            if (_collider.Raycast(ray, out hit, 100f))
            {

                int rayX = (int)(hit.textureCoord.x * _textureSize);
                int rayY = (int)(hit.textureCoord.y * _textureSize);

                if (_oldRayX != rayX || _oldRayY != rayY)
                {
                    DrawQuad(rayX, rayY);
                    //DrawCircle(rayX, rayY);
                    _oldRayX = rayX;
                    _oldRayY = rayY;
                }
                _texture.Apply();
            }
            //Debug.Log("kefefjeij");
            yield return null;
        }
    }
    public void SetTexture(Texture2D setTexture)
    {
        _texture = setTexture;
        _texture.Apply();
        OnValidate();
    }
    public Texture2D GetTexture()
    {
        return _texture;
    }
    public Material GetMaterial()
    {
        return _material;
    }
    public TextureWrapMode GetWrapMode()
    {
        return _textureWrapMode;
    }
    public FilterMode GetFilterMode()
    {
        return _filterMode;
    }
    public int GetTextureSize()
    {
        return _textureSize;
    }
    public void FillTextureWithWhite()
    {
        Color[] pixels = new Color[_textureSize * _textureSize];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.white;
        }

        _texture.SetPixels(pixels);
        _texture.Apply();
    }

    void OnValidate()
    {
        if (_texture == null)
        {
            _texture = new Texture2D(_textureSize, _textureSize);
        }
        if (_texture.width != _textureSize)
        {
            //_texture.Resize(_textureSize, _textureSize);
            _texture.Reinitialize(_textureSize, _textureSize);
        }
        _texture.wrapMode = _textureWrapMode;
        _texture.filterMode = _filterMode;
        _material.mainTexture = _texture;
        _texture.Apply();
    }

    void DrawQuad(int rayX, int rayY)
    {
        for (int y = 0; y < _brushSize; y++)
        {
            for (int x = 0; x < _brushSize; x++)
            {
                _texture.SetPixel(rayX + x - _brushSize / 2, rayY + y - _brushSize / 2, _color);
            }
        }
    }

    void DrawCircle(int rayX, int rayY)
    {
        for (int y = 0; y < _brushSize; y++)
        {
            for (int x = 0; x < _brushSize; x++)
            {

                float x2 = Mathf.Pow(x - _brushSize / 2, 2);
                float y2 = Mathf.Pow(y - _brushSize / 2, 2);
                float r2 = Mathf.Pow(_brushSize / 2 - 0.5f, 2);

                if (x2 + y2 < r2)
                {
                    int pixelX = rayX + x - _brushSize / 2;
                    int pixelY = rayY + y - _brushSize / 2;

                    if (pixelX >= 0 && pixelX < _textureSize && pixelY >= 0 && pixelY < _textureSize)
                    {
                        Color oldColor = _texture.GetPixel(pixelX, pixelY);
                        Color resultColor = Color.Lerp(oldColor, _color, _color.a);
                        _texture.SetPixel(pixelX, pixelY, resultColor);
                    }

                }


            }
        }
    }
}
