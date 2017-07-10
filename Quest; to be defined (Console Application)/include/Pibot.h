#ifndef PIBOT_H
#define PIBOT_H
#include <string> // Need this line or it complains
#include <Player.h>
#include <Sound.h>
#include <CombatAdmin.h>
#include <Narrator.h>

using namespace std; // See main.cpp.

class Pibot
{
    public:
        Pibot();
        void pibotSay(string sayWhat,int msgType); // Prints a msg to the console depending on the type of message.
        void combatTut(Player *player,Sound *sound,CombatAdmin *comAd,Narrator *narrator,int combatSpeed); // Combat tutorail as directed by PIbot, the combat admin handles all other combat.
        void manipTest(string Answer,Player *player); // "Test" of PIbot's manipulation techniques.
        void stateItemStats(Weapon *weapon, Armour *armour); // State the stats of both a piece of armour and a weapon.
        void stateWepStats(Weapon *weapon);
        void stateArmStats(Armour *armour);
        string numberToString(int number);
        void handleFirstQAndASess(Player *player,CombatAdmin *comAd); // Handle the Q and A session so to speak.
};

#endif // PIBOT_H
