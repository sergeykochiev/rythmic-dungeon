using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

// TODO:
// add main menu
// add game end animation
// add sounds
// add cool textures
// automatically update camera and field from constants
// FIX enemies still go off the field
// FIX shaking is not working
// FIX min and max are not counted properly for %2==1 field sizes

public class Player : AliveBehaviour 
{
    private PowerController power;
    private Color NormalColor = new Color(0.8f, 0.9f, 1f);
    private Color DeadColor = new Color(0.1f, 0.1f, 0.2f);

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
            Vector3 targetPos = transform.position + Vector3.up;
            if (targetPos.y > Constants.MaxFieldPos) return;
            QueueMove(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 targetPos = transform.position + Vector3.down;
            if (targetPos.y < Constants.MinFieldPos) return;
            QueueMove(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 targetPos = transform.position + Vector3.left;
            if (targetPos.x < Constants.MinFieldPos) return;
            QueueMove(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 targetPos = transform.position + Vector3.right;
            if (targetPos.x > Constants.MaxFieldPos) return;
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