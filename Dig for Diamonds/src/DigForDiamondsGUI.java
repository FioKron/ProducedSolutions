import java.awt.BorderLayout;
import java.awt.Container;
import java.awt.FlowLayout;
import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.util.ArrayList;

import javax.swing.DefaultListModel;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JList;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.UIManager;
import javax.swing.UnsupportedLookAndFeelException;

/**
 * This is the GUI class that implements how the game will look and feel,
 * although there are some methods in here that do some mundane tasks such as getting a string version of the game state.
 * @author James Moran.
 * @version 1.0.0 as of 20/03/2012
 */

public class DigForDiamondsGUI extends JFrame implements MouseListener // As this class is a frame and needs to listen for mouse events.

{
	private static final long serialVersionUID = -3424325970708891711L; // Generated serialVersionUID. 
	private JButton buttonForfeit; // The forfeit button.
	private JButton buttonStart; // The start game button.
	private JButton buttonReturnMainMenu; // Return to the main menu.
	private ArrayList<ImmutablePocket> goalPockets; // An array of goal pockets for each player.
	private ArrayList<MutablePocket> playerPockets;
	private DigForDiamonds control; // The control class for the game.
	private ArrayList<Player> players; // an array of players.
	private JPanel panelPlayer1; // Panels to hold each of the player's pockets.
	private JPanel panelPlayer2;
	private JPanel panelPlayer2GoalPock; // This panel holds the goal pocket of player 2.
	private boolean needToReset = false; // This flag determines whether the game needs to reset and if so it will do so.
	private JPanel panelState; // This panel holds a few buttons and displays the state of the game.
	private JPanel panelPlayerList; // This panel holds the player list.
	private JLabel labelPlayerNumbers; // This label displays the number of players currently playing
	private JLabel labelVersionNum; // This label displays the version number.
	private JScrollPane scrollPlayerList;
	private JList listPlayers; // This list holds information on the current players in the game and their scores. 
	private Font stateFont; // The font used for the label that displays the gamestate.
	private JLabel labelGameState; // The label that actually display the gamestate
	private Container main; // The container that holds all the components.
	private int totalPlayers; // The number of players in the game.
	private GameState gameState = GameState.waitingForStart ; // This variable is of the enum, gamestate, and holds the state of the game.
	private String gameStateString; // The string version of the game state.
	private DefaultListModel playerStatus; // This default list model holds that status of each player.
	

	private enum GameState // This enum holds the state of the game.
	{
		waitingForStart, // Before the start button is pressed.
		gameWelcome, // During phases of the starting sequence.
		decideWhoGoesFirst,
		twoPlayersPlayerOneGoesFirst,
		twoPlayersPlayerTwoGoesFirst,
		twoPlayersNowPlayerOnesTurn, // After this, it is either one player's turn or the other player's turn.
		twoPlayersNowPlayerTwosTurn,
		aPlayerHasWon // The state of one playing having won the game.
	}

	/**
	 * Convert the string version of the game state to the GameState version.
	 * This is required for other classes.
	 */

	private void setGameState() // Convert the string version of the game state to the GameState version.
	{
		if (gameStateString.equalsIgnoreCase("waitingForStart"))
		{
			gameState = GameState.waitingForStart;
		}
		else if (gameStateString.equalsIgnoreCase("gameWelcome"))
		{
			gameState = GameState.gameWelcome;
		} 
		else if (gameStateString.equalsIgnoreCase("decideWhoGoesFirst"))
		{
			gameState = GameState.decideWhoGoesFirst;
		} 
		else if (gameStateString.equalsIgnoreCase("twoPlayersPlayerOneGoesFirst"))
		{
			gameState = GameState.twoPlayersPlayerOneGoesFirst;
		} 
		else if (gameStateString.equalsIgnoreCase("twoPlayersPlayerTwoGoesFirst"))
		{
			gameState = GameState.twoPlayersPlayerTwoGoesFirst;
		} 
		else if (gameStateString.equalsIgnoreCase("twoPlayersNowPlayerOnesTurn"))
		{
			gameState = GameState.twoPlayersNowPlayerOnesTurn;
		} 
		else if (gameStateString.equalsIgnoreCase("twoPlayersNowPlayerTwosTurn"))
		{
			gameState = GameState.twoPlayersNowPlayerTwosTurn;
		} 
		else if (gameStateString.equalsIgnoreCase("aPlayerHasWon"))
		{
			gameState = GameState.aPlayerHasWon;
		} 

	}

