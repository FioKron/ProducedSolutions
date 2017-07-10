#include <CombatAdmin.h>
#include <Windows.h>
#include <iostream>
#include <Sound.h>
#include <Narrator.h>
#include <sstream>
#include <enemy.h>
#include <vector>
#include <iomanip> // For manipulating the value conversion will be precise too etc.


CombatAdmin::CombatAdmin() // Default constructor so to speak.
{
    //Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.

    enemyHasUsedTargetLockSuccess = false; // Make this check false to begin with.
    timesToAttack = 0; // Start this var off at 0 until it happens...
}

string CombatAdmin::doubleToString(double number) // Both of these functions have more precision to them at the moment, but leave them both so that combat works properly now (in how damage is displayed etc).
{
    ostringstream oss; // Converts from double to string.

    //Perform the conversion and return the results.

    oss << fixed << setprecision(2) << number; // Set precision to 2 decimal places, same in other functions like this too.
    return oss.str();
}

string CombatAdmin::intToString(int number)
{
    ostringstream oss; // Converts from int to string.

    //Perform the conversion and return the results.

    oss << fixed << setprecision(2) << number;
    return oss.str();
}

void CombatAdmin::healthSet(double newHealth, string playerName) // The player's health has been changed, maybe use this func some time...
{
    string strHealth = doubleToString(newHealth); // Convert the health to a string
    comAdSay("Your health; " + playerName + ". Has been set to " + strHealth + ".",1); // State the results like so.
}

void CombatAdmin::comAdSay(string sayWhat,int messageType) // The Combat Admin say something...
{
    if (messageType == 1) // The combat admin prints the text and ends the line.
    {
        cout << "COMBATADMIN:" + sayWhat << endl; // Print the COMBATADMIN tag and what it says.
        Sleep(3000); // Wait.
    }
    else if (messageType == 2) // The combat admin prints the text without ending the line (so more text can be printed on the same line for example).
    {
        cout << "COMBATADMIN:" + sayWhat; // Print the COMBATADMIN tag and what it says.
        //Sleep(2000);
    }
    else if (messageType == 3) // Do not say the tag or print a new line. Remember that I have lowered the wait to 1/10th of a second now...
    {
        cout << sayWhat;
        Sleep(100);// Wait for not too long...
    }
    else if (messageType == 4) // The combat admin prints the text but does end the line, just prints it without the combat admin's name tag.
    {
        cout << sayWhat << endl; // Does not print the COMBAT ADMIN tag but does do an endl.
        Sleep(100); // Wait, but for much less so, e.g if the items the player has need to be listed they can be listed quickly.
    }
    else if (messageType == 5)
    {
        cout << "COMBATADMIN:" + sayWhat << endl; // Print the tag and end the line as normal.
        Sleep (500); // Wait, but as this is the message type of not having to wait too longer only wait for a 1/6 of the time as per the first message type.
    }

}

void CombatAdmin::stateCurrentEquipmentStats(vector<Item *> items) // State the stats of the current equipment the player has, idk why this is here...
{

}

void CombatAdmin::playerFindsChest(Player *player,Weapon *weapon,Armour *armour)
{
    double tempGold; // The gold the player finds from this chest.

    comAdSay(player->getName() + " has found a chest!",1);

    tempGold = rand() % 1000;
    player->goldChange(tempGold);
    playerFindsGold(player->getName(), tempGold, player->getGoldCount());

    // Test the weight of the items.

    double testWeightWep = player->getCurrentWeight() + weapon->getItemWeight(); // Testing of the weight of the weapon and armour need to be in different places.

    if (testWeightWep > player->getMaxWeight()) // The player did not have enough weight left to carry the weapon.
    {
        comAdSay("ERROR: " + player->getName() + " does not have enough weight left to wield this weapon.",1);
        // Perhaps add in a way for the player to discard an item.
    }
    else if (testWeightWep <= player->getMaxWeight()) // The player is below or has just reached their weight limit.
    {
        player->addBackPackItem(this,weapon,false);
        player->weightChange(weapon->getItemWeight(),this);

        playerFindsItem(player,weapon,2);
    }

    double testWeightArm = player->getCurrentWeight() + armour->getItemWeight(); // After adding the weight of the weapon if the player was able to hold it, test the weight of the armour.

    if (testWeightArm > player->getMaxWeight()) // The player did not have enough weight left to carry the armour.
    {
        comAdSay("ERROR: " + player->getName() + " does not have enough weight left to weild this armour.",1);
        // Perhaps add in a way for the player to discard an item.
    }
    else if (testWeightArm <= player->getMaxWeight()) // The player is below or has just reached their weight limit.
    {
        player->addBackPackItem(this,armour,false);
        player->weightChange(armour->getItemWeight(),this);

        playerFindsItem(player,armour,2);
    }
}

void CombatAdmin::playerAoeAtkk(Player *player,Enemy *enemy1,Enemy *enemy2,int combatSpeed) // Do an AOE attack.
{
    if (player->getActiveWeapon()->getUsesAmmo() == true)// If the player's weapon uses ammo...
    {
        if (player->getActiveWeapon()->getAmmoCnt() == 0) // if the player's weapon is out of ammo they can not attack, else check to see if the weapon is jammable.
        {
            comAdSay("This weapon is out of ammo! Attack failed!",combatSpeed);
        }
        else // This weapon has ammo
        {
            if (player->getActiveWeapon()->getIsJamable() == true) // If the player's weapon is jammable....
            {
                if(player->getActiveWeapon()->hasJammed() == true) // Check to see if the weapon jams.
                {
                    comAdSay("Weapon jammed! Attack failed!",combatSpeed); // If the weapon is jammed no ammo is used.
                }
                else // For weapons that jam and use ammo.
                {
                    playerAttkJamAmmo(player,enemy1,combatSpeed,true); // As this an AOE attack, attack both the enemies by doing half dmg to each as handled by the attack function.
                    playerAttkJamAmmo(player,enemy2,combatSpeed,true);
                }
            }
            else // This weapon uses ammo but does not jam.
            {
                playerAttkNonJamAmmo(player,enemy1,combatSpeed,true); // Remember, both enemies are hit by this AOE attack, so hit them both.
                playerAttkNonJamAmmo(player,enemy2,combatSpeed,true);
            }
        }
    }
    else if (player->getActiveWeapon()->getUsesAmmo() == false && player->getActiveWeapon()->getIsJamable() == false) // For weapons that are neither jammable nor use ammo.
    {
        playerAttkNonJamNonAmmo(player,enemy1,combatSpeed,true);
        playerAttkNonJamNonAmmo(player,enemy2,combatSpeed,true);
    }
    else if (player->getActiveWeapon()->getUsesAmmo() == false && player->getActiveWeapon()->getIsJamable() == true) // For weapons that jam but do not use ammo.
    {
        if (player->getActiveWeapon()->getIsJamable() == true) // If the player's weapon is jammable....
        {
            if(player->getActiveWeapon()->hasJammed() == true) // Check to see if the weapon jams.
            {
                comAdSay("Weapon jammed! Attack failed!",combatSpeed); // If the weapon is jammed no ammo is used.
            }
            else // For weapons that jam and do not ammo.
            {
                playerAttkJamNonAmmo(player,enemy1,combatSpeed,true); // As this an AOE attack, attack both the enemies by doing half dmg to each as handled by the attack function.
                playerAttkJamNonAmmo(player,enemy2,combatSpeed,true);
            }
        }
    }
}

