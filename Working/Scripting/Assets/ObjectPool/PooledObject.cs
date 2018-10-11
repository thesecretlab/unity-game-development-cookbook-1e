using UnityEngine;
using System.Collections;

// BEGIN object_pool_tag
// A simple script that just exists to store a reference to an ObjectPool.
// Exists to be used by the ReturnToPool extension method.
public class PooledObject : MonoBehaviour
{
    public ObjectPool owner;
}
// END object_pool_tag

// BEGIN object_pool_extension
// A class that adds a new method to the GameObject class: ReturnToPool.
public static class PooledGameObjectExtensions {

    // This method is an extension method (note the 'this' parameter.) This
    // means that it's a new method that's added to all instances of the
    // GameObject class; you call it like this:
    //
    // gameObject.ReturnToPool()

    // Returns an object to the object pool that it was created from
    public static void ReturnToPool(this GameObject gameObject) {

        // Find the PooledObject component.
        var pooledObject= gameObject.GetComponent<PooledObject>();

        // Does it exist?
        if (pooledObject == null) {
            // If it doesn't, it means that this object never came from
            // a pool.
            Debug.LogErrorFormat(gameObject,
                "Cannot return {0} to object pool, because it was not " +
                                 "created from one.", gameObject);
            return;
        }

        // Tell the pool that we came from that this object should be
        // returned.
        pooledObject.owner.ReturnObject(gameObject);
    }
}
// END object_pool_extension