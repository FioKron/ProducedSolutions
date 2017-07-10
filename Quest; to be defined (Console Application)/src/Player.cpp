#include <Player.h>
#include <iostream> // Required for input and output
#include <windows.h> // Needed to use the sleep function.
#include <Weapon.h>
#include <Armour.h>
#include <CombatAdmin.h>
#include <Item.h>
#include <vector>
#include <iomanip> // For manipulating the value conversion will be precise too etc.
#include <sstream>

Player::Player(Weapon *unArmed,Armour *unArmoured) // Default Player constructor, unlike the other classes we know that for this constructor at least the player:
{
    health = 100; // starts off at max health
    goldCount = 0; // starts off with no money
    maxWeight = 50; // can carry quite a bit
    currentWeight = 0; // but has nothing on them
    maxBackPckSlts = 16; // has a certain amount of space in their backpack
    usedBackPckSlts = 0; // but is not using any of it
    currBackPackPointer = 0; // having no items in the backpack, the pointer will be at 0 (other bugs may occur... remember to fix the other bugs...).
    maxHealth = 100; // and has a maximum health equal to their actual health (at the moment).
    activeWeapon = unArmed; // Make them have a weapon and armour that acts like they have none.
    activeArmour = unArmoured; // Remember to have a * on both sides.
    setNameFuncCalled = false; // Just to make sure the player's name is not changed just as they are being created.

    // Remember that for each new param in a constructor, you need to add this to that function call duh.

    // Make all the backpack slots have a "Nothing" item in them, remember the capital N...
    for (int counter = 0; counter < maxBackPckSlts; counter++)
    {
        Item *nothing = new Item(); // The nothing pointer, not used for other then setting all slots to nothing...
        nothing->setItemName("Nothing"); // Set up this item to "Nothing" with no weight and no value.
        nothing->setItemWeight(0);
        nothing->setItemCost(0);
        backPack.push_back(nothing); // Put this into the backpack.
    }

    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}

bool Player::removeBackpackItem(int removeIndex,bool itemSold,CombatAdmin *comAd) // Remove an item from the player if they sell it for example (Return true if an item can be removed, false if not).
{
    if (removeIndex < 0) // if the index is -1 or less...
    {
        // return a false value, nothing else happens (invalid array index).
        return false;
    }
    else // Index must be greater then -1.
    {
        if (getBackPackItemName(removeIndex) == "Nothing") // That backpack item name is nothing so there is nothing in the backpack slot (this could be checked with the cost too to see if there is an item in that slot).
        {
            // Return false and nothing else happens once again.
            return false;
        }
        else // There must be something in that slot
        {
            usedBackPckSlts --; // Decrement the used backpack slots as this item has been removed.
            // The changing of weight has to be done in two steps to work.
            double weightLoss = backPack[removeIndex]->getItemWeight(); // Get the value to change the weight by.
            weightLoss = -weightLoss; // Then negate it.
            weightChange(weightLoss,comAd); // Change the weight by what the item weighed so the weight is changed appropriately(but use the negative value to do so thus).

            backPack[removeIndex]->setItemNameToNothing(); // Make the item name equal to nothing (maybe change to make it get rid of the pointer properly or not, we shall see).
            // USE THIS FUNCTION TO SET THE ITEM'S NAME TO NOTHING FROM NOW ON!

            // Set itemHasBeenSold to true if it has been sold else do nothing.
            if (itemSold == false) // If the item has not been sold...
            {
                // Do nothing.
            }
            else if (itemSold == true) // else it must have.
            {
                backPack[removeIndex]->setItemHasBeenSold(); // So call the setItemHasBeenSold function (which sets the value to true).
                // Then perhaps make the item no longer exist and just delete it, or maybe make it so the player can buy back the item...
            }
            // The cost cannot be changed here or they will not be changed by the merchant when the player sells an item, so it is changed there.

            currBackPackPointer = 0; // Make the pointer equal to 0 to act as a starting point for changing the pointer to a valid location.
            string nameTest = backPack[currBackPackPointer]->getItemName(); // Test the first item's name.

            // The while statement below works (may need to set itemName to "Nothing" elsewhere too) but...
            while (nameTest != "Nothing") // keep incrementing the backpack slot by 1 untill a slot can be found where there is no item so a new item can be added.
            { // This while loop only runs if the above bool statement is true so to speak, but may need a more proper removal function...
                currBackPackPointer ++;
                nameTest = backPack[currBackPackPointer]->getItemName(); // Get the item's name to test.
            }

            return true; // Return true as that index selected was valid and there was an item in that slot to be removed.
        }
    }
}

// Odd how these 3 functions need a * yet the player's constructor does not and actually crashes if you have them...hmm...
// Make it so these functions maybe tell the player what their active wep and arm are now.

