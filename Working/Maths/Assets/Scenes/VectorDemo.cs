using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorDemo : MonoBehaviour {


    Vector3 someOtherObjectPosition {
        get {
            return Vector3.one;
        }
    }

    private void Start()
    {
        {
            // You define a Vector2 with two numbers: x and y
            // BEGIN vector2
            Vector2 direction = new Vector2(0.0f, 2.0f);
			// END vector2

            // Some built-in vectors:
            // BEGIN vector2builtin
            var up    = Vector2.up;    // ( 0,  1)
            var down  = Vector2.down;  // ( 0, -1)
            var left  = Vector2.left;  // (-1,  0)
            var right = Vector2.right; // ( 1,  0)
            var one   = Vector2.one;   // ( 1,  1)
            var zero  = Vector2.zero;  // ( 0,  0)
            // END vector2builtin
        }

        {
			// Vector3 is similar, but adds a third dimension
            // BEGIN vector3
            Vector3 point = new Vector3(1.0f, 2f, 3.5f);

            var up      = Vector3.up;      // ( 0,  1,  0)
            var down    = Vector3.down;    // ( 0, -1,  0)
            var left    = Vector3.left;    // (-1,  0,  0)
            var right   = Vector3.right;   // ( 1,  0,  0)
            var forward = Vector3.forward; // ( 0,  0,  1)
            var back    = Vector3.back;    // ( 0,  0, -1)
            var one     = Vector3.one;     // ( 1,  1,  1)
            var zero    = Vector3.zero;    // ( 0,  0,  0)
            // END vector3
        }

        {
            // Every Transform component has these vectors defined, relative
            // to their current rotation. For example:

            // BEGIN local_directions
            var myForward = transform.forward;
            // END local_directions
        } 

        {
            // Vectors can be added together
            // BEGIN vector_add_subtract
            var v1 = new Vector3(1f, 2f, 3f);
            var v2 = new Vector3(0f, 1f, 6f);

            var v3 = v1 + v2; // (1, 3, 9)
			// END vector_add_subtract
			
            // Vectors can be subtracted from each other
            // BEGIN vector_subtract
			var v4 = v2 - v1; // (-1, -1, 3)
            // END vector_subtract
        }

        {
            // The "magnitude" of a vector is its length. It's calculated
            // by taking the square root of the sum of the squares of its
            // components.

            // A vector whose magnitude is 1 is called a "unit" vector
            // BEGIN vector_magnitude
            var forwardMagnitude = Vector3.forward.magnitude; // = 1

            var vectorMagnitude = new Vector3(2f, 5f, 3f).magnitude; // ~= 6.16
			// END vector_magnitude
            // You can use this to figure out the distance between two vectors:
			// BEGIN vector_distance
            var point1 = new Vector3(5f, 1f, 0f);
            var point2 = new Vector3(7f, 0f, 2f);

            var distance = (point2 - point1).magnitude; // = 3
			// END vector_distance
            // There's also the built-in method, Distance:
            // BEGIN vector_distance2
            Vector3.Distance(point1, point2);
            // END vector_distance2

            // Calculating the magnitude of a vector requires a square root.
            // However, if you just care about finding out if one distance is
            // bigger than another, you can skip the square root. and work with
            // the square of the magnitude. Doing this is a bit faster, and we
            // care quite a lot about fast calculations in game development. 
            // To get this, use the sqrMagnitude property.
			// BEGIN vector_magnitude2
            var distanceSquared = (point2 - point1).sqrMagnitude; // = 9
			// END vector_magnitude2
            // Lots of operations work best on vectors that have a magnitude of 1.
            // A vector with magnitude of 1 is also called a "unit" vector, because
            // its magnitude is a single unit (that is, one.)

            // You can take a vector and produce a new one that has the same 
            // direction but with magnitude of 1 by dividing it by its own 
            // magnitude. 
            // This is called "normalizing" it.
			// BEGIN vector_normalise
            var bigVector = new Vector3(4, 7, 9); // magnitude = 12.08
            var unitVector = bigVector / bigVector.magnitude; // magnitude = 1
			// END vector_normalise
            // This is a common operation, so you can directly access a 
            // normalized version of a vector by using the 'normalized' property:
            // BEGIN vector_normalise2
            var unitVector2 = bigVector.normalized;
            // END vector_normalise2
        }

        {

            // You can scale a vector by a scalar (a regular number) like this:
            // BEGIN vector_scaling
            var v1 = Vector3.one * 4; // = (4, 4, 4)
            // END vector_scaling

            // You can scale a vector by another by using the Scale method. This
            // performs component-wise scaling; that is,
            // v1.Scale(v2) = (v1.x * v2.x, v1.y * v2.y, v1.z * v2.z)
            // Note that Scale modifies the vector in-place - it doesn't return
            // a new vector.
			// BEGIN vector_scaling2
            v1.Scale(new Vector3(3f, 1f, 0f)); // = (12f, 4f, 0f)
            // END vector_scaling2
        }

        {
            // The dot product measures the difference between the directions
            // that two vectors are pointing.

            // The dot product between two vectors aiming in the same direction
            // is 1:

            // BEGIN dot_product
            var parallel = Vector3.Dot(Vector3.left, Vector3.left); // 1
			// END dot_product
			
            // The dot product between two vectors aiming in opposite directions
            // is -1:
            
            // BEGIN dot_product2	
            var opposite = Vector3.Dot(Vector3.left, Vector3.right); // -1
            // END dot_product2
           
            // The dot product between two vectors at right-angles to each other
            // is 0:
            
            // BEGIN dot_product3
            var orthogonal = Vector3.Dot(Vector3.up, Vector3.forward); // 0
            // END dot_product3

            // The dot product is also the arc cosine of the angle between the
            // two vectors. (Mathf.Acos works in radians.)
           
            // BEGIN dot_product4
            var orthoAngle = Mathf.Acos(orthogonal);
            var orthoAngleDegrees = orthoAngle * Mathf.Rad2Deg; // = 90
            // END dot_product4

            // The dot product is a good way to tell if an object is in front
            // of you or behind you.
            
            // BEGIN dot_product5	
            var directionToOtherObject = someOtherObjectPosition - transform.position;
            var differenceFromMyForwardDirection = 
                Vector3.Dot(transform.forward, directionToOtherObject);

            if (differenceFromMyForwardDirection > 0) {
                // The object is in front of us
            } else if (differenceFromMyForwardDirection < 0) {
                // The object is behind us
            } else {
                // The object neither before or behind us - it's at a perfect
                // right angle to our forward direction.
            }
            // END dot_product5
        }

        {
            // The cross product between two vectors returns a third vector
            // that's orthogonal to (that is, at right angles to) both of them.

            // The cross product is only defined for 3D vectors.
            // BEGIN cross_product
            var up = Vector3.Cross(Vector3.forward, Vector3.right);
            // END cross_product

        }

        {
            // MoveTowards returns a new vector that moves from A to B, limiting
            // its total distance to a given factor. This is useful for 
            // preventing overshooting.

            // Move from (0,0,0) to (1,1,1), but don't move any further than
            // 0.5 units
			// BEGIN move_towards
            var moved = Vector3.MoveTowards(Vector3.zero, Vector3.one, 0.5f);
            // = (0.3, 0.3, 0.3) (a vector that has a magnitude of 0.5)
            // END move_towards
        }

        {
            // Reflect will bounce a vector off a plane defined by a normal
            // BEGIN vector_reflect
            var v = Vector3.Reflect(new Vector3(0.5f, -1f, 0f), Vector3.up);
            // = (0.5, 1, 0)
            // END vector_reflect
        }

        {
            // Lerp will linearly interpolate between two inputs, given a number
            // between 0 and 1. If you provide 0, you'll get the first vector,
            // if you provide 1 you'll provide the second, and if you provide
            // 0.5, you'll get somewhere right in the middle of the two.
            // BEGIN vector_lerp
            var lerped = Vector3.Lerp(Vector3.zero, Vector3.one, 0.65f);
            // = (0.65, 0.65, 0.65)
            // END vector_lerp

            // If you specify a number outside of the range of 0-1, Lerp will
            // clamp it to 0-1. If you don't want this, use LerpUnclamped:
            // BEGIN vector_lerp2
            var unclamped = Vector3.LerpUnclamped(Vector3.zero, Vector3.right, 2.0f);
            // = (2, 0, 0)
            // BEGIN vector_lerp2
            	
        }


    }

}
