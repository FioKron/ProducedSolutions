import java.awt.event.MouseEvent;
import java.util.ArrayList;

import javax.swing.JLabel;

/**
 * This is the control class for the dig for diamonds GUI.
 * This class controls most aspects of the game such as deciding if someone can take a turn,
 * or if someone has won the game.
 * 
 * @author James Moran.
 * @version 1.0.0 as of 20/03/2012
 *
 */

public class DigForDiamonds 
{
	
	/**
	 * Default blank constructor.
	 */

	public DigForDiamonds()
	{

	}

	/**
	 * Do the first stage of making a move; taking the diamonds from the pocket the player chose.
	 * @param numberOfPlayers The number of players playing this instance of the game.   
	 * @param thisPocket The pocket that the player chose to take diamonds from.
	 * @return diamondsToRedestribute // The diamonds that need to be distributed to adjacent pockets.
	 */

	private int makeMove(int numberOfPlayers,MutablePocket thisPocket) // Make a move
	{
		int diamondsToRedestribute = thisPocket.removeDiamonds(); // Remove the diamonds from the selected pocket.. 
		return diamondsToRedestribute; // Return the value.
	}

	/**
	 * Control method for the forfeit button; handles what happens when one of the players gives up.
	 * @param numberOfPlayers The number of players playing this instance of the game.  
	 * @param players An arraylist that holds information on all of the players currently playing.
	 * @param labelGameState This label displays the state of the game.
	 * @param gameState A string version of the GameState enum.
	 * @return gameState A string version of the GameState enum. 
	 */

	public String forfeit(int numberOfPlayers,ArrayList<Player> players,JLabel labelGameState,String gameState) // One of the players forfeits the game.
	{
		if (gameState != "aPlayerHasWon") // Someone can only give up if no one has won.
		{
			switch (numberOfPlayers) // If no one has given up...
			{
			case 2:
				if (players.get(0).getTurnState() == true) // If it is player one's turn...
				{ // They give up and player 2 wins by default.
					labelGameState.setText(players.get(0).getPlayerName() + " gives up, " + players.get(1).getPlayerName() + " wins!");	
					gameState = "aPlayerHasWon";
				}
				else if (players.get(1).getTurnState() == true) // If it is player two's turn...
				{// They give up and player 1 wins by default.
					labelGameState.setText(players.get(1).getPlayerName() + " gives up, " + players.get(0).getPlayerName() + " wins!");		
					gameState = "aPlayerHasWon";
				}	
				break;
			}		
		}

		return gameState; // Return the state of the game.

	}

	/**
	 * Whenever a player left clicks on a small pocket,
	 * this method will see if the player can take a turn via a series of checks.
	 * @param players An arraylist that holds information on all of the players currently playing.
	 * @param goalPockets An arraylist that holds information on all of the Immutable pockets in play.
	 * @param playerPockets An arraylist that holds information on all of the mutable pockets in play.
	 * @param totalPlayers The total number of players playing.
	 * @param pocketPressed The pocket that has been clicked on.
	 * @param labelGameState This label displays the state of the game. 
	 * @param gameState A string version of the GameState enum.
	 * @return gameState A string version of the GameState enum.
	 */

