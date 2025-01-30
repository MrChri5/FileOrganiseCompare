# FileOrganiseCompare
**Project File Comparison Organiser**

This program is used to read all the files in a specified read folder (App.congif settingsReadDirectory), and for each one check 
if a file with the same name but possibly a different file extension exists in the specified compare 
folder (settingsCompareDirectory). A specific file extension (settingsFileExtension) may be chosen to look for in the compare 
folder. Files in the read folder can then be moved to a given folder (settingsMoveMatchDirectory) if a matching file is found. 
Files that do not have a matching file can be moved to a separate folder (settingsMoveNoMatchDirectory). These two settings can 
be left blank and the files will not be moved. Instead a message will be output to the console for each one.

This program was designed to help tidy up my photo editing folders where there are unedited images in one folder and editing files (.xcf) 
with the same name as the unedited image in another folder.
