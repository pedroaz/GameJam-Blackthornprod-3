using UnityEngine;

public class Enemy : MonoBehaviour
{
    Timer shootTimer;

    // saved for efficiency
    Rigidbody2D rb2d;
    Vector2 forceVector;

    /// <summary>
    /// Initializes object. We don't use Start for this because
    /// we want to initialize objects as they're added to the
    /// pool
    /// </summary>
    public void Initialize()
    {
        // save for efficiency
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        // set force vector
        forceVector = new Vector2(GameConstants.EnemyImpulseForce, 0);

        // set up shoot timer
        shootTimer = gameObject.AddComponent<Timer>();
        shootTimer.Duration = GameConstants.EnemyShootDelaySeconds;
        shootTimer.AddTimerFinishedListener(HandleShootTimerFinished);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Shoots a bullet and restarts the shoot timer
    /// </summary>
    void HandleShootTimerFinished()
    {
        shootTimer.Run();

        // shoot bullet
        GameObject bullet = ObjectPool.GetEnemyBullets(1)[0];
        Vector2 bulletPos = gameObject.transform.position;
        bulletPos += new Vector2(GameConstants.EnemyBulletXOffset, GameConstants.EnemyBulletYOffset);
        bullet.GetComponent<Bullet>().StopMoving();
        bullet.transform.position = bulletPos;
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().StartMoving(BulletDirection.Down);
        bullet.GetComponent<Rigidbody2D>().AddTorque(-5.0f, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Stops the enemy and its shoot timer
    /// </summary>
    public void Deactivate()
    {
        if (rb2d != null) rb2d.velocity = Vector2.zero;
        if (shootTimer != null) shootTimer.Stop();
    }
}
