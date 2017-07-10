#include <Pibot.h>
#include <iostream> // Required for input and output
#include <windows.h> // Needed to use the sleep function.
#include <Sound.h>
#include <Enemy.h>
#include <CombatAdmin.h>
#include <sstream> // For converting between types.
#include <iomanip> // For manipulating what the value conversion will be precise too etc.

Pibot::Pibot()
{
    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.

}

void Pibot::pibotSay(string sayWhat,int msgType) // Say something.
{
   switch (msgType) // Check what type of message will be said.
    {
        case 1: // Standard PIbot message.
        {
            cout << "PIbot:" + sayWhat << endl; // Print the "PIbot:" tag and then what PIbot says.
            Sleep(3000); // Wait.
            break; // Break out of switch.
        }

        case 2: // This type of message is said when PIbot wants input from the user.
        {
            cout << "PIbot:" + sayWhat; // Still print a message to the console with the PIbot tag, but without endl so the user input will be on the same line
            break; // Break out of switch.
        }
    }
}

void Pibot::combatTut(Player *player,Sound *sound,CombatAdmin *comAd,Narrator *narrator,int combatSpeed) // Combat tutorial as directed by PIbot.
{
    string temp; // Holds the player's choice in combat.

    sound->playSoundLooped("Fighting.wav"); // This plays the fighting music.

    // Instance Sacramento with the standard constructor.
    Enemy *sacramento = new Enemy();

    comAd->comAdWarning(sacramento->getName(),"",1,combatSpeed);

    sacramento->gotAggro();

    // NOTE: if you see "Some guy" or similar, then it should be sacramento, should be just in comments at least...
    // PIbot has a dialogue with some guy for a bit while "Sacramento" stalls.
    pibotSay("Well thanks for warning us combat admin old fellow, old pal (not really heh).",1);
    pibotSay("But Argh, who are you?",1);
    sacramento->enemySay("Quiet you! I'm here to kill " + player->getName() + "!");
    sacramento->enemySay("I got the armour I need, you will never guess my one little weakness!");
    pibotSay("Well " + player->getName() + ", looks like you're going to have to take this lovely person down a notch!",1);
    pibotSay("Well it seems there are quite a few items lying around... (strange too but useful)",1);
    pibotSay("Looking around, it looks like you can do a number of things:",1);
    pibotSay("1: Throw a brick at him.",1);
    pibotSay("2: Run away.",1);
    pibotSay("3: Use a grenade.(On him)",1);
    pibotSay("4: Scan the enemy.",1);
    pibotSay("So, what is it going to be," + player->getName() + ", 1, 2, 3 or 4? (just enter the number of the option you want,1,2 etc, or one,two or even One,TwO etc)",2);

    do // When the player enters an option (or nothing really of the value) the system uses getline to get the line using cin and the temp varible as a store.
    {
        getline(cin, temp);
    } while(temp == "");

    player->playerSay(temp);

    temp = comAd->toUpperCase(temp);

        if (temp == "1" || temp == "ONE")
    {
        // As it is the combat tutorial, weapons and armour are not yet used, a bit of hard coding was done here...
        comAd->comAdAtkNote("Brick",200,sacramento->getName(),player->getName(),combatSpeed);
        sacramento->healthChange(-200);
        comAd->comAdStateEntHp(sacramento->getName(), sacramento->getHealth(),combatSpeed);
        sacramento->enemyDeath();
        comAd->entDefeated(sacramento->getName(),combatSpeed);

        sound->stopAllSound();
        //Fight music stops and the fight outro plays.
        sound->playSoundNonLooped("Fightend.wav");

        pibotSay("Well done! You sure showed him.",1);
        pibotSay("I don't think you will need that brick again, so we will just leave it here.",1);
    }
    else if(temp == "2" || temp == "TWO")
    {
        sacramento->enemySay("Not so fast " + player->getName() + "!");
        comAd->comAdAtkNote("Hunting Shotgun",100,player->getName(),sacramento->getName(),combatSpeed);
        player->healthChange(-100);
        comAd->comAdStateEntHp(player->getName(), player->getHealth(),combatSpeed);
        comAd->entDefeated(player->getName(),combatSpeed);
        sacramento->enemyTaunt();

        sound->stopAllSound();
        //Fight music stops and the fight outro plays.
        sound->playSoundNonLooped("Fightend.wav");

        narrator->narratorSay("Dead",3); // The player has met a loss condition.
    }
    else if(temp == "3" || temp == "THREE")
    {
        comAd->comAdAtkNote("Grenade", 199, sacramento->getName(),player->getName(),combatSpeed);
        sacramento->healthChange(-199);
        comAd->comAdStateEntHp(sacramento->getName(),sacramento->getHealth(),combatSpeed);

        sacramento->enemySay("Argh!! Screw this!");

        comAd->comAdSay("It seems the guy has just teleported away...fight over.",1);
        pibotSay("Hmm, seems that guy has got away...ah well.",1);

        sound->stopAllSound();
        //Fight music stops and the fight outro plays.
        sound->playSoundNonLooped("Fightend.wav");
    }
    else if(temp == "4" || temp == "FOUR")
    {
        comAd->comAdStateScanResults(sacramento->getName(),sacramento->getHealth(),combatSpeed); // State the scan results for this tutorial.
        pibotSay("Hmm, well that's all good and well " + player->getName() + " ,but I think you should do something! (Out of 1,2 or 3 now, enter the number of the option you want or one,TwO etc)",2);

        temp = "";

        do
        {
            getline(cin, temp);
        } while(temp == "");

        player->playerSay(temp);

        temp = comAd->toUpperCase(temp);

        if (temp == "1" || temp == "ONE")
        {
            comAd->comAdAtkNote("Brick",200,sacramento->getName(),player->getName(),combatSpeed);
            sacramento->healthChange(-200);
            comAd->comAdStateEntHp(sacramento->getName(), sacramento->getHealth(),combatSpeed);
            sacramento->enemyDeath();
            comAd->entDefeated(sacramento->getName(),combatSpeed);

            sound->stopAllSound();
            //Fight music stops and the fight outro plays.
            sound->playSoundNonLooped("Fightend.wav");


            pibotSay("Well done! You sure showed him.",1);
            pibotSay("I don't think you will need that brick again, so we will just leave it here.",1);
        }
        else if(temp == "2" || temp == "TWO")
        {
            sacramento->enemySay("Not so fast " + player->getName() + "!");
            comAd->comAdAtkNote("Hunting Shotgun",100,player->getName(),sacramento->getName(),combatSpeed);
            player->healthChange(-100);
            comAd->comAdStateEntHp(player->getName(), player->getHealth(),combatSpeed);
            comAd->entDefeated(player->getName(),combatSpeed);
            sacramento->enemyTaunt();

            sound->stopAllSound();
            //Fight music stops and the fight outro plays.
            sound->playSoundNonLooped("Fightend.wav");

            narrator->narratorSay("Dead",3); // The player has met a loss condition.

        }
        else if(temp == "3" || temp == "THREE")
        {
            comAd->comAdAtkNote("Grenade", 199, sacramento->getName(),player->getName(),combatSpeed);
            sacramento->healthChange(-199);
            comAd->comAdStateEntHp(sacramento->getName(),sacramento->getHealth(),combatSpeed);

            sacramento->enemySay("Argh!! Screw this!");

            comAd->comAdSay("It seems the guy has just teleported away...fight over.",1);
            pibotSay("Hmm, seems that guy has got away...ah well.",1);

            sound->stopAllSound();
            //Fight music stops and the fight outro plays.
            sound->playSoundNonLooped("Fightend.wav");
        }
        else
        {
            comAd->comAdSay("ERROR: not a valid command!",1);
            pibotSay("What are you doing " + player->getName() + "?!",1);
            sacramento->enemyTaunt();
            comAd->comAdAtkNote("Hunting Shotgun",100,player->getName(),sacramento->getName(),combatSpeed);
            player->healthChange(-100);
            comAd->comAdStateEntHp("Player", player->getHealth(),combatSpeed);
            comAd->entDefeated(player->getName(),combatSpeed);
            sacramento->enemyTaunt();

            sound->stopAllSound();
            //Fight music stops and the fight outro plays.
            sound->playSoundNonLooped("Fightend.wav");

            narrator->narratorSay("Dead",3); // The player has met a loss condition.
        }

    }
    else
    {
        comAd->comAdSay("ERROR: not a valid command!",1);
        pibotSay("What are you doing " + player->getName() + "?!",1);
        sacramento->enemyTaunt();
        comAd->comAdAtkNote("Hunting Shotgun",100,player->getName(),sacramento->getName(),combatSpeed);
        player->healthChange(-100);
        comAd->comAdStateEntHp("Player", player->getHealth(),combatSpeed);
        comAd->entDefeated(player->getName(),combatSpeed);
        sacramento->enemyTaunt();

        sound->stopAllSound();
        //Fight music stops and the fight outro plays.
        sound->playSoundNonLooped("Fightend.wav");

        narrator->narratorSay("Dead",3); // The player has met a loss condition.
    }

    // Maybe make it so sacramento is not deleted here...decide later(e.g. if hit with a grenade they may come back, and with spec abils too,hmm...)...
    delete sacramento;
    sacramento = 0;
}

