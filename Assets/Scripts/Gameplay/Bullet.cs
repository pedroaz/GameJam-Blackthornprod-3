﻿using UnityEngine;

/// <summary>
/// An enumeration of the bullet directions
/// </summary>
public enum BulletDirection
{
    Random,
    Up,
    UpLeftLayer1,
    UpLeftLayer2,
    UpRightLayer1,
    UpRightLayer2,
    Down,
}

public class Bullet : MonoBehaviour
{
    Timer changeDirectionLayer1Timer;
    Timer changeDirectionLayer2Timer;

    Rigidbody2D rb2d;

    Vector2 defaultForceVector;
    float damageDealt = 1.0f;

    SpriteRenderer bulletSprite;

    /// <summary>
    /// Returns the amount of damage this bullet should deal
    /// </summary>
    public float DamageDealt { get { return damageDealt; } }

    /// <summary>
    /// Initializes object. We don't use Start for this because
    /// we want to initialize objects as they're added to the
    /// pool
    /// </summary>
    public void Initialize()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        bulletSprite = gameObject.GetComponent<SpriteRenderer>();

        //Set force vector
        if(gameObject.CompareTag("PlayerBullet")) defaultForceVector = new Vector2(GameConstants.PlayerBulletImpulseForce, 0);
        if(gameObject.CompareTag("EnemyBullet")) defaultForceVector = new Vector2(GameConstants.EnemyBulletImpulseForce, 0);

        //Adds the layer 1 timer in case it is needed
        changeDirectionLayer1Timer = gameObject.AddComponent<Timer>();
        changeDirectionLayer1Timer.Duration = 0.05f;
        changeDirectionLayer1Timer.AddTimerFinishedListener(HandleChangeDirection);

        //Adds the layer 2 timer in case it is needed
        changeDirectionLayer2Timer = gameObject.AddComponent<Timer>();
        changeDirectionLayer2Timer.Duration = 0.075f;
        changeDirectionLayer2Timer.AddTimerFinishedListener(HandleChangeDirection);
    }

    // Update is called once per frame
    void Update()
    {
        // if a bullet is active and not moving,
        // return it to the pool
        if (gameObject.activeInHierarchy &&
            rb2d.velocity.magnitude < 0.1f)
        {
            ObjectPool.ReturnBullet(gameObject);
        }
    }

    /// <summary>
    /// Starts the bullet moving in the given direction
    /// </summary>
    /// <param name="direction">movement direction</param>
    public void StartMoving(BulletDirection direction)
    {
        // apply impulse force to get projectile moving
        int forceSignal = 1;
        Vector3 forceVector = defaultForceVector;

        switch (direction)
        {
            case BulletDirection.Up:
                forceVector = Quaternion.AngleAxis(90, Vector3.forward) * forceVector;
                break;
            case BulletDirection.Down:
                forceVector = Quaternion.AngleAxis(-90, Vector3.forward) * forceVector;
                break;
            case BulletDirection.Random:
                forceVector = Quaternion.AngleAxis(Random.Range(-120,-60), Vector3.forward) * forceVector;
                break;
            case BulletDirection.UpLeftLayer1:
                forceVector = Quaternion.AngleAxis(105, Vector3.forward) * forceVector;
                changeDirectionLayer1Timer.Run();
                break;
            case BulletDirection.UpLeftLayer2:
                forceVector = Quaternion.AngleAxis(120, Vector3.forward) * forceVector;
                changeDirectionLayer2Timer.Run();
                break;
            case BulletDirection.UpRightLayer1:
                forceVector = Quaternion.AngleAxis(75, Vector3.forward) * forceVector;
                changeDirectionLayer1Timer.Run();
                break;
            case BulletDirection.UpRightLayer2:
                forceVector = Quaternion.AngleAxis(60, Vector3.forward) * forceVector;
                changeDirectionLayer2Timer.Run();
                break;
            default: break;
        }

        bulletSprite.flipX = forceSignal == -1;
        rb2d.AddForce(forceVector * forceSignal, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Stops the bullet
    /// </summary>
    public void StopMoving()
    {
        rb2d.velocity = Vector2.zero;
    }

    /// <summary>
    /// Handles the bullets changing direction back to center
    /// </summary>
    void HandleChangeDirection()
    {
        StopMoving();
        StartMoving(BulletDirection.Up);
    }

    /// <summary>
    /// Called when the bullet becomes invisible
    /// </summary>
    void OnBecameInvisible()
    {
        // return to the pool
        ObjectPool.ReturnBullet(gameObject);
    }
}
