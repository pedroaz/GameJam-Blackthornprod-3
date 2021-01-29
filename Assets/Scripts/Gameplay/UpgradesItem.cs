using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UpgradeTypes
{
    Weapons,
    Speed,
    Health
}

public class UpgradesItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private UpgradesSlot initialSlot;

    [SerializeField]
    private UpgradeTypes upgradeType;

    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private bool droppedOnSlot = false;
    private List<UpgradesItem> upgradeItemsList = new List<UpgradesItem>();
    private float originalTransparency;

    /// <summary>
    /// The slot where this item is positioned
    /// </summary>
    public UpgradesSlot InitialSlot { get { return initialSlot; } set { initialSlot = value; } }

    /// <summary>
    /// The Rect of this object
    /// </summary>
    public RectTransform Rect { get { return rect; } }

    /// <summary>
    /// Accessor for the droppedOnSlot variable
    /// </summary>
    public bool DroppedOnSlot { get { return droppedOnSlot; } set { droppedOnSlot = value; } }

    // <summary>
    /// Accessor for the upgradeType variable
    /// </summary>
    public UpgradeTypes UpgradeType { get { return upgradeType; } }

    void Awake()
    {
        //Saving the transform for later
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Creates the Items list
        upgradeItemsList = new List<UpgradesItem>(FindObjectsOfType<UpgradesItem>());

        // Sets the upgrade item animations to looping and initially not playing
        gameObject.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.wrapMode = WrapMode.Loop;
        gameObject.GetComponent<Animator>().speed = 0.0f;

        // Saves the transparency set in the inspector
        originalTransparency = canvasGroup.alpha;
    }

    /// <summary>
    /// Called when the mouse starts moving with the item clicked down
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Only allow dragging if the game is unpaused
        if (MenuManager.IsGamePaused) return;

        //Allows the click events to go through to the next object
        //canvasGroup.blocksRaycasts = false;
        foreach(UpgradesItem item in upgradeItemsList)
        {
            item.UpdateBlockRaycast(false);
        }

        //Add transparency
        canvasGroup.alpha = 0.75f * originalTransparency;

        //Updates the droppedOnSlot variable
        droppedOnSlot = false;
    }

    /// <summary>
    /// Called when the mouse is moving with the item being clicked down
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        //Only allow dragging if the game is unpaused
        if (MenuManager.IsGamePaused)
        {
            //Move the object back to the default spot
            rect.anchoredPosition += initialSlot.Rect.anchoredPosition / canvas.scaleFactor;

            return;
        }

        //Move the object only when moving the mouse
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    /// <summary>
    /// Called when the mouse is released after having clicked the item
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        //Blocks the click events from going through to the next object
        //canvasGroup.blocksRaycasts = true;
        foreach (UpgradesItem item in upgradeItemsList)
        {
            item.UpdateBlockRaycast(true);
        }

        //Removes transparency
        canvasGroup.alpha = originalTransparency;

        StartCoroutine(CheckDroppedOnSlot());
    }

    /// <summary>
    /// Checks if the item was dropped on top of a slot
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckDroppedOnSlot()
    {
        //Adds a small delay to let the slot message arrive
        yield return new WaitForSeconds(0.15f);

        //Check if the item was dropped on a slot
        if (!droppedOnSlot)
        {
            Rect.anchoredPosition = initialSlot.Rect.anchoredPosition;
        }
    }

    /// <summary>
    /// Updates teh value of the CanvasGroup -> BlockRaycasts variable
    /// </summary>
    /// <param name="newValue"></param>
    public void UpdateBlockRaycast(bool newValue)
    {
        canvasGroup.blocksRaycasts = newValue;
    }

    /// <summary>
    /// Updates teh value of the droppedOnSlot variable
    /// </summary>
    /// <param name="newValue"></param>
    public void UpdateDroppedOnSlot(bool newValue)
    {
       droppedOnSlot = newValue;
    }
}