void CombatAdmin::playerBasicAttack(Weapon *unarmed,Player *player,Enemy *enemy1,Enemy *enemy2,int combatSpeed,bool isMultiCombat) // For the player's basic attack.
{
    string temp = ""; // Make a temp var here too and make it equal to nothing
    // Remember, the last var in playerAttketc is false in this player's basic attack function as it will check if the player's weapon has AOE or not.

    if (player->getActiveWeapon()->getUsesAmmo() == true)// If the player's weapon uses ammo...
    {
        if (player->getActiveWeapon()->getAmmoCnt() == 0) // if the player's weapon is out of ammo they can not attack, else check to see if the weapon is jammable.
        {
            comAdSay("This weapon is out of ammo! Attack failed!",combatSpeed);
            comAdSay("You will have to use your fists therefore, these will be equipped now.",combatSpeed);
            player->equipWeapon(unarmed,this);
        }
        else
        {
            if (player->getActiveWeapon()->getIsJamable() == true) // If the player's weapon is jammable....
            {
                if(player->getActiveWeapon()->hasJammed() == true) // Check to see if the weapon jams.
                {
                    comAdSay("Weapon jammed! Attack failed!",combatSpeed); // If the weapon is jammed no ammo is used.
                }
                else // For weapons that jam and use ammo.
                {
                    if (isMultiCombat == true) // This combat has multiple enemies, may need even more code for choing the target if there is 3v1 etc.
                    {
                        if (player->getActiveWeapon()->getHasAoe() == true) // If the weapon has AOE, ask the player if they want to use it.
                        {
                            // Ask the player what they want to do.
                            comAdSay("Does " + player->getName() + " want to do an AOE(area of effect) attack, enter yes or sure to do so, or no or nope to not do so.",combatSpeed);
                            temp == ""; // Clear the temp var for now.

                            do
                            {
                                getline(cin, temp);
                            } while(temp == ""); // Get input

                            player->playerSay(temp);

                            temp = toUpperCase(temp);

                            if (temp == "YES" || temp == "SURE") // If they want to use AOE...
                            {
                                playerAoeAtkk(player,enemy1,enemy2,combatSpeed); // Do an AOE attack from the player versus all enemies.
                            }
                            else if (temp == "NO" || temp == "NOPE") // If not...
                            {
                                temp == ""; // Clear the temp var here too.
                                goto notUsingAoe1; // Simple use this goto to skip this bit of the if statement, and assume the weapon did not have AOE capability so this is not used.
                                // All goto tags must be different though.
                            }
                            else
                            {
                                comAdSay("As you have not given a sensible answer, no AOE will be used for this attack.",combatSpeed); // Be kindish to the player.
                                temp == ""; // Clear the temp var here too.
                                goto notUsingAoe1; // Use the goto tag again.
                            }
                        }
                        else if (player->getActiveWeapon()->getHasAoe() == false) // If the weapon does not have AOE, ask the freindly who they want to target.
                        {
                            notUsingAoe1: // The goto tag, easy way I could think of to not use AOE in this case.

                            comAdSay("Who does " + player->getName() + " want to target? (enter 1 or 2 for enemy 1 or 2 ,or one or two in any case structure, or enter the target's name in any case structure)",combatSpeed); // Ask who the player wants to attack in this combat.

                            do // Do while is needed here.
                            {
                                getline(cin, temp);
                            } while(temp == "");

                            player->playerSay(temp);

                            temp = toUpperCase(temp);

                            // Remember to boost both the enemy names to uppercase or entering thier name will not work.

                            if (temp == "1" || temp == "ONE"|| temp == toUpperCase(enemy1->getName())) // Check which target the player wants to attack.
                            {
                                if (enemy1->getHealth() == 0) // The player's attack on this target fails as they are already dead.
                                {
                                    comAdSay("ERROR: this target has already been defeated!",combatSpeed); // The combat admin lets out a cry at this so to speak.
                                }
                                else if (enemy1->getHealth() > 0) // This attack does function as intended.
                                {
                                    playerAttkJamAmmo(player,enemy1,combatSpeed,false); // The player attacks the first enemy.
                                }
                            }
                            else if (temp == "2" || temp == "TWO" || temp == toUpperCase(enemy2->getName()))
                            {
                                if (enemy2->getHealth() == 0) // The player's attack on this target fails as the target is already dead.
                                {
                                    comAdSay("ERROR: this target has already been defeated!",combatSpeed); // The combat admin lets out a cry at this so to speak.
                                }
                                else if (enemy2->getHealth() > 0) // This attack does function as intended.
                                {
                                    playerAttkJamAmmo(player,enemy2,combatSpeed,false); // The player attacks the second enemy(this comment seems to be wrong further down, may change this later...).
                                }
                            }
                            else // The player has chosen an invalid target.
                            {
                                comAdSay("ERROR: you have chosen an invalid target.",combatSpeed);
                            }
                        }
                    }
                    else if (isMultiCombat == false) // There is only one enemy which in 1v1 will always be enemy1.
                    {
                        playerAttkJamAmmo(player,enemy1,combatSpeed,false);
                    }
                }
            }
            else // For weapons that use ammo but do not jam.
            {
                if (isMultiCombat == true) // There are multiple enemies.
                {
                    if (player->getActiveWeapon()->getHasAoe() == true) // If the weapon has AOE, ask the player if they want to use it.
                    {
                        // Ask the player what they want to do.
                        comAdSay("Does " + player->getName() + " want to do an AOE(area of effect) attack, enter yes or sure to do so, or no or nope to not do so.",combatSpeed);
                        temp == ""; // Clear the temp var for now.

                        do
                        {
                            getline(cin, temp);
                        } while(temp == ""); // Get input

                        player->playerSay(temp);

                        temp = toUpperCase(temp);

                        if (temp == "YES" || temp == "SURE") // If they want to use AOE...
                        {
                            playerAoeAtkk(player,enemy1,enemy2,combatSpeed); // Do an AOE attack from the player versus all enemies.
                        }
                        else if (temp == "NO" || temp == "NOPE") // If not...
                        {
                            temp == ""; // Clear the temp var here too.
                            goto notUsingAoe2; // Simple use this goto to skip this bit of the if statement, and assume the weapon did not have AOE capability.
                        }
                        else
                        {
                            comAdSay("As you have not given a sensible answer, no AOE will be used for this attack.",combatSpeed); // Be kindish to the player.
                            temp == ""; // Clear the temp var here too.
                            goto notUsingAoe2; // Use the goto tag again.
                        }
                    }
                    else if (player->getActiveWeapon()->getHasAoe() == false) // If the weapon does not have AOE, ask the freindly who they want to target.
                    {
                        notUsingAoe2:

                        comAdSay("Who does " + player->getName() + " want to target? (enter 1 or 2 for enemy 1 or 2 ,or one or two in any case structure, or enter the target's name in any case structure)",combatSpeed); // Ask who the player wants to attack in this combat.

                        do // Do while is needed here.
                        {
                            getline(cin, temp);
                        } while(temp == "");

                        player->playerSay(temp);

                        temp = toUpperCase(temp);

                        if (temp == "1" || temp == "ONE"|| temp == toUpperCase(enemy1->getName())) // Check which target the player wants to attack.
                        {
                            if (enemy1->getHealth() == 0) // The player's attack on this target fails as they are already dead.
                            {
                                comAdSay("ERROR: this target has already been defeated!",combatSpeed); // The combat admin lets out a cry at this so to speak.
                            }
                            else if (enemy1->getHealth() > 0) // This attack does function as intended.
                            {
                                playerAttkNonJamAmmo(player,enemy1,combatSpeed,false); // The player attacks the first enemy.
                            }
                        }
                        else if (temp == "2" || temp == "TWO" || temp == toUpperCase(enemy2->getName()))
                        {
                            if (enemy2->getHealth() == 0) // The player's attack on this target fails as they are already dead.
                            {
                                comAdSay("ERROR: this target has already been defeated!",combatSpeed); // The combat admin lets out a cry at this so to speak.
                            }
                            else if (enemy2->getHealth() > 0) // This attack does function as intended.
                            {
                                playerAttkNonJamAmmo(player,enemy2,combatSpeed,false); // The player attacks the first enemy.
                            }
                        }
                        else // The player has chosen an invalid target.
                        {
                            comAdSay("ERROR: you have chosen an invalid target.",combatSpeed);
                        }
                    }
                }
                else if (isMultiCombat == false)// There is only one.
                {
                    playerAttkNonJamAmmo(player,enemy1,combatSpeed,false);
                }
            }
        }
    }
    else if (player->getActiveWeapon()->getUsesAmmo() == false && player->getActiveWeapon()->getIsJamable() == false) // For weapons that are neither jammable nor use ammo.
    {
        if (isMultiCombat == true) // There are multiple enemies.
        {
            if (player->getActiveWeapon()->getHasAoe() == true) // If the weapon has AOE, ask the player if they want to use it.
            {
                // Ask the player what they want to do.
                comAdSay("Does " + player->getName() + " want to do an AOE(area of effect) attack, enter yes or sure to do so, or no or nope to not do so.",combatSpeed);
                temp == ""; // Clear the temp var for now.

                do
                {
                    getline(cin, temp);
                } while(temp == ""); // Get input

                player->playerSay(temp);

                temp = toUpperCase(temp);

                if (temp == "YES" || temp == "SURE") // If they want to use AOE...
                {
                    playerAoeAtkk(player,enemy1,enemy2,combatSpeed); // Do an AOE attack from the player versus all enemies.
                }
                else if (temp == "NO" || temp == "NOPE") // If not...
                {
                    temp == ""; // Clear the temp var here too.
                    goto notUsingAoe3; // Simple use this goto to skip this bit of the if statement, and assume the weapon did not have AOE capability.
                }
                else
                {
                    comAdSay("As you have not given a sensible answer, no AOE will be used for this attack.",combatSpeed); // Be kindish to the player.
                    temp == ""; // Clear the temp var here too.
                    goto notUsingAoe3; // Use the goto tag again.
                }
            }
            else if (player->getActiveWeapon()->getHasAoe() == false) // If the weapon does not have AOE, ask the freindly who they want to target.
            {
                notUsingAoe3:

                comAdSay("Who does " + player->getName() + " want to target? (enter 1 or 2 for enemy 1 or 2 ,or one or two in any case structure, or enter the target's name in any case structure)",combatSpeed); // Ask who the player wants to attack in this combat.

                do // Do while is needed here.
                {
                    getline(cin, temp);
                } while(temp == "");

                player->playerSay(temp);

                temp = toUpperCase(temp);

                if (temp == "1" || temp == "ONE"|| temp == toUpperCase(enemy1->getName())) // Check which target the player wants to attack.
                {
                    if (enemy1->getHealth() == 0) // The player's attack on this target fails as they are already dead.
                    {
                        comAdSay("ERROR: this target has already been defeated!",combatSpeed); // The combat admin lets out a cry at this so to speak.
                    }
                    else if (enemy1->getHealth() > 0) // This attack does function as intended.
                    {
                        playerAttkNonJamNonAmmo(player,enemy1,combatSpeed,false); // The player attacks the first enemy.
                    }
                }
                else if (temp == "2" || temp == "TWO" || temp == toUpperCase(enemy2->getName()))
                {
                    if (enemy2->getHealth() == 0) // The player's attack on this target fails as they are already dead.
                    {
                        comAdSay("ERROR: this target has already been defeated!",combatSpeed); // The combat admin lets out a cry at this so to speak.
                    }
                    else if (enemy2->getHealth() > 0) // This attack does function as intended.
                    {
                        playerAttkNonJamNonAmmo(player,enemy2,combatSpeed,false); // The player attacks the first enemy.
                    }
                }
                else // The player has chosen an invalid target.
                {
                    comAdSay("ERROR: you have chosen an invalid target.",combatSpeed);
                }
            }
        }
        else if (isMultiCombat == false) // There is only one.
        {
            playerAttkNonJamNonAmmo(player,enemy1,combatSpeed,false);
        }
    }
    else if (player->getActiveWeapon()->getUsesAmmo() == false && player->getActiveWeapon()->getIsJamable() == true) // For weapons that jam but do not use ammo such as chainsaws.
    {
        if(player->getActiveWeapon()->hasJammed() == true) // Check to see if the weapon jams.
        {
            comAdSay("Weapon jammed! Attack failed!",combatSpeed);
        }
        else // For weapons that jam but do not use ammo.
        {
            if (isMultiCombat == true) // There are multiple enemies.
            {
                if (player->getActiveWeapon()->getHasAoe() == true) // If the weapon has AOE, ask the player if they want to use it.
                {
                    // Ask the player what they want to do.
                    comAdSay("Does " + player->getName() + " want to do an AOE(area of effect) attack, enter yes or sure to do so, or no or nope to not do so.",combatSpeed);
                    temp == ""; // Clear the temp var for now.

                    do
                    {
                        getline(cin, temp);
                    } while(temp == ""); // Get input

                    player->playerSay(temp);

                    temp = toUpperCase(temp);

                    if (temp == "YES" || temp == "SURE") // If they want to use AOE...
                    {
                        playerAoeAtkk(player,enemy1,enemy2,combatSpeed); // Do an AOE attack from the player versus all enemies.
                    }
                    else if (temp == "NO" || temp == "NOPE") // If not...
                    {
                        temp == ""; // Clear the temp var here too.
                        goto notUsingAoe4; // Simple use this goto to skip this bit of the if statement, and assume the weapon did not have AOE capability.
                    }
                    else
                    {
                        comAdSay("As you have not given a sensible answer, no AOE will be used for this attack.",combatSpeed); // Be kindish to the player.
                        temp == ""; // Clear the temp var here too.
                        goto notUsingAoe4; // Use the goto tag again.
                    }
                }
                else if (player->getActiveWeapon()->getHasAoe() == false) // If the weapon does not have AOE, ask the freindly who they want to target.
                {
                    notUsingAoe4:

                    comAdSay("Who does " + player->getName() + " want to target? (enter 1 or 2 for enemy 1 or 2 ,or one or two in any case structure, or enter the target's name in any case structure)",combatSpeed); // Ask who the player wants to attack in this combat.

                    do // Do while is needed here.
                    {
                        getline(cin, temp);
                    } while(temp == "");

                    player->playerSay(temp);

                    temp = toUpperCase(temp);

                    if (temp == "1" || temp == "ONE"|| temp == toUpperCase(enemy1->getName())) // Check which target the player wants to attack.
                    {
                        if (enemy1->getHealth() == 0) // The player's attack on this target fails as they are already dead.
                        {
                            comAdSay("ERROR: this target has already been defeated!",combatSpeed); // The combat admin lets out a cry at this so to speak.
                        }
                        else if (enemy1->getHealth() > 0) // This attack does function as intended.
                        {
                            playerAttkJamNonAmmo(player,enemy1,combatSpeed,false); // The player attacks the first enemy.
                        }
                    }
                    else if (temp == "2" || temp == "TWO" || temp == toUpperCase(enemy2->getName()))
                    {
                        if (enemy2->getHealth() == 0) // The player's attack on this target fails as they are already dead.
                        {
                            comAdSay("ERROR: this target has already been defeated!",combatSpeed); // The combat admin lets out a cry at this so to speak.
                        }
                        else if (enemy2->getHealth() > 0) // This attack does function as intended.
                        {
                            playerAttkJamNonAmmo(player,enemy2,combatSpeed,false); // The player attacks the first enemy.
                        }
                    }
                    else // The player has chosen an invalid target.
                    {
                        comAdSay("ERROR: you have chosen an invalid target.",combatSpeed);
                    }
                }
            }
            else if (isMultiCombat == false) // There is only one.
            {
                playerAttkJamNonAmmo(player,enemy1,combatSpeed,false);
            }
        }
    }
    // Be careful in making sure the correct enemy is targeted if they have already been defeated so to speak so nothing happens if the player targets them.
    // Nothing here once again for deciding if the enemy or enemies the player was facing have been defeated as that is handle via combat round/ encounter code etc.
}

