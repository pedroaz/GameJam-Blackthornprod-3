using UnityEngine;

/// <summary>
/// An enumeration of the bullet directions
/// </summary>
public enum BulletDirection
{
    Up,
    Down
}

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb2d;
    Vector2 defaultForceVector;
    SpriteRenderer bulletSprite;

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
        defaultForceVector = new Vector2(GameConstants.BulletImpulseForce, 0);
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
    /// Called when the bullet becomes invisible
    /// </summary>
    void OnBecameInvisible()
    {
        // return to the pool
        ObjectPool.ReturnBullet(gameObject);
    }
}
