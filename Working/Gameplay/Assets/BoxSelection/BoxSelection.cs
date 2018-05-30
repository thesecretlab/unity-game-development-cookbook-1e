using UnityEngine;
using System.Collections;
using System;

// BEGIN box_selection
// Handles the input and display of a selection box.
public class BoxSelection : MonoBehaviour
{
    
    // Draggable inspector reference to the Image GameObject's RectTransform.
    public RectTransform selectionBox;

    // This variable will store the location of wherever we first click before dragging.
    private Vector2 initialClickPosition = Vector2.zero;

    // The rectangle that the box has dragged, in screen space.
    public Rect SelectionRect { get; private set; }

    // If true, the user is actively dragging a box
    public bool IsSelecting { get; private set; }

    // Configure the visible box
    private void Start()
    {
        // Setting the anchors to be positioned at zero-zero means that
        // the box's size won't change as its parent changes size
        selectionBox.anchorMin = Vector2.zero;
        selectionBox.anchorMax = Vector2.zero;

        // Setting the pivot point to zero means that the box will pivot around
        // its lower-left corner
        selectionBox.pivot = Vector2.zero;

        // Hide the box at the start
        selectionBox.gameObject.SetActive(false);
    }

    void Update()
    {
        // When we start dragging, record the position of the mouse, and start
        // showing the box
        if (Input.GetMouseButtonDown(0))
        {
            // Get the initial click position of the mouse. No need to convert to GUI space
            // since we are using the lower left as anchor and pivot.
            initialClickPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // Show the box
            selectionBox.gameObject.SetActive(true);
        }

        // While we are dragging, update the position and size of the box based
        // on the mouse position
        if (Input.GetMouseButton(0))
        {
            // Store the current mouse position in screen space.
            Vector2 currentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // Figure out the lower-left corner, and the upper-right corner
            var xMin = Mathf.Min(currentMousePosition.x, initialClickPosition.x);
            var xMax = Mathf.Max(currentMousePosition.x, initialClickPosition.x);
            var yMin = Mathf.Min(currentMousePosition.y, initialClickPosition.y);
            var yMax = Mathf.Max(currentMousePosition.y, initialClickPosition.y);

            // Build a rectangle from these corners
            var screenSpaceRect = Rect.MinMaxRect(xMin, yMin, xMax, yMax);

            // The anchor of the box has been configured to be its lower-left
            // corner, so by setting its anchoredPosition, we set its lower-left
            // corner.
            selectionBox.anchoredPosition = screenSpaceRect.position;

            // The size delta is how far the box extends from its anchor.
            // Because the anchor's minimum and maximum are the same point,
            // changing its size delta directly changes its final size.
            selectionBox.sizeDelta = screenSpaceRect.size;

            // Update our selection box
            SelectionRect = screenSpaceRect;

        }

        // When we release the mouse button, hide the box, and record that we're
        // no longer selecting
        if (Input.GetMouseButtonUp(0))
        {
            SelectionComplete();

            // Hide the box
            selectionBox.gameObject.SetActive(false);

            // We're no longer selecting
            IsSelecting = false;
        }
    }

    // Called when the user finishes dragging a selection box.
    void SelectionComplete()
    {

        // Get the component attached to this scene
        Camera mainCamera = GetComponent<Camera>();

        // Get the bottom-left and top-right corners of the screen-space
        // selection view, and convert them to viewport space
        var min = mainCamera.ScreenToViewportPoint(
            new Vector3(SelectionRect.xMin, SelectionRect.yMin));
        var max = mainCamera.ScreenToViewportPoint(
            new Vector3(SelectionRect.xMax, SelectionRect.yMax));

        // We want to create a bounding box in viewport space. We have the X and
        // Y coordinates of the bottom-left and top-right corners; now we'll 
        // include the Z coordinates.
        min.z = mainCamera.nearClipPlane;
        max.z = mainCamera.farClipPlane;

        // Construct our bounding box
        var viewportBounds = new Bounds();
        viewportBounds.SetMinMax(min, max);

        // Check each object that has a Selectable component
        foreach (var selectable in FindObjectsOfType<BoxSelectable>()) {

            // Figure out where this object is in viewport space
            var viewportPoint = mainCamera.WorldToViewportPoint(selectable.transform.position);

            // Is that point within our viewport bounding box? If it is, they're
            // selected.
            var selected = viewportBounds.Contains(viewportPoint);

            if (selected) {
                // Let them know.            
                selectable.Selected();
            }
        }

    }
}
// END box_selection
