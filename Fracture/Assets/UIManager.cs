using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text player1Score;
	public Text player2Score;

	public Text player1Lose;
	public Text player2Lose;

	public Text turnText;
	public Text playerText;

	public Text gameOverText;
	public Image gameOverButtonImage;
	public Text gameOverButtonText;

	public GameObject gameOverPanel;

	void Start(){
		gameOverPanel.SetActive(false);
	}

	public void updateScores(int p1Current, int p1Max, int p2Current, int p2Max, int p1Lose, int p2Lose, int turnNumber, int activePlayer){
		player1Score.text = p1Current + " / " + p1Max;
		player2Score.text = p2Current + " / " + p2Max;
		player1Lose.text = "Lose at " + p1Lose.ToString() + " tiles";
		player2Lose.text = "Lose at " + p2Lose.ToString() + " tiles";
		turnText.text = "Turn " + turnNumber;

		playerText.text = "Player " + activePlayer;
		if(activePlayer == 1){
			playerText.color = Color.blue;
		}
		else{
			playerText.color = Color.red;
		}

	}

	public void setGameOverText(int winner){
		gameOverPanel.SetActive(true);
		gameOverText.text = "Player " + winner + " Wins!";

		if(winner == 1){
			gameOverText.color = Color.blue;
		}
		else{
			gameOverText.color = Color.red;
		}


	}
}
