using UnityEngine;

[CreateAssetMenu(menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Moving")]
    [Range(1, 20)] public float moveSpeed = 10;
    [Range(1, 100)] public float maxFallSpeed = 25;
    [Range(0, 1)] public float wallSlideSlow = 0.1f;
    [Range(0, 1)] public float airAdjustMultiplier = 0.2f;
    [Range(0, 1)] public float momentumResist = 0.1f;
    [Header("Jumping")]
    [Range(1, 10)] public float jumpHeight = 3;
    [Range(0, 5)] public float airHangThreshold = 0.3f;
    [Range(0.001f, 0.01f)] public float wallJumpLerp = 0.009f;
    [Range(0, 1)] public float wallJumpRecoveryThreshold = 0.3f;
    [Header("Gravity")]
    [Range(1, 10)] public float baseGravity = 2;
    [Range(1, 10)] public float fallGravityMultiplier = 3;
}
