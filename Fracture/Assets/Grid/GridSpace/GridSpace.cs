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
	public int tileOwner;
	
	private IEnumerator bounceLoop = null;
	private IEnumerator highlightLoop = null;
	// Use this for initialization

	public void setup(GridPosition position, GridManager manager, int tileOwner){
		this.position = position;
		this.manager = manager;
		this.tileOwner = tileOwner;
		startBounce(2);
	}

	void OnMouseDown(){
		//Debug.Log("You have clicked tile" + position.posX + "," + position.posY);
		manager.gameManager.gameObject.GetComponent<InputManager>().spaceClicked(this);
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

	public void damageTileOnly(int dmg){
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
		if(isOccupied()){
			occupant.GetComponent<Occupant>().kill();
		}

		manager.removeFromGrid(position);
		manager.gameManager.panelDestroyed();
		Destroy(gameObject, 0f);
	}

	public void startBounce(int bounceNumber){
		if(bounceLoop == null){
			bounceLoop = bouncing(bounceNumber);
			StartCoroutine(bounceLoop);
		}	
	}

	IEnumerator bouncing(int num){
		float rot = 0f;
		float rotSpeed = 2f * Mathf.PI;
		float maxHeight = .4f;
		Vector3 startPos = transform.position;
		int bounceNo = 0;

		while(bounceNo < num || bounceNo == -1){
			if(num != -1){ bounceNo = (int)(rot /Mathf.PI);}

			rot += rotSpeed * Time.deltaTime;
			transform.position = new Vector3(startPos.x, startPos.y - Mathf.Sin(rot) * maxHeight, startPos.z);
			yield return null;
		}

		transform.position = startPos;
		bounceLoop = null;
	}

	public void highlight(){
		Color c = tileOwner == 1 ? Color.blue : Color.red;

		highlightLoop = highlight(c);
		StartCoroutine(highlightLoop);
	}

	IEnumerator highlight(Color c){
		float rot = 0f;
		float rotSpeed = .45f * Mathf.PI;
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();

		Color colDiff = Color.white - c;

		while(true){
			float changeVal = Mathf.Abs(Mathf.Sin(rot));
			rot += rotSpeed * Time.deltaTime;

			renderer.color = Color.white - colDiff * changeVal;
			yield return null;
		}
	}

	public void stopHighlight(){
		if(highlightLoop != null){
			StopCoroutine(highlightLoop);
		}
		highlightLoop = null;
		GetComponent<SpriteRenderer>().color = Color.white;
	}
}



