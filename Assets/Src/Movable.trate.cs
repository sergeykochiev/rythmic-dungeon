using UnityEngine;

public class MovableTrait
{
    public const float DefaultCellSize = 1f;
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
    private readonly float speedMultiplier = 0.01f;
    private MoveProperties queuedMove;
    private MoveProperties currentMove;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private readonly float cellSize;

    public MovableTrait(Transform transform, float cellSize = DefaultCellSize)
    {
        this.cellSize = cellSize;
        this.transform = transform;
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
        isMoving = true;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    private bool IsAchievedPosition()
    {
        return Vector3.Distance(transform.position, targetPosition) < 0.001f;
    }

    private void EndMove()
    {
        transform.position = targetPosition;
        isMoving = false;
        currentMove = null;
    }

    private void AdvanceMove()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            cellSize * speedMultiplier
        );
    }

    public void Update()
    {
        if (isMoving == false) return;
        if (IsAchievedPosition())
        {
            EndMove();
            return;
        }
        AdvanceMove();
    }
}