using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomListSimulator
{
    public class RandomManager
    {
        private Random randomGenerator;

        public RandomManager()
        {
            randomGenerator = new Random();
        }

        public int GetRandomIndex(string[] data, string[] filterData)
        {
            var returnIndex = 0;

            do
            {
                returnIndex = randomGenerator.Next(data.Length);
            } while (filterData.Contains(data[returnIndex]));

            return returnIndex;
        }

        public string[] RandomOrder(string[] data)
        {
            var returnList = new List<string>();
            //Match the old list but with empty strings
            foreach(var name in data)
            {
                returnList.Add("");
            }

            var randomNewIndex = 0;

            //Re-sort the name list with the new random order
            foreach(var name in data)
            {
                do
                {
                    randomNewIndex = randomGenerator.Next(data.Length);
                } while (returnList[randomNewIndex] != "");

                returnList[randomNewIndex] = name;
            }

            return returnList.ToArray();
        }
    }
}
