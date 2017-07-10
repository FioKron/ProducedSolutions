using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace TermTwoGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Button : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Vector2 position; // The button's posistion on the screen as a 2D vector.
        private int buttonWidth;// Variable width and height, may use for all buttons in the future; default button is 200 width, 100 height.
        private int buttonHeight; 
        private Rectangle buttonRectangle; // The rectangle for the button.
        private Texture2D buttonTexture; // The texture for the button.
        private Texture2D buttonTickedTexture; // Only used for tick box buttons.
        private Texture2D buttonUntickedTexture; // Only used for tick box buttons.
        private Game game;
        private bool isButtonEnabled = true; // Flag for whether the button is enabled or not.
        private MouseState currentMouseState; // Current and previous mouse states for determining whether a button has been clicked on, is being held down or has been clicked on then released.
        private MouseState previousMouseState;
        private bool ticked = false;

        
        public Button(Game game,Vector2 newPos,ref Texture2D theTexture, int width, int height) // Standard constructor for the usual kind of buttons.
            : base(game)
        {
            position = new Vector2(); // Set up the button's position.
            position = newPos;

            buttonWidth = width;
            buttonHeight = height;
            buttonTexture = theTexture; // and it's texture.
            buttonRectangle = new Rectangle(0, 0, buttonWidth, buttonHeight); // Set up the rectangle to determine whether the mouse has hit it..
            this.game = game; // and set up the game.

            currentMouseState = Mouse.GetState(); // Set up the initial mouse states.
            previousMouseState = currentMouseState;
        }

        public Button(Game game, Vector2 newPos, ref Texture2D buttonUnticked,ref Texture2D buttonTicked, int width, int height) // Tick box button which also allows setting of width and height.
            : base(game)
        {
            position = new Vector2(); // Set up the button's position.
            position = newPos;

            buttonUntickedTexture = buttonUnticked; // and it's texture.
            buttonTickedTexture = buttonTicked; // also the texture for if it is ticked.
            buttonWidth = width;
            buttonHeight = height;

            buttonTexture = buttonUntickedTexture; // As this is a tick box button, set the texture to unticked by default.
            buttonRectangle = new Rectangle(0, 0, buttonWidth, buttonHeight); // Set up the rectangle to determine whether the mouse has hit it..
            this.game = game; // and set up the game.

            currentMouseState = Mouse.GetState(); // Set up the initial mouse states.
            previousMouseState = currentMouseState;
        }

        public void enableButton() // Enable this button.
        {
            isButtonEnabled = true;
        }

        public void disableButton() // Disable this button.
        {
            isButtonEnabled = false;
        }


        private bool isMouseOver() // Used to determine if the mouse is hovered over a button
        {
            MouseState mouseState = Mouse.GetState(); // Get the mouse's state.
            
            Rectangle buttonCollRect = this.GetBounds(); // Make a collision rectangle that is set to the bounds of the button's rectangle
            Rectangle mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1); // Make a small rectangle to test whether it intersects the button.

            if (isButtonEnabled == true) // Only check if the mouse is over if the button is enabled.
            {
                if (mouseRectangle.Intersects(buttonCollRect)) // If the mouse is within the bounds of the rectangle..
                {
                    return true; // State that this is true.
                }
                else
                {
                    return false; // Otherwise its not.
                }
            }
            else
            {
                return false;
            }           
   
        }

        public bool getTicked() // Get whether the button is ticked or not.
        {
            return ticked;
        }

        public Rectangle GetBounds() // Get the bounds of the button, used for collisions, like with the mouse for example.
        {
            return new Rectangle((int)position.X, (int)position.Y, buttonWidth, buttonHeight);
        }

        public bool isLeftClicked() // check if this button has been left clicked.
        {

            if (isButtonEnabled == true) // Only preform the checks to see if the button is left clicked if the button is enabled.
            {
                if (this.isMouseOver()) // First check if the button has a mouse over it
                {
                    if (previousMouseState.LeftButton == ButtonState.Pressed 
                        && currentMouseState.LeftButton == ButtonState.Released)  // then check if the left mouse button has been pressed. (Must now be clicked on then released for this to work).
                    {
                        if (buttonTickedTexture != null) // If this is a tick box button...
                        {
                            if (ticked == false) // If the button is unticked..
                            {
                                ticked = true; // It is now ticked.
                                buttonTexture = buttonTickedTexture; // Show that this is the case.
                            }
                            else
                            {
                                ticked = false; // The button is now unticked.
                                buttonTexture = buttonUntickedTexture; // Show that this is the case.
                            }

                        }

                        return true; // If it has return true.
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }      
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Game.Content.Load<Texture2D>("Pointing Arrow");

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); // Make a new spritebatch

            sBatch.Draw(buttonTexture, position, buttonRectangle, Color.Blue); // Then draw the ship with the texture, init pos and its hitbox and a color for its background, Remember that the "white" space can be an colour as per the 3rd param in the draw method above.

            base.Draw(gameTime);
        }
    }
}
