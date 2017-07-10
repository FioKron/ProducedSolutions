#include <Enemy.h>
#include <iostream> // Required for input and output
#include <windows.h> // Needed to use the sleep function.

Enemy::Enemy() // Sacramento tut type of enemy.
{
    name = "Sacramento";
    aggroMsg = "I've found them!";
    health = 200;
    deathcry = "Noooo!";
    taunt[0] = "Hahha!";
    taunt[1] = "Too easy!";
    taunt[2] = "You never should have come here!";
    taunt[3] = "Weakling!";
    maxHealth = 200;
    // Has no weapon or armour, could make instagib into a weapon for this enemy insted of a string literal.
    hasSpecialAbilties = false; // This enemy has no special abilties.

    // This enemy does not need to be reset if defeated and/or damaged.
    // Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}

Enemy::Enemy(string newName) // Private Matterson early random encounter type of enemy.
{
    name = newName;
    aggroMsg = "Suprised to see me?";
    health = 100;
    deathcry = "Argh!";
    taunt[0] = "Diee!"; // The taunts all enemies have. 4 taunts total.
    taunt[1] = "Hahhaha";
    taunt[2] = "I've got you now!";
    taunt[3] = "This won't hurt... too much!";
    maxHealth = 100;
    hasSpecialAbilties = true; // This enemy has special abilties now.

    // Set the reset values.
    nameResetVal = name;
    aggroMsgResetVal = aggroMsg;
    healthResetVal = health;
    deathcryResetVal = deathcry;
    tauntResetVal[0] = taunt[0]; // The taunts all enemies have. 4 taunts total.
    tauntResetVal[1] = taunt[1];
    tauntResetVal[2] = taunt[2];
    tauntResetVal[3] = taunt[3];
    maxHealthResetVal = maxHealth;
    hasSpecialAbiltiesResetVal = hasSpecialAbilties;

    // The weapon and armour are set to null though, so that they can be reassigned later (if ever)
    activeWeapon = new Weapon(0,0,"Null",0,false,false,0,0,0,0,false);
    activeArmour = new Armour(0,0,"Null",0,0);
    // These technically count as a weapon and armour but are effectively not.

    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}

Enemy::Enemy(string newName, string newAggMsg, double newHealth, string newDeathcry, string taunt1,string taunt2, string taunt3, string taunt4, double newMxHp,bool newHasSpecAbils) // Other types of enemies
{
    // Set attributes to whatever the function call dictates.
    name = newName;
    aggroMsg = newAggMsg;
    deathcry = newDeathcry;
    taunt[0] = taunt1;
    taunt[1] = taunt2;
    taunt[2] = taunt3;
    taunt[3] = taunt4;
    maxHealth = newMxHp;
    health = newHealth; // Set up the enemy's hp to the value stated and set the maximum value to the value stated.
    hasSpecialAbilties = newHasSpecAbils; // Set up if the enemy has spec abils or not.

    // Set the reset values.
    nameResetVal = name;
    aggroMsgResetVal = aggroMsg;
    healthResetVal = health;
    deathcryResetVal = deathcry;
    tauntResetVal[0] = taunt[0]; // The taunts all enemies have. 4 taunts total.
    tauntResetVal[1] = taunt[1];
    tauntResetVal[2] = taunt[2];
    tauntResetVal[3] = taunt[3];
    maxHealthResetVal = maxHealth;
    hasSpecialAbiltiesResetVal = hasSpecialAbilties;

    // The weapon and armour are set to null though, so that they can be reassigned later (if ever)
    activeWeapon = new Weapon(0,0,"Null",0,false,false,0,0,0,0,false);
    activeArmour = new Armour(0,0,"Null",0,0);
    // These technically count as a weapon and armour but are effectively not.

    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}

void Enemy::resetValues() // Reset this enemy.
{
    name = nameResetVal;
    aggroMsg = aggroMsgResetVal;
    health = healthResetVal;
    deathcry = deathcryResetVal;
    taunt[0] = tauntResetVal[0];
    taunt[1] = tauntResetVal[1];
    taunt[2] = tauntResetVal[2];
    taunt[3] = tauntResetVal[3];
    maxHealth = maxHealthResetVal;
    hasSpecialAbilties = hasSpecialAbiltiesResetVal;
    // This is so in the next random encounter, the player will fight the same pointer if it is the prolouge/ between Winter and Gest(where there are repeated random ents), but they will be fought as if they are a fresh pointer.

    // The weapon and armour do not need to be re-equipped. The weapon is not reset though.
}

bool Enemy::getHasSpecialAbilities() // Get this member var.
{
    return hasSpecialAbilties;
}

void Enemy::autoEquip(Weapon *weapon,Armour *armour) // Interesting func here, equips a weapon and armour for an instance of the enemy class. (Objects passed as pointers)
{
    *activeWeapon = *weapon;
    *activeArmour = *armour;
}

Weapon *Enemy::getActiveWeapon() // all getx functions for accesing protected members. (Pointers if need be)
{
    return activeWeapon;
}

Armour *Enemy::getActiveArmour()
{
    return activeArmour;
}

void Enemy::gotAggro()
{
    enemySay(aggroMsg);
}

void Enemy::enemySay(string sayWhat)
{
    cout << name + ": " + sayWhat << endl; // Print the enemy's name as a tag and then what the enemy says.
    Sleep(3000); // Wait.

}

double Enemy::getMaxHealth() // Get the max health the enemy can have.
{
    return maxHealth;
}

void Enemy::enemyTaunt() // Say a random taunt.
{
    int tauntNum;

    tauntNum = rand() % 3; // determine which taunt num will be said.
    enemySay(taunt[tauntNum]); // Say this random taunt.
}

void Enemy::enemyDeath()
{
    enemySay(deathcry);
}

string Enemy::getName()
{
    return name;
}

void Enemy::healthChange(double newHealth) // changes the enemy's health, a positive value if they gain health a negative one if they lose health.
{
    health += newHealth;
    // However...

    if (health > maxHealth) // They can not have more health then their maximum health
    {
        health = maxHealth; // So set their health to maximum if this happens.
    }
    else if (health < 0) // They also can not have less then 0 health. (Negative health)
    {
        health = 0; // So set their health to 0 if this happens.
    }
}

double Enemy::getHealth()
{
    return health;
}
