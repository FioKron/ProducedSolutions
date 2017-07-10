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
    /// 
    /// TODO: UPDATE THE DRAW METHOD TO MAKE IT SO UNITS HAVE GRAPHICS WHILE HOLDING WEAPONS WHILE PRONE AND THAT THESE ARE DRAWN AS NEEDED.
    /// TEST MORE OF UNITS GOING PRONE AND STUFF, SEEMS TO WORK FOR NOW.
    /// </summary>
    public class Unit : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D unitTexture; // The unit's texture.
        private Rectangle spriteRectangle; // The rectangle for determining collosions of the UNIT with other things.
        private Rectangle screenBounds; // The rectangle for determining the bounds of the screen.
        private Vector2 position; // The position of this unit.
        private const int UNIT_WIDTH = 51; // The width and height of the unit when standing.
        private const int UNIT_HEIGHT = 88;
        private UnitState unitState;
        private OrderState orderState;
        private float Yspeed = 0.0f; // These two things effect their point of aim with a given weapon.
        private float Xspeed = 5.0f; 
        private LightMachineGun lmg; // A light machine gun the unit can carry.
        private Texture2D bulletTexture; // The texture of the bullets the units lmg fires.
        private Texture2D airBombTexture; // The texture of the bombs the airplanebomber drops.
        private Texture2D airBomberTexture; // The texture of this units bomber air strike call in.
        private Game game; // Required to set up this game component.
        private int unitHealth;
        private SpriteFont statusFont; // This font hovers over the turret and displays it's status.
        private bool unitControlled = false; // Whether or not the player is activly controlling this unit.
        private bool unitActive = false; // Whether the unit is active or not. ( Can be damaged etc)
        private int unitID;
        private float groundLevel; // The ground level of the unit, setup to be what the unit's starting Y pos is.
        private MouseState currentMouseState; // Current and previous mouse states for determining whether a unit has been clicked on, is being held down or has been clicked on then released.
        private MouseState previousMouseState;
        private bool collided = false; // Boolean flag for determining whether the unit has collided with something or not.
        private bool isJumping = false; // Boolean flag for whether or not the unit is jumping.
        private bool isProne = false; // Bool flag to say whether the unit is prone or not.
        private bool airStrikeReady = true; // Bool flag for whether or not the unit can use an air strike.
        private float jumpSpeed = 3; // The speed the unit can jump at, initiall set to two. (test value, seems to work well, test out higher and lower values)
        private const float UNIT_GRAVITY = 0.1f; // The gravity that this unit has. values for speed and gravity are set to change.
        private int animationTimer; // Timer used to do animations with.
        private int airStrikeTimer; // Timer used for airStrikes.
        private AudioComponent unitAudioComponent; // The unit's audio component.


        enum UnitState // The state of the unit, currently for simplcity (due to no animations) it is either idle or holding a weapon. 
        {
            idle, // Not holding a weapon, standing
            idleProne, // Not holding a weapon, prone.
            holdingLMG,
            holdingAirStrikePointer,
            holdingLMGProne,
            holdingAirStrikePointerProne,
            destroyed // Destroyed.
        }

        enum OrderState // What order state the unit is in, as in what orders it has or has not been given.
        {
            noOrders, // Act under no orders.
            fightingRetreatLeft, // Do a fighting retreat to the left.
            fightingRetreatRight, // Do a fighting retreat to the right.
            retreatLeft, // Just retreat left or right.
            retreatRight,
            duckAndCover // Order the units to take cover and go prone.
        }

        public Unit(Game game, Vector2 newPos,ref Texture2D ammoTexture,ref Texture2D airBombAmmoTexture,ref Texture2D bomberTexture,int newUnitID)
            : base(game)
        {
           // The init pos bullets from the mg should spawn at.
            unitAudioComponent = new AudioComponent(Game); // Create the unit's audio component.
            Game.Components.Add(unitAudioComponent);

            unitTexture = Game.Content.Load<Texture2D>("Unit"); // Set up the texture.           

            position = new Vector2(); // Set up the position.
            position = newPos;
            bulletTexture = ammoTexture;
            unitHealth = 100;
            unitID = newUnitID;
            airBomberTexture = bomberTexture; // Set up the air strike's texture.
            airBombTexture = airBombAmmoTexture; // And the texture of the bombs it uses. 
            unitActive = true; // Make the unit active.
            // Create rectangle.

            spriteRectangle = new Rectangle(0, 0, UNIT_WIDTH, UNIT_HEIGHT); // Create a rectangle for the unit that can be used for determining collisions etc.

            groundLevel = position.Y; // Set up the ground level of the unit, as usually the unit cannot go below this, will have to tweak as needed.

            screenBounds = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height); // Create a rectangle for the screenbounds to stop the UNIT flying of the screen.
            this.game = game; // Set up the game.
            statusFont = Game.Content.Load<SpriteFont>("SpriteFont2");
            lmg = new LightMachineGun(game, position, Yspeed, Xspeed, ref bulletTexture); // Set up the current weapon for the unit.
            currentMouseState = Mouse.GetState(); // Set up the initial mouse states.
            previousMouseState = currentMouseState;
        }

        public bool getUnitControled() // Get whether or not this unit is controlled.
        {
            return unitControlled;
        }

        private void animateUnitLMGShoot() // Animate the lmg being shot. (Side note, the guns have quite some recoil on them... unintended but cool :))
        {
            if (unitState == UnitState.holdingLMG) // If the unit is standing...
            {
                if (animationTimer == 0) // If the timer is 0 (starting off or has been reset)
                {
                    spriteRectangle.X = 0; // set the frame back to the first one.
                }
                else if (animationTimer % 5 == 0) // The timer value is a multiple of 5. 
                {
                    if (spriteRectangle.X == 52) // Check where the rectangle is..
                    {
                        spriteRectangle.X = 0; // invert between 0 and 52
                    }
                    else
                    {
                        spriteRectangle.X = 52; // change to 52.
                    }

                }
                else if (animationTimer >= 100)
                {
                    animationTimer = 0;
                }

            }
            else if (unitState == UnitState.holdingLMGProne)
            {
                if (animationTimer == 0) // If the timer is 0 (starting off or has been reset)
                {
                    spriteRectangle.X = 0; // set the frame back to the first one.
                }
                else if (animationTimer % 5 == 0) // The timer value is a multiple of 5. (so 20 frames a second animation total as 100 / 5 = 20, changed for testing but put back to 5 when done as it works good.
                {
                    if (spriteRectangle.X == 89) // Check where the rectangle is..
                    {
                        spriteRectangle.X = 0; // invert between 0 and 89
                    }
                    else
                    {
                        spriteRectangle.X = 89; // change to 89.
                    }

                }
                else if (animationTimer >= 100)
                {
                    animationTimer = 0;
                }

            }
            

        }

        public String getOrderStateString() // Get the order state as a string.
        {
            String stringOrderState = "";

            if (orderState == OrderState.retreatLeft) // Need more like these as the need arises.
            {
                stringOrderState = "retreatLeft";
            }

            return stringOrderState;
        }

        public int getUnitID()
        {
            return unitID;
        }


        private bool isMouseOver() // Used to determine if the mouse is hovered over a unit
        {
            MouseState mouseState = Mouse.GetState(); // Get the mouse's state.

            Rectangle unitCollRect = this.GetBounds(); // Make a collision rectangle that is set to the bounds of the unit's rectangle.
            Rectangle mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1); // Make a small rectangle to test whether it intersects the button.


            if (mouseRectangle.Intersects(unitCollRect)) // If the mouse is within the bounds of the rectangle..
            {
                return true; // State that this is true.
            }
            else
            {
                return false; // Otherwise its not.
            }

        }


        public bool isLeftClicked() // check if this unit has been left clicked.
        {

            if (this.isMouseOver()) // First check if the unit has a mouse over it
            {
                if (previousMouseState.LeftButton == ButtonState.Pressed
                    && currentMouseState.LeftButton == ButtonState.Released)  // then check if the left mouse button has been pressed. (Must now be clicked on then released for this to work).
                {
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

        public void setUnitControled(bool newControlStatus)
        {
            unitControlled = newControlStatus;
        }

        public Vector2 getPos()
        {
            return position;
        }

        public bool getUnitActive()
        {
            return unitActive;
        }

        public bool checkCollision(Rectangle rect) // Check collisions between this unit and other things. (Like slicebots)
        {
            if (unitState != UnitState.destroyed) // Only check for collisions if the unit is not destroyed.
            {
                Rectangle spriterect = new Rectangle((int)position.X, (int)position.Y, UNIT_WIDTH, UNIT_WIDTH); // Make a new rectangle that fits the target...
                // Then see if it intersects something. (This method call returns a boolean value; true or false)
                collided = spriterect.Intersects(rect); // Make sure this line is called in order to prevent double hitting nonsense.               
            }
            return collided;
        }

        public String getUnitStateString() // Update this too.
        {
            if (unitState == UnitState.idle)
            {
                return "idle";
            }
            else if (unitState == UnitState.holdingLMG)
            {
                return "holdingLMG";
            }
            else if (unitState == UnitState.destroyed)
            {
                return "destroyed";
            }
            else if (unitState == UnitState.holdingAirStrikePointer)
            {
                return "holdingAirStrikePointer";
            }
            else if (unitState == UnitState.holdingAirStrikePointerProne)
            {
                return "holdingAirStrikePointerProne";
            }
            else
            {
                return "UnitStateNotFound";
            }

        }

        public void beDamaged(int damage) // The turret gets damaged.
        {
            unitHealth -= damage;

            if (unitHealth <= 0)
            {
                unitHealth = 0;
                this.Dispose(); // Get rid of the turret for now.
                unitState = UnitState.destroyed;
                unitActive = false;
            }

        }

        public LightMachineGun getMachineGun()
        {
            return lmg;
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

        public void forceMoveRight(float amount) // force the unit to move right to a certain degree.
        {
            position.X += amount; // Be forced to the right.

            if (position.X > screenBounds.Width - UNIT_WIDTH) // But can't fall off the screen.
            {
                position.X = screenBounds.Width - UNIT_WIDTH;
            }
        }

        public void forceMoveLeft(float amount) // force the unit to move left.
        {
            position.X -= amount; // Be forced to the left.

            if (position.X < screenBounds.Left) // Make sure the unit cannot fall off the screen though.
            {
                position.X = screenBounds.Left;
            }
        }

        public void setOrdersFightingRetreatLeft() // Order a fighting retreat left.
        {
            orderState = OrderState.fightingRetreatLeft;
        }

        public void setOrdersRetreatLeft() // Order a retreat to the left.
        {
            orderState = OrderState.retreatLeft; 
        }

        public void setOrdersDuckAndCover() // Order units to take cover.
        {
            orderState = OrderState.duckAndCover;
        }

        public void setOrdersRetreatRight() // Orders a retreat to the right.
        {
            orderState = OrderState.retreatRight;
        }

        public void setOrdersFightingRetreatRight() // Order a fighting retreat right
        {
            orderState = OrderState.fightingRetreatRight;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            KeyboardState keyboard = Keyboard.GetState(); // Make a keyboard for determining movement
            GamePadState gamePad = GamePad.GetState(PlayerIndex.One); // Make a 360 gamepad to do the same.
            animationTimer++; // Update  the animation timer value.

            if (airStrikeReady == false) // Only decrement the airstrike timer if it is false.
            {
                airStrikeTimer--;

                if (airStrikeTimer <= 0) // When the timer reaches zero or less then it the air strike is ready to use again. (Make sure it is a less than sign or it screws this up)
                {
                    airStrikeTimer = 0;
                    airStrikeReady = true;
                }
            }

            /// The following if statements check what keys/buttons are pressed and move the unit accordingly.
            /// If the UNIT is about to go off the screen the program makes sure it does not.
            /// The keys can be any keys on the keyboard, when reconfiguration is possible, for now they are hard coded.

            // Update the inital starting point for bullets.

            previousMouseState = currentMouseState; // Update the states of the mouse.
            currentMouseState = Mouse.GetState();

            if (unitState != UnitState.destroyed && unitControlled == true) // If the unit is not destroyed and the player is controlling it...
            {
                lmg.changeStartPos(position);

                // Make the unit go left or right

                if (keyboard.IsKeyDown(Keys.A) || gamePad.IsButtonDown(Buttons.LeftThumbstickLeft))
                {
                    if (orderState == OrderState.noOrders) // If acting under no order bounuses...
                    {
                        if (isProne == false) // Check whether the unit is not in the 'prone' stance as units move slower while prone.
                        {
                            position.X -= 1; // Update position.
                        }
                        else // They are prone so..
                        {
                            position.X -= 0.25f; // move at 1/4 speed.
                        }
                        
                    }
                    else if (orderState == OrderState.fightingRetreatLeft || orderState == OrderState.retreatLeft) // Otherwise boost speed by 3x when moving left.
                    {
                        if (isProne == false) // Check whether the unit is not in the 'prone' stance as units move slower while prone.
                        {
                            position.X -= 3; // Update position.
                        }
                        else // They are prone so..
                        {
                            position.X -= 1; // move at 1/4 speed.
                        }
                        
                    }
                    

                    if (keyboard.IsKeyUp(Keys.LeftShift)) // Only aim in that direction if shift is not pressed (To allow backpedealing) .
                    {
                        aimLeft(); // Now updated to make it so the unit fires in the direction they are facing/moving... for now.
                        lmg.changeFiringAngle(Yspeed, Xspeed);
                    }

                    
                    
                }

                if (keyboard.IsKeyDown(Keys.D) || gamePad.IsButtonDown(Buttons.LeftThumbstickRight))
                {
                    if (isProne == false) // Check whether the unit is not in the 'prone' stance as units move slower while prone.
                    {
                        position.X += 1; // Update position.
                    }
                    else // They are prone so..
                    {
                        position.X += 0.25f; // move at 1/4 speed.
                    }

                    if (keyboard.IsKeyUp(Keys.LeftShift))
                    {
                        aimRight();
                        lmg.changeFiringAngle(Yspeed, Xspeed);
                    }
                    
                }

                if (keyboard.IsKeyDown(Keys.S) || gamePad.IsButtonDown(Buttons.LeftThumbstickDown)) // The unit goes prone.
                {
                    if (unitState == UnitState.idle)
                    {
                        unitState = UnitState.idleProne;
                    }
                    else if (unitState == UnitState.holdingLMG)
                    {
                        unitState = UnitState.holdingLMGProne;
                    }
                    else if (unitState == UnitState.holdingAirStrikePointer)
                    {
                        unitState = UnitState.holdingAirStrikePointerProne;
                    }

                    if (isProne == false) // Only adjust the units dimensions if it is not already prone.
                    {
                        spriteRectangle.Height = 51;
                        spriteRectangle.Width = 88;

                        position.Y += 37; // Make the unit lower down.
                        isProne = true;
                    }                
                }

                if (keyboard.IsKeyDown(Keys.W) || gamePad.IsButtonDown(Buttons.LeftThumbstickUp)) // Stand up again.
                {
                    if (keyboard.IsKeyUp(Keys.LeftShift))
                    {
                        aimUp();
                        lmg.changeFiringAngle(Yspeed, Xspeed); // Change the firing angle as per it being updated here.
                    }

                    if (unitState == UnitState.idleProne) // Check what prone state the unit is in then make them in the same standing state. (e.g. holding lmg prone to holding lmg while standing etc)
                    {
                        unitState = UnitState.idle;
                    }
                    else if (unitState == UnitState.holdingLMGProne)
                    {
                        unitState = UnitState.holdingLMG;
                    }
                    else if (unitState == UnitState.holdingAirStrikePointerProne)
                    {
                        unitState = UnitState.holdingAirStrikePointer;
                    }

                    if (isProne == true)
                    {
                        spriteRectangle.Height = 88;
                        spriteRectangle.Width = 51;

                        position.Y -= 37; // This much seems to make the unit stand and go prone to the right level as needed.
                        isProne = false;
                    }

                    
                }

                if (keyboard.IsKeyDown(Keys.D1) || gamePad.IsButtonDown(Buttons.DPadRight)) // Switch between different weapons.
                {
                    if (isProne == false)
                    {
                        unitState = UnitState.holdingLMG;
                    }
                    else
                    {
                        unitState = UnitState.holdingLMGProne;
                    }
                    
                }

                if (keyboard.IsKeyDown(Keys.D2))
                {
                    if (isProne == false)
                    {
                        unitState = UnitState.holdingAirStrikePointer;
                    }
                    else
                    {
                        unitState = UnitState.holdingAirStrikePointerProne;
                    }
                }

                if (keyboard.IsKeyDown(Keys.D0) || gamePad.IsButtonDown(Buttons.RightStick)) // Switch to holding nothing.
                {
                    if (isProne == false)
                    {
                        unitState = UnitState.idle;
                    }
                    else
                    {
                        unitState = UnitState.idleProne;
                    }
                    
                }
                // Weapons that are automatic in nature. (change controller buttons to have previous and current states too.)
                if ((currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed) || gamePad.IsButtonDown(Buttons.RightTrigger)) //Left mouse or right trigger to fire.
                {
                    if (unitState == UnitState.holdingLMG || unitState == UnitState.holdingLMGProne) // Check what weapon the unit is holding, in this case if it is holding an lmg...
                    {
                        lmg.machineGunBurst();
                        animateUnitLMGShoot(); // Animate the unit firing.
                    }
                }
                else if((currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Released) || gamePad.IsButtonUp(Buttons.RightTrigger) && unitControlled == true)  // Otherwise, make sure the sprite rectangle is centred over the unit's idle animation.
                {
                    spriteRectangle.X = 0;
                }

                // One click weapons.
                
                if ((currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released) || gamePad.IsButtonDown(Buttons.RightTrigger)) //Left mouse or right trigger to fire.
                {
                    if ((unitState == UnitState.holdingAirStrikePointer || unitState == UnitState.holdingAirStrikePointerProne) && airStrikeReady == true) // Only allow an air srike to be called in if its ready.
                    {
                        Game.Components.Add(new AirPlaneBomber(game, ref airBomberTexture,ref airBombTexture, new Vector2(0, 0), 1,new Vector2(currentMouseState.X,currentMouseState.Y)));

                        airStrikeHasBeenUsed(); // The airstrike has now been used.

                        unitAudioComponent.playCue("Unit callout request airstrike");

                        unitAudioComponent.playCue("Air strike pilot confirm command"); // Make sure to use this audio file for the bomber pilot.

                        unitAudioComponent.playCue("Bomber flying"); // The sound of the bomber flying.
                    }
                }


                if (((isJumping == false && keyboard.IsKeyDown(Keys.Space) && isProne == false) || (isJumping == false && gamePad.IsButtonDown(Buttons.X) && isProne == false))) // Jump, the jump button really should be w not space, was merly for testing.
                    // The units can only jump if they are not prone and are already jumping.
                {
                    isJumping = true; // Make it so the unit is now jumping if they were not already, i.e. they can only jump if they are not already jumping.
                }

                if (isJumping == true) // If this unit is jumping...
                {
                    doJump(); // Do a jump. 
                }


                // Make sure it can not go off the screen at any edge.

                if (position.X < screenBounds.Left)
                {
                    position.X = screenBounds.Left;
                }

                if (position.X > screenBounds.Width - UNIT_WIDTH)
                {
                    position.X = screenBounds.Width - UNIT_WIDTH;
                }

                if (position.Y < screenBounds.Top)
                {
                    position.Y = screenBounds.Top;
                }

                if (position.Y > screenBounds.Height - UNIT_HEIGHT)
                {
                    position.Y = screenBounds.Height - UNIT_HEIGHT;
                }


            }
            else if (unitState != UnitState.destroyed && unitControlled == false && orderState == OrderState.noOrders) // Resort to AI control acting under no orders.
            {
                if (isProne == true)
                {
                    spriteRectangle.Height = 88;
                    spriteRectangle.Width = 51;

                    position.Y -= 37; // This much seems to make the unit stand and go prone to the right level as needed.
                    isProne = false;
                }


                lmg.changeStartPos(position);


                if (isJumping == true) // If this unit is jumping...
                {
                    doJump(); // Do a jump. 
                }

                try
                {
                    foreach (GameComponent gc in Game.Components) // check for targets..
                    {
                        if (gc is PlasmaTurret) // If there is a plasma turret.
                        {
                            PlasmaTurret autoTurret = ((PlasmaTurret)gc); // get this local plasma turret.

                            if (autoTurret.getTurretActive() == true) // Then check if its active and if so...
                            {
                                aimRight(); // Aim right..
                                lmg.changeFiringAngle(Yspeed, Xspeed); // Adjust the weapon with new aim...
                                unitState = UnitState.holdingLMG; // Switch to the lmg...
                                lmg.machineGunBurst(); // Shoot.
                                animateUnitLMGShoot(); // Animate the unit firing.
                                
                            }
                        }
                        else if (gc is Slicebot) // If there is a slicebot..
                        {
                            Slicebot slicebot = ((Slicebot)gc); // Get this local slice bot.

                            if (slicebot.getSlicebotActive() == true) // If the slicebot is active...
                            {
                                aimRight(); // Aim right..
                                lmg.changeFiringAngle(Yspeed, Xspeed); // Adjust the weapon with new aim...
                                unitState = UnitState.holdingLMG; // Switch to the lmg...
                                lmg.machineGunBurst(); // Shoot.
                                animateUnitLMGShoot(); // Animate the unit firing.
                            }
                        }

                    }

                }
                catch (InvalidOperationException ioe) // swallow this exception for now. (Don't sue me :( )
                { }            
            }
            else if (unitState != UnitState.destroyed && unitControlled == false && orderState == OrderState.fightingRetreatLeft) // Otherwise if a fighting retreat left is ordered...
            {
                if (isProne == true) // For now make the units stand up to run away.
                {
                    spriteRectangle.Height = 88;
                    spriteRectangle.Width = 51;

                    position.Y -= 37; // This much seems to make the unit stand and go prone to the right level as needed.
                    isProne = false;
                }

                lmg.changeStartPos(position); // Update the init pos of the units firing.
                aimRight(); // Aim right..
                lmg.changeFiringAngle(Yspeed, Xspeed); // Adjust the weapon with new aim.
                unitState = UnitState.holdingLMG; // Switch to the lmg...
                lmg.machineGunBurst(); // Shoot.
                animateUnitLMGShoot(); // Animate the unit firing.

                position.X -= 3; // Move at 3x speed.

                if (position.X < screenBounds.Left) // Make sure the units do not fall off the screen.
                {
                    position.X = screenBounds.Left;
                }

            }
            else if (unitState != UnitState.destroyed && unitControlled == false && orderState == OrderState.retreatLeft) // Otherwise if just a retreat to the left has been ordered..
            {
                if (isProne == true) // For now make the units stand up to run away.
                {
                    spriteRectangle.Height = 88;
                    spriteRectangle.Width = 51;

                    position.Y -= 37; // This much seems to make the unit stand and go prone to the right level as needed.
                    isProne = false;
                }

                spriteRectangle.X = 0; // Make sure the sprite rectangle is focused on thier idle animation.

                unitState = UnitState.idle; // Just run away, don't focus on using weapons.

                position.X -= 3; // Move at 3x speed.

                if (position.X < screenBounds.Left) // Make sure the units do not fall off the screen.
                {
                    position.X = screenBounds.Left;
                }            
            }
            else if (unitState != UnitState.destroyed && unitControlled == false && orderState == OrderState.duckAndCover) // Order the units to take cover.
            {
                if (isProne == false) // If the units are not already prone...
                {
                    spriteRectangle.Height = 51; // Make them so.
                    spriteRectangle.Width = 88;

                    position.Y += 37; // This much seems to make the unit stand and go prone to the right level as needed.
                    isProne = true;
                }

                lmg.changeStartPos(position); // Update the init pos of the units firing.
                aimRight(); // Aim right..
                lmg.changeFiringAngle(Yspeed, Xspeed); // Adjust the weapon with new aim.
                unitState = UnitState.holdingLMGProne; // Switch to the lmg...
                lmg.machineGunBurst(); // Shoot.
                animateUnitLMGShoot(); // Animate the unit firing.
            }
            else if (unitState != UnitState.destroyed && unitControlled == false && orderState == OrderState.fightingRetreatRight) // Otherwise if a fighting retreat Right is ordered...
            {
                if (isProne == true) // For now make the units stand up to run away.
                {
                    spriteRectangle.Height = 88;
                    spriteRectangle.Width = 51;

                    position.Y -= 37; // This much seems to make the unit stand and go prone to the right level as needed.
                    isProne = false;
                }

                lmg.changeStartPos(position); // Update the init pos of the units firing.
                aimLeft(); // Aim left...
                lmg.changeFiringAngle(Yspeed, Xspeed); // Adjust the weapon with new aim.
                unitState = UnitState.holdingLMG; // Switch to the lmg...
                lmg.machineGunBurst(); // Shoot.
                animateUnitLMGShoot(); // Animate the unit firing.

                position.X += 3; // Move at 3x speed.

                if (position.X > screenBounds.Width - UNIT_WIDTH) // Make sure they cannot fall off the screen.
                {
                    position.X = screenBounds.Width - UNIT_WIDTH;
                }

            }
            else if (unitState != UnitState.destroyed && unitControlled == false && orderState == OrderState.retreatRight) // Otherwise if just a retreat to the right has been ordered..
            {
                if (isProne == true) // For now make the units stand up to run away.
                {
                    spriteRectangle.Height = 88;
                    spriteRectangle.Width = 51;

                    position.Y -= 37; // This much seems to make the unit stand and go prone to the right level as needed.
                    isProne = false;
                }

                spriteRectangle.X = 0; // Make sure the sprite rectangle is focused on thier idle animation.

                unitState = UnitState.idle; // Just run away, don't focus on using weapons.

                position.X += 3; // Move at 3x speed.

                if (position.X > screenBounds.Width - UNIT_WIDTH) // Make sure they cannot fall off the screen.
                {
                    position.X = screenBounds.Width - UNIT_WIDTH;
                }
            }

            try
            {
                foreach (GameComponent gc in Game.Components) // Update each bomber in the game components.
                {
                    if (gc is AirPlaneBomber)
                    {
                        AirPlaneBomber thisBomber = ((AirPlaneBomber)gc);
                        thisBomber.Update(gameTime);
                    }
                }
            }
            catch (InvalidOperationException ioe)
            {}

            lmg.Update(gameTime);          

            base.Update(gameTime);
        }

        public bool getAirStrikeReady()
        {
            return airStrikeReady;
        }

        public void airStrikeHasBeenUsed()
        {
            if (airStrikeReady == true) // Only do this if airstrike ready is true. (To ensure that the player does not have to wait for the cooldown again if it was already cooling down)
            {
                airStrikeReady = false; // The airstrike has been used so it is no longer ready.
                airStrikeTimer = 500; // Set the timer to 5 seconds.
            }           
        }

        private void doJump() // Do what is needed to jump.
        {
            
            position.Y -= jumpSpeed; // Increase height by velocity...

            jumpSpeed -= UNIT_GRAVITY; // but decrease this velocity by gravity.

            if (position.Y >= groundLevel) // Check if the unit has hit the ground. (This is backwards to what you would think as pos y being greater means it is lower)
            {
                position.Y = groundLevel; // Set the height to ground level...
                isJumping = false; // and stop jumping.
                jumpSpeed = 3; // Reset the velocity.
            }

        }

        public void aimRight() // Aim up, left or right depending on the pressed keys above. (For bullet firing weapons anyway)
        {
              Yspeed = 0.0f;
              Xspeed = 5.0f; 
        }

        public void aimLeft()
        {
            Yspeed = 0.0f;
            Xspeed = -5.0f;
        }

        public void aimUp()
        {
            Yspeed = -5.0f;
            Xspeed = 0.0f;
        }

        public Rectangle GetBounds() // Get the bounds of this turret, handy for determining collisions.
        {
            return new Rectangle((int)position.X, (int)position.Y, UNIT_WIDTH, UNIT_HEIGHT);
        }


        public override void Draw(GameTime gameTime) // Now loads the textures in when needed.
        {

            SpriteBatch sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); // Make a new spritebatch
            Vector2 statusTextVector = new Vector2(position.X + 10, position.Y - 20); // offset the status text so it is above the unit.
            Vector2 airStrikeStatusVector = new Vector2(500, 0); // Status text vector for the readyness of the airstrike.

            if (unitState != UnitState.destroyed) // If the unit is not destroyed...
            {
                if (unitState == UnitState.idle) // Check what state the unit is in and draw it with the appropirate texture.
                {
                    unitTexture = Game.Content.Load<Texture2D>("Unit");
                    sBatch.Draw(unitTexture, position, spriteRectangle, Color.White); // Then draw the unit with the texture, init pos and its hitbox and a color for its background.
                    lmg.Draw(gameTime);
                }
                else if (unitState == UnitState.holdingLMG)
                {
                    unitTexture = Game.Content.Load<Texture2D>("Unit holding LMG");
                    sBatch.Draw(unitTexture, position, spriteRectangle, Color.White);
                    lmg.Draw(gameTime);
                }
                else if (unitState == UnitState.holdingAirStrikePointer)
                {
                    unitTexture = Game.Content.Load<Texture2D>("Unit holding Airstrike Pointer");
                    sBatch.Draw(unitTexture, position, spriteRectangle, Color.White);
                    lmg.Draw(gameTime);
                }
                else if (unitState == UnitState.idleProne)
                {
                    unitTexture = Game.Content.Load<Texture2D>("Unit prone");
                    sBatch.Draw(unitTexture, position, spriteRectangle, Color.White);
                    lmg.Draw(gameTime);
                }
                else if (unitState == UnitState.holdingLMGProne) 
                {
                    unitTexture = Game.Content.Load<Texture2D>("Unit Holding LMG prone");
                    sBatch.Draw(unitTexture, position, spriteRectangle, Color.White);
                    lmg.Draw(gameTime);
                }
                else if (unitState == UnitState.holdingAirStrikePointerProne)
                {
                    unitTexture = Game.Content.Load<Texture2D>("Unit Holding Airstrike Pointer prone");
                    sBatch.Draw(unitTexture, position, spriteRectangle, Color.White);
                    lmg.Draw(gameTime);

                }

                sBatch.DrawString(statusFont, "" + unitHealth, statusTextVector, Color.Red); // Draw the status above the unit.

                if (airStrikeReady == false)
                {
                    sBatch.DrawString(statusFont, "Time till airstrike ready:" + airStrikeTimer / 100, airStrikeStatusVector, Color.Red);
                }

            }

            foreach (GameComponent gc in Game.Components) // Draw each bomber in the game components.
            {
                if (gc is AirPlaneBomber)
                {
                    AirPlaneBomber thisBomber = ((AirPlaneBomber)gc);
                    thisBomber.Draw(gameTime);
                }
            }
               
            base.Draw(gameTime);

        }
    }
}
