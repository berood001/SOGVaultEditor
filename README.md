Download the Swords of Glass Vault Editor Here:
https://github.com/berood001/SOGVaultEditor/releases/download/game/Swords-of-Glass-VaultEditor.zip

Swords of Glass is an old dungeon crawler that I played as a kid in the mid 80s. I have no idea why but I decided to
go back and try to beat the game again. On level 3 I got mad at dropping all my items in quicksand, and then decided to hack the vault file.
I didn't know how to code in the 80s, but 40 years later, I have come back for my revenge.

See the game here and download in the Swords of Glass Shrine:
http://www.thealmightyguru.com/Reviews/SwordsOfGlass/Index.html

Swords of Glass Vault Editor is a program written in C# WPF that allows a user to hack the hex encoded vault file
for the game and give themselves items or make changes to items. 

I wrote this in a few hours and haven't tested every possible use case. I don't have catches and validation built in, so if you
don't use the program correctly it will just crash. For instance, you dont put the line item of the row you want to change and 
hit the change button. You will get a null exception error and the program crashes. 

The main issue I have seen is that the quality/quantity field it is easy to put in numbers that get translated to a 3 digit hex 
value when the program only allows 2 digits. The program will crash when you try to change the line. I put in default quantity/
quality numbers that should work, but they haven't all been tested.

How to use:
1. MAKE A BACK UP OF YOUR VAULT.DAT FILE. The program hasn't created a corrupt VAULT.DAT in all my testing, but keep a back up in case.
2. Open program and set the location of the current VAULT.DAT file. (File isn't certified, so windows will make you confirm that you want to open it)
3. Get Vault Button Click - and you will get a list of rows with items and their hex values (IF ID = 00 that line is empty)
4. Type in the row # you wish to change next to the ChangeValueItem button.
5. Pick the item you want to add in the dropdown.
6. ID = Item ID (hex), Bonus (+value added to item), Qual(number of items or the condition of the item), Qual256 (multipler for quality, remember we are using 2 digit hex codes so you need a multipler for larger numbers), Buffer (length of item name, always 10), Item Name (Item Name as ascii)
7. Set the destination you wish to export the new VAULT.DAT file. I will need to be named VAULT.DAT and in the exe folder for the game to read it
8. Click Export Vault Button (there are no alerts or satisfying messages saying everything was a success, the file will just appear)
9. If needed move the file you created to game folder and rename  

