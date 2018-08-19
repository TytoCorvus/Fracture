using System.Collections;
using System.Collections.Generic;
using System;

public class OccupantTracker{

	public static OccupantTracker instance;

	private List<Occupant> player1Occupants;
	private List<Occupant> player2Occupants;

	private Queue<Occupant> updateQueue;
	private GameManager gameManager;

	public OccupantTracker(GameManager gm){
		player1Occupants = new List<Occupant>();
		player2Occupants = new List<Occupant>();
		updateQueue = new Queue<Occupant>();
		gameManager = gm;
		instance = this;
	}

	public void updateOccupants(int playerNumber){
		updateQueue.Clear();

		if(playerNumber == 1){
			//clean(player1Occupants);
			player1Occupants.ForEach(occupant => {
				if(occupant != null){
					updateQueue.Enqueue(occupant);
				}
			});
		}
		else if(playerNumber == 2){
			player2Occupants.ForEach(occupant => {
				//clean(player2Occupants);
				if(occupant != null){
					updateQueue.Enqueue(occupant);
				}
			});
		}

		nextInQueue();
	}

	public void nextInQueue(){
		Occupant occupant = null;
		if(updateQueue.Count != 0){
			occupant = updateQueue.Dequeue();
				if(occupant != null){
					occupant.endOfTurnUpdate(nextInQueue);
				}
		} 
		else if(updateQueue.Count == 0){
			queueCompleted();
		}
	}

	public void queueCompleted(){
		gameManager.updateGameState();
		gameManager.newTurnAnimation();
	}

	public void activatePlayerOccupants(int player){

	}

	public void deactivateAllOccupants(){
		
	}

	public void cleanQueue(){
		while(updateQueue.Count > 0){
			updateQueue.Dequeue();
		}
	}

	public void addOccupant(Occupant occupant, int playerNum){
		if(playerNum == 1){
			player1Occupants.Add(occupant);
		}
		if(playerNum == 2){
			player2Occupants.Add(occupant);
		}
	}

	public int getNumberOfOccupants(){
		return player1Occupants.Count;
	}

	public void removeOccupant(Occupant occupant, int playerNum){
		if(playerNum == 1){
			player1Occupants.Remove(occupant);
		}
		if(playerNum == 2){
			player2Occupants.Remove(occupant);
		}
	}

	public void clean(List<Occupant> toClean){
		for(int i = 0; i < toClean.Count; i++){
			if(toClean[i] == null){
				toClean.RemoveAt(i);
				i--;
			}
		}
	}

}
