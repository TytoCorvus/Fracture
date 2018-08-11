using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour {

	private GridManager manager;
	private GameObject occupant;
	// Use this for initialization
	void setup(){

	}
	
	public void setOccupant(GameObject obj){
		occupant = obj;
		obj.transform.parent = transform;
		obj.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + manager.getZDiff() / 2f);
	}

	public GameObject getOccupant(){
		return occupant;
	}

	public GameObject getAndRemoveOccupant(){
		GameObject temp = occupant;
		occupant = null;
		return temp;
	}
}