void Pibot::handleFirstQAndASess(Player *player,CombatAdmin *comAd) // Handle the player's first question and answer session.
{ // Need a comAd pointer for casting the question to upper case... or anything else the player gave.
    string temp = ""; // Make a temp var here for getting the player's answer.

    pibotSay("It is question time!",1); // Introduce it.

    pibotSay("I will answer 3 questions you have.",1); // Tell them about how it works.
    pibotSay("These can include: 'Why are you following me?' and 'What year is it?' etc, think up something to ask me and I will give you an answer hopefully.",1); // The PIbot then asks them.
    pibotSay("(Enter it like I showed you above, or just type 'leave' to exit this session of Q and A.)",2); // Confirm how to give a correct answer.
    // Get the first answer.
    do // Changed to a do while loop for added confirmation of entry.
    {
        getline(cin, temp);
    } while (temp == "");

    //The player says the answer they gave.
    player->playerSay(temp);

    // Converted to upper case so the user could have entered the answer in whatever case structure they wanted to.
    temp = comAd->toUpperCase(temp);

    int questionsLeftToAsk = 3; // As they can ask 3 questions, hence 3 for this var.

    while (questionsLeftToAsk > 0) // As long as they can ask some more questions...
    {
        questionsLeftToAsk --; // Decrement the questions left to ask.
        string strQuesLeft = numberToString(questionsLeftToAsk); // Perform a conversion.

        pibotSay("You have " + strQuesLeft + " question(s) left you can ask me " + player->getName() + "(or if you just want to leave, we shall now do so).",1); // The PIbot confirms how many questions left you can ask.

        if (temp == "WHY ARE YOU FOLLOWING ME?" || temp == "WHY ARE YOU FOLLOWING ME") // These questions must be entered in as per here though...hmm...
        {
            pibotSay("Simple, beacuase I need a new master and so does that combat admin thing...I think...",1);
        }
        else if (temp == "WHAT YEAR IS IT?" || temp == "WHAT YEAR IS IT")
        {
            pibotSay("Ooo, I am not sure, I think it must be at least the 25th centaury or so, maybe a bit later as I was made at the end of the 25th.",1); // The PIbot sort of confirms the time this game is set...
        }
        else if (temp == "WHERE ARE WE GOING?" || temp == "WHERE ARE WE GOING")
        {
            pibotSay("We are heading to a place known as Winterbourne, I just need to remember how to get their...good place though.",1);
        }
        else if (temp == "LEAVE") // The player wants to end this Q and A session.
        {
            pibotSay("Ok, we will end it here then.",1);
            questionsLeftToAsk = 0; // Make it so they have no questions left to ask.
        }
        else // The player gives an invalid question.
        {
            pibotSay("Hmm, I do not know how to answer that... if it is even a question...",1); // The PIbot is a bit confused...
        }
        // Add more Q's and A's when needed for later on!

        if (questionsLeftToAsk > 0) // Only allow the player to ask more questions if they have them left to ask.
        {
            pibotSay("Ok, you can ask even more questions...remember...",1); // Tell them about how it works again.
            pibotSay("These can include: 'Why are you following me?' and 'What year is it?' etc, think up something to ask me and I will give you an answer hopefully.",1); // The PIbot then asks them.
            pibotSay("(Enter it like I showed you above, or just type 'leave' to exit this session of Q and A.)",2); // Confirm how to give a correct answer.
            // Get the next few answers.
            do // Changed to a do while loop for added confirmation of entry.
            {
                getline(cin, temp);
            } while (temp == "");

            //The player says the answer they gave.
            player->playerSay(temp);

            // Converted to upper case so the user could have entered the answer in whatever case structure they wanted to.
            temp = comAd->toUpperCase(temp);
        }
    }
}

