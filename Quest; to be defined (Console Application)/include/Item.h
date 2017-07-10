#ifndef ITEM_H
#define ITEM_H
#include <string> // Need this line or it complains

using namespace std; // See main.cpp as to why this is here.

class Item // class here
{
    protected:
        string itemName; // The item's name as a string
        double itemWeight; // The items's weight as a double, double as this will allow more complicated calcualtions.
        double itemCost; // How much the item is worth.
        bool itemHasBeenSold; // Whether the item has been sold or not (Seems to still need to be initalised in the weapon and armour's constructors...).
        bool itemNameSet; // For checking whether the name,weight and cost have been set at least once or not.
        bool itemWeightSet;
        bool itemCostSet;
    public: //Prototype functions
        void setItemName(string temp); // Set and get functions as this is a value class so to speak, these functions may be changed so that you can not set the start/modified values to something invalid
        string getItemName(); // This class basically sets up value objects that will become other things like armour or weapons.
        void setItemWeight(double temp); // Most of the public functions are therefore just setters and getters per say.
        double getItemWeight();
        double getItemCost();
        void setItemCost(double newItemCost);
        bool getItemHasBeenSold(); // Get the item has been sold value.
        void setItemHasBeenSold(); // Set that the item has been sold (set the value to true).
        void setItemNameWeightCostToInit(); // Set the name, weight and cost checks to thier init values.
        void setItemNameToNothing(); // Set the item's name to nothing.

};

#endif // ITEM_H
