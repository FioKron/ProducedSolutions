#ifndef NARRATOR_H
#define NARRATOR_H
#include <string> // Need this line or it complains

using namespace std; // See main.cpp

class Narrator // Narrator does not need a constructor at this moment in time and only has one function
{
    public:
        Narrator();
        void narratorSay(string sayWhat,int msgType); // Prints a msg to the console which varies depending on its type.
        void rollCredits(int creditSpeed,bool isOpeningCredits); // Roll the credits, last param is for checking whether these credits are at the end or not.
        void checkChpt0EndAns(string answer,int creditSpeed); // Check the answer by the player as to if they want to carry on with the story or not.
};

#endif // NARRATOR_H
