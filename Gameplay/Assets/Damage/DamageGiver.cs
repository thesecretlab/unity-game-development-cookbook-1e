using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN damage_giver
public class DamageGiver : MonoBehaviour {

    [SerializeField] int damageAmount = 1;

    // Called when the object collides with another
    private void OnCollisionEnter(Collision collision)
    {
        // Does the object we hit have a damage receiver?

        var otherDamageReceiver = collision
            .gameObject.GetComponent<DamageReceiver>();

        if (otherDamageReceiver != null) {

            // Tell it to take damage.
            otherDamageReceiver.TakeDamage(damageAmount);
        }

        // Destroy this projectile
        Destroy(gameObject);

    }
}
// END damage_giver