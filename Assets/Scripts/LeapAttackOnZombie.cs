using UnityEngine;
using System.Collections;
using Leap;

public class LeapAttackOnZombie: MonoBehaviour {				
	public int attackDamage = 5;
	public float attackInterval = 2.0f;
	private float nextAttackTime;
	private float timer = 0.0f;

	private Controller _leap_controller = null;
	private SwipeGesture swipeGesture = null;

	HandModel GetHand(Collider other)
	{
		HandModel hand_model = null;
		// Navigate a maximum of 3 levels to find the HandModel component.
		int level = 1;
		Transform parent = other.transform.parent;
		while (parent != null && level < 3) {
			hand_model = parent.GetComponent<HandModel>();
			if (hand_model != null) {
				break;
			}
			parent = parent.parent;
		}

		return hand_model;
	}

	// Update is called once per frame
	void Update () {
		
	} 



	void OnTriggerEnter(Collider collider){
		HandModel hand_model = GetHand (collider);
		if (hand_model != null)
				if (timer >= attackInterval && GameManager.gm != null) {
			HandController _hand_controller = hand_model.GetController();
				if (_hand_controller != null)
					print ("_hand_controller");
				_leap_controller = _hand_controller.GetLeapController ();
				_leap_controller.EnableGesture (Gesture.GestureType.TYPE_SWIPE);
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
					ZombieHealth enemyHealth = GetComponent<ZombieHealth> ();
					if (enemyHealth != null) {
						enemyHealth.TakeDamage (attackDamage, transform.position);
						timer = 0;
					}
				}
				timer += Time.deltaTime;
	}

	void OnTriggerStay(Collider collider){
			if (collider.gameObject.tag == "Hands") {	
				if (timer >= attackInterval && GameManager.gm != null) {
					ZombieHealth enemyHealth = GetComponent<ZombieHealth> ();
					if (enemyHealth != null) {
						enemyHealth.TakeDamage (attackDamage, transform.position);
						timer = 0;
					}
				}
				timer += Time.deltaTime;
			}
		}

}

