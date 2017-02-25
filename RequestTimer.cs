using System;

namespace TerrionicsMod
{
    class RequestTimer
    {
        private static ulong counter;

        public static void update()
        {
            counter++;
        }

        public static ulong getCount()
        {
            return counter;
        }
    }
}
