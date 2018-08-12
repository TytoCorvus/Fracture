using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour {

	public SpriteRenderer cracksSpriteRenderer;

	private GridManager manager;
	private GameObject occupant;

	private GridPosition position;

	private int spaceMaxHealth = 3;
	private int spaceCurrentHealth = 3;
	
	// Use this for initialization

	public void setup(GridPosition position, GridManager manager){
		this.position = position;
		this.manager = manager;
	}
	
	public void setOccupant(GameObject obj){
		if(isOccupied()){
			return;
		}
		occupant = obj;
		obj.transform.SetParent(transform);
		//obj.transform.position = obj.transform.position + transform.position - new Vector3(0f, 0f, manager.getZDiff() / 2);
		obj.transform.position = transform.position - new Vector3(0f, -.75f, manager.getZDiff() / 2);
	}

	public GameObject getOccupant(){
		return occupant;
	}

	public GameObject getAndRemoveOccupant(){
		GameObject temp = occupant;
		occupant = null;
		return temp;
	}

	public void removeOccupant(){
		occupant = null;
	}

	public GridManager getManager(){
		return manager;
	}

	public GridPosition getPosition(){
		return position;
	}

	public bool isOccupied(){
		if(occupant != null){
			return true;
		}

		return false;
	}

	public void damage(int dmg){
		if(isOccupied()){
			occupant.GetComponent<Occupant>().damage(dmg);
			return;
		}
		spaceCurrentHealth -= dmg;
		updateStatus();		
	}

	public void repair(int val){
		spaceCurrentHealth += val;
		if(spaceCurrentHealth > spaceMaxHealth){
			spaceCurrentHealth = spaceMaxHealth;
		}
		updateStatus();
	}

	private void updateStatus(){
		if(spaceCurrentHealth == spaceMaxHealth){
			cracksSpriteRenderer.sprite = null;
		}
		if(spaceCurrentHealth == 2){
			Texture2D tex = manager.cracks1;
			cracksSpriteRenderer.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(.5f, .5f));
		}
		else if(spaceCurrentHealth == 1){
			Texture2D tex = manager.cracks2;
			cracksSpriteRenderer.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(.5f, .5f));
		}
		else if(spaceCurrentHealth <= 0){
			die();
		}
	}

	private void die(){
		manager.removeFromGrid(position);
		Destroy(gameObject, 0f);
	}
}