void Player::autoEquip(Weapon *weapon,Armour *armour,CombatAdmin *comAd) // Interesting func here, equips a weapon and armour for the player class instance(Objects passed as pointers).
{
    *activeWeapon = *weapon;
    *activeArmour = *armour;

    comAd->comAdSay(name + " has had the " + activeWeapon->getItemName() + " equipped in their active weapon equipment slot, it will be used unless it is changed therefore.",1);
    comAd->comAdSay("They have also had the " + activeArmour->getItemName() + " equipped in their active armour equipment slot, it will be used unless it is changed therefore.",1);

    // This function does not return true or false as it does not need to.
}

bool Player::equipWeapon(Weapon *weapon,CombatAdmin *comAd) // Make the player's active weapon one that is passed into this function.
{
    bool weaponEquipped = true; // Make it so this function returns that the weapon was equiped (so it can be checked if needed).

    *activeWeapon = *weapon; // Remember how to change pointer values so to speak...

    comAd->comAdSay(name + " has had the " + activeWeapon->getItemName() + " equipped in their active weapon equipment slot, it will be used unless it is changed therefore.",1);

    return weaponEquipped; // Return that the weapon has been equipped.
}

bool Player::equipArmour(Armour *armour,CombatAdmin *comAd) // Make the player's active armour one that is passed into this function.
{
    bool armourEquipped = true; // Make it so this function returns that the armour was equiped (so it can be checked if needed).

    *activeArmour = *armour;

    comAd->comAdSay(name + " has had the " + activeArmour->getItemName() + " equipped in their active armour equipment slot, it will be used unless it is changed therefore.",1);

    return armourEquipped; // Return that this armour has been equipped.

}

Weapon *Player::getActiveWeapon() // all getx functions for accesing protected members. (Pointers if need be)
{
    return activeWeapon;
}

Armour *Player::getActiveArmour()
{
    return activeArmour;
}

double Player::getMaxHealth()
{
    return maxHealth; // Get the player's maximum health.
}

void Player::healthChange(double newHealth) // changes the player's health, a positive value if they gain health a negative one if they lose health.
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

void Player::playerSay(string sayWhat) // The player says something.
{
    cout << name + ": " + sayWhat << endl; // Print the player's name as a tag and then what the player says.
    Sleep(3000); // Wait.
}

double Player::getHealth()
{
    return health;
}

void Player::goldChange(double newGold) // Gain or lose gold (e.g. spend gold to lose it, loot stuff to gain it).
{
    goldCount += newGold; // Add and remove gold (negative int to remove gold).

    if(goldCount < 0) // The player can not have negative gold though...
    {
        goldCount = 0; // so set it to 0 if they have negative gold.
    }
}

double Player::getGoldCount() // Get the amt of gold the player has.
{
    return goldCount;
}

double Player::getUsedBackPackSlots()
{
    return usedBackPckSlts;
}

double Player::getMaxBackPackSlots()
{
    return maxBackPckSlts;
}

double Player::getPlayerWeight() // Remember that the player cannot excede a maxium weight.
{
    return currentWeight;
}

bool Player::weightChange(double newWeight,CombatAdmin *comAd) // Gain or lose weight. (e.g. discard items to lose weight, loot items to gain weight)
{
    if (currentWeight + newWeight > maxWeight) // The new thing the player was going to get is too heavy...
    {
        comAd->comAdSay("ERROR: you do not have enough weight left to carry this item.",1); // Give the player a general phrase that, regardless of how they got the thing that changes thier weight, (bought,found etc) they cannot wield it.
        return false; // The Combat Admin tells them of the error and this function returns false to show their weight has not been changed.
    }
    else if (currentWeight + newWeight <= maxWeight) // They have enough weight limit.
    {
        currentWeight += newWeight;

        if (currentWeight > maxWeight) // Checking to make sure they are not carrying too much or a negative amount of weight.
        {
            currentWeight = maxWeight;

        }
        else if (currentWeight < 0)
        {
            currentWeight = 0;
        }
        // Convert thier current weight from a double to a string.
        string strWeight = doubleToString(currentWeight);

        // Tell the player that thier weight has been changed succesfully and what it now is (the player should know at least soonish why this is the case).
        comAd->comAdSay("Your weight has been changed succesfully, it is now " + strWeight + "kgs.",1);

        return true;
    }
}

string Player::doubleToString(double number) //Change from a double to a string with some precision.
{
    ostringstream oss; // Converts from double to string.

    //Perform the conversion and return the results.

    oss << fixed << setprecision(2) << number;
    return oss.str();
}