void CombatAdmin::enemyBasicAttack(Weapon *unarmed,Player *player,Enemy *enemy,Narrator *narrator,Sound *sound,int combatSpeed) // This may need to be changed in the future so as well as the player having a choice of who to attack, the enemy must decide which friendly to attack too.
{
    if (enemy->getActiveWeapon()->getUsesAmmo() == true)// Same checks as for the player's weapon, check to see if weapon uses ammo-
    // attack it if does not else, check to see if the weapon has ammo, if it does not, attack fails, else, check to see-
    // if weapon jams, if it does, attack fails else attack goes through (and uses ammo).
    {
        if (enemy->getActiveWeapon()->getAmmoCnt() == 0)
        {
            comAdSay("The enemy tried to attack but their weapon was out of ammo!",combatSpeed);
            enemy->enemySay("Grr out of ammo!");
            enemy->enemySay("Guess I will just punch you then!"); // This enemy changes to their fists.
            enemy->autoEquip(unarmed,enemy->getActiveArmour()); // But using the auto equip func, their armour has to be re-equipped by getting their active armour...
        }
        else
        {
            if (enemy->getActiveWeapon()->getIsJamable() == true)
            {
                if(enemy->getActiveWeapon()->hasJammed())
                {
                    comAdSay("The enemy tried to attack but their weapon jammed!",combatSpeed);
                    enemy->enemySay("Gah, useless thing!");
                }
                else  // Same as above.
                {
                    enemyAttkJamAmmo(player,enemy,combatSpeed);
                }
            }
            else // This weapon uses ammo but does not jam.
            {
                enemyAttkNonJamAmmo(player,enemy,combatSpeed);
            }
        }

    }
    else if (enemy->getActiveWeapon()->getUsesAmmo() == false && enemy->getActiveWeapon()->getIsJamable() == false) // The enemy attacks with a weapon that does not use ammo and does not jam.
    {
        enemyAttkNonJamNonAmmo(player,enemy,combatSpeed);
    }
    else if (enemy->getActiveWeapon()->getUsesAmmo() == false && enemy->getActiveWeapon()->getIsJamable() == true) // The enemy attacks with a weapon that does not use ammo but can jam.
    {
        if(player->getActiveWeapon()->hasJammed() == true) // Check to see if the weapon jams.
        {
            comAdSay("The enemy tried to attack but their weapon jammed!",combatSpeed);
            enemy->enemySay("Gah, useless thing!");
        }
        else // For weapons that jam but do not use ammo.
        {
            enemyAttkJamNonAmmo(player,enemy,combatSpeed);
        }
    }

    if (player->getHealth() == 0) // Player was defeated by this attack.
    {
        enemy->enemyTaunt(); // This enemy taunts for killing the player while the other one does nothing if this was a 2v1.

        entDefeated(player->getName(),combatSpeed);

        sound->playSoundNonLooped("Fightend.wav");

        narrator->narratorSay("Dead",3); // A loss state has been met.
    }
}

