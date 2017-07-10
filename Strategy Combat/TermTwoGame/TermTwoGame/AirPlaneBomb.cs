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
    public class AirPlaneBomb : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D texture; // The basic attributes of the bomb sprite.
        private Rectangle spriteRectangle;
        private Vector2 position;
        private String source; // The source of this bomb's creation.
        private float Yspeed; // The x and y speed, which combine to make a velocity 
        private float Xspeed;
        private int damage; // The damge this bomb does.
        private const int AIRBOMB_WIDTH = 40; // The bomb's height and width, remember that if you change these it will actually recscale the image. (Leave as is for now)
        private const int AIRBOMB_HEIGHT = 68;
        private bool collided = false;
        private BombState bombState;

        enum BombState
        {
            inactive,
            active,
            destroyed
        }

        public AirPlaneBomb(Game game,ref Texture2D theTexture,Vector2 initPos,float initYSpeed, float initXSpeed,String newSource) // Standard air bomb constructor.
            : base(game)
        {
            texture = theTexture;
            position = new Vector2();
            spriteRectangle = new Rectangle(0, 0, AIRBOMB_WIDTH, AIRBOMB_HEIGHT);
            position = initPos;
            Yspeed = initYSpeed;
            Xspeed = initXSpeed;
            source = newSource;

            damage = 200; // A fairly damaging bomb.

            bombState = BombState.active;
        }

        public String getSource() // Get the source of this bullet as a string.
        {
            return source;
        }

        public bool getCollided() // Get whether this bullet has collided with anything.
        {
            return collided;
        }

        public int getDamage()
        {
            return damage;
        }

        public String getAirBombStateString()
        {
            if (bombState == BombState.active)
            {
                return "active";
            }
            else if (bombState == BombState.destroyed)
            {
                return "destroyed";
            }
            else if (bombState == BombState.inactive)
            {
                return "inactive";
            }
            else
            {
                return "BombStateNotFound";
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (bombState == BombState.active) // Only update the bomb if it is active.
            {
                if (collided == true) // If the bomb has destroyed something.
                {
                    Dispose(); // Destroy the bomb too.
                    bombState = BombState.destroyed;
                }

                if ((position.Y >= Game.Window.ClientBounds.Height) || (position.X >= Game.Window.ClientBounds.Width)
                    || (position.X <= 0) || (position.Y <= 0))
                {
                    collided = true;
                    Dispose();
                }

                position.Y += Yspeed;
                position.X += Xspeed;

            }

            base.Update(gameTime);
        }

        public bool checkCollision(Rectangle rect) // Check collisions between this bullet and other things. (Like turrets)
        {
            if (bombState == BombState.active) // Only check for collisions if the bomb is active.
            {
                Rectangle spriterect = new Rectangle((int)position.X, (int)position.Y, AIRBOMB_WIDTH, AIRBOMB_WIDTH); // Make a new rectangle that fits the target...
                // Then see if it intersects something. (This method call returns a boolean value; true or false)
                collided = spriterect.Intersects(rect); // Make sure this line is called in order to prevent double hitting nonsense.               
            }
            return collided;
        }

        public void destroyBomb() // Set this bomb's state to destroyed.
        {
            bombState = BombState.destroyed;
            position.X = 10000; // Move the bombs to a position where they cannot do anything.
            position.Y = 10000;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); // Make a new spritebatch

            if (bombState == BombState.active) // Only draw the bullet if it is active.
            {
                sBatch.Draw(texture, position, spriteRectangle, Color.White); // Then draw the bullet with the texture, vector hitbox and back colour.
            }

            base.Draw(gameTime);
        }

        public void explode() // This will need to be done later when explosion animations are available.
        {
            // TODO: Explode when the bomb hits something.
        }
    }
}
