using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SpriteRenderer))]
abstract public class AliveBehaviour : MonoBehaviour
{
    public Direction directionInstance;
    public MovableTrait movable;
    public ShakableTrait shakable;
    public RythmicTrait rythmic;
    public SpriteRenderer spriteRenderer;
    private bool isDead = true;

    private void Start()
    { 
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = GetAliveColor();
        directionInstance.arrowRenderer.color = GetDirectionColor();
        movable = new MovableTrait(transform);
        shakable = new ShakableTrait(transform);
        rythmic = new RythmicTrait();
        AliveOnStart();
    }

    abstract public Color GetAliveColor();
    abstract public Color GetDeadColor();
    abstract public Color GetDirectionColor();

    public void BeBorn()
    {
        isDead = false;
        spriteRenderer.color = GetAliveColor();
        directionInstance.HideSprite();
        rythmic.RythmStart();
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void Die()
    {
        isDead = true;
        spriteRenderer.color = GetDeadColor();
        directionInstance.HideSprite();
        rythmic.RythmStop();
    }

    private void Update()
    {
        if (isDead) return;
        movable.Update();
        shakable.Update();
        AliveOnUpdate();
    }

    abstract public void AliveOnStart();
    abstract public void AliveOnUpdate();
}