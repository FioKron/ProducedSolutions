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
    public class LightMachineGun : Microsoft.Xna.Framework.DrawableGameComponent // To allow the bullets to draw too
    {
        private Texture2D bulletTexture;
        private int damage; // The damage this weapon does.
        private int timer; // Timer value used to time shots.

        private AudioComponent lmgAudioComponent;

        private Vector2 unitPos;
        private float Yspeed; // These two things effect their point of aim with this weapon
        private float Xspeed;

        private Game game;

        public LightMachineGun(Game game,Vector2 thisUnitPos,float initY,float initX, ref Texture2D ammoTexture)
            : base(game)
        {
            lmgAudioComponent = new AudioComponent(Game); // Set up the light machine gun's audio component.
            Game.Components.Add(lmgAudioComponent);

            unitPos = thisUnitPos;
            Yspeed = initY;
            Xspeed = initX;
            bulletTexture = ammoTexture;
            damage = 10; // Does 10 damage a shot.
            timer = 0;
            // TODO: Construct any child components here
            this.game = game;
        }

        public void machineGunBurst() // Be able to fire a bullet every so often.
        {
            if (timer > 10) // Fire shots every 10th of a second.
            {
                timer = 0;
            }

            if (timer == 10)
            {              
                Game.Components.Add(new Bullet(game, ref bulletTexture, new Vector2(unitPos.X + 20,unitPos.Y + 35), Yspeed, Xspeed, "Lmg")); // add a new bullet at the adjusted unit's position so it fires from where the gun is.  
                lmgAudioComponent.playCue("Unit lmg shot"); // Play the sound for firing a shot.
            }
            

        }


        public int getDamage()
        {
            return damage;
        }

        public void changeFiringAngle(float newY, float newX) // Change the firing angle of the weapon, may be redundent by simply parsing the values into machineGunBurst.
        {
            Xspeed = newX;
            Yspeed = newY;

        }

        public void changeStartPos(Vector2 newUnitPos) // Change the initial starting posistion for the bullet. May have same redundency as the above method.
        {
            unitPos = newUnitPos;
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
            //Update each of the bullets in turn.
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
            {}



            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw each of the bullets in turn if needed.
            foreach (GameComponent gc in Game.Components)
            {
                if (gc is Bullet)
                {
                    Bullet thisBullet = ((Bullet)gc);
                    thisBullet.Draw(gameTime);
                }
            }


            base.Draw(gameTime);

        }
    }
}
