#ifndef ENEMY_H
#define ENEMY_H
#include <string>
#include <Weapon.h>
#include <Armour.h>

using namespace std;

class Weapon;

class Enemy
{
    public:
        Enemy(); // Constructor for the 'SomeGuy' type of enemy.
        Enemy(string newName); // Constructor for the 'Goon' type of enemy.
        Enemy(string newName, string newAggMsg, double newHealth, string newDeathcry, string taunt1,string taunt2, string taunt3, string taunt4, double newMxHp,bool newHasSpecAbils); // Constructor for other types of enemies.
        void autoEquip(Weapon *weapon,Armour *armour); // Give the enemy an active weapon and armour set.
        Weapon *getActiveWeapon(); // Generic getx accesor functions for protected members.
        Armour *getActiveArmour();
        void gotAggro(); // When the enemy spots you (the player that is), they will get aggresive.
        void enemySay(string sayWhat); // The enemy says something.
        void enemyTaunt(); // The enemy taunts.
        void enemyDeath(); // Stuff that handles what happens when the enemy dies.
        string getName();
        void healthChange(double health); // Called when the enemy gains or loses health. (e.g. lose health by taking damage via fighting, gain health via health potions)
        double getHealth(); // Get the current and max health of the enemy.
        double getMaxHealth();
        bool getHasSpecialAbilities();
        void resetValues(); // Reset this enemy so they can be used again.
    protected:
        string name; // The enemy's name.
        string aggroMsg; // What the enemy says when they get aggresive.
        double health; // The enemy's current health.
        string deathcry; // The enemy's deathcry, what they yell on death.
        string taunt[4]; // Array to hold taunts, 0-3 standard array index.
        double maxHealth; // The maximum health of the enemy.
        Weapon *activeWeapon; // The enemy's active weapon.
        Armour *activeArmour; // The enemy's active armour.
        bool hasSpecialAbilties; // For whether the enemy has special abilties or not, may need to outline in more detail somewhere what those are.

};

#endif // ENEMY_H
