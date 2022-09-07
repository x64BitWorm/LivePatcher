# Harry Potter and the Sorcerers Stone Patch
This patch allows you to fly in the game by pressing the "jump" button <br />
video: https://drive.google.com/file/d/17NcFl5bOKL6YLswcZs2xKeAL5GmdcKYw/view?usp=sharing <br />
usage: copy HarryPotterPatch.ptch to "System" folder in game (Hp.exe file is located in this folder) and run ".ptch" file with compiled LivePatcher.exe (omit .ptch in arguments) <br />
implementation: initially, the jump in the game is available if the player is in the "walking" state. while jumping, the player enters the "flying" state and therefore cannot jump again immediately. the patch replaces the true state of the player for a piece of code in which the game checks for the ability to jump <br />
