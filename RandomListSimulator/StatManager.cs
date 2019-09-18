using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RandomListSimulator
{
    public class StatManager
    {
        private List<TurnObject> turnsData;

        private RandomManager rndManager;

        private string simFolderPath = ".\\Sims\\";

        public StatManager(RandomManager manager)
        {
            rndManager = manager;
            turnsData = new List<TurnObject>();
        }

        public void ResetData()
        {
            turnsData = new List<TurnObject>();
        }

        public void ProcessTurn(string[] data)
        {
            var newOrder = rndManager.RandomOrder(data);
            var newTurnData = new TurnObject();

            for(var i = 0; i < newOrder.Length; i++)
            {
                var newNameObject = new NameObject();
                newNameObject.Name = newOrder[i];
                newNameObject.OrderIndex = i;

                newTurnData.SortedTurnData.Add(newNameObject);
            }

            turnsData.Add(newTurnData);
        }

        public void SaveTurnStats()
        {
            var finalFileValue = "";
            var topLine = "Total Simulations: " + turnsData.Count() + Environment.NewLine;

            var statsData = CalculateStatsForPlayers();

            var simId = Guid.NewGuid();

            finalFileValue += topLine;
            finalFileValue += Environment.NewLine;
            finalFileValue += Environment.NewLine;

            foreach(var player in statsData)
            {
                player.GenerateRuns(turnsData);
                var playerString = "";
                playerString += "Name: " + player.PlayerName + Environment.NewLine;
                playerString += Environment.NewLine;
                playerString += "Top 3rd Total: " + player.TopThirdCount + Environment.NewLine;
                playerString += "Middle 3rd Total: " + player.MiddleThirdCount + Environment.NewLine;
                playerString += "Bottom 3rd Total: " + player.BottomThirdCount + Environment.NewLine;

                playerString += Environment.NewLine;
                playerString += "Longest Top Run: " + player.LongestTopRun + Environment.NewLine;
                playerString += "Longest Mid Run: " + player.LongestMidRun + Environment.NewLine;
                playerString += "Longest Bottom Run: " + player.LongestBottomRun + Environment.NewLine;

                playerString += Environment.NewLine;
                playerString += "Position : Count" + Environment.NewLine;
                foreach(var turnOrder in player.Stats)
                {
                    playerString += (turnOrder.Position + 1) + " : " + turnOrder.Count + Environment.NewLine;
                }

                playerString += Environment.NewLine;
                playerString += Environment.NewLine;

                finalFileValue += playerString;
            }

            //Create the new file and save it
            var fileCheck = new FileInfo(simFolderPath);
            fileCheck.Directory.Create();

            File.WriteAllText(simFolderPath + simId + ".txt", finalFileValue);   
        }

        private List<SaveDataForPlayer> CalculateStatsForPlayers()
        {
            var returnList = new List<SaveDataForPlayer>();
            var playerCount = turnsData[0].SortedTurnData.Count();

            foreach(var turn in turnsData)
            {
                foreach(var playerObject in turn.SortedTurnData)
                {
                    var playerName = playerObject.Name;

                    //If the data doesn't exist, create it
                    if(returnList.FirstOrDefault(x => x.PlayerName == playerName) == null)
                    {
                        returnList.Add(new SaveDataForPlayer(playerCount, playerName));
                    }

                    returnList.First(x => x.PlayerName == playerName).Stats.First(x => x.Position == playerObject.OrderIndex).Count += 1;
                }
            }


            return returnList;
        }
    }

    public struct NameObject
    {
        public string Name;
        public int OrderIndex;
    }

    public class TurnObject
    {
        public List<NameObject> SortedTurnData;

        public TurnObject()
        {
            SortedTurnData = new List<NameObject>();
        }
    }
}
