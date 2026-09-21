using System;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class BlackjackLogic
{
    private int money = 1000;
    private int bet = 0;
    private Deck deck = new Deck();

    private List<Hand> playerHand = new List<Hand>();
    private Hand dealerHand = new Hand();

    private readonly Action<string> DisplayMessage;

    private TaskCompletionSource<string> _inputTCS;

    List<string> emotions = new List<string>
{
    "great sorrow",
    "anguish",
    "joy",
    "happiness",
    "delight",
    "ecstasy",
    "euphoria",
    "bliss",
    "contentment",
    "satisfaction",
    "gratitude",
    "relief",
    "hope",
    "optimism",
    "excitement",
    "enthusiasm",
    "anticipation",
    "wonder",
    "awe",
    "admiration",
    "love",
    "affection",
    "fondness",
    "tenderness",
    "adoration",
    "devotion",
    "compassion",
    "empathy",
    "sympathy",
    "trust",
    "belonging",
    "connection",
    "pride",
    "confidence",
    "courage",
    "determination",
    "inspiration",
    "amusement",
    "playfulness",
    "curiosity",
    "fascination",
    "interest",
    "serenity",
    "peace",
    "calm",
    "tranquility",
    "nostalgia",
    "longing",
    "yearning",
    "desire",
    "passion",
    "infatuation",
    "attraction",
    "envy",
    "jealousy",
    "resentment",
    "bitterness",
    "hatred",
    "loathing",
    "disgust",
    "contempt",
    "anger",
    "rage",
    "fury",
    "irritation",
    "annoyance",
    "frustration",
    "exasperation",
    "agitation",
    "impatience",
    "indignation",
    "betrayal",
    "disappointment",
    "discouragement",
    "despair",
    "hopelessness",
    "helplessness",
    "grief",
    "mourning",
    "sadness",
    "sorrow",
    "melancholy",
    "misery",
    "loneliness",
    "isolation",
    "emptiness",
    "heartbreak",
    "regret",
    "remorse",
    "guilt",
    "shame",
    "embarrassment",
    "humiliation",
    "insecurity",
    "self-doubt",
    "inferiority",
    "fear",
    "terror",
    "horror",
    "dread",
    "panic",
    "anxiety",
    "worry",
    "apprehension",
    "nervousness",
    "unease",
    "uncertainty",
    "confusion",
    "bewilderment",
    "perplexity",
    "disbelief",
    "shock",
    "surprise",
    "astonishment",
    "amazement",
    "suspicion",
    "distrust",
    "skepticism",
    "ambivalence",
    "indifference",
    "apathy",
    "boredom",
    "weariness",
    "exhaustion",
    "overwhelm",
    "vulnerability",
    "insecurity",
    "exposure",
    "alienation",
    "disconnection",
    "resignation",
    "acceptance",
    "forgiveness",
    "reconciliation",
    "reluctance",
    "hesitation",
    "doubt",
    "conflict",
    "tension",
    "anticipation",
    "eagerness",
    "urgency",
    "impatience",
    "satisfaction",
    "triumph",
    "victory",
    "accomplishment",
    "fulfillment",
    "empowerment",
    "liberation",
    "freedom",
    "reassurance",
    "security",
    "safety",
    "warmth",
    "comfort",
    "affection",
    "appreciation",
    "respect",
    "reverence",
    "humility",
    "shyness",
    "awkwardness",
    "timidity",
    "cautiousness",
    "wariness",
    "defensiveness",
    "possessiveness",
    "protectiveness",
    "sympathy",
    "pity",
    "compassion",
    "kindness",
    "generosity",
    "altruism",
    "satisfaction",
    "pride",
    "vanity",
    "arrogance",
    "superiority",
    "inferiority",
    "ambition",
    "motivation",
    "drive",
    "persistence",
    "resilience",
    "perseverance",
    "defeat",
    "failure",
    "humiliation",
    "desperation",
    "franticness",
    "restlessness",
    "agitation",
    "irritability",
    "hostility",
    "aggression",
    "vengeance",
    "vindictiveness",
    "malice",
    "cruelty",
    "spite",
    "schadenfreude",
    "reluctant hope",
    "bittersweetness",
    "melancholy",
    "wistfulness",
    "sentimentality",
    "yearning",
    "homesickness",
    "estrangement",
    "loss",
    "grief",
    "devastation",
    "desolation",
    "despair",
    "anguish",
    "torment",
    "agony",
    "suffering",
    "distress",
    "affliction",
    "desperation",
    "terror",
    "existential dread",
    "paranoia",
    "foreboding",
    "ominous anticipation",
    "relief",
    "reassurance",
    "renewed hope",
    "optimism",
    "faith",
    "trust",
    "wonder",
    "reverence",
    "transcendence",
    "spiritual awe",
    "inner peace",
    "serenity",
    "contentment",
    "acceptance",
    "gratitude",
    "joyful anticipation",
    "ecstatic joy",
    "overwhelming happiness",
    "quiet happiness",
    "profound sadness",
    "deep loneliness",
    "crushing disappointment",
    "burning anger",
    "consuming hatred",
    "paralyzing fear",
    "overwhelming anxiety",
    "deep affection",
    "unconditional love",
    "profound gratitude",
    "bittersweet nostalgia",
    "quiet melancholy",
    "restless curiosity",
    "intense fascination",
    "overwhelming awe",
    "guilty pleasure",
    "moral outrage",
    "righteous indignation",
    "quiet resentment",
    "suppressed anger",
    "hidden sadness",
    "repressed fear",
    "guarded optimism",
    "cautious hope",
    "desperate longing",
    "unrequited love",
    "forbidden desire",
    "shattered trust",
    "betrayed affection",
    "emotional exhaustion",
    "numbness",
    "emotional detachment",
    "emptiness",
    "inner turmoil",
    "emotional conflict",
    "quiet desperation",
    "overwhelming relief",
    "unexpected joy",
    "bittersweet joy",
    "triumphant exhilaration"
};

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

    private Task<string> RequestInputAsync()
    {
        _inputTCS = new TaskCompletionSource<string>(
            TaskCreationOptions.RunContinuationsAsynchronously);
        return _inputTCS.Task;
    }

    public async Task MainMenu()
    {
        DisplayMessage("\nWelcome to the game of Blackjack!" +
               "\n The source of your poverty since 1602!\n");

        while (true)
        {
            DisplayMessage("\nType 'start' to begin a new game, or 'exit' to quit.");

            string input = await RequestInputAsync();

            if (input == "start")
            {
                await GameLoop();
            }
            else if (input == "exit")
            {
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                DisplayMessage("\nInvalid command. Please type 'start' or 'exit'.");
            }
        }
    }

    public int[] CardValue(Card card)
    {
        int[] values = new int[2]; // Array to hold possible values for Ace


        switch (card.Rank)
        {
            case 'A':
                values[0] = 1; // Ace as 1
                values[1] = 11; // Ace as 11
                break;
            case 'K':
            case 'Q':
            case 'J':
            case 'T':
                values[0] = 10;
                values[1] = 10;
                break;
            default:
                values[0] = int.Parse(card.Rank.ToString());
                values[1] = int.Parse(card.Rank.ToString());
                break;
        }

        return values;
    }

    public List<int> HandValues(List<Card> hand)
    {
        List<int> handValues = new List<int> { 0 };

        foreach (Card card in hand)
        {
            int[] cardValues = CardValue(card);
            var currentCount = handValues.Count;
            if (card.Rank == 'A')
            {
                for (int i = 0; i < currentCount; i++)
                {
                    handValues.Add(handValues[i] + cardValues[1]); // Add Ace as 11
                    handValues[i] += cardValues[0]; // Add Ace as 1
                }
            }
            else
            {
                for (int i = 0; i < currentCount; i++)
                {
                    handValues[i] += cardValues[0]; // Add card value
                }
            }
        }

        List<int> shortenedValues = new List<int>();
        while (handValues.Count > 0)
        {
            int value = handValues[0];
            handValues.RemoveAt(0);
            if (!shortenedValues.Contains(value))
            {
                shortenedValues.Add(value);
            }
        }


        return shortenedValues;
    }

    public bool IsBlackjack(List<int> handValues)
    {
        bool isBlackjack = false;
        if (handValues.Contains(21))
        {
            isBlackjack = true;
        }
        return isBlackjack;
    }

    public void DisplayHand(List<Card> hand)
    {
        foreach (Card card in hand)
        {
            DisplayMessage($"{card.Rank}{card.Suit} ");
        }
    }

    public bool ContinueCheck()
    {
        foreach (var hand in playerHand)
        {
            if (!hand.isDone && !hand.isBust && !hand.isBlackjack)
            {
                return true;
            }
        }
        return false;
    }

    public async Task GetValidBet()
    {
        DisplayMessage($"\nYou have ${money}. How much would you like to bet? (Enter a number between 1 and {money})");
        string betString = await RequestInputAsync();
        if (int.TryParse(betString, out int betAmount) && betAmount > 0 && betAmount <= money)
            {
                bet = betAmount;
            }
            else
            {
                DisplayMessage("\nInvalid bet. Please enter a valid amount.");
                await GetValidBet();
            }
    }

    public async Task EndOfGameMenu()
    {
        string input = await RequestInputAsync();
        switch (input)
        {
            case "start":
                DisplayMessage("\nThe dealer shuffles the deck. Starting a new round...");
                break;
            case "quit":
                DisplayMessage($"\nThe dealer points to the door. You leave with ${money} kr.");
                System.Windows.Forms.Application.Exit();
                break;
            default:
                DisplayMessage("\nInvalid command. Please type 'start' or 'quit'.");
                await EndOfGameMenu();
                break;
        }
    }

    public async Task GameLoop()
    {

        await GetValidBet();

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


        playerHand.First().isBlackjack = IsBlackjack(HandValues(playerHand.First().cards));
        dealerHand.isBlackjack = IsBlackjack(HandValues(dealerHand.cards));

        if (playerHand.First().isBlackjack)
        {
            DisplayMessage("\nYou have a blackjack! Await end of game.");
        }



        while (ContinueCheck())
        {
            DisplayMessage($"\nYour cards: ");


            for (int i = 0; i < playerHand.Count; i++)
            {
                var hand = playerHand[i];
                if (ContinueCheck() && !hand.isDone && !hand.isBust && !hand.isBlackjack)
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
                            string handValues = string.Join(" ", HandValues(hand.cards));
                            DisplayMessage($"({handValues})");

                            break;
                        case "stand":
                            hand.isDone = true;
                            DisplayMessage("\nYou chose to stand. This hand is no longer in play.");
                            DisplayMessage($"\nFinal value of your hand: {string.Join(", ", HandValues(hand.cards))}");
                            break;
                        case "split":
                            if (hand.cards.Count == 2 && CardValue(hand.cards[0])[0] == CardValue(hand.cards[1])[0])
                            {
                                Hand newHand = new Hand();
                                newHand.cards.Add(hand.cards[1]);
                                hand.cards.RemoveAt(1);
                                playerHand.Add(newHand);
                                DisplayMessage("\nYou have split your hand into two hands.");
                                DisplayMessage($"\nFirst hand: {hand.cards[0].Rank}{hand.cards[0].Suit}");
                                DisplayMessage($"\nSecond hand: {newHand.cards[0].Rank}{newHand.cards[0].Suit}");
                                DisplayMessage("\nYou will now play the first hand. After that, you will play the second hand. Enjoy!");
                            }
                            else
                            {
                                DisplayMessage("\nYou can only split if you have two cards of the same rank, pick a new action.");
                                i--; // Decrement i to repeat the current hand's turn
                            }
                            break;
                        case "double":
                            hand.cards.Add(deck.DrawCard());
                            hand.isDone = true;
                            DisplayMessage($"\nYou chose to double down. You drew: {hand.cards.Last().Rank}{hand.cards.Last().Suit}");
                            break;
                    }
                    if (HandValues(hand.cards).Min() > 21)
                    {
                        hand.isBust = true;
                        DisplayMessage("\nHand busted!");
                    }
                    else if (HandValues(hand.cards).Contains(21))
                    {
                        hand.isDone = true;
                        DisplayMessage("\nYou got 21, hand is done! If you have more hands in play, they will be processed next.");
                    }
                }
            }
        }

        DisplayMessage("\nAll player hands are done. Dealer's turn.");
        Random random = new Random();
        string dealerEmotionalState = this.emotions[random.Next(this.emotions.Count)];
        DisplayMessage($"\nThe dealer reveals the second card with an expression of {dealerEmotionalState}! \nDealer's cards: {dealerHand.cards[0].Rank}{dealerHand.cards[0].Suit} {dealerHand.cards[1].Rank}{dealerHand.cards[1].Suit}");
        while (HandValues(dealerHand.cards).Min() < 17)
        {
            Card drawnCard = deck.DrawCard();
            dealerHand.cards.Add(drawnCard);
            dealerEmotionalState = this.emotions[random.Next(this.emotions.Count)];
            DisplayMessage($"\nPerhaps another card will give the dealer a sense of {dealerEmotionalState}!");
            DisplayMessage($"\nDealer draws: {drawnCard.Rank}{drawnCard.Suit}");
            DisplayMessage($"\nDealer's current hand: {string.Join(" ", dealerHand.cards.Select(c => $"{c.Rank}{c.Suit}"))}");
            DisplayMessage($"\nDealer's current hand values: {string.Join(", ", HandValues(dealerHand.cards))}");
        }
        if (HandValues(dealerHand.cards).Min() > 21)
        {
            dealerHand.isBust = true;
        }
        int dealerBestValue = HandValues(dealerHand.cards).Where(v => v <= 21).DefaultIfEmpty(0).Max();
        if (dealerHand.isBlackjack)
        {
            DisplayMessage($"\nDealer's final hand: {string.Join(" ", dealerHand.cards.Select(c => $"{c.Rank}{c.Suit}"))}");
            DisplayMessage($"\nDealer's final hand values: {string.Join(", ", HandValues(dealerHand.cards))}");
            DisplayMessage($"\nDealer's best hand value: {dealerBestValue}");
        } else if (dealerHand.isBust)
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
                int playerBestValue = HandValues(hand.cards).Where(v => v <= 21).DefaultIfEmpty(0).Max();
                if (playerBestValue > dealerBestValue)
                {
                    DisplayMessage($"\nYour hand with cards {string.Join(" ", hand.cards.Select(c => $"{c.Rank}{c.Suit}"))} wins against the dealer's hand!");
                }
                else if (playerBestValue < dealerBestValue)
                {
                    DisplayMessage($"\nYour hand with cards {string.Join(" ", hand.cards.Select(c => $"{c.Rank}{c.Suit}"))} loses against the dealer's hand.");
                }
                else
                {
                    DisplayMessage($"\nYour hand with cards {string.Join(" ", hand.cards.Select(c => $"{c.Rank}{c.Suit}"))} ties with the dealer's hand.");
                }
            }
        }
        dealerEmotionalState = this.emotions[random.Next(this.emotions.Count)];
        DisplayMessage($"\nRound over. The dealer looks at you with {dealerEmotionalState} before giving you two options." +
            $"\nStart, to play again." +
            $"\nQuit, to stop where you are.");


        await EndOfGameMenu();
        await GameLoop();
    }

}
