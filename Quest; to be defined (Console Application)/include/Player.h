#ifndef PLAYER_H
#define PLAYER_H
#include <string> // Need this line or it complains

 // Header files that I have made for my classes are needed for this one
#include <Weapon.h>
#include <Armour.h>
#include <CombatAdmin.h>
#include <Item.h>
#include <vector> // For the player's backpack.

class Weapon; // Errors are thrown if this line is not here, same for combat admin
class CombatAdmin;

using namespace std;

class Player
{
    public:
        Player(Weapon *unArmed,Armour *unArmoured); // Default player constructor
        void autoEquip(Weapon *weapon,Armour *armour,CombatAdmin *comAd); // Give the player an active weapon and armour set out of any weapons and armour that have been instanced.
        bool equipArmour(Armour *armour,CombatAdmin *comAd);
        bool equipWeapon(Weapon *weapon,CombatAdmin *comAd);
        Weapon *getActiveWeapon(); // generic getx functions for accesors to protected members.
        Armour *getActiveArmour();
        double getUsedBackPackSlots();
        double getMaxBackPackSlots();
        void healthChange(double newHealth); // The player's health changes.
        void playerSay(string sayWhat); // The player says something.
        double getHealth();
        void goldChange(double newGold); // The player's gold amount changes.
        double getGoldCount();
        bool weightChange(double newWeight,CombatAdmin *comAd); // The player's carrying weight changes.
        void addBackPackItem(CombatAdmin *comAd,Item *thisItem,bool itemWasBought); // The player gets a backpack item and adds it to their backpack, itemWasBought is either true or false if the item was bought hence the name.
        string getBackPackItemName(int index); // Return a backpack item.
        double getCurrentWeight(); // Get the current or max weight of this player.
        double getMaxWeight();
        string getName();
        void setName(string newName);
        double getPlayerWeight();
        double getBackPackItemCost(int index); // Get the cost of an item.
        double getBackPackItemWeight(int index); // Get the weight of an item.
        bool removeBackpackItem(int removeIndex,bool itemSold,CombatAdmin *comAd); // Take an item from the player's inventory if they sell it etc (remove index is the index of the item to be removed and the bool value is to check whether the item has been removed by selling it).
        string doubleToString(double number); // Convert from a double to a string.
        double getMaxHealth(); // Get the max health of the player.
        void changeMaxHealth(int newMax,CombatAdmin *comAd); // Change the player's max health, but this can only be done by a mul of 20 (so has to be an int newVal for the % opp to work).
        void changeMaxCarryLim(int newMax,CombatAdmin *comAd); // Change the player's max carry lim by a mul of 10.
    protected:
        double health; // The player's current health
        string name; // The player's name
        double goldCount; // The player's current amount of gold.
        double maxWeight; // The maximum weight a player can carry.
        double currentWeight; // The current carrying weight of the player.
        double maxBackPckSlts; // The maximum Back back slots the player has.
        double usedBackPckSlts; // The current number of back pack slots in use.
        int currBackPackPointer; // Pointer to the current backpack slot.
        vector<Item *> backPack; // The player's backpack (16 slots total as vectors start counting from 0, so max index of 15 means 16 slots as slot 0 is also a slot in the vector...).
        double maxHealth; // Maximum health of the player.
        Weapon *activeWeapon; // The player's active weapon.
        Armour *activeArmour; // The player's active armour set.
        bool setNameFuncCalled; // For whether the player's name has been set or not (it can only be set one time when they choose their name, maybe they can change it later).
};

#endif // PLAYER_H