void CombatAdmin::entsHaveJoinedYourParty(Enemy *firstEnt,Enemy *secondEnt) // Some ents have joined the player's party.
{
    comAdSay(firstEnt->getName() + " and " + secondEnt->getName() + " have joined your party!",1); // So tell the player.
    // Maybe say how many are in the player's party therefore.
    // This func may need amending later.
}

void CombatAdmin::comAdWarning(string enemyName1,string enemyName2,int enemyNum,int combatSpeed) // Warn the player of enemies.
{
    if (enemyNum == 1) // Enemy num is for the number of enemies in this combat, so if just one, the comAd tells the player about the one enemey present, and if two, the two enemies present etc.
    {
        comAdSay("Warning: the enemy known as'" + enemyName1 + "',has Been detected. Starting combat.",combatSpeed);
    }
    else if (enemyNum == 2)
    {
        comAdSay("Warning: the enemies known as '" + enemyName1 + "' and '" + enemyName2 + "',have been detected. Starting combat.",combatSpeed);
    }
}

void CombatAdmin::comAdAtkNote(string attack, double damage,string target,string aggresor,int combatSpeed) // Note the damage done, but don't round the values.
{
    string strDamage = doubleToString(damage); // Convert that value to a string.
    comAdSay(aggresor + "'s " + attack + " hit " + target + " for " + strDamage + " points of damage!",combatSpeed); // State what has happened.
}

void CombatAdmin::entDefeated(string entName,int combatSpeed)
{
    comAdSay(entName + " has been defeated!",combatSpeed);
}

void CombatAdmin::comAdStateEntHp(string ent, double hp,int combatSpeed) // State the hp of the ent but don't round the values.
{
    string strHp = doubleToString(hp); // Convert that value to a string.
    comAdSay(ent + "'s health is now " + strHp + " !",combatSpeed); // State the change.
}

void CombatAdmin::comAdStateScanResults(string enemyName, double enemyHealth,int combatSpeed) // State the scan results of the tutorial battle.
{
    string strHealth = doubleToString(enemyHealth); // Convert the health value to a string.

    comAdSay("The following information has been found out about the enemy:",combatSpeed); // Display the scan results.
    comAdSay("Name: " + enemyName + ".",combatSpeed);
    comAdSay("Health: " + strHealth + ".",combatSpeed);
    comAdSay("Weakness: projectiles that do not explode.",combatSpeed);
}

bool CombatAdmin::isRandEncounter() // Determine the chance of a random encounter occuring when there is a point where the player potentially has one.
{
    double encountChance; // Return true or false, encoutChance is used to dertermine whether this ecounter happened or not.
    bool isRandEncounter = false; // The chance.

    encountChance = rand() % 100; // 50% chance approx of a random encounter.

    if (encountChance <= 50) // Return true or false therefore.
    {
        isRandEncounter = true;
    }
    else
    {
        isRandEncounter = false;
    }

    return isRandEncounter;
}

// For some reason in this demo, the encounter code name needs to be such.
void CombatAdmin::randomEncounter1V1(Weapon *unarmed,Player *player,Enemy *enemy,Sound *sound,Narrator *narrator,int combatSpeed,bool playerHasScanner,bool playerHiredMercs,bool playerIsInWinter,bool playerHasSurgeNCo,bool playerHasEquipAbil,Weapon *machinePistol,Armour *lgBodyArm,Weapon *lightMG,Armour *hgBodyArm,Weapon *handGrenades,Armour *clunkyPowerSuit,Weapon *rocketLauncher,Weapon *battleRifle,Armour *lgPowArm,Armour *modArm) // Random encounters in the prologue and maybe 1 at the start of chapter 1.
{ // Combat speed is how fast the messages will go. This code is only for 1v1 random encounters, for all future random encounters make more functions.

    // Priv matt or any other enemies are passed into this function...

    bool escaped = false; // For whether the player has escaped or not.

    comAdWarning(enemy->getName(),"",1,combatSpeed); // Warn the player.
    sound->playSoundLooped("Fighting.wav"); // Play the fighting music.

    enemy->gotAggro();

    do // Keep doing combat rounds till the fight is over.
    {
        escaped = combatRound1V1(unarmed,player,enemy,narrator,sound,escaped,combatSpeed,playerHasScanner,playerHiredMercs,playerIsInWinter,playerHasSurgeNCo,playerHasEquipAbil,machinePistol,lgBodyArm,lightMG,hgBodyArm,handGrenades,clunkyPowerSuit,rocketLauncher,battleRifle,lgPowArm,modArm);
    }
    while ((player->getHealth() > 0) && (enemy->getHealth() > 0) && (escaped == false)); // Keep doing the combat round until one of these conditions is true.

    if (player->getHealth() == 0) // Player was defeated.
    {
        enemy->enemyTaunt();

        entDefeated(player->getName(),combatSpeed);

        sound->playSoundNonLooped("Fightend.wav");

        narrator->narratorSay("Dead",3); // A loss state has been met.
    }
    else if (enemy->getHealth() == 0) // The enemy has been defeated.
    {
        double tempGold; // The gold the player finds in the combat.

        tempGold = rand() % 200;
        player->goldChange(tempGold);

        enemy->enemyDeath();

        sound->playSoundNonLooped("Fightend.wav");

        entDefeated(enemy->getName(),combatSpeed);
        playerFindsGold(player->getName(), tempGold, player->getGoldCount());
    }

    if (escaped == true) // If the player got away...
    {
        enemy->enemySay("Looks like for you, escape would be the best option!"); // The enemy taunts the player.
    }
    // Priv matt or any other enemy pointers cannot be deleted here, as they may be used later. They are thus deleted just before the program ends in main.
}

