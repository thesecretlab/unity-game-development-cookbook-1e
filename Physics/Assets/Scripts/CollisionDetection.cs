using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {

    // BEGIN trigger_detection
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogFormat("Object {0} entered trigger {1}!",
                        other.name, this.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.LogFormat("Object {0} exited trigger {1}!",
                        other.name, this.name);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.LogFormat("Object {0} remained in trigger {1}!", 
                        other.name, this.name);
    }
    // END trigger_detection

    // BEGIN collision_detection
    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogFormat("Object {0} started touching {1}!",
                        collision.gameObject.name, this.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.LogFormat("Object {0} stopped touching {1}!",
                        collision.gameObject.name, this.name);
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.LogFormat("Object {0} remained touching {1}!",
                        collision.gameObject.name, this.name);
    }
    // END collision_detection
}