	public String attemptTurn(ArrayList<Player> players,ArrayList<ImmutablePocket> goalPockets
			,ArrayList<MutablePocket> playerPockets,int totalPlayers,MouseEvent pocketPressed,JLabel labelGameState,String gameState) // Attempt to take a turn.
	{
		int diamondsToDistibute = 0; // The diamonds to distribute to adjacent pockets.
		boolean anotherGo = false;

		for (Player thisPlayer: players) // for each player..
		{
			for (MutablePocket thisPocket: playerPockets) // and for each mutable pocket...
			{

				if (thisPlayer.getTurnState() == true // check if it is the player's turn, the pocket they clicked on was this one,the pocket belongs to them AND the pocket has diamonds in it AND it's enabled.
						&& pocketPressed.getSource() == thisPocket 
						&& thisPocket.getOwnerName().equalsIgnoreCase(thisPlayer.getPlayerName())
						&& thisPocket.getDiamondCount() > 0
						&& thisPocket.getEnabled() == true)
				{
					diamondsToDistibute = makeMove(totalPlayers,thisPocket); // If this is so make a move.
					switch (totalPlayers) // Then..
					{
					case 2: // If two players are playing...
						anotherGo = redistributeDiamonds(diamondsToDistibute, players, goalPockets, playerPockets, thisPlayer, thisPocket, totalPlayers); // Redistribute the diamonds.
						thisPocket.update(); // Update the pocket.

						if (isThisPlayersPocketsEmpty(totalPlayers, playerPockets) == false) // Only let players take more turns if one player's pockets are not empty.
						{
							if (anotherGo == false) // Only make it the other players turn if this player does not get another go.
							{
								if (thisPlayer.getPlayerID() == 0) // So make it the other player's turn.
								{
									players.get(1).setIsPlayerTurn(true);
									thisPlayer.setIsPlayerTurn(false); // It is now no longer this player's turn..
									labelGameState.setText("It is now " + players.get(1).getPlayerName() +  "'s turn.");
									gameState = "twoPlayersNowPlayerOnesTurn";
								}
								else
								{
									players.get(0).setIsPlayerTurn(true);
									thisPlayer.setIsPlayerTurn(false); // It is now no longer this player's turn..
									labelGameState.setText("It is now " + players.get(0).getPlayerName() +  "'s turn.");
									gameState = "twoPlayersNowPlayerTwosTurn";
								}

							}

						}
						else // Someone's pockets (The player who's go it is) are empty so..
						{
							takeRemainingDiamonds(thisPlayer, totalPlayers, playerPockets, goalPockets); // That player gets to take the remaining diamonds.
							determineWinner(players, totalPlayers,labelGameState); // Then the winner can be determined.
							gameState = "aPlayerHasWon";

						}

						break;
					}
				} // Just below does not work, need fix, possible operator error with != not working for strings, look into it.
				else if (thisPlayer.getTurnState() == true // Now the code will check for each of the cases in which the a pocket should not be selected..
						&& pocketPressed.getSource() == thisPocket 
						&& thisPocket.getOwnerName() != thisPlayer.getPlayerName() // This one checks to see if the pocket does not belong to them
						&& thisPocket.getDiamondCount() > 0
						&& thisPocket.getEnabled() == true) 
				{
					labelGameState.setText("invalid pocket; Please select one of your own pockets."); // Inform the player if this is the case.
				}
				else if (thisPlayer.getTurnState() == true
						&& pocketPressed.getSource() == thisPocket 
						&& thisPocket.getOwnerName().equalsIgnoreCase(thisPlayer.getPlayerName()) 
						&& thisPocket.getDiamondCount() == 0 // This one checks whether the pocket has diamonds in it.
						&& thisPocket.getEnabled() == true) 
				{
					labelGameState.setText("Empty pocket; please select a pocket that has diamonds in it."); // Inform the player if this is the case.
				}
			}

		}

		return gameState;

	}

	/**
	 * This method handles the process of distributing diamonds to adjacent pockets.
	 * Note that due to a design flaw, there is a consitant bug that will make it so
	 * that if a player has 4 diamonds in thier first pocket and they are player one, they 
	 * will not be able to double move when they should and a diamond is 'lost'.
	 * @param diamonds The number of diamonds that need redistributing.
	 * @param players An arraylist that holds information on all of the players currently playing.
	 * @param goalPockets An arraylist that holds information on all of the Immutable pockets in play.
	 * @param playerPockets An arraylist that holds information on all of the mutable pockets in play.
	 * @param thisPlayer The player who's turn it is.
	 * @param thisPocket The pocket that was first selected by the player whos turn it is.
	 * @param totalPlayers The total number of players playing.
	 * @return doesPlayerGetAnotherGo Flag for determining whether a player will get another turn or not after this one.
	 */