	/**
	 * Change the gamestate to a string value and return this value.
	 * @return getGameStateString The string version of the gamestate.
	 */

	private String gameStateToString() // Convert the game state into a string type.
	{
		String getGameStateString = "";

		switch (gameState) // Check the state of the game and change the messages as needed.
		{
		case waitingForStart:
			getGameStateString = "waitingForStart";
			break;
		case gameWelcome:
			getGameStateString = "gameWelcome";
			break;
		case decideWhoGoesFirst:
			getGameStateString = "decideWhoGoesFirst";
			break;
		case twoPlayersPlayerOneGoesFirst:
			getGameStateString = "twoPlayersPlayerOneGoesFirst";
			break;
		case twoPlayersPlayerTwoGoesFirst:
			getGameStateString = "twoPlayersPlayerTwoGoesFirst";
			break;
		case twoPlayersNowPlayerOnesTurn: 
			getGameStateString = "twoPlayersNowPlayerOnesTurn";
			break;
		case twoPlayersNowPlayerTwosTurn:
			getGameStateString = "twoPlayersNowPlayerTwosTurn";
			break;
		case aPlayerHasWon:
			getGameStateString = "aPlayerHasWon";
			break;	
		}

		return getGameStateString;

	}

	/**
	 * Default blank constructor.
	 */

	public DigForDiamondsGUI()
	{

	}

	/**
	 * Initalise the GUI and all the required objects.
	 * @param numberOfPlayers The number of players that will be playing.
	 * @param playerNames An array of the names of each player that will be playing, used to init the player objects.
	 */

	public DigForDiamondsGUI(final int numberOfPlayers,String [] playerNames) // Number of players has to be final or it complains, the number of players once set would not change anyhow.
	{	
		super(); 

		try // Must surround this method in a try catch block.
		{
			UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName()); // Set the look and feel of the UI to that of the OS that this is being run on.
		} 
		catch (ClassNotFoundException e) // In all tests of the application none of these exceptions has ever come up, may require more testing on different operating systems. 
		{
			e.printStackTrace();
		} 
		catch (InstantiationException e) 
		{
			e.printStackTrace();
		} 
		catch (IllegalAccessException e) 
		{
			e.printStackTrace();
		} 
		catch (UnsupportedLookAndFeelException e)
		{
			e.printStackTrace();
		}


		players = new ArrayList<Player>();  // Instance the array of players..
		goalPockets = new ArrayList<ImmutablePocket>(); // then ImmutablePockets...
		playerPockets = new ArrayList<MutablePocket>(); // then the MutablePockets...
		control = new DigForDiamonds(); // as well as the control...
		buttonForfeit = new JButton("Forfeit"); // and finally the button for giving up.
		buttonForfeit.setVisible(false); // Make it so the forfeit button is not visible right away. (As players can not give up if they are not yet playing)
		buttonStart = new JButton("Start the game"); // Instance the start button too.
		buttonReturnMainMenu = new JButton("Return to main menu");
		labelPlayerNumbers = new JLabel("Players: " + numberOfPlayers);
		labelVersionNum = new JLabel ("Version Number: 1.0.0"); // Update the version number as needed.
		listPlayers = new JList(); // Instance the list of players.
		listPlayers.setSize(100,100); // Set the size of the player list.

		main = getContentPane(); // Make a container..
		main.setLayout(new BorderLayout (3,4)); // Set up its layout.
		stateFont = new Font("Times New Roman",0, 24); // Init the font.
		labelGameState = new JLabel("Press Start to begin."); // Init the label.
		labelGameState.setFont(stateFont); // Set the label's font to the value of statefont.

