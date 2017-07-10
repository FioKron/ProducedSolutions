#ifndef ARMOUR_H
#define ARMOUR_H

#include <Item.h> // Need to include Item.h as weapon extends Item.
#include <string> // Need this line or it complains
#include <time.h>
#include <windows.h>

using namespace std;

class Armour : public Item // As armour is a type of item
{
    public:
        Armour(); // Empty constructor for player's active armour
        Armour(double initialdmgRes,double initialAtSpMd,string startName,double initialWg,double initItemCost); // Default armour constructor
        double getAttSpdMod(); // Getx for accessor functions to protected members.
        double getDmgResist();

    protected:
        double dmgResist; // The damage resistance in % terms of a set of armour.
        double attSpdMod; // The attack speed modifier of a set of armour in % terms. (E.g. clunky armour slows a player down in combat)

};

#endif // ARMOUR_H
