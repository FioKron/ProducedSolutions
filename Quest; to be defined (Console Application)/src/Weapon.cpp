#include <Weapon.h> // Including the weapon header.
#include <CombatAdmin.h>

//Note that in the current place i am in the book, it says constructors can be overloaded, that is delcared with more or less params and have the constructors do different things.
Weapon::Weapon(double initDmg,double initSpd,string startName,double initWg,bool initIsJam,bool initUsAmm,double initJmChnce,double initMxAmCnt,double initCurAmCnt,double initItemCost,bool initHasAoe)
{ // Weapon constructor to set up the weapon, thus the values may be changed if it is known that the values must be certain things when a weapon is created.
    damage = initDmg;
    speed = initSpd;
    setItemNameWeightCostToInit(); // Make it so setting up the weapon works properly.
    setItemName(startName);
    setItemWeight(initWg);
    setItemCost(initItemCost);
    isJammable = initIsJam;
    usesAmmo = initUsAmm;
    jamChance = initJmChnce;
    maxAmmoCnt = initMxAmCnt;
    currAmmoCnt = initCurAmCnt;
    itemHasBeenSold = false; // Make it so this weapon has not been sold.
    hasAOE = initHasAoe;

    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}

double Weapon::getMaxAmmoCnt()
{
    return maxAmmoCnt;
}

void Weapon::ammoChange(double changeAmt,CombatAdmin *comAd) // This function allows you to change the amount of ammo if a weapon either postivly or negativly.
{
    currAmmoCnt += changeAmt; // Change the ammo left...

    if (currAmmoCnt <= 0.00) // The player has no ammo in their weapon, this value may need changing if the player has 0.50 rounds left for example, check this works later...
    // This value still needs to be changed, the system just rounds it down to 0 it seems...
    {
        currAmmoCnt = 0.00; // Make sure the ammo in the weapon is 0 and then the comAd can tell the player.
        comAd->comAdSay("This weapon is now out of ammo!",1);
    }
    else if (currAmmoCnt >= maxAmmoCnt) // This weapon seems to have too much ammo or the max amount.
    {
        currAmmoCnt = maxAmmoCnt; // Make it so that the ammo in the weapon cannot go above the max amt and tell the player that this amt has been reached.
        comAd->comAdSay("Max ammo storage amount reached.",1);
    }
}

bool Weapon::getHasAoe()
{
    return hasAOE; // get whether the weapon does AOE damage or not.
}

bool Weapon::hasJammed() // Check whether the weapon has jammed or not if the weapon is jammable.
{
    double jamState;
    bool isJammed = false;

    jamState = rand() % 100 / jamChance; // As the jamstate is divided by the jamChange the higher this is, the worse the weapon is at jamming.
    // Remember, do not make the jamchance 0 or errors will occur, if the jamchance is 100 though the weapon has a 100% chance of jamming, so balance it out % wise :)

    if (jamState <= 1) // If the chance was less then or equal to 1...
    {
        isJammed = true; // The weapon has jammed.
    }
    else // Else the weapon has not jammed.
    {
        isJammed = false;
    }

    return isJammed;
}

double Weapon::getSpeed() // Rest of these functions get all of the protected members of Weapon for use in other functions, these can not be changed, only inited in the constructor.
{
    return speed;
}

double Weapon::getDamage()
{
    return damage;
}

bool Weapon::getIsJamable()
{
    return isJammable;
}

bool Weapon::getUsesAmmo()
{
    return usesAmmo;
}

double Weapon::getAmmoCnt()
{
    return currAmmoCnt;
}