void CombatAdmin::handleEnemySpecialAbilties(Weapon *unarmed,Player *player,Enemy *enemy,Narrator *narrator,Sound *sound,int combatSpeed) // Handle this enemy's special abilties, remember, they may have more then one spec abil so code will be needed for maybe more then one spec abil therefore.
{
    string enemyID = enemy->getName(); // Get this enemy's name to ID who they are.

    if (enemyID == "Mercenary Jones") // This enemy has the target lock spec ability; merc jones.
    {
        if (enemyHasUsedTargetLockSuccess == true) // Check whether they have used this ability, if so they just taunt the player with this fact.
        {
            enemy->enemySay("Hah! You have already been locked on to!"); // Say the taunt so to speak.
        }
        else if (enemyHasUsedTargetLockSuccess == false) // If they have not...
        {
            enemy->enemySay("Time to attempt a lock on!"); // Say that they will do this, but...
            int hasLockedOn = rand() % 100; // The chance of whether the enemy locks on or not, 50% chance.

            if (hasLockedOn >= 50) // They have locked on succesfully.
            {
                enemyHasUsedTargetLockSuccess = true; // So set this value to true.
                enemy->enemySay("Haha! you are locked on to sucker!"); // The enemy taunts the player.
                enemy->enemySay("Time to be shot at by me too!"); // The enemy still shoots at the player.
            }
            else if (hasLockedOn < 50) // They have not.
            {
                enemyHasUsedTargetLockSuccess = false; // So set this to the init value.
                enemy->enemySay("Argh! Damnit! you evaded my lock on!"); // This enemy is upset with this.
                enemy->enemySay("Guess I will just attempt to shoot at you then!"); // The enemy says what they are going to do next.
            }
        }
    }
    else if (enemyID == "Mercenary Smith") // This enemy is merc smith, with the smg spray spec abil so to speak.
    {
        if (enemyHasUsedTargetLockSuccess == false) // If the player has not been locked onto...
        {
            enemy->enemySay("Argh! why could you not lock onto " + player->getName() + " Jones!?"); // The other merc is annoyed by this therefore.
        }
        else if (enemyHasUsedTargetLockSuccess == true) // The player has been locked on to thus...
        {
            timesToAttack ++; // Increment the numbers of attacks in this volly by 1, thus if the player is locked onto, they take more and more dmg from this spec abil.

            enemy->enemySay("Time for more dakka!"); // The enemy implies they are gonna shoot the player a fair bit...
            comAdSay("WARNING:" + enemy->getName() +  " is about to attack you multiple times!",combatSpeed); // The comAd warns the player therefore.

            for (int counter = 0; counter < timesToAttack; counter++) // Do a loop for the number of times to attack here.
            {
                enemyBasicAttack(unarmed,player,enemy,narrator,sound,combatSpeed); // Remember, the enemy pointer should be this merc...check.
            }

            enemy->enemySay("One more shot to add to that!"); // Add a basic attack too.
            // This sorta makes sense...by locking on the other merc can spray at the player and thus empty more ammo but do more dmg without fear of missing...hmm...
        }
    }
    else if (enemyID == "Private Matterson") // Handle matterson's spec abils.
    {
        // They seem to have a porta mortar...check this spec abil works correctly.

        enemy->enemySay("Now you will see my secret(if you did not see before at least) weapon!"); // The enemy says what they are going to do(regardless of whether the player has seen this attack or not)...

        comAdSay("WARNING, " + enemy->getName() + " is about to fire an explosive porta' mortar shell at you!",combatSpeed);

        int mortarHit = rand() % 100; // For checking whether the mortar hit or not.

        if (mortarHit <= 80) // This mortar misses. Thus, this mortar thus has an accuracy of approx 20% (% chance).
        {
            comAdSay(enemy->getName() + "'s special attack missed!",combatSpeed); // The comAd tells the player that priv matt's attack missed.
            enemy->enemySay("Gah! I missed!");
            enemy->enemySay("Time to use ole' reliable then!"); // The enemy is annoyed at missing, they just do a basic attack(coded elsewhere).
        }
        else if (mortarHit > 80) // The mortar scores a hit, but the dmg this attack can do is between 10 and 30, if not a direct hit, may be due to AOE blargh etc.
        {
            double mortarDmg = rand() % 20 + 10; // Generate a random number between 10 and 30 for how much damage is done.

            comAdSay(enemy->getName() + "'s mortar has hit you!",combatSpeed); // The comAd tells the player they have been hit.
            enemy->enemySay("Oh yea! A hit!"); // The enemy is pleased.

            comAdAtkNote("Porta' mortar",mortarDmg,player->getName(),enemy->getName(),combatSpeed); // State the results as usual.
            mortarDmg = -mortarDmg; // Negate it to make it deal some dmg to the player.
            player->healthChange(mortarDmg); // Change the player's health, the player's armour does not help against this attack.
            comAdStateEntHp(player->getName(),player->getHealth(),combatSpeed);

            enemy->enemySay("Ole' reliable wants a shot too!"); // This enemy does a basic attack too(as per above).
        }
    }
}

void CombatAdmin::playerAttkJamAmmo(Player *player,Enemy *enemy,int combatSpeed,bool isAoeAttk) // The function for working out the damage the player does with weapons that jam and use ammo to an enemy.
{
    double playerDmg; // The damage the player does to an enemy.

    if (isAoeAttk == true) // If this attack is an AOE attack, divide the dmg up evenly between the enemies.
    {
        playerDmg = (player->getActiveWeapon()->getDamage() - (player->getActiveWeapon()->getDamage() * enemy->getActiveArmour()->getDmgResist())) / 2; // Get the player's final damage to the enemy.
        // At the moment, for AOE attacks, divide the dmg by 2 as there is currently only 2 enemies max per combat.
        player->getActiveWeapon()->ammoChange(-0.50,this); // Take off half a point of ammo as if this attack uses aoe only one ammo total is used. As 0.50 + 0.50 = 1.00 so still 1.
    }
    else if (isAoeAttk == false) // If it is not, subtract 1 ammo as per normal
    {
        playerDmg = player->getActiveWeapon()->getDamage() - (player->getActiveWeapon()->getDamage() * enemy->getActiveArmour()->getDmgResist()); // Get the player's final damage to the enemy.
        player->getActiveWeapon()->ammoChange(-1.00,this); // Subtract one ammmo (as this weapon uses ammo, weapons that do not use ammo skip this step) but remember to have 2 zero's after it to show ammo is a double.
    }

    comAdAtkNote(player->getActiveWeapon()->getItemName(), playerDmg, enemy->getName(), player->getName(),combatSpeed); // State what has(as far as the player knows) happened.
    playerDmg = -playerDmg; // negate the damage as we are taking it away from the enemy's health.
    enemy->healthChange(playerDmg); // here, change the enemy's health by a negative value, i.e. damage them.
    comAdStateEntHp(enemy->getName(), enemy->getHealth(),combatSpeed); // State what the enemy's health is now.

    string strAmmoLeft = doubleToString(player->getActiveWeapon()->getAmmoCnt()); // The ammo the player's active weapon has left.
    comAdSay("This weapon has " + strAmmoLeft + " round(s) left.",combatSpeed); // Maybe make it so the player is asked if they want to change the weapon in the future.
}

void CombatAdmin::playerAttkNonJamAmmo(Player *player,Enemy *enemy,int combatSpeed,bool isAoeAttk) // The function for working out the damage the player does with weapons that do not jam but use ammo to an enemy.
{
    double playerDmg;

    if (isAoeAttk == true) // If this attack is an AOE attack, divide the dmg up evenly between the enemies.
    {
        playerDmg = (player->getActiveWeapon()->getDamage() - (player->getActiveWeapon()->getDamage() * enemy->getActiveArmour()->getDmgResist())) / 2; // Get the player's final damage to the enemy.
        // At the moment, for AOE attacks, divide the dmg by 2 as there is currently only 2 enemies max per combat.
        player->getActiveWeapon()->ammoChange(-0.50,this); // Take off half a point of ammo as if this attack uses aoe only one ammo total is used. As 0.50 + 0.50 = 1.00 so still 1.
    }
    else if (isAoeAttk == false) // If it is not, subtract 1 ammo as per normal
    {
        playerDmg = player->getActiveWeapon()->getDamage() - (player->getActiveWeapon()->getDamage() * enemy->getActiveArmour()->getDmgResist()); // Get the player's final damage to the enemy.
        player->getActiveWeapon()->ammoChange(-1.00,this); // Subtract one ammmo (as this weapon uses ammo, weapons that do not use ammo skip this step) but remember to have 2 zero's after it to show ammo is a double.
    }

    // REMEMBER: if no ammo is used in a weapon, the isAoeAttk var does not need to be checked as no ammo will be taken away.

    comAdAtkNote(player->getActiveWeapon()->getItemName(), playerDmg, enemy->getName(), player->getName(),combatSpeed);
    playerDmg = -playerDmg;
    enemy->healthChange(playerDmg);
    comAdStateEntHp(enemy->getName(), enemy->getHealth(),combatSpeed);

    string strAmmoLeft = doubleToString(player->getActiveWeapon()->getAmmoCnt()); // The ammo the player's active weapon has left.
    comAdSay("This weapon has " + strAmmoLeft + " round(s) left.",combatSpeed); // Maybe make it so the player is asked if they want to change the weapon in the future.
}

