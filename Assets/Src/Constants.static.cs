using UnityEngine;

public static class Constants
{
    public const int MaxDifficultyLevel = 20;
    public const int MaxFieldSize = 16;
    public const int MinFieldPos = -MaxFieldSize / 2 + 1;
    public const int MaxFieldPos = MaxFieldSize / 2 -1;
    public const int GameEndCameraShake = 50;
    public readonly static Vector2[] Directions = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
}