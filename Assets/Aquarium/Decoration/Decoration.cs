using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Decoration : MonoBehaviour
{
    public DecorationData data;
    private Vector2 originalPosition;
    private bool isDragging = false;
    private Collider2D decorationCollider;
    private float cameraWidth;
    public DecorationIcons ownButton;

    private void Start()
    {
        decorationCollider = GetComponent<Collider2D>();
        if (decorationCollider == null)
        {
            Debug.LogError("Decoration must have a Collider2D component!");
        }
        cameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
    }

    public void Initialize(DecorationData decorationData)
    {
        data = decorationData;
        GetComponent<SpriteRenderer>().sprite = data.decorSprite;
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnBeginDrag();
        }

        if (isDragging)
        {
            OnDrag();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            OnEndDrag();
        }
    }

    private void OnBeginDrag()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            isDragging = true;
            originalPosition = transform.position;
        }
    }

    private void OnDrag()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = worldPosition;
    }

    // End dragging
    private void OnEndDrag()
    {
        isDragging = false;

        if (transform.position.x + 0.5f >= cameraWidth || transform.position.x - 0.5f <= -cameraWidth)
        {
            if (AquariumManagger.Instance != null)
            {
                AquariumManagger.Instance.RemoveDecorationFromSave(data, transform.position);
            }

            AquariumManagger.Instance.SaveDecorations();
           
            Destroy(gameObject);
            return;
        }

        if (IsInsideAquarium(transform.position))
        {
            Debug.Log("Decoration placed correctly in the aquarium.");
        }
        else
        {
            transform.position = originalPosition;
            Debug.Log("Decoration returned to its original position.");
        }
    }

    private bool IsInsideAquarium(Vector2 position)
    {
        return position.y <= -1f;
    }

    private void OnDestroy()
    {
        ownButton.RefreshIcon();
    }
}

