# XML-Unity-Save-System
A flexible, expansible Save System for Unity developers; all in one file.


# How to add this to your project
Just drag and drop the ```SaveManager.cs``` script into your project and you are ready to use it


# How to use it via code
Whenever you want to do a save or load access the static ```SaveManager``` class

Use one of the overloads of the ```Save``` methods to save, same for the ```Load``` ones. There are already created classes to handle your data: ```PlayerData```, ```GeneralData```, ```CustomizationData```; but you can create your own ones, inside them you will have to include the variables you want to serialize to the afterwards created XML file in the user's computer.
Also static members of this classes are found inside the ```SaveManager``` class, so you don't have to create instances for every time you load or save.

There is a ```public enum DocType``` to select what type of class you are saving or loading your data from.

Additional methods such as ```LoadAndSave``` or ```SaveAndLoad``` are provided making use of Actions delegates for specific use cases.
