using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WalkingBomb : Bomb {

	private int bombDamage = 1;
	private float walkSpeed = 2f;
	private bool detonated = false;

	public Texture2D blueBomb;
	public Texture2D redBomb;

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


	public void sprite(){
		if(owner == 1){}
		else{ 
			SpriteRenderer renderer = GetComponent<SpriteRenderer>();
			Sprite tempSprite = Sprite.Create(redBomb, new Rect(0f, 0f, redBomb.width, redBomb.height), new Vector2(.5f, .5f));
			renderer.sprite = tempSprite;
		}
	}

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
		Vector3 direction = (destination - transform.position).normalized;

		CameraManager.getInstance().follow(gameObject, callback);

		while((transform.position - destination).magnitude > 0.05f){

			Vector3 newPos = transform.position + direction * Time.deltaTime * walkSpeed;

			transform.position = newPos;
			yield return null;

		}

		newSpace.setOccupant(gameObject);
		space = newSpace;
		transform.position = destination;
		CameraManager.getInstance().release();
	}

	public override void damage(int n){
		if(n > 0){
			detonate();
		}
	}

	public override void kill(){
		space.removeOccupant();
		OccupantTracker.instance.removeOccupant(this, owner);
		Destroy(gameObject, 0f);
	}


	public override void detonate(){
		if(detonated){
			space.damageTileOnly(bombDamage);
			return;
		}

		detonated = true;
		List<GridSpace> targets = space.getManager().getSiblings(space.getPosition());
		targets.Add(space);	
		List<GridSpace> hasBombs = new List<GridSpace>();
/* 
		for(int i = 0; i < targets.Count; i++){
			if(targets[i].isOccupied() && (targets[i].getOccupant().GetType() == typeof(Bomb) && targets[i].getOccupant() != this)){
				hasBombs.Add(targets[i]);
			}
		}
*/
		targets.ForEach(gridSpace => {
			if(gridSpace == space){
				gridSpace.damageTileOnly(bombDamage);
			}
			else if(gridSpace != space && gridSpace.isOccupied() && gridSpace.getOccupant().GetComponent<Occupant>().GetType() == typeof(Bomb)){
				gridSpace.damageTileOnly(bombDamage);
				((Bomb)gridSpace.getOccupant().GetComponent<Occupant>()).detonate();
			}
			else{
				gridSpace.damage(bombDamage);
			}
		
		});

		hasBombs.ForEach(gridSpace => {
			gridSpace.getOccupant().GetComponent<Bomb>().detonate();
		});
		
		kill();
	}

}
