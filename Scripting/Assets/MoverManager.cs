using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN singleton_complex_example
public class MoverManager : Singleton<MoverManager>
{
    public void ManageMovers() {
        Debug.Log("Doing some useful work!");
    }

    // BEGIN singleton_complex_example_usage_holder
    public void UsageDemo() {
        // BEGIN singleton_complex_example_usage
        MoverManager.instance.ManageMovers();
        // END singleton_complex_example_usage
    }
    // END singleton_complex_example_usage_holder
}
// END singleton_complex_example
