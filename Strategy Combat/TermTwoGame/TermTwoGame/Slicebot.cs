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
    /// Note that currently unlike the plasma turret the slicebot has more then one type of damage resistance.
    /// </summary>
    public class Slicebot : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D slicebotTexture; // The texture of the slicebot.
        private Rectangle spriteRectangle; // The rectangle for determining collosions of the slicebot with other things.
        private Vector2 position; // The position of this slicebot.
        private float health; // The slicebot's health.
        private const int SLICEBOT_WIDTH = 51; // The width and height of the slicebot.
        private const int SLICEBOT_HEIGHT = 47;
        private int animationTimer; 
        private SpriteFont statusFont; // This font hovers over the slicebot and displays it's status.
        private int damage; // The damage the slicebot does per slice.
        private bool slicebotActive = false; // Whether or not the slicebot is active.
        private float bulletDamageMul; // The damage resistance of the slicebot to bullets.
        private float airBombDamageMul; // The damage multiplyer or the slicebot to bombs.
        private SlicebotState slicebotState; // Enum to represent the state of the slicebot.
        private Unit[] squad; // The squad of units the slicebot wants to slice up.
        private Game game;
        private AudioComponent slicebotAudioComp; // So the slicebot can play sounds too.
        private int whirTimer;
        private bool isMissionTwoSlicebot; // Whether this slicebot was spawned as a result of mission two or not.


        enum SlicebotState
        {
            active,
            mildDamage, // The slicebot is still active but has taken mild damage.
            moderateDamage, // The slicebot is still active but has taken moderate damage.
            heavyDamage, // The slicebot is still active but has taken heavy damage.
            criticalDamage, // The slicebot is still active but has taken critical damage to all systems.
            destroyed
        }

        

        public Slicebot(Game game,Vector2 newPos, ref Texture2D theTexture,Unit[] theSquad, bool newIsMissionTwoSlicebot)
            : base(game)
        {
            slicebotAudioComp = new AudioComponent(Game);
            Game.Components.Add(slicebotAudioComp);

            isMissionTwoSlicebot = newIsMissionTwoSlicebot;

            slicebotTexture = theTexture;
            position = new Vector2(); // Set up the position
            position = newPos;
            health = 500;
            damage = 1; // Does damage each time it touches an enemy unit or otherwise so keep it somewhat low.
            bulletDamageMul = 0.01f; // High damage resistance to bullets.
            airBombDamageMul = 3.0f; // Takes extra damage from air bombs!
            slicebotActive = true;

            spriteRectangle = new Rectangle(0, 0, SLICEBOT_WIDTH, SLICEBOT_HEIGHT);

            squad = theSquad;
            this.game = game;

            statusFont = Game.Content.Load<SpriteFont>("SpriteFont1");
           
        }

        public bool getIsMissTwoSlicebot()
        {
            return isMissionTwoSlicebot;
        }

        private void dragTowards(Unit[] squad) // Drag units towards the slicebot.
        {
            foreach (Unit thisUnit in squad)
            {
                if (thisUnit.getPos().X > position.X) // The unit must be to the right of this slicebot.
                {
                    thisUnit.forceMoveLeft(2);
                }
                else
                {
                    thisUnit.forceMoveRight(2); // As the units start of the left side of the screen, force them to move to the right by more then they can move to the left.
                }
               
            }
        }

        public float getHealth()
        {
            return health;
        }

        public void beDamaged(float incomingDamage, String type) // The turret gets damaged.
        {
            float totalDamage = 0;

            if (type == "Bullet") // Calcualte bullet damage..
            {
                totalDamage = incomingDamage * bulletDamageMul;
            }
            else if (type == "AirBomb") // Calculate bomb damage...
            {
                totalDamage = incomingDamage * airBombDamageMul;
            }

            health -= totalDamage;

            if (health <= 0)
            {
                health = 0;
                this.Dispose(); // Get rid of the turret for now.
                slicebotState = SlicebotState.destroyed;
                slicebotActive = false;
            }
            else if (health < 450 && health > 350) // Take care with this logic, the following statements determine how damaged the turret is and show this.
            {
                slicebotState = SlicebotState.mildDamage;
            }
            else if (health < 349 && health > 250)
            {
                slicebotState = SlicebotState.moderateDamage;
            }
            else if (health < 249 && health > 150)
            {
                slicebotState = SlicebotState.heavyDamage;
            }
            else if (health < 149)
            {
                slicebotState = SlicebotState.criticalDamage;
            }

        }

        public bool getSlicebotActive()
        {
            return slicebotActive;
        }

        public int getDamage()
        {
            return damage;
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
            animationTimer++; // Update  the animation timer value.
            whirTimer++;

            if (slicebotState != SlicebotState.destroyed) // If the slicebot is active...
            {
                animateSlicebotSlice();
                dragTowards(squad); // Drag the squad towards itself.
                playSlicebotWhir(); // Make the sound too.
            }

            base.Update(gameTime);
        }

        private void playSlicebotWhir() // Play the whiring sound at 0.3 second intervals. May get annoying quite quickly.
        {
            if (whirTimer < 2 && whirTimer > 0)
            {
                slicebotAudioComp.playCue("Slicebot saw");
            }
            else if (whirTimer > 30) 
            {
                whirTimer = 0;
            }
            
        }


        public override void Draw(GameTime gameTime)
        {

            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); // Make a new spritebatch

            Vector2 statusTextVector = new Vector2(position.X, position.Y - 50);

            int healthRound = (int)health; // cast health to an int, which will round it.

            if (slicebotState == SlicebotState.active) // Draw the non damaged slicebot. Does not need to load texture as already loaded.
            {
                sBatch.Begin();
                sBatch.Draw(slicebotTexture, position, spriteRectangle, Color.White); // Then draw the slicebot with the texture, init pos and its hitbox and a color for its background.
                sBatch.DrawString(statusFont, "" + healthRound, statusTextVector, Color.Red); // Draw the status above the slicebot.
                sBatch.End();

                base.Draw(gameTime);
            }
            else if (slicebotState == SlicebotState.mildDamage)
            {
                slicebotTexture = Game.Content.Load<Texture2D>("Slice bot Mild damage");

                sBatch.Begin();
                sBatch.Draw(slicebotTexture, position, spriteRectangle, Color.White); // Then draw the slicebot with the texture, init pos and its hitbox and a color for its background.
                sBatch.DrawString(statusFont, "" + healthRound, statusTextVector, Color.Red); // Draw the status above the slicebot.
                sBatch.End();


                base.Draw(gameTime);

            }
            else if (slicebotState == SlicebotState.moderateDamage)
            {
                slicebotTexture = Game.Content.Load<Texture2D>("Slice bot Moderate damage");

                sBatch.Begin();
                sBatch.Draw(slicebotTexture, position, spriteRectangle, Color.White); // Then draw the slicebot with the texture, init pos and its hitbox and a color for its background.
                sBatch.DrawString(statusFont, "" + healthRound, statusTextVector, Color.Red); // Draw the status above the slicebot.
                sBatch.End();

                base.Draw(gameTime);

            }
            else if (slicebotState == SlicebotState.heavyDamage)
            {
                slicebotTexture = Game.Content.Load<Texture2D>("Slice bot Heavy damage");

                sBatch.Begin();
                sBatch.Draw(slicebotTexture, position, spriteRectangle, Color.White); // Then draw the slicebot with the texture, init pos and its hitbox and a color for its background.
                sBatch.DrawString(statusFont, "" + healthRound, statusTextVector, Color.Red); // Draw the status above the slicebot.
                sBatch.End();

                base.Draw(gameTime);

            }
            else if (slicebotState == SlicebotState.criticalDamage)
            {                
                slicebotTexture = Game.Content.Load<Texture2D>("Slice bot Critical damage");

                sBatch.Begin();
                sBatch.Draw(slicebotTexture, position, spriteRectangle, Color.White); // Then draw the slicebot with the texture, init pos and its hitbox and a color for its background.
                sBatch.DrawString(statusFont, "" + healthRound, statusTextVector, Color.Red); // Draw the status above the slicebot.
                sBatch.End();

                base.Draw(gameTime);

            }
        }

        private void animateSlicebotSlice() // Sorta works!, Use the grid you made to make sure the images perfectly line up!
        {
            if (animationTimer == 0) // If the timer is 0 (starting off or has been reset)
            {
                
                spriteRectangle.X = 0; // set the frame back to the first one.
            }
            else if (animationTimer % 2 == 0) // The timer value is a multiple of 5. (so 20 frames a second animation total as 100 / 5 = 20, changed for testing but put back to 5 when done as it works good.
            {
                if (spriteRectangle.X == 52) // Check where the rectangle is..
                {
                    spriteRectangle.X = 0; // invert between 0 and 52
                }
                else
                {
                    spriteRectangle.X += 52; // change to 52.
                }

            }               
            else if (animationTimer > 100)
            {
                animationTimer = 0;
            }

        }

        public void deactivateSlicebot() // Deactivate the slicebot.
        {
            slicebotActive = false;
        }

        public void activateSliceBot()
        {
            slicebotActive = true;
        }

        public Rectangle GetBounds() // Get the bounds of this slicebot, handy for determining collisions.
        {
            return new Rectangle((int)position.X, (int)position.Y, SLICEBOT_WIDTH, SLICEBOT_HEIGHT);
        }
    }
}
