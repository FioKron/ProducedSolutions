#ifndef WEAPON_H
#define WEAPON_H
#include <Item.h> // Need to include Item.h as weapon extends Item.
#include <string> // Need this line or it complains
#include <CombatAdmin.h>

class CombatAdmin;

using namespace std;

class Weapon : public Item
{
    public://Prototype functions
        Weapon(double initDmg,double initSpd,string startName,double initWg,bool initIsJam,bool initUsAmm,double initJmChnce,double initMxAmCnt,double initCurAmCnt,double initItemCost,bool initHasAoe); // Weapon constructor
        double getSpeed(); // All these get functions are used to read values from the object, they cannot be obtained though and changed.
        double getDamage();
        bool getIsJamable();
        bool getUsesAmmo();
        double getAmmoCnt();
        double getMaxAmmoCnt();
        void ammoChange(double changeAmt,CombatAdmin *comAd);
        bool hasJammed();
        bool getHasAoe();
    protected:
        double damage; // The weapon's damage.
        double speed; // The weapon's attack speed.
        bool isJammable; // Whether the weapon can be jammed
        bool usesAmmo; // Whether the weapon uses ammo or not
        bool hasAOE; // Whether the weapon has AOE or not.
        double jamChance; // The chance the weapon has to jam, 0 if the weapon cannot jam.
        double maxAmmoCnt; // The maximum amount of ammo the weapon can hold, 0 if it does not hold ammo
        double currAmmoCnt; // The current amount of ammo in the weapon, 0 if the weapon does not hold ammo.

};

#endif // WEAPON_H
