using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WalkingBomb : Bomb {

	private int bombDamage = 1;
	private float walkSpeed = 2f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.B)){
			detonate();
		}
	}

	public override void updateUI(){}
	public override void act(Action callback){
		move(callback);
	}

	public void move(Action callback){
		int moveValue = 0;
		if(owner == 1){
			moveValue = 1;
		}
		if(owner == 2){
			moveValue = -1;
		}

		GridPosition newSpace = new GridPosition(space.getPosition().posX, space.getPosition().posY + moveValue);

		if(space.getManager().isSpaceInGrid(newSpace) && !space.getManager().getSpace(newSpace).isOccupied()){
			space.removeOccupant();
			StartCoroutine(walkToNewSpace(space.getManager().getSpace(newSpace), callback));
		}
		else{
			callback();
		}
	}

	IEnumerator walkToNewSpace(GridSpace newSpace, Action callback){
		Transform target = newSpace.gameObject.transform;
		Vector3 destination = new Vector3(target.position.x, target.position.y + 0.75f, target.position.z - space.getManager().getZDiff());
		//Vector3 destination = new Vector3(transform.position.x + )
		Vector3 direction = (destination - transform.position).normalized;

		CameraManager.getInstance().follow(gameObject, null);

		Debug.Log("Starting grid: " + space.getPosition().posX + "," + space.getPosition().posY);
		Debug.Log("Ending grid: " + newSpace.getPosition().posX + "," + newSpace.getPosition().posY);

		Debug.Log("Starting Position: " + transform.position.ToString());
		Debug.Log("Target Position: " + destination.ToString());

		while((transform.position - destination).magnitude > 0.05f){

			Vector3 newPos = transform.position + direction * Time.deltaTime * walkSpeed;

			transform.position = newPos;
			yield return null;

		}

		newSpace.setOccupant(gameObject);
		space = newSpace;
		transform.position = destination;
		CameraManager.getInstance().release();
		callback();
	}

	public override void damage(int n){
		if(n > 0){
			kill();
		}
	}

	public override void kill(){
		space.removeOccupant();
		OccupantTracker.instance.removeOccupant(this, owner);
		Destroy(gameObject, 0f);
	}


	public override void detonate(){
		List<GridSpace> targets = space.getManager().getSiblings(space.getPosition());
		Debug.Log(targets.Count + " Siblings found");
		targets.Add(space);
		/* 
		List<GridSpace> hasBombs = new List<GridSpace>();
		for(int i = 0; i < targets.Count; i++){
			if(targets[i].isOccupied() && (targets[i].getOccupant().GetType() == typeof(Bomb) && targets[i].getOccupant() != this)){
				hasBombs.Add(targets[i]);
			}
		}

		targets.ForEach(gridSpace => {
			if(!hasBombs.Contains(gridSpace)){
				gridSpace.damage(bombDamage);
			}
		});

		hasBombs.ForEach(gridSpace => {
			gridSpace.getOccupant().GetComponent<Bomb>().detonate();
		});
		*/
		targets.ForEach(gridSpace => {
			gridSpace.damage(1);
		});
		kill();
	}

}
