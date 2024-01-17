using System;
using UnityEngine;
using System.Collections;
//using System.Diagnostics;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour {

	public bool isFly = false;
	public float speed = 6.0F;
	public float runSpeed = 10.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	public float rotateSpeed = 3.0F;
	public float gorMouseSensitivity = 1.0F;
	public float verMouseSensitivity = 1.0F;
	public float maxXAngle = 80.0F;
	public bool helpOnStart = true;

	public Light flashlight;
	public Camera cam;
	public GameObject ui;

	private Vector2 currentRotation;
	private Vector3 upCameraPosition = new Vector3(0.0F, 1.85F, 0.0F);
	private Vector3 downCameraPosition = new Vector3(0.0F, 1.0F, 0.0F);
	private float cameraSitSpeed = 10.0f;

	private bool isSit = false;
	private bool isSitMove = false;
	private bool isRun = false;

	private CharacterController player;
	private Vector3 moveDirection = Vector3.zero;


	void Start ()
	{
		player = GetComponent<CharacterController>();

		Cursor.lockState = CursorLockMode.Locked;

		if (!cam)
		{
			try
			{
				cam = GetComponentsInChildren<Camera>()[0];
			}
			catch (IndexOutOfRangeException e)
			{
				Debug.Log("Camera not found");
				Debug.Log(e);
			}

		}

		if (ui)
		{
			if (!ui.activeInHierarchy && helpOnStart)
			{
				ui.SetActive(true);
			}
			else if (ui.activeInHierarchy && !helpOnStart)
			{
				ui.SetActive(false);
			}
		}

	}
	void Update()
	{
		if (player.isGrounded || isFly) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			if (isRun) {
				moveDirection *= runSpeed;
			} 
			else 
			{
				moveDirection *= speed;
			}

			if (Input.GetButton("Jump") && !isFly)
			{
				moveDirection.y = jumpSpeed;
			}
			// Up in fly mode
			else if (Input.GetKey(KeyCode.R) && isFly)
			{
				moveDirection.y = jumpSpeed;
			}
			// Down in fly mode
			else if (Input.GetKey(KeyCode.F) && isFly)
			{
				moveDirection.y = -jumpSpeed;
			}

		}

		if (!isFly)
		{
			moveDirection.y -= gravity * Time.deltaTime;
		}

		currentRotation.x = -Input.GetAxis("Mouse Y") * verMouseSensitivity;
		currentRotation.y = Input.GetAxis("Mouse X") * gorMouseSensitivity;


		if(cam.transform.eulerAngles.x + currentRotation.x <= maxXAngle || 
			cam.transform.eulerAngles.x + currentRotation.x >= 360.0 - maxXAngle)
		{

		}
		else
		{
			currentRotation.x = 0.0F;
		}

		transform.Rotate(0, currentRotation.y, 0);
		cam.transform.Rotate(currentRotation.x, 0, 0);

		// Left mouse button pressed
		if(Input.GetMouseButtonDown(0))
		{
			if (ui)
			{
				if (ui.activeInHierarchy)
				{
					ui.SetActive(false);
				}
				else
				{
					ui.SetActive(true);
				}
			}
		}
		// Right mouse button pressed
		if(Input.GetMouseButtonDown(1))
		{
			if (isFly)
			{
				isFly = false;
			}
			else
			{
				isFly = true;
			}
		}
		// Flashlight on/off
		if (Input.GetKeyDown(KeyCode.L))
		{
			if (flashlight)
			{
				if (flashlight.isActiveAndEnabled)
				{
					flashlight.enabled = false;
				}
				else if (!flashlight.isActiveAndEnabled)
				{
					flashlight.enabled = true;
				}
			}
		}
		// Run modifier
		if (Input.GetKey(KeyCode.LeftShift))
		{
			isRun = true;
		}
		else
		{
			isRun = false;
		}
		// Sit/stand
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (!isSit)
			{
				isSit = true;
				isSitMove = true;
			}
			else
			{
				isSit = false;
				isSitMove = true;
			}
		}

		if (isSitMove)
		{
			if (isSit)
			{
				cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, downCameraPosition, cameraSitSpeed * Time.deltaTime);
			}
			else
			{
				cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, upCameraPosition, cameraSitSpeed * Time.deltaTime);
			}

			if (cam.transform.localPosition == downCameraPosition || cam.transform.localPosition == upCameraPosition)
			{
				isSitMove = false;
			}
		}




		
		player.Move(moveDirection * Time.deltaTime);
	}
}