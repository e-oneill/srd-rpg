using UnityEngine;
using System;
using System.Security.Cryptography;

namespace RPG.Core
{
    public class randomnumbergenerator : MonoBehaviour {
        public static System.Random rand;
        public static RNGCryptoServiceProvider rg = new RNGCryptoServiceProvider();


        public static int DiceRoll(byte numSides)
        {
            if (numSides <= 0)
            {
                return 1;
            }
            byte[] randomnumber = new byte[1];
            do
            {
                rg.GetBytes(randomnumber);
            }
            while (!IsFairRoll(randomnumber[0], numSides));
            byte byteResult = (byte)((randomnumber[0] % numSides) +1);
            return Convert.ToInt32(byteResult);
        }

        private static bool IsFairRoll(byte roll, byte numSides)
        {
            int fullSetsofValues = Byte.MaxValue / numSides;

            return roll < numSides * fullSetsofValues;
        }
    }
}