void CombatAdmin::playerAttkNonJamNonAmmo(Player *player,Enemy *enemy,int combatSpeed,bool isAoeAttk) // The function for working out the damage the player does with weapons that do not jam nor use ammo to an enemy.
{
    double playerDmg;

    if (isAoeAttk == true) // if this attack is an AOE attack, divide the dmg up evenly between the enemies.
    {
        playerDmg = (player->getActiveWeapon()->getDamage() - (player->getActiveWeapon()->getDamage() * enemy->getActiveArmour()->getDmgResist())) / 2; // Get the player's final damage to the enemy.
        // At the moment, for AOE attacks, divide the dmg by 2 as there is currently only 2 enemies max per combat.
    }
    else if (isAoeAttk == false)
    {
        playerDmg = player->getActiveWeapon()->getDamage() - (player->getActiveWeapon()->getDamage() * enemy->getActiveArmour()->getDmgResist()); // Get the player's final damage to the enemy.
    }

    comAdAtkNote(player->getActiveWeapon()->getItemName(), playerDmg, enemy->getName(), player->getName(),combatSpeed);
    playerDmg = -playerDmg;
    enemy->healthChange(playerDmg);
    comAdStateEntHp(enemy->getName(), enemy->getHealth(),combatSpeed);
}

void CombatAdmin::playerAttkJamNonAmmo(Player *player,Enemy *enemy,int combatSpeed,bool isAoeAttk) // The function for working out the damage the player does with weapons that jam but do not use ammo.
{
    double playerDmg;

    if (isAoeAttk == true) // if this attack is an AOE attack, divide the dmg up evenly between the enemies.
    {
        playerDmg = (player->getActiveWeapon()->getDamage() - (player->getActiveWeapon()->getDamage() * enemy->getActiveArmour()->getDmgResist())) / 2; // Get the player's final damage to the enemy.
        // At the moment, for AOE attacks, divide the dmg by 2 as there is currently only 2 enemies max per combat.
    }
    else if (isAoeAttk == false)
    {
        playerDmg = player->getActiveWeapon()->getDamage() - (player->getActiveWeapon()->getDamage() * enemy->getActiveArmour()->getDmgResist()); // Get the player's final damage to the enemy.
    }

    comAdAtkNote(player->getActiveWeapon()->getItemName(), playerDmg, enemy->getName(), player->getName(),combatSpeed);
    playerDmg = -playerDmg;
    enemy->healthChange(playerDmg);
    comAdStateEntHp(enemy->getName(), enemy->getHealth(),combatSpeed);
}

void CombatAdmin::enemyAttkJamAmmo(Player *player,Enemy *enemy,int combatSpeed) // The next 4 functions including this one are the same as the player one's, just for the enemy.
{
    double enemyDmg;

    enemy->enemyTaunt();
    enemyDmg = enemy->getActiveWeapon()->getDamage() - (enemy->getActiveWeapon()->getDamage() * player->getActiveArmour()->getDmgResist());
    enemy->getActiveWeapon()->ammoChange(-1.00,this); // They use 1 ammo in thier weapon for this attack.
    comAdAtkNote(enemy->getActiveWeapon()->getItemName(), enemyDmg, player->getName(), enemy->getName(),combatSpeed);
    enemyDmg = -enemyDmg;
    player->healthChange(enemyDmg);
    comAdStateEntHp(player->getName(), player->getHealth(),combatSpeed);
}

void CombatAdmin::enemyAttkNonJamAmmo(Player *player,Enemy *enemy,int combatSpeed)
{
    double enemyDmg; // The damage this enemy does in this attack.

    enemy->enemyTaunt();
    enemyDmg = enemy->getActiveWeapon()->getDamage() - (enemy->getActiveWeapon()->getDamage() * player->getActiveArmour()->getDmgResist());
    enemy->getActiveWeapon()->ammoChange(-1.00,this);
    comAdAtkNote(enemy->getActiveWeapon()->getItemName(), enemyDmg, player->getName(), enemy->getName(),combatSpeed);
    enemyDmg = -enemyDmg;
    player->healthChange(enemyDmg);
    comAdStateEntHp(player->getName(), player->getHealth(),combatSpeed);
}

void CombatAdmin::enemyAttkNonJamNonAmmo(Player *player,Enemy *enemy,int combatSpeed)
{
    double enemyDmg;

    enemy->enemyTaunt();
    enemyDmg = enemy->getActiveWeapon()->getDamage() - (enemy->getActiveWeapon()->getDamage() * player->getActiveArmour()->getDmgResist());
    comAdAtkNote(enemy->getActiveWeapon()->getItemName(), enemyDmg, player->getName(), enemy->getName(),combatSpeed);
    enemyDmg = -enemyDmg; // Beacause to remove health the damage must be negative.
    player->healthChange(enemyDmg);
    comAdStateEntHp(player->getName(), player->getHealth(),combatSpeed);
}

void CombatAdmin::enemyAttkJamNonAmmo(Player *player,Enemy *enemy,int combatSpeed)
{
    double enemyDmg;

    enemy->enemyTaunt();
    enemyDmg = enemy->getActiveWeapon()->getDamage() - (enemy->getActiveWeapon()->getDamage() * player->getActiveArmour()->getDmgResist());
    comAdAtkNote(enemy->getActiveWeapon()->getItemName(), enemyDmg, player->getName(), enemy->getName(),combatSpeed);
    enemyDmg = -enemyDmg; // Beacause to remove health the damage must be negative.
    player->healthChange(enemyDmg);
    comAdStateEntHp(player->getName(), player->getHealth(),combatSpeed);
}