void Player::addBackPackItem(CombatAdmin *comAd, Item *thisItem, bool itemWasBought) // The player has found(or bought) an item and so it is added to their backpack
{
    usedBackPckSlts ++; // Increment used slots for testing...

    int itemCost = thisItem->getItemCost();
    int backPackSlotsLeft = maxBackPckSlts - usedBackPckSlts; // Get the remaning backpack slots left.

    if (usedBackPckSlts > maxBackPckSlts) // Backpack is full, can be checked this way due to what is just above...
    {
        usedBackPckSlts = maxBackPckSlts; // Set usedslots to max (if somehow they got an item when they were not supposed to).
        comAd->comAdSay("ERROR: Backpack is full, please discard an item if you want to have this one.",1); // Tell the player via comAd that thier backpack is full.
        // Add code to remove an item so they can add the new one they just found, if they want the item do the same as below else do not.
    }
    else if (usedBackPckSlts <= maxBackPckSlts) // The player has slots left in their backpack.
    {
        if (currentWeight > maxWeight) // The player is carrying too much.
        {
            comAd->comAdSay("ERROR: no more weight allowed, please discard an item if you wish to have this one.",1);
            // Allow to player to discard the item if they want, needs to be coded in just below.
        }
        else if (currentWeight <= maxWeight) // The player can have the item as they are not carrying too much.
        {
            backPack[currBackPackPointer] = thisItem; // The current backpack slot gets the item, remember to overide the current thing in that slot or...
            currBackPackPointer ++; // The current backpack slot is increased by one so more items can be added later so that the curr pointer points to an unused location in this vector.

            if (currBackPackPointer > maxBackPckSlts - 1) // As of adding this item, the backpack is full if filled up in order (as in no items are removed, as the pointer is one above the limit, 16 slots, but invalid index in the array as it is from 0-15 not 0-16 or 1-16).
            {
                currBackPackPointer = maxBackPckSlts - 1; // So set the pointer to the final index in the array. (which is one less then the max capacity due to the way indexes work for vectors)
            }

            if (itemWasBought == true) // Only make this annoucement if the item was bought.
            {
                string strItemCost = doubleToString(itemCost); // Convert the required values.
                string strBackPackSltLef = doubleToString(backPackSlotsLeft);
                string strUsedBckPckSlts = doubleToString(usedBackPckSlts);

                comAd->comAdSay(getName() + " now has the item called " + thisItem->getItemName() + ".",1); // Annouce the player has this item
                comAd->comAdSay("They have spent " + strItemCost + " gold pieces for this item.",3);
                comAd->comAdSay("Have " + strBackPackSltLef + " backpack slots remaining and have used " + strUsedBckPckSlts + " backpack slots." ,3);
            }
        }
    }
}

void Player::changeMaxHealth(int newMax,CombatAdmin *comAd) // Change the player's max health as per the player wanting to do so, or an enemy spec abil...if an enemy changes the max health of the player you may need to call healthChange to make the player's health valid.
{
    // More validation in this func may be needed, eyes on...
    if (newMax % 20 > 0) // If the newMax value is not a mul of 20 do nothing to the max health value.
    {
        comAd->comAdSay("ERROR: the max health change was unsuccessful (remember that it needs to be a mul of 20).",1); // The comAd states the change was not done.
    }
    else if (newMax % 20 == 0) // Else the value is a mul of 20 so change it.
    {
        maxHealth += newMax; // Add the value (remember to have a negative value if you want to reduce the player's max health passed into this func).
        string strMax = doubleToString(maxHealth); // Perform the conversion and tell the player the change was done succesfully.
        comAd->comAdSay("Change successful, your new max health is now:" + strMax ,1);
    }
}

void Player::changeMaxCarryLim(int newMax,CombatAdmin *comAd) // Change the player's max weight limit.
{
    // More validation in this func may be needed, eyes on...
    if (newMax % 10 > 0) // If the newMax value is not a mul of 10 do nothing to the max weight value.
    {
        comAd->comAdSay("ERROR: the max weight limit change was unsuccessful (remember that it needs to be a mul of 10).",1); // The comAd states the change was not done.
    }
    else if (newMax % 10 == 0) // Else the value is a mul of 10 so change it.
    {
        maxWeight += newMax; // Add the value (remember to have a negative value if you want to reduce the player's max weight passed into this func).
        string strMax = doubleToString(maxWeight); // Perform the conversion and tell the player the change was done succesfully.
        comAd->comAdSay("Change successful, your new max carry weight is now:" + strMax + "KGS.",1);
    }
}

string Player::getBackPackItemName(int index) // Get the name of what item a backpack contains.
{
    // Return the value of the item's name that is in this slot in the backpack array but only if there is an item in that slot.

    return backPack[index]->getItemName(); // Return the item's name.
}

double Player::getBackPackItemCost(int index) // Get what the cost of said item is.
{
    return backPack[index]->getItemCost(); // Return the item's cost.
}

double Player::getBackPackItemWeight(int index) // Get the weight of an item.
{
    return backPack[index]->getItemWeight(); //Get the item's weight.
}

double Player::getCurrentWeight() // Get the player's current weight.
{
    return currentWeight;
}

double Player::getMaxWeight() // Get the player's max weight.
{
    return maxWeight;
}

string Player::getName() // Get the player's name.
{
    return name;
}

void Player::setName(string newName)
{
    if (setNameFuncCalled == true) // This function has been called already
    {
        // So do nothing.
    }
    else // The player's name gets changed.
    {
        name = newName; // Set the player's name to the new name...
        setNameFuncCalled = true; // And make it so that the setNameFuncCalled var is set to true so this function does not change the player's name if called again...
    }
}
