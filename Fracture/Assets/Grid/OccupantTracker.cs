using System.Collections;
using System.Collections.Generic;

public class OccupantTracker{

	private List<Occupant> player1Occupants;
	private List<Occupant> player2Occupants;

	public void updateOccupants(int playerNumber){

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
