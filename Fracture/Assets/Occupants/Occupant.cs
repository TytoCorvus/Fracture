using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Occupant : MonoBehaviour {

	public Canvas occupantUI;
	public SpriteRenderer spriteRenderer;
	public GridSpace space;

	public int maxHealth = 1;
	public int currentHealth = 1;
	public int turnsBetweenActions = 1;
	public int turnsSinceLastAction = 0;

	public virtual void endOfTurnUpdate(){
		turnsSinceLastAction++;
		if(turnsSinceLastAction == turnsBetweenActions){
			
			act();
			turnsSinceLastAction = 0;
		}
	}

	public abstract void updateUI();
	public abstract void act();

	public abstract void kill();

	public abstract void damage(int n);
}
