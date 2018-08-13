using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Occupant : MonoBehaviour {

	public Canvas occupantUI;
	public SpriteRenderer spriteRenderer;
	public GridSpace space;

	public int owner = 0;

	public int maxHealth = 1;
	public int currentHealth = 1;
	public int turnsBetweenActions = 1;
	public int turnsSinceLastAction = 0;

	public virtual void setup(GridSpace space, int owner, OccupantTracker tracker){
		space.setOccupant(gameObject);
		this.space = space;
		this.owner = owner;
		tracker.addOccupant(this, owner);
	}

	public virtual void endOfTurnUpdate(Action callback){
		turnsSinceLastAction++;
		if(turnsSinceLastAction == turnsBetweenActions){
			turnsSinceLastAction = 0;
			act(callback);
			
		}
	}

	public abstract void updateUI();
	public abstract void act(Action callback);

		public virtual void damage(int n){
		currentHealth -= n;
		if(n <= 0){
			kill();
		}
	}

	public virtual void kill(){
		space.removeOccupant();
		Destroy(gameObject, 0f);
	}



}
