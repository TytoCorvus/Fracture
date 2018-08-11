using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour {

	private GridManager manager;
	private GameObject occupant;

	private GridPosition position;
	// Use this for initialization
	public void setup(GridPosition position, GridManager manager){
		this.position = position;
		this.manager = manager;
	}
	
	public void setOccupant(GameObject obj){
		occupant = obj;
		obj.transform.SetParent(transform);
		obj.transform.position = obj.transform.position + transform.position - new Vector3(0f, 0f, manager.getZDiff() / 2);
	}

	public GameObject getOccupant(){
		return occupant;
	}

	public GameObject getAndRemoveOccupant(){
		GameObject temp = occupant;
		occupant = null;
		return temp;
	}

	public GridManager getManager(){
		return manager;
	}

	public GridPosition getPosition(){
		return position;
	}
}



