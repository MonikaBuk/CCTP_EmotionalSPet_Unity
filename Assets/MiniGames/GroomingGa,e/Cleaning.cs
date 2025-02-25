using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.UI.VirtualMouseInput;


public class Cleaning : MonoBehaviour
{
    [SerializeField] private Texture2D dirtMaskTextureBase;
    [SerializeField] private Texture2D dirtBrush;
    [SerializeField] private Material material;
    [SerializeField] private TextMeshProUGUI uiText;
    [SerializeField] private Texture2D cursorImage;

    private Texture2D dirtMaskTexture;
    private float dirtAmountTotal;
    private float dirtAmount;
    private Vector2Int lastPaintPixelPosition;
    public UnityEngine.CursorMode cursorMode = UnityEngine.CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    private AudioSource brushAudioSource;

    private void Awake()
    {
     
        dirtMaskTexture = new Texture2D(dirtMaskTextureBase.width, dirtMaskTextureBase.height, TextureFormat.RGBA32, false);
        dirtMaskTexture.SetPixels(dirtMaskTextureBase.GetPixels());
        dirtMaskTexture.Apply();

        material.SetTexture("_DirtMask", dirtMaskTexture);
        material.SetFloat("_Dirtiness", 1);


        dirtAmountTotal = 0f;
        for (int x = 0; x < dirtMaskTexture.width; x++)
        {
            for (int y = 0; y < dirtMaskTexture.height; y++)
            {
                dirtAmountTotal += dirtMaskTexture.GetPixel(x, y).g; 
            }
        }
        dirtAmount = dirtAmountTotal;
        StartCoroutine(UpdateDirtPercentageCoroutine());
    }

    private void Start()
    {
        uiText.text = " 0% Clean";
        brushAudioSource = GetComponent<AudioSource>();
        Cursor.SetCursor(cursorImage, hotSpot, cursorMode);
    }

    void Update()
    {
        int brushSize = 100; // Increase this for a bigger brush
        float scaleFactor = brushSize / (float)dirtBrush.width;

        if (Mouse.current.leftButton.isPressed)
        {

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                if (!brushAudioSource.isPlaying)
                {
                    brushAudioSource.Play();
                }
                Vector2 textureCoord = raycastHit.textureCoord;
                int pixelX = (int)(textureCoord.x * dirtMaskTexture.width);
                int pixelY = (int)(textureCoord.y * dirtMaskTexture.height);

                Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);
                int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) +
                                         Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
                int maxPaintDistance = 7;

                if (paintPixelDistance < maxPaintDistance)
                    return;

                lastPaintPixelPosition = paintPixelPosition;

                int pixelXOffset = pixelX - (brushSize / 2);
                int pixelYOffset = pixelY - (brushSize / 2);

                for (int x = 0; x < brushSize; x++)
                {
                    for (int y = 0; y < brushSize; y++)
                    {
                        int brushX = (int)(x / scaleFactor);
                        int brushY = (int)(y / scaleFactor);

                        if (brushX < 0 || brushX >= dirtBrush.width || brushY < 0 || brushY >= dirtBrush.height)
                            continue;

                        float distance = Vector2.Distance(new Vector2(x, y), new Vector2(brushSize / 2, brushSize / 2));
                        if (distance > brushSize / 2) continue;
                        float falloff = 1 - (distance / (brushSize / 2));

                        Color pixelDirt = dirtBrush.GetPixel(brushX, brushY);
                        int pixelPosX = Mathf.Clamp(pixelXOffset + x, 0, dirtMaskTexture.width - 1);
                        int pixelPosY = Mathf.Clamp(pixelYOffset + y, 0, dirtMaskTexture.height - 1);

                        Color pixelDirtMask = dirtMaskTexture.GetPixel(pixelPosX, pixelPosY);

                        float removedAmount = pixelDirtMask.g - (pixelDirtMask.g * pixelDirt.g * falloff);
                        dirtAmount -= removedAmount;

                        dirtMaskTexture.SetPixel(
                            pixelPosX,
                            pixelPosY,
                            new Color(0, pixelDirtMask.g * pixelDirt.g * falloff, 0)
                        );
                    }
                }


                dirtMaskTexture.Apply();
            }
            else
            {
                if (brushAudioSource.isPlaying)
                    brushAudioSource.Stop();
            }
        }
        else
        {
           
            if (brushAudioSource.isPlaying)
                brushAudioSource.Stop();
        }
    }
    




    private IEnumerator UpdateDirtPercentageCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Color[] maskPixels = dirtMaskTexture.GetPixels();

            float currentDirtAmount = 100f;

            foreach (Color maskPixel in maskPixels)
            {
                currentDirtAmount += maskPixel.g;
            }

            float cleanPercentage = 1f - (currentDirtAmount / dirtAmountTotal);
            int precentage = Mathf.RoundToInt(cleanPercentage * 4f * 100f);

           

            if (precentage >= 100)
            {
                uiText.text = "Well done";
                material.SetFloat("_Dirtiness", 0);
                PetStats.wasCleaned = true;
                PlayerStats.AddMoney(5);
                ChangeBackCursor();
                SceneManager.LoadScene("PetScene");
            }
            else
            {
                uiText.text = precentage + "% Clean";
            }
        }
    }

    private void OnDestroy()
    {
        StopCoroutine(UpdateDirtPercentageCoroutine());
    }
    public void ChangeBackCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);

    }
}

