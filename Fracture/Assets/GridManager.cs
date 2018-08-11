using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {

	public GameObject gridSpacePrefab;
	public GameObject cannonPrefab;

	public Texture2D blueTileImage;
	public Texture2D redTileImage;

	private Dictionary<GridPosition , GridSpace> grid;
	private OccupantTracker occupantTracker;
	private Vector2 moveX = new Vector2(1.5f, -.8f);
	private Vector2 moveY = new Vector2(1.5f, .8f);

	private float zDiff = 0.01f;
	
	// Use this for initialization
	void Start () {
		grid = new Dictionary<GridPosition, GridSpace>();
		buildRegularMap(4, 6);
		GridSpace temp = getSpace(new GridPosition(-2, -2));
		GameObject tempObj = Instantiate(cannonPrefab);
		if(temp == null){
			Debug.Log("temp is null");
		}
		temp.setOccupant(tempObj);
		Cannon c = tempObj.GetComponent<Cannon>();
		c.space = temp;
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.Space)){
			turnPassed();
		}
	}

	private void buildRegularMap(int width, int height){
		GridPosition cameraTarget;
		//cameraTarget = new GridPosition(width % 2, height % 2);
		
		for(int i=-(width/2); i < width/2 + width%2; i++){
			for(int j=-(height/2); j < height/2 + height%2; j++){
				if(j < 0){
					addToGrid(new GridPosition(i, j), 0);
				}
				else{
					addToGrid(new GridPosition(i, j), 1);
				}
			}
		}
	}

	// Update is called once per frame

	
	public bool isSpaceInGrid(GridPosition gp){
		return grid.ContainsKey(gp);
	}
	public GridSpace getSpace(GridPosition gp){
		return grid[gp];
	}

	public void addToGrid(GridPosition gp, int playerNumber){
		//Do nothing if it already exists in the dictionary
		if(grid.ContainsKey(gp)){
			Debug.Log("Cannot add - Grid space already exists");
			return;
		}

		
		GameObject temp = Instantiate(gridSpacePrefab);
		temp.transform.SetParent(transform);
		Vector2 newPos = gp.posX * moveX + gp.posY * moveY;
		temp.transform.position = new Vector3(newPos.x, newPos.y, -zDiff * (gp.posX - gp.posY));

		SpriteRenderer sprite = temp.GetComponent<SpriteRenderer>();
		Texture2D tex;

		switch(playerNumber){
			case 0:
				tex = blueTileImage;
				break;
			case 1:
				tex = redTileImage;
				break;
			default:
				tex = blueTileImage;
				break;
		}

		sprite.sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(.5f, .5f));


		GridSpace space = temp.GetComponent<GridSpace>();
		space.setup(gp, this);
		grid.Add(gp, space);
	}

	public void removeFromGrid(GridPosition gp){
		if(grid.ContainsKey(gp)){
			grid.Remove(gp);
		}
	}

	public void addtoSpace(GridPosition gp, GameObject new_occupant){
		if(grid.ContainsKey(gp)){
			grid[gp].setOccupant(new_occupant);
		}
	}


	public float getZDiff(){
		return zDiff;
	}

	public List<GridSpace> getSiblings(GridPosition gp){
		List<GridSpace> result = new List<GridSpace>();

		if(grid.ContainsKey(new GridPosition(gp.posX, gp.posY + 1))){
			result.Add(grid[new GridPosition(gp.posX, gp.posY + 1)]);
		}
		if(grid.ContainsKey(new GridPosition(gp.posX, gp.posY - 1))){
			result.Add(grid[new GridPosition(gp.posX, gp.posY - 1)]);
		}
		if(grid.ContainsKey(new GridPosition(gp.posX - 1, gp.posY))){
			result.Add(grid[new GridPosition(gp.posX - 1, gp.posY)]);
		}
		if(grid.ContainsKey(new GridPosition(gp.posX + 1, gp.posY))){
			result.Add(grid[new GridPosition(gp.posX + 1, gp.posY)]);
		}

		return result;
	}

	public void turnPassed(){

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
