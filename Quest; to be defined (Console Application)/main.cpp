#include <iostream> // Required for input and ouput
#include <Item.h> // Item header file.
#include <Weapon.h> // Header files that I have made for my classes are needed for this program
#include <sstream> // Needed for proper type conversion functions
#include <windows.h> // for PlaySound() and other functions like sleep.
#include <time.h> // Needed to seed the rand() function and other functions maybe.
#include <mmsystem.h> // Not sure about this one, possibly defunct in this program.
#include <stdio.h> // Needed for a similar kind of output as iostream for various functions error msgs.
#include <Narrator.h> // The narrators's header file.
#include <Pibot.h> // Other header files of classes.
#include <Armour.h>
#include <Player.h>
#include <Weapon.h>
#include <CombatAdmin.h>
#include <Merchant.h> // The first NPC the player encounters.
#include <HealthAndAmmoSuppiler.h> // For the second NPC the player encounters.

/**
	IMPORTANT, PLEASE READ:

	2017 Overview

	I produced this project during the 1, 2 and 3rd quarter of 2013. I have not looked at this project in a while and honestly,
	I cringe at certain aspects of it (such as spelling mistakes in symbol names/dialog,
	grammatical mistakes in dialog, as well as both of these types of mistakes occurring in relation to comments).
	In all fairness, during this year, I was not in the right state of mind (given an incident that occurred in the 4th quarter
	of 2012).

	As per the 4th 'To-Do' item, it was submitted to the formally known
	service of 'Steam Greenlight' (now 'Steam Direct'). I removed it from this service after it received 69%
	of its rating’s as 'dislike' ratings, with only 31% 'like' ratings. But I received important feedback in
	hindsight, which I have kept a log of and can show you if you so wish.	
*/

// PLEASE NOTE THAT IF CODE IS COMMENTED OUT, IT IS NOT USED CURRENTLY AND MAY NEVER BE USED AT ALL.

// IMPORTANT Make sure under project -> build options ...
// Need dir of include files location in the search directories under compiler for the whole project (not debug or release, both) or the compiler will not find the files.
// DO THIS TO MAKE SURE THE COMPILER CAN FIND ALL THE LIBRARIES AND STUFF.

// THINGS TO DO BELOW!
// 1. Eyes on this demo, it seems combatRound and randomEncounter code has to have 1V1 at the end of the func name to work and more params it seems...
// 2. It seems that any changes I make to the main game that effect the game here too have to be changed here, hmm...
// 3. Make sure to include any new params needed for funcs too that are not declared here when they are declared and used in the main version.
// 4. Maybe make this demo a bit longer or explain how many encounters there could be(on info put on steam greenlight, perhaps in some sort of spoiler) depending on the player's choice and why there may be only one random encounter chance etc.
// 5. As per task 2, it seems if you make changes to the main game, it makes changes here too and vice versa, so eyes on...(But do not update the header files here, in the main game only though)
// 6. NOTE: you do not need to check and set the ammo to the correct values in this demo as the data here is never saved KEEP THIS IN MIND!
// 7. You may need some of the pointers as per the main game if you extend this demo's length as per the esmode for example, not just filler pointers that are used now, so keep this in mind and eyes on if you do...
// End of things to do.

// include console I/O methods (conio.h for windows, wrapper for linux as it is hidden away....)
#if defined(WIN32)
#include <conio.h>
#else
#include "../common/conio.h"
#endif

// VECTORS SEEM TO BE A BETTER FORM OF COLLECTION THEN ARRAY'S, USE EM! The player's backpack now seems to function better with vectors for example...
// I THINK FOR PATCHES IN THE FUTURE, YOU WILL JUST NEED TO UPLOAD THE FILE THAT HAS BEEN CHANGED, NOT ALL OF THEM, E.G. THIS MAIN FILE OR ANY HEADER OR IMPLEMENTATION FILES THAT HAVE HAD CHANGES!
// USE FOWARD SLASHES THAT GO TO THE RIGHT LIKE JUST ABOVE, E.G. /.

// FUTURE REF: SINGLETON PATTERN IS FOR ONLY ALLOWING ONE INSTANCE OF A CLASS, SO SEE pgs 320-blah for info on the singleton pattern and how to do it in c++!!!
// ALSO UNDEFINED REFERENCE ERRORS COULD MEAN YOU HAVE A FUNCTION IN A HEADER FILE THAT IS NOT WRITTEN OUT IN THE CPP FILE!

using namespace std; // So I don't have to keep typing std:: for various things.
//functions in main are generic functions that may be required here, the classes may get some of these functions.

// Foward referenced functions

// Remeber that as well as in the header files, the implementation of this must have a * after the return type and before the class:: so cake *class::caler() must be done too.
// This is needed to be done to return a pointer correctly and also make sure not to return the pointer like "return *pointer" do "return pointer" etc.

