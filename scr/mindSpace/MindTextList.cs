using System;
using System.Collections.Generic;
using System.IO;

namespace Game.MindSpace
{
    public class MindTextList
    {
        private List<string> voidText = new List<string>();

        public MindTextList()
        {
            using (StreamReader reader = new StreamReader("textassets/regularMindText.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    voidText.Add(line);
                }
            }
        }

        public String GetRandomMindText()
        {
            Random rr = new Random();
            int randomint = (int)Math.Round(rr.NextDouble() * (voidText.Count - 1.001f));
            return voidText[randomint];
        }
    }
}