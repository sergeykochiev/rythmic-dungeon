using UnityEngine;

public static class Constants
{
    public const int MaxDifficultyLevel = 20;
    public const int MaxFieldSize = 16;
    public const int MinFieldPos = -(MaxFieldSize - 1) / 2;
    public const int MaxFieldPos = (MaxFieldSize - 1) / 2;
    public const int GameEndCameraShake = 10;
    public readonly static Vector2[] Directions = {Vector2.up, Vector2.down, Vector2.left, Vector2.right};
}