		labelPlayerNumbers.setFont(stateFont);  // As well as for these labels.
		labelVersionNum.setFont(stateFont);

		totalPlayers = numberOfPlayers; // Set up the total players value to that of the number of players. 

		int mainCounter = 0; // Counter used to set everything up, does the large bit of the counting.
		int mutPockCounter = 0; // This counter is used in the for loop that sets up the mutable pockets for each player.
		int playerPocketCounter = 0; // This counter is used to actually set up each pocket's text.

		// Init the pockets.
		for (mainCounter = 0; mainCounter < numberOfPlayers; mainCounter ++) // Start setting up the game for each player
		{
			players.add(new Player(playerNames[mainCounter],mainCounter)); // Make a new player with a name and an ID.
			goalPockets.add(new ImmutablePocket(playerNames[mainCounter],mainCounter)); // Each player then gets one goal pocket...
			goalPockets.get(mainCounter).update(); // Set up the goal pockets with the initial amount of diamonds.

			for (mutPockCounter = 0; mutPockCounter < 4; mutPockCounter ++) // And four other pockets that they can pick from to move diamonds out of.
			{ 
				playerPockets.add(new MutablePocket(playerNames[mainCounter],playerPocketCounter));
				playerPockets.get(playerPocketCounter).update(); // Set up the pocket to display the initial amount of diamonds.

				playerPocketCounter ++; // Increment the counter for player pockets so that each one gets the correct text.
			} 

		} // Repeat for each player, creating the player then pockets then allocating them to each player.

		panelPlayerList = new JPanel(); // Instance the panel to hold the scroll pane...
		this.setUpPockets(numberOfPlayers);

		playerStatus = new DefaultListModel(); // Instance the default list model for the players...

		updatePlayerList(); // Add each player's nessecary information to the list..


		listPlayers.setModel(playerStatus); // Set the JList of players to this model.

		scrollPlayerList = new JScrollPane(); // Instance the scroll pane to hold this list.
		scrollPlayerList.setSize(100,100);
		scrollPlayerList.add(listPlayers); // Add the list to it..
		scrollPlayerList.setViewportView(listPlayers); // Set the scrollPane to the viewpane of the list of players.

		panelState = new JPanel(); // Instance the state panel.
		panelState.add(buttonForfeit); // Add the required components.
		panelState.add(buttonStart);
		panelState.add(buttonReturnMainMenu);
		panelState.add(labelGameState);
		panelState.add(labelPlayerNumbers);
		panelState.add(labelVersionNum);

		panelPlayerList.add(scrollPlayerList); // Add the scroll pane to it.

		// Set up action listeners

		buttonForfeit.addActionListener (new ActionListener() 
		{
			public void actionPerformed (ActionEvent evt) 
			{		   
				gameStateString = control.forfeit(numberOfPlayers, players, labelGameState,gameStateToString());
				gameStateString = "waitingForStart"; // enable the game to be played again.
				disableControls(); // Disable the controls as the game has now ended.
				setGameState();
				updatePlayerList();
				buttonForfeit.setVisible(false);
				buttonStart.setVisible(true);
				buttonStart.setText("Play again");
				needToReset = true;
			}

		});

		buttonStart.addActionListener (new ActionListener() 
		{
			public void actionPerformed (ActionEvent evt) 
			{
				//	go through the starting sequence.

				start();

				if (gameState == GameState.twoPlayersNowPlayerOnesTurn || gameState == GameState.twoPlayersNowPlayerTwosTurn )
				{
					buttonStart.setVisible(false); // Make the start button invisible as by now the game has already begun.
					buttonForfeit.setVisible(true); // And make the forfeit button visible.				
				}

			}

		});

		buttonReturnMainMenu.addActionListener (new ActionListener() 
		{
			public void actionPerformed (ActionEvent evt) 
			{
				control.returnToMainMenu(DigForDiamondsGUI.this);
			}

		});



