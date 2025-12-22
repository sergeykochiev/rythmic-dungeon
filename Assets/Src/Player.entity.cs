using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

// TODO:
// add powerups (weapons)
// add main menu
// add game end animation
// add sounds
// add cool textures
// FIX enemies still go off the board

public class Player : AliveBehaviour 
{
    private Color NormalColor = Color.white;
    private Color DeadColor = Color.black;
    // private readonly float inputWindowSeconds = 0.2f;

    public override Color GetAliveColor()
    {
        return NormalColor;
    }

    public override Color GetDeadColor()
    {
        return DeadColor;
    }

    public override Color GetDirectionColor()
    {
        return DeadColor;
    }

    public override void AliveOnStart()
    {   
        transform.position.Set(0, 0, 0);
        gameObject.name = "Hero";
        gameObject.tag = "Player";
        BeBorn();
    }

    private void QueueMove(Vector2 direction)
    {
        movable.QueueMove(new(direction, 1));
        directionInstance.RotateDirection(direction);
        directionInstance.ShowSprite();
    }

    private void QueueInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (transform.position.y > Constants.MaxFieldPos - 1) return;
            QueueMove(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (transform.position.y < Constants.MinFieldPos + 1) return;
            QueueMove(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (transform.position.x < Constants.MinFieldPos - 1) return;
            QueueMove(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (transform.position.x > Constants.MaxFieldPos + 1) return;
            QueueMove(Vector2.right);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Die();
        }      
    }

    private void FixedUpdate()
    {  
        if (IsDead()) return;
        if(!rythmic.FixedUpdate()) return;
        movable.InitMove();
        movable.ResetQueuedMove();
        directionInstance.HideSprite();
    }

    public override void AliveOnUpdate()
    {
        QueueInput();
    }
}