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
    public class Bullet : Microsoft.Xna.Framework.DrawableGameComponent // This is for a bullet fired by player weapons that fire bullets.
    {
        private Texture2D texture; // The basic attributes of the bullet sprite.
        private Rectangle spriteRectangle;
        private Vector2 position;
        private String source; // The source of this bullet's creation.
        private float Yspeed; // The x and y speed, which combine to make a velocity 
        private float Xspeed;
        private const int BULLET_WIDTH = 15; // The bullet's height and width, remember that if you change these it will actually recscale the image. (Leave as is for now)
        private const int BULLET_HEIGHT = 15;
        private bool collided = false;
        private BulletState bulletState;
        private const float BULLET_GRAVITY = 0.5f; //Test of gravity for bullets. 
        
        //TODO: May need a state like for the turret in order to not draw the bullet when it is disposed.
        enum BulletState
        {
            inactive,
            active,
            destroyed
        }

        

        public Bullet(Game game, ref Texture2D theTexture,Vector2 initPos,float initYSpeed, float initXSpeed,String newSource)
            : base(game)
        {
            // TODO: Construct any child components here
            texture = theTexture;
            position = new Vector2();
            spriteRectangle = new Rectangle(3, 2, BULLET_WIDTH, BULLET_HEIGHT);
            position = initPos;
            Yspeed = initYSpeed;
            Xspeed = initXSpeed;
            source = newSource;

            bulletState = BulletState.active; // Make the bullet active.

            //Yspeed = 0; For reference this will make it go in a straight line to the right at 10 pixelseconds^-1.
            //Xspeed = 10;
            
        }


        public String getSource() // Get the source of this bullet as a string.
        {
            return source;
        }

        public bool getCollided() // Get whether this bullet has collided with anything.
        {
            return collided; 
        }

        public String getBulletStateString()
        {
            if (bulletState == BulletState.active)
            {
                return "active";
            }
            else if (bulletState == BulletState.destroyed)
            {
                return "destroyed";
            }
            else if (bulletState == BulletState.inactive)
            {
                return "inactive";
            }
            else
            {
                return "BulletStateNotFound";
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
            if (bulletState == BulletState.active) // Only update the bullet if it is active.
            {
                if (collided == true) // If the bullet has destroyed something.
                {
                    Dispose(); // Destroy the bullet too.
                    bulletState = BulletState.destroyed;
                }

                if ((position.Y >= Game.Window.ClientBounds.Height) || (position.X >= Game.Window.ClientBounds.Width)
                    || (position.X <= 0) || (position.Y <= 0))
                {
                    collided = true;
                    Dispose();
                }

                position.Y += Yspeed;
                position.X += Xspeed;

                //position.Y += BULLET_GRAVITY; Was a test of gravity on bullets.
            }
            

            base.Update(gameTime);
        }

        public bool checkCollision(Rectangle rect) // Check collisions between this bullet and other things. (Like turrets)
        {
            if (bulletState == BulletState.active) // Only check for collisions if the bullet is active.
            {
                Rectangle spriterect = new Rectangle((int)position.X, (int)position.Y, BULLET_WIDTH, BULLET_WIDTH); // Make a new rectangle that fits the target...
                // Then see if it intersects something. (This method call returns a boolean value; true or false)
                collided = spriterect.Intersects(rect); // Make sure this line is called in order to prevent double hitting nonsense.               
            }
            return collided;        
        }

        public void destroyBullet() // Set this bullet's state to destroyed.
        {
            bulletState = BulletState.destroyed;
            position.X = 10000; // Move the bullets to a position where they cannot do anything.
            position.Y = 10000;
        }



        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); // Make a new spritebatch

            if (bulletState == BulletState.active) // Only draw the bullet if it is active.
            {
                try
                {
                    sBatch.Draw(texture, position, spriteRectangle, Color.White); // Then draw the bullet with the texture, vector hitbox and back colour.
                }
                catch (InvalidOperationException ioe) // swallow this exception for now. (Don't sue me :( )
                { } 
            }

            base.Draw(gameTime);
        }

    }
}
