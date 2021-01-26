using UnityEngine;

/// <summary>
/// An enumeration of explosion sizes
/// </summary>
public enum ExplosionSize
{
    Small,
    Default,
    Big
}

/// <summary>
/// An explosion
/// </summary>
public class Explosion : MonoBehaviour
{
    //Cached for efficiency
    Animator anim;
    Vector3 smallScale = new Vector3(0.2f, 0.2f, 1);
    Vector3 defaultScale = new Vector3(1, 1, 1);
    Vector3 bigScale = new Vector3(1.5f, 1.5f, 1);

    /// <summary>
    /// Called to start the explosion
    /// </summary>
    public void Initialize()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Plays the explosion animation
    /// </summary>
    /// <param name="size">Size of the explosion to play</param>
    public void PlayExplosion(ExplosionSize size)
    {
        switch (size)
        {
            case ExplosionSize.Small:
                gameObject.transform.localScale = smallScale;
                break;
            case ExplosionSize.Default:
                gameObject.transform.localScale = defaultScale;
                break;
            case ExplosionSize.Big:
                gameObject.transform.localScale = bigScale;
                break;
            default: break;
        }
        anim.SetBool("playAnim", true);
    }

    // Update is called once per frame
    void Update()
    {
        //Destroy the Game Object if the explosion has finished its animation
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            gameObject.transform.localScale = defaultScale;
            ObjectPool.ReturnExplosion(gameObject);
        }
    }
}
