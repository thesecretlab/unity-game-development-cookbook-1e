using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BEGIN poisson_disc_sampler
// With thanks to Gregory Schlomoff, who wrote this implementation
// and released it into the public domain, and to Robert Bridson of the 
// University of British Columbia, for developing the efficient algorithm
// that this code implements.

// http://gregschlom.com/devlog/2014/06/29/Poisson-disc-sampling-Unity.html
// http://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph07-poissondisk.pdf

public class PoissonDiscSampler
{
    // Maximum number of attempts before marking a sample as inactive.
    private const int k = 30;  

    // The rectangle in which points will be placed
    private readonly Rect rect;

    // radius squared
    private readonly float radius2;  

    // The cell size of the grid of points
    private readonly float cellSize;

    // The grid of points
    private Vector2[,] grid;

    // The list of locations near which we're trying to add new points to.
    private  List<Vector2> activeSamples = new List<Vector2>();

    // Create a sampler with the following parameters:
    //
    // width:  each sample's x coordinate will be between [0, width]
    // height: each sample's y coordinate will be between [0, height]
    // radius: each sample will be at least `radius` units away from any other 
    // sample, and at most 2 * `radius`.
    public PoissonDiscSampler(float width, float height, float radius)
    {
        rect = new Rect(0, 0, width, height);
        radius2 = radius * radius;
        cellSize = radius / Mathf.Sqrt(2);
        grid = new Vector2[Mathf.CeilToInt(width / cellSize),
                           Mathf.CeilToInt(height / cellSize)];
    }

    // Return a lazy sequence of samples. You typically want to call this in 
    // a foreach loop, like so:
    //   foreach (Vector2 sample in sampler.Samples()) { ... }
    public IEnumerable<Vector2> Samples()
    {
        // First sample is chosen randomly
        Vector2 firstSample = new Vector2(Random.value * rect.width, 
                                          Random.value * rect.height);
        
        yield return AddSample(firstSample);

        while (activeSamples.Count > 0) {

            // Pick a random active sample
            int i = (int) Random.value * activeSamples.Count;
            Vector2 sample = activeSamples[i];

            // Try `k` random candidates between [radius, 2 * radius] from that
            // sample.
            bool found = false;
            for (int j = 0; j < k; ++j) {

                float angle = 2 * Mathf.PI * Random.value;

                float r = Mathf.Sqrt(Random.value * 3 * radius2 + radius2); 

                Vector2 candidate = sample + r 
                    * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                // Accept candidates if it's inside the rect and farther than 
                // 2 * radius to any existing sample.
                if (rect.Contains(candidate) && IsFarEnough(candidate)) {
                    found = true;
                    yield return AddSample(candidate);
                    break;
                }
            }

            // If we couldn't find a valid candidate after k attempts, remove
            // this sample from the active samples queue
            if (!found) {
                activeSamples[i] = activeSamples[activeSamples.Count - 1];
                activeSamples.RemoveAt(activeSamples.Count - 1);
            }
        }
    }

    private bool IsFarEnough(Vector2 sample)
    {
        GridPos pos = new GridPos(sample, cellSize);

        int xmin = Mathf.Max(pos.x - 2, 0);
        int ymin = Mathf.Max(pos.y - 2, 0);
        int xmax = Mathf.Min(pos.x + 2, grid.GetLength(0) - 1);
        int ymax = Mathf.Min(pos.y + 2, grid.GetLength(1) - 1);

        for (int y = ymin; y <= ymax; y++) {
            for (int x = xmin; x <= xmax; x++) {
                Vector2 s = grid[x, y];
                if (s != Vector2.zero) {
                    Vector2 d = s - sample;
                    if (d.x * d.x + d.y * d.y < radius2) return false;
                }
            }
        }

        return true;

        // Note: we use the zero vector to denote an unfilled cell in the grid.
        // This means that if we were to randomly pick (0, 0) as a sample, it
        // would be ignored for the purposes of proximity-testing and we might
        // end up with another sample too close from (0, 0). This is a very
        // minor issue.
    }

    // Adds the sample to the active samples queue and the grid before
    // returning it
    private Vector2 AddSample(Vector2 sample)
    {
        activeSamples.Add(sample);
        GridPos pos = new GridPos(sample, cellSize);
        grid[pos.x, pos.y] = sample;
        return sample;
    }

    // Helper struct to calculate the x and y indices of a sample in the grid
    private struct GridPos
    {
        public int x;
        public int y;

        public GridPos(Vector2 sample, float cellSize)
        {
            x = (int)(sample.x / cellSize);
            y = (int)(sample.y / cellSize);
        }
    }
}
// END poisson_disc_sampler