using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN poisson_disc_demo
public class PoissonDiscDemo : MonoBehaviour {


    // The area in which we'll place our points
    [SerializeField] Vector2 size = new Vector2(10,10);

    // The points won't be any closer than this to each other
    [SerializeField] float cellSize = 0.5f;

    // The list of points we'll show
    List<Vector3> points;

    // Calculate the points to show when the game starts
    private void Awake()
    {
        // Create a list of points from the sampler
        points = new List<Vector3>();

        var sampler = new PoissonDiscSampler(size.x, size.y, cellSize);

        foreach (var point in sampler.Samples()) {
            points.Add(new Vector3(point.x, transform.position.y, point.y));
        }
    }

    // Visualise the points we've calculated
    private void OnDrawGizmos()
    {
        // Early out if we have no list to use
        if (points == null) {
            return;
        }

        Gizmos.color = Color.white;

        // Draw each point in the scene
        foreach (var point in points) {
            Gizmos.DrawSphere(transform.position + point, 0.1f);
        }
    }

}
// END poisson_disc_demo