// It seems this function has to match being '1V1' too...
bool CombatAdmin::combatRound1V1(Weapon *unarmed,Player *player, Enemy *enemy,Narrator *narrator,Sound *sound, bool ran,int combatSpeed,bool playerHasScanner,bool playerHiredMercs,bool playerIsInWinter,bool playerHasSurgeNCo,bool playerHasEquipAbil,Weapon *machinePistol,Armour *lgBodyArm,Weapon *lightMG,Armour *hgBodyArm,Weapon *handGrenades,Armour *clunkyPowerSuit,Weapon *rocketLauncher,Weapon *battleRifle,Armour *lgPowArm,Armour *modArm) // For 1v1.
{
    // Currently, sound does not seem to be used... change that to be so...maybe...
    string temp = ""; // Temp String that stores what the player enters

    double enemySpeed; // The overall speed of the enemy.
    double playerSpeed; // The overall speed of the player.
    double runChance; // The chance of the player getting away from 0-100, if it is 50 or above the player gets away.

    // The enemy's health does not need to be checked as if they are defeated, the encounter code makes it so thier health is checked where it is 0 or not so it does not need to be checked in here.
    comAdSay("What will " + player->getName() + " do?",combatSpeed);
    comAdSay("1.Basic Attack.",combatSpeed);
    comAdSay("2.Run.",combatSpeed); // Make these options more dynamic, like reading them from an array that gains options as the player gets access to them.
    if (playerHasScanner == true) // Check whether the player has a scanner by this combat or not.
    {
        comAdSay("3.Scan",combatSpeed); // If they do, this is another command the player can do.
    }
    else if (playerHasScanner == false) // Otherwise...
    {
        // Do nothing.
    }

    // Need another check here to check if the player hired the mercs, another command the player can choose.

    // Tell the player how to enter commands.
    comAdSay("Just enter a single digit of what you want to do, e.g. 1,2 etc or one or two,or even oNe TwO etc will work too",combatSpeed);
    comAdSay("Also, you can enter what the command is called but must be done so in full, e.g. Run, Basic attack, etc, but once again this can be entered in any case structure as above.",combatSpeed);

    do
    {
        getline(cin, temp);
    } while(temp == "");

    player->playerSay(temp);

    temp = toUpperCase(temp);

    // Need to add more options for both the enemy and the player as to abilities they can do on thier turn.
    if (temp == "1" || temp == "ONE" || temp == "BASIC ATTACK" || temp == "BASICATTACK") // Remember to include the command name (also without spaces true condition) for if statements checking combat choices in the future.
    {
        playerSpeed = player->getActiveWeapon()->getSpeed() - (player->getActiveWeapon()->getSpeed() * player->getActiveArmour()->getAttSpdMod()); // Get the player's final speed.
        enemySpeed = enemy->getActiveWeapon()->getSpeed() - (enemy->getActiveWeapon()->getSpeed() * enemy->getActiveArmour()->getAttSpdMod()); // Get the enemy's final speed.

        if (playerSpeed >= enemySpeed) //Player goes first,as can be seen in the code, if the player's and the enemy's speed are equal the player still goes first.
        {
            playerBasicAttack(unarmed,player,enemy,NULL,combatSpeed,false); // The player makes a basic attack in this 1v1 hence false for the last param.

            if (enemy->getHealth() == 0) //Enemy does not fight back, as they are dead.
            {

            }
            else if(enemy->getHealth() > 0) // Enemy fights back, using the same checks as the player, see the players fighting code for information about the following code:
            {
                if (enemy->getHasSpecialAbilities() == false) // This enemy does not have special abilties.
                {

                }
                else if (enemy->getHasSpecialAbilities() == true) // This enemy has special abilties.
                {
                    handleEnemySpecialAbilties(unarmed,player,enemy,narrator,sound,combatSpeed); // Handle this enemy using special abilities. May need a bit more code as to determine these abilties per enemy.
                }

                enemyBasicAttack(unarmed,player,enemy,narrator,sound,combatSpeed); // Whether they do or do not do any special abilties, do a basic attack.
            }
        }
        else if (playerSpeed < enemySpeed) //Enemy goes first
        {
            if (enemy->getHasSpecialAbilities() == false) // This enemy does not have special abilties.
            {

            }
            else if (enemy->getHasSpecialAbilities() == true) // This enemy has special abilties.
            {
                handleEnemySpecialAbilties(unarmed,player,enemy,narrator,sound,combatSpeed); // Handle this enemy using special abilities. May need a bit more code as to determine these abilties per enemy.
            }

            enemyBasicAttack(unarmed,player,enemy,narrator,sound,combatSpeed); // Whether they do or do not do any special abilties, do a basic attack.

            if (player->getHealth() == 0) //Player does not fight back, as they are dead.
            {

            }
            else if (player->getHealth() > 0)// Player fights back
            {
                playerBasicAttack(unarmed,player,enemy,NULL,combatSpeed,false); // The player makes a basic attack in this 1v1 hence false for the last param.
            }
        }
    }
    else if(temp == "2" || temp == "TWO" || temp == "RUN") // Same as the first command above, make sure to include the needed conditions.
    {
        runChance = rand() % 100; // Determine the chance of the player getting away.
        if (runChance >= 50) // the player gets away
        {
            enemy->enemySay("Gah! they got away!"); // The enemy is not too happy about the player getting away.
            // Do not need to check if the enemy died here as that is done elsewhere...

            comAdSay(player->getName() + " succesfully escaped!",combatSpeed);
            sound->playSoundNonLooped("Fightend.wav");

            ran = true;
        }
        else if ((runChance < 50) && (runChance >= 25)) //The player gets away, but takes damage as they run
        {
            if (enemy->getHasSpecialAbilities() == false) // This enemy does not have special abilties.
            {

            }
            else if (enemy->getHasSpecialAbilities() == true) // This enemy has special abilties.
            {
                handleEnemySpecialAbilties(unarmed,player,enemy,narrator,sound,combatSpeed); // Handle this enemy using special abilities. May need a bit more code as to determine these abilties per enemy.
            }

            enemyBasicAttack(unarmed,player,enemy,narrator,sound,combatSpeed); // Whether they do or do not do any special abilties, do a basic attack.

            // If the player was defeated afer this attempt to get away, say so.

            if (player->getHealth() == 0) // Player was defeated.
            {
                enemy->enemyTaunt(); // As this was 1v1, this enemy is the only one that needs to taunt if the player was defeated now.

                entDefeated(player->getName(),combatSpeed);

                sound->playSoundNonLooped("Fightend.wav");

                narrator->narratorSay("Dead",3); // A loss state has been met.
            }

            // This code will not run if the player was defeated on their attempt to run.
            sound->playSoundNonLooped("Fightend.wav");

            enemy->enemySay("Gah! that person still got away!"); // The enemy is not too happy about the player getting away.
            // Checking whether the enemy is alive or not does not need to be checked in this 1v1 as that is done elsewhere...

            comAdSay(player->getName() + " succesfully escaped!",combatSpeed); // Reguardless of what random event occured, the player may still get away unless they don't but that is handled above
            ran = true;
        }
        else if (runChance < 25) // The player fails to get away
        {
            if (enemy->getHasSpecialAbilities() == false) // This enemy does not have special abilties.
            {

            }
            else if (enemy->getHasSpecialAbilities() == true) // This enemy has special abilties.
            {
                handleEnemySpecialAbilties(unarmed,player,enemy,narrator,sound,combatSpeed); // Handle this enemy using special abilities. May need a bit more code as to determine these abilties per enemy.
            }

            enemyBasicAttack(unarmed,player,enemy,narrator,sound,combatSpeed); // Whether they do or do not do any special abilties, do a basic attack.

            // No check here is needed to see if this enemy is dead or alive, as they should be one or the other without a need to check so...
            enemy->enemySay("Haha! you failed to escape from me!"); // The enemy is fairly happy that the player did not get away...

            comAdSay(player->getName() + " failed to escape!",combatSpeed); // If the player was deafeated afer this attempt to get away, then the encounter code deals with that.
            ran = false;
        }
    }
    else if (temp == "3" || temp == "THREE" || temp == "SCAN") // The player uses the scan command.
    {
        if (playerHasScanner == true) // If the player has a scanner...
        {
            playerScanEnemyOrEnemies(player,enemy,NULL,combatSpeed,1); // So scan the enemy (there is only one,hence 1 for the last param, for bigger combat it needs to scan multiple enemies and the enemy needs to still take thier turn).
        }
        else if (playerHasScanner == false) // If they do not...
        {
            comAdSay("ERROR:" + player->getName() + " does not yet have a scanner, choose something else to do.",combatSpeed); // The comAd says the player does not have a scanner.
        }

        if (enemy->getHasSpecialAbilities() == false) // This enemy does not have special abilties.
        {

        }
        else if (enemy->getHasSpecialAbilities() == true) // This enemy has special abilties.
        {
            handleEnemySpecialAbilties(unarmed,player,enemy,narrator,sound,combatSpeed); // Handle this enemy using special abilities. May need a bit more code as to determine these abilties per enemy.
        }

        enemyBasicAttack(unarmed,player,enemy,narrator,sound,combatSpeed); // Whether they do or do not do any special abilties, do a basic attack.
    }
    else // The player must have given an invalid command.
    {
        comAdSay("ERROR: not a valid command!",combatSpeed); // The player does nothing and the enemy still attacks.

        if (enemy->getHasSpecialAbilities() == false) // This enemy does not have special abilties.
        {
            // So do nothing.
        }
        else if (enemy->getHasSpecialAbilities() == true) // This enemy has special abilties.
        {
            handleEnemySpecialAbilties(unarmed,player,enemy,narrator,sound,combatSpeed); // Handle this enemy using special abilities. May need a bit more code as to determine these abilties per enemy.
        }

        enemyBasicAttack(unarmed,player,enemy,narrator,sound,combatSpeed); // Whether they do or do not do any special abilties, do a basic attack.
    }

    return ran; // Return whether the player ran away or not.
}

string CombatAdmin::toUpperCase(string str) // makes a string upper case
{
    char *upper = new char[str.length() + 1];
    strcpy(upper,str.c_str());
    strupr(upper);
    return string(upper);
}

