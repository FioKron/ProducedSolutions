#ifndef HEALTHANDAMMOSUPPILER_H
#define HEALTHANDAMMOSUPPILER_H

#include <string> // Need this line or it complains
#include <iostream> // For saying things for example
#include <NonPlayerCharacter.h> // As the merchant is a non player character
#include <vector>
#include <Item.h>
#include <Armour.h>
#include <Weapon.h>

class HealthAndAmmoSuppiler : public NonPlayerCharacter // As this char is a NPC so to speak.
{
    public:
        HealthAndAmmoSuppiler(string newName); // Default constructor.
        void handleResponse(Player *player,CombatAdmin *comAd,string answer); // Handle the response the player gives.
    protected: // All of the funcs below are protected as they are only called when the player gives an answer as to what they want to do.
        int costPerAmt; // The cost per the amount to heal.
        int amtPerCost; // The amount of HP to heal per gold count.
        void healPlayer(Player *player,CombatAdmin *comAd); // All of these functions take gold from the player if the player wants this supiler's services, heal the player.
        void increasePlayerMaxHP(Player *player,CombatAdmin *comAd); // Increase the player's max HP.
        void givePlayerWepAmmo(Player *player,CombatAdmin *comAd); // This function gives the player ammo if they need it (e.g. They already have full ammo or their weapon does not use it this guy will not.).
        void increasePlayerWeightLim(Player *player,CombatAdmin *comAd); // Increase the player's max weight lim.
        bool hpAlreadyIncresed; // Whether the player has already increased their HP or not.
        bool weightAlreadyInc; // Whether the player has already increased their weight lim or not.
    private:
};

#endif // HEALTHANDAMMOSUPPILER_H