void Pibot::stateItemStats(Weapon *weapon, Armour *armour) // State the stats of some items the player finds, the player is only told about jam and ammo mechanics to some degree if they want to know.
{
    pibotSay("Well as I am a personal intelligence robot, I have some of which to show you about what you just found...",1);

    // Perform the conversions.
    string damage = numberToString(weapon->getDamage());
    string speed = numberToString(weapon->getSpeed());
    string wepWeight = numberToString(weapon->getItemWeight());
    string wepValue = numberToString(weapon->getItemCost());
    string ammoRemaining = numberToString(weapon->getAmmoCnt());
    string dmgResist = numberToString(armour->getDmgResist() * 100); // Get the value as a resitance %.
    string spdModifier = numberToString(armour->getAttSpdMod() * 100); // same here too.
    string armWeight = numberToString(armour->getItemWeight());
    string armValue = numberToString(armour->getItemCost());
    string jamable = ""; // Set later for whether the weapon is jamable or not.

    pibotSay("Weapon:",1); // State some of the stats of the weapon...
    pibotSay("Name: " + weapon->getItemName() + ".",1);
    pibotSay("Damage: " + damage + " points.",1);
    pibotSay("Speed: " + speed + " (This is relativly how fast the weapon is, the higher the number, the better).",1);
    pibotSay("Weight: " + wepWeight + " kgs.",1);
    pibotSay("Value: " + wepValue + " gold peices.",1);
    pibotSay("Current ammo remaning: " + ammoRemaining + " (if this weapon runs out of ammo you will have to wait to equip another you find/buy, but soon you will be able to change equipment in combat).",1);
    pibotSay("You can go un-armed soon too if you have no weapon spare.",1);

    if (weapon->getIsJamable() == true) // Get whether the weapon is jamable or not.
    {
        jamable = "it is.";

    }
    else if (weapon->getIsJamable() == false)
    {
        jamable = "it is not.";
    }

    pibotSay("Is this weapon jamable: " + jamable + " (If this weapon is jamable it will not work for a turn of combat if it jams, but only one turn mind, else it will never have this downside).",1);

    pibotSay("Armour:",1); // And the armour...
    pibotSay("Name: " + armour->getItemName() + ".",1);
    pibotSay("Damage resistance: " + dmgResist + " % resistance to damage.",1); // Get the resitance as a percentage and not the decimal value.
    pibotSay("Speed modifier: " + spdModifier + " % modifier to your weapon's speed (the lower the number the better).",1);
    pibotSay("Weight: " + armWeight + " kgs.",1);
    pibotSay("Value: " + armValue + " gold peices.",1);
}

