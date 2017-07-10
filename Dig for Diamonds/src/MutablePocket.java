
/**
 * This class holds all the information required for mutable pockets to function.
 * @author James Moran.
 * @version 1.0.0 as of 20/03/2012
 */


public class MutablePocket extends Pocket 
{

	/**
	 * Default blank constructor.
	 */
	public MutablePocket()
	{

	}

	/**
	 * Most often used constructor for creating this type of pocket.
	 * @param newOwnerName The player who now owns this pocket.
	 * @param newPocketIndex The index of this pocket as it is stored in the array for this type of pocket.
	 */

	public MutablePocket(String newOwnerName,int newPocketIndex) 
	{
		super(newOwnerName,3,newPocketIndex); // Call on the super class's constructor, Set up this mutable pocket to start with 4 diamonds in it.
	}

	private static final long serialVersionUID = -5735264341287655535L; // Generated serialVersionUID.

	/**
	 * Remove diamonds from this pocket and return the value.
	 * @return temp The amount of diamonds taken from this pocket to be removed
	 */


	public int removeDiamonds()
	{
		int temp = super.getDiamondCount(); // Hold on to the diamonds as a temp var for now..

		super.setDiamondCount(0); // Remove all the diamonds from this pocket.

		super.update(); // Then update the pocket.

		return temp;	
	}

}