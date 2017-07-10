#include <HealthAndAmmoSuppiler.h> // Remember to use <> as apposed to "" or else...

HealthAndAmmoSuppiler::HealthAndAmmoSuppiler(string newName) // Default constructor.
{
    name = newName; // Make this NPC's name the new name assigned to it.
    costPerAmt = 2; // 2 gold per amt to heal for this supp.
    amtPerCost = 1; // 1 HP per the gold cost so 2 gold for each 1 HP is the cost to the player, this constructor may need changing in the future...
    hpAlreadyIncresed = false; // This supp has not yet incresed the player's HP.
    weightAlreadyInc = false; // Nor their carry weight.
    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}

void HealthAndAmmoSuppiler::handleResponse(Player *player,CombatAdmin *comAd,string answer) // Handle the response the player gives.
{
    if (answer == "HEAL" || answer == "HEAL!" || answer == "HEAL ME" || answer == "HEAL ME!" || answer == "FULL HEAL" || answer == "FULLHEAL" || answer == "FULL HEAL!" || answer == "FULLHEAL!") // The player wants to be healed.
    {
        healPlayer(player,comAd); // Heal the player thus, as they want some form of healing...
    }
    else if(answer == "MAX WEIGHT" || answer == "WEIGHT") // The player wants to increase their max weight.
    {
        increasePlayerWeightLim(player,comAd); // So do so.
    }
    else if (answer == "MAX HP" || answer == "HP") // The player wants a max HP upgrade... it would seem...
    {
        increasePlayerMaxHP(player,comAd); // So do so.
    }
    else if (answer == "AMMO" || answer == "AMMO!") // The player wants ammo.
    {
        givePlayerWepAmmo(player,comAd); // So give them the chance to get some(maybe vary it per weapon...).
    }
    else if (answer == "LEAVE" || answer == "DEPART" || answer == "VACATE" || answer == "BE GONE" || answer == "BEGONE" || answer == "BYE" || answer == "NO") // Typical answers if the player wants to leave and do nothing else, as I have thought up.
    {
        characterSay("Oh, guess I will just keep offering my services then to others who pass my way, goodbye!",1);
    }
    else // This char is a bit annoyed...
    {
        characterSay("Guess I will just be out of here then...",1);
    }
}

