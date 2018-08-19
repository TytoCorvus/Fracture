using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CallbackTest : MonoBehaviour {

	// Use this for initialization
	
	public void startCounting(Action callback){
		StartCoroutine(countToFive(callback));
	}

	IEnumerator countToFive(Action callback){

		bool done = false;
		int currentNumber = 0;

		while(!done){
			Debug.Log("Count: " + currentNumber);
			currentNumber++;
			if(currentNumber == 5){ done = true;}
			yield return new WaitForSeconds(.5f);
		}

		callback();
	}

	public void done(){
		Debug.Log("I'm done!");
	}

}
