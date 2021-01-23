using UnityEngine;

public class GameConstants
{
    // Player Ship
    public const float PlayerShootDelaySeconds = 0.1f;
    public const float ShipMoveUnitsPerSecond = 10f;
    public const float ShipBulletXOffset = 0.1f;
    public const float ShipBulletYOffset = 0.1f;

    // Bullets
    public const float BulletImpulseForce = 20f;

    // Enemy Ships
    public const float EnemyImpulseForce = -3f;
    public const float EnemyBulletXOffset = -0.75f;
    public const float EnemyBulletYOffset = 0.04f;
    public const float EnemyShootDelaySeconds = 0.5f;

    // Object Pools
    public const int InitialBulletPoolCapacity = 150;
    public const int InitialEnemyPoolCapacity = 15;
}
