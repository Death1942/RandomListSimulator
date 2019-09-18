using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomListSimulator
{
    public class SaveDataForPlayer
    {
        public int NumberOfPlayers;
        public string PlayerName;
        public int TopThirdCount
        {
            get
            {
                var topData = Stats.Where(x => IsPositionInTop(x.Position));
                return topData.Sum(x => x.Count);
            }
        }
        public int MiddleThirdCount
        {
            get
            {
                var middleData = Stats.Where(x => IsPositionInMid(x.Position));
                return middleData.Sum(x => x.Count);
            }
        }

        public int BottomThirdCount
        {
            get
            {
                var bottomData = Stats.Where(x => IsPositionInBottom(x.Position));
                return bottomData.Sum(x => x.Count);
            }
        }

        private int Top3rdLimit = 0;
        private int Middle3rdLimit = 0;
        private int Bottom3rdLimit = 0;

        public int LongestTopRun = 0;
        public int LongestMidRun = 0;
        public int LongestBottomRun = 0;

        public List<TurnDataTracked> Stats;

        public SaveDataForPlayer(int numberOfPlayers, string playerName)
        {
            NumberOfPlayers = numberOfPlayers;
            PlayerName = playerName;

            Top3rdLimit = numberOfPlayers / 3;
            Middle3rdLimit = (numberOfPlayers / 3 * 2);
            Bottom3rdLimit = numberOfPlayers;

            Stats = new List<TurnDataTracked>();

            for(var i = 0; i < numberOfPlayers; i++)
            {
                Stats.Add(new TurnDataTracked { Position = i });
            }
        }

        public void GenerateRuns(List<TurnObject> turnsData)
        {
            var run = 0;
            var positionFlag = 0;
            foreach(var turnData in turnsData)
            {
                var position = turnData.SortedTurnData.First(x => x.Name == PlayerName).OrderIndex;
                if(IsPositionInTop(position))
                {
                    var newPositionFlag = 1;
                    if(positionFlag == 0)
                    {
                        positionFlag = 1;
                    }

                    if(newPositionFlag == positionFlag)
                    {
                        run++;
                    }
                    else
                    {
                        SavePositionData(positionFlag, run);
                        positionFlag = newPositionFlag;
                        run = 0;
                    }
                }
                else if (IsPositionInMid(position))
                {
                    var newPositionFlag = 2;
                    if (positionFlag == 0)
                    {
                        positionFlag = 2;
                    }

                    if (newPositionFlag == positionFlag)
                    {
                        run++;
                    }
                    else
                    {
                        SavePositionData(positionFlag, run);
                        positionFlag = newPositionFlag;
                        run = 0;
                    }
                }
                else if (IsPositionInBottom(position))
                {
                    var newPositionFlag = 3;
                    if (positionFlag == 0)
                    {
                        positionFlag = 3;
                    }

                    if (newPositionFlag == positionFlag)
                    {
                        run++;
                    }
                    else
                    {
                        SavePositionData(positionFlag, run);
                        positionFlag = newPositionFlag;
                        run = 0;
                    }
                }
            }
            SavePositionData(positionFlag, run);
        }

        private void SavePositionData(int positionFlag, int run)
        {
            switch (positionFlag)
            {
                case 1:
                    {
                        //Top
                        LongestTopRun = run > LongestTopRun ? run : LongestTopRun;
                        break;
                    }
                case 2:
                    {
                        //Mid
                        LongestMidRun = run > LongestMidRun ? run : LongestMidRun;
                        break;
                    }
                case 3:
                    {
                        //Bottom
                        LongestBottomRun = run > LongestBottomRun ? run : LongestBottomRun;
                        break;
                    }
            }
        }

        private bool IsPositionInTop(int position)
        {
            return position <= Top3rdLimit;
        }

        private bool IsPositionInMid(int position)
        {
            return position <= Middle3rdLimit && position > Top3rdLimit;
        }

        private bool IsPositionInBottom(int position)
        {
            return position <= Bottom3rdLimit && position > Middle3rdLimit;
        }
    }

    public class TurnDataTracked
    {
        public int Position;
        public int Count;

        public TurnDataTracked()
        {

        }
    }
}