void Pibot::stateArmStats(Armour *armour)
{
    pibotSay("Hmm, looking at this armour...",1);

    // Perform the conversions.
    string dmgResist = numberToString(armour->getDmgResist() * 100); // Get the value as a resitance %.
    string spdModifier = numberToString(armour->getAttSpdMod() * 100); // same here too.
    string armWeight = numberToString(armour->getItemWeight());
    string armValue = numberToString(armour->getItemCost());

    pibotSay("Name: " + armour->getItemName() + ".",1); // State the stats of this armour.
    pibotSay("Damage resistance: " + dmgResist + " % resistance to damage.",1); // Get the resitance as a percentage and not the decimal value.
    pibotSay("Speed modifier: " + spdModifier + " % modifier to your weapon's speed (the lower the number the better).",1);
    pibotSay("Weight: " + armWeight + " kgs.",1);
    pibotSay("Value: " + armValue + " gold peices.",1);
}

void Pibot::stateWepStats(Weapon *weapon) // Perhaps say how the player can go un-armed if they want?
{
    pibotSay("Hmm, looking at this weapon...",1);

    string damage = numberToString(weapon->getDamage()); // Perform the conversions.
    string speed = numberToString(weapon->getSpeed());
    string wepWeight = numberToString(weapon->getItemWeight());
    string wepValue = numberToString(weapon->getItemCost());
    string ammoRemaining = numberToString(weapon->getAmmoCnt());
    string jamable = ""; // Set later for whether the weapon is jamable or not.

    pibotSay("Name: " + weapon->getItemName() + ".",1); // State the stats of the weapon.
    pibotSay("Damage: " + damage + " points.",1);
    pibotSay("Speed: " + speed + " (This is relativly how fast the weapon is, the higher the number, the better).",1);
    pibotSay("Weight: " + wepWeight + " kgs.",1);
    pibotSay("Value: " + wepValue + " gold peices.",1);
    pibotSay("Current ammo remaning: " + ammoRemaining + " (if this weapon runs out of ammo you will have to wait to equip another you find/buy, but soon you will be able to change equipment in combat).",1);
    pibotSay("You can go un-armed soon too if you have no weapon spare.",1);

    if (weapon->getIsJamable() == true) // Get whether the weapon is jamable or not.
    {
        jamable = "it is.";

    }
    else if (weapon->getIsJamable() == false)
    {
        jamable = "it is not.";
    }

    pibotSay("Is this weapon jamable: " + jamable + " (If this weapon is jamable it will not work for a turn of combat if it jams, but only one turn mind, else it will never have this downside).",1);
}

