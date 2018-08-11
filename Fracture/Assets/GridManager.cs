using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

	public GameObject gridSpacePrefab;

	private Dictionary<GridPosition , GridSpace> grid;
	private Vector2 moveX = new Vector2(1.5f, -.8f);
	private Vector2 moveY = new Vector2(1.5f, .8f);

	private float zDiff = 0.01f;
	
	// Use this for initialization
	void Start () {
		grid = new Dictionary<GridPosition, GridSpace>();
		buildRegularMap(4, 6);
	}
	
	private void buildRegularMap(int width, int height){
		GridPosition cameraTarget;
		//cameraTarget = new GridPosition(width % 2, height % 2);
		
		for(int i=-(width/2); i < width/2 + width%2; i++){
			for(int j=-(height/2); j < height/2 + height%2; j++){
				addToGrid(new GridPosition(i, j), 0);
			}
		}
	}

	// Update is called once per frame

	public void addToGrid(GridPosition gp, int playerNumber){
		if(!grid.ContainsKey(gp)){
			GameObject temp = Instantiate(gridSpacePrefab);
			temp.transform.SetParent(transform);
			Vector2 newPos = gp.posX * moveX + gp.posY * moveY;
			temp.transform.position = new Vector3(newPos.x, newPos.y, -zDiff * (gp.posX - gp.posY));
		}
		else{
			Debug.Log("Grid space already exists");
		}
	}

	private int findCorrectSiblingIndex(GridPosition gp){
		//Needs to be at a lower index than top-left and top-right
		//Needs to be at a higher index than bot-left and bot-right
		int result = 0;
		
		return result;
	}

	public GridSpace getSpace(GridPosition gp){
		return new GridSpace();
	}
	
}

public struct GridPosition{
	public int posX, posY;
	public GridPosition(int x, int y){
		posX = x;
		posY = y;
	}
	
}

public enum GridDirection {RIGHT = 0, UP = 1, LEFT = 2, DOWN = 3}
