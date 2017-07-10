#ifndef COMBATADMIN_H
#define COMBATADMIN_H

#include <string> // Need this line or it complains
#include <Player.h>
#include <Sound.h>
#include <Enemy.h>
#include <Narrator.h>
#include <vector>

using namespace std;

class Enemy;
class Player; // As the combat admin needs enemy and player instances.

class CombatAdmin // Code yet to be commented here, will come soon.
{
    public:
        CombatAdmin();
        void healthSet(double newHealth, string playerName);
        void comAdSay(string sayWhat, int messageType);
        void playerFindsChest(Player *player,Weapon *weapon,Armour *armour);
        void comAdWarning(string enemyName1,string enemyName2,int enemyNum,int combatSpeed);
        void comAdAtkNote(string attack, double damage,string target,string aggresor,int combatSpeed);
        void entDefeated(string entName,int combatSpeed);
        void comAdStateEntHp(string ent, double hp,int combatSpeed);
        void comAdStateScanResults(string enemyName, double enemyHealth,int combatSpeed);
        string doubleToString(double number); // for converting ints and doubles to strings.
        string intToString(int number);
        bool isRandEncounter();
        void randomEncounter1V1(Weapon *unarmed,Player *player,Enemy *enemy,Sound *sound,Narrator *narrator,int combatSpeed,bool playerHasScanner,bool playerHiredMercs,bool playerIsInWinter,bool playerHasSurgeNCo,bool playerHasEquipAbil,Weapon *machinePistol,Armour *lgBodyArm,Weapon *lightMG,Armour *hgBodyArm,Weapon *handGrenades,Armour *clunkyPowerSuit,Weapon *rocketLauncher,Weapon *battleRifle,Armour *lgPowArm,Armour *modArm); // Combat speed in both of these functions is the speed combat should be run at.
        bool combatRound1V1(Weapon *unarmed,Player *player, Enemy *enemy,Narrator *narrator,Sound *sound, bool ran,int combatSpeed,bool playerHasScanner,bool playerHiredMercs,bool playerIsInWinter,bool playerHasSurgeNCo,bool playerHasEquipAbil,Weapon *machinePistol,Armour *lgBodyArm,Weapon *lightMG,Armour *hgBodyArm,Weapon *handGrenades,Armour *clunkyPowerSuit,Weapon *rocketLauncher,Weapon *battleRifle,Armour *lgPowArm,Armour *modArm);
        void playerAoeAtkk(Player *player,Enemy *enemy1,Enemy *enemy2,int combatSpeed); // The player does an AOE attack.
        void playerScanEnemyOrEnemies(Player *player,Enemy *enemy1,Enemy *enemy2,int combatSpeed,int numberOfEnemies); // Scan the enemy if the player has the scanner, currently coded for two enemies.
        void playerFindsItem(Player *player, Item *item,int messageType);
        void playerFindsGold(string playerName,double coinCnt,double playerCoinCnt); // For the player gaining and losing gold.
        void playerLosesGold(string playerName,double coinCnt,double playerCoinCnt);
        string toUpperCase(string str);
        void playerAttkJamAmmo(Player *player, Enemy *enemy,int combatSpeed,bool isAoeAttk); // For attacks the player makes that jam and use ammo.
        void playerAttkNonJamAmmo(Player *player, Enemy *enemy,int combatSpeed,bool isAoeAttk); // For attacks the player makes that use ammo but do not jam.
        void playerAttkNonJamNonAmmo(Player *player, Enemy *enemy,int combatSpeed,bool isAoeAttk); // For attacks the player makes that do not jam nor use ammo.
        void playerAttkJamNonAmmo(Player *player, Enemy *enemy,int combatSpeed,bool isAoeAttk); // For attacks the player makes that can jam but do not use ammo.
        void enemyAttkJamAmmo(Player *player, Enemy *enemy,int combatSpeed); // For attacks the enemy makes that jam and use ammo.
        void enemyAttkNonJamAmmo(Player *player, Enemy *enemy,int combatSpeed); // For attacks the enemy makes that use ammo but do not jam.
        void enemyAttkNonJamNonAmmo(Player *player, Enemy *enemy,int combatSpeed); // For attacks the enemy makes that do not jam nor use ammo.
        void enemyAttkJamNonAmmo(Player *player, Enemy *enemy,int combatSpeed); // For attacks the enemy makes that can jam but do not use ammo.
        void stateCurrentEquipmentStats(vector<Item *> items); // State the equipment the player has.
        void handleEnemySpecialAbilties(Weapon *unarmed,Player *player,Enemy *enemy,Narrator *narrator,Sound *sound,int combatSpeed); // Handle this enemy's special abilties as this is part of combat, so it is the combat admin's duty.
        void playerBasicAttack(Weapon *unarmed,Player *player,Enemy *enemy1,Enemy *enemy2,int combatSpeed,bool isMultiCombat); // The player attacking an enemy of enemies with a basic attack, isMulCom var is for whether there are more then one enemies in this combat.
        void enemyBasicAttack(Weapon *unarmed,Player *player,Enemy *enemy,Narrator *narrator,Sound *sound,int combatSpeed); // The enemy version of this.
        void entsHaveJoinedYourParty(Enemy *firstEnt,Enemy *secondEnt); // May need more params for this func in the future, but for now just the two enemies.
    protected:
        bool enemyHasUsedTargetLockSuccess; // For whether or not the enemy has locked on to the player in the "You stole our stuff!" encounter.
        int timesToAttack; // The number of times that merc smith can attack per spec abil.
};

#endif // COMBATADMIN_H