string Pibot::numberToString(int number)
{
    ostringstream oss; // Converts from number to string.

    // Perform the conversion and return the results.
    oss << number;

    return oss.str();
}

void Pibot::manipTest(string answer,Player *player) // The manipulation test the PIbot can do.
{

    string manipTest; // The string to be manipulated.

    if (answer =="NO") // Depending on the player's answer...
    {
        pibotSay("Very well, guess it was not so cool after all.",1);
    }
    else if (answer == "YES")
    {
        pibotSay("Good, just a small test of my text manipulation techniques",1);
        pibotSay("Give me a word or phrase to manipulate if you don't mind:",2);

        while(manipTest == "")
        {
            getline(cin, manipTest);
        }

        player->playerSay(manipTest);

        pibotSay("Right I will use '" + manipTest + "' as my word/phrase for this test.",1);
        //Removed the manipulation for now, need to learn how to do it in C++ properly.

        pibotSay("Hmm interesting, this comes out as " + manipTest + ".",1);
        pibotSay("Whoops, that is exactly the same, guess I'm not functioning properly.",1);
        pibotSay("Ok, maybe that was not THAT interesting, but I'll tell you what I was going to tell you earlier before that moron attacked us.",1);
        pibotSay("The tip I want give to you is that from the looks of it, we should head left in the road up ahead.",1);

    }
    else
        pibotSay("Fine, if you're not going to give me a real answer, we can just keep on moving.",1);
}