// All of these functions need completing...
void HealthAndAmmoSuppiler::healPlayer(Player *player,CombatAdmin *comAd) // Heal the player, this function seems to work, but once again, keep eyes on...
{
    string tempAns = ""; // The var used to get the player's answer, as they can only do a full heal, this says so, maybe make ammo have more specifics or if the player says 'full ammo' give them as much as they can afford if they cannot afford it all.
    double amtToHealFull = 0; // The amt to heal to full as per a calculation.
    int costOfHeal = 0; // The cost of this heal.

    string strCostPerHealAmt = doubleToString((double)costPerAmt); // Convert the cost per amt and amt per cost to strings as this may differ later on...(add more to this class's constructor to do so these values can be set to w/e)
    string strAmtPerCost = doubleToString((double)amtPerCost);

    characterSay("Ok I have the tech to heal you, it costs " + strCostPerHealAmt + " gold per " + strAmtPerCost + "HP.",1);
    characterSay("I can only heal you to full, but I can do so in one quick man-over mind, good machine I have here.",1); // Tell them that they can be healed to full HP.
    // Tell the player how to do what they want :)
    characterSay("Enter 'full heal' to be healed therefore.",1);
    characterSay("Remember you can tell me in any case structure(the full heal request), so, tell me 'full heal' or similar to do so, or anything else to not be healed at all.",1);

    tempAns = getStringAnswer(tempAns); // Get the answer the player gives.

    player->playerSay(tempAns); // The player says the answer they gave.

    tempAns = comAd->toUpperCase(tempAns); // Change this value to upper case as per usual...

    // Check if the player wants to be healed to full health.

    if (tempAns == "FULL HP" || tempAns == "FULL HP!" || tempAns == "FULLHP!" || tempAns == "FULL HEAL" || tempAns == "FULLHP" || tempAns == "FULLHEAL" || tempAns == "FULL HEAL!" || tempAns == "FULLHEAL!") // If they do(possibilities for such are given here)...
    {
        amtToHealFull = player->getMaxHealth() - player->getHealth();// Find how much HP it would take to heal them to full, converted from a double to an int therefore.
        string strFullHealAmt = doubleToString(amtToHealFull); // Get the value the player needs to be healed by and tell the player they will be healed this much.
        characterSay("Ok, to heal you too full I will need to heal you by " + strFullHealAmt + " points of health, so I will do so, assuming you have enough gold...",1); // Confirm that the player will be healed by this much.
        costOfHeal = (amtToHealFull / amtPerCost) * costPerAmt; // Then find out the cost of this, done as an int so this cost is rounded to the nearest gold piece...
        // Vars are used here for working this out.(done as such as it finds out the times the amt per cost needs to be healed then works out the cost by mul-ing that by the cost per amt).
        if (costOfHeal > 0) // If the cost was greater then zero(i.e. they had some HP to be healed to get full health)...
        {
            // Do as follows.
            if (player->getGoldCount() < costOfHeal) // If the player has less gold then required for a full heal...
            {
                comAd->comAdSay("ERROR: you do not have enough gold to be healed.",1); // The comAd says the player does not have enough even for a little heal so to speak...
                characterSay("Hmm, seems you do not have enough gold for a heal then!",1); // The supp complains.
            }
            else if (player->getGoldCount() >= costOfHeal) // Otherwise they have enough for a full heal.
            {
                player->goldChange(-((double)costOfHeal)); // Change the player's gold, remember to negate the value if they loose gold...
                comAd->playerLosesGold(player->getName(),(double)costOfHeal,player->getGoldCount()); // The comAd states what has happened.
                player->healthChange((double)amtToHealFull); // Give the player health.
                comAd->comAdStateEntHp(player->getName(),player->getHealth(),5); // A 5 here for fast combat speed, tell the player how much HP they have.
                characterSay("Ok, I healed you too full HP, happy days.",1); // The HAAS says what they have done.
            }
        }
        else if (costOfHeal <= 0) // If it was nothing though or somehow less(i.e. they do not need to be healed at all)...
        {
            characterSay("Gah, you did not need to be healed to full!",1); // This sup complains.
        }
    }
    else // Otherwise they will not be healed.
    {
        characterSay("Gah, you did not want to be fully healed then!(if you entered in an invalid value such as 'cake' then I will not heal you to full nor can I heal you a specific value such as 1 or 'one' etc if you told me that)",1);
        // This supp cannot heal the player by a certain value, only to full, if the player enters a silly value though...
    }

    tempAns = ""; // Whipe the temp ans here.
}



