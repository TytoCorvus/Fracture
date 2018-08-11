using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Occupant {

	public GameObject bombPrefab;

	public override void updateUI(){

	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			act();
		}
	}
	public override void act(){
		GridPosition target = new GridPosition(space.getPosition().posX, space.getPosition().posY + 1);
		bool targetAvailable = space.getManager().isSpaceInGrid(target);


		if(targetAvailable){
			GridSpace bombSpace = space.getManager().getSpace(target);
			GameObject temp = Instantiate(bombPrefab);

			//Occupant o = temp.GetComponent<Occupant>();
			bombSpace.setOccupant(temp);
		}
	}
	public override void kill(){

	}

	public override void damage(int n){

	}
}
