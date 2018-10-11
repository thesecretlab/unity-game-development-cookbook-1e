using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN simple_singleton
public class SimpleSingleton : MonoBehaviour
{

    // A 'static' variable is shared between all instances
    // of the class
    public static SimpleSingleton instance;

    void Awake() {
        // When this object wakes up, it sets the instance variable to
        // itself; because the instance variable is public and static, any
        // class from any location can access it and call its methods.
        instance = this;
    }

    // An example method, that any other part of the code can call, as long
    // as there's a game object in the scene that has a SimpleSingleton
    // component attached
    public void DoSomething() {
        Debug.Log("I'm the singleton! Hello!");
    }

    // BEGIN simple_singleton_example_holder
    void UsageExample() {
        // BEGIN simple_singleton_example
        // Accessing the singleton and calling a method on it
        SimpleSingleton.instance.DoSomething();
        // END simple_singleton_example
    }
    // END simple_singleton_example_holder
}
// END simple_singleton