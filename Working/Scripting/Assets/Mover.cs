// BEGIN mover_original
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// END mover_original

// tricky snippet processing thing so that the book looks like
// it contains two classes that have the same name
// BEGIN mover_updated
// BEGIN mover_original
public class Mover : MonoBehaviour
{
// END mover_original
    public Vector3 direction = Vector3.up;
    
    // BEGIN mover_serializefield_variable
    // BEGIN mover_serializefield
    // This variable is private, but will appear in the Inspector
    // because of the SerializeField attribute
    [SerializeField]
    // END mover_serializefield
    float speed = 0.1f;
    // END mover_serializefield_variable

    // BEGIN mover_update
    void Update()
    {
        var movement = direction * speed;

        // BEGIN mover_time
        // Multiply by delta time; movement now represents
        // 'units per second', rather than 'units per frame'
        movement *= Time.deltaTime;

        // END mover_time
        this.transform.Translate(movement);
    }
    // END mover_update
}
// END mover_updated

public class MoverOriginal : MonoBehaviour
{
    // BEGIN mover_original
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
// END mover_original
