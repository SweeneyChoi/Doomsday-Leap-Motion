using UnityEngine;
using System.Collections;
using Leap;

public class LeapAttack: MonoBehaviour {				
	public int attackDamage = 1;
	public float attackInterval = 1.0f;
	private float nextAttackTime;
	private float timer = 0.0f;

	private Controller _leap_controller;
	private SwipeGesture swipeGesture = null;
	// Use this for initialization
	void Start () {
		HandController _hand_controller = GetComponentInParent<HandController> ();
		if (_hand_controller == null)
			print ("NULL hand");
		_leap_controller = _hand_controller.GetLeapController ();
		_leap_controller.EnableGesture (Gesture.GestureType.TYPE_SWIPE);
		_leap_controller.EnableGesture (Gesture.GestureType.TYPE_SCREEN_TAP);
	}
	/*
	// Update is called once per frame
	void Update () {
		if (_leap_controller != null) {
			Debug.Log ("_leap_controller is connected!");
			Frame frame = _leap_controller.Frame();
			if (!frame.Gestures ().IsEmpty) {
				for (int i = 0; i < frame.Gestures ().Count; i++) {
					if(frame.Gesture(i).Type == Gesture.GestureType.TYPE_SWIPE){
						Gesture gesture = frame.Gesture (i);
						swipeGesture = new SwipeGesture(gesture);
					}
				}
			}
		}
	} 
	*/


	void OnTriggerEnter(Collider collider){
		if (swipeGesture != null) {
			Debug.Log ("hey swipe your hands!");
			if (collider.gameObject.tag == "Enemy") {	
				if (timer >= attackInterval && GameManager.gm != null) {
					ZombieHealth enemyHealth = collider.transform.gameObject.GetComponent<ZombieHealth> ();
					if (enemyHealth != null) {
						enemyHealth.TakeDamage (attackDamage, transform.position);
						timer = 0;
					}
				}
				timer += Time.deltaTime;
			}
		}
	}

	void OnTriggerStay(Collider collider){
		if (swipeGesture != null) {
			if (collider.gameObject.tag == "Enemy") {	
				if (timer >= attackInterval && GameManager.gm != null) {
					ZombieHealth enemyHealth = collider.transform.gameObject.GetComponent<ZombieHealth> ();
					if (enemyHealth != null) {
						enemyHealth.TakeDamage (attackDamage, transform.position);
						timer = 0;
					}
				}
				timer += Time.deltaTime;
			}
		}
	}

}

