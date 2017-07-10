#ifndef MERCHANT_H
#define MERCHANT_H

#include <string> // Need this line or it complains
#include <iostream> // For saying things for example
#include <NonPlayerCharacter.h> // As the merchant is a non player character
#include <vector>
#include <Item.h>
#include <Armour.h>
#include <Weapon.h>

// Add more when required.
using namespace std;

class Player;
class CombatAdmin;

class Merchant : public NonPlayerCharacter
{
    public: // Prototype functions
        Merchant(string newName);// Default constructor
        string handleResponse(Player *player,CombatAdmin *comAd,string answer,Weapon *unArmed,Armour *unArmoured); // Handle the response the player gives.
        void addItem(Item *thisItem);
        void addWeapon(Weapon *thisWeapon);
        void addArmour(Armour *thisArmour);
        string getInventoryItemName(int index);
        double getInventoryItemCost(int index);
        void removeItem(int index); // Remove one of the merchants item's from the specific slot.
    protected:
        string buy(Player *player,CombatAdmin *comAd,Weapon *unArmed,Armour *unArmoured); // The merchant buys things from the player
        void sell(Player *player,CombatAdmin *comAd,int sellStage); // The merchant sells things to the player
        vector<Item *> inventory; // The merchants collection of Items.
        vector<Weapon *> weapons; // The merchant's colletion of weapons.
        vector<Armour *> armour; // The merchant's colletion of armour.
    private:
};

#endif // MERCHANT_H
