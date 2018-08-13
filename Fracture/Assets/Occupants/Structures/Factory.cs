using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Factory : Occupant {

	public GameObject bombPrefab;

	public Texture2D player2Sprite;

	public override void updateUI(){}

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
		GridPosition target = new GridPosition(0, 0);
		if(owner == 1){
			target = new GridPosition(space.getPosition().posX, space.getPosition().posY + 1);
		} 
		else{
			target = new GridPosition(space.getPosition().posX, space.getPosition().posY - 1);
		}
		bool targetAvailable = !space.getManager().getSpace(target).isOccupied();

		if(targetAvailable){
			GridSpace bombSpace = space.getManager().getSpace(target);
			GameObject temp = Instantiate(bombPrefab);

			Occupant o = temp.GetComponent<Occupant>();
			o.setup(bombSpace, owner, OccupantTracker.instance);
			//((WalkingBomb)o).move(callback);
			callback();
		}
		else{
			callback();}
	}
	public override void kill(){

	}

	public override void damage(int n){

	}


}
