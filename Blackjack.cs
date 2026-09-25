using System;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class BlackjackLogic
{
    public enum MenuAction
    {
        Start,
        Quit,
        Invalid
    }
    private int money = 1000;
    private int bet = 0;
    private Deck deck = new Deck();

    private BlackjackRules rules = new BlackjackRules();

    private List<Hand> playerHand = new List<Hand>();
    private Hand dealerHand = new Hand();

    private readonly Action<string> DisplayMessage;

    private TaskCompletionSource<string> _inputTCS;

    private Emotions emotions = new Emotions();



    public BlackjackLogic(Action<string> displayMessage)
    {
        DisplayMessage = displayMessage;
    }

    public void SubmitInput(string input)
    {
        input ??= string.Empty;
        _inputTCS?.TrySetResult(input.Trim().ToLower());
        _inputTCS = null;
    }

    public async Task<string> RequestInputAsync()
    {
        _inputTCS = new TaskCompletionSource<string>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        return await _inputTCS.Task;
    }

    public async Task MainMenu()
    {
        DisplayMessage("\nWelcome to the game of Blackjack!" +
               "\n The source of your poverty since 1602!\n");

        while (true)
        {
            DisplayMessage("\nType 'start' to begin a new game, or 'quit' to quit.");

            string input = await RequestInputAsync();
            switch (GetGameAction(input))
            {
                case MenuAction.Start:
                    await GameLoop();
                    break;
                case MenuAction.Quit:
                    return;
                default:
                    DisplayMessage("\nInvalid command. Please type 'start' or 'quit'.");
                    break;
            }
        }
    }

    public void DisplayHand(List<Card> hand)
    {
        foreach (Card card in hand)
        {
            DisplayMessage($"{card.Rank}{card.Suit} ");
        }
    }

    public async Task<int> GetValidBet()
    {
        DisplayMessage(
            $"\nYou have ${money}. How much would you like to bet? " +
            $"(Enter a number between 1 and {money})");

        while (true)
        {
            string input = await RequestInputAsync();

            if (rules.TryGetValidBet(input, money, out int betAmount))
            {
                return betAmount;
            }

            DisplayMessage(
                "\nInvalid bet. Please enter a valid amount.");
        }
    }

    public MenuAction GetGameAction(string input)
    {
        input = input.ToLower().Trim();
        return input switch
        {
            "start" => MenuAction.Start,
            "quit" => MenuAction.Quit,
            _ => MenuAction.Invalid
        };
    }

    public MenuAction EndOfGameMenu(string input)
    {
        MenuAction endOfGameAction = GetGameAction(input);
        switch (endOfGameAction)
        {
            case MenuAction.Start:
                DisplayMessage("\nThe dealer shuffles the deck. Starting a new round...");
                break;
            case MenuAction.Quit:
                DisplayMessage($"\nThe dealer points to the door. You leave with {money} kr.");
                break;
            default:
                    DisplayMessage("\nInvalid command. Please type 'start' or 'quit'.");
                break;
        }
        return endOfGameAction;
    }

    public async Task GameLoop()
    {
        string input;

        bet = await GetValidBet();
        money = money - bet;

        playerHand.Clear();
        dealerHand.Clear();
        deck.InitializeDeck();
        // Start a new round of Blackjack
        DisplayMessage("\nStarting a new round...");
        // Draw two cards for the player
        playerHand.Add(new Hand());
        playerHand.First().cards.Add(deck.DrawCard());
        playerHand.First().cards.Add(deck.DrawCard());
        // Draw two cards for the dealer
        dealerHand.cards.Add(deck.DrawCard());
        dealerHand.cards.Add(deck.DrawCard());
        DisplayMessage($"\nDealer's visible card: {dealerHand.cards[0].Rank}{dealerHand.cards[0].Suit}");


        playerHand.First().isBlackjack = rules.IsBlackjack(rules.HandValues(playerHand.First().cards));
        dealerHand.isBlackjack = rules.IsBlackjack(rules.HandValues(dealerHand.cards));

        if (playerHand.First().isBlackjack)
        {
            DisplayMessage("\nYou have a blackjack! Await end of game.");
        }



        while (rules.HandCanContinue(playerHand))
        {
            DisplayMessage($"\nYour cards: ");


            for (int i = 0; i < playerHand.Count; i++)
            {
                var hand = playerHand[i];
                if (rules.HandCanContinue(playerHand) && !hand.isDone && !hand.isBust && !hand.isBlackjack)
                {
                    DisplayHand(hand.cards);
                    string playerAction = await RequestInputAsync();


                    switch (playerAction)
                    {
                        case "hit":
                            Card drawnCard = deck.DrawCard();
                            DisplayMessage($"\nCongratulations! You drew: {drawnCard.Rank}{drawnCard.Suit}");
                            hand.cards.Add(drawnCard);
                            DisplayMessage($"\nYour current hand has the cards and values: ");
                            string handCards = string.Join(" ", hand.cards.Select(c => $"{c.Rank}{c.Suit}"));
                            DisplayMessage(handCards);
                            string handValues = string.Join(" ", rules.HandValues(hand.cards));
                            DisplayMessage($"({handValues})");

                            break;
                        case "stand":
                            hand.isDone = true;
                            DisplayMessage("\nYou chose to stand. This hand is no longer in play.");
                            DisplayMessage($"\nFinal value of your hand: {string.Join(", ", rules.HandValues(hand.cards))}");
                            break;
                        case "split":
                            if (rules.CanSplit(hand))
                            {
                                DisplayMessage(rules.Split(hand,playerHand,ref money,bet));
                            }
                            else
                            {
                                DisplayMessage("\nYou can only split if you have two cards of the same rank, pick a new action.");
                                i--; // Decrement i to repeat the current hand's turn
                            }
                            break;
                        case "double":
                            DisplayMessage(rules.Double(ref money,ref bet,hand,deck));
                            break;
                    }
                    if (rules.HandValues(hand.cards).Min() > 21)
                    {
                        hand.isBust = true;
                        DisplayMessage("\nHand busted!");
                    }
                    else if (rules.HandValues(hand.cards).Contains(21))
                    {
                        hand.isDone = true;
                        DisplayMessage("\nYou got 21, hand is done! If you have more hands in play, they will be processed next.");
                    }
                }
            }
        }

        DisplayMessage("\nAll player hands are done. Dealer's turn.");
        DisplayMessage($"\nThe dealer reveals the second card with an expression of {emotions.EmotionalState()}! \nDealer's cards: {dealerHand.cards[0].Rank}{dealerHand.cards[0].Suit} {dealerHand.cards[1].Rank}{dealerHand.cards[1].Suit}");
        while (rules.HandValues(dealerHand.cards).Min() < 17)
        {
            Card drawnCard = deck.DrawCard();
            dealerHand.cards.Add(drawnCard);
            DisplayMessage($"\nPerhaps another card will give the dealer a sense of {emotions.EmotionalState()}!");
            DisplayMessage($"\nDealer draws: {drawnCard.Rank}{drawnCard.Suit}");
            DisplayMessage($"\nDealer's current hand: {string.Join(" ", dealerHand.cards.Select(c => $"{c.Rank}{c.Suit}"))}");
            DisplayMessage($"\nDealer's current hand values: {string.Join(", ", rules.HandValues(dealerHand.cards))}");
        }
        if (rules.HandValues(dealerHand.cards).Min() > 21)
        {
            dealerHand.isBust = true;
        }
        int dealerBestValue = rules.HandValues(dealerHand.cards).Where(v => v <= 21).DefaultIfEmpty(0).Max();
        if (dealerHand.isBlackjack)
        {
            DisplayMessage($"\nDealer's final hand: {string.Join(" ", dealerHand.cards.Select(c => $"{c.Rank}{c.Suit}"))}");
            DisplayMessage($"\nDealer's final hand values: {string.Join(", ", rules.HandValues(dealerHand.cards))}");
            DisplayMessage($"\nDealer's best hand value: {dealerBestValue}");
        }
        else if (dealerHand.isBust)
        {
            foreach (var hand in playerHand)
            {
                if (!hand.isBust)
                {
                    DisplayMessage($"\nYour hand with cards {string.Join(" ", hand.cards.Select(c => $"{c.Rank}{c.Suit}"))} wins against the dealer's busted hand!");
                }
                else
                {
                    DisplayMessage($"\nYour hand with cards {string.Join(" ", hand.cards.Select(c => $"{c.Rank}{c.Suit}"))} also busted, so you lose.");
                }
            }
        }
        else
        {
            DisplayMessage($"\nDealer stands with a hand value of {dealerBestValue}.");
        }

        foreach (var hand in playerHand)
        {
            if (!hand.isBust && !dealerHand.isBust)
            {
                int playerBestValue = rules.HandValues(hand.cards).Where(v => v <= 21).DefaultIfEmpty(0).Max();
                if (playerBestValue > dealerBestValue)
                {
                    DisplayMessage($"\nYour hand with cards {string.Join(" ", hand.cards.Select(c => $"{c.Rank}{c.Suit}"))} wins against the dealer's hand!");
                    money = money + bet * 2;
                    if (hand.isBlackjack)
                    {
                        money = money + bet;
                    }
                }
                else if (playerBestValue < dealerBestValue)
                {
                    DisplayMessage($"\nYour hand with cards {string.Join(" ", hand.cards.Select(c => $"{c.Rank}{c.Suit}"))} loses against the dealer's hand.");
                }
                else
                {
                    DisplayMessage($"\nYour hand with cards {string.Join(" ", hand.cards.Select(c => $"{c.Rank}{c.Suit}"))} ties with the dealer's hand.");
                    money = money + bet;
                }
            }
        }
        DisplayMessage($"\nRound over. The dealer looks at you with {emotions.EmotionalState()} before giving you two options." +
            $"\nStart, to play again." +
            $"\nQuit, to stop where you are.");

        MenuAction endOfGameAction;
        do
        {
            input = await RequestInputAsync();
            endOfGameAction = EndOfGameMenu(input);
        } while (endOfGameAction == MenuAction.Invalid);
        if (endOfGameAction == MenuAction.Start)
            await GameLoop();
    }

}
