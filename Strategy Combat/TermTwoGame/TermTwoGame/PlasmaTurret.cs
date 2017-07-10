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
    /// THINGS TO ADD: I
    /// 
    /// IMPROVE TURRET ANIMATION AND MAKE IT SO WHEN IT CHANGES DAMAGE STATES THAT RESETS IT TO THE FIRST PHASE OF FIRING.
    /// E.G. ON FRAME 5 THEN IS SET TO MILD DMG, SO SET IT TO FRAME ONE OF THE MILD DMG FIRING ANIM.
    /// 
    ///
    /// </summary>
    public class PlasmaTurret : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D turretTexture; // The turret's texture.
        private Rectangle spriteRectangle; // The rectangle for determining collosions of the turret with other things.
        private Texture2D ammoTexture; // Texture for the turret's ammuntion. 
        private Vector2 position; // The position of this turret.
        private float health; // The turret's health.
        private const int PLASTURRET_WIDTH = 91; // The width and height of the Turret.
        private const int PLASTURRET_HEIGHT = 120;
        private SpriteFont statusFont; // This font hovers over the turret and displays it's status.
        private Game game;
        private TurretState turretState; // An enum for the state of the turret.
        private int timer; // Timer value used to time shots.
        private Vector2 shotAngle; // The angle used to shoot with.
        private int damage; // The damage the turret does per shot.
        private bool turretActive = false;
        private float damageResistance; // The damage resistance of the turret.
        private AudioComponent plasTurrAudioComp; // So the plasma turret can play sounds.
        private bool isMissionOnePlasTurret; // Whether this plasma turret is the tut mission plasma turret.

        enum TurretState // The state of the turret.
        {
            active, // The turret is active and not damaged; not that the turret is only not active if it is destroyed...
            mildDamage, // The turret is still active but has taken mild damage.
            moderateDamage, // The turret is still active but has taken moderate damage.
            heavyDamage, // The turret is still active but has taken heavy damage.
            criticalDamage, // The turret is still active but has taken critical damage to all systems.
            destroyed // The turret is outright destroyed.           
        }


        public PlasmaTurret(Game game,Vector2 newPos, ref Texture2D theTexture, ref Texture2D projTexture, bool newIsMissionOnePlasTurret)
            : base(game)
        {
            plasTurrAudioComp = new AudioComponent(Game); // Instance the plasma turret's audio component.
            Game.Components.Add(plasTurrAudioComp); // Then add it to the game components.

            isMissionOnePlasTurret = newIsMissionOnePlasTurret;

            turretTexture = theTexture; // Set up the texture
            ammoTexture = projTexture;
            position = new Vector2(); // Set up the position
            position = newPos;
            health = 300; // Start the health at 300.
            damage = 50; // And the damage to 50.
            damageResistance = 0.2f;
            shotAngle = new Vector2(-2.5f,0.0f);
            turretActive = true; // Make the turret active.
            // Create rectangle
            
            spriteRectangle = new Rectangle(0, 0, PLASTURRET_WIDTH, PLASTURRET_HEIGHT); // Create a rectangle for the turret that can be used for determining collisions etc.

            this.game = game; // Set up the game.
            statusFont = Game.Content.Load<SpriteFont>("SpriteFont1");
            // TODO: Construct any child components here
        }

        public bool getIsMissOneTurr() // Get whether this is the mission one plasma turret.
        {
            return isMissionOnePlasTurret;
        }

        public bool getTurretActive()
        {
            return turretActive;
        }

        public int getDamage()
        {
            return damage;
        }

        public float getHealth() // Get this turret's health.
        {
            return health;
        }

        public void beDamaged(float incomingDamage,String damageType) // The turret gets damaged.
        {
            float totalDamage = 0;

            if (damageType == "Bullet") // The plasma turret has resistance against bullets...
            {
                totalDamage = incomingDamage * damageResistance;
            }
            else if (damageType == "AirBomb") // But none against bombs, however it takes no extra damage from bombs like the slicebot does.
            {
                totalDamage = incomingDamage; 
            }
            

            health -= totalDamage;

            if (health <= 0)
            {

                health = 0;
                this.Dispose(); // Get rid of the turret for now.
                turretState = TurretState.destroyed;
                turretActive = false;
            }
            else if (health < 250 && health > 200) // Take care with this logic, the following statements determine how damaged the turret is and show this.
            {              
                turretState = TurretState.mildDamage;
            }
            else if (health < 199 && health > 150)
            {
                turretState = TurretState.moderateDamage;
            }
            else if (health < 149 && health > 100)
            {
                turretState = TurretState.heavyDamage;
            }
            else if (health < 99 && health > 50)
            {
                turretState = TurretState.criticalDamage;
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
            timer++;

            try
            {
                foreach (GameComponent gc in Game.Components)
                {
                    if (gc is Bullet)
                    {
                        Bullet thisBullet = ((Bullet)gc);
                        thisBullet.Update(gameTime);
                    }
                }
            }
            catch (InvalidOperationException ioe) // swallow this exception for now. (Don't sue me :( )
            { }

            if (turretState != TurretState.destroyed) // If the turret is not destroyed; shoot.
            {
                autoShoot();
                animateFiringSequence(); // as well as animate the firing sequence.
            }

            base.Update(gameTime);
        }

        private void autoShoot()
        {
            if (timer > 100) // Fire shots every second
            {
                timer = 0;
            }

            if (timer == 0) // Play the charging sound.
            {
                plasTurrAudioComp.playCue("Plasma turret charge");
            }

            if (timer == 100)
            {
                Vector2 initPos = new Vector2(position.X, position.Y + 30);
                Game.Components.Add(new Bullet(game, ref ammoTexture, initPos,shotAngle.Y, shotAngle.X, "Plasma Turret"));
                plasTurrAudioComp.playCue("Plasma turret shoot");
            }

        }

        private void animateFiringSequence() // Sorta works!, Use the grid you made to make sure the images perfectly line up!
        {
            if (timer == 0) // If the timer is 0 (starting off or has been reset)
            {
                spriteRectangle.X = 0; // set the frame back to the first one.
            }
            else if (timer % 10 == 0) // The timer value is a multiple of 10.
            {
                spriteRectangle.X += 92; // Add the width plus one.
            }

        }

        public override void Draw(GameTime gameTime)
        {

            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); // Make a new spritebatch

            Vector2 statusTextVector = new Vector2(position.X,position.Y - 50);

            if (turretState == TurretState.active) // Draw the non damaged turret, does not need to load texture as already loaded.
            {
                sBatch.Begin();
                sBatch.Draw(turretTexture, position, spriteRectangle, Color.White); // Then draw the turret with the texture, init pos and its hitbox and a color for its background.
                sBatch.DrawString(statusFont, "" + health, statusTextVector, Color.Red); // Draw the status above the turret.
                sBatch.End();

                base.Draw(gameTime);
            }
            else if (turretState == TurretState.mildDamage) // Otherwise draw the differing states of a damaged turret.
            {
                turretTexture = Game.Content.Load<Texture2D>("Plasma Turret Mild damage"); // Load the mild damage texture.

                sBatch.Begin();
                sBatch.Draw(turretTexture, position, spriteRectangle, Color.White); // Then draw the turret with the texture, init pos and its hitbox and a color for its background.
                sBatch.DrawString(statusFont, "" + health, statusTextVector, Color.Red); // Draw the status above the turret.
                sBatch.End();

                base.Draw(gameTime);
            }
            else if (turretState == TurretState.moderateDamage)
            {
                turretTexture = Game.Content.Load<Texture2D>("Plasma Turret Moderate damage"); // Load the moderate damage texture.

                sBatch.Begin();
                sBatch.Draw(turretTexture, position, spriteRectangle, Color.White); // Then draw the turret with the texture, init pos and its hitbox and a color for its background.
                sBatch.DrawString(statusFont, "" + health, statusTextVector, Color.Red); // Draw the status above the turret.
                sBatch.End();

                base.Draw(gameTime);
            }
            else if (turretState == TurretState.heavyDamage)
            {
                turretTexture = Game.Content.Load<Texture2D>("Plasma Turret Heavy damage"); // Load the heavy damage texture.

                sBatch.Begin();
                sBatch.Draw(turretTexture, position, spriteRectangle, Color.White); // Then draw the turret with the texture, init pos and its hitbox and a color for its background.
                sBatch.DrawString(statusFont, "" + health, statusTextVector, Color.Red); // Draw the status above the turret.
                sBatch.End();

                base.Draw(gameTime);
            }
            else if (turretState == TurretState.criticalDamage)
            {
                turretTexture = Game.Content.Load<Texture2D>("Plasma Turret Critical damage"); // Load the critical damage texture.

                sBatch.Begin();
                sBatch.Draw(turretTexture, position, spriteRectangle, Color.White); // Then draw the turret with the texture, init pos and its hitbox and a color for its background.
                sBatch.DrawString(statusFont, "" + health, statusTextVector, Color.Red); // Draw the status above the turret.
                sBatch.End();

                base.Draw(gameTime);
            }

            foreach (GameComponent gc in Game.Components)
            {
                if (gc is Bullet)
                {
                    Bullet thisBullet = ((Bullet)gc);
                    thisBullet.Draw(gameTime);
                }
            }
            
        }

        public void deactivateTurret() // Deactivate the turret. 
        {
            turretActive = false;
        }

        public void activateTurret() // Activate the turret.
        {
            turretActive = true;
        }

        public Rectangle GetBounds() // Get the bounds of this turret, handy for determining collisions.
        {
            return new Rectangle((int)position.X, (int)position.Y, PLASTURRET_WIDTH, PLASTURRET_HEIGHT);
        }
    }
}
