using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Cannon : Occupant {

	public GameObject cannonBallPrefab;
	public Texture2D player2Sprite;

	public override void updateUI(){

	}

	public override void setup(GridSpace space, int owner, OccupantTracker tracker){
		base.setup(space, owner, tracker);
		turnsBetweenActions = 1;
		
		if(owner == 2){
			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			Sprite tempSprite = Sprite.Create(player2Sprite, new Rect(0f, 0f, player2Sprite.width, player2Sprite.height), new Vector2(.5f, .5f));
			renderer.sprite = tempSprite;
		}
	}
	public override void act(Action callback){
		GridPosition target;

		if(owner == 1){
			target = new GridPosition(space.getPosition().posX, space.getPosition().posY + 3);
		}
		else{
			target = new GridPosition(space.getPosition().posX, space.getPosition().posY - 3);
		}

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
		Vector3 endPosition = new Vector3(t.position.x, t.position.y + 0.4f, t.position.z - space.getManager().getZDiff());

		float maxHeight = 2.8f;
		float currentPercent = 0f;

		CameraManager.getInstance().follow(temp, callback);

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
		Destroy(temp, 0f);
		
	}
}
