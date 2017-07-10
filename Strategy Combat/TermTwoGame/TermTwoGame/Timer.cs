using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TermTwoGame
{
    class Timer
    {
        private int time; // The current time in ms for this timer class.

        public void tick()
        {
            time += 1;
        }

        public void reset()
        {
            time = 0;
        }

        public int getCurrentTime()
        {
            return time;
        }

    }

}