// Forward referenced functions.
void seedRandom(); // Seeds the random number so it will be random as apposed to pseudo random.
string getStringAnswer(string temp); // Gets the player's new name.
string forkInTheRoad(string answer,Player *player,Narrator *narrator,Pibot *piebot); // PIbot hears what the player has to say about where they should go in this fork in the road.

// Remember to check if making stuff needed at the top of the main function works properly like pointers and not that they must be declared later on, CHECK THIS WORKS.

// REMEMBER THAT YOU NEED TO USE SET PRECISION AND ANOTHER THING IN CHANING DOUBLES PROPERLY TO NUMBERS TO MAKE THEM DISPLAY PROPERLY!
// This as well as displaying the stats of item's for sale properly fixes that long bugbear I had to so to speak of not having damage in combat shown properly (i.e. not precise enough).
// REMEMBER TO NOT USE TOO MANY GOTO STATEMENTS AND THAT THEY ONLY SEEM TO BE ABLE TO BE USED IN THIS MAIN FUNCTION!
// MAYBE LOOK INTO GETTING THE GAME ON OTHER DIGITAL DISTRIBUTION PLATFORMS SUCH AS GAMER'S GATE ETC(not just steam)!
int main(int argc, char* argv[]) // This demo version of the game seems to work but EYES ON AND MAKE SURE THE NEW INFO ABOUT EQUIPMENT WORKS AND THE RESETTING OF PRIV MATT!
{
    // Varibles and object pointers decalred here.
    bool testWepWeight = false;
    bool testArmWeight = false;
    CombatAdmin *comAd = new CombatAdmin(); // Handles combat.
    Narrator *narrator = new Narrator(); // The Narrator that says stuff.
    Pibot *piebot = new Pibot(); // PIbot, the player's trusty companion.
    Weapon *machinePistol = new Weapon(25,75,"Heavy Machine Pistol",3,true,true,10,300,200,750,false); // Create a heavy calibre machine pistol (damage,Attack speed,the name,its weight and cost etc,but not in that order).
    Armour *lgBodyArm = new Armour(0.10,0.20,"Low Grade Body Armour",10,500); // Create some low grade body armour(damage resist,attackspdmod,name,weight and cost etc).
    Weapon *lightMG = new Weapon(40,60,"Light Machine Gun",10,true,true,25,600,200,1250,false); // Items the player will find from the first chest they loot.
    Armour *hgBodyArm = new Armour(0.25,0.30,"High Grade Body Armour",12,800);
    //New player pointer must be instanced here now for debugging purposes.
    Weapon *unarmed = new Weapon(5,90,"Fists",0,false,false,0,0,0,0,false); // To represent the player having no weapons.
    Armour *clothes = new Armour (0.01,0.05,"Unarmoured",0,0); // To represent the player having no armour, being kind towards the player a bit...
    Enemy *privMatt = new Enemy("Private Matterson"); // The priv matter enemy, equipment is equipped in this enemy's constructor.
    Weapon *cheapRifle = new Weapon(14,60,"Cheap Rifle",10,true,true,20,400,300,200,false); // The weapon and armour Matterson has.
    Armour *poorArmour = new Armour(0.05,0.15,"Poor Armour",10,75);
    privMatt->autoEquip(cheapRifle,poorArmour); // Equip the weapon and armour.
    // Keeps eyes on to make sure fists work if needed in un-armed combat if needed...
    Weapon *fists = new Weapon(5,90,"Fists",0,false,false,0,0,0,0,false); // For both the player and the enemy having no weapons if they need to use it as unarmed is changed it seems...
    Player *player1 = new Player(unarmed,clothes); // The player character, remember to always pass pointers without the * or & as they are not needed it would seem...
    Merchant *gestMerchant = new Merchant("John"); // The first merchant the 'team' visits in Gestlehiem and first NPC as it turns out.
    HealthAndAmmoSuppiler *gestHPAndAmmoSup = new HealthAndAmmoSuppiler("Kevin"); // The second NPC that can do 4 things as per one of the 'to-do' tasks.
    // The items the player can buy from the first merchant they meet.
    // If there is a 0 as one of the parameters for a weapon, it will tend to be the chance of a weapon jamming, hence if it is 0, it has no chance to jam.
    Weapon *handGrenades = new Weapon(400,40,"Hand Grenades",5,false,true,0,10,4,900,true); // Some hand grenades the player can buy.
    Armour *clunkyPowerSuit = new Armour(0.45,0.50,"Clunky Power Suit",25,1100); // As well as an old an clunky power suit.
    Weapon *rocketLauncher = new Weapon(200,15,"Rocket Launcher",20,false,true,0,5,3,1500,true); // Items the player finds later on...
    Armour *lgPowArm = new Armour(0.40,0.40,"Low Grade Power Armour",8,1500); // Power armour has something that makes it less dense to the user. This is explained to the player soon after they find it in the story...
    Armour *modArm = new Armour(0.30,0.30,"Moderate Body Armour",11,1300); // This armour protects the user less then the power armour, but is somewhat less restrictive.
    Weapon *battleRifle = new Weapon(80,65,"Winter Militia BR",8,true,true,15,250,125,1100,false); // The Winter militia battle rifle the player can find.
    string forkAnswer = ""; // An answer that will go with the player on their adventures. Determined whether the player went left or right on the first path they come to.
    string temp = ""; // Temp string for input and output
    string upCaseTemp = ""; // Temp for checking the players name at the beginning.
    bool isRandEncounter = false; //  Whether it will be a random encounter or not.
    int speedOfCredits = 0; // The speed the player wants to see the credits at.
    int speedOfCombat = 1; // The speed the player wants combat at (must be 1 for the tutorial battle and then can be changed a bit later).
    bool playerHasFoundScanner = false; // For whether the player has found the scanner or not (got to that point in the game or not thus).
    Item *scanner = new Item(); // Set up this new item for basic measures, used later.
    bool playerWasAbleToHaveRl = false; // Used to check whether the player was able to pick up the items after the player finds the chest they were in.
    bool playerWasAbleToHaveLgArm = false;
    bool playerHasBoughtGrenades = false; // For checking whether the player has bought the powersuit and the grenades or not.
    bool playerHasBoughtPowerSuit = false;
    string playerSoldEqippedItem = ""; // What kind of item the player has sold if they had it equipped.
    bool mercsHired = false; // For whether or not the player hired the mercs, used later if ever.
    bool playerHasChangeEquipAbil = false; // For whether the player now has the ability to change weps and armour in combat.
    bool playerIsInWinterbourne = false; // When the player gets to Winter for example, they cannot run from encounters.
    bool playerHasSurgeNSquad = false; // For whether the player has surge and his squad or not, for if the player can use that abil or not thus(one of the squad members does something, 1 of 3 thus, they all do different things, the abil chooses which one does something).
    Weapon *fillWep = new Weapon(0,0,"",0,false,false,0,0,0,0,false);
    Armour *fillArm = new Armour(0,0,"",0,0); // The fillers, as the demo does not use some items at all...but eyes on...
    // However, the demo may be extended to include weapons that are not used in the prologue, but later on.
    // So include these weapons if needed but do not chance the func calls in the prologue of this demo as this is not needed, just full func calls if more is added where the correct pointers are needed.
    bool playerHasHealAbil = false; // A filler too, the player never gets this here, but eyes on...
    SetConsoleTitle("Quest: To Be Defined demonstration"); // This method to set the console title to w/e only works for windows 2000 professional or later, keep this in mind...
    // This method also does not auto appear when I start typing it in, so eyes on this method...
    // Test this game on the windows 98 comp anyhow to see if it buggers up or just ignores this method.
    // The fake startup tasks intro(i.e the intro I use).

    narrator->narratorSay("Init boot sequence...",2); // first param is what will be outputed on the console, 2nd one is how it will be said

    narrator->narratorSay("Executable file running...",2); // as it is 2 it will not have "Narrator: saysx" it will just say as is here.

    // start the sound engine with default parameters
	Sound *sound = new Sound();
	sound->stopAllSound(); // Stopping the sound here if it is somehow playing...
    Sleep(3000);
    // Play the standard peaceful music and loop it.
    narrator->narratorSay("Starting sound...",2);

	sound->playSoundLooped("Peaceful.wav"); // This function plays the sound that you pass into the param on a loop(remember to check if you need the .type in the string constant's name of the file...).
    Sleep(3000);

    narrator->narratorSay("Declaring variables...",2); // it actually declares the variables it needs at the start before, just used for setting the scene
    narrator->narratorSay("This is version 1.0.0 of this demonstration of this story telling application; remember that this is just a story...",2); // The narrator tells the player the version and that this is just a story...
    narrator->narratorSay("There are either no or limited graphical options in this program, but there are other options to make further in.",2); // The player is told they can do some game based options when needed...
    narrator->narratorSay("The story can be paused if you hold on to the scroll bar to pause the story at the current moment.",2); // The player can pause the game if they hold on to a scroll bar, but they must do so continuously.
    narrator->narratorSay("From here on in, if you are told to enter a value, the story means for you to type it in and press the 'Enter' key on the keyboard.",2);
    narrator->narratorSay("As this is just a demonstration, this will last until the end of the Prologue (chapter 0).",2);
    narrator->narratorSay("You cannot save or load, nor is there the ability to mod the story in this demo. The full version has those capabilities, but will only be released if this piece of software is greenlit.",2); // Tell the player they need the full ver to get this stuff.
    // Seeds the random function so it will produce a set of random numbers each time as opposed to pseudo random ones (well, seemingly random ones, still pseduo but seemingly random)
    seedRandom();

    // Display the opening credits, after the non-narrator has asked the player whether the want them to be displayed fast or slow.

    narrator->narratorSay("Would you like the opening credits to be displayed fast or slow? (Enter either fast or slow to confirm and press enter)",4);

    // Get the answer the player gives.
    temp = getStringAnswer(temp);

    temp = comAd->toUpperCase(temp);

    if (temp == "FAST")
    {
        speedOfCredits = 5;
    }
    else if (temp == "SLOW")
    {
        speedOfCredits = 2; // 2 makes the credits display at normal speed, 5 makes them display 6 times faster.
    }
    else
    {
        narrator->narratorSay("Not a valid answer but the credits must still be displayed, they will be displayed slow though (relativly speaking).",2); // The non-narrator states that the credits still needs to be displayed.
        speedOfCredits = 2; // So the credits will be said using this parameter value.
    }


    narrator->rollCredits(speedOfCredits,true); // Roll the credits at speed '2' or '5' so to speak with them being the opening credits, hence true for the other param.

    temp = ""; // Set temp back to nothing.

    narrator->narratorSay("Program ready, starting main program...",2);
    // Remember to include the space in the title after the colon now...DO IT.
    // Now that all of the stuff above has been taken care of, the program can start proper.
    narrator->narratorSay("Quest: To Be Defined; chapter 0: Prologue",2);
    narrator->narratorSay("Welcome to Quest: To Be Defined, a tale as told by me; the Narrator(it could become epic in 1125 years even, hmm...).",1);
    narrator->narratorSay("Our story begins with you(I say 'you' as I do not yet know your name) walking down a harmless country road.",1);
    narrator->narratorSay("You soon find what appears to be a deactivated robot.",1);
    narrator->narratorSay("However...",1);

    // Player has found PIbot.
    // Like the Narrator PIbot can say different types of messages, if it is one, it will say a standard message, if it is 2, it will say a message without a line break to get input(to avoid confusion).
    piebot->pibotSay("Greetings unknown person! I think I must have been falling asleep here waiting for someone like you to walk down the road and notice me!",1);
    piebot->pibotSay("I am PIbot, that is; 'Personal Intelligence' robot",1);
    piebot->pibotSay("I say falling asleep, but I don't mean really, I am a robot after all... but I can imatate sleep, must have done so while I was being bored, waiting.",1);
    piebot->pibotSay("Guess I must have been left here after the war of twenty-one...ah well.",1);
    piebot->pibotSay("As such, I have something of the upmost importance to say, but first, what is your name? (Your name cannot have no characters at all in it, you should have at least blank spaces in your name...)",2); // Pibot confirms they cannot be called an empty string, it must have at least blank spaces, if the player does call themselves "   " for example though...
    // Let the player use invalid chars for a name that cannot be a filename.
    // Get the player's name
    temp = getStringAnswer(temp);

    upCaseTemp = comAd->toUpperCase(temp); // Check using the upper case temp varible, only used here.

    // Check what the user entered, idk how to make all the conditions appear on the screen at once for this while loop...
    // Thus make it so they can't call themselves the same name as some characters in the game, remember to add more statements to this condition below when more chars are added.

    // No checks for spaces for the last few chars as you could call youself any char in this story's name with a space in front of it(so, early on into this while loop, this only counts for one of the cases for entering it).
    while(upCaseTemp == "PIBOT" || upCaseTemp == "COMBAT ADMIN" || upCaseTemp == "COMBATADMIN" || upCaseTemp == "NARRATOR" || upCaseTemp == "JOHN" || upCaseTemp == "SACRAMENTO" || upCaseTemp == "PRIVATE MATTERSON" || upCaseTemp == "PRIVATEMATTERSON" || upCaseTemp == "MERCANARY JONES" || upCaseTemp == "MERCANARYJONES" || upCaseTemp == "MERCANARY SMITH" || upCaseTemp == "MERCANARYSMITH" || upCaseTemp == "JOHN" || upCaseTemp == "KEVIN" || upCaseTemp == "SARGON" || upCaseTemp == "KASON" || upCaseTemp == "RICKFORD" || upCaseTemp == "SURGE" || upCaseTemp == "TERLON" || upCaseTemp == "GRANT" || upCaseTemp == "JANET" || upCaseTemp == "ROLON" || upCaseTemp == "SGT MAVON" || upCaseTemp == "LT LEON" || upCaseTemp == "KORTHON" || upCaseTemp == "CRIFEN" || upCaseTemp == "FIELD MARSHAL ARTEMIS" || upCaseTemp == "ROBOT OF SHIELDING MARK 5" || upCaseTemp == "SGT AVLON" || upCaseTemp == "LT LUCILEe" || upCaseTemp == "CPT KRALON" || upCaseTemp == "MJR MASON" || upCaseTemp == "COL CAVLON" || upCaseTemp == "PRACTICE BOT 1" || upCaseTemp == "PRACTICE BOT 2")
    {
        upCaseTemp = ""; // Wipe the varible used to check or this program will get stuck in an infinite loop.
        temp = ""; // And this one too.
        piebot->pibotSay("Hmm, that name seems a bit strange... what is your name actually?",1); // Tell the player to enter the name again.
        temp = getStringAnswer(temp);
        upCaseTemp = comAd->toUpperCase(temp);
    }

    if (temp == " " || temp == "  " || temp == "   " || temp == "     " || temp == "      " ) // Only do this if the player entered a blank space for their name (at most 5 blank spaces accounted for at the moment...).
    {
        piebot->pibotSay("Hmm, that seems like a name of nothing so I will just call you blank space, hehe.",1);
        player1->setName("Blank Space"); // The PIbot teases the player by setting their name to 'Blank Space'.
    }

    player1->setName(temp); // The system will not do this if their name has already been set to blank space.

    // The player says their name, may make them say more stuff too.
    player1->playerSay(temp);
    //Clear temp for later use.
    temp = "";
    // PIbot properly greets the player:
    piebot->pibotSay("Well hello there " + player1->getName() + " it is important to know the name of my new master and so I had to ask.",1);
    narrator->narratorSay("(PIbot will not be in your party at the moment but will follow you where-ever you will be going)",2);
    piebot->pibotSay("Right, I can't tell you here, so lets go down the road a bit and I will tell you there.",1);
    // Moving on....
    narrator->narratorSay("So our group as it were; " + player1->getName() + " and PIbot, headed down the road a fair distance to make sure they could get some privacy.",1);
    narrator->narratorSay("When PIbot thought they had gone far enough, it was ready to tell " + player1->getName() + " what it knew...",1);

    // PIbot is about to say something, but....
    piebot->pibotSay("Right the thing that I need to tell you is tha-",1);
    // The combat admin find the team and shows itslf.
    narrator->narratorSay("The PIbot is about to say what it knows, but they get inturupted...",1);
    comAd->comAdSay("Greetings team that requires my help and assistance, I am the combat admin, I administrate combat and other things and little else...",1);
    piebot->pibotSay("Well that is good lovely helper thing, regardless, I guess I best tell you what I know as soon as I can " + player1->getName() + " ,or we may be in trouble...",1);
    //Once again the PIbot wants to say what it knows but...
    piebot->pibotSay("Ok, once again, what I need to tell you is tha-",1);

    sound->stopAllSound(); // Stop the music here.
    // Looks like it's time to fight...
    piebot->combatTut(player1,sound,comAd,narrator,speedOfCombat);

    // Play the peaceful music again.
    sound->playSoundLooped("Peaceful.wav");

    // The Combat Admin says why it was not able to provide too much admin work in that battle.
    comAd->comAdSay("I am sorry I was not able to help administrate that combat too well.",1);
    comAd->comAdSay("But I think for all combats after that one, I should be able to help more.",1);
    piebot->pibotSay("Well that is good then, guess the war may have done some damage to you...",1);
    comAd->comAdSay("Aye, I dare say the war of twenty-one played the part it did...but now we can move on.",1);

    // They are about to move on but the PIbot suggests something to the Combat Admin...

    piebot->pibotSay("Hmm, but just before we do, I think the you can can manage combat a bit faster COMBATADMIN.",1);
    comAd->comAdSay("Yea, I can do my administration duties faster too if you wish, do you want me to do all future combats faster or at the speed I did it just then? (Just enter fast or normal, normal being the speed I did it at then, to confirm)",2);

    // Perhaps make this a place where the player can ask questions (after a bit more dialouge from the PIbot at least) such as how combat works as per one of the to do items.

    //Get and then check the answer the player gives.
    temp = getStringAnswer(temp);

    //The player says the answer they gave.
    player1->playerSay(temp);

    //Converted to upper case so the user could have entered the answer in whatever case structure they wanted to.
    temp = comAd->toUpperCase(temp);

    if (temp == "FAST") // If the player wants combat to be faster the combat admin confirms this.
    {
        comAd->comAdSay("Ok I will do my duties in combat much faster.",1);
        speedOfCombat = 5; // Make the comad set the var to 5 in combat so it says combat messages faster.
    }
    else if (temp == "NORMAL") // The combat admin still confirms the speed will be kept as normal.
    {
        comAd->comAdSay("I will keep my duties at normal speed then.",1);
        speedOfCombat = 1; // Keep the speed at 1 so to speak.
    }
    else // The combat admin will not change the speed to be faster if the player gives an interesting answer to the question.
    {
        comAd->comAdSay("Guess I will not do anything faster then.",1);
        speedOfCombat = 1; // Keep it the same.
    }

    temp = ""; // Clean the temp varible again.

    // The combat admin confirms with the player that the speed value of combat can still be changed.
    comAd->comAdSay("Ok, just to confirm, I can change the speed of combat later when we get a moment if you wish to change it again (or change it at all).",1);

    // Give the player a chance to ask some questions.

    piebot->pibotSay("Hmm, all these events that have happened so far may be a bit much for you " + player1->getName() + ".",1);
    piebot->pibotSay("But tell you what, now you can have a chance to ask me some questions if you wish, do you want to? (Enter yes or no to confirm)",1);

    // Get and then check the answer the player gives.
    temp = getStringAnswer(temp);

    //The player says the answer they gave.
    player1->playerSay(temp);

    // Converted to upper case so the user could have entered the answer in whatever case structure they wanted to.
    temp = comAd->toUpperCase(temp);

    if (temp == "YES")
    {
        piebot->pibotSay("Well I am a personal intelligence robot, so...",1); // The PIbot confirms the yes answer.
        piebot->handleFirstQAndASess(player1,comAd); // Handle this first Q and A sess so to speak.
    }
    else if (temp == "NO")
    {
        piebot->pibotSay("Well, I guess you are perfectly ok with these events then, defiantly normal...let us move on therefore.",1); // The PIbot does not seem to care.
    }
    else
    {
        piebot->pibotSay("Guess you really do not seem to care...*sigh*...",1); // Somehow the pibot sighs...little hint of time there too...
    }

    temp = ""; // Clear the temp var once again.

    piebot->pibotSay("Well, time to move on then...",1); // The PIbot says it is time to move on.

    // After the short introduction, the focus jumps back to here.
    // The narrator then gives the into outro (if you know what I mean, so to speak).
    narrator->narratorSay("So our epic tale(it may become epic soon enough...) begins, with "
            + player1->getName() + " braving their first encounter, the story now continues "
				+ "into a new area with PIbot advising our character on where they should go next. Well... sort of...",1);

    // PIbot tries to talk to the Combatadmin but Combatadmin reports an error that there is no combat running.
    // The irony is that CombatAdmin actually has a say method (see the Combatadmin class), but is scripted to only say stuff-
    // when there is combat (or other things it needs to announce), PIbot then explains to the player that this is all it does, while PIbot on the other hand can seemingly -
    // talk at any time, but that is scripted too.
    piebot->pibotSay("So Combat admin, what do you like to do?",1);
    comAd->comAdSay("ERROR;There are no combats currently running! (Or anything else to notify you of)",1);
    piebot->pibotSay("Gah, looks like COMBATADMIN is only really a tool for telling you about combat(or other announcements), you can't talk to it unlike me.",1);
    piebot->pibotSay("Speaking of which, want to try out my latest functionality? It's rather cool (Enter yes or no to confirm).",2);

    // Get and then check the answer the player gives.

    temp = getStringAnswer(temp);

    //The player says the answer they gave.

    player1->playerSay(temp);

    //Converted to upper case so the user could have entered the answer in whatever case structure they wanted to.

    temp = comAd->toUpperCase(temp);

    piebot->manipTest(temp,player1); // Do the manip test func.

    //Reset the answer temp.
    temp = "";

    // Carrying on...

    narrator->narratorSay("PIbot, Combat Admin and our worthy character, " + player1->getName() + ", are nearing a fork in the road ahead.",1);
    piebot->pibotSay("Hmm, there is a fork in the road, wonder which way we should go " + player1->getName() + ", left or right? (Enter left or right to confirm)",2);

    temp = getStringAnswer(temp);

    player1->playerSay(temp);


    temp = comAd->toUpperCase(temp);
    forkAnswer = temp;

    // The player then states if they want to go left or right, if they give something that is not along the lines of-
    // left or right, then PIbot picks for them.

    forkAnswer = forkInTheRoad(temp,player1,narrator,piebot); // Handle setting the fork answer per say.

    temp = ""; // Whipe the temp var for now.

    // forkAnswer is then used to determine whether what happens.

    comAd->comAdSay("Ok, just saying mind, any enemies we see from now on will have special abilities, so look out for what they can do...",1); // The comAd warns the player of enemy spec abils...

    // Potential encounter options here.
    if (forkAnswer == "LEFT")
    {
        //The player has gone left at the fork in the road.
        piebot->pibotSay("Hey look " + player1->getName() + ", a couple of items lying around. They look useful.",1);
        //Player finds two items, a machine pistol and some low grade body armour.

        testWepWeight = player1->weightChange(machinePistol->getItemWeight(),comAd); // Check the weight as this func returns a bool value.
        testArmWeight = player1->weightChange(lgBodyArm->getItemWeight(),comAd);

        if (testWepWeight == false) // The player does not have enough weight
        {
            // So do nothing.
        }
        else if (testWepWeight == true) // Add the weight of the new weapon to the player's exisiting weight.
        {
            player1->addBackPackItem(comAd,machinePistol,false);
            comAd->playerFindsItem(player1,machinePistol,2);
        }

        if (testArmWeight == false) // The player does no have enough weight
        {
            // So do nothing.
        }
        else if (testArmWeight == true) // Add the weight of the new weapon to the player's exisiting weight.
        {
            player1->addBackPackItem(comAd,lgBodyArm,false);
            comAd->playerFindsItem(player1,lgBodyArm,2); // Entering anything but one will not cause the combat admin to state the weight the player now is.
        }

        player1->autoEquip(machinePistol,lgBodyArm,comAd); // Equip the items, the player should be able to equip both (enough weight left, they should have 0 before finding these items).

        // Here, as the player should be able to get both items, it does not need to be tested which item's (if any) they got and inform them correctly on any items they did get as they should have got both.
        // The PIbot informs the player that the items have been equipped.
        piebot->pibotSay("You have automatically equipped both of those items, as something is better then nothing...",1);
        // The PIbot asks the player a question.
        piebot->pibotSay("So, want to know some stats for your new stuff? (Enter yes or no to confirm)",2);

        //Get and then check the answer the player gives.

        temp = getStringAnswer(temp);

        //The player says the answer they gave.

        player1->playerSay(temp);

        //Converted to upper case so the user could have entered the answer in whatever case structure they wanted to.

        temp = comAd->toUpperCase(temp);

        //Check the answer.

        if(temp == "YES") // If the player wants to know...
        {
            piebot->stateItemStats(machinePistol,lgBodyArm); // Show them the stats.
        }
        else if (temp == "NO") // If they do not though...
        {
            piebot->pibotSay("Well, guess you do not need to know then.",1); // Do not show them.
        }
        else // If the player enters in something else, perhaps have more options for yes and no in this if statement too.
        {
            piebot->pibotSay("It seems you don't really care...",1);
        }

        //Reset the answer temp.
        temp = "";

        // State the names of both the items the player just found.
        piebot->pibotSay("Hmm, that " + player1->getActiveWeapon()->getItemName() + " and " + player1->getActiveArmour()->getItemName() + " look useful...",1);
    }
    else if (forkAnswer == "RIGHT")
    {
        //The player has gone right.

        piebot->pibotSay("Hmm, looks like there is not alot around here.",1);
        piebot->pibotSay("Which usually means its quiet... too quiet.",1);
        piebot->pibotSay("Be on your guard...",1);

        isRandEncounter = comAd->isRandEncounter(); // Determine the chance for a random encounter.

        if (isRandEncounter == true)
        {
            // The player must fight.
            piebot->pibotSay("Well this looks rather ominous...",1);
            sound->stopAllSound(); // Stop sound here too.
            comAd->randomEncounter1V1(fists,player1,privMatt,sound,narrator,speedOfCombat,playerHasFoundScanner,mercsHired,playerIsInWinterbourne,playerHasSurgeNSquad,playerHasChangeEquipAbil,playerHasHealAbil,machinePistol,lgBodyArm,lightMG,hgBodyArm,handGrenades,clunkyPowerSuit,rocketLauncher,battleRifle,lgPowArm,modArm,fillWep,fillArm,fillWep,fillArm,fillWep,fillArm); // The fighting music is played here so does not need to be stopped.
            sound->playSoundLooped("Peaceful.wav");
            privMatt->resetValues(); // Reset this enemy after this combat(Remember to do this for all enemy pointers that will need to be reset if used again for example).
        }
        else if (isRandEncounter == false)
        {
            // The player gets lucky... this time.
            piebot->pibotSay("Hmm, thought there was someone here.",1);

        }

        // A short break here...

        narrator->narratorSay("PIbot and " + player1->getName() + " continue down the road, but PIbot stops again to look at something...",1);
        piebot->pibotSay("Hmm, I think there might be something here...",1);
        piebot->pibotSay("Oo, look " + player1->getName() + ", some coins!",1);

        // Give the player some coins

        player1->goldChange(200); // Hence positive gold change value here.
        comAd->playerFindsGold(player1->getName(), 200, player1->getGoldCount());

        piebot->pibotSay("Let us carry on... wait.. hang on a second...",1);

        isRandEncounter = comAd->isRandEncounter(); // Determine whether there is a random encounter.

        if (isRandEncounter == true)
        {
            // The player must fight again.
            piebot->pibotSay("hmmm....",1);
            sound->stopAllSound(); // Stop sound here too.
            comAd->randomEncounter1V1(fists,player1,privMatt,sound,narrator,speedOfCombat,playerHasFoundScanner,mercsHired,playerIsInWinterbourne,playerHasSurgeNSquad,playerHasChangeEquipAbil,playerHasHealAbil,machinePistol,lgBodyArm,lightMG,hgBodyArm,handGrenades,clunkyPowerSuit,rocketLauncher,battleRifle,lgPowArm,modArm,fillWep,fillArm,fillWep,fillArm,fillWep,fillArm);
            sound->playSoundLooped("Peaceful.wav");
            privMatt->resetValues(); // Reset this enemy after this combat.
        }
        else if (isRandEncounter == false)
        {
            // The player gets lucky... this time.
            piebot->pibotSay("Hmm, thought there was someone here too.",1);
        }
    }

    // PIbot comments on the enemies so far.
    piebot->pibotSay("I do wonder who that guy was at the beginning, was his name Sacramento? I cannot remember too well it seems...",1);
    piebot->pibotSay("Also, it seems there may be more than one of those other guys we may have been coming across, strange...",1);
    piebot->pibotSay("Hmm... I think I hear something again, wait up a second...",1);
    // No matter whether the player goes left or right, a random encounter occurs here.

    isRandEncounter = comAd->isRandEncounter(); // Determine whether this happens or not.

    if (isRandEncounter == true)
    {
        //The player must fight again.
        piebot->pibotSay("hmmm....",1);
        sound->stopAllSound(); // Stop sound here too.
        comAd->randomEncounter1V1(fists,player1,privMatt,sound,narrator,speedOfCombat,playerHasFoundScanner,mercsHired,playerIsInWinterbourne,playerHasSurgeNSquad,playerHasChangeEquipAbil,playerHasHealAbil,machinePistol,lgBodyArm,lightMG,hgBodyArm,handGrenades,clunkyPowerSuit,rocketLauncher,battleRifle,lgPowArm,modArm,fillWep,fillArm,fillWep,fillArm,fillWep,fillArm);
        sound->playSoundLooped("Peaceful.wav");
        privMatt->resetValues(); // Reset this enemy after this combat.
    }
    else if (isRandEncounter == false)
    {
        // The player gets lucky... this time.
        piebot->pibotSay("Hmm, thought there was someone here too.",1);
    }

    // Rounding off chapter 0 (Prolouge).
    narrator->narratorSay("This concludes Chapter 0 (Prolouge) of our epic(once again, soon...) tale.",1);
    narrator->narratorSay("Keep in mind though, that it does not matter which way our main character;"
                           + player1->getName() + ", has gone. For usually, all paths must merge sooner... or later.",1);
    narrator->narratorSay("Our tale will resume in Chapter 1 (Going Places), stay informed...",1);

    narrator->narratorSay("As this is merely a demonstration, you cannot continue on, get the full version to see what will happen, should it become available.",1); // Tell the player they need the full version to carry on.
    narrator->narratorSay("As such, the credits will be displayed here, but thank-you for participating in this section of this story though.",1); // Tell the player that the credits will be displayed.

    narrator->rollCredits(speedOfCredits,false); // Roll the end credits(Hence false for the last param in the func call), just before the clean-up phase and the program's termination.
    // Remember to change vars to the correct values in debug mode or, for example, the function just above will run, but do nothing. KEEP EYES ON THIS!
    // Handle deletion of objects, doing it like this for now, when the program is about to end.
    delete sound;
    sound = 0;

    delete narrator;
    narrator = 0;

    delete piebot;
    piebot = 0;

    delete player1;
    player1 = 0;

    delete comAd;
    comAd = 0;

    delete gestMerchant;
    gestMerchant = 0;

    delete privMatt; // Priv matt now needs deletion as well.
    privMatt = 0;

    return 0; // The program ends here.
}

