#include <Merchant.h>
#include <string> // Need this line or it complains
#include <iostream> // For saying things for example
#include <NonPlayerCharacter.h>
#include <CombatAdmin.h>
#include <Windows.h>
#include <enemy.h>
#include <Item.h>
#include <Weapon.h>
#include <sstream> // For converting between types.
#include <iomanip> // For manipulating what the value conversion will be precise too etc.
#include <vector> // For showing the cost and weight of an item properly.

class Player; // Required for no errors.
class CombatAdmin;

Merchant::Merchant(string newName)
{
    name = newName; // Set this merchant's name to the new one.

    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}

string Merchant::buy(Player *player,CombatAdmin *comAd,Weapon *unArmed,Armour *unArmoured) // The merchant will buy things from the player.
{
    string answer = ""; // The answer to whether the player wants to sell anything.
    int counter = 0; // The counter for the for loop, set it to 0.
    bool equippedWepWasSold = false; // For whether the player sold an equipped wep or arm or not, add more when needed.
    bool equippedArmWasSold = false;

    // Only allow the player to sell things if they are able to.

    if (player->getUsedBackPackSlots() > 0) // If they have more then one used backpack slot...
    {
        comAd->comAdSay("This is a list of all of what is in " + player->getName() +"'s backpack, even if it is nothing:",1); // List the entire backpack.

        do
        {
            string strCounter = intToString(counter); // Make a string version of the counter varible.
            string strTestMaxSlots = intToString(player->getMaxBackPackSlots() - 1); // Even though this is a double it only needs to be converted to the whole number, but -1 to as that should be correct...
            // Instead of using vectors for both the weight and cost, that does not seem to work so I just used cout as using vectors and trying to state what should be valid posistions in the vectors causes a crash. Hmm...
            comAd->comAdSay("",5); // Print the combat admin tag, remember that this all needs to be done fast

            if (strCounter == strTestMaxSlots) // If this is the last number to be placed end the line else...
            {
                cout << strCounter + "." << endl;
            }
            else // Do this.
            {
                comAd->comAdSay(strCounter + ".",3); // Then the number and a dot (but not the tag).
            }

            if (player->getBackPackItemName(counter) == "Nothing") // If the name of the item is "Nothing" then...
            {
                // Do nothing.
            }
            else if (player->getBackPackItemName(counter) != "Nothing") // The item in this backpack slot has a name...
            {
                comAd->comAdSay(player->getBackPackItemName(counter) + ".",4);  // Then the item's details needed for selling it.
                comAd->comAdSay("This item can be sold for ",3);
                cout << player->getBackPackItemCost(counter); // So there must be an item there... state it's cost and weight.
                comAd->comAdSay(" gold pieces and you will have ",3);
                cout << player->getBackPackItemWeight(counter);
                comAd->comAdSay("kgs free to wield other items.",4);
            }

            counter ++; // Increment the counter to display all the items.

        } while (counter < player->getMaxBackPackSlots()); // Keep doing this until all item's have been listed(even slots that have nothing).

        // Dev note: The above code seems to function correctly. So if you find any problems it should not be due to this code or maybe it will be.

        // Allowing the player to just sell one item at a time.

        characterSay("So, tell me the number of the item you wish to sell (only a single digit like 1 or 2 etc) or anything else to sell me nothing (I will ignore what you say if you say something rude though...).",1);
        characterSay("But if you see no item name or other stats,there is no item to sell from that slot (you may have sold it already).",2);

        answer = getStringAnswer(answer); // Get the player's answer.

        player->playerSay(answer); // The player character says the player's answer.

        answer = comAd->toUpperCase(answer); // boost the answer to uppercase.

        // Perhaps tell the player what thier weight now is as they sold an item.
        // Also maybe find a way to not have getting the item's cost hard coded.

        int intAnswer = stringToNumber(answer);

        if (player->getActiveArmour()->getItemName() == player->getBackPackItemName(intAnswer)) // The player has sold their active weapon or armour...
        {
            equippedArmWasSold = true; // So return true.
        }

        if (player->getActiveWeapon()->getItemName() == player->getBackPackItemName(intAnswer))
        {
            equippedWepWasSold = true; // Etc here too.
        }

        if (answer == "0" && player->removeBackpackItem(intAnswer,true,comAd) == true) // If they enter 0 and that backpack slot has an item...
        {
            characterSay("Well, that items looks pretty decent...",1); // The merchant says something.
            player->goldChange(player->getBackPackItemCost(0)); // The player's gold member var gets the gold from selling the item...
            comAd->playerFindsGold(player->getName(),player->getBackPackItemCost(0),player->getGoldCount()); // the comAd annouces that they have got the gold.

            // If the removeBackPackItem function returns true the item has been removed, if it was false the item has not been removed.
        }
        else if (answer == "1" && player->removeBackpackItem(intAnswer,true,comAd) == true) // If they enter 1 and that backpack slot has an item...
        {
            characterSay("Well, that items looks pretty decent...",1); // The merchant says something.
            player->goldChange(player->getBackPackItemCost(1)); // The player's gold member var gets the gold from selling the item...
            comAd->playerFindsGold(player->getName(),player->getBackPackItemCost(1),player->getGoldCount()); // the comAd annouces that they have got the gold.

            // If the removeBackPackItem function returns true the item has been removed, if it was false the item has not been removed.
        }
        else if (answer == "2" && player->removeBackpackItem(intAnswer,true,comAd) == true) // If they enter 2 and that backpack slot has an item...
        {
            characterSay("Well, that items looks pretty decent...",1); // The merchant says something.
            player->goldChange(player->getBackPackItemCost(2)); // The player's gold member var gets the gold from selling the item...
            comAd->playerFindsGold(player->getName(),player->getBackPackItemCost(2),player->getGoldCount()); // the comAd annouces that they have got the gold.

            // If the removeBackPackItem function returns true the item has been removed, if it was false the item has not been removed.
        }
        else if (answer == "3" && player->removeBackpackItem(intAnswer,true,comAd) == true) // If they enter 3 and that backpack slot has an item...
        {
            characterSay("Well, that items looks pretty decent...",1); // The merchant says something.
            player->goldChange(player->getBackPackItemCost(3)); // The player's gold member var gets the gold from selling the item...
            comAd->playerFindsGold(player->getName(),player->getBackPackItemCost(2),player->getGoldCount()); // the comAd annouces that they have got the gold.

            // If the removeBackPackItem function returns true the item has been removed, if it was false the item has not been removed.
        }

        // If the player sells an item, 0 or 1 or 2 or 3 etc will still appear (at the current time) but the item will not be there and if you try and sell it you will not be able to.
        // The player's weight is changed in the weightChange member function of the player and the combat admin also tells the player what thier new weight is.
        // Also it seems that you need to increase the if else statement every time the merchant has another item they can buy, maybe make it so that is not the case...

        else // The merchant is sarcastic towards the player due to the player's response.
        {
            characterSay("Hmm, you have such great knowledge...",1);
        }

    }
    else if (player->getUsedBackPackSlots() <= 0) // If they have no used backpack slots (or somehow less then 0), they have nothing to sell...
    {
        comAd->comAdSay("ERROR: " + player->getName() + " has nothing to sell.",1);
        characterSay("Well, looks like I will not be buying from you then...",1);
    }

    // The return check.
    if (equippedArmWasSold == true) // This may need changing.
    {
        string armSold = "Armour";
        return armSold;
    }
    else if (equippedWepWasSold == true)
    {
        string wepSold = "Weapon";
        return wepSold;
    }
}

