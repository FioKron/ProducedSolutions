#ifndef SOUND_H
#define SOUND_H
#include <string>
#include <windows.h> // for PlaySound() and other functions like sleep.

using namespace std;

class Sound
{
    public:
        Sound(); // Default constructor.
        void playSoundLooped(const CHAR* loopedSound); // Play sound on a loop or not.
        void playSoundNonLooped(const CHAR* unLoopedSound);
        void stopAllSound(); // Stop all the sound, just making sure...
};

#endif // SOUND_H
