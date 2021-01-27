using System.Collections.Generic;

public class GameConstants
{
    // Player Ship
    public const float PlayerShootDelaySeconds = 0.1f;
    public const float ShipMoveUnitsPerSecond = 10f;
    public const float ShipBulletXOffset = 0f;
    public const float ShipBulletYOffset = 0.1f;
    public const float MaxShipHealth = 100.0f;
    public const int ShipBulletCollisionDamage = 12;
    public const int ShipEnemyCollisionDamage = 20;
    public const float ShieldBaseProtectionPercent = 0.8f;

    // Bullets
    public const float BulletImpulseForce = 20f;

    // Enemy Ships
    public const float EnemySpawnDelaySeconds = 1.0f;
    public const float EnemyBulletXOffset = 0f;
    public const float EnemyBulletYOffset = -0.2f;
    public static readonly Dictionary<EnemyTypes, EnemyStats> EnemyShipStats
        = new Dictionary<EnemyTypes, EnemyStats>
        {
            //Types of enemy and their respective speed, health, rate of fire and active time
            {EnemyTypes.Fast, new EnemyStats(2.5f, 1.0f, 0.1f, 1.0f) },
            {EnemyTypes.Strong, new EnemyStats(1.0f, 2.0f, 0.5f, 1.0f) },
            {EnemyTypes.Tank, new EnemyStats(0.5f, 5.0f, 1.0f, 1.0f) },
        };

    // HUD
    public static float ShieldSliderStep = 0.005f;

    // Object Pools
    public const int InitialBulletPoolCapacity = 150;
    public const int InitialEnemyPoolCapacity = 15;
    public const int InitialExplosionPoolCapacity = 10;
}
