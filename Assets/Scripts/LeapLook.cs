using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class LeapLook : MonoBehaviour {
	public enum RotationAxes
	{
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2
	}
	public RotationAxes axes = RotationAxes.MouseXAndY;

	public float sensitivityLeap = 0.12f; // 手势旋转视最小范围

	public float sensitivityHor = 9.0f;
	public float sensitivityVert = 9.0f;

	public float minimumVert = -45.0f;
	public float maxmumVert = 45.0f;

	private float _rotationX = 0;

	private HandController _handController;
	private HandModel hand_model;

	void Start() {
		Rigidbody body = GetComponent<Rigidbody> ();
		if (body != null)
			body.freezeRotation = true;
		_handController = GetComponentInChildren<HandController> ();

	}

	// Update is called once per frame
	void Update () {
		 hand_model = GetComponentInChildren<HandModel> ();
		if (hand_model != null && _handController != null) {
			
			Hand hand = hand_model.GetLeapHand();
			Vector palmPosition = hand.PalmPosition;
			Vector3 wristPostion = hand_model.GetWristPosition();
			Vector3 direction = wristPostion - _handController.transform.position; // world direction
			direction = transform.InverseTransformDirection(direction);// local direction
			if (axes == RotationAxes.MouseX) {
				if(Mathf.Abs(direction.x) >= sensitivityLeap)
					transform.Rotate (0,  direction.x * sensitivityHor, 0);
			} else if (axes == RotationAxes.MouseY) {
				_rotationX -= Input.GetAxis ("Mouse Y") * sensitivityHor;
				_rotationX = Mathf.Clamp (_rotationX, minimumVert, maxmumVert);

				float rotationY = transform.localEulerAngles.y;

				transform.localEulerAngles = new Vector3 (_rotationX, rotationY, 0);
			} else {
				_rotationX -= Input.GetAxis ("Mouse Y") * sensitivityVert;
				_rotationX = Mathf.Clamp (_rotationX, minimumVert, maxmumVert);

				float delta = Input.GetAxis ("Mouse X") * sensitivityHor;
				float rotationY = transform.localEulerAngles.y + delta;

				transform.localEulerAngles = new Vector3 (_rotationX, rotationY, 0);
			}
		}
		}
	}

	