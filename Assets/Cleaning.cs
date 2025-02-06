using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class Cleaning : MonoBehaviour
{
    [SerializeField] private Texture2D dirtMaskTextureBase;
    [SerializeField] private Texture2D dirtBrush;
    [SerializeField] private Material material;
    [SerializeField] private TextMeshProUGUI uiText;

    private Texture2D dirtMaskTexture;
    private float dirtAmountTotal;
    private float dirtAmount;
    private Vector2Int lastPaintPixelPosition;

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

    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red); 

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                Vector2 textureCoord = raycastHit.textureCoord;
                int pixelX = (int)(textureCoord.x * dirtMaskTexture.width);
                int pixelY = (int)(textureCoord.y * dirtMaskTexture.height);

                Vector2Int paintPixelPosition = new Vector2Int(pixelX, pixelY);
                int paintPixelDistance = Mathf.Abs(paintPixelPosition.x - lastPaintPixelPosition.x) +
                                         Mathf.Abs(paintPixelPosition.y - lastPaintPixelPosition.y);
                int maxPaintDistance = 40;

                if (paintPixelDistance < maxPaintDistance)
                    return;

                lastPaintPixelPosition = paintPixelPosition;

                // Paint Square in Dirt Mask
                int squareSize = 200;
                int pixelXOffset = pixelX - (dirtBrush.width / 2);
                int pixelYOffset = pixelY - (dirtBrush.height / 2);

                for (int x = 0; x < squareSize; x++)
                {
                    for (int y = 0; y < squareSize; y++)
                    {
                        dirtMaskTexture.SetPixel(
                            pixelXOffset + x,
                            pixelYOffset + y,
                            Color.black
                        );
                    }
                }

                dirtMaskTexture.Apply();
            }
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
            int precentage = Mathf.RoundToInt(cleanPercentage * 2.3f * 100f);

           

            if (precentage >= 100)
            {
                uiText.text = "Well done";
                material.SetFloat("_Dirtiness", 0);
                PetStats.Instance.CleanPet();
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
}