void CombatAdmin::playerScanEnemyOrEnemies(Player *player,Enemy *enemy1,Enemy *enemy2,int combatSpeed,int numberOfEnemies) // Perform a scan manoeuvre.
{
    string specAbilStatement; // For describing whether the enemy has special abilties or not.
    string strMaxHealth;// The max and current health of this enemy.
    string strCurrHealth;
    string strWepDmg; // Some stats for the weapons and armour of the enemy.
    string strWepSpeed;
    string strArmDmgResist;
    string strArmSpdMod;

    if (numberOfEnemies == 1) // If there is only one enemy.
    {
        // State the stats for just enemy1, the one and only.
        // Do the conversions.
        strMaxHealth = doubleToString(enemy1->getMaxHealth());
        strCurrHealth = doubleToString(enemy1->getHealth());
        strWepDmg = doubleToString(enemy1->getActiveWeapon()->getDamage());
        strWepSpeed = doubleToString(enemy1->getActiveWeapon()->getSpeed());
        strArmDmgResist = doubleToString(enemy1->getActiveArmour()->getDmgResist() * 100); // Convert this value to a % and the one below.
        strArmSpdMod = doubleToString(enemy1->getActiveArmour()->getAttSpdMod() * 100);

        // State the results for enemy1 as that should always be the enemy used is there is only one.
        comAdSay("Name:" + enemy1->getName() + ".",combatSpeed); // State the basic stats.
        comAdSay("Current Health:" + strCurrHealth + "." ,combatSpeed);
        comAdSay("Maximum Health:" + strMaxHealth + ".",combatSpeed);
        comAdSay("Weapon:" + enemy1->getActiveWeapon()->getItemName() + ".",combatSpeed); // Show the enemy's equipment and some stats.
        comAdSay("This weapon does: " + strWepDmg + " points of damage per basic attack(base dmg. Before armour is factored in)." ,combatSpeed);
        comAdSay("This weapon has a speed value of " + strWepSpeed + ".",combatSpeed);
        comAdSay("Armour:" + enemy1->getActiveArmour()->getItemName() + ".",combatSpeed);
        comAdSay("This armour has a damage resist of " + strArmDmgResist + "%.",combatSpeed);
        comAdSay("This armour has a speed modifier of " + strArmSpdMod + "%.",combatSpeed);

        if (enemy1->getHasSpecialAbilities() == true) // Check whether this enemy has spec abils or not, same below too.
        {
            specAbilStatement = "this enemy does";

        }
        else if (enemy1->getHasSpecialAbilities() == false)
        {
            specAbilStatement = "this enemy does not";
        }
        comAdSay("Whether the enemy has special abilties or not:" + specAbilStatement + ".",combatSpeed); // Stat whether they have spec abilities or not.
    }
    else if (numberOfEnemies == 2)
    {
        // State the stats for just enemy1.
        // Do the conversions.
        strMaxHealth = doubleToString(enemy1->getMaxHealth());
        strCurrHealth = doubleToString(enemy1->getHealth());
        strWepDmg = doubleToString(enemy1->getActiveWeapon()->getDamage());
        strWepSpeed = doubleToString(enemy1->getActiveWeapon()->getSpeed());
        strArmDmgResist = doubleToString(enemy1->getActiveArmour()->getDmgResist() * 100); // Convert this value to a % and the one below.
        strArmSpdMod = doubleToString(enemy1->getActiveArmour()->getAttSpdMod() * 100);

        comAdSay("Name:" + enemy1->getName() + ".",combatSpeed); // State the basic stats.
        comAdSay("Current Health:" + strCurrHealth + "." ,combatSpeed);
        comAdSay("Maximum Health:" + strMaxHealth + ".",combatSpeed);
        comAdSay("Weapon:" + enemy1->getActiveWeapon()->getItemName() + ".",combatSpeed); // Show the enemy's equipment and some stats.
        comAdSay("This weapon does: " + strWepDmg + " points of damage per basic attack(base dmg. Before armour is factored in)." ,combatSpeed);
        comAdSay("This weapon has a speed value of " + strWepSpeed + ".",combatSpeed);
        comAdSay("Armour:" + enemy1->getActiveArmour()->getItemName() + ".",combatSpeed);
        comAdSay("This armour has a damage resist of " + strArmDmgResist + "%.",combatSpeed);
        comAdSay("This armour has a speed modifier of " + strArmSpdMod + "%.",combatSpeed);

        if (enemy1->getHasSpecialAbilities() == true)
        {
            specAbilStatement = "this enemy does";

        }
        else if (enemy1->getHasSpecialAbilities() == false)
        {
            specAbilStatement = "this enemy does not";
        }
        comAdSay("Whether the enemy has special abilties or not:" + specAbilStatement + ".",combatSpeed); // Stat whether they have spec abilities or not.

        // State the stats for just enemy2.
        // Do the conversions.
        strMaxHealth = doubleToString(enemy2->getMaxHealth());
        strCurrHealth = doubleToString(enemy2->getHealth());
        strWepDmg = doubleToString(enemy2->getActiveWeapon()->getDamage());
        strWepSpeed = doubleToString(enemy2->getActiveWeapon()->getSpeed());
        strArmDmgResist = doubleToString(enemy2->getActiveArmour()->getDmgResist() * 100); // Convert this value to a % and the one below.
        strArmSpdMod = doubleToString(enemy2->getActiveArmour()->getAttSpdMod() * 100);

        comAdSay("Name:" + enemy2->getName() + ".",combatSpeed); // State the basic stats.
        comAdSay("Current Health:" + strCurrHealth + "." ,combatSpeed);
        comAdSay("Maximum Health:" + strMaxHealth + ".",combatSpeed);
        comAdSay("Weapon:" + enemy2->getActiveWeapon()->getItemName() + ".",combatSpeed); // Show the enemy's equipment and some stats.
        comAdSay("This weapon does: " + strWepDmg + " points of damage per basic attack(base dmg. Before armour is factored in)." ,combatSpeed);
        comAdSay("This weapon has a speed value of " + strWepSpeed + ".",combatSpeed);
        comAdSay("Armour:" + enemy2->getActiveArmour()->getItemName() + ".",combatSpeed);
        comAdSay("This armour has a damage resist of " + strArmDmgResist + "%.",combatSpeed);
        comAdSay("This armour has a speed modifier of " + strArmSpdMod + "%.",combatSpeed);

        if (enemy2->getHasSpecialAbilities() == true) // Determine whether this enemy has spec abil's or not, all will do apart from the tut enemy.
        {
            specAbilStatement = "this enemy does";

        }
        else if (enemy2->getHasSpecialAbilities() == false)
        {
            specAbilStatement = "this enemy does not";
        }
        comAdSay("Whether the enemy has special abilties or not:" + specAbilStatement + ".",combatSpeed); // Stat whether they have spec abilities or not.
    }
}

void CombatAdmin::playerFindsItem(Player *player, Item *item,int messageType) // messageType is the type of message that should be said.
{
    string strIWeight = doubleToString(item->getItemWeight()); // Convert the weight of the item and the player from a double to a string.
    string strPWeight = doubleToString(player->getPlayerWeight()); // Left to a precision of 2 decimal places as the player could find items that weigh 0.5kgs for example.
    // The players weight is not tested here, tested in the places where they can find an item as weight is changed before this function is called.

    if (messageType == 1) // Generally, in the weightChange function of the player the comAd states the player's weight, so it usally does not need to be stated here...
    {
        comAdSay(player->getName() + " is now carrying " + strPWeight + "kgs worth of items.",1);
    }
    else // The player's new weight has already been mentioned, the why is the comAdSay line just below.
    {
        // Do nothing.
    }
    comAdSay(player->getName() + " has found: " + item->getItemName() + ". It weighs: " + strIWeight + "kgs.",1); // State what the player has found.

}

void CombatAdmin::playerFindsGold(string playerName,double coinCnt,double playerCoinCnt) // The player finds some coins (Or gets some from selling), the systems knows how many the player has and how many they have found. (Per var)
{
    int rndCoinCnt = (int) coinCnt; // Round the coin count from the player and the item.
    int rndPlayerCoinCnt = (int) playerCoinCnt; // They have been rounded as you cannot have half a coin etc.

    string strCoinCnt = intToString(rndCoinCnt); // Convert that value to a string.
    string strPCoinCnt = intToString(rndPlayerCoinCnt);

    comAdSay(playerName + " has gained " + strCoinCnt + " gold coins!",1); // state the coin count as it is now.
    comAdSay(playerName + " now has " + strPCoinCnt + " gold coins.",1);
}

void CombatAdmin::playerLosesGold(string playerName,double coinCnt,double playerCoinCnt) // The player loses  some coins, the systems knows how many the player has and how many they have lost. (Per var)
{
    int rndCoinCnt = (int) coinCnt; // Round the coin count from the player and the item.
    int rndPlayerCoinCnt = (int) playerCoinCnt; // They can be rounded as you cannot have half a coin etc.

    string strCoinCnt = intToString(rndCoinCnt); // Convert that value to a string.
    string strPCoinCnt = intToString(rndPlayerCoinCnt);

    comAdSay(playerName + " has lost " + strCoinCnt + " gold coins!",1); // state the coin count as it is now.
    comAdSay(playerName + " now has " + strPCoinCnt + " gold coins.",1);
}