string forkInTheRoad(string answer,Player *player,Narrator *narrator,Pibot *piebot) // Find out which way the player choses to go... or not...
{
    double piebotForkAnswer = rand() % 100; // What PIbot will choose if the player gives an odd answer for this question.

    if (answer == "LEFT") // Announce that they shall head left.
    {
        piebot->pibotSay("Very well, we shall head left.",1);
        narrator->narratorSay("So " + player->getName() + " and PIbot went left in the fork in the road, where a welcome suprise was waiting....",1);
    }
    else if (answer == "RIGHT") // Announce that they shall head right.
    {
        piebot->pibotSay("Very well, we shall head right.",1);
        narrator->narratorSay("So " + player->getName() + " and PIbot went right in the fork in the road, where a not so welcome suprise was waiting....",1);
    }
    else // Pibot choses for the player.
    {
        piebot->pibotSay("If you're not going give me a proper answer I shall choose for you.",1); // PIbot chooses randomly.
        if (piebotForkAnswer > 50) // Announce that they shall head left.
        {
            answer = "LEFT";
            piebot->pibotSay("We shall head left.",1);
            narrator->narratorSay("So " + player->getName() + " and PIbot went left in the fork in the road, where a welcome suprise was waiting....",1);
        }
        else if (piebotForkAnswer <= 50) // Announce that they shall head right.
        {
            answer = "RIGHT";
            piebot->pibotSay("We shall head right.",1);
            narrator->narratorSay("So " + player->getName() + " and PIbot went right in the fork in the road, where a not so welcome suprise was waiting....",1);
        }
    }

    return answer;
}

void seedRandom()
{
    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}

string getStringAnswer(string temp) // Get an answer as a string, e.g. the player's name.
{
    do // A do while loop for added confirmation of entry.
    {
        getline(cin, temp);
    } while (temp == "");

    return temp;
}