		// Add the panels to the content pane.
		main.add("Center",panelState);
		main.add("East",panelPlayerList);

		// Add a mouse listener to each mutable pocket to listen for mouse actions.

		for (MutablePocket thisPocket : playerPockets)
		{
			thisPocket.addMouseListener(this); 
		}

		//Set it to exit the program on closing this window.

		setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
		this.setLocationRelativeTo(null); // Centre the window.
		this.setResizable(false); // Make sure it cannot be resized.
	}

	/**
	 * Update the list of players.
	 */

	private void updatePlayerList() 
	{
		playerStatus.clear();

		for(Player thisPlayer: players) // Add the status of each player to the default list model.
		{
			playerStatus.addElement(thisPlayer.toString());
		}

	}

	/**
	 * Do what is required to reset the state of the game so another game can be played.
	 */

	private void reset() // reset the game so another game can be played.
	{
		enableControls(); // enable the controls again..

		for (MutablePocket thisMutPocket: playerPockets) // Reset the pockets to their initial values.
		{
			thisMutPocket.setDiamondCount(3);
		}

		for(ImmutablePocket thisImmPocket: goalPockets)
		{
			thisImmPocket.setDiamondCount(0);
		}

		for(Player thisPlayer: players) // Same for the player..
		{
			thisPlayer.resetPlayer();
		}

		updatePlayerList(); // Update the player list.

	}

	/**
	 * Do what is required to start the game.
	 */

	private void start() // Start the game
	{
		double startingPlayerDecider = 0; // Used in the process of deciding who goes first.	

		if (needToReset == true) // If the game needs to be reset...
		{
			reset(); // Then do so..
		}

		switch (gameState) // Check the state of the game and change the messages as needed.
		{
		case waitingForStart:
			labelGameState.setText("Welcome to dig for diamonds.");	
			buttonStart.setText("Next"); // The start button now acts as the next button.
			gameState = GameState.gameWelcome;
			gameStateString = gameStateToString();
			break;
		case gameWelcome:
			labelGameState.setText("We shall now randomly decide who goes first.");	
			gameState = GameState.decideWhoGoesFirst;
			gameStateString = gameStateToString();
			break;
		case decideWhoGoesFirst:
			switch(totalPlayers) // Decide based on the number of players who goes first.
			{
			case 2: // For two players...
				startingPlayerDecider = control.randomNumber(100,0); // Generate a random number between 0 and 100

				if (startingPlayerDecider >= 50) // If it is greater then or equal to 50...
				{
					players.get(0).setIsPlayerTurn(true); // Player 1 goes first.	
					labelGameState.setText("Player 1 shall go first this time.");	
					gameState = GameState.twoPlayersPlayerOneGoesFirst;
					gameStateString = gameStateToString();
				}
				else
				{
					players.get(1).setIsPlayerTurn(true); // Else player two goes first.
					labelGameState.setText("Player 2 shall go first this time.");	
					gameState = GameState.twoPlayersPlayerTwoGoesFirst;
					gameStateString = gameStateToString();
				}
				break;
			}
			break;
		case twoPlayersPlayerOneGoesFirst:
			gameState = GameState.twoPlayersNowPlayerOnesTurn;
			labelGameState.setText("It is now " + players.get(0).getPlayerName() + "'s turn.");	
			gameStateString = gameStateToString();
			break;
		case twoPlayersPlayerTwoGoesFirst:
			gameState = GameState.twoPlayersNowPlayerTwosTurn;
			labelGameState.setText("It is now " + players.get(1).getPlayerName() + "'s turn.");	
			gameStateString = gameStateToString();
			break;
		}		
	}

	/**
	 * Disable the controls of the game.
	 */

	public void disableControls() // Disable the game's controls when needed.
	{
		for (MutablePocket thisPocket: playerPockets) // Disable each control in turn.
		{
			thisPocket.disablePocket();
		}

		for(ImmutablePocket thisPocket: goalPockets)
		{
			thisPocket.disablePocket();
		}		
	}

	/**
	 * Enable the controls when needed.
	 */

	public void enableControls() 
	{
		for (MutablePocket thisPocket: playerPockets) // Enable each control in turn
		{
			thisPocket.enablePocket();
		}

		for(ImmutablePocket thisPocket: goalPockets)
		{
			thisPocket.enablePocket();
		}

	}

	/**
	 * Set up the pockets depending on the number of players.
	 * @param numberOfPlayers The number of players currently playing.
	 */

	public void setUpPockets(int numberOfPlayers)
	{

		switch(numberOfPlayers) // Now it is time to setup the pockets depending on the number of players
		{
		case 2: // For two players...
			panelPlayer1 = new JPanel(); // Instance the panels
			panelPlayer2 = new JPanel();
			panelPlayer2GoalPock = new JPanel();

			panelPlayer1.setLayout(new FlowLayout(FlowLayout.CENTER));
			panelPlayer2.setLayout(new FlowLayout(FlowLayout.CENTER));


			panelPlayer1.add(playerPockets.get(0)); // Add in player 1's pockets...
			panelPlayer1.add(playerPockets.get(1));
			panelPlayer1.add(playerPockets.get(2));
			panelPlayer1.add(playerPockets.get(3));
			panelPlayerList.add(goalPockets.get(0)); // Add player 1's goal pocket to the right side of the screen...

			panelPlayer2.add(playerPockets.get(7)); // Add in player two's pockets...
			panelPlayer2.add(playerPockets.get(6));
			panelPlayer2.add(playerPockets.get(5));
			panelPlayer2.add(playerPockets.get(4));
			panelPlayer2GoalPock.add(goalPockets.get(1));


			main.add("North", panelPlayer1); // Add these panels to the container.
			main.add("South", panelPlayer2);
			main.add("West",panelPlayer2GoalPock);
			break;

		}

	}

	/**
	 * These methods are required as a result of implementing the mouse listener interface, they are not needed.
	 */

	public void mouseClicked(MouseEvent event){}
	public void mouseEntered(MouseEvent arg0){}	
	public void mouseExited(MouseEvent arg0){}
	public void mouseReleased(MouseEvent arg0){}
	/**
	 * Check who's turn it is and then make sure it is that player's turn.
	 * This was put in as a work around for when the game was reset, it would set
	 * the player that it chose to have the first turn as true yet later 'forgot' 
	 * that it had done this. This function is here to facilitate this purpose to
	 * ensure it is always someone's go until the game is over.
	 */

	public void checkSetPlayerTurn() // Check and if needed set the players turns so it is their turn.
	{
		switch (totalPlayers) // Determine how many players there are...
		{
		case 2: // For two players...
			switch (gameState)
			{
			case twoPlayersNowPlayerOnesTurn: // Make sure it's player one's turn.
				players.get(0).setIsPlayerTurn(true);
				break;				
			case twoPlayersNowPlayerTwosTurn: // Make sure it's player two's turn.
				players.get(1).setIsPlayerTurn(true);
				break;							
			}
		}

	}
	
	/**
	 * Mouse event for when the mouse is pressed.
	 * @param pocketPressed The pocket that has been clicked on
	 */

	public void mousePressed(MouseEvent pocketPressed) 
	{
		checkSetPlayerTurn(); // Perform an extra check on whos turn it is.
		gameStateString = control.attemptTurn(players,goalPockets,playerPockets,totalPlayers,pocketPressed, labelGameState,gameStateString);

		setGameState();
		updatePlayerList();

		if (gameStateString == "aPlayerHasWon") // If Someone has won...
		{
			gameStateString = "waitingForStart"; // enable the game to be played again.
			disableControls(); // Disable the controls as the game has now ended.
			setGameState();
			updatePlayerList();
			buttonForfeit.setVisible(false);
			buttonStart.setVisible(true);
			buttonStart.setText("Play again");
			needToReset = true;
		}
	}
}