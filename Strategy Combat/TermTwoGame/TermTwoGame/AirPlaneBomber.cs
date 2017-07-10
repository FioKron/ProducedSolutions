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


namespace TermTwoGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class AirPlaneBomber : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D texture; // The basic attributes of the bomber sprite.
        private Texture2D bombTexture; // The texture of the bombers bomb's.
        private Rectangle spriteRectangle;
        private Vector2 position; // The bombers position. 
        private Vector2 dropLocation; // Where the bomb is supposed to drop the bombs.
        private float Xspeed; // As the bomber is only going to be moving along the x axis (for now) it only needs an X speed.
        private const int AIRBOMBER_WIDTH = 175; // The bombers's height and width, remember that if you change these it will actually recscale the image; also make them appropirate for the bomber.
        private const int AIRBOMBER_HEIGHT = 63;
        private AirPlaneBomberState airPlaneBomberState;
        private Game game;
        private int animationTimer; // Timer used for animations.

        enum AirPlaneBomberState // The state of this aeroplane bomber.
        {
            inactive,
            active,
            destroyed
        }

        public AirPlaneBomber(Game game, ref Texture2D theTexture,ref Texture2D ammoTexture, Vector2 initPos, float initXSpeed,Vector2 newDropLocation)
            : base(game)
        {
            //Standard constructor for this bomber.
            texture = theTexture;
            position = new Vector2();
            spriteRectangle = new Rectangle(0, 0, AIRBOMBER_WIDTH, AIRBOMBER_HEIGHT); // Adjust as needed.
            position = initPos; // Make this 0,0 to start off the bomber in the top left corner of the screen.
            Xspeed = initXSpeed;
            bombTexture = ammoTexture;
            dropLocation = newDropLocation;

            airPlaneBomberState = AirPlaneBomberState.active;

            this.game = game; // Set up the game var.
        }

        public String getAirBomberStateString() // Get the state of the air plane bomber as a string.
        {
            if (airPlaneBomberState == AirPlaneBomberState.active)
            {
                return "active";
            }
            else if (airPlaneBomberState == AirPlaneBomberState.destroyed)
            {
                return "destroyed";
            }
            else if (airPlaneBomberState == AirPlaneBomberState.inactive)
            {
                return "inactive";
            }
            else
            {
                return "airPlaneBomberStateStateNotFound";
            }

        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        private void animateBomber() // Animate the bomber flying across the sky, jets burning.
        {

            if (animationTimer == 0) // If the timer is 0 (starting off or has been reset)
            {
                spriteRectangle.X = 0; // set the frame back to the first one.
            }
            else if (animationTimer % 10 == 0) // The timer value is a multiple of 10. 
            {
                if (spriteRectangle.X == 176) // Check where the rectangle is..
                {
                    spriteRectangle.X = 0; // invert between 0 and 176
                }
                else
                {
                    spriteRectangle.X = 176; // change to 176.
                }

            }
            else if (animationTimer >= 100) // Reset the timer when it gets to a second.
            {
                animationTimer = 0;
            }

        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            animationTimer++;

            if (airPlaneBomberState == AirPlaneBomberState.active) // Only update the bomber if it is active.
            {
                position.X += Xspeed; // Move across the screen.
                animateBomber(); // Animate this movement.
                
                if (position.X == dropLocation.X) // If over and just past the drop location..
                {
                    dropBomb(); // Drop a bomb...
                }
            }

            try
            {
                foreach (GameComponent gc in Game.Components) // Update each airplane bomb this bomber has dropped.
                {
                    if (gc is AirPlaneBomb)
                    {
                        AirPlaneBomb thisAirBomb = ((AirPlaneBomb)gc);
                        thisAirBomb.Update(gameTime);
                    }
                }
            }
            catch (InvalidOperationException ioe) // swallow this exception for now. (Don't sue me :( )
            { }
            

            base.Update(gameTime);
        }

        private void dropBomb() // Drop a bomb when over the target area.
        {
            //Need to make the airstrike more accurate or predictably inaccurate. (randomised to the same degree each time)
            Game.Components.Add(new AirPlaneBomb(game,ref bombTexture,new Vector2(position.X + (AIRBOMBER_WIDTH /2) - 90,position.Y + AIRBOMBER_HEIGHT),1,0,"PlayerSummonedBomber")); // Add a new bomb to the game components. (this bomber needs a source too)
        }

        public void destroyBomber() // Set this bombers's state to destroyed.
        {
            airPlaneBomberState = AirPlaneBomberState.destroyed;
            position.X = 10000; // Move the bombers to a position where they cannot do anything.
            position.Y = 10000;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); // Make a new spritebatch

            if (airPlaneBomberState == AirPlaneBomberState.active) // Only draw the bullet if it is active.
            {
                sBatch.Draw(texture, position, spriteRectangle, Color.White); // Then draw the bullet with the texture, vector hitbox and back colour.
            }


            foreach (GameComponent gc in Game.Components) // Draw each airplane bomb this bomber has dropped.
            {
                if (gc is AirPlaneBomb)
                {
                    AirPlaneBomb thisAirBomb = ((AirPlaneBomb)gc);
                    thisAirBomb.Draw(gameTime);
                }
            }
            
            base.Draw(gameTime);
        }
    }
}
