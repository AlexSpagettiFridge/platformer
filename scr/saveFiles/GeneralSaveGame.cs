using System;
using System.Collections.Generic;
using System.IO;

namespace Game
{
    public class GeneralSaveGame
    {
        public bool newGame = true;
        public int slotId, totalBerries = 0, totalScrolls = 0;
        public String currentMap = "tutorial";
        public bool isAtMarmalade = false;
        public int saveSpotId = -1;
        public List<String> activatedEvents = new List<string>();
        private int berryCounter = 0, scrollCounter;

        public GeneralSaveGame(int slotId)
        {
            this.slotId = slotId;
            if (!Directory.Exists("savegames"))
            {
                Directory.CreateDirectory("savegames");
            }
            if (!Directory.Exists("savegames/slot" + slotId))
            {
                Directory.CreateDirectory("savegames/slot" + slotId);
            }
        }

        public static GeneralSaveGame Load(int slotId)
        {
            GeneralSaveGame saveGame = new GeneralSaveGame(slotId);
            if (File.Exists(saveGame.GetPath()))
            {
                using (StreamReader file = new StreamReader(saveGame.GetPath()))
                {
                    saveGame.newGame = false;
                    saveGame.currentMap = file.ReadLine();
                    saveGame.isAtMarmalade = Convert.ToBoolean(file.ReadLine());
                    saveGame.saveSpotId = Convert.ToInt32(file.ReadLine());
                    saveGame.totalBerries = Convert.ToInt32(file.ReadLine());
                    saveGame.totalScrolls = Convert.ToInt32(file.ReadLine());
                    String eventName;
                    while ((eventName = file.ReadLine()) != ">")
                    {
                        saveGame.activatedEvents.Add(eventName);
                    }
                    return saveGame;
                }
            }
            else
            {
                return saveGame;
            }
        }

        public void Save()
        {
            totalBerries += berryCounter;
            berryCounter = 0;
            using (StreamWriter file = new StreamWriter(GetPath()))
            {
                file.WriteLine(currentMap);
                file.WriteLine(isAtMarmalade);
                file.WriteLine(saveSpotId);
                file.WriteLine(totalBerries);
                file.WriteLine(totalScrolls);
                foreach (String eventName in activatedEvents)
                {
                    file.WriteLine(eventName);
                }
                file.WriteLine(">");
            }
        }

        public LocalSaveGame GetCurrentMapLocalSaveGame()
        {
            return LocalSaveGame.Load(currentMap, slotId);
        }

        public String GetPath()
        {
            return "savegames/slot" + slotId + "/general.sav";
        }

        public static int LastLoadedSave
        {
            get
            {
                if (!Directory.Exists("savegames"))
                {
                    Directory.CreateDirectory("savegames");
                }
                if (!File.Exists("savegames/lastsave.txt"))
                {
                    LastLoadedSave = 1;
                    return 1;
                }
                using (StreamReader file = new StreamReader("savegames/lastsave.txt"))
                {
                    return (Convert.ToInt32(file.ReadLine()));
                }
            }
            set
            {
                using (StreamWriter file = new StreamWriter("savegames/lastsave.txt"))
                {
                    file.WriteLine(value);
                }
            }
        }

        private List<LocalSaveGame> GetAllLocalSaves()
        {
            List<LocalSaveGame> localSaves = new List<LocalSaveGame>();
            String[] filePaths = Directory.GetFiles("savegames/slot" + slotId);
            foreach (String path in filePaths)
            {
                if (path.EndsWith("general.sav"))
                {
                    continue;
                }
                localSaves.Add(LocalSaveGame.LoadByPath(path));
            }
            return localSaves;
        }

        public void Delete()
        {
            foreach (LocalSaveGame localSaveGame in GetAllLocalSaves())
            {
                localSaveGame.Delete();
            }
            File.Delete(GetPath());
        }

        public int BerryCounter
        {
            get => berryCounter;
            set => berryCounter = value;
        }

        public int ScrollCounter
        {
            get => berryCounter;
            set => scrollCounter = value;
        }
    }
}