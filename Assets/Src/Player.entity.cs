using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

// TODO:
// add main menu
// add game end animation
// add sounds
// add cool textures
// automatically update camera and field from constants
// FIX enemies still go off the board
// FIX shaking
// FIX min and max are not counted properly for %2==1 field sizes

public class Player : AliveBehaviour 
{
    private PowerController power;
    private Color NormalColor = Color.white;
    private Color DeadColor = Color.black;

    private bool isPowerUseQueued = false;
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

    public void SetPower(PowerController power)
    {
        this.power = power;
        power.CyclicStart();
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
    }

    private void QueueInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!power.IsReadyToUse()) return;
            QueuePower();
        }
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

    private void QueuePower()
    {
        isPowerUseQueued = true;
        spriteRenderer.color = Color.limeGreen;
    }

    private void UsePower()
    {
        if (!isPowerUseQueued) return;
        power.Use();
        isPowerUseQueued = false;
        spriteRenderer.color = NormalColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy>().IsDead()) return;
            Die();
        }      
    }

    private void FixedUpdate()
    {  
        if (IsDead()) return;
        if(!rythmic.FixedUpdate()) return;
        UsePower();
        movable.InitMove();
        movable.ResetQueuedMove();
        directionInstance.SetNoDirection();
    }

    public override void AliveOnUpdate()
    {
        QueueInput();
    }
}