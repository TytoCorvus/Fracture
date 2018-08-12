using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Factory : Occupant {

	public GameObject bombPrefab;

	public override void updateUI(){

	}

	void Update(){
	}

	public override void setup(GridSpace space, int owner, OccupantTracker tracker){
		base.setup(space, owner, tracker);
		turnsBetweenActions = 1;
	}
	public override void act(Action callback){
		GridPosition target = new GridPosition(space.getPosition().posX, space.getPosition().posY + 1);
		bool targetAvailable = !space.getManager().getSpace(target).isOccupied();

		if(targetAvailable){
			Debug.Log("Target available");
			GridSpace bombSpace = space.getManager().getSpace(target);
			GameObject temp = Instantiate(bombPrefab);

			Occupant o = temp.GetComponent<Occupant>();
			o.setup(bombSpace, owner, OccupantTracker.instance);
			//((WalkingBomb)o).move(callback);
			callback();
		}
		else{
			Debug.Log("Target NOT available");
			callback();}
	}
	public override void kill(){

	}

	public override void damage(int n){

	}


}
