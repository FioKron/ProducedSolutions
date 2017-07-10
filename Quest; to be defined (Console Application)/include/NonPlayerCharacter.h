#ifndef NONPLAYERCHARACTER_H
#define NONPLAYERCHARACTER_H

#include <string> // Need this line or it complains
#include <iostream> // For saying things for example
#include <windows.h> // For the sleep function

// Add more when required.
using namespace std;

class NonPlayerCharacter
{
    public:
        NonPlayerCharacter(); // default constructor
        string getName(); // Get the name of this char, remember too that some of the other functions are used by NPC's so they are here as this is a parent class so to speak.
        string getStringAnswer(string temp); // Get an answer as a string, e.g. the player's name.
        void characterSay(string sayWhat, int msgType); // This char says something.
        string intToString(int number);
        string doubleToString(double number);
        int stringToNumber(string conversionString);
    protected:
        string name; // The char's name.
    private:
};

#endif // NONPLAYERCHARACTER_H
