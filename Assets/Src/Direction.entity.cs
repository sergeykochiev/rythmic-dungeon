using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Direction : MonoBehaviour
{
    public SpriteRenderer arrowRenderer;

    public void SetArrowColor(Color arrowColor)
    {
        arrowRenderer.color = arrowColor;
    }
    
    public void HideSprite()
    {
        arrowRenderer.sortingOrder = 0;
    }

    public void ShowSprite()
    {
        arrowRenderer.sortingOrder = 1;
    }

    public void RotateDirection(Vector2? direction)
    {
        if (!direction.HasValue) return;
        ShowSprite();
        int x = (int)direction.Value.x;
        int y = (int)direction.Value.y;
        float angle = 180;
        if (!(y == 0 && x == -1))
        {
            angle = y * 90 + x * y * -45;
        }
        transform.rotation = Quaternion.Euler(0,0,angle);
    }

    void Start()
    {
        arrowRenderer = GetComponent<SpriteRenderer>();
    } 
}