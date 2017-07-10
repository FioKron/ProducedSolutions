#include <Item.h> // Including the item header.

void Item::setItemName(string temp) // Essentially a value object so it has get and set functions, but you can only set each value once... usally when a child of the item class is created.
{
    if (itemNameSet == true) // The item's name has already been set...
    {
        // So do nothing.
    }
    else if (itemNameSet == false) // The item's name has not yet been set.
    {
        itemName = temp; // So set it...
        itemNameSet = true; // and make so the nameSet var is now true.
    }

}
string Item::getItemName() // Get the item's name.
{
    return itemName;
}
void Item::setItemWeight(double temp)
{
    if (itemWeightSet == true) // The item's weight has already been set.
    {
        // So do nothing
    }
    else if (itemWeightSet == false) // The item's weight has not yet been set.
    {
        itemWeight = temp; // So set it...
        itemWeightSet == true; // and make it so the weightSet var is now true.
    }
}
double Item::getItemWeight()
{
    return itemWeight;
}

double Item::getItemCost()
{
    return itemCost;
}

void Item::setItemCost(double newItemCost)
{
    if (itemCostSet == true) // The value of the item has already been set...
    {
        // So do nothing.
    }
    else if (itemCostSet == false) // The item's value has not yet been set.
    {
        itemCost = newItemCost; // So set it...
        itemCostSet = true; // and make it so the costSet var is now true.
    }
}

void Item::setItemNameToNothing() // Set the item's name to nothing.
{
    itemName = "Nothing"; // Thus, do so, set it to the string var "Nothing".
}

void Item::setItemNameWeightCostToInit() // Initalise the values for checking whether the name,weight and cost of an item have been set.
{
    itemNameSet = false; // Set all the values to false.
    itemCostSet = false;
    itemWeightSet = false;
}

bool Item::getItemHasBeenSold()
{
    return itemHasBeenSold;
}

void Item::setItemHasBeenSold() // Make it so the item has now been sold.
{
    itemHasBeenSold = true; // Thus, make the var set to true.
}
