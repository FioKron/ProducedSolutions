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
    /// This is the main type for your game
    /// TODO: THings to add:
    /// Add pickups to survival mode.
    /// Death animins explosions etc
    /// more enemy types
    /// 
    /// NOTE THAT A MEGA BUG THAT APPEARS WHEN PLAYING IS THAT STUFF NEITHER DRAWS NOR UPDATES WHEN IT IS NOT SUPPOSED TO BUT IS STILL THERE.
    /// SEE THE SURVIVAL CLEAN UP METHOD FOR POTENTIAL SOLOTUION.
    /// ALSO NOTE THAT WITH THIS IMPLEMENTATION IT DOES NOT RUN VERY SMOOTHLY.
    /// SEE UPDATE METHOD FOR POTENTIAL FIX.
    /// 
    /// 
    /// MEGA BIG NOTE: MAKE SO ADDING THE SLICEBOT AND PLASMA TURRET TO THE TUTORIAL LEVELS IS MORE DYNAMIC AND THEN CHANGE THE STATE CHECKING METHODS.
    /// 
    /// 
    /// BIGEST TODO: IMPLEMENT SINGLE PLAYER SURVIVAL MODE; PUT PICKUPS IN THAT + OTHER ENEMIES AND OTHER TRICKS.
    /// 
    /// Note: I have fixed the unit selection arrow drawing when it is not supposed to.
    ///
    /// 
    /// CURRENTLY THIS IMPLEMENTATION OF THE SLICEBOT SEEMS TO BE FINE BUT ALWAYS MAKE SURE TO CHECK THAT THINGS ARE NOT NULL WHEN CHECKING THEM.
    /// KINDA CONFIRMED BY HELP BROUGHT UP; NULL REFERENCE EXCEPTION CAUSES THE CRASH. (BUT SEE BELOW)
