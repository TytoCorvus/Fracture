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

	public void updateScores(int p1Current, int p1Max, int p2Current, int p2Max, int p1Lose, int p2Lose, int turnNumber, int activePlayer){
		player1Score.text = p1Current + " / " + p1Max;
		player2Score.text = p2Current + " / " + p2Max;
		player1Lose.text = p1Lose.ToString();
		player2Lose.text = p2Lose.ToString();
		turnText.text = "Turn " + turnNumber;

		playerText.text = "Player " + activePlayer;

	}
}
