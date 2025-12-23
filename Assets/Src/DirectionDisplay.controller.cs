using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DirectionDisplayController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite discreteDirection;
    public Sprite diagonalDirection;
    public Sprite noDirection;

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    private void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }

    public void SetNoDirection()
    {
        SetSprite(noDirection);
    }

    public void HideSprite()
    {
        spriteRenderer.sortingOrder = 0;
    }

    public void ShowSprite()
    {
        spriteRenderer.sortingOrder = 1;
    }

    private void SwitchNormal(Vector2 direction)
    {
        SetSprite(discreteDirection);
        float angle = -90;
        if (!(direction.y == 0 && direction.x == -1))
        {
            angle = direction.y * 90 + direction.x * direction.y * -45 + 90;
        }
        transform.rotation = Quaternion.Euler(0,0,angle);
    }

    private void SwitchDiagonal(Vector2 direction)
    {
        SetSprite(diagonalDirection);
        transform.localScale.Set(direction.x, direction.y, 1f);
    }

    public void RotateDirection(Vector2? direction)
    {
        if (!direction.HasValue) return;
        int x = (int)direction.Value.x;
        int y = (int)direction.Value.y;
        if (x != 0 && y != 0)
        {
            SwitchDiagonal(direction.Value);
            return;
        }
        SwitchNormal(direction.Value);
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    } 
}