/// 
    /// SUPER MEGA NOTE; MAKE SURE TO RUN IN WINDOWED MODE IN ORDER TO FIND BUGS AND STUFF, CAN JUST RUN IN FULLSCREEN THEN SWITCH TO WINDOWED IN THE PROGRAM.
    /// THIS HAS HELPED FIX ALOT OF BUGS SO BE SURE TO DO IT IF CRASHES OCCUR ETC
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private SpriteFont gameFont; // The game's font.
        private Texture2D credits; // The texture for the credits button.
        private Texture2D singlePlayer; // The texture for the single player options menu.
        private Texture2D multiPlayer; // The texture for the multi player options menu.
        private Texture2D options; // The texture for the options menu.
        private Texture2D back; // The texture for the back button.
        private Texture2D singleMissions; // The texture for the single player missions button.
        private Texture2D exit; // The texture for the exit button.
        private Texture2D missionOne; // The texture for the first single player mission button.
        private Texture2D missionOneBackground; // The texture for the background of the first mission.
        private Texture2D menuBackGround; // The texture for the menu background
        private Texture2D unit; // The default unit texture.
        private Texture2D plasmaTurret; // The plasma turret's texture.
        private Texture2D bullet; // Texture for standard issue bullets like lmg bullets.
        private Texture2D windowedModeUnticked; // The texture for the windowed mode tick button.
        private Texture2D windowedModeTicked; // The other texture for the windowed mode tick button.
        private Texture2D plasmaProjectile; // Texture for plasma projectiles.
        private Texture2D missionTwo; // The texture for the second single player mission button.
        private Texture2D singlePlayerSurvival; // The texture for the single player Survival button.
        private Texture2D airPlaneBomberTexture; // The air plane bomber's texture.
        private Texture2D airBombAmmoTexture; // The texture for the air plane bomber's ammo.
        private Texture2D slicebotTexture;
        private Unit[] squad; // An array of units, the squad the player has control over.
        private PointingArrow squadOneArrow;
        private Button buttonCredits; // The credits menu button.
        private Button buttonMissOne; // The first single player mission button.
        private Button buttonSingle; // The single player button.
        private Button buttonOptions;// The options button.
        private Button buttonWindowedMode; // This button is abit different in that it will change between showing a tick and nothing to denote windowed mode being on and off.
        private Button buttonBack; // The back button.
        private Button buttonSinglePlayerMissions; // The single player missions button.
        private Button buttonExit;
        private Button buttonMissTwo; // The second single player mission button.
        private Button buttonSinglePlayerSurvival; // The button for the single player survival mode.
        private GameState gameState; // The state of the game.
        private int selectedUnit = 0; // The currently selected unit. (For mission squads)
        private Texture2D pointArrowTexture; // The texture for the arrow that points to which unit is selected
        private GamePadState currentGamePadState; // The current and previous gamepad states, used for better control when using a 360 gamepad. (So players don't cycle through the units too quickly and can't accuractly pick the one they want to select)
        private GamePadState previousGamePadState;
        private KeyboardState currentKeyBoardState; // The current and previous keyboard states for better telling if whether keys and held of have been pressed etc.
        private KeyboardState previousKeyBoardState;
        private MouseState currentMouseState; // Current and previous mouse states for determining whether a button has been clicked on, is being held down or has been clicked on then released.
        private MouseState previousMouseState;
        private SpriteFont tutorialMessageFont; // Font used for the credits as well as some other things
        private SpriteFont actualTutMessageFont; // Font used for the actual tutorial messages.
        private bool noUnitSelected = false; // value for whether or not a unit is selected.
        private bool gameInPlay = false; // Flag for whether or not a game is in a play state.
        private MissionState missionState; // The state of what mission the player is on.
        private int survivalElaspedTime; // The time the survival mission has been going on for.
        private AudioComponent audioComponent;
        private int sliceBotAddCounter = 0;
        private int plasmaTurretAddCounter = 0;
        private int timeTillNextSliceBot = 6000; // spawn slice bots every 60 seconds. = 6000
        private int timeTillNextPlasmaTurret = 3000; // Spawn plasma turrets every 30 seconds. = 3000
        private float missOnePlasmaTurretHealth; // The health of the mission one plasma turret, to determine whether it has been destroyed or not.
        private float missTwoSlicebotHealth; // The health of the mission two slicebot, to determine whether it has been destroyed or not.

        enum GameState // This enum here determines the state of the game
        // Currently the user can be looking at:
        { // (Note that for enums, any instance of this enum will default to the value at the top of the list, so this works as intended)
            mainMenu, // The main menu and all options..
            singlePlayerMenu, // the singleplayer menu....
            multiPlayerMenu, // the multiplayer menu....
            options, // or the options menu.
            credits, // or the credits.
            singlePlayerMissions, // There is also a single player missions menu.
            missionOneWelcome, // This state represents the game welcoming the player.
            missionOneMovementControls, // Introducting movement controls...
            missionOneCombatControls, // and combat controls...
            missionOneUnitSelectionControls, // as well as unit selection controls
            missionOneMakeReady, // This state represents the game just before the player hits ready.
            missionOnePlayStageOne, // This state represents when mission one is being played.
            missionOnePlayerLost, // This state represents the player losing this mission.
            missionOnePlayerWon, // This state represents the player winning the first mission.
            missionTwoWelcome, // The initial state of mission two.
            missionTwoAlert, // Alert state of mission two.
            missionTwoPlayStageOne, // The first state of mission two being played, runs for about 5 seconds.
            missionTwoWarning, // Warning state of mission two.    
            missionTwoHelpBackPedal, // Backpedal help state.
            missionTwoHelpOrders, // Orders help state.
            missionTwoGetReady, // Get ready after help state.
            missionTwoPlayStageTwo, // The second stage of mission two being played, runs for longer but is not the penultmate state.
            missionTwoHelpRetreatOrder, // The state of telling the player about just retreating and not firing.
            missionTwoHelpSelectNoUnits, // The state of telling the player how to select no units.
            missionTwoPlayStageThree, // The third state of play of the second mission.
            missionTwoWellDoneStageThree, // Congratulate the player after doing the 2nd objective.
            missionTwoHelpAirStrike, // The state of telling the player how to use airstrikes.
            missionTwoPlayStageFour, // The final stage of mission two.
            missionTwoPlayerLost, // The state of the player losing the second mission.
            missionTwoPlayerWon, // The state of the player winning the second mission.
            singlePlayerSurvivalWelcome, // The welcome state of the single player surivial mission.
            singlePlayerSurvivalHint, // Give a hint to the player before they begin.
            singlePlayerSurvivalPlayStageOne, // The first and hopefully only stage of play, the player plays till they lose.
            singlePlayerSurvivalPlayerLost // The loss state of the single player survival mission.
        }

        enum MissionState // The state of what mission the player is on.
        {
            noMission, // The state of having no mission.
            missionOne, // The state playing mission one.
            missionTwo, // The state of playing mission two.
            singlePlayerSurvival // The state of playing the single player survival mission.
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.IsFullScreen = false; // In order to record gameplay of the game; must be in windowed mode, full screen will cause black borders at the moment too so fix that.
            this.graphics.PreferredBackBufferWidth = this.Window.ClientBounds.Width; // The prefered width and height of the game, 800x600.
            this.graphics.PreferredBackBufferHeight = this.Window.ClientBounds.Height;
            this.Window.Title = "Strategy combat"; // Set the title to the name of the game.
            squad = new Unit[4]; // Instance a new array of units. 

            Content.RootDirectory = "Content"; // Content directory for game content, leave this as autogenerated.
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            audioComponent = new AudioComponent(this); // Init the audio part of the game.
            Components.Add(audioComponent); // Add it to the game components.

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
      
            spriteBatch = new SpriteBatch(GraphicsDevice);// Make a new spritebatch..
            Services.AddService(typeof(SpriteBatch), spriteBatch); // Add this to the services.

            // TODO: use this.Content to load your game content here
            gameFont = Content.Load <SpriteFont>("SpriteFont1"); // Load all the textures and fonts.
            menuBackGround = Content.Load<Texture2D>("Menu background"); 
            singlePlayer = Content.Load<Texture2D>("Single Player");
            multiPlayer = Content.Load<Texture2D>("Multi Player");
            options = Content.Load<Texture2D>("Options");
            credits = Content.Load<Texture2D>("Credits");
            windowedModeUnticked = Content.Load<Texture2D>("Windowed Mode Unticked");
            windowedModeTicked = Content.Load<Texture2D>("Windowed Mode ticked");

            back = Content.Load<Texture2D>("Back");
            singleMissions = Content.Load<Texture2D>("Single Player Missions");
            exit = Content.Load<Texture2D>("Exit");
            missionOne = Content.Load<Texture2D>("Mission 1");
            missionOneBackground = Content.Load<Texture2D>("Mission 1 Background");
            unit = Content.Load<Texture2D>("Unit");
            plasmaTurret = Content.Load<Texture2D>("Plasma Turret");
            bullet = Content.Load<Texture2D>("Bullet");
            plasmaProjectile = Content.Load<Texture2D>("Plasma Projectile");
            pointArrowTexture = Content.Load<Texture2D>("Pointing Arrow");
            tutorialMessageFont = Content.Load<SpriteFont>("SpriteFont2");
            actualTutMessageFont = Content.Load<SpriteFont>("SpriteFont3");
            missionTwo = Content.Load<Texture2D>("Mission 2");
            slicebotTexture = Content.Load<Texture2D>("Slice bot");
            airPlaneBomberTexture = Content.Load<Texture2D>("Air Bomber");
            airBombAmmoTexture = Content.Load<Texture2D>("Air Bomb");
            singlePlayerSurvival = Content.Load<Texture2D>("Single player survival");

            currentGamePadState = GamePad.GetState(PlayerIndex.One);
            previousGamePadState = currentGamePadState;

            currentKeyBoardState = Keyboard.GetState();
            previousKeyBoardState = Keyboard.GetState();

            currentMouseState = Mouse.GetState(); // Set up the initial mouse states.
            previousMouseState = currentMouseState;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void checkForOrderInput() // Check for input of orders.
        {

            if ((previousKeyBoardState.IsKeyDown(Keys.T) && currentKeyBoardState.IsKeyUp(Keys.T)) || (previousGamePadState.IsButtonDown(Buttons.DPadUp) && currentGamePadState.IsButtonUp(Buttons.DPadUp))) // If a T or dpad up has been pressed...
            {
                foreach (Unit thisUnit in squad)
                {
                    thisUnit.setOrdersFightingRetreatLeft(); // Order a fighting retreat to the left for each unit.                   
                }
                audioComponent.playCue("Unit order fighting retreat");

            }

            if ((previousKeyBoardState.IsKeyDown(Keys.C) && currentKeyBoardState.IsKeyUp(Keys.C)) || (previousGamePadState.IsButtonDown(Buttons.DPadDown) && currentGamePadState.IsButtonUp(Buttons.DPadDown))) // If a C or dpad down has been pressed...
            {
                foreach (Unit thisUnit in squad)
                {
                    thisUnit.setOrdersRetreatLeft(); // Order a retreat to the left for each unit.
                }
                audioComponent.playCue("Unit order retreat");
            }

            if ((previousKeyBoardState.IsKeyDown(Keys.E) && currentKeyBoardState.IsKeyUp(Keys.E)) || (previousGamePadState.IsButtonDown(Buttons.DPadUp) && currentGamePadState.IsButtonUp(Buttons.DPadUp))) // If a T or dpad up has been pressed...
            {
                foreach (Unit thisUnit in squad)
                {
                    thisUnit.setOrdersFightingRetreatRight(); // Order a fighting retreat to the left for each unit.                   
                }
                audioComponent.playCue("Unit order fighting retreat");

            }

            if ((previousKeyBoardState.IsKeyDown(Keys.R) && currentKeyBoardState.IsKeyUp(Keys.R)) || (previousGamePadState.IsButtonDown(Buttons.DPadDown) && currentGamePadState.IsButtonUp(Buttons.DPadDown))) // If a C or dpad down has been pressed...
            {
                foreach (Unit thisUnit in squad)
                {
                    thisUnit.setOrdersRetreatRight(); // Order a retreat to the left for each unit.
                }
                audioComponent.playCue("Unit order retreat");
            }

            if ((previousKeyBoardState.IsKeyDown(Keys.Y) && currentKeyBoardState.IsKeyUp(Keys.Y)) || (previousGamePadState.IsButtonDown(Buttons.LeftShoulder) && currentGamePadState.IsButtonUp(Buttons.LeftShoulder))) // If a C or dpad down has been pressed...
            {
                foreach (Unit thisUnit in squad)
                {
                    thisUnit.setOrdersDuckAndCover(); // Order each unit to take cover.
                }

                audioComponent.playCue("Unit order duck and cover");
            }
            

        }

        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //MEGA NOTE; NEED TO MAKE SURE THINGS ONLY UPDATE WHEN THEY NEED TO, ADD MORE LOGIC HERE OR GAME WILL RUN POORLY! 

            previousMouseState = currentMouseState; // Set up current and previous controler states.
            currentMouseState = Mouse.GetState();

            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            previousKeyBoardState = currentKeyBoardState;
            currentKeyBoardState = Keyboard.GetState();
   
            checkForGameExit();

            checkForUnitChanged(); // Check if the player has selected another unit.

            // Init the buttons.
            if (buttonSingle == null && buttonOptions == null && buttonBack == null
                && buttonSinglePlayerMissions == null && buttonExit == null && buttonMissOne == null) // If none of the buttons are instanced...
            {
                initButtons(); // Make it so.
            }

            updateButtons(gameTime); // Update the buttons.            

            handleButtonEventsAndPresses(); // Handle button events and presses.
            

            if (missionState == MissionState.missionOne) // Only handle states for missions when it is that mission.
            {
                checkForOrderInput(); // Check to see if any orders have been given to the squad.
                doMissionOneStateHandle(gameTime); // Handle state changes for the first mission.
                handleGlobalWeapons(); // Make sure to do this after doing state handles otherwise global weapons won't work.
            }
            else if (missionState == MissionState.missionTwo)
            {
                checkForOrderInput(); // Check to see if any orders have been given to the squad.
                doMissionTwoStateHandle(gameTime); // And the second mission.
                handleGlobalWeapons(); // Make sure to do this after doing state handles otherwise global weapons won't work.
            }
            else if (missionState == MissionState.singlePlayerSurvival)
            {
                checkForOrderInput(); // Check to see if any orders have been given to the squad.
                doSinglePlayerSurvivalStateHandle(gameTime); // As well as the survival mode.
                handleGlobalWeapons(); // Make sure to do this after doing state handles otherwise global weapons won't work.
            }

            if (gameInPlay == true) // Only do game logic if the game is in play.
            {
                doGameLogic(); // Do any logic required for the game. (Mostly collosions)
            }

            

            // Removed checkMissxWinLoss from here for now.

            //base.Update(gameTime); Left out in order to make sure only the selected unit updates, not the others, put back in if needed.
        }

        private void handleGlobalWeapons() // Handle weapons with a 'global cooldown'
        {
            // One click weapons.
            if ((currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released) || (currentGamePadState.IsButtonDown(Buttons.RightTrigger) && previousGamePadState.IsButtonUp(Buttons.RightTrigger))) //Left mouse or right trigger to fire.
            {
                foreach (Unit thisUnit in squad)
                {
                    if (thisUnit.getUnitControled() == true && (thisUnit.getUnitStateString() == "holdingAirStrikePointerProne" || thisUnit.getUnitStateString() == "holdingAirStrikePointer")) // If this unit is being controlled and they have the airstrike pointer equipped...
                    {
                        squad[0].airStrikeHasBeenUsed();
                        squad[1].airStrikeHasBeenUsed();
                        squad[2].airStrikeHasBeenUsed();
                        squad[3].airStrikeHasBeenUsed();
                    }
                }
            }

        }

        private void doSinglePlayerSurvivalStateHandle(GameTime gameTime) // Do the handling of game states for the single player survival gamemode.
        {
            if (gameState == GameState.singlePlayerSurvivalWelcome) // Wait for the player to start this gamemode.
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.singlePlayerSurvivalHint;
                }

            }
            else if (gameState == GameState.singlePlayerSurvivalHint)
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.singlePlayerSurvivalPlayStageOne;
                }

            }
            else if (gameState == GameState.singlePlayerSurvivalPlayStageOne)
            {
                gameInPlay = true;

                handleSinglePlayerSurvivalPlayStageOneEvents(gameTime); // Handle all events to do with the single player survival.
            }
            else if (gameState == GameState.singlePlayerSurvivalPlayerLost)
            {
                gameInPlay = false;

                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A))) // For now check for inputs of A and Enter.
                {
                    gameState = GameState.mainMenu; // For now return to the main menu.
                    missionState = MissionState.noMission; // Set the mission state to no mission.
                    cleanUpSinglePlayerSurvival(); // Clean up.
                    enableAllButtons(); // Enable all the buttons again.
                }
            }
        }

        private void handleSinglePlayerSurvivalPlayStageOneEvents(GameTime gameTime)
        {

            survivalElaspedTime++; // Increase the time the player has survived for.
            sliceBotAddCounter++; // As well as other counters.
            plasmaTurretAddCounter++;

            if (squad != null)
            {
                if (squad[0].getUnitActive() == false // If all the members of the squad are inactive... (I.e. dead)
                && squad[1].getUnitActive() == false
                && squad[2].getUnitActive() == false
                && squad[3].getUnitActive() == false)
                {
                    gameState = GameState.singlePlayerSurvivalPlayerLost; // Set the state to the loss state.
                    audioComponent.playCue("Mission failed");
                }
            }

            foreach (Unit thisUnit in squad) // Update the squad.
            {
                if (thisUnit != null)
                {
                    thisUnit.Update(gameTime);
                }

            }

            if (squadOneArrow != null) // Update the unit selected arrow if it is instanced.
            {
                squadOneArrow.Update(gameTime);
                if (squad[selectedUnit].getUnitStateString() != "destroyed") // Makes it so the arrow will only be updated if the selected unit is not destroyed.
                {
                    squadOneArrow.updateArrrowPos(squad[selectedUnit].getPos()); // Update the arrow's position 
                }
            }

            try
            {
                foreach (GameComponent gc in Components) // Update each game component that is not a unit.
                {
                    if (gc is PlasmaTurret) // If its a bullet
                    {
                        PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc); // Get this local plasma turret.

                        if (thisPlasmaTurret != null)
                        {
                            if (thisPlasmaTurret.getTurretActive() == true && thisPlasmaTurret.getIsMissOneTurr() == false)
                            {
                                thisPlasmaTurret.Update(gameTime);
                            }

                        }
                    }
                    else if (gc is Slicebot)
                    {
                        Slicebot thisSliceBot = ((Slicebot)gc); // Get this local slicebot.

                        if (thisSliceBot != null)
                        {
                            if (thisSliceBot.getSlicebotActive() == true && thisSliceBot.getIsMissTwoSlicebot() == false)
                            {
                                thisSliceBot.Update(gameTime);
                            }

                        }
                    }
                }

            }
            catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
            { }

            if (sliceBotAddCounter >= timeTillNextSliceBot) // If it is time to add a new slicebot...
            {
                Components.Add(new Slicebot(this, new Vector2((float)generateRandomNumber(700,50),305), ref slicebotTexture, squad,false)); // Add one at a random location.
                sliceBotAddCounter = 0; // Reset the counter

                if (timeTillNextSliceBot > 1000) // Only allow slicebots to be spawned at a max rate of 1 every 10 seconds.
                {
                    timeTillNextSliceBot -= 100; // Lower the time needed to add a new one.
                }
                
                audioComponent.playCue("Unit callout enemy mech");
            }

            if (plasmaTurretAddCounter >= timeTillNextPlasmaTurret) // If it is time to add a new plasma turret...
            {
                Components.Add(new PlasmaTurret(this, new Vector2((float)generateRandomNumber(700, 50), 231), ref plasmaTurret, ref plasmaProjectile,false)); // Do so at a random location.
                plasmaTurretAddCounter = 0; // Reset the counter.

                if (timeTillNextPlasmaTurret > 500) // Only allow plasma turrets be to spawned at a max rate of 1 every 5 seconds.
                {
                    timeTillNextPlasmaTurret -= 100; // Lower the time needed to add a new one.
                }
                
                audioComponent.playCue("Unit callout enemy mech");
            }
           
            
            // Add in code to update rest of objects + counter vars.

        }

        private void handleButtonEventsAndPresses() // Handle all things associated with buttons and changes of state with them.
        {
            // Check the state of the game, in particular to button presses.
            if (gameState == GameState.mainMenu)
            {
                if (buttonOptions.isLeftClicked())
                {
                    gameState = GameState.options;
                }
                else if (buttonCredits.isLeftClicked())
                {
                    gameState = GameState.credits;
                }
                else if (buttonSingle.isLeftClicked())
                {
                    gameState = GameState.singlePlayerMenu;
                }
                else if (buttonExit.isLeftClicked())
                {
                    Exit();
                }
            }
            else if (gameState == GameState.singlePlayerMenu)
            {
                if (buttonSinglePlayerMissions.isLeftClicked())
                {
                    gameState = GameState.singlePlayerMissions;

                }
                else if (buttonSinglePlayerSurvival.isLeftClicked())
                {
                    cleanUpSinglePlayerSurvival(); // Clean up the single player survival mission.
                    setUpSinglePlayerSurvival(); // Set up this mission.
                    gameState = GameState.singlePlayerSurvivalWelcome; // Go to the welcome phase of the single player survival mission.
                    missionState = MissionState.singlePlayerSurvival;
                    disableAllButtons(); // Disable all the buttons.                                     
                }
                else if (buttonBack.isLeftClicked())
                {
                    gameState = GameState.mainMenu;

                }
            }
            else if (gameState == GameState.singlePlayerMissions)
            {
                if (buttonBack.isLeftClicked())
                {
                    gameState = GameState.singlePlayerMenu;
                }
                else if (buttonMissOne.isLeftClicked())
                {
                    cleanUpLevelOne(); // Clean up the first mission. 
                    setUpMissionOne(); // Init the mission.
                    gameState = GameState.missionOneWelcome; // Go to the welcome phase of the first mission.
                    missionState = MissionState.missionOne;
                    disableAllButtons(); // Disable all the buttons.                                     
                }
                else if (buttonMissTwo.isLeftClicked())
                {
                    cleanUpLevelTwo(); // Clean up the first mission. 
                    setUpMissionTwo(); // Init the mission.
                    gameState = GameState.missionTwoWelcome; // Go to the welcome phase of the first mission.
                    missionState = MissionState.missionTwo;
                    disableAllButtons(); // Disable all the buttons.  

                }
            }
            else if (gameState == GameState.options) // Note that the option buttons require captions on what they do such as the windowed mode tick box.
            {
                if (buttonWindowedMode.isLeftClicked()) // Do what is required of this option button.
                {
                    this.graphics.ToggleFullScreen(); // Toggle between fullscreen and windowed mode at will.
                }
                else if (buttonBack.isLeftClicked())
                {
                    gameState = GameState.mainMenu;
                }
            }
            else // For all other states. (Such as the unimplemented multiplayer mode and the credits)
            {
                if (buttonBack.isLeftClicked())
                {
                    gameState = GameState.mainMenu;
                }

            }

        }

        private void updateButtons(GameTime gameTime)
        {
            if (buttonSingle != null) // If one of the buttons is not null they all are so..
            {
                buttonSingle.Update(gameTime); // Update them all.
                buttonOptions.Update(gameTime);
                buttonBack.Update(gameTime);
                buttonSinglePlayerMissions.Update(gameTime);
                buttonExit.Update(gameTime);
                buttonMissOne.Update(gameTime);
                buttonMissTwo.Update(gameTime);
                buttonCredits.Update(gameTime);
                buttonWindowedMode.Update(gameTime);
                buttonSinglePlayerSurvival.Update(gameTime);
            }

        }

        private void cleanUpSinglePlayerSurvival() // Expand to clean up more things as needed, may need to also clean up turrets and slicebots.
        { // Could try cleaning up these things by using bedamaged on them and setting thier health to 0;
            try
            {
                foreach (GameComponent gc in Components) // Check each game component.
                {
                    if (gc is Bullet) // If its a bullet
                    {
                        Bullet thisBullet = ((Bullet)gc); // A local bullet.

                        thisBullet.destroyBullet(); // Destroy this bullet.

                        //Components.Remove(thisBullet); // Remove this bullet from the game components.
                    }
                    else if (gc is AirPlaneBomber) // Or a bomber..
                    {
                        AirPlaneBomber thisBomber = ((AirPlaneBomber)gc);

                        thisBomber.destroyBomber();
                    }
                    else if (gc is AirPlaneBomb) // Or a bomb dropped by a bomber..
                    {
                        AirPlaneBomb thisAirBomb = ((AirPlaneBomb)gc);

                        thisAirBomb.destroyBomb();
                    }

                }
            }
            catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
            { }

        }

        private void checkForGameExit() // Check to see if the player is trying to exit the game.
        {

            if ((previousKeyBoardState.IsKeyDown(Keys.Escape) && currentKeyBoardState.IsKeyUp(Keys.Escape) )|| GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) // If esc is pressed then depressed.. or the back button is pressed..
            {
                Exit(); // exit the game.
            }
        }

        private void checkForUnitChanged() // Check to see if the player has selected another unit.
        {
           
            // De selecting units.
            if ((previousMouseState.RightButton == ButtonState.Pressed && currentMouseState.RightButton == ButtonState.Released) || (previousGamePadState.IsButtonDown(Buttons.B) && currentGamePadState.IsButtonUp(Buttons.B))) // If the right mouse or B button has been pressed...
            { // Now works, but be careful.
                if (squad[selectedUnit] != null)
                {
                    squad[selectedUnit].setUnitControled(false); // Unselect the currently selected unit to select no units.
                    noUnitSelected = true;
                }
            }

            // There used to be a correposonding bit here for the unit, but the unit's know whether they can do things by themselves.
            // Could do this for the turret as well.
            // Alternative way of selecting units.

            if (missionState == MissionState.missionOne || missionState == MissionState.missionTwo || missionState == MissionState.singlePlayerSurvival) // Only check for unit changes if the game is in a mission state. (Make sure to do this or it makes it seem that you cannot change units!)
            {
                if (squad[0] != null) // If one of the squad members has been instanced, the others have as well so...
                {
                    if (squad[0].isLeftClicked() == true && squad[0].getUnitActive() == true) // check whether each unit is active and has been left clicked.
                    {
                        noUnitSelected = false;
                        squad[selectedUnit].setUnitControled(false); // Then loose control of the currently selected unit.
                        selectedUnit = 0; // set the selected unit to this one.
                        squad[selectedUnit].setUnitControled(true); // set this unit to be controlled now.      
                        playRandomUnitSelectedSound();
                    }
                    else if (squad[1].isLeftClicked() == true && squad[1].getUnitActive() == true)
                    {
                        noUnitSelected = false;
                        squad[selectedUnit].setUnitControled(false);
                        selectedUnit = 1;
                        squad[selectedUnit].setUnitControled(true);
                        playRandomUnitSelectedSound();
                    }
                    else if (squad[2].isLeftClicked() == true && squad[2].getUnitActive() == true)
                    {
                        noUnitSelected = false;
                        squad[selectedUnit].setUnitControled(false);
                        selectedUnit = 2;
                        squad[selectedUnit].setUnitControled(true);
                        playRandomUnitSelectedSound();
                    }
                    else if (squad[3].isLeftClicked() == true && squad[3].getUnitActive() == true)
                    {
                        noUnitSelected = false;
                        squad[selectedUnit].setUnitControled(false);
                        selectedUnit = 3;
                        squad[selectedUnit].setUnitControled(true);
                        playRandomUnitSelectedSound();
                    }

                    
                }

                if (previousGamePadState.IsButtonDown(Buttons.Y) && currentGamePadState.IsButtonUp(Buttons.Y))// Cycle through the units, need to change this too to make sure you can very quickly cycle through the units. 
                { // Now only works for gamepad as the player can otherwise just select a unit by left clicking on it with the mouse.
                    squad[selectedUnit].setUnitControled(false); // Remove control of this one.

                    selectedUnit++; // Increment the selected unit.


                    if (selectedUnit > 3) // If it gets to the end, set it back to zero again.
                    {
                        selectedUnit = 0;
                    }

                    squad[selectedUnit].setUnitControled(true); // If all is good after this set control to this unit.
                    noUnitSelected = false;

                    playRandomUnitSelectedSound();
                }

            }

            
        }

        private double generateRandomNumber(int uppper, int lower)
        {
            Random randomGen = new Random();

            double randomNum = (randomGen.NextDouble() * (uppper - lower + 1)) + lower; 
            
            return randomNum;

        }

        private void playRandomMissionWinSound()
        {
            double selectSoundDecider = generateRandomNumber(100, 0);

            if (selectSoundDecider <= 100 && selectSoundDecider > 50)
            {
                audioComponent.playCue("Unit callout mission end one");
            }
            else if (selectSoundDecider <= 50 && selectSoundDecider > 0)
            {
                audioComponent.playCue("Unit callout mission end two");
            }

        }

        private void playRandomUnitSelectedSound()
        {
            double selectSoundDecider = generateRandomNumber(150, 0);

            if (selectSoundDecider <= 150 && selectSoundDecider > 101)
            {
                audioComponent.playCue("Unit selected callouts 1");
            }
            else if (selectSoundDecider <= 100 && selectSoundDecider > 51)
            {
                audioComponent.playCue("Unit selected callouts 2");
            }
            else if (selectSoundDecider <= 50 && selectSoundDecider > 0)
            {
                audioComponent.playCue("Unit selected callouts 3");
            }
            
        }

        private void playRandomUnitCalloutEnemyMechDestroyedSound()
        {
            double selectSoundDecider = generateRandomNumber(100, 0);

            if (selectSoundDecider <= 100 && selectSoundDecider > 50)
            {
                audioComponent.playCue("Unit callout enemy mech destroy one");
            }
            else if (selectSoundDecider <= 50 && selectSoundDecider > 0)
            {
                audioComponent.playCue("Unit callout enemy mech destroy two");
            }
        }

        private void initButtons()
        {
            Vector2 singlePlayerVector = new Vector2(300, 100); // Where the single player button is to be on the screen.
            Vector2 creditsVector = new Vector2(300, 250); // Where the multi player button is to be on the screen.
            Vector2 optionsVector = new Vector2(300, 400); // Where the options button is to be on the screen.
            Vector2 backVector = new Vector2(550, 100); // Where the back button is to be on the screen.
            Vector2 singlePlayerMissionsVector = new Vector2(300, 100); // Where the single player missions button is to be on the screen.
            Vector2 exitVector = new Vector2(580, 400); // Where the exit button is to be on the screen.
            Vector2 missionOneVector = new Vector2(300, 100); // Where the first mission button is to be on the screen.
            Vector2 windowedModeVector = new Vector2(100, 100); // Where the windowed mode option button is to be on the screen.
            Vector2 missionTwoVector = new Vector2(300, 250); // Where the mission two button is to be on the screen.
            Vector2 singlePlayerSurvivalVector = new Vector2(300, 250); // Where the single player Survival button is to be on the screen.

            buttonSingle = new Button(this, singlePlayerVector, ref singlePlayer, 200, 100); // instance the buttons. with the game, vector texture size and width.
            buttonOptions = new Button(this, optionsVector, ref options, 200, 100);
            buttonBack = new Button(this, backVector, ref back, 200, 100);
            buttonSinglePlayerMissions = new Button(this, singlePlayerMissionsVector, ref singleMissions, 200, 100);
            buttonExit = new Button(this, exitVector, ref exit, 200, 100);
            buttonMissOne = new Button(this, missionOneVector, ref missionOne, 200, 100);
            buttonMissTwo = new Button(this, missionTwoVector, ref missionTwo, 200, 100);
            buttonWindowedMode = new Button(this, windowedModeVector, ref windowedModeUnticked, ref windowedModeTicked, 63, 53); // Note that tick buttons use a different contructor.
            buttonSinglePlayerSurvival = new Button(this, singlePlayerSurvivalVector, ref singlePlayerSurvival, 200, 100);
            buttonCredits = new Button(this, creditsVector, ref credits, 200, 100);

            Components.Add(buttonSingle); // Add them to the components.
            Components.Add(buttonOptions);
            Components.Add(buttonBack);
            Components.Add(buttonSinglePlayerMissions);
            Components.Add(buttonExit);
            Components.Add(buttonMissOne);
            Components.Add(buttonMissTwo);
            Components.Add(buttonWindowedMode);
            Components.Add(buttonCredits);
        }

        private void doMissionOneStateHandle(GameTime gameTime) // Handle states and updating in mission one.
        {
            if (gameState == GameState.missionOneWelcome) // Check to see what state the game is in for the first mission, update tutorial messages as needed and such. (They go off the edge of the screen, fix that)
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionOneMovementControls;
                }

            }
            else if (gameState == GameState.missionOneMovementControls) // Else ifs are required or states leak into the next ones causing it to skip to starting the mission right off after pressing enter or A at the welcome sequence.
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionOneCombatControls;
                }

            }
            else if (gameState == GameState.missionOneCombatControls)
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionOneUnitSelectionControls;
                }
            }
            else if (gameState == GameState.missionOneUnitSelectionControls)
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionOneMakeReady;
                }
            }
            else if (gameState == GameState.missionOneMakeReady)
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    audioComponent.playCue("Unit callout enemy mech"); // Play the audio cue for spotting an enemy mech.
                    gameState = GameState.missionOnePlayStageOne;                 
                }
            }
            else if (gameState == GameState.missionOnePlayStageOne) // Only update the game if the game is in play; means that if the player wins or looses, the game will no longer update.
            {
                gameInPlay = true;

                //audioComponent.playCue("Unit callout enemy mech"); // Play the audio cue for spotting an enemy mech.

                if (squad[0].getUnitActive() == false // If all the members of the squad are dead...
                && squad[1].getUnitActive() == false
                && squad[2].getUnitActive() == false
                && squad[3].getUnitActive() == false)
                {
                    audioComponent.playCue("Mission failed");
                    gameState = GameState.missionOnePlayerLost; // Set the state to the loss state.
                }
                else // Otherwise....
                {
                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is PlasmaTurret) // This code is not run when the plasma turret is destroyed.
                            {
                                PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc);

                                if (thisPlasmaTurret.getIsMissOneTurr() == true && thisPlasmaTurret.getTurretActive() == true) // Only update the turret that is of the first mission.
                                {
                                    thisPlasmaTurret.Update(gameTime);
                                }

                            }

                            if (missOnePlasmaTurretHealth == 0) // The turret has been destroyed.
                            {
                                playRandomUnitCalloutEnemyMechDestroyedSound(); // Play a random enemy mech destroyed sound.
                                gameState = GameState.missionOnePlayerWon; // Set the state to the win state.
                                playRandomMissionWinSound();
                            }                         
                        }

                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }                    
                }

                foreach (Unit thisUnit in squad)
                {
                    if (thisUnit != null)
                    {
                        thisUnit.Update(gameTime);
                    }

                }

                if (squadOneArrow != null) // Update the unit selected arrow if it is instanced.
                {
                    squadOneArrow.Update(gameTime);
                    if (squad[selectedUnit].getUnitStateString() != "destroyed") // Makes it so the arrow will only be updated if the selected unit is not destroyed.
                    {
                        squadOneArrow.updateArrrowPos(squad[selectedUnit].getPos()); // Update the arrow's position 
                    }
                }

                
            }
            else if (gameState == GameState.missionOnePlayerLost) // If the player has won or lost.
            {
                gameInPlay = false;

                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A))) // For now check for inputs of A and Enter.
                {
                    gameState = GameState.mainMenu; // For now return to the main menu.
                    missionState = MissionState.noMission;
                    cleanUpLevelOne(); // Clean up.
                    enableAllButtons(); // Enable all the buttons again.
                }
            }
            else if (gameState == GameState.missionOnePlayerWon)
            {
                gameInPlay = false;

                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A))) // For now check for inputs of A and Enter.
                {
                    gameState = GameState.mainMenu; // For now return to the main menu.
                    missionState = MissionState.noMission;
                    cleanUpLevelOne(); // Clean up.
                    enableAllButtons(); // Enable all the buttons again.
                }

            }
        }

        private void doMissionTwoStateHandle(GameTime gameTime)
        {
            if (gameState == GameState.missionTwoWelcome) // Check to see what state the game is in for the first mission, update tutorial messages as needed and such. (They go off the edge of the screen, fix that)
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionTwoAlert; 
                }
            }
            else if (gameState == GameState.missionTwoAlert)
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    audioComponent.playCue("Unit callout enemy mech"); // Play the audio cue for spotting an enemy mech.
                    gameState = GameState.missionTwoPlayStageOne;
                    spawnSliceBot();
                }
            }
            else if (gameState == GameState.missionTwoPlayStageOne) // Do what is required of playing this mission.
            {
                gameInPlay = true;             

                if (squad != null) // Crashes here if does not check that slicebot is not null.
                {
                    if (squad[0].getUnitActive() == false // If all the members of the squad are inactive... (I.e. dead)
                    && squad[1].getUnitActive() == false
                    && squad[2].getUnitActive() == false
                    && squad[3].getUnitActive() == false)
                    {
                        audioComponent.playCue("Mission failed");
                        gameState = GameState.missionTwoPlayerLost; // Set the state to the loss state.
                    }
                    else // Otherwise....
                    {
                        try
                        {
                            foreach (GameComponent gc in Components)
                            {
                                if (gc is Slicebot) // May need to check whether it is the mission two slicebot or not, same for plasma turret.
                                {
                                    Slicebot thisSlicebot = ((Slicebot)gc);

                                    if (thisSlicebot.getIsMissTwoSlicebot() == true && thisSlicebot.getSlicebotActive() == true)
                                    {
                                        thisSlicebot.Update(gameTime);
                                    }
                                   
                                }

                                if (missTwoSlicebotHealth == 0)
                                {
                                    playRandomUnitCalloutEnemyMechDestroyedSound();

                                    gameState = GameState.missionTwoPlayerWon; // Set the state to the win state.
                                    playRandomMissionWinSound();
                                }
                            }
                        }
                        catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                        { }                   
                    }

                }


                
                foreach (Unit thisUnit in squad)
                {
                    if (thisUnit != null)
                    {
                        thisUnit.Update(gameTime);
                    }

                }

                if (squadOneArrow != null) // Update the unit selected arrow if it is instanced.
                {
                    squadOneArrow.Update(gameTime);
                    if (squad[selectedUnit].getUnitStateString() != "destroyed") // Makes it so the arrow will only be updated if the selected unit is not destroyed.
                    {
                        squadOneArrow.updateArrrowPos(squad[selectedUnit].getPos()); // Update the arrow's position 
                    }
                }

                if (squad != null) // If the squad has been instanced...
                {
                    Vector2 leadingUnitVector = new Vector2();
                    leadingUnitVector = squad[3].getPos();

                    if (leadingUnitVector.X > 300) // If the unit has moved past a certain point.
                    {
                        gameState = GameState.missionTwoWarning; // Warn the player about the danger..
                    }                    
                }
            }
            else if (gameState == GameState.missionTwoWarning)
            {
                gameInPlay = false;

                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionTwoHelpBackPedal;
                }

            }
            else if (gameState == GameState.missionTwoHelpBackPedal)
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionTwoHelpOrders;
                }

            }
            else if (gameState == GameState.missionTwoHelpOrders)
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionTwoGetReady;
                }

            }
            else if (gameState == GameState.missionTwoGetReady)
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionTwoPlayStageTwo;
                }

            }
            else if (gameState == GameState.missionTwoPlayStageTwo)
            {
                gameInPlay = true;

                if (squad != null) // Crashes here if does not check that slicebot is not null.
                {
                    if (squad[0].getUnitActive() == false // If all the members of the squad are inactive... (I.e. dead)
                    && squad[1].getUnitActive() == false
                    && squad[2].getUnitActive() == false
                    && squad[3].getUnitActive() == false)
                    {
                        audioComponent.playCue("Mission failed");
                        gameState = GameState.missionTwoPlayerLost; // Set the state to the loss state.
                    }
                    else // Otherwise....
                    {
                        try
                        {
                            foreach (GameComponent gc in Components)
                            {
                                if (gc is Slicebot) // May need to check whether it is the mission two slicebot or not, same for plasma turret.
                                {
                                    Slicebot thisSlicebot = ((Slicebot)gc);

                                    if (thisSlicebot.getIsMissTwoSlicebot() == true && thisSlicebot.getSlicebotActive() == true)
                                    {
                                        thisSlicebot.Update(gameTime);
                                    }
                                }

                                if (missTwoSlicebotHealth == 0)
                                {
                                    playRandomUnitCalloutEnemyMechDestroyedSound();

                                    gameState = GameState.missionTwoPlayerWon; // Set the state to the win state.
                                    playRandomMissionWinSound();
                                }
                            }
                        }
                        catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                        { }
                    }
                }
                

                foreach (Unit thisUnit in squad)
                {
                    if (thisUnit != null)
                    {
                        thisUnit.Update(gameTime);
                    }

                }

                if (squadOneArrow != null) // Update the unit selected arrow if it is instanced.
                {
                    squadOneArrow.Update(gameTime);
                    if (squad[selectedUnit].getUnitStateString() != "destroyed") // Makes it so the arrow will only be updated if the selected unit is not destroyed.
                    {
                        squadOneArrow.updateArrrowPos(squad[selectedUnit].getPos()); // Update the arrow's position 
                    }
                }

                if (squad != null) // If the squad has been instanced...
                {
                    foreach (Unit thisUnit in squad) // Check if any of the squad members have retreated back far enough.
                    {
                        Vector2 leadingUnitVector = new Vector2();
                        leadingUnitVector = thisUnit.getPos();

                        if (leadingUnitVector.X < 100) // If the unit has moved back past a certain point...
                        {
                            gameState = GameState.missionTwoHelpRetreatOrder; // Congratulate the player and move on.
                        }   
                    }                                 
                }
            }
            else if (gameState == GameState.missionTwoHelpRetreatOrder)
            {
                gameInPlay = false;

                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionTwoHelpSelectNoUnits;
                }
            }
            else if (gameState == GameState.missionTwoHelpSelectNoUnits)
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionTwoPlayStageThree;
                }
            }
            else if (gameState == GameState.missionTwoPlayStageThree) // Third stage of play, need to check here that the player has done as asked.
            {
                gameInPlay = true;

                if (squad != null) // Crashes here if does not check that slicebot is not null.
                {
                    if (squad[0].getUnitActive() == false // If all the members of the squad are inactive... (I.e. dead)
                    && squad[1].getUnitActive() == false
                    && squad[2].getUnitActive() == false
                    && squad[3].getUnitActive() == false)
                    {
                        audioComponent.playCue("Mission failed");
                        gameState = GameState.missionTwoPlayerLost; // Set the state to the loss state.
                    }
                    else // Otherwise....
                    {
                        try
                        {
                            foreach (GameComponent gc in Components)
                            {
                                if (gc is Slicebot) // May need to check whether it is the mission two slicebot or not, same for plasma turret.
                                {
                                    Slicebot thisSlicebot = ((Slicebot)gc);

                                    if (thisSlicebot.getIsMissTwoSlicebot() == true && thisSlicebot.getSlicebotActive() == true)
                                    {
                                        thisSlicebot.Update(gameTime);
                                    }
                                }

                                if (missTwoSlicebotHealth == 0)
                                {
                                    playRandomUnitCalloutEnemyMechDestroyedSound();

                                    gameState = GameState.missionTwoPlayerWon; // Set the state to the win state.
                                    playRandomMissionWinSound();
                                }
                            }
                        }
                        catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                        { }
                    }

                }

                foreach (Unit thisUnit in squad)
                {
                    if (thisUnit != null)
                    {
                        thisUnit.Update(gameTime);
                    }

                }

                if (squadOneArrow != null) // Update the unit selected arrow if it is instanced.
                {
                    squadOneArrow.Update(gameTime);
                    if (squad[selectedUnit].getUnitStateString() != "destroyed") // Makes it so the arrow will only be updated if the selected unit is not destroyed.
                    {
                        squadOneArrow.updateArrrowPos(squad[selectedUnit].getPos()); // Update the arrow's position 
                    }
                }


                if (noUnitSelected == true && squad[selectedUnit].getOrderStateString() == "retreatLeft") // Check that there are no units selected and the squad has been given the order to retreat left..
                {
                    gameState = GameState.missionTwoWellDoneStageThree; // TODO: Check the state of the game for the new states such as this one.
                }
            }
            else if (gameState == GameState.missionTwoWellDoneStageThree)
            {
                gameInPlay = false;

                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionTwoHelpAirStrike;
                }
            }
            else if (gameState == GameState.missionTwoHelpAirStrike)
            {
                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A)))
                {
                    gameState = GameState.missionTwoPlayStageFour;
                }
            }
            else if (gameState == GameState.missionTwoPlayStageFour) //4th and final stage of play, now the player must destroy the slicebot, preferably with an airstrike.
            {
                gameInPlay = true;

                if (squad != null) // Crashes here if does not check that slicebot is not null.
                {
                    if (squad[0].getUnitActive() == false // If all the members of the squad are inactive... (I.e. dead)
                    && squad[1].getUnitActive() == false
                    && squad[2].getUnitActive() == false
                    && squad[3].getUnitActive() == false)
                    {
                        audioComponent.playCue("Mission failed");
                        gameState = GameState.missionTwoPlayerLost; // Set the state to the loss state.
                    }
                    else // Otherwise....
                    {
                        try
                        {
                            foreach (GameComponent gc in Components)
                            {
                                if (gc is Slicebot) // May need to check whether it is the mission two slicebot or not, same for plasma turret.
                                {
                                    Slicebot thisSlicebot = ((Slicebot)gc);

                                    if (thisSlicebot.getIsMissTwoSlicebot() == true && thisSlicebot.getSlicebotActive() == true)
                                    {
                                        thisSlicebot.Update(gameTime);
                                    }
                                }

                                if (missTwoSlicebotHealth == 0)
                                {
                                    playRandomUnitCalloutEnemyMechDestroyedSound();

                                    gameState = GameState.missionTwoPlayerWon; // Set the state to the win state.
                                    playRandomMissionWinSound();
                                }
                            }
                        }
                        catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                        { }
                    }

                }

                foreach (Unit thisUnit in squad)
                {
                    if (thisUnit != null)
                    {
                        thisUnit.Update(gameTime);
                    }

                }

                if (squadOneArrow != null) // Update the unit selected arrow if it is instanced.
                {
                    squadOneArrow.Update(gameTime);
                    if (squad[selectedUnit].getUnitStateString() != "destroyed") // Makes it so the arrow will only be updated if the selected unit is not destroyed.
                    {
                        squadOneArrow.updateArrrowPos(squad[selectedUnit].getPos()); // Update the arrow's position 
                    }
                }

            }
            else if (gameState == GameState.missionTwoPlayerLost) // If the player has won or lost.
            {
                gameInPlay = false;

                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A))) // For now check for inputs of A and Enter.
                {
                    gameState = GameState.mainMenu; // For now return to the main menu.
                    missionState = MissionState.noMission; // No mission active now.
                    cleanUpLevelTwo(); // Clean up.
                    enableAllButtons(); // Enable all the buttons again.
                }
            }
            else if (gameState == GameState.missionTwoPlayerWon)
            {
                gameInPlay = false;

                if ((previousKeyBoardState.IsKeyDown(Keys.Enter) && currentKeyBoardState.IsKeyUp(Keys.Enter)) || (previousGamePadState.IsButtonDown(Buttons.A) && currentGamePadState.IsButtonUp(Buttons.A))) // For now check for inputs of A and Enter.
                {
                    gameState = GameState.mainMenu; // For now return to the main menu.
                    missionState = MissionState.noMission; // No mission active now.
                    cleanUpLevelTwo(); // Clean up.
                    enableAllButtons(); // Enable all the buttons again.
                }

            }
        }

        private void spawnSliceBot() // Spawn a slicebot at a set location, could make this more dynamic.
        {
            Components.Add(new Slicebot(this, new Vector2(700, 305), ref slicebotTexture, squad,true)); // 303 seems to be ground level for the slice bot.

            try
            {
                foreach (GameComponent gc in Components)
                {
                    if (gc is Slicebot) // This code is not run when the plasma turret is destroyed.
                    {
                        Slicebot thisSlicebot = ((Slicebot)gc);

                        if (thisSlicebot.getIsMissTwoSlicebot() == true)
                        {
                            missTwoSlicebotHealth = thisSlicebot.getHealth();
                        }
                    }
                }

            }
            catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
            { } 
        }

        private void cleanUpLevelTwo() // Clean up the second level.
        {
            try
            {
                foreach (GameComponent gc in Components) // Check each game component.
                {
                    if (gc is Bullet) // If its a bullet
                    {
                        Bullet thisBullet = ((Bullet)gc); // A local bullet.

                        thisBullet.destroyBullet(); // Destroy this bullet.

                        //Components.Remove(thisBullet); // Remove this bullet from the game components.
                    }
                    else if (gc is AirPlaneBomber) // Or a bomber..
                    {
                        AirPlaneBomber thisBomber = ((AirPlaneBomber)gc);

                        thisBomber.destroyBomber();
                    }
                    else if (gc is AirPlaneBomb) // Or a bomb dropped by a bomber..
                    {
                        AirPlaneBomb thisAirBomb = ((AirPlaneBomb)gc);

                        thisAirBomb.destroyBomb();
                    }

                }
            }
            catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
            {}  
        }

        private void setUpMissionTwo() // Set up the second level.
        {
            const int GROUND_LEVEL_UNIT = 263; // The effective ground of the first level (and second level) for units.
           
            // set up each squad memeber in turn with their init pos etc.

            squad[0] = new Unit(this, new Vector2(0, GROUND_LEVEL_UNIT),ref bullet, ref airBombAmmoTexture, ref airPlaneBomberTexture, 0);
            squad[1] = new Unit(this, new Vector2(60, GROUND_LEVEL_UNIT), ref bullet, ref airBombAmmoTexture, ref airPlaneBomberTexture, 1);
            squad[2] = new Unit(this, new Vector2(120, GROUND_LEVEL_UNIT), ref bullet, ref airBombAmmoTexture, ref airPlaneBomberTexture, 2);
            squad[3] = new Unit(this, new Vector2(180, GROUND_LEVEL_UNIT), ref bullet, ref airBombAmmoTexture, ref airPlaneBomberTexture, 3);

           
            squad[selectedUnit].setUnitControled(true); // Set control of the default selected unit in the squad.

            //Set up the arrow to point to the selected squad member.

            squadOneArrow = new PointingArrow(this, ref pointArrowTexture, squad[selectedUnit].getPos());

            // Add them all to the game components.
            Components.Add(squad[0]);
            Components.Add(squad[1]);
            Components.Add(squad[2]);
            Components.Add(squad[3]);          
            Components.Add(squadOneArrow);
        }

        private void cleanUpLevelOne() // Clean up level one.
        {
            try
            {
                foreach (GameComponent gc in Components) // Check each game component.
                {
                    if (gc is Bullet) // If its a bullet
                    {
                        Bullet thisBullet = ((Bullet)gc); // A local bullet.

                        thisBullet.destroyBullet(); // Destroy this bullet.

                        //Components.Remove(thisBullet); // Remove this bullet from the game components.
                    }
                    else if (gc is AirPlaneBomber) // Or a bomber..
                    {
                        AirPlaneBomber thisBomber = ((AirPlaneBomber)gc);

                        thisBomber.destroyBomber();
                    }
                    else if (gc is AirPlaneBomb) // Or a bomb dropped by a bomber..
                    {
                        AirPlaneBomb thisAirBomb = ((AirPlaneBomb)gc);

                        thisAirBomb.destroyBomb();
                    }                    
                }
            }
            catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
            {}

        }

        private void disableAllButtons() // Disable all the menu buttons while the actual game is running.
        {
            buttonBack.disableButton();
            buttonExit.disableButton();
            buttonMissOne.disableButton();
            buttonMissTwo.disableButton();
            buttonOptions.disableButton();
            buttonSingle.disableButton();
            buttonSinglePlayerMissions.disableButton();
            buttonWindowedMode.disableButton();
            buttonSinglePlayerSurvival.disableButton();
        }

        private void enableAllButtons() // Enable all the buttons again when the player stops playing the game and returns to the main menu.
        {
            buttonBack.enableButton();
            buttonExit.enableButton();
            buttonMissOne.enableButton();
            buttonMissTwo.enableButton();
            buttonOptions.enableButton();
            buttonSingle.enableButton();
            buttonSinglePlayerMissions.enableButton();
            buttonWindowedMode.enableButton();
            buttonSinglePlayerSurvival.enableButton();
        }


        private void setUpMissionOne() // Set up the first mission
        {
            const int GROUND_LEVEL_UNIT = 263; // The effective ground of the first level for units.

            // set up each squad memeber in turn with their init pos etc.

            squad[0] = new Unit(this, new Vector2(0, GROUND_LEVEL_UNIT), ref bullet, ref airBombAmmoTexture, ref airPlaneBomberTexture, 0);
            squad[1] = new Unit(this, new Vector2(60, GROUND_LEVEL_UNIT), ref bullet, ref airBombAmmoTexture, ref airPlaneBomberTexture, 1);
            squad[2] = new Unit(this, new Vector2(120, GROUND_LEVEL_UNIT), ref bullet, ref airBombAmmoTexture, ref airPlaneBomberTexture, 2);
            squad[3] = new Unit(this, new Vector2(180, GROUND_LEVEL_UNIT), ref bullet, ref airBombAmmoTexture, ref airPlaneBomberTexture, 3);

            squad[selectedUnit].setUnitControled(true); // Set control of the default selected unit in the squad.
            //Set up the turret.
            squadOneArrow = new PointingArrow(this,ref pointArrowTexture, squad[selectedUnit].getPos());
            
            // Add them all to the game components.
            Components.Add(squad[0]);
            Components.Add(squad[1]);
            Components.Add(squad[2]);
            Components.Add(squad[3]);
            Components.Add(new PlasmaTurret(this, new Vector2(700, 231), ref plasmaTurret, ref plasmaProjectile,true)); // 231 refers to the effective ground level for the turret.
            Components.Add(squadOneArrow);

            try
            {
                foreach (GameComponent gc in Components)
                {
                    if (gc is PlasmaTurret) // This code is not run when the plasma turret is destroyed.
                    {
                        PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc);

                        if (thisPlasmaTurret.getIsMissOneTurr() == true)
                        {
                            missOnePlasmaTurretHealth = thisPlasmaTurret.getHealth();
                        }
                    }
                }

            }
            catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
            { } 



        }

        private void setUpSinglePlayerSurvival() // Set up the single player survival mission.
        {
            const int GROUND_LEVEL_UNIT = 263; // The effective ground of the first level for units, may need changing.

            // set up each squad memeber in turn with their init pos etc.
            squad[0] = new Unit(this, new Vector2(0, GROUND_LEVEL_UNIT), ref bullet, ref airBombAmmoTexture, ref airPlaneBomberTexture, 0);
            squad[1] = new Unit(this, new Vector2(60, GROUND_LEVEL_UNIT), ref bullet, ref airBombAmmoTexture, ref airPlaneBomberTexture, 1);
            squad[2] = new Unit(this, new Vector2(120, GROUND_LEVEL_UNIT), ref bullet, ref airBombAmmoTexture, ref airPlaneBomberTexture, 2);
            squad[3] = new Unit(this, new Vector2(180, GROUND_LEVEL_UNIT), ref bullet, ref airBombAmmoTexture, ref airPlaneBomberTexture, 3);

            squad[selectedUnit].setUnitControled(true); // Set control of the default selected unit in the squad.
            //Set up the turret.
            squadOneArrow = new PointingArrow(this, ref pointArrowTexture, squad[selectedUnit].getPos());

            // Add them all to the game components.
            Components.Add(squad[0]);
            Components.Add(squad[1]);
            Components.Add(squad[2]);
            Components.Add(squad[3]);
            Components.Add(squadOneArrow);

        }

        private void doGameLogic() // Do logic in the game like collisions and stuff.
        {
            bool hasCollisionWithTurret = false; // Init to false.
            bool hasCollisionWithSlicebot = false;
            bool hasCollisionWithUnit = false;
            bool unitHasCollisionWithSlicebot = false;           
            bool bombHasCollisionWithSlicebot = false;
            bool bombHasCollisionWithTurret = false;

            

            /// Big note; && thisBullet.getBulletStateString() == "active" is required in order to prevent bullets from colliding with turrets or units when the mission is set up once more after having played it once already.
            try
            {
                foreach (GameComponent gc in Components) // Check each game component.
                {
                    if (gc is Bullet) // If its a bullet
                    {
                        Bullet thisBullet = ((Bullet)gc); // A local bullet.

                        if (thisBullet.getSource() == "Lmg") // for possible sources, if it was an lmg that created this bullet...
                        {
                            foreach (GameComponent otherGc in Components) // Check each game component.
                            {
                                if (otherGc is PlasmaTurret) // Check if bullets are collided with the plasma turret.
                                {
                                    PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)otherGc);

                                    if (thisPlasmaTurret != null) // Required to prevent null pointer exceptions, same for other stuff.
                                    {
                                        if (thisPlasmaTurret.getTurretActive() == true && thisBullet.getBulletStateString() == "active") // If the turret is active.. and the projectile is too..
                                        {
                                            hasCollisionWithTurret = ((Bullet)gc).checkCollision(thisPlasmaTurret.GetBounds()); // check whether there is a collision with the plasma turret.

                                            if (hasCollisionWithTurret) // If there is...
                                            {
                                                int damage = squad[selectedUnit].getMachineGun().getDamage(); // Get the damage.
                                                thisPlasmaTurret.beDamaged(damage,"Bullet"); // Damage the turret.

                                                if (thisPlasmaTurret.getIsMissOneTurr() == true) // If it is the mission one plasma turret..
                                                {
                                                    missOnePlasmaTurretHealth = thisPlasmaTurret.getHealth(); // Get it's health.
                                                }

                                                break; // Break out of this.
                                            }

                                        }
                                    }
                                }
                                else if (otherGc is Slicebot)
                                {
                                    Slicebot thisSliceBot = ((Slicebot)otherGc);

                                    if (thisSliceBot != null) // Required to prevent null pointer exceptions, same for other stuff.
                                    {
                                        if (thisSliceBot.getSlicebotActive() == true && thisBullet.getBulletStateString() == "active") // If the turret is active.. and the projectile is too..
                                        {
                                            hasCollisionWithSlicebot = ((Bullet)gc).checkCollision(thisSliceBot.GetBounds()); // check whether there is a collision with the plasma turret.

                                            if (hasCollisionWithSlicebot) // If there is...
                                            {
                                                int damage = squad[selectedUnit].getMachineGun().getDamage(); // Get the damage.
                                                thisSliceBot.beDamaged(damage,"Bullet"); // Damage the turret.

                                                if (thisSliceBot.getIsMissTwoSlicebot() == true) // If this is the mission two slicebot...
                                                {
                                                    missTwoSlicebotHealth = thisSliceBot.getHealth(); // Get its health.
                                                }

                                                break; // Break out of this.
                                            }

                                        }
                                    }
                                }
                            }
                         
                        }
                        else if (thisBullet.getSource() == "Plasma Turret")
                        {
                            foreach (Unit unit in squad) // For each unit in the squad...
                            {
                                if (unit.getUnitActive() == true && thisBullet.getBulletStateString() == "active") // If this unit is active...
                                {
                                    hasCollisionWithUnit = ((Bullet)gc).checkCollision(unit.GetBounds()); // check whether there is a collision with any of the units

                                    if (hasCollisionWithUnit) // If there is...
                                    {

                                        int damage = 50; // Get the damage. (Its 50 but need proper dynamic way to get it).
                                        unit.beDamaged(damage); // Damage the unit.

                                        break; // Break out of this.
                                    }

                                }

                            }
                        }
                    }
                    else if (gc is AirPlaneBomb) // If its an air plane bomb...
                    {
                        AirPlaneBomb thisAirBomb = ((AirPlaneBomb)gc); // get this local bomb...

                        if (thisAirBomb.getSource() == "PlayerSummonedBomber") // If the bomb was summoned by a player summoned bomber...
                        {
                            foreach (GameComponent otherGc in Components)
                            {
                                if (otherGc is PlasmaTurret) // Check if bomb are hitting the the plasma turret.
                                {
                                    PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)otherGc);

                                    if (thisPlasmaTurret != null) // Required to prevent null pointer exceptions, same for other stuff.
                                    {
                                        if (thisPlasmaTurret.getTurretActive() == true && thisAirBomb.getAirBombStateString() == "active") // If the turret is active.. and the projectile is too..
                                        {
                                            bombHasCollisionWithTurret = ((AirPlaneBomb)gc).checkCollision(thisPlasmaTurret.GetBounds()); // check whether there is a collision with the plasma turret.

                                            if (bombHasCollisionWithTurret) // If there is...
                                            {
                                                int damage = thisAirBomb.getDamage(); // Get the damage.
                                                thisPlasmaTurret.beDamaged(damage,"AirBomb"); // Damage the turret.
                                                audioComponent.playCue("Air bomb explode");
                                                break; // Break out of this.
                                            }

                                        }
                                    }
                                }
                                else if (otherGc is Slicebot)
                                {
                                    Slicebot thisSliceBot = ((Slicebot)otherGc);

                                    if (thisSliceBot != null) // Required to prevent null pointer exceptions, same for other stuff.
                                    {
                                        if (thisSliceBot.getSlicebotActive() == true && thisAirBomb.getAirBombStateString() == "active") // If the turret is active.. and the projectile is too..
                                        {
                                            bombHasCollisionWithSlicebot = ((AirPlaneBomb)gc).checkCollision(thisSliceBot.GetBounds()); // check whether there is a collision with the plasma turret.

                                            if (bombHasCollisionWithSlicebot) // If there is...
                                            {
                                                int damage = thisAirBomb.getDamage(); ; // Get the damage.
                                                thisSliceBot.beDamaged(damage, "AirBomb"); // Damage the slicebot.
                                                audioComponent.playCue("Air bomb explode");

                                                if (thisSliceBot.getIsMissTwoSlicebot() == true) // If this is the mission two slicebot...
                                                {
                                                    missTwoSlicebotHealth = thisSliceBot.getHealth(); // Get its health.
                                                }

                                                break; // Break out of this.
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
            {}


            // Check for collision between the unit and the slicebot, did this way for now but could put in main loop.

            try
            {
                foreach (Unit thisUnit in squad)
                {
                    foreach (GameComponent gc in Components) // Check whether the squad is colliding with anything.
                    {
                        if (gc is Slicebot) // Such as slicebots...
                        {
                            Slicebot thisSliceBot = ((Slicebot)gc); // Get this local slicebot.

                            if (thisUnit != null && thisSliceBot != null) // If the unit AND THE SLICEBOT have been instanced... IMPORTANT MAKE CHECKS ON BOTH OR IT CRASHES.
                            {
                                if (thisUnit.getUnitActive() == true && thisSliceBot.getSlicebotActive() == true)
                                {
                                    unitHasCollisionWithSlicebot = thisUnit.checkCollision(thisSliceBot.GetBounds());

                                    if (unitHasCollisionWithSlicebot) // If there is a collision with the slicebot...
                                    {
                                        int damage = thisSliceBot.getDamage(); // Get the damage.
                                        thisUnit.beDamaged(damage); // Damage the unit.

                                        break; // Break out of this.
                                    }
                                }
                            }

                        }                        
                    }
                   
                }

            }
            catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
            { }

            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            Vector2 textVector = new Vector2(200,10); // Need to make this more dynamic later, the vector for where the title should be on the screen.
            Vector2 tutTextVector = new Vector2(0, 10); // This vector allows more of the tutorial text to be drawn on one line.
            Vector2 windowedModeCaptionVector = new Vector2(200, 100); // This vector is for the placing of the caption of the windowedmodeCation vector.
            Vector2 tutTextVectorLine2 = new Vector2(0, 40); // This vector is for allowing another line of text in the same state. (May add more of these)
            Vector2 tutTextVectorLine3 = new Vector2(0, 70);
            Vector2 tutTextVectorLine4 = new Vector2(0, 100);
            Vector2 tutTextVectorLine5 = new Vector2(0, 130);

            switch (gameState) // Check the state of the game, then draw/ do not draw components as nessecary.
            {
                case GameState.mainMenu: // Only draw the buttons if it is the main menu, else do not.
                    spriteBatch.Begin();
                    spriteBatch.Draw(menuBackGround, new Rectangle(0, 0, 800, 600), Color.LightGray); // Draw the menu background.
                    buttonCredits.Draw(gameTime); // Credits in place of multiplayer.
                    buttonOptions.Draw(gameTime);
                    buttonSingle.Draw(gameTime);
                    buttonExit.Draw(gameTime);
                    spriteBatch.End();

                    spriteBatch.Begin(); 
                    spriteBatch.DrawString(gameFont, "Strategy combat", textVector, Color.Chocolate); // Put the title on the screen.
                    spriteBatch.End();

                    break;
                case GameState.multiPlayerMenu: // Repeat as nessecary.
                    spriteBatch.Begin();
                    spriteBatch.Draw(menuBackGround, new Rectangle(0, 0, 800, 600), Color.LightGray); // Draw the menu background.
                    spriteBatch.DrawString(gameFont, "Multi player", textVector, Color.Chocolate); 
                    buttonBack.Draw(gameTime);
                    spriteBatch.End();
                    break;
                case GameState.credits:
                    spriteBatch.Begin(); 
                    spriteBatch.Draw(menuBackGround, new Rectangle(0, 0, 800, 600), Color.LightGray); // Draw the menu background.
                    spriteBatch.DrawString(gameFont, "CREDITS", textVector, Color.Chocolate);
                    spriteBatch.DrawString(tutorialMessageFont, "Programming: James Moran", new Vector2(0, 60), Color.Chocolate);
                    spriteBatch.DrawString(tutorialMessageFont, "Game design: James Moran", new Vector2(0, 90), Color.Chocolate);
                    spriteBatch.DrawString(tutorialMessageFont, "Art design: James Moran", new Vector2(0, 120), Color.Chocolate);
                    spriteBatch.DrawString(tutorialMessageFont, "Various voices: Eddie 'Sarge' Gustavsson", new Vector2(0, 150), Color.Chocolate);
                    spriteBatch.DrawString(tutorialMessageFont, "Non-bespoke Sound effects: Freesound.org (All those below)", new Vector2(0, 180), Color.Chocolate);
                    spriteBatch.DrawString(tutorialMessageFont, "Light machine gun sound: 'Matt G'", new Vector2(0, 210), Color.Chocolate);
                    spriteBatch.DrawString(tutorialMessageFont, "Plasma turret charging sound: 'wildweasel'", new Vector2(0, 240), Color.Chocolate);
                    spriteBatch.DrawString(tutorialMessageFont, "Plasma turret shooting: 'Ch0cchi'", new Vector2(0, 270), Color.Chocolate);
                    spriteBatch.DrawString(tutorialMessageFont, "Slicebot whir/saw sound: 'acclivity'", new Vector2(0, 300), Color.Chocolate);
                    spriteBatch.DrawString(tutorialMessageFont, "Bomb explosion sound: 'sandyrb'", new Vector2(0, 330), Color.Chocolate);
                    spriteBatch.DrawString(tutorialMessageFont, "Bomber flying sound: 'Robinhood76'", new Vector2(0, 360), Color.Chocolate);
                    
                    buttonBack.Draw(gameTime);
                    spriteBatch.End();
                    break;
                case GameState.options:
                    spriteBatch.Begin(); 
                    spriteBatch.Draw(menuBackGround, new Rectangle(0, 0, 800, 600), Color.LightGray); // Draw the menu background.
                    spriteBatch.DrawString(gameFont, "Options", textVector, Color.Chocolate); 
                    buttonBack.Draw(gameTime);
                    spriteBatch.DrawString(tutorialMessageFont, "Windowed mode on/off.", windowedModeCaptionVector, Color.Chocolate);                    
                    buttonWindowedMode.Draw(gameTime);
                    spriteBatch.End();
                    break;
                case GameState.singlePlayerMenu:
                    spriteBatch.Begin(); 
                    spriteBatch.Draw(menuBackGround, new Rectangle(0, 0, 800, 600), Color.LightGray); // Draw the menu background.
                    spriteBatch.DrawString(gameFont, "Single Player", textVector, Color.Chocolate);
                    buttonSinglePlayerMissions.Draw(gameTime);
                    buttonSinglePlayerSurvival.Draw(gameTime);
                    buttonBack.Draw(gameTime);
                    spriteBatch.End();
                    break;
                case GameState.singlePlayerMissions:
                    spriteBatch.Begin(); 
                    spriteBatch.Draw(menuBackGround, new Rectangle(0, 0, 800, 600), Color.LightGray); // Draw the menu background.
                    spriteBatch.DrawString(gameFont, "Missions", textVector, Color.Chocolate);
                    buttonMissOne.Draw(gameTime);
                    buttonMissTwo.Draw(gameTime);
                    buttonBack.Draw(gameTime);
                    spriteBatch.End();
                    break;
                case GameState.missionOneWelcome:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(actualTutMessageFont, "Welcome to strategy combat; a game of many weapons and enemies for their use.", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A to continue)", tutTextVectorLine2, Color.Chocolate);
                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);
                    
                    // As well as the plasma turret.                                      
                    spriteBatch.End();

                    foreach (GameComponent gc in Components)
                    {
                        if (gc is PlasmaTurret)
                        {
                            PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc);

                            if (thisPlasmaTurret.getIsMissOneTurr() == true)
                            {
                                thisPlasmaTurret.Draw(gameTime);
                            }
                        }
                    }  

                    break;
                case GameState.missionOneMovementControls:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(actualTutMessageFont, "Controls for units are as follows: w,a and d (or the left analog stick on controllers)", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "for movement up, left and right.", tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A to continue)", tutTextVectorLine3, Color.Chocolate);
                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);
                    
                    // As well as the plasma turret.                   
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is PlasmaTurret)
                            {
                                PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc);

                                if (thisPlasmaTurret.getIsMissOneTurr() == true)
                                {
                                    thisPlasmaTurret.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }

                    break;
                case GameState.missionOneCombatControls:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(actualTutMessageFont, "Press the number keys or use the d-pad to select weapons, right trigger or left mouse fires.", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A to continue)", tutTextVectorLine2, Color.Chocolate);
                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);
                    
                    // As well as the plasma turret.                                    
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is PlasmaTurret)
                            {
                                PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc);

                                if (thisPlasmaTurret.getIsMissOneTurr() == true)
                                {
                                    thisPlasmaTurret.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.missionOneUnitSelectionControls:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(actualTutMessageFont, "To select a unit left click on it or press Y on the gamepad to cycle through units.", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A to continue)", tutTextVectorLine2, Color.Chocolate);
                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);
                    
                    // As well as the plasma turret.
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is PlasmaTurret)
                            {
                                PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc);

                                if (thisPlasmaTurret.getIsMissOneTurr() == true)
                                {
                                    thisPlasmaTurret.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }

                    break;
                case GameState.missionOneMakeReady:

                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(actualTutMessageFont, "Now, to begin the fight press A on the gamepad or enter on the keyboard.", tutTextVector, Color.Chocolate);
                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);
                    
                    // As well as the plasma turret.                 
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is PlasmaTurret)
                            {
                                PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc);

                                if (thisPlasmaTurret.getIsMissOneTurr() == true)
                                {
                                    thisPlasmaTurret.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }

                    break;

                case GameState.missionOnePlayStageOne: // Begin playing the first mission.
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(actualTutMessageFont, "Objective; Destroy the enemy plasma turret.", tutTextVector, Color.Chocolate);
                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);
                    
                    // As well as the plasma turret.                    

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    } 
 
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is PlasmaTurret)
                            {
                                PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc);

                                if (thisPlasmaTurret.getIsMissOneTurr() == true)
                                {
                                    thisPlasmaTurret.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }

                    break;
                case GameState.missionOnePlayerLost: // Both the loss state and the win state cause the game to not update so to speak, technically desirable but may cause problems; see the Update method for more info.
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(actualTutMessageFont, "Mission failed; squad wiped out.(Press enter or A to continue)", tutTextVector, Color.Chocolate);
                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);
                    
                    // As well as the plasma turret.                   
                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }  
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is PlasmaTurret)
                            {
                                PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc);

                                if (thisPlasmaTurret.getIsMissOneTurr() == true)
                                {
                                    thisPlasmaTurret.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }

                    break;
                case GameState.missionOnePlayerWon:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(actualTutMessageFont, "Mission succesful; enemy turret has been destroyed. (Press enter or A to continue)", tutTextVector, Color.Chocolate);
                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);
                    
                    // As well as the plasma turret.
                    

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }  
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is PlasmaTurret)
                            {
                                PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc);

                                if (thisPlasmaTurret.getIsMissOneTurr() == true)
                                {
                                    thisPlasmaTurret.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.missionTwoWelcome: // The intro to mission two.
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(actualTutMessageFont, "Hello again and welcome to mission 2.", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "In this mission we shall be going over more advanced controls and weapons.", tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A on the gamepad to continue)", tutTextVectorLine3, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);
                   
                    spriteBatch.End();
                    break;
                case GameState.missionTwoAlert:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(actualTutMessageFont, "Wait... what's that sound?", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A on the gamepad to continue)", tutTextVectorLine2, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                   
                    spriteBatch.End();
                    break;
                case GameState.missionTwoPlayStageOne: // The play state of mission two, alot of jumping around between states is required here.
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    spriteBatch.DrawString(tutorialMessageFont, "Objective: ???", tutTextVector, Color.Chocolate);
                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }                 
 
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }

                    break;
                case GameState.missionTwoWarning: // This state is switched too after the slicebots are spawned in and start moving towards the player's units.
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    spriteBatch.DrawString(actualTutMessageFont, "Great... a slicebot;they are sharp balls of death.", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "They will pull your units towards them and then slice them up.", tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "You will have use some tactics in order to beat this machine...", tutTextVectorLine3, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A on the gamepad to continue)", tutTextVectorLine4, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }

                    
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.missionTwoHelpBackPedal:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend); // Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "To start with you will need to learn to move backwards while shooting.", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "To do this, aim the shot in the direction you want then hold the shift key or right bumper.", tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "This will then not change your unit's point of aim but allow you to shoot in a direction different to the one you are moving in.", tutTextVectorLine3, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A on the gamepad to continue)", tutTextVectorLine4, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }

                    
                   
                    spriteBatch.End();
                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }

                    break;
                case GameState.missionTwoHelpOrders:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "Unfortnatly, this will only help the unit you are controlling and whats more,", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "will not escape the grip of the slicebot.However, you can give your units orders to help them out.", tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "Press T Or the DPAD Up to order your units to utilise the 'fighting retreat' order.", tutTextVectorLine3, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A on the gamepad to continue)", tutTextVectorLine4, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }

                    
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.missionTwoGetReady:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "This will pump your units full of adrenaline in order to help them fight and move faster", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "It will also retreat them to the nearest rally point, this is the far left side of the battlefield.", tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "Using this, order your units to back away from the slicebot and keep shooting while doing so.", tutTextVectorLine3, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A on the gamepad to continue)", tutTextVectorLine4, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }

                    
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.missionTwoPlayStageTwo:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "Objective;order your units to retreat from the slicebot", tutTextVector, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }

                    
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.missionTwoHelpRetreatOrder:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "Well done, you have succesfully retreated your units away from the slicebot.", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "However, it seems that bullets are not very effective against the slicebot.", tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "Insted, press C or DPADdown to merley order your units to retreat left but not shoot.", tutTextVectorLine3, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A on the gamepad to continue)", tutTextVectorLine4, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }

                    
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.missionTwoHelpSelectNoUnits:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "Also; if you are confident in your orders, you can select no units.", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "Press the right mouse button or B and then your units will just act under any orders you have given them.", tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "Combine both of these actions now to make your units stay away from the slicebot and hold fire.", tutTextVectorLine3, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A on the gamepad to continue)", tutTextVectorLine4, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }

                    
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.missionTwoPlayStageThree:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "Objective; order your units to retreat(not fighting retreat) and unselect them.", tutTextVector, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }

                    
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.missionTwoWellDoneStageThree: // Congratulate the player on a job well done... so far.
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "Very good, now as you can see, the units will act under orders alone.", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "Now that your units are safe we still need to deal with that slicebot...", tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "I think I can get some air support for your units to use.", tutTextVectorLine3, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A on the gamepad to continue)", tutTextVectorLine4, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }
                  
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.missionTwoHelpAirStrike:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "First reselect a unit then press 2 or DPAD left to equip the airstrike.", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "Then left click on where you want to call in the airstrike.", tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "This will call in a precision airstrike on that location.", tutTextVectorLine3, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A on the gamepad to continue)", tutTextVectorLine4, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }

                    
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.missionTwoPlayStageFour:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "Objective; destroy the slicebot. ", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Call in an airstrike to more effciently destroy it)", tutTextVectorLine2, Color.Chocolate);
  
                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }

                    
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                   
                    break;

                case GameState.missionTwoPlayerLost:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "Mission failed; squad wiped out.(Press enter or A to continue)", tutTextVector, Color.Chocolate);


                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }
                   
                    spriteBatch.End();

                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.missionTwoPlayerWon:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();


                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "Mission Success; Slicebot threat elimnated.(Press enter or A to continue)", tutTextVector, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }

                                      
                    spriteBatch.End();
                    try
                    {
                        foreach (GameComponent gc in Components)
                        {
                            if (gc is Slicebot)
                            {
                                Slicebot thisSlicebot = ((Slicebot)gc);

                                if (thisSlicebot.getIsMissTwoSlicebot() == true)
                                {
                                    thisSlicebot.Draw(gameTime);
                                }
                            }
                        }
                    }
                    catch (InvalidOperationException ioe) // Swallow this exception for now. (Don't sue me :( )
                    { }
                    break;
                case GameState.singlePlayerSurvivalWelcome: // The single player survival welcome state.
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "Hello again and welcome to the single player survival mode.", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "In this mode, survive against waves of enemies for as long as possible.", tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "You will have to use all the tactics you have learned and more to survive here...", tutTextVectorLine3, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A on the gamepad to continue)", tutTextVectorLine4, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    spriteBatch.End();
                    break;
                case GameState.singlePlayerSurvivalHint:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "You will get supply drops of health every so often.", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "Waves of enemies will get progressively more and more overwhelming.", tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "One hint though, press Y on the keyboard or left bumper to make your units take cover.", tutTextVectorLine3, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "Good luck out there!", tutTextVectorLine4, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "(Press enter or A on the gamepad to start)", tutTextVectorLine5, Color.Chocolate);

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    spriteBatch.End();
                    break;
                case GameState.singlePlayerSurvivalPlayStageOne:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "Objective:Survive!", tutTextVector, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "Time till next slicebot:" + (timeTillNextSliceBot - sliceBotAddCounter) / 100, tutTextVectorLine2, Color.Chocolate);
                    spriteBatch.DrawString(actualTutMessageFont, "Time till next plasma turret:" + (timeTillNextPlasmaTurret - plasmaTurretAddCounter) / 100, tutTextVectorLine3, Color.Chocolate);  

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }  

                    spriteBatch.End();

                    foreach (GameComponent gc in Components) // Check each game component.
                    {
                        if (gc is PlasmaTurret) // If its a bullet
                        {
                            PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc); // Get this local plasma turret.

                            if (thisPlasmaTurret != null)
                            {
                                if (thisPlasmaTurret.getTurretActive() == true && thisPlasmaTurret.getIsMissOneTurr() == false)
                                {
                                    thisPlasmaTurret.Draw(gameTime);
                                }

                            }
                        }
                        else if (gc is Slicebot)
                        {
                            Slicebot thisSliceBot = ((Slicebot)gc); // Get this local slicebot.

                            if (thisSliceBot != null)
                            {
                                if (thisSliceBot.getSlicebotActive() == true && thisSliceBot.getIsMissTwoSlicebot() == false)
                                {
                                    thisSliceBot.Draw(gameTime);
                                }
                                
                            }
                        }
                    }

                    break;

                case GameState.singlePlayerSurvivalPlayerLost:
                    spriteBatch.Begin();
                    spriteBatch.Draw(missionOneBackground, new Rectangle(0, 0, 800, 600), Color.LightGray);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);// Need to make the tut text run over more lines or smaller or both in order to see all of what is done here.
                    spriteBatch.DrawString(actualTutMessageFont, "Mission over; squad wiped out. (Press A or Enter to continue)", tutTextVector, Color.Chocolate);                   

                    squad[0].Draw(gameTime); // Draw all of the members of the squad.
                    squad[1].Draw(gameTime);
                    squad[2].Draw(gameTime);
                    squad[3].Draw(gameTime);

                    if (squad[selectedUnit].getUnitStateString() != "destroyed" && noUnitSelected == false) // Only draw the arrow if the selected unit is not destroyed.
                    {
                        squadOneArrow.Draw(gameTime); // And the pointing arrow.     
                    }  

                    spriteBatch.End();

                    foreach (GameComponent gc in Components) // Check each game component.
                    {
                        if (gc is PlasmaTurret) // If its a bullet
                        {
                            PlasmaTurret thisPlasmaTurret = ((PlasmaTurret)gc); // Get this local plasma turret.

                            if (thisPlasmaTurret != null)
                            {
                                if (thisPlasmaTurret.getTurretActive() == true && thisPlasmaTurret.getIsMissOneTurr() == false)
                                {
                                    thisPlasmaTurret.Draw(gameTime);
                                }

                            }
                        }
                        else if (gc is Slicebot)
                        {
                            Slicebot thisSliceBot = ((Slicebot)gc); // Get this local slicebot.

                            if (thisSliceBot != null)
                            {
                                if (thisSliceBot.getSlicebotActive() == true && thisSliceBot.getIsMissTwoSlicebot() == false)
                                {
                                    thisSliceBot.Draw(gameTime);
                                }
                                
                            }
                        }
                    }

                    break;


                    

            } 
        }
    }
}
