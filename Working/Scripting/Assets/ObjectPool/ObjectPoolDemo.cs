using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN object_pool_demo
// An example of using an object pool.
public class ObjectPoolDemo : MonoBehaviour
{
    // The object pool from which we'll be getting our objects
    [SerializeField]
    private ObjectPool pool;

    IEnumerator Start() {

        // Get and place an object from the pool every 0.1 to 0.5 seconds
        while (true) {

            // Get (or create, we don't care which) an object from pool
            var o = pool.GetObject();

            // Pick a point somewhere inside a sphere of radius 4
            var position = Random.insideUnitSphere * 4;

            // Place it
            o.transform.position = position;

            // Wait between 0.1 and 0.5 seconds and do it again
            var delay = Random.Range(0.1f, 0.5f);

            yield return new WaitForSeconds(delay);
        }
    }
}
// END object_pool_demo