void HealthAndAmmoSuppiler::givePlayerWepAmmo(Player *player,CombatAdmin *comAd) // Give the player's equipped weapon ammo, this func seems to work once again, but eyes on to make sure it does...
{ // Maybe have an option here to allow the player to change weapons so they can give another weapon ammo?
    string tempAns = ""; // The answer the player gives.
    string strMaxAmmoCnt = doubleToString(player->getActiveWeapon()->getMaxAmmoCnt()); // Convert the max ammo count of this weapon, if it uses ammo, to a string.
    string strCurrAmmoCnt = doubleToString(player->getActiveWeapon()->getAmmoCnt()); // Convert the current ammo count of this weapon, if it uses ammo, to a string.


    double costPerAmmoUnit = 0; // Used later for the value it will cost the player to fill their weapon up per ammo unit.

    characterSay("Ok, I can give your weapon some ammo(the one you are wielding mind, your active weapon, not any other weapons you may own).",1);
    characterSay("Your weapon has " + strCurrAmmoCnt + " ammo remaining out of " + strMaxAmmoCnt + " ammo.",1); // Tell the player how much ammo they have out of the max amount allowed in their weapon, but if this weapon is fully loaded or does not use ammo...
    characterSay("So tell me 'full ammo' (in any case structure) to get a full load of ammo.",1); // Tell the player how to get ammo and then let them do so.
    characterSay("Remember that if you say you want ammo at all but your weapon does not use any, just say 'exit' (in any case structure) to leave without me getting a bit annoyed...",1); // This supp tells the player not to ask for ammo if their weapon does not use ammo...

    tempAns = getStringAnswer(tempAns); // Get the answer the player gives.

    player->playerSay(tempAns); // The player says the answer they gave.

    tempAns = comAd->toUpperCase(tempAns); // Change this value to upper case as per usual...

    if (strMaxAmmoCnt == strCurrAmmoCnt) // This weapon is fully loaded so...
    {
        characterSay("You are fully loaded there! You do not need any ammo! (for that weapon anyhow)",1); // Tell the player and exit this function.
    }
    else if (tempAns == "EXIT") // This statment must be in the if else or the merchant will always get annoyed if the player's weapon does not use ammo but they ask for ammo anyway.
    {
        characterSay("Ok then, let me give you some other help if you want.",1); // This supp is not as annoyed as per the statement just below...
    }
    else if (player->getActiveWeapon()->getUsesAmmo() == false) // If this weapon does not use ammo...
    {
        characterSay("Your weapon does not use ammo at all! I told you not to ask for ammo if your weapon does not use it!",1); // The supp gets annoyed.
    }
    else if (tempAns == "FULL AMMO" || tempAns == "FULLAMMO" || tempAns == "FULL AMMO!" || tempAns == "FULLAMMO!" ) // This weapon does not have full ammo and does use some...
    {
        characterSay("Ok, so your weapon needs ammo to be topped up, I will do so.",1); // Confirm the ammo to be toppped up.
        // Work out the cost per ammo unit.
        if (player->getActiveWeapon()->getMaxAmmoCnt() >= 500) // The max ammo count is 500 or greater.
        {
            costPerAmmoUnit = 0.5; // So 0.5 gold per ammo unit.
        }
        else if ((player->getActiveWeapon()->getMaxAmmoCnt() < 500) && (player->getActiveWeapon()->getMaxAmmoCnt() >= 200)) // The max ammo count is between 499 and 200.
        {
            costPerAmmoUnit = 2; // So 2 gold per ammo unit here.
        }
        else if ((player->getActiveWeapon()->getMaxAmmoCnt() < 200) && (player->getActiveWeapon()->getMaxAmmoCnt() >= 0)) // The max ammo count is between 199 and 0.
        {
            costPerAmmoUnit = 8; // So 8 gold per ammo unit here, going up x4 each stage atm, may change a bit later.
        }
        // These vars are declared here as this is only when they are needed.

        double amtToRefill = player->getActiveWeapon()->getMaxAmmoCnt() - player->getActiveWeapon()->getAmmoCnt(); // Work out the amt of ammo needed as per the max ammo count - the current ammo count

        double totalRefillCost = amtToRefill * costPerAmmoUnit; // Get the total cost as per the amt to refill x by the cost per ammo unit.

        if (player->getGoldCount() < totalRefillCost) // After working out the cost, the player does not have enough gold to refill their ammo.
        {
            comAd->comAdSay("ERROR: you do not have enough gold to refill to full ammo capacity.",1); // The comAd confirms the player does not have enough.
            characterSay("I was going to top up your weapon...but you do not have enough gold it seems!",1); // The supp is a bit annoyed.
        }
        else if (player->getGoldCount() >= totalRefillCost) // The player has enough gold to refill ammo for this weapon so...
        {
            player->getActiveWeapon()->ammoChange(amtToRefill,comAd); // Change the player's ammo positivly.
            // The ammo change function should tell the player that their weapon is now full but the comAd will say what their ammo is too.
            string strAmmoLeft = doubleToString(player->getActiveWeapon()->getAmmoCnt()); // Perform the conversion to get the correct value to tell the player how much ammo is in their weapon.
            comAd->comAdSay("There are " + strAmmoLeft + " ammo units left in this weapon, this should be the maximum amount as confirmed by me just before.",1); // The comAd confirms how much ammo there is in the weapon.

            player->goldChange(-(totalRefillCost)); // Negate the value just as it is passed into this function so it takes away from the player's gold count.
            comAd->playerLosesGold(player->getName(),totalRefillCost,player->getGoldCount()); // State the player has lost some gold.
            characterSay("There we go, and for not too much, to remind you/let you know, the more ammo the weapon can hold the less it costs to refill it.",1); // The supp confirms the ammo has now been topped up.
        }
    }
    else // Otherwise the player does not want ammo or just to leave without being a bit annoying to the merchant it seems(as tempAns does not equal 'exit' or 'full ammo')...
    {
        characterSay("Guess you don't want any ammo then...",1); // The supp guesses the player does not want ammo.
    }

    tempAns = ""; // Whipe the temp ans here.
}

