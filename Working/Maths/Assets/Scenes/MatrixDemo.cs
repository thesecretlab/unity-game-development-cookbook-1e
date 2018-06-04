using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixDemo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
        {
            // BEGIN matrix_intro
            // A matrix is a grid of numbers, just like a vector is a column of
            // numbers.
            var matrix = new Matrix4x4();

            // You can set and get the values at the various locations in a matrix.
            var m00 = matrix[0, 0];

            matrix[0, 1] = 2f;
            // END matrix_intro
        }

        {
            // BEGIN matrix_with_vector
            // Matrices are powerful for two reasons: first, when you multiply them
            // with a vector, you get a modified vector, and those modifications
            // can be things like movement, rotation, scaling, shearing, perspective
            // projections, and more. Second, when you multiply a matrix by another
            // matrix, you get a matrix that combines the effect of both of them.

            // In computer graphics, we typically use 4x4 matrices, because they
            // can be used to perform the widest range of common geometrical 
            // transformations.

            // For example, let's create a matrix that moves ("translates") a vector 
            // by 5 units on the X axis. Don't worry too much about the details of 
            // why each number is in each location at the moment.

            // Create a new matrix using four Vector4s. Each one of these is a 
            // _column_, not a row.
            var translationMatrix = new Matrix4x4(
                new Vector4(1, 0, 0, 0),
                new Vector4(0, 1, 0, 0),
                new Vector4(0, 0, 1, 0),
                new Vector4(5, 0, 0, 1)
            );

            // This creates a matrix that looks like this:
            // 1  0  0  5
            // 0  1  0  0
            // 0  0  1  0
            // 0  0  0  1

            // When we multiply a 3-component vector by a 4x4 matrix, we add a 
            // 1 to the end of the vector (forming a 4-component vector.) (This
            // additional component is usually referred to as the 'w' component.)

            // Multiplying this matrix by a 4-component vector V performs the 
            // following result:
            // 1*Vx  +  0*Vy  +  0*Vz  +  5*Vw = resultX
            // 0*Vx  +  1*Vy  +  0*Vz  +  0*Vw = resultY
            // 0*Vx  +  0*Vy  +  1*Vz  +  0*Vw = resultZ
            // 0*Vx  +  0*Vy  +  0*Vz  +  1*Vw = resultW

            // For example, let's multiply the point (0,1,2) with this matrix.

            // First, we add our 'w' component:

            // Vx = 0, Vy = 1, Vz = 2, Vw = 1

            // 1*0  +  0*1  +  0*2  +  5*1 = 6
            // 0*0  +  1*1  +  0*2  +  0*1 = 1
            // 0*0  +  0*1  +  1*2  +  0*1 = 2
            // 0*0  +  0*1  +  0*2  +  1*1 = 1

            // We then discard the 4th component, and we have our result.

            // Our final result is therefore (6, 1, 2).

            // Rather than doing all of this work ourselves, Unity's Matrix4x4 type
            // has a MultiplyPoint method.

            var input = new Vector3(0, 1, 2);

            var result = translationMatrix.MultiplyPoint(input);
            // = (6, 1, 2)

            // You might be wondering why the matrix has the 4th row at all,
            // since it just means we need to add and remove a useless fourth
            // component to our vectors. The reason why it's there is that the
            // fourth row is necessary for operations like perspective projections.

            // However, if you're only doing transformations like translations, 
            // rotations and scales, you can get away with only using part of the
            // matrix, and can use Matrix4x4's MultiplyPoint4x3 instead. It's
            // a bit faster, but can only be used for translations, rotations
            // and scales.
            // END matrix_with_vector
        }

        {
            // BEGIN matrix_translate
            var input = new Vector3(0, 1, 2);

            // You can also create a matrix that translates a point using
            // helper methods:

            var translationMatrix = Matrix4x4.Translate(new Vector3(5, 1, -2));
            var result = translationMatrix.MultiplyPoint(input);
            // = (5, 2, 0)
            // END matrix_translate
        }
        {
            // BEGIN matrix_rotate
            // Matrices can also rotate a point around the origin. First,
            // create a quaternion that describes the rotation:

            var rotate90DegreesAroundX = Quaternion.Euler(90, 0, 0);

            var rotationMatrix = Matrix4x4.Rotate(rotate90DegreesAroundX);

            var input = new Vector3(0, 0, 1);

            var result = rotationMatrix.MultiplyPoint(input);
            // = (0, -1, 0); the point has moved from in front of the origin to
            // below it

            // If your vector represents a direction, and you want to use 
            // a matrix to rotate the vector, you can use MultiplyVector. This
            // method uses only the parts of the matrix that are necessary to
            // do a rotation. It's a bit faster.
            result = rotationMatrix.MultiplyVector(input);
            // = (0, -1, 0) - the same result.
            // END matrix_rotate
        }

        {
            // BEGIN matrix_scale
            // Matrices can also scale a point away from the origin.

            var scale2x2x2 = Matrix4x4.Scale(new Vector3(2f, 2f, 2f));

            var input = new Vector3(1f, 2f, 3f);

            var result = scale2x2x2.MultiplyPoint3x4(input);
            // = (2, 4, 6)
            // END matrix_scale
        }

        {
            // BEGIN matrix_concatenate
            // When you multiply matrices together, you get a new matrix that,
            // when multiplied with a vector, produces the same result as if
            // you'd multiplied the vector by each of the original matrices in 
            // order. In other words, if you think of matrices as an "instruction"
            // to modify a point, you can combine multiple matrices into a single
            // step. When you combine matrices together like this, we call it
            // "concatenating" the matrices.

            var translation = Matrix4x4.Translate(new Vector3(5, 0, 0));
            var rotation = Matrix4x4.Rotate(Quaternion.Euler(90, 0, 0));
            var scale = Matrix4x4.Scale(new Vector3(1, 5, 1));

            var combined = translation * rotation * scale;

            var input = new Vector3(1, 1, 1);
            var result = combined.MultiplyPoint(input);
            Debug.Log(result);
            // = (6, 1, 5)

            // Note that the order of multiplication matters! Matrix multiplication
            // is not "commutative", while multiplying regular numbers is.

            // For example, 2 * 5 == 5 * 2 == 10.
            // But, translation * rotation != rotation * translation.
            // This makes sense, because translating and then rotating a point
            // will produce a different result than rotation and then translating 
            // it.

            // Combining matrices with multiplication will apply them in reverse
            // order of multiplication. Given a point P and matrices A, B and C:

            // P * (A * B * C) == (A * (B * (C * P)))

            // You can create a combined translate-rotate-scale matrix using
            // the Matrix4x4.TRS method:
            var transformMatrix = Matrix4x4.TRS(
                new Vector3(5, 0, 0),
                Quaternion.Euler(90, 0, 0),
                new Vector3(1, 5, 1)
            );

            // This new matrix will scale, rotate, and then translate any point you
            // apply it to.
            // END matrix_concatenate

        }

        {
            // BEGIN matrix_transform
            // You can get the matrix that converts a point in this component's
            // local space to world space, applying the translation, rotation,
            // and scaling from this object (as well as all of its parents.)

            var localToWorld = this.transform.localToWorldMatrix;

            // You can also get the matrix that converts from world-space to
            // local space, too.
            var worldToLocal = this.transform.worldToLocalMatrix;
            // END matrix_transform
        }







	}
	
}
