
/**
 * This class holds the information required for immutable pockets to function.
 * @author James Moran.
 * @version 1.0.0 as of 20/03/2012
 */

public class ImmutablePocket extends Pocket
{
	/**
	 * The default constructor for the immutable pocket, this calls on the super class's constructor.
	 */
	public ImmutablePocket()
	{
		super();
	}

	/**
	 * Most often used constructor for creating immutable pockets.
	 * @param newOwnerName The player who now owns this pocket.
	 * @param newPocketIndex The index of this pocket as it is stored in the array for this type of pocket.
	 */

	public ImmutablePocket(String newOwnerName,int newPocketIndex) 
	{
		super(newOwnerName,0,newPocketIndex); // Call on the super class's constructor, set up this pocket to have no diamonds in it initially as it is a goal pocket.
	}

	private static final long serialVersionUID = -511297031596607846L; // Generated serialVersionUID.

}
