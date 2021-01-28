using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradesItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private UpgradesSlot initialSlot;

    private RectTransform rect;
    private CanvasGroup canvasGroup;
    private bool droppedOnSlot = false;
    private List<UpgradesItem> upgradeItemsList = new List<UpgradesItem>();

    /// <summary>
    /// The slot where this item is positioned
    /// </summary>
    public UpgradesSlot InitialSlot { get { return initialSlot; } set { initialSlot = value; } }

    /// <summary>
    /// The Rect of this object
    /// </summary>
    public RectTransform Rect { get { return rect; } }

    void Awake()
    {
        //Saving the transform for later
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        //Adds this as a listener to the Shield Changed event
        EventManager.AddUpgradeItemDroppedListener(HandleUpgradeItemDropped);
    }

    // Start is called before the first frame update
    void Start()
    {
        upgradeItemsList = new List<UpgradesItem>(FindObjectsOfType<UpgradesItem>());
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
        canvasGroup.alpha = 0.75f;

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
        canvasGroup.alpha = 1.0f;

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

    /// <summary>
    /// Handles when the Upgrade Item is dropped into a slot
    /// </summary>
    private void HandleUpgradeItemDropped()
    {
        droppedOnSlot = true;
    }
}