	private boolean redistributeDiamonds(int diamonds,ArrayList<Player> players
			,ArrayList<ImmutablePocket> goalPockets,ArrayList<MutablePocket> playerPockets,
			Player thisPlayer, MutablePocket thisPocket,int totalPlayers) // Redistribute diamonds to the required pockets.
	{
		int distributeCounter = 0; // Counter used in the redistribution of diamonds for loop, init to 1 in the for loop otherwise a diamond will be put back in the same pocket.
		int startingPoint = thisPocket.getPocketIndex(); // Does not work if player 1 tries to double move by selecting their first pocket
		int diamondsDistributed = 0; // The number of diamonds that have been distributed.
		boolean doesPlayerGetAnotherGo = false; // Determines whether the player gets another go or not.

		for (distributeCounter = 1; diamondsDistributed < diamonds; distributeCounter ++) // Distribute each of the diamonds to each adjacent pocket.
		{
			switch (totalPlayers)
			{
			case 2:
				int totalPoint = startingPoint + distributeCounter; // The totalPoint that diamonds should be put in next.

				if (totalPoint == 4 &&  startingPoint != 0) // If this index would be 4 (Player 1's immutable pocket)
				{ //The if statement just above will cause it so player1 can never get a double turn if they choose their
					//first pocket and it has 4 diamonds in it. This is because starting point will always be zero.
					// This consistency error is the only one that has been identified in the program but is near impossible to fix without large refactoring efforts.

					if (thisPlayer.getPlayerID() == 0 ) // If they are player one..
					{
						goalPockets.get(0).addDiamond(); // Add a diamond to their goal pocket
						thisPlayer.increaseDiamondCount(0); // Increase their count of winning diamonds.
						if (diamondsDistributed + 1 == diamonds) // If this is the last diamond to distribute..
						{
							doesPlayerGetAnotherGo = true; // Then this player gets another turn. (As per the rules of the game)
						}
					}
					else if (thisPlayer.getPlayerID() == 1 )
					{
						// Do nothing.
					}
					startingPoint = 0; // Set the pointers up so it will now point to the other player's first pocket.						
					distributeCounter = 3;

				} 
				else if(totalPoint == playerPockets.size()) // If this index would be 8 (Player 2's immutable pocket)
				{
					if (thisPlayer.getPlayerID() == 1 ) // If they are player two..
					{
						goalPockets.get(1).addDiamond(); // Add a diamond to this pocket...
						thisPlayer.increaseDiamondCount(0); // Increase their count of winning diamonds...
						if (diamondsDistributed + 1 == diamonds) // If this is the last diamond to distribute..
						{
							doesPlayerGetAnotherGo = true; // Then this player gets another turn. (As per the rules of the game)
						}
					}
					else if (thisPlayer.getPlayerID() == 0 )
					{
						// Do nothing.
					}

					startingPoint = 0; // As this is the last pocket of all pockets, if there are still diamonds left to distribute go back to the first one.
					distributeCounter = -1; // set the distribute counter to be the first one (as if its 0 it will increment and skip it)


				}
				else // It must be a player pocket, increase it by one.
				{
					playerPockets.get(startingPoint + distributeCounter).addDiamond();
				}
				break;
			}

			diamondsDistributed ++;
		}

		return doesPlayerGetAnotherGo;

	}

	/**
	 * This method will generate pseudo random numbers that are much more random then they would normally be.
	 * @param uppper The upper limit for generating numbers.
	 * @param lower The lower limit for generating numbers.
	 * @return randomNumber The generated random number to be returned.
	 */

	public double randomNumber(int uppper,int lower) // Used to generate much more random numbers then normal
	{
		double randomNumber = (Math.random() * (uppper - lower + 1))+ lower; 
		return randomNumber;
	}

	/**
	 * This method determines whether all of any players pockets are empty.
	 * @param numOfPlayers The number of players currently playing.
	 * @param playerPockets An arraylist that holds information on all of the mutable pockets in play.
	 * @return pocketsEmpty Flag set to true if all of any player's pockets are emtpy.
	 */

	private boolean isThisPlayersPocketsEmpty(int numOfPlayers,ArrayList<MutablePocket> playerPockets)
	{
		boolean pocketsEmpty = false; // Set or left as default value depending on whether a certain player's player pockets are empty.

		switch (numOfPlayers)
		{
		case 2: // For two players...
			if (playerPockets.get(0).getDiamondCount() == 0 
					&& playerPockets.get(1).getDiamondCount() == 0 
					&& playerPockets.get(2).getDiamondCount() == 0 
					&& playerPockets.get(3).getDiamondCount() == 0) // If all of player 1's pockets are empty...
			{
				pocketsEmpty = true; // Then someone's pockets are empty

			}
			else if (playerPockets.get(4).getDiamondCount() == 0 
					&& playerPockets.get(5).getDiamondCount() == 0 
					&& playerPockets.get(6).getDiamondCount() == 0 
					&& playerPockets.get(7).getDiamondCount() == 0) // If all of player 2's pockets are empty...
			{
				pocketsEmpty = true; // Then someone's pockets are empty			
			}
			break;
		}

		return pocketsEmpty;
	}

