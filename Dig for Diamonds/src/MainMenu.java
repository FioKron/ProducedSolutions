import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JOptionPane;
import javax.swing.JScrollPane;

/**
 * The control class for the main menu.
 * @author James Moran.
 * @version 1.0.0 as of 20/03/2012
 */


public class MainMenu extends JFrame // Needs to extend JFrame or showing the message dialog is not possible.
{

	private static final long serialVersionUID = -353401679019737712L; // Generated serialVersionUID.

	/**
	 * Default empty constructor.
	 */

	public MainMenu()
	{

	}

	/**
	 * Do the first stage of what is required for a new game.
	 * @param buttonNewGame The new game button.
	 * @param buttonAbout The about information button.
	 * @param buttonExit The exit button.
	 * @param title The title.
	 * @param listPlayerNum The list control of potential numbers of players.
	 * @param scrollPanePlayerNum The scroll pane for the list above.
	 * @param buttonNext The next button.
	 */

	public void newGame(
			JButton buttonNewGame, 
			JButton buttonAbout, 
			JButton buttonExit, JLabel title, JList listPlayerNum, JScrollPane scrollPanePlayerNum, JButton buttonNext)
	{

		buttonNewGame.setVisible(false); // Hide all the top layer buttons.
		buttonAbout.setVisible(false);
		buttonExit.setVisible(false);

		scrollPanePlayerNum.setVisible(true); // Show all the second layer components.
		buttonNext.setVisible(true);

		title.setText("Please select the number of players that will be playing from the list below"); // Give instructions for what to do next.

	}

	/**
	 * Do what is required next in creating a new game.
	 * @param mainMenu The instance of the main game menu.
	 * @param buttonNewGame The new game button.
	 * @param buttonAbout The about information button.
	 * @param buttonExit The exit button.
	 * @param title The title.
	 * @param listPlayerNum The list control of potential numbers of players.
	 * @param scrollPanePlayerNum The scroll pane for the list above.
	 * @param numPlayers The number of players that will be playing.
	 */

	public void next(DigForDiamondsMainMenu mainMenu,
			JButton buttonNewGame, 
			JButton buttonAbout, 
			JButton buttonExit, JLabel title, JList listPlayerNum, JScrollPane scrollPanePlayerNum, JButton buttonNext,int numPlayers)
	{
		String[] playerNames = new String[4]; // An array used to hold on to the names of players remember that if it is to hold 4 elements 
		//, a four must be here to signify it holds that many but the index will be from 0-3.
		int counter = 0; // The for loop's counter.

		for (counter = 0; counter < numPlayers; counter++) // Still need to make this so if you close or try to cancel the input box it comes up saying invalid input like with the above one.
		{ // Perhaps use a user made exception here.
			int playerNumber = counter; // set the player number init to the counter.

			playerNumber ++; // Then make it what the player number actually is.

			playerNames[counter] = JOptionPane.showInputDialog(MainMenu.this, "Please enter the name of player " + playerNumber ,
					"Name", JOptionPane.PLAIN_MESSAGE); // Enter the names of each player

			while (playerNames[counter].trim().equalsIgnoreCase("")) // Make sure
			{
				playerNames[counter] = JOptionPane.showInputDialog(MainMenu.this, "The name cannot be just spaces; please try again. " + playerNumber ,
						"Name", JOptionPane.PLAIN_MESSAGE); // Get the name again..			
			}

			if (counter > 0) // Check to make sure that this player name is not the same as the last one.
			{
				while (playerNames[counter].trim().equalsIgnoreCase(playerNames[counter - 1].trim()))
				{
					playerNames[counter] = JOptionPane.showInputDialog(MainMenu.this, "Please enter the name of player " + playerNumber ,
							"Name", JOptionPane.PLAIN_MESSAGE); // Enter the names of each player				
				}

			}											
		}

		DigForDiamondsGUI game = new DigForDiamondsGUI(numPlayers,playerNames); // Make a new instance of the main game

		game.setSize(1000,300); // Set it's size...	
		game.setTitle("Dig for Diamonds"); // and title.
		game.setVisible(true); // Show the frame.
		game.setLocationRelativeTo(null); // Centre the screen.
		mainMenu.dispose(); // Dispose of the main menu as its not needed anymore. 

	}

	/**
	 * Control method for the about button.
	 */

	public void about()
	{
		JOptionPane.showMessageDialog(MainMenu.this, "Author: James Moran, Created: Feburary 2012, version 1.0.0 as of 20/03/2012"
				, "About",JOptionPane.INFORMATION_MESSAGE); // Display the about information.
	}

	/**
	 * Control method for the exit button.
	 */

	public void exit()
	{
		System.exit(0); // Exit the game
	}

}
