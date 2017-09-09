using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// // from answers.unity3d.com/questions/1188342


public class controllermovement : MonoBehaviour
{
	public GameObject player;

	SteamVR_Controller.Device device;
	SteamVR_TrackedObject controller;

	Vector2 touchpad;

	float sensitivityX = 0.75F;
	float sensitivityForward = 1.5F;
	private Vector3 playerPos;

	void Start()
	{
		controller = gameObject.GetComponent<SteamVR_TrackedObject>();
	}

	// Update is called once per frame
	void Update()
	{
		device = SteamVR_Controller.Input((int)controller.index);
		//If finger is on touchpad
		if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
		{
			//Read the touchpad values
			touchpad = device.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);


			// Handle movement via touchpad
			if (touchpad.y > 0.2f || touchpad.y < -0.2f) {
				// Move Forward
				player.transform.position += controller.transform.forward * Time.deltaTime * (touchpad.y * sensitivityForward);

			}

			// handle rotation via touchpad
			if (touchpad.x > 0.3f || touchpad.x < -0.3f) {
				player.transform.Rotate (0, touchpad.x * sensitivityX, 0);
				// center of rotation is wonky in evl demoi as vive camerarig has an offset that needs to be moved up
				// band then rotation rotates about the center of the tracked vive area not the user
				// and laser pointer script needs to affect the new (parent( wand movement script not the (child) camera
			}

			//Debug.Log ("Touchpad X = " + touchpad.x + " : Touchpad Y = " + touchpad.y);
		}
	}
}