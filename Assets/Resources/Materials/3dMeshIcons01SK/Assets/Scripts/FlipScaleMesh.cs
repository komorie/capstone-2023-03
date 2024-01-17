using UnityEngine;
using System;

public class FlipScaleMesh : MonoBehaviour {

	// Scale the model along the local x axis
	public bool scaleX = false;
	// Active camera
	private Camera activeCamera;
	// Whether the model is scaled along the z-axis (and the x-axis if the scaleX variable is true)
	private bool isScale = false;
	// Forward direction of this object
	private Vector3 forward;
	// Camera position relative to this object
	private Vector3 camPosToThis;
	// Dot product between forward direction of this object and relative camera position
	private float dotProd;


	void Update() {

		dotProd = GetDotProd();

		if((dotProd > 0.0f && isScale == true)||(dotProd < 0.0f && isScale == false))
		{
			float x;
			if(scaleX)
			{
				x = -1 * transform.localScale.x;
			}
			else
			{
				x = transform.localScale.x;
			}

			float y = transform.localScale.y;
			float z = -1 * transform.localScale.z;

			transform.localScale = new Vector3(x, y, z);

			isScale = !isScale;
		}
	}

	float GetDotProd()
	{
		activeCamera = Camera.main;

		forward = transform.TransformDirection(Vector3.forward);
		//Camera.main이 비활성화면 실행 안하게
		if (activeCamera == null)
		{
			return 0.0f;
        }
		else
		{
            camPosToThis = activeCamera.transform.position - transform.position;

            return Vector3.Dot(forward, camPosToThis);
        }
	}
}
