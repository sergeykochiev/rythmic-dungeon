using Unity.VisualScripting;
using UnityEngine;

public class Enemy : AliveBehaviour
{
    private Color AggressiveColor = new Color(1f, 0.2f, 0.2f); // Bright red
    private Color NormalColor = new Color(0.8f, 0.1f, 0.1f);    // Dark red
    private Color DeadColor = new Color(0.5f, 0f, 0.5f);        // Purple
    private Color DirectionColor = Color.cyan;                  // Cyan for visibility
    public Player playerInstance;
    private float aggressiveChance = 0;
    private int maxSpeedWhenAggressive = 1;
    private int speedWhenPassive = 1;
    public bool isAggressive;
    private int difficultyLevel = 0;
    private bool isNextMoveReady = false;
    private float moveReadyWindowSeconds;

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
        return DirectionColor;
    }

    public override void AliveOnStart()
    {
        tag = "Enemy";
        moveReadyWindowSeconds = rythmic.rythmIntervalSeconds / 2;
        BeBorn();
    }

    public void SetDifficultyLevel(int difficultyLevel)
    {
        this.difficultyLevel = difficultyLevel;
        if (difficultyLevel > Constants.MaxDifficultyLevel) difficultyLevel = Constants.MaxDifficultyLevel;
        aggressiveChance = (float)difficultyLevel / 10;
        speedWhenPassive = difficultyLevel / 6 + 1;
        maxSpeedWhenAggressive = difficultyLevel / 3 + 1;
    }

    public void SetPlayerInstance(Player playerInstance)
    {
        this.playerInstance = playerInstance;
    }
    
    private Vector3 GetPlayerBasedDirection()
    {
        Vector2 pos = new(0, 0);
        float diffX = playerInstance.transform.position.x - transform.position.x;
        float diffY = playerInstance.transform.position.y - transform.position.y;
        if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
        {
            pos.x = diffX > 0 ? 1 : -1;
        } else
        {
            pos.y = diffY > 0 ? 1 : -1;
        }
        return pos;
    }

    private Vector2 GetRandomDirection()
    {
        Vector2 pos = new(0, 0)
        {
            x = Random.Range(-1, 2),
            y = Random.Range(-1, 2),
        };
        return pos;
    }

    private void UpdateAggressive()
    {
        isAggressive = Random.Range(0f, 1f) < aggressiveChance;
    }

    private int GetSpeedWhenAggressive()
    {
        return Random.Range(speedWhenPassive + 1, maxSpeedWhenAggressive + 1);
    }

    private MovableTrait.MoveProperties GetNewMove()
    {
        if (isAggressive)
        {
            Vector3 direction = GetPlayerBasedDirection();
            Vector3 targetPos = transform.position + direction;
            if (IsWithinBounds(targetPos))
            {
                return new(direction, GetSpeedWhenAggressive());
            }
        }
        
        // Try random directions until we find one within bounds
        for (int i = 0; i < 10; i++)
        {
            Vector2 randomDir = GetRandomDirection();
            Vector3 targetPos = transform.position + (Vector3)randomDir;
            if (IsWithinBounds(targetPos))
            {
                return new(randomDir, speedWhenPassive);
            }
        }
        
        // If no valid direction found, don't move
        return new(Vector2.zero, 0);
    }
    
    private bool IsWithinBounds(Vector3 position)
    {
        return position.x >= Constants.MinFieldPos && 
               position.x <= Constants.MaxFieldPos && 
               position.y >= Constants.MinFieldPos && 
               position.y <= Constants.MaxFieldPos;
    }

    private void ChangeColor()
    {
        if (isAggressive)
        {
            spriteRenderer.color = AggressiveColor;
            return;
        }
        spriteRenderer.color = NormalColor;
    }

    private void InitMove()
    {
        isNextMoveReady = false;
        movable.InitMove();
        movable.ResetQueuedMove();
    }

    private void Evolve()
    {
        shakable.InitShake(50);
        SetDifficultyLevel(difficultyLevel + 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsDead()) return;
        if (collision.collider.CompareTag("Enemy"))
        {
            if (!collision.gameObject.GetComponent<Enemy>().IsDead())
            {
                Die();
            } else
            {
                Evolve();
            }
        }
        if (collision.collider.CompareTag("Power"))
        {
            Die();
        }
    }

    private void ReadyNextMove()
    {
        UpdateAggressive();
        MovableTrait.MoveProperties move = GetNewMove();
        if (move.direction != Vector2.zero)
        {
            movable.QueueMove(move);
            directionInstance.RotateDirection(move.direction);
        }
        ChangeColor();
        isNextMoveReady = true;
    }

    private void FixedUpdate()
    {
        if (IsDead()) return;
        if (rythmic.TimePassed() + moveReadyWindowSeconds > rythmic.rythmIntervalSeconds && !isNextMoveReady)
        {
            ReadyNextMove();
        }
        if (!rythmic.FixedUpdate()) return;
        InitMove();
        directionInstance.SetNoDirection();
    }

    public override void AliveOnUpdate()
    {
    }
}
