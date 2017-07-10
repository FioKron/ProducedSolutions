import java.awt.BorderLayout;
import java.awt.Container;
import java.awt.FlowLayout;
import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

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
 * This is the main menu GUI class for the application and acts as the entry point.
 * @author James Moran.
 * @version 1.0.0 as of 20/03/2012
 */


public class DigForDiamondsMainMenu extends JFrame 
{

	private static final long serialVersionUID = 8828080805761384956L;
	private JButton buttonNewGame; // The three buttons for the main menu
	private JButton buttonAbout;
	private JButton buttonExit;
	private JButton buttonNext;
	private JLabel title; // The title label.
	private JScrollPane scrollPanePlayerNum;
	private JList listPlayerNum; // A list of potential numbers of players for the game.
	private Font font; // The font for the title.	
	private JPanel panelNew; // Panels to hold each of the components.
	private JPanel panelAbout;
	private JPanel panelExit;
	private JPanel panelTitle;
	private JPanel panelPlayerNum;
	private DefaultListModel playerNums;
	private MainMenu control; // The main menu's control class.
	private static DigForDiamondsMainMenu mainMenu; // A field of the class itself, used to make references to it later 

	/**
	 * Default blank constructor.
	 */

	public DigForDiamondsMainMenu() // Blank Constructor.
	{

	}

	public DigForDiamondsMainMenu(int fontSize) // The main menu constructor.
	{
		super();
		try // Must surround this method in a try catch block.
		{
			UIManager.setLookAndFeel(UIManager.getSystemLookAndFeelClassName()); // Set the look and feel of the UI to that of the OS that this is being run on.
		} 
		catch (ClassNotFoundException e)  // In all tests of the application none of these exceptions has ever come up, may require more testing on different operating systems. 
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

		playerNums = new DefaultListModel(); // Instance the list model for the number of players..
		playerNums.addElement(2); // Add 2 to this listmodel. 


		Container main = getContentPane(); // Make a container..
		main.setLayout(new BorderLayout (3,4)); // Set up its layout.

		control = new MainMenu(); // Instance each of the components
		buttonNewGame = new JButton("New Game");
		buttonAbout = new JButton("About");
		buttonExit = new JButton("Exit Game");
		buttonNext = new JButton("Next");
		title = new JLabel("Dig For Diamonds");
		font = new Font("Times New Roman",0, fontSize); // Instance the font as a non-monospaced 24pt font that is not underlined etc.
		title.setFont(font);

		scrollPanePlayerNum = new JScrollPane(); // Instance the scroll pane of player numbers...
		scrollPanePlayerNum.setSize(100,100); // set its size...


		listPlayerNum = new JList(); // Set up the list for choosing from to get the number of players.
		scrollPanePlayerNum.setViewportView(listPlayerNum); // Set the scrollpane's viewport to this list.
		listPlayerNum.setModel(playerNums); // Set the player list to use the playerNums default list model.
		listPlayerNum.setSize(100,100);

		panelNew = new JPanel(); // Instance the panels..
		panelAbout = new JPanel();
		panelExit = new JPanel();
		panelTitle = new JPanel();
		panelPlayerNum = new JPanel();

		panelNew.setLayout(new FlowLayout(FlowLayout.CENTER)); // Set the layout of the panels.
		panelAbout.setLayout(new FlowLayout(FlowLayout.CENTER));
		panelExit.setLayout(new FlowLayout(FlowLayout.CENTER));
		panelTitle.setLayout(new FlowLayout(FlowLayout.CENTER));
		panelPlayerNum.setLayout(new FlowLayout(FlowLayout.CENTER));

		panelNew.add(buttonNewGame); // Add the components to the panels..
		panelAbout.add(buttonAbout);
		panelExit.add(buttonExit);
		panelTitle.add(title);
		panelPlayerNum.add(scrollPanePlayerNum); // add it to the panel.
		panelPlayerNum.add(buttonNext);

		main.add("North",panelTitle);
		main.add("Center",panelAbout);
		main.add("East",panelExit);
		main.add("West",panelNew);
		main.add("South",panelPlayerNum);

		scrollPanePlayerNum.setVisible(false); // Hide the second layer of components for now.
		buttonNext.setVisible(false);

		// Add anonymous action listeners.

		buttonNewGame.addActionListener (new ActionListener() 
		{
			public void actionPerformed (ActionEvent evt) 
			{		   
				control.newGame(buttonNewGame,buttonAbout,buttonExit,title,listPlayerNum,scrollPanePlayerNum,buttonNext);
			}

		});

		buttonNext.addActionListener (new ActionListener() 
		{
			public void actionPerformed (ActionEvent evt) 
			{
				if (listPlayerNum.getSelectedIndex() == -1) // Check whether an item is selected...
				{
					title.setText("Please select a valid player number."); // If not inform the user.
				}
				else // If one is...
				{
					Integer integerPlayerNum = (Integer) listPlayerNum.getSelectedValue(); // Use int's wrapper class to get an object version of this type of the selected value in the list..
					int playerNum = (int) integerPlayerNum; // cast this value to an int.

					control.next(mainMenu,buttonNewGame,buttonAbout,buttonExit,title,listPlayerNum,
							scrollPanePlayerNum,buttonNext,playerNum); // Do the next step required in making a new game.			
				}
			}

		});


		buttonAbout.addActionListener (new ActionListener() 
		{
			public void actionPerformed (ActionEvent evt) 
			{		   
				control.about();
			}

		});

		buttonExit.addActionListener (new ActionListener() 
		{
			public void actionPerformed (ActionEvent evt) 
			{		   
				control.exit();
			}

		});

		//Set this window to exit the application on being closed.

		setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
	}

	/**
	 * The entry point for this application, used to instance the main menu.
	 * @param args the main function's default parameter, not used.
	 */

	public static void main (String args[]) 
	{
		mainMenu = new DigForDiamondsMainMenu(24); // Instance the mainMenu
		mainMenu.setSize(800,600); // Set its size...
		mainMenu.setTitle("Dig for Diamonds"); // and title.
		mainMenu.setVisible(true); // Then make it visible.
		mainMenu.setResizable(false); // Make it non resizeable. 
		mainMenu.setLocationRelativeTo(null); // Centre this window.
	}

}


