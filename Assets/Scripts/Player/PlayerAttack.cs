using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Leap;

public class PlayerAttack : MonoBehaviour {

	public int shootingDamage = 1;				
	public float shootingRange = 50.0f;			
	public float shootingInterval = 1.0f;
	public AudioClip shootingAudio;				
	public GameObject GunShootingEffect;
	public GameObject bulletEffect;
	public Transform shootingEffectTransform;	

	private LineRenderer gunLine;		
	private bool isShooting;	
	private bool isShootingLeap;
	private Camera myCamera;			
	private Ray ray;					
	private RaycastHit hitInfo;
	private GameObject instantiation;

	private float nextShootingTime;
	private float timer = 0.0f;

	private static float LINE_RENDERER_START=0.02f;	
	private static float LINE_RENDERER_END=0.05f;	

	private HandController hand_controller;
	private Controller leap_controller;

	void Start () {
		hand_controller = GameObject.Find("RobotPlayer").GetComponentInChildren<HandController>();
		gunLine = GetComponent<LineRenderer> ();		
		if (gunLine != null) gunLine.enabled = false;	
		myCamera = GetComponentInParent<Camera> ();
		if (hand_controller != null) {
			leap_controller = hand_controller.GetLeapController ();
			leap_controller.EnableGesture (Gesture.GestureType.TYPE_SCREEN_TAP);
		}

	}
		
	void LateUpdate () {
		if (leap_controller != null) {
			leap_controller = hand_controller.GetLeapController ();
			Frame frame = leap_controller.Frame ();
			GestureList gesture = frame.Gestures ();
			if (!gesture.IsEmpty) {
				for (int i = 0; i < gesture.Count; i++) {
					Debug.Log (gesture.Count);
					isShootingLeap = frame.Gesture (i).IsValid ? true : false;
					if (isShootingLeap)
						break;
					Debug.Log (gesture [i].Type);
				}
			}
		}
		isShooting=CrossPlatformInputManager.GetButton("Fire1");	
		if ((isShootingLeap || isShooting) && timer >= shootingInterval && (GameManager.gm==null || GameManager.gm.gameState == GameManager.GameState.Playing)) {
			Shoot ();
			timer = 0;
		} else if (gunLine != null)	
			gunLine.enabled = false;
		
		timer += Time.deltaTime;
	}

	//射击函数
	void Shoot()
	{
		AudioSource.PlayClipAtPoint (shootingAudio, transform.position);	
		//枪口火焰特效
		if (GunShootingEffect != null && shootingEffectTransform != null) {								
			(Instantiate (GunShootingEffect, 
				shootingEffectTransform.position, 
				shootingEffectTransform.rotation) as GameObject).transform.parent = shootingEffectTransform;
		}
		ray.origin = myCamera.transform.position;		
		ray.direction = myCamera.transform.forward;		
		if (gunLine != null) {
			gunLine.enabled = true;							
			gunLine.SetPosition (0, transform.position);	
		}

		if (Physics.Raycast (ray, out hitInfo, shootingRange)) {
			if (hitInfo.transform.gameObject.tag.Equals ("Enemy")) {	
				ZombieHealth enemyHealth = hitInfo.transform.gameObject.GetComponent<ZombieHealth> ();
				if (enemyHealth != null) {
					enemyHealth.TakeDamage (shootingDamage, myCamera.transform.position);
				}
			}
			if (gunLine != null) {
				gunLine.SetPosition (1, hitInfo.point);	
				gunLine.SetWidth (LINE_RENDERER_START, 	
					Mathf.Clamp ((hitInfo.point - ray.origin).magnitude / shootingRange, 
						LINE_RENDERER_START, LINE_RENDERER_END));
			}
			if (bulletEffect != null) {
				Instantiate (bulletEffect, hitInfo.point, Quaternion.identity);
			}
		} else {
			if (bulletEffect != null) {
				Instantiate (bulletEffect, ray.origin + ray.direction * shootingRange, Quaternion.identity);
			}

		}

	}
}
