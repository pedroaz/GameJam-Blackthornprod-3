using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    Renderer objectRenderer;
    Vector2 scrollDirection = new Vector2(0, 1);

    // Start is called before the first frame update
    void Start()
    {
        //Cached for efficiency
        objectRenderer = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Scroll the texture
        objectRenderer.material.mainTextureOffset += GameConstants.BackgroundScrollSpeed * Time.deltaTime * scrollDirection;
    }
}
