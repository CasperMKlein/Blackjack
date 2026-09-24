using System;
using System.Xml;

public class Card
{
    private char suit;
    private char rank;

    public char Suit
    {
        get { return suit; }
        set { suit = value; }
    }
    public char Rank
    {
        get { return rank; }
        set { rank = value; }
    }
}

public class Deck
{
    public List<Card> cards;

    public Deck()
    {
        cards = new List<Card>();
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        cards.Clear();
        char[] suits = { '♥', '♦', '♣', '♠' }; // Hearts, Diamonds, Clubs, Spades
        char[] ranks = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
        int index = 0;
        foreach (char suit in suits)
        {
            foreach (char rank in ranks)
            {
                cards.Add(new Card { Suit = suit, Rank = rank });
                index++;
            }
        }
    }

    public Card DrawCard()
    {
        Random random = new Random();
        if (0 < cards.Count)
        {
            var randomIndex = random.Next(0, cards.Count);
            var pickedCard = cards[randomIndex];
            cards.RemoveAt(randomIndex);
            return pickedCard;
        }
        else
        {
            throw new InvalidOperationException("No more cards in the deck.");
        }
    }
}

public class Hand
{
    public List<Card> cards = new List<Card>();
    public bool isDone = false;
    public bool isBust = false;
    public bool isBlackjack = false;

    public void Clear()
    {
        cards.Clear();
        isDone = false;
        isBust = false;
        isBlackjack = false;
    }
}