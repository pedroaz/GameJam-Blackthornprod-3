using UnityEngine;

public class ShipController : MonoBehaviour
{
    float colliderHalfHeight;
    float colliderHalfWidth;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        colliderHalfWidth = collider.size.x / 2;
        colliderHalfHeight = collider.size.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