	/**
	 * Determine who has won depending on the number of diamonds they have.
	 * @param players An arraylist that holds information on all of the players currently playing.
	 * @param numOfPlayers The number of players currently playing.
	 * @param labelGameState A label that displays the state of the game.
	 */

	private void determineWinner(ArrayList<Player> players,int numOfPlayers,JLabel labelGameState)
	{
		switch (numOfPlayers) // Determine how many players are playing...
		{
		case 2: // For two player.s..
			if (players.get(0).getNumberOfDiamonds() > players.get(1).getNumberOfDiamonds() ) // If player 1 has more diamonds then player 2..
			{
				labelGameState.setText(players.get(0).getPlayerName() + " has won!"); // Then they have won.
			}
			else if (players.get(0).getNumberOfDiamonds() == players.get(1).getNumberOfDiamonds())  // If neither player has more diamonds then the other.
			{
				labelGameState.setText("Its a draw, neither player wins!"); // Then the game is a tie.
			}
			else if (players.get(1).getNumberOfDiamonds() > players.get(0).getNumberOfDiamonds() ) // If player 2 has more then player 1...
			{
				labelGameState.setText(players.get(1).getPlayerName() + " has won!"); // Then player 2 has won instead.
			}
			break;
		}

	}

	/**
	 * When one player has emptied all their pockets, they then take the remaining diamonds that are still in play
	 * and add them to their total. 
	 * @param thisPlayer The player whos turn it was.
	 * @param numOfPlayers The number of players currently playing.
	 * @param goalPockets An arraylist that holds information on all of the Immutable pockets in play.
	 * @param playerPockets An arraylist that holds information on all of the mutable pockets in play.
	 */

	private void takeRemainingDiamonds(Player thisPlayer,int numOfPlayers,ArrayList<MutablePocket> playerPockets,ArrayList<ImmutablePocket> goalPockets)
	{
		switch (numOfPlayers)
		{
		case 2: 
			if (thisPlayer.getPlayerID() == 0) // Then player one has cleared their pockets.
			{
				int diamondsToGive = playerPockets.get(4).removeDiamonds() 
				+ playerPockets.get(5).removeDiamonds() 
				+ playerPockets.get(6).removeDiamonds() 
				+ playerPockets.get(7).removeDiamonds(); // So they get to take all the diamonds from player 2's pockets.

				goalPockets.get(0).addDiamonds(diamondsToGive); // Add diamonds to player 1's goalPocket.
				thisPlayer.increaseDiamondCount(diamondsToGive); // And to the player's own tally.

			}
			else if (thisPlayer.getPlayerID() == 1) // Then player two has cleared their pockets.
			{
				int diamondsToGive = playerPockets.get(0).removeDiamonds() 
				+ playerPockets.get(1).removeDiamonds() 
				+ playerPockets.get(2).removeDiamonds() 
				+ playerPockets.get(3).removeDiamonds(); // So they get to take all the diamonds from player 1's pockets.

				goalPockets.get(0).addDiamonds(diamondsToGive); // Add diamonds to player 2's goalPocket.
				thisPlayer.increaseDiamondCount(diamondsToGive); // And to the player's own tally.

			}
			break;

		}

	}

	/**
	 * Control method for the return to main menu button.
	 * @param game The current instance of the game.
	 */

	public void returnToMainMenu(DigForDiamondsGUI game) 
	{
		DigForDiamondsMainMenu mainMenu = new DigForDiamondsMainMenu(24); // Declare and instance the main menu.

		mainMenu.setSize(800,600); // Set its size...
		mainMenu.setTitle("Dig for Diamonds"); // and title.
		mainMenu.setVisible(true); // Then make it visible.
		mainMenu.setResizable(false); // Make it non resizeable. 
		mainMenu.setLocationRelativeTo(null); // Centre the main menu.

		game.dispose(); // Get rid of this instance of the game as it is not required any more.
	}

}
