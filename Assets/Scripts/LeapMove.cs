using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class LeapMove : MonoBehaviour {
	public enum RotationAxes
	{
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2
	}
	public float speed = 5.0f;
	public float gravity = -9.8f;
	public float senstivityXForward = 1.2f;
	public float senstivityXBackward = 0.2f;
	public float sensitvityZForward = 0.8f;
	public float sensitvityZBackward = -0.5f;


	private CharacterController _charController;
	private HandController _handController;
	private HandModel hand_model;


	// Use this for initialization
	void Start () {
		_charController = GetComponent<CharacterController> ();
		_handController = GetComponentInChildren<HandController> ();
	}

	
	// Update is called once per frame
	void Update () {
		hand_model = GetComponentInChildren<HandModel> ();
		if (hand_model != null && _handController != null) {
			//Debug.Log (hand_model.GetWristPosition ()+this.transform.position);
			Hand hand = hand_model.GetLeapHand();
			Vector palmPosition = hand.PalmPosition;
			float deltaX = 0;
			float deltaZ = 0;
			Vector3 wristPostion = hand_model.GetWristPosition();
			Vector3 direction = wristPostion - _handController.transform.position; // world direction
			direction = transform.InverseTransformDirection(direction);// local direction
			if (direction.z >= sensitvityZForward)
				deltaZ = speed;
			if (direction.z <= sensitvityZBackward)
				deltaZ = -1*speed * 0.35f;
			//print (direction);
			Vector3 movement = new Vector3 (deltaX, 0, deltaZ);
			movement = Vector3.ClampMagnitude (movement, speed);
			movement.y = gravity;
			movement *= Time.deltaTime;
			movement = transform.TransformDirection (movement);
			_charController.Move (movement);
		}
	}
}
