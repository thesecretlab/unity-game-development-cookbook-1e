using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN raycast_interaction_using
using UnityEngine.EventSystems;
// END raycast_interaction_using

// BEGIN raycast_interaction
public class ObjectMouseInteraction : 
    MonoBehaviour,
    IPointerEnterHandler, // Handles the mouse cursor entering the object
    IPointerExitHandler, // Handles the mouse cursor exiting the object
    IPointerUpHandler, // Handles the mouse button lifting up on this object
    IPointerDownHandler, // Handles the mouse button being pressed on this object
    IPointerClickHandler // Handles when the mouse is pressed and released on this object
,
IMoveHandler
{

    // BEGIN raycast_interaction_setup
    Material material;

    void Start() {
        material = GetComponent<Renderer>().material;
    }
    // END raycast_interaction_setup

    // BEGIN raycast_interaction_methods
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.LogFormat("{0} clicked!", gameObject.name);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogFormat("{0} pointer down!", gameObject.name);

        material.color = Color.green;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.LogFormat("{0} pointer enter!", gameObject.name);

        material.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.LogFormat("{0} pointer exit!", gameObject.name);

        material.color = Color.white;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.LogFormat("{0} pointer up!", gameObject.name);

        material.color = Color.yellow;
    }

    // END raycast_interaction_methods

}
// END raycast_interaction
