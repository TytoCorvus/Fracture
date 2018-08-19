using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

	public UIManager uiManager;
	public GridManager gridManager;

	private GameManager gameManager;

	private bool placedThisTurn = false;
	private bool movedThisTurn = false;
	private bool destroyedThisTurn = false;
	private InputState inputState = InputState.NONE;

	public GameObject factoryPrefab;
	public GameObject cannonPrefab;

	private GameObject selectedBuild;
	private GameObject selectedOccupant;
	private List<GridSpace> moveSpaces;

	public GameObject actionPanel;


	void Start(){
		gameManager = GetComponent<GameManager>();
	}

	public void spaceClicked(GridSpace gridSpace){
		switch(inputState){
			case InputState.PLACE:
				if(selectedBuild != null && !placedThisTurn && gridSpace.tileOwner == gameManager.getActivePlayer()){
					GameObject newBuild = Instantiate(selectedBuild);
					gridSpace.startBounce(1);
					newBuild.GetComponent<Occupant>().setup(gridSpace, gameManager.getActivePlayer(), gridManager.getOccupantTracker());
					placedThisTurn = true;
					toActionPanel();
				}
				break;
			case InputState.MOVE:
				if(gridSpace.isOccupied() && gridSpace.tileOwner == gameManager.getActivePlayer() && gridSpace.getOccupant().GetComponent<Occupant>().owner == gameManager.getActivePlayer()){
					selectedOccupant = gridSpace.getOccupant();
					if(moveSpaces != null){ moveSpaces.ForEach(space => { space.stopHighlight();});}

					moveSpaces = gridManager.getMoveSpaces(gridSpace);
					moveSpaces.ForEach(space => {
						space.highlight();
					});
				}
				else if(selectedOccupant != null && moveSpaces != null && !gridSpace.isOccupied()){
					selectedOccupant.GetComponent<Occupant>().space.removeOccupant();
					if(moveSpaces != null){ moveSpaces.ForEach(space => { space.stopHighlight();});}
					gridSpace.setOccupant(selectedOccupant);
					selectedOccupant.GetComponent<Occupant>().space = gridSpace;
					moveSpaces = null;
					selectedOccupant = null;
					movedThisTurn = true;
					updateButtons();
				}
				
				break;
			case InputState.DESTROY:
				if(gridSpace.tileOwner == gameManager.getActivePlayer() && gridSpace.isOccupied() && gridSpace.getOccupant().GetComponent<Occupant>().owner == gameManager.getActivePlayer()){
					gridSpace.getOccupant().GetComponent<Occupant>().kill();
					destroyedThisTurn = true;
					setInputState(InputState.NONE);
					updateButtons();
				}
				break;
			case InputState.NONE:
				break;
			default:
				break;
		}

	}

	public void setInputState(InputState inState){
		switch(inState){
			case InputState.PLACE:
				if(placedThisTurn){ showNegative(inState.ToString());}
				else{
					if(!(inState == InputState.MOVE) && moveSpaces != null){ moveSpaces.ForEach(space => { space.stopHighlight();});}
					inputState = inState;
				}
				break;
			case InputState.MOVE:
				if(movedThisTurn){showNegative(inState.ToString());}
				else{
					if(!(inState == InputState.MOVE) && moveSpaces != null){ moveSpaces.ForEach(space => { space.stopHighlight();});}
					inputState = inState;
					selectedOccupant = null;
					moveSpaces = null;
				}
				break;
			case InputState.DESTROY:
				if(destroyedThisTurn){showNegative(inState.ToString());}
				else{
					inputState = inState;
					if(!(inState == InputState.MOVE) && moveSpaces != null){ moveSpaces.ForEach(space => { space.stopHighlight();});}
					}
				break;
			case InputState.NONE:
				inputState = inState;
				if(!(inState == InputState.MOVE) && moveSpaces != null){ moveSpaces.ForEach(space => { space.stopHighlight();});}
				break;
			default:
				if(!(inState == InputState.MOVE) && moveSpaces != null){ moveSpaces.ForEach(space => { space.stopHighlight();});}
				break;
		}
	}

	public void endTurn(){
		reset();
		gameManager.endTurn();
	}

	public void reset(){
		placedThisTurn = movedThisTurn = destroyedThisTurn = false;
		setInputState(InputState.NONE);
		updateButtons();
	}

	private void showNegative(string str){
		uiManager.showMessage(str);
	}

	public void destroyButtonClick(){
		setInputState(InputState.DESTROY);
	}

	public void moveButtonClick(){
		setInputState(InputState.MOVE);
	}

	public void toBuildPanel(){
		if(!placedThisTurn){
			setInputState(InputState.PLACE);
			StartCoroutine(returnToBP());
		}	
	}

	IEnumerator returnToBP(){
		uiManager.actionPanelAnimator.SetTrigger("TriggerOut");
		yield return null;
		uiManager.buildPanelAnimator.SetTrigger("TriggerIn");
	}

	public void toActionPanel(){
		setInputState(InputState.NONE);
		updateButtons();
		StartCoroutine(returnToAP());
	}

	IEnumerator returnToAP(){
		uiManager.buildPanelAnimator.SetTrigger("TriggerOut");
		yield return null;
		uiManager.actionPanelAnimator.SetTrigger("TriggerIn");
		
	}

	public void updateButtons(){

		actionPanel.transform.Find("PlaceButton").GetComponent<Button>().interactable = !placedThisTurn;
		actionPanel.transform.Find("DeleteButton").GetComponent<Button>().interactable = !destroyedThisTurn;
		actionPanel.transform.Find("MoveButton").GetComponent<Button>().interactable = !movedThisTurn;
	}

	public void buildSelect(string str){
		if(inputState == InputState.PLACE){
			switch(str){
				case "Factory":
					selectedBuild = factoryPrefab;
					break;
				case "Cannon":
					selectedBuild = cannonPrefab;
					break;
				default:
					//selectedBuild = null;
					break;
			}
		}
	}

}

public enum InputState {PLACE, MOVE, DESTROY, NONE}
