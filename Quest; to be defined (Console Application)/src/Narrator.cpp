#include <Narrator.h> // Need the narrator header file.
#include <iostream> // Required for input and output
#include <windows.h> // Needed to use the sleep function.
#include <time.h>
Narrator::Narrator()
{
    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}

void Narrator::narratorSay(string sayWhat,int msgType) // Say a message of a certain type.
{
    switch (msgType) // Check what type of message will be said.
    {
        case 1: // Standard narrator message.
        {
            cout << "Narrator:" + sayWhat << endl; // Print the "Narrator:" tag and then what the narrator says.
            Sleep(3000); // Wait.
            break; // Break out of switch.
        }

        case 2: // Messages of this type are only said at the start for now, for a fake "Starting up" sequence to set the scene.
        {
            cout << sayWhat << endl; // Just print the "pure" message to the console.
            Sleep(3000); // Then wait.
            break; // Break out of switch.
        }

        case 3: // Said if the player dies or otherwise meets a loss condition.
        {
            cout << "Narrator:So the story of our brave character comes to an end, who knows what they would have discovered if they had been successful...." << endl; // Print the player loss message.
            Sleep(15000); // Then wait so the player can read it before the program ends.
            exit(0); // End the program.
            break; // Break out of switch.
        }

        case 4: // Messages of this type are said when the player is asked a question by the narrator (rare, usually at the end of the chapter).
        {
            cout << sayWhat; // Just print the pure message to the console but do not start a new line (unlike in type 2 where a new line is started, this is so the answer can be on the same line).
            Sleep(3000); // Then wait.
            break; // Break out of switch
        }
        case 5: // Messages of this type are only said when the credits are being displayed.
        {
            cout << sayWhat << endl; // Just print the "pure" message to the console.
            Sleep(500); // Then wait (but for less time).
            break; // Break out of switch.
        }
    }

}

void Narrator::checkChpt0EndAns(string answer,int creditSpeed) // Check
{
    if (answer == "YES" || answer == "SURE" || answer == "OK") // If the player has entered yes or sure or even ok in any possible way (e.g. Yes or YEs etc for yes) return to the main sub.
    {
       // Do nothing here, intro when we go back into the main sub.
    }
    else // If it is anything else (such as no) then exit the program after giving an ending.
    {
        // The narrator teases the player so to speak.
        narratorSay("So this ends the tale of our lovely character, who knows what would had happened if they carried on...",1);
        rollCredits(creditSpeed,false); // As this is technically the end credits display time here too.
        exit(0);  // Terminate program after this,may need to do checks in main so pointers can be disposed of properly.
    }
}

void Narrator::rollCredits(int creditSpeed,bool isOpeningCredits) // Roll the credits. The parameter is the speed it should be, either type 2 or 5 msgtype (3000 ms for type 2 and 500 ms for type 5, so relativly slow or fast).
{ // The bool var checks whether it is the opening credits or not.

    if (isOpeningCredits == true) // If it is the opening credits...
    {
        narratorSay("OPENING CREDITS:",creditSpeed); // Say so.
        narratorSay("These credits will also be displayed at the end of this application.",creditSpeed); // Say they will be displayed at the end of the game too, if the player gets that far...
    }
    else if (isOpeningCredits == false) // If it is not the opening credits and thus the ending credits...
    {
        narratorSay("ENDING CREDITS:",creditSpeed); // Say so.
    }

    // Then state all the credits.
    narratorSay("Programming by: James Andrew Moran.",creditSpeed);
    narratorSay("Design by: James Andrew Moran.",creditSpeed);
    narratorSay("Testing by: James Andrew Moran.",creditSpeed);
    narratorSay("Icon artwork by: James Andrew Moran.",creditSpeed);
    // This line was in the code, but now I consider just commincating JAM to be enough, hence this line was left out, but may be added back in...narratorSay("To confirm; James Andrew Moran is from Newbury, Berkshire, England, this was communicated to avoid confusion with other James Andrew Moran's, therefore, it was this person who has been credited as above.",creditSpeed);
    narratorSay("Additional testing by: Edward Gustavsson,Mikkel P. F. Laursen,Michael Walker",creditSpeed);
    narratorSay(",Joeri Smits and Catherine Moran.",creditSpeed);
    narratorSay("Writing and proofreading assistance by: Catherine Moran.",creditSpeed);
    narratorSay("Resources used:",creditSpeed);
    narratorSay("Integrated Development Enviroment: Code::Blocks.",creditSpeed);
    narratorSay("This program and the Integrated Development Enviroment use the GNU GENERAL PUBLIC LICENSE version 3, 29 June 2007.",creditSpeed);
    narratorSay("To see what this licence entails, please go to http://www.codeblocks.org/license/3.",creditSpeed);
    narratorSay("As such, the code for this program can be edited, find the code in the redist folder where it was installed.",creditSpeed);
    narratorSay("Sound editing: Audacity.",creditSpeed);
    narratorSay("http://www.cplusplus.com/reference/cstdlib/rand/ for generating random numbers by properly seeding the random number generator using the time.",creditSpeed);
    narratorSay("C++ All-In-One For Dummies.",creditSpeed);
    narratorSay("http://stackoverflow.com",creditSpeed);
    narratorSay("Explore 7: by INON ZUR.",creditSpeed); // Change the music credits when I get the new music (by Jimmy from sweden, friend of Sarge).
    narratorSay("Battle 2 and finale:Bethesda Softworks (Unknown artist)",creditSpeed);
    narratorSay("Quest: to be defined is copyright 2013 of James Andrew Moran. All rights reserved.",creditSpeed); // Change this credit to have the name of the company and not just me.
}
