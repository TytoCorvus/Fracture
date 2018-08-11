using System.Collections;
using System.Collections.Generic;

public class OccupantTracker{

	private List<Occupant> player1Occupants;
	private List<Occupant> player2Occupants;

	private Queue<Occupant> updateQueue;

	public OccupantTracker(){
		player1Occupants = new List<Occupant>();
		player2Occupants = new List<Occupant>();
		updateQueue = new Queue<Occupant>();
	}

	public void updateOccupants(int playerNumber){
		updateQueue.Clear();
		player1Occupants.ForEach(occupant => {
			updateQueue.Enqueue(occupant);
		});
		nextInQueue();
	}

	public void nextInQueue(){
		Occupant occupant = null;
		if(updateQueue.Count != 0){
			occupant = updateQueue.Dequeue();
		} 
		if(occupant != null){
			occupant.endOfTurnUpdate(nextInQueue);
		}
		if(updateQueue.Count == 0){
			queueCompleted();
		}
	}

	public void queueCompleted(){

	}

	public void addOccupant(Occupant occupant, int playerNum){
		if(playerNum == 1){
			player1Occupants.Add(occupant);
		}
		if(playerNum == 2){
			player2Occupants.Add(occupant);
		}
	}
}
