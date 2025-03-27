using UnityEngine;

public class BoostData
{
    public float speedMultiplier;
    public float jumpMultiplier;

    public BoostData(float speed, float jump)
    {
        speedMultiplier = speed;
        jumpMultiplier = jump;
    }
}

public enum BoostEffect
{
    Speed,
    JumpHeight,
    SpeedAndJump
}


