#include <Armour.h>
#include <time.h>
#include <Item.h>

Armour::Armour(double initialdmgRes,double initialAtSpMd,string startName,double initialWg,double initItemCost) // Setting up a piece of armour with typed in params.
{
    setItemNameWeightCostToInit(); // Make it so setting up the item works properly.
    setItemName(startName); // Set up this item.
    setItemCost(initItemCost);
    setItemWeight(initialWg);
    dmgResist = initialdmgRes; // Set up the armour correctly.
    attSpdMod = initialAtSpMd;
    itemHasBeenSold = false; // Make it so this piece of armour has not been sold.

    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}

Armour::Armour() // Set the armour equal to nothing.
{
    dmgResist = 0; // Set everything that needs to be set to blank effectivly and so to speak.
    attSpdMod = 0;
    setItemCost(0);
    setItemName("");
    setItemWeight(0);
    itemHasBeenSold = false;

    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}


 // getx accessor functions.
double Armour::getAttSpdMod()
{
    return attSpdMod;
}

double Armour::getDmgResist()
{
    return dmgResist;
}


