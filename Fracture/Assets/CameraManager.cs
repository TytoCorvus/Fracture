using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	private static bool available = true;
	private static bool released = false; 
	private static GameObject following = null;
	private static Vector3 neutralPosition = new Vector3(0f, 0f, -10f);
	private static float neutralCamSize = 6f;
	private static Camera camera;
	private static CameraManager instance;

	void Start(){
		camera = GetComponent<Camera>();
		instance = this;
	}

	public static bool getAvailable(){
		return available;
	}

	public static CameraManager getInstance(){
		return instance;
	}

	public void follow(GameObject g, Action callback, bool zoom = false, float zoomAmt = 0f){
		if(available){
			if(zoom){
				zoomThenFollow();
				return;
			}
			CameraManager.following = g;
			available = false;
			StartCoroutine(followObject(callback));
		}
	}

	private void zoomThenFollow(){

	}

	public void release(){
		released = true;
	}
	IEnumerator followObject(Action callback){

		while(!released){
			if(CameraManager.following != null){
				Vector3 newPos = transform.position;
				Vector3 followingInCamPlane = new Vector3(following.transform.position.x, following.transform.position.y, transform.position.z);
				newPos = Vector3.Lerp(newPos, followingInCamPlane, 0.7f);
				transform.position = newPos;

			}
			yield return null;
		}

		Debug.Log("released!");
		following = null;
		released = false;
		StartCoroutine(returnToNeutral(callback));
	}

	IEnumerator returnToNeutral(Action callback){
		while((transform.position - neutralPosition).magnitude > 0.05f){
			Vector3 newPos = transform.position;
			newPos = Vector3.Lerp(newPos, neutralPosition, 0.5f);
			transform.position = newPos;
			yield return null;
		}

		Debug.Log("Returned to neutral");
		transform.position = neutralPosition;
		available = true;

		if(callback != null){
			callback();
		}
		
	}


}
