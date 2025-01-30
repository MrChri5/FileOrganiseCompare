using System.Configuration;

namespace FileOrganiseCompare
{
    class Program
    {
        static void Main()
        {
            //Initialize file counts.
            int totalFilesRead = 0;
            int totalFilesMatch = 0;

            //Get Read and Compare directories. Give error and exit if either does not exist.
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["settingsReadDirectory"])
                || string.IsNullOrEmpty(ConfigurationManager.AppSettings["settingsCompareDirectory"]))
            {
                Console.WriteLine("Read directory or Compare directory missing from App.config.");
                Console.Read();
                return;
            }
            DirectoryInfo ReadDir = new DirectoryInfo(ConfigurationManager.AppSettings["settingsReadDirectory"]);
            DirectoryInfo CompareDir = new DirectoryInfo(ConfigurationManager.AppSettings["settingsCompareDirectory"]);
            if (!ReadDir.Exists)
            {
                Console.WriteLine($"settingsReadDirectory {ConfigurationManager.AppSettings["settingsReadDirectory"]} does not exist.");
                Console.Read();
                return;
            }
            if (!CompareDir.Exists)
            {
                Console.WriteLine($"settingsCompareDirectory {ConfigurationManager.AppSettings["settingsCompareDirectory"]} does not exist.");
                Console.Read();
                return;
            }
            //Output Read and Compare file paths
            Console.WriteLine($"Reading files from: {ReadDir.FullName}");
            Console.WriteLine($"Comparing against files from: {CompareDir.FullName}");

            //Set File Extension to search for in the Compare directory. Set to wildcard (*) if none specified.
            string searchExtension = "*";
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["settingsFileExtension"]))
            {
                searchExtension = ConfigurationManager.AppSettings["settingsFileExtension"];
                searchExtension = searchExtension.TrimStart('.');
                Console.WriteLine($"\tthat have file extension: .{searchExtension}");
            }

            //Get directories to determine where/if files are moved when the do or don't have a match.
            bool moveMatched = false;
            bool moveNoMatched = false;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["settingsMoveMatchDirectory"]))
            {
                DirectoryInfo MoveMatchDir = new DirectoryInfo(ConfigurationManager.AppSettings["settingsMoveMatchDirectory"]);
                if (MoveMatchDir.Exists)
                {
                    moveMatched = true;
                    Console.WriteLine($"Matched files will be moved to {MoveMatchDir.FullName}");
                }
                else
                    Console.WriteLine($"settingsMoveMatchDirectory {ConfigurationManager.AppSettings["settingsMoveMatchDirectory"]} does not exist.");
            }
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["settingsMoveNoMatchDirectory"]))
            {
                DirectoryInfo MoveNoMatchDir = new DirectoryInfo(ConfigurationManager.AppSettings["settingsMoveNoMatchDirectory"]);
                if (MoveNoMatchDir.Exists)
                {
                    moveNoMatched = true;
                    Console.WriteLine($"Files with no matches will be moved to: {MoveNoMatchDir.FullName}");
                }
                else
                    Console.WriteLine($"settingsMoveNoMatchDirectory {ConfigurationManager.AppSettings["settingsMoveNoMatchDirectory"]} does not exist.");
            }
            if (!moveMatched)
                Console.WriteLine("Matched files will be logged.");
            if (!moveNoMatched)
                Console.WriteLine("Files with no matches will be logged.");

            //Wait for response before starting process.           
            Console.WriteLine("\nPress enter to continue.");
            Console.Read();
            Console.WriteLine();

            //Begin looping through files in the Read Directory
            foreach (FileInfo ReadFile in ReadDir.EnumerateFiles())
            {
                ++totalFilesRead;
                //take the name of the read file and remove the File Extension
                String searchName = ReadFile.Name.Substring(0, ReadFile.Name.Length - ReadFile.Extension.Length + 1) + searchExtension;
                //move or log the file depending on if a matching file was found
                if (CompareDir.EnumerateFiles(searchName).Any())
                {
                    ++totalFilesMatch;
                    if (moveMatched)
                    {
                        //move the file
                        try
                        {
                            ReadFile.MoveTo(ConfigurationManager.AppSettings["settingsMoveMatchDirectory"] + $@"\{ReadFile.Name}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"{e.Message} - {ReadFile.Name}");
                        }
                    }
                    else
                    {
                        //log that a match was found
                        Console.WriteLine($"   Match found - {ReadFile.Name}");
                    }
                }
                else
                {
                    if (moveNoMatched)
                    {
                        //move the file
                        try
                        {
                            ReadFile.MoveTo(ConfigurationManager.AppSettings["settingsMoveNoMatchDirectory"] + $@"\{ReadFile.Name}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"{e.Message} - {ReadFile.Name}");
                        }
                    }
                    else
                    {
                        //log that no match was found
                        Console.WriteLine($"No match found - {ReadFile.Name}");
                    }
                }
            }

            //log stats
            Console.WriteLine($"\nFiles Read: {totalFilesRead}");
            Console.WriteLine($"Files Matched: {totalFilesMatch}");
            //Keep displaying Console
            Console.Read();
        }
    }
}