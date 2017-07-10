import javax.swing.JLabel;


/**
 * This class is the parent class for mutable pockets and immutable pockets, holding the required functionality of both.
 * @author James Moran.
 * @version 1.0.0 as of 20/03/2012
 */

public class Pocket extends JLabel // As all pockets are labels.
{

	private static final long serialVersionUID = 3785118591881026861L; // Generated serialVersionUID.
	private int diamondCount; // The number of diamonds in this pocket.
	private String ownerName; // The name of the owner of this pocket.
	private int pocketIndex; // The index of this pocket.
	private boolean isEnabled = true; // by default the pockets are enabled till they are disabled.
	
	/**
	 * Empty default constructor.
	 */
	public Pocket()
	{
		
	}
	
	/**
	 * Get whether this pocket is enabled or not.
	 * @return isEnabled Flag for whether or not the pocket is enabled.
	 */
	
	public boolean getEnabled()
	{
		return isEnabled;
	}
	
	/**
	 * Enables the pocket to be used.
	 */
	
	public void enablePocket() // Enable this pocket for use.
	{
		isEnabled = true;		
	}
	
	/**
	 * Disable the pocket.
	 */
	
	public void disablePocket() // Disable use of this pocket.
	{
		isEnabled = false;
	}
	
	/**
	 * Most often used constructor for creating pockets, this is a super constructor.
	 * @param newOwnerName The new owner for this pocket.
	 * @param newDiamondCount The diamonds that this pocket starts with.
	 * @param newPocketIndex The index of this pocket.
	 */
	
	public Pocket(String newOwnerName, int newDiamondCount,int newPocketIndex)
	{
		super(); // For safety, make a call to this class's super constructor.
		ownerName = newOwnerName; // Then set up the rest of the class.
		diamondCount = newDiamondCount;
		pocketIndex = newPocketIndex;
	}
	
	/**
	 * Update the text information of this pocket(How it is seen by the user).
	 */
	
	public void update()
	{
		this.setText("Diamonds: " + diamondCount);
	}
	
	/**
	 * Add one diamond to this pocket.
	 */
	
	public void addDiamond() // Add one diamond to this pocket.
	{
		diamondCount ++; // Add the diamond...
		this.update(); // then update the pocket.
	}
	
	/**
	 * Get the diamonds held in this pocket.
	 * @return diamondCount The diamonds that this pocket holds.
	 */
	
	public int getDiamondCount()
	{
		return diamondCount;	
	}
	
	/**
	 * Get the index of this pocket.
	 * @return pocketIndex The index of this pocket.
	 */
	
	public int getPocketIndex()
	{
		return pocketIndex;
	}
	
	/**
	 * Set the amount of diamonds held in this pocket.
	 * @param newDiamondCount
	 */
	
	public void setDiamondCount(int newDiamondCount)
	{
		diamondCount = newDiamondCount;
		this.update();
	}
	
	/**
	 * Add diamonds to this pocket.
	 * @param amountToAdd The diamonds to add to his pocket.
	 */
	
	public void addDiamonds(int amountToAdd) // Add diamonds to this pocket, often done when remaining diamonds are taken.
	{
		diamondCount += amountToAdd; // Update the count of diamonds.
		this.update(); // then update the pocket.
		
	}
	
	/**
	 * Get the owner of this pocket.
	 * @return ownerName The owner of this pocket.
	 */
	
	public String getOwnerName()
	{
		return ownerName;
	}
	
}
