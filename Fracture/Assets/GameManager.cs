using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GridManager gridManager;
	public UIManager uiManager;

	private int activePlayer = 1;
	
	private int player1TotalTiles;
	private int player2TotalTiles;

	private int player1CurrentTiles;
	private int player2CurrentTiles;

	private int turnNumber;

	// Use this for initialization
	void Start () {
		turnNumber = 1;
		gridManager.setup(null);
		getBoardStatusInitial();
		updateUI();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)){
			endTurn();
		}
		
		//Janky but reliable way of switching between 1 and 2 :P
		
	}

	private void endTurn(){
		gridManager.endTurn(activePlayer);
	}

	public void updateGameState(){
		getBoardStatus();
		if(activePlayer == 2){
			turnNumber++;
		}
		activePlayer = 3 - activePlayer;
		updateUI();
	}

	private void updateUI(){
		uiManager.updateScores(player1CurrentTiles, player1TotalTiles, player2CurrentTiles, player2TotalTiles, getLosingValue(1), getLosingValue(2), turnNumber, activePlayer);
	}

	private void checkVictoryState(){

	}

	public void panelDestroyed(){
		getBoardStatus();
		updateUI();
	}

	private void getBoardStatus(){
		List<GridSpace> tempSpaces = gridManager.getWholeGrid();
		int p1temp = 0;
		int p2temp = 0;
		tempSpaces.ForEach(gs => {
			if(gs.tileOwner == 1){
				p1temp++;
			}
			else{
				p2temp++;
			}
		});

		player1CurrentTiles = p1temp;
		player2CurrentTiles = p2temp;

		checkEndGame();
	}


	private void getBoardStatusInitial(){
		List<GridSpace> tempSpaces = gridManager.getWholeGrid();
		int p1temp = 0;
		int p2temp = 0;
		tempSpaces.ForEach(gs => {
			if(gs.tileOwner == 1){
				p1temp++;
			}
			else{
				p2temp++;
			}
		});

		player1CurrentTiles = p1temp;
		player2CurrentTiles = p2temp;
		player1TotalTiles = p1temp;
		player2TotalTiles = p2temp;
	}

	private void checkEndGame(){
		int p1Losing = getLosingValue(1);
		int p2Losing = getLosingValue(2);

		if(activePlayer == 1){
			if(player1CurrentTiles <= p1Losing){
				gameOver(2);
				return;
			}
			else if(player2CurrentTiles <= p2Losing){
				gameOver(1);
				return;
			}
		}
		else if(activePlayer == 2){
			if(player2CurrentTiles <= p2Losing){
				gameOver(1);
				return;
			}
			else if(player1CurrentTiles <= p1Losing){
				gameOver(2);
				return;
			}
		}

	}

	private int getLosingValue(int playerNumber){
		if(playerNumber == 1){
			return (player1TotalTiles / 2);
		}
		else{
			return (player2TotalTiles / 2);
		}
	}

	private void gameOver(int winner){
		Debug.Log("Player " + winner + " wins!");
	}


}