void HealthAndAmmoSuppiler::increasePlayerMaxHP(Player *player,CombatAdmin *comAd) // Increase the max HP of the player, this func seems to be feature complete but eyes on...
{
    string tempAns = ""; // The answer the player gives.
    double costOfHPInc = 0; // The cost for increasing the player's max HP.
    double negCostOfHPInc = 0; // The value used in negation.
    double hpInc = 0; // The HP value the player will be increased by.

    characterSay("Ok I will not heal you (but I can), for this service, I can increase your max HP.",1); // Tell the player what they are gonna be able to do.
    characterSay("The tech I have here can give you more constitution, but at a price(Note that if you increase it at all, I cannot increase it again, I can only do it once)...",1);

    characterSay("I can increase your health by 20, 40 or 60 HP; just tell me 20, 40 or 60 (or the word for such, such as twenty, forty etc, in any case structure) to increase your health by that much.",1);
    characterSay("It will cost 100 gold per 20 HP, so to increase your HP by 60, it will cost 300 gold for example, just tell me how much you want it increased by if by anything at all.",1); // Now get the player's answer.
    // after confirming how to give the answer the player may want to give...
    tempAns = getStringAnswer(tempAns); // Get the answer the player gives.

    player->playerSay(tempAns); // The player says the answer they gave.

    tempAns = comAd->toUpperCase(tempAns); // Change this value to upper case as per usual...

    if (hpAlreadyIncresed == true) // This supp has already incresed the player's HP so do nothing to this value...
    {
        characterSay("Aw, you have already done one HP increase request, I cannot do another as my machine can only do it once it seems.",1); // Tell the player that only one request can be done.
    }
    else if (hpAlreadyIncresed == false) // The supp has not increased the player's HP so...
    {
        if (tempAns == "20" || tempAns == "TWENTY") // The player wants a HP increase of 20HP so...
        {
            costOfHPInc = 100; // Set the price to the correct price as per what was said.
            hpInc = 20; // Increase the player's HP by this much.

            if (player->getGoldCount() < costOfHPInc) // The player does not have enough gold for this.
            {
                comAd->comAdSay("ERROR: you do not have enough gold to increase your HP",1); // The comAd confirms this.
                characterSay("Aw, guess you did not have enough gold then, ah well, I can do other things...",1); // This supp reminds the player they can do other tasks.
            }
            else if (player->getGoldCount() >= costOfHPInc) // The player has enough gold for this.
            {
                negCostOfHPInc = -costOfHPInc; // Get the negated value.
                player->goldChange(negCostOfHPInc); // Do the function parse here.
                comAd->playerLosesGold(player->getName(),costOfHPInc,player->getGoldCount());

                player->changeMaxHealth(hpInc,comAd); // Change the player's max health by the value.
                characterSay("There we go, you are now more beefy, good luck out there!",1); // The char spurs on the player a bit perhaps?
                hpAlreadyIncresed = true; // Set this value to true now as this can increase to the player's HP can only be done once.
            }
        }
        else if (tempAns == "40" || tempAns == "FORTY") // The player wants a HP increase of 40HP so...
        {
            costOfHPInc = 200; // Set the price to the correct price as per what was said.
            hpInc = 40; // Increase the player's HP by this much.

            if (player->getGoldCount() < costOfHPInc) // The player does not have enough gold for this.
            {
                comAd->comAdSay("ERROR: you do not have enough gold to increase your HP",1); // The comAd confirms this.
                characterSay("Aw, guess you did not have enough gold then, ah well, I can do other things...",1); // This supp reminds the player they can do other tasks.
            }
            else if (player->getGoldCount() >= costOfHPInc) // The player has enough gold for this.
            {
                negCostOfHPInc = -costOfHPInc; // Get the negated value.
                player->goldChange(negCostOfHPInc); // Do the function parse here.
                comAd->playerLosesGold(player->getName(),costOfHPInc,player->getGoldCount());

                player->changeMaxHealth(hpInc,comAd); // Change the player's max health by the value.
                characterSay("There we go, you are now more beefy, good luck out there!",1); // The char spurs on the player a bit perhaps?
                hpAlreadyIncresed = true; // Set this value to true now as this can increase to the player's HP can only be done once.
            }
        }
        else if (tempAns == "60" || tempAns == "SIXTY") // The player wants a HP increase of 60HP so...
        {
            costOfHPInc = 300; // Set the price to the correct price as per what was said.
            hpInc = 60; // Increase the player's HP by this much.

            if (player->getGoldCount() < costOfHPInc) // The player does not have enough gold for this.
            {
                comAd->comAdSay("ERROR: you do not have enough gold to increase your HP",1); // The comAd confirms this.
                characterSay("Aw, guess you did not have enough gold then, ah well, I can do other things...",1); // This supp reminds the player they can do other tasks.
            }
            else if (player->getGoldCount() >= costOfHPInc) // The player has enough gold for this.
            {
                negCostOfHPInc = -costOfHPInc; // Get the negated value.
                player->goldChange(negCostOfHPInc); // Do the function parse here.
                comAd->playerLosesGold(player->getName(),costOfHPInc,player->getGoldCount());

                player->changeMaxHealth(hpInc,comAd); // Change the player's max health by the value.
                characterSay("There we go, you are now more beefy, good luck out there!",1); // The char spurs on the player a bit perhaps?
                hpAlreadyIncresed = true; // Set this value to true now as this can increase to the player's HP can only be done once.
            }
        }
        else // Otherwise the player does not give an answer that matches what was asked...
        {
            characterSay("Aw, guess you do not want a HP increase for now then or you gave me an invalid value to increase it by, shame...",1); // The supp takes a bit of pity on the player...
            // The hp check value is not set to true here.
        }
    }

    tempAns = ""; // Whipe the temp ans here.
}

