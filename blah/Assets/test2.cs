using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour {

	private CallbackTest callbackTest;
	// Use this for initialization
	void Start () {
		callbackTest = GetComponent<CallbackTest>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			callbackTest.startCounting(callback1);
		}

		if(Input.GetKeyDown(KeyCode.Q)){
			callbackTest.startCounting(callback2);
		}
	}

	public void callback1(){
		Debug.Log("Doing one thing");
	}

	public void callback2(){
		GameObject.CreatePrimitive(PrimitiveType.Cube);
	}
}
