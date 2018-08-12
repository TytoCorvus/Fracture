using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Cannon : Occupant {

	public GameObject cannonBallPrefab;

	public override void updateUI(){

	}

	void Update(){
	}
	public override void act(Action callback){
		GridPosition target = new GridPosition(space.getPosition().posX, space.getPosition().posY + 3);
		bool targetAvailable = space.getManager().isSpaceInGrid(target);

		if(targetAvailable){
			Transform endpoint = space.getManager().getSpace(target).gameObject.transform;
			StartCoroutine(cannonLaunch(endpoint, callback));
		}
		else{
			callback();
		}
	}

	IEnumerator cannonLaunch(Transform t, Action callback){
		GameObject temp = Instantiate(cannonBallPrefab);
		temp.transform.position = transform.position + new Vector3(0.6f, 0.5f, 0f);
		Vector3 startPosition = temp.transform.position;
		Vector3 endPosition = new Vector3(t.position.x, t.position.y + 0.4f, startPosition.z);

		float maxHeight = 2.8f;
		float currentPercent = 0f;

		CameraManager.getInstance().follow(temp, null);

		while((temp.transform.position - endPosition).magnitude > 0.1f){
			currentPercent += 0.02f;

			temp.transform.position = (endPosition - startPosition) * currentPercent + startPosition;
			temp.transform.position += new Vector3(0f, (float) (-Math.Pow((currentPercent - 0.5f) * 2, 2) + 1) * maxHeight ,0f);

			yield return null;
		}

		temp.transform.position = endPosition;

		t.GetComponent<GridSpace>().damage(1);
		yield return new WaitForSeconds(0.3f);
		CameraManager.getInstance().release();
		callback();
		Destroy(temp, 0f);
		
	}

	public override void kill(){

	}

	public override void damage(int n){

	}
}