void HealthAndAmmoSuppiler::increasePlayerWeightLim(Player *player,CombatAdmin *comAd) // This func seems to work in all cases(already done,10,20,30 random request) but eyes on...
{
    string tempAns = ""; // The answer the player will give.
    double costOfWeightInc = 0; // The cost of this service before negation.
    double negCostOfWeightInc = 0; // The cost of this service subtracted from the player.
    double weightInc = 0; // The weight value the player will be increased by.

    characterSay("Ok, for a good price I can increase your strength a bit so you can carry more.",1); // Tell the player about what is gonna be done.
    characterSay("This machine is very helpful in this regard, you do nothing, yet you can get stronger!",1);

    characterSay("Right, it will cost 200 gold per 10KG weight limit increase.",1);
    characterSay("You can only give me one request, and it has to either be an increase of 10,20 or 30KGS.",1);
    characterSay("So tell me the number or the word (ten,twenty etc, in any case structure) if you want an increase, anything else to not have one.",1);

    // after confirming how to give the answer the player may want to give...
    tempAns = getStringAnswer(tempAns); // Get the answer the player gives.

    player->playerSay(tempAns); // The player says the answer they gave.

    tempAns = comAd->toUpperCase(tempAns); // Change this value to upper case as per usual...

    if (weightAlreadyInc == true) // This supp has already incresed the player's weight lim so do nothing to this value...
    {
        characterSay("Aw, you have already done one weight limit increase request, I cannot do another as my machine can only do it once it seems.",1); // Tell the player that only one request can be done.
    }
    else if (weightAlreadyInc == false) // Otherwise they can increase the player's weight so...
    {
        if (tempAns == "10" || tempAns == "TEN") // The player wants a weight limit increase of 10KGS so...
        {
            costOfWeightInc = 200; // Set the price to the correct price as per what was said.
            weightInc = 10; // Increase the player's weight limit by this much.

            if (player->getGoldCount() < costOfWeightInc) // The player does not have enough gold for this.
            {
                comAd->comAdSay("ERROR: you do not have enough gold to increase your weight limit.",1); // The comAd confirms this.
                characterSay("Aw, guess you did not have enough gold then, ah well, I can do other things...",1); // This supp reminds the player they can do other tasks.
            }
            else if (player->getGoldCount() >= costOfWeightInc) // The player has enough gold for this.
            {
                negCostOfWeightInc = -costOfWeightInc; // Get the negated value.
                player->goldChange(negCostOfWeightInc); // Do the function parse here.
                comAd->playerLosesGold(player->getName(),costOfWeightInc,player->getGoldCount()); // Say that the player has lost gold.

                player->changeMaxCarryLim(weightInc,comAd); // Change the player's weight limit by this value.
                characterSay("There we go, you are now a bit stronger, good luck out there!",1); // The char spurs on the player a bit perhaps?
                weightAlreadyInc = true; // Set this value to true now as this increase to the player's weight limit can only be done once.
            }
        }
        else if (tempAns == "20" || tempAns == "TWENTY") // The player wants a weight limit increase of 20KGS so...
        {
            costOfWeightInc = 400; // Set the price to the correct price as per what was said.
            weightInc = 20; // Increase the player's weight limit by this much.

            if (player->getGoldCount() < costOfWeightInc) // The player does not have enough gold for this.
            {
                comAd->comAdSay("ERROR: you do not have enough gold to increase your weight limit.",1); // The comAd confirms this.
                characterSay("Aw, guess you did not have enough gold then, ah well, I can do other things...",1); // This supp reminds the player they can do other tasks.
            }
            else if (player->getGoldCount() >= costOfWeightInc) // The player has enough gold for this.
            {
                negCostOfWeightInc = -costOfWeightInc; // Get the negated value.
                player->goldChange(negCostOfWeightInc); // Do the function parse here.
                comAd->playerLosesGold(player->getName(),costOfWeightInc,player->getGoldCount()); // Say that the player has lost gold.

                player->changeMaxCarryLim(weightInc,comAd); // Change the player's weight limit by this value.
                characterSay("There we go, you are now a bit stronger, good luck out there!",1); // The char spurs on the player a bit perhaps?
                weightAlreadyInc = true; // Set this value to true now as this increase to the player's weight limit can only be done once.
            }
        }
        else if (tempAns == "30" || tempAns == "THIRTY") // The player wants a weight limit increase of 30KGS so...
        {
            costOfWeightInc = 600; // Set the price to the correct price as per what was said.
            weightInc = 30; // Increase the player's weight limit by this much.

            if (player->getGoldCount() < costOfWeightInc) // The player does not have enough gold for this.
            {
                comAd->comAdSay("ERROR: you do not have enough gold to increase your weight limit.",1); // The comAd confirms this.
                characterSay("Aw, guess you did not have enough gold then, ah well, I can do other things...",1); // This supp reminds the player they can do other tasks.
            }
            else if (player->getGoldCount() >= costOfWeightInc) // The player has enough gold for this.
            {
                negCostOfWeightInc = -costOfWeightInc; // Get the negated value.
                player->goldChange(negCostOfWeightInc); // Do the function parse here.
                comAd->playerLosesGold(player->getName(),costOfWeightInc,player->getGoldCount()); // Say that the player has lost gold.

                player->changeMaxCarryLim(weightInc,comAd); // Change the player's weight limit by this value.
                characterSay("There we go, you are now a bit stronger, good luck out there!",1); // The char spurs on the player a bit perhaps?
                weightAlreadyInc = true; // Set this value to true now as this increase to the player's weight limit can only be done once.
            }
        }
        else // Otherwise the player does not give an answer that matches what was asked...
        {
            characterSay("Aw, guess you do not want a weight limit increase for now then or you gave me an invalid value to increase it by, shame...",1); // The supp takes a bit of pity on the player...
            // The weight lim check value is not set to true here.
        }
    }

    tempAns = ""; // Whipe the temp ans here.
}
