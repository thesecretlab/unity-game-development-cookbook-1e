using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN material_change_color
public class ChangeMaterialColor : MonoBehaviour {

    // The colours we're fading between
    [SerializeField] Color fromColor = Color.white;
    [SerializeField] Color toColor = Color.green;

    // The speed with which're fading
    [SerializeField] float speed = 1f;

    // A cached reference to the renderer
    // (The 'new' keyword makes the compiler not warn us about the fact that
    // we're overriding an existing property that we inherit from MonoBehaviour;
    // this property is deprecated, so we aren't using it and it's ok to 
    // override it)
    new Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        // Convert time to a number that smoothly moves between -1 and 1
        float t = Mathf.Sin(Time.time * speed);

        // Convert this to one that moves from 0 to 2
        t += 1;

        // Divide it by 2 to make it move from 0 to 1;
        t /= 2;

        // Interpolate between the two colours
        var newColor = Color.Lerp(fromColor, toColor, t);

        // Apply the new colour
        renderer.material.color = newColor;
    }

}
// END material_change_color