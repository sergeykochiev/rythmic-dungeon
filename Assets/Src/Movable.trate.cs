using UnityEngine;

public class MovableTrait : AnimatedTrait
{
    public const float DefaultCellSize = 1f;
    public const float DefaultSpeed = 0.01f;
    public class MoveProperties
    {
        public int distance;
        public Vector2 direction;

        public MoveProperties(Vector2 direction, int distance)
        {
            this.direction = direction;
            this.distance = distance;
        }
    }
    private readonly Transform transform;
    private MoveProperties queuedMove;
    private MoveProperties currentMove;
    private Vector3 targetPosition;
    private readonly float cellSize;

    public MovableTrait(Transform transform, float cellSize = DefaultCellSize, float speed = DefaultSpeed)
    {
        this.cellSize = cellSize;
        this.transform = transform;
        SetAnimationSpeed(speed);
    }

    public void QueueMove(MoveProperties moveProperties)
    {
        queuedMove = moveProperties;
    }

    public void ResetQueuedMove()
    {
        queuedMove = null;
    }

    private Vector3 GetTargetPosition()
    {
        return transform.position + cellSize * currentMove.distance * (Vector3)currentMove.direction;
    }

    public void InitMove()
    {
        if (queuedMove == null) return;
        currentMove = new MoveProperties(
            queuedMove.direction, queuedMove.distance
        );
        targetPosition = GetTargetPosition();
        AnimationStart();
    }

    public override void OnAnimationStart()
    {
    }

    public override bool IsAnimationEndConditionMet()
    {
        return Vector3.Distance(transform.position, targetPosition) < 0.001f;
    }

    public override void OnAnimationEnd()
    {
        currentMove = null;
        transform.position = targetPosition;
    }

    public override void OnAnimationStep(float animationSpeed)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            cellSize * animationSpeed
        );
    }
}