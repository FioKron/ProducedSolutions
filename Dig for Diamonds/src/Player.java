
/**
 * This class holds all the information that is needed about the player as well as some other required fucntions.
 * @author James Moran.
 * @version 1.0.0 as of 20/03/2012
 */

public class Player 
{
	private String playerName; // The player's name
	private int numberOfDiamonds; // The number of diamonds the player has. (In their goal pocket)
	private int playerID;
	private boolean isPlayerTurn = false; // Whether it is this player's turn or not, starts as false.

	/**
	 * Default blank constructor
	 */

	public Player()
	{

	}

	/**
	 * Reset the player to what they would be before the game starts.
	 */

	public void resetPlayer()
	{
		numberOfDiamonds = 0;
		isPlayerTurn = false;
	}

	/**
	 * Override of the toString method.
	 * @return A string version of important data that the player holds.
	 */

	public String toString() // Get the required information from the player as a string.
	{
		return  playerName + " : " + "Diamonds: " + numberOfDiamonds;
	}

	/**
	 * The number of diamonds the player holds.
	 * @return numberOfDiamonds The number of diamonds that the player has (In thier goal pocket).
	 */

	public int getNumberOfDiamonds()
	{
		return numberOfDiamonds;
	}

	/**
	 * Most often used constructor for the player.
	 * @param newPlayerName The new name for the player
	 * @param newPlayerID The ID number of this player.
	 */

	public Player(String newPlayerName, int newPlayerID)
	{
		playerName = newPlayerName;
		playerID = newPlayerID;
	}

	/**
	 * Increase the amount of diamonds the player has.
	 * @param amount The amount to increase by, 0 to increase by one, otherwise increase the number of diamonds by the value of this param.
	 */

	public void increaseDiamondCount(int amount) // Increase the count of diamonds, 0 (Or less) to increase by just one.
	{
		if (amount > 0)
		{
			numberOfDiamonds += amount;
		}
		else
		{
			numberOfDiamonds ++;
		}

	}

	/**
	 * Get the player name and return its value.
	 * @return playerName The name of this player.
	 */


	public String getPlayerName()
	{
		return playerName;
	}

	/**
	 * Set the player's turn state.
	 * @param turnState what isPlayerTurn should be set to.
	 */

	public void setIsPlayerTurn(boolean turnState)
	{
		isPlayerTurn = turnState;
	}

	/**
	 * Get the player's turn state.
	 * @return isPlayerTurn A flag for determining whether it is the player's turn or not.
	 */

	public boolean getTurnState()
	{
		return isPlayerTurn;
	}

	/**
	 * Get the player's ID number.
	 * @return playerID The player's ID number.
	 */

	public int getPlayerID()
	{
		return playerID;
	}




}