string Merchant::getInventoryItemName(int index)
{
    return inventory[index]->getItemName(); // Return the item.
}

double Merchant::getInventoryItemCost(int index)
{
    return inventory[index]->getItemCost();
}

void Merchant::addItem(Item *thisItem)
{
    inventory.push_back(thisItem);
}

void Merchant::addWeapon(Weapon *thisWeapon)
{
    weapons.push_back(thisWeapon); // Add the weapon to the inventory.
}

void Merchant::addArmour(Armour *thisArmour)
{
    armour.push_back(thisArmour); // Add the armour to the inventory.
}

void Merchant::removeItem(int index) // Remove an item at the specified index.
{
    inventory[index] = NULL;
}

void Merchant::sell(Player *player,CombatAdmin *comAd,int sellStage) // The merchant will sell things to the player.
{
    string answer = ""; // The answer to whether the player wants to buy anything.

    characterSay("hmm, let me show you what I have to sell to you.",1);

    if (sellStage == 1) // The sell stage is what is a way of determining what the merchant is selling, so items can be instanced here,stage one is where the Gestlehiem merchants sells to the player (have other stages for other merchants in the future...).
    {
        // Perhaps make this part of selling to the player into one function.
        if (player->getUsedBackPackSlots() >= player->getMaxBackPackSlots()) // The player cannot buy from this merchant as they have no space (set to check if they somehow have more then they are allowed hence >).
        {
            characterSay("It seems you cannot buy from me as you have no space in your backpack...",1);
        }
        else if(player->getUsedBackPackSlots() < player->getMaxBackPackSlots()) // The player has space to put an item in thier backpack so they can buy one if they wish.
        {
            if (player->getCurrentWeight() >= player->getMaxWeight()) // The player is already at max weight or has gone over it (some how).
            {
                comAd->comAdSay("ERROR: " + getName() + " is unable to carry any more items, please get rid of an item if you wish to purchase this one.",1); // com ad states the error.
                characterSay("Hmm, thank you lovely robot for telling me that, seems you cannot wield anything form me, so be it...",1);
                // Add a remove item method call here that the player gives the values of to remove the item they want.
            }
            else if (player->getCurrentWeight() < player->getMaxWeight()) // The player can buy something as they are able to wield it (they have enough weight).
            {
                string item0Value = intToString(inventory[0]->getItemCost()); // The value of the items for sale (converted from double to string).
                string item1Value = intToString(inventory[1]->getItemCost());

                string item0Weight = intToString(inventory[0]->getItemWeight()); // The weight of the items for sale (converted from a double to string).
                string item1Weight = intToString(inventory[1]->getItemWeight());

                string wepDamage = intToString(weapons[0]->getDamage()); // Get some of the more specific stats of the weapons as a string from a double.
                string wepSpeed = intToString(weapons[0]->getSpeed()); // However, for damage and speed it does not need to use doubleToString as it should not need to be rounded.
                string armDmgResist = intToString(armour[0]->getDmgResist() * 100); // Same for the % of damage reduced and speed reduction stats for armour too.
                string armSpdModifier = intToString(armour[0]->getAttSpdMod() * 100);

                characterSay("",3); // Print the name tag.
                characterSay("0.",4); // Then the number and a dot (but not the tag).
                if (inventory[0]->getItemHasBeenSold() == true) // Only print the item's name if it is still there, if not...
                {
                    // Print nothing.
                }
                else if (inventory[0]->getItemHasBeenSold() == false) // The pointer points to something
                {
                    characterSay("This weapon is worth " + item0Value  + " gold peices: " + inventory[0]->getItemName() + ". This weapon weighs " + item0Weight + "kgs.",4); // Then the item's name, weight and cost.
                    characterSay("This weapon does " + wepDamage + " points of damage. This weapon also has a speed of " + wepSpeed + " (The higher the number the better).",4);
                }


                characterSay("",3); // Print the name tag.
                characterSay("1.",4); // Then the number and a dot (but not the tag).
                if (inventory[1]->getItemHasBeenSold() == true) // Only print the item's name if it is still there, if not...
                {
                    // Print nothing.
                }
                else if (inventory[1]->getItemHasBeenSold() == false) // The pointer points to something
                {
                    characterSay("This armour is worth " + item1Value + " gold peices: " + inventory[1]->getItemName() + ". This armour weighs " + item1Weight + "kgs.",4); // Then the item's name, weight and cost.
                    characterSay("This armour is " + armDmgResist + "% resistant to damage. This armour also has a speed modifier of " + armSpdModifier + "% (The lower the number the better).",4);
                }

                characterSay("So just state the number of the item you wish to buy(just a single digit, 1 or 2 etc) or anything else if you want to buy nothing at all.",1);
                characterSay("But if you see a blank space in the display of my inventory you cannot buy what was in that slot (as you may have already bought it).",2);

                answer = getStringAnswer(answer); // Get the player's answer.

                player->playerSay(answer); // The player character says the player's answer.

                answer = comAd->toUpperCase(answer); // boost the answer to uppercase.

                double testWeight0 = player->getCurrentWeight() + inventory[0]->getItemWeight(); // For testing whether the player has enough weight for the items.
                double testWeight1 = player->getCurrentWeight() + inventory[1]->getItemWeight();

                if (player->getGoldCount() >= inventory[0]->getItemCost()) // They have enough gold (or more then enough) to buy at least the hand grenades.
                {
                    if (answer == "0" && testWeight0 <= player->getMaxWeight() && inventory[0]->getItemHasBeenSold() == false && player->getGoldCount() >= inventory[0]->getItemCost()) // Buy the grenades (if the player is able to, e.g. has enough gold etc).
                    {
                        characterSay("Ooo yea! I'm making loads a money! I hope your happy with the item...",1); // The merchant is happy.
                        player->weightChange(inventory[0]->getItemWeight(),comAd);
                        player->addBackPackItem(comAd,inventory[0],true); // The player buys the item hence why itemBought is true.

                        comAd->playerFindsItem(player,inventory[0],2);
                        player->goldChange(-(inventory[0]->getItemCost()));
                        comAd->playerLosesGold(player->getName(),inventory[0]->getItemCost(),player->getGoldCount());

                        inventory[0]->setItemHasBeenSold(); // Mark the item as now being sold.
                    }
                    else if (answer == "1" && testWeight1 <= player->getMaxWeight() && inventory[1]->getItemHasBeenSold() == false && player->getGoldCount() >= inventory[1]->getItemCost()) // Buy the power suit (if the player is able to, e.g. has enough gold etc).
                    {
                        characterSay("Ooo yea! I'm making loads a money! I hope your happy with the item...",1); // The merchant is happy.
                        player->weightChange(inventory[1]->getItemWeight(),comAd);
                        player->addBackPackItem(comAd,inventory[1],true); // The player buys the item hence why itemBought is true.

                        comAd->playerFindsItem(player,inventory[1],2);
                        player->goldChange(-(inventory[1]->getItemCost()));
                        comAd->playerLosesGold(player->getName(),inventory[1]->getItemCost(),player->getGoldCount());

                        inventory[1]->setItemHasBeenSold(); // Mark the item as now being sold.
                    }
                    else if (testWeight1 > player->getMaxWeight() || testWeight0 > player->getMaxWeight()) // The player cannot carry either of the items.
                    {
                        characterSay("Hmm, it seems you cannot wield that item...sell me something if you want to clear up a bit of weight first... and fast.",1);
                    }
                    else if (player->getGoldCount() < inventory[0]->getItemCost() || player->getGoldCount() < inventory[1]->getItemCost()) // They have less then required to buy either item.
                    {
                        characterSay("Hmm, it seems you cannot afford that item, you need a bit more cash it seems...try selling me some stuff to make some money.",1);
                    }
                    else // The merchant is a bit annoyed.
                    {
                        characterSay("Ooo.. but I have such great items! so be it...(just to remind you,if you tried to buy an item from a blank space you cannot do so as you may have already bought it)",1);
                    }

                }
                else if (player->getGoldCount() < inventory[0]->getItemCost()) // They have less gold required to even buy the cheapest item...
                {
                    characterSay("I am sorry but it seems you do not have enough money to buy even my cheapest item from me...perhaps sell something to me first.",1); // The merchant advises the player on how to quickly get more money.
                }
            }
        }
    }
    else if (sellStage == 2) // Another sell stage... for a different merchant perhaps.
    {

    }

}

string Merchant::handleResponse(Player *player,CombatAdmin *comAd,string answer,Weapon *unArmed,Armour *unArmoured) // Handle the answer the player gave.
{ // The comAd is needed if they need to passed into the buy and sell funcs.
    string equItemSold = ""; // For checking whether the player has sold an item they had equipped or not.

    if (answer == "BUY") // If the player wants to buy, the merchant will sell.
    {
        sell(player,comAd,1); // Hence sell here.
        // Sell stage may need to be made more dynamic...
    }
    else if (answer == "SELL") // If the player wants to sell, the merchant will buy.
    {
        equItemSold = buy(player,comAd,unArmed,unArmoured); // Hence buy here.
    }
    else if (answer == "LEAVE" || answer == "DEPART" || answer == "VACATE" || answer == "BE GONE" || answer == "BEGONE" || answer == "BYE" || answer == "NO") // Typical answers if the player wants to leave, as I have thought up.
    {
        characterSay("Oh, you want to be off then? So be it, goodbye fair soul.",1);
    }
    else // If the player says anything else...
    {
        characterSay("Well, if you are going to give me such an interesting response, I best be off, cya.",1);
    }

    return equItemSold; // Return whether an item the player had equipped was sold or not and what the type of item it was.
}
