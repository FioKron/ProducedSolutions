#include <NonPlayerCharacter.h>

#include <string> // Need this line or it complains
#include <iostream> // For saying things for example
#include <windows.h> // For the sleep function
#include <time.h> // Needed for seeding the random number function.
#include <sstream> // For converting between types.
#include <iomanip> // For manipulating what the value conversion will be precise too etc.

// Add more when required.

NonPlayerCharacter::NonPlayerCharacter()
{
    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}

string NonPlayerCharacter::intToString(int number) // It seems that this still works on doubles. This function also rounds the values to the nearest whole number it seems.
{
    ostringstream oss; // Converts from number to string.

    // Perform the conversion and return the results.
    oss << number;

    return oss.str();
}

string NonPlayerCharacter::getStringAnswer(string temp)
{
    do
    {
        getline(cin, temp);
    } while(temp == ""); // Get input

    return temp;
}

string NonPlayerCharacter::doubleToString(double number) // Required for proper double precision.
{
    ostringstream oss; // Converts from number to string.

    // Perform the conversion and return the results.
    oss << fixed << setprecision(2) << number;

    return oss.str();
}

int NonPlayerCharacter::stringToNumber(string convert) // Convert a string to a number.
{
    istringstream iss(convert); // Do it as an int though.
    int result;

    iss >> result;
    return result;
}

void NonPlayerCharacter::characterSay(string sayWhat, int msgType)
{
    switch (msgType) // Check what type of message will be said.
    {
        case 1: // Standard message.
        {
            cout << name + ":" + sayWhat << endl; // Print the "NPCname:" tag and then what the NPC says where name is the name of the NPC.
            Sleep(3000); // Wait.
            break; // Break out of switch.
        }

        case 2: // This type of message is said when the NPC wants input from the user.
        {
            cout << name + ":" + sayWhat; // Still print a message to the console with the NPCname's tag, but without endl so the user input will be on the same line
            break; // Break out of switch.
        }

        case 3: // The NPC character prints the text without ending the line (so more text can be printed on the same line for example).
        {
            cout << name + ":" + sayWhat; // Print the name tag and what it says.
            //Sleep(2000); // Wait.
            break;
        }

        case 4:// The NPC character prints the text but does end the line, just prints it without the NPC character's name tag.
        {
            cout << sayWhat << endl; // Does not print the characters name tag but does print what it says.
            Sleep(500); // Wait, but for much less so if the items the player can buy be listed they can be listed quickly.
            break;
        }
    }

}

string NonPlayerCharacter::getName()
{
    return name;
}
