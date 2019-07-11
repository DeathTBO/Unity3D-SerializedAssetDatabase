using System.IO;
using System.Collections.Generic;

namespace SerializedAssets.Storage
{
    public class SerializedAssetDatabaseBlacklist
    {
        public List<string> blacklistedFiles;

        public List<string> blacklistedDirectories;

        public static SerializedAssetDatabaseBlacklist Default
        {
            get
            {
                SerializedAssetDatabaseBlacklist blacklist = new SerializedAssetDatabaseBlacklist();

                blacklist.blacklistedFiles.Add(".cs");

                blacklist.blacklistedDirectories.Add("Assets/SerializedAssetDatabase");
                blacklist.blacklistedDirectories.Add($"Editor");
                
                return blacklist;
            }
        }

        public SerializedAssetDatabaseBlacklist()
        {
            blacklistedFiles = new List<string>();

            blacklistedDirectories = new List<string>();
        }
        
        public bool CheckFile(string file)
        {
            for (int i = 0; i < blacklistedFiles.Count; i++)
            {
                if (file.EndsWith(blacklistedFiles[i]))
                    return true;
            }

            return false;
        }

        public bool CheckDirectory(string directory)
        {
            for (int i = 0; i < blacklistedDirectories.Count; i++)
            {
                if (directory.EndsWith(blacklistedDirectories[i]))
                    return true;
            }

            return false;
        }
    }
}