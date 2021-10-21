using System;
using System.IO;
using System.Collections.Generic;

namespace Game
{
    public class LocalSaveGame
    {
        public string mapName;
        public int slotId;
        public List<int> berriesCollected = new List<int>();
        public List<int> scrollsCollected = new List<int>();

        public LocalSaveGame(string mapName, int slotId)
        {
            this.mapName = mapName;
            this.slotId = slotId;
        }

        public static LocalSaveGame LoadByPath(string path)
        {
            int trimIndex = path.LastIndexOf("savegames/slot") + 14;
            path = path.Substring(trimIndex);

            int slashIndex = path.IndexOf('\\');
            int slotId = Convert.ToInt32(path.Substring(0, slashIndex));
            path = path.Substring(slashIndex + 1);

            return Load(path, slotId);
        }

        public static LocalSaveGame Load(string mapName, int slotId)
        {
            LocalSaveGame saveGame = new LocalSaveGame(mapName, slotId);
            if (File.Exists(saveGame.getPath()))
            {
                using (StreamReader file = new StreamReader(saveGame.getPath()))
                {
                    while (file.Peek() >= 0 && file.Peek() != '\r')
                    {
                        char[] readChars = new char[3];
                        file.Read(readChars, 0, 3);
                        saveGame.berriesCollected.Add(Convert.ToInt32(new string(readChars)));
                    }

                    file.ReadLine();

                    while (file.Peek() >= 0 && file.Peek() != '\r')
                    {
                        char[] readChars = new char[2];
                        file.Read(readChars, 0, 2);
                        saveGame.scrollsCollected.Add(Convert.ToInt32(new string(readChars)));
                    }
                }
            }
            return saveGame;
        }

        public void Save()
        {
            using (StreamWriter file = new StreamWriter(getPath()))
            {
                foreach (int berryId in berriesCollected)
                {
                    if (berryId < 100) { file.Write("0"); }
                    if (berryId < 10) { file.Write("0"); }
                    file.Write(berryId);
                }
                file.WriteLine("");
                foreach (int scrollId in scrollsCollected)
                {
                    if (scrollId < 10) { file.Write("0"); }
                    file.Write(scrollId);
                }
            }
        }

        public string getPath()
        {
            return "savegames/slot" + slotId + "/" + mapName + ".sav";
        }

        public void Delete(){
            File.Delete(getPath());
        }
    }
}