using UnityEngine;

public class EnergyShield : MonoBehaviour
{
    private float shieldCurrentEnergy = 1.0f;
    private SpriteRenderer shieldRenderer;

    // Start is called before the first frame update
    void Start()
    {
        shieldRenderer = gameObject.GetComponent<SpriteRenderer>();

        //Adds this as a listener to the Shield Changed event
        EventManager.AddShieldChangedListener(HandleShieldChangedEvent);
    }

    /// <summary>
    /// Processes trigger collisions with other game objects
    /// </summary>
    /// <param name="other">information about the other collider</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // if colliding with a bullet, return bullet to pool and take damage
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            //Plays the shield damaged sound
            AudioManager.PlaySFX(AudioClipNames.ShieldDamage);

            // Rolls a random chance to check if the bullets makes it past
            float bulletChance = (float) Random.Range(0, 100)/100;

            // Only allow the bullet through if the chance is greater than the protection provided
            if(bulletChance < shieldCurrentEnergy * GameConstants.ShieldBaseProtectionPercent)
            {
                ObjectPool.ReturnBullet(other.gameObject);
            }
        }
    }

    /// <summary>
    /// Changes the properties of the shield
    /// </summary>
    /// <param name="energyValue">How much energy is the shield using</param>
    void HandleShieldChangedEvent(float energyValue)
    {
        //Updates the shield's energy
        shieldCurrentEnergy = energyValue;

        //Update the shield's opacity
        Color currentColor = shieldRenderer.color;
        currentColor.a = energyValue;
        shieldRenderer.color = currentColor;

        //Plays the shield full energy sound
        if (shieldCurrentEnergy >= 1.0f)
        {
            AudioManager.PlaySFX(AudioClipNames.ShieldFullEnergy);
        }
    }
}
