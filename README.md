# Blackjack
Simple Blackjack game

## Description:
This project is a text-based implementation of the card game Blackjack.
The game starts with a bet. A bet in this game is between 1 and 50.000 kr and cannot exceed the player's current amount of money. The player starts with 1,000 kr.
To win a game of blackjack you need to achieve a card value higher than the dealer without exceeding a point value of 21.
Cards have the value of the number on the card while J, Q, K have the value of 10, and A has the value of either 1 or 11, whichever is closest to 21 and if possible also without exceeding it will be prioritized.
The player is initially dealt two cards, as is the dealer. One of the dealer's cards is revealed to the player.
If the player has a value of 21 with their initial two cards, they have a Blackjack. The round ends immediately. If the dealer also has a Blackjack, the result is a tie. Otherwise, the player wins and receives double their bet.

During a normal round, the player has the following options:
Hit: Draw another card and add it to the player's hand.
Double: Double the current bet, draw exactly one additional card, and end the hand.
Stand: End the current hand without drawing another card.
Split: If the player's hand contains exactly two cards of the same value, the player may place an additional bet and split the cards into two separate hands.

A hand ends when the player chooses to stand or double, achieves 21, or busts by exceeding 21.
When the player has finished playing all their hands, it becomes the dealer's turn. The dealer reveals their hidden card and draws cards until their total value is at least 17.
The dealer will then reveal their other card. If the dealer has blackjack it will tie with the player if they have blackjack. If the player has 21 through drawing cards, and the dealer has blackjack, the dealer wins, and vice versa.
If the dealer busts, every player hand that has not already busted wins. Otherwise, each remaining player hand is compared against the dealer's hand.

The game presents the player with text-based menus between rounds. The main menu contains the following options:
Start: Start a new game.
Quit: Exit the game.

When starting a game, the player is prompted to enter a bet between 1 and 50,000 kr. The bet cannot exceed the player's current balance.
During a game, the player can enter hit, stand, double, or split to perform the corresponding action.

## Software Architecture
The project is structured using a separation between the game logic and the user interface.

The main components of the project are:

Game: Controls the overall flow of a Blackjack game, including starting rounds, handling the player's turns, handling the dealer's turn, and determining the result of a round.
Player: Represents the player and keeps track of the player's money and hands.
Dealer: Represents the dealer and is responsible for playing according to the dealer's rules.
Hand: Represents a hand of cards and contains the cards belonging to that hand.
Card: Represents an individual playing card, including its suit and value.
Deck: Represents the deck of cards and is responsible for creating, storing, and drawing cards.
User Interface: Handles input from the player and displays game information and results in the console. The UI communicates with the game logic by passing the player's commands to the appropriate game functionality.

The game logic is kept separate from the console input and output where possible. This allows the rules of Blackjack to be handled independently from how the player interacts with the game.

## Building the Project

The project requires the .NET SDK to be installed.

Clone or download the project.
Open a terminal in the project directory.
Restore the project dependencies:
dotnet restore
Build the project:
dotnet build

A successful build indicates that the project has compiled correctly.

Testing

The automated tests can be run from the project directory using:

dotnet test

The command builds the test project and executes all available tests. The test output will show whether the individual tests passed or failed.

Running the Game

After building the project, the game can be started using:

dotnet run

The game will start in the main menu.

Select start to begin a game. You will then be asked to enter your bet.

During the game, enter one of the available commands:

hit
stand
double
split

After the round has ended, the result and updated player balance are displayed and the player is returned to the main menu.

To exit the game, type "quit" from the main menu.