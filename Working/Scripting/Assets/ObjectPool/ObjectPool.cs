using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN object_pool_notifier
public interface IObjectPoolNotifier {
    // Called when the object is being returned to the pool.
    void OnEnqueuedToPool();

    // Called when the object is leaving the pool, or has just been created.
    // If 'created' is true, the object has just been created, and is not being
    // recycled.

    // (Doing it this way means you use a single method to do the object's
    // setup, for both your first time and all subsequent times.)
    void OnCreatedOrDequeuedFromPool(bool created);
}
// END object_pool_notifier

// BEGIN object_pool_class
public class ObjectPool : MonoBehaviour
{

    // The prefab that will be instantiated.
    [SerializeField]
    private GameObject prefab;

    // The queue of objects that are not currently in use
    private Queue<GameObject> inactiveObjects = new Queue<GameObject>();

    // Gets an object from the pool. If there isn't one in the queue, a new one
    // is created.
    public GameObject GetObject() {

        // Are there any inactive objects to re-use?
        if (inactiveObjects.Count > 0) {

            // Get an object from the queue
            var dequeuedObject = inactiveObjects.Dequeue();

            // Queued objects are children of the pool, so we move them back to
            // the root
            dequeuedObject.transform.parent = null;
            dequeuedObject.SetActive(true);

            // If there are any MonoBehaviours on this object that implement 
            // IObjectPoolNotifier, let them know that this object just left
            // the pool

            var notifiers = dequeuedObject
                .GetComponents<IObjectPoolNotifier>();

            foreach (var notifier in notifiers) {
                // Notify the script that it left the pool
                notifier.OnCreatedOrDequeuedFromPool(false);
            }

            // Return the object for use
            return dequeuedObject;

        } else {

            // There's nothing in the pool to re-use. Create a new object.

            var newObject = Instantiate(prefab);

            // Add the pool tag so that it's able to return to the pool when
            // done.
            var poolTag = newObject.AddComponent<PooledObject>();

            poolTag.owner = this;

            // Mark the pool tag so that it never shows up in the Inspector.
            // There's nothing configurable on it - it only exists to store
            // a reference back to the pool that creates it.
            poolTag.hideFlags = HideFlags.HideInInspector;

            // Notify the object that it was created
            var notifiers = newObject
                .GetComponents<IObjectPoolNotifier>();

            foreach (var notifier in notifiers)
            {
                // Notify the script that it was just created
                notifier.OnCreatedOrDequeuedFromPool(true);
            }

            // Return the object we just created.
            return newObject;
        }
    }

    // Disables an object and returns it to the queue for later reuse.
    public void ReturnObject(GameObject gameObject)
    {
        // Find any component that we need to notify
        var notifiers = gameObject
                .GetComponents<IObjectPoolNotifier>();

        foreach (var notifier in notifiers)
        {
            // Let it know that it's returning to the pool
            notifier.OnEnqueuedToPool();
        }

        // Disable the object and make it a child of this one, so that it
        // doesn't clutter up the hierarchy
        gameObject.SetActive(false);
        gameObject.transform.parent = this.transform;

        // Put the object into the inactive queue
        inactiveObjects.Enqueue(gameObject);


    }
}
//END object_pool_class