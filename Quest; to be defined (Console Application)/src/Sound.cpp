#include <Sound.h>
#include <stdio.h> // Needed for a similar kind of output as iostream for various functions error msgs.
#include <windows.h>
#include <time.h>

// Remember that you may need the file type (.wav etc) in the parameter for both of the playerSound functions.

Sound::Sound() // Using the sound constructor.
{
	//Seeding the rand function.
    srand (time(NULL));

    rand(); // Make a call to rand to make sure it is properly seeded and does not put big numbers first or small numbers.
}

void Sound::stopAllSound()
{
    PlaySound(NULL,0,0); // To stop asyncrounus sound from playing, put this func where it is needed(any time sound is changed for example).
}

void Sound::playSoundLooped(const CHAR *loopedSound) // Needs to be a const char to work, play a sound on a loop.
{
    PlaySound(loopedSound,NULL,SND_ASYNC|SND_LOOP|SND_FILENAME); // You need to use | to have multiple flags, this is the last param of the play sound function.
    // Keep an eye on playSound though...maybe implement directsound...
}

void Sound::playSoundNonLooped(const CHAR *nonLoopedSound) // Do not play a sound on a loop.
{
    PlaySound(nonLoopedSound,NULL,SND_ASYNC|SND_FILENAME); // Remember that you need the second param to be NULL for this func to work.
}

// Remember that you need the SND_FILENAME flag as one of the flags in the last param of the PlaySound func to let it know that the char put into it is the filename, just to make sure.
