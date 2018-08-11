using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WalkingBomb : Bomb {

	private int bombDamage = 1;
	private int playerOwner = 1;
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
		if(playerOwner == 1){
			moveValue = 1;
		}
		if(playerOwner == 2){
			moveValue = -1;
		}

		GridPosition newSpace = new GridPosition(space.getPosition().posX, space.getPosition().posY + moveValue);

		if(space.getManager().isSpaceInGrid(newSpace)){
			StartCoroutine(walkToNewSpace(space.getManager().getSpace(newSpace), callback));
		}
	}

	IEnumerator walkToNewSpace(GridSpace space, Action callback){
		Transform target = space.gameObject.transform;
		Vector3 destination = new Vector3(target.position.x, target.position.y, target.position.z - space.getManager().getZDiff()) + transform.position;
		Vector3 direction = (destination - transform.position).normalized;

		while((transform.position - destination).magnitude > 0.05f){
			transform.position += direction * Time.deltaTime * walkSpeed;
			yield return null;
		}

		transform.position = destination;
		callback();
	}

	public override void damage(int n){
		if(n > 0){
			kill();
		}
	}

	public override void kill(){
		Destroy(gameObject, 0f);
	}

	public override void detonate(){
		List<GridSpace> targets = space.getManager().getSiblings(space.getPosition());
		targets.Add(space);
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

		kill();
	}

}
