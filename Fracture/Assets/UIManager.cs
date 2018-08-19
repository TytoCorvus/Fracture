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

	private IEnumerator messageCoroutine;
	
	public Image messageBackground;
	public Text messageText;
	public float messageDuration = 1.8f;

	public Animator actionPanelAnimator; 
	public Animator buildPanelAnimator;
	void Start(){
		gameOverPanel.SetActive(false);
		//showMessage("This is a temp message!");
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

	public void showMessage(string message){
		if(messageCoroutine == null){
			messageCoroutine = messageCycle(message);
			StartCoroutine(messageCoroutine);
		}
	}

	IEnumerator messageCycle(string message){
		messageText.text = message;

		float alpha = 0f;
		while(alpha <= 1f){
			alpha += 0.02f;

			Color tempColor = messageText.color;
			tempColor.a = alpha;
			messageText.color = tempColor;

			tempColor = Color.black;
			tempColor.a = alpha * 0.5f;
			messageBackground.color = tempColor;

			yield return null;
		}

		yield return new WaitForSeconds(messageDuration);

		while(alpha >= 0f){
			alpha -= 0.02f;

			Color tempColor = messageText.color;
			tempColor.a = alpha;
			messageText.color = tempColor;

			tempColor = messageBackground.color;
			tempColor.a = alpha * 0.5f;
			messageBackground.color = tempColor;

			yield return null;
		}

		alpha = 0f;
		Color tempColor2 = messageText.color;
		tempColor2.a = alpha;
		messageText.color = tempColor2;

		tempColor2 = messageBackground.color;
		tempColor2.a = alpha * 0.5f;
		messageBackground.color = tempColor2;

		messageCoroutine = null;
	}

	public void hideActionPanel(){
		actionPanelAnimator.SetTrigger("TriggerOut");
	}

	public void showActionPanel(){
		actionPanelAnimator.SetTrigger("TriggerIn");
	}



}
