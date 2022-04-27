using System.IO;
using UnityEngine;

namespace UVL.Demos
{
    public static class DemoData
    {
        public static string DIRECTORY => FindDirectory("Data", TEST_DIRECTORY);

        private static string TEST_DIRECTORY => FindDirectory("Tests", UVL_DIRECTORY);
        private static string UVL_DIRECTORY => FindDirectory("UVL", Application.dataPath);

        private static string FindDirectory(string target, string currentDirectory)
        {
            string testDirectory = null;

            if (Path.GetFileName(currentDirectory) == target)
            {
                testDirectory = currentDirectory;
            }
            else
            {
                foreach (var child in Directory.GetDirectories(currentDirectory))
                {
                    string childDirectory = FindDirectory(target, child);
                    if (childDirectory != null)
                    {
                        testDirectory = childDirectory;
                        break;
                    }
                }
            }

            return testDirectory;
        }
    }
}