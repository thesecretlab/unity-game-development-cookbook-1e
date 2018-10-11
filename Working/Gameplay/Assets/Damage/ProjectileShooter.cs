using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN projectile_shooter
public class ProjectileShooter : MonoBehaviour {

    // The projectile to instantiate copies of
    [SerializeField] GameObject projectilePrefab = null;

    // The amount of time to wait before creating another projectile
    [SerializeField] float timeBetweenShots = 1;

    // The speed that new projectiles should be moving at
    [SerializeField] float projectileSpeed = 10;

    // On start, begin shooting projectiles
    void Start () {
        // Start creating projectiles
        StartCoroutine(ShootProjectiles());
    }
    
    // Loop forever, creating a projectile every 'timeBetweenShots' seconds
    IEnumerator ShootProjectiles() {
        while (true) {
            ShootNewProjectile();

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    // Creates a new projectile and starts it moving
    void ShootNewProjectile() {
        
        // Spawn the new object with the emitter's position and rotation
        var projectile = Instantiate(
            projectilePrefab,
            transform.position,
            transform.rotation
        );

        // Get the rigidbody on the new projectile
        var rigidbody = projectile.GetComponent<Rigidbody>();

        if (rigidbody == null) {
            Debug.LogError("Projectile prefab has no rigidbody!");
            return;
        }

        // Make it move away from the emitter's forward direction at 
        // 'projectileSpeed' units per second
        rigidbody.velocity = transform.forward * projectileSpeed;

        // Get both the projectile's collider, and the emitter's collider
        var collider = projectile.GetComponent<Collider>();
        var myCollider = this.GetComponent<Collider>();

        // If both of them are valid, tell the physics system to ignore
        // collisions between them (to prevent projectiles from colliding
        // with their source)
        if (collider != null && myCollider != null) {
            Physics.IgnoreCollision(collider, myCollider);
        }

    }
}
// END projectile_shooter
