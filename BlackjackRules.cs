using System;
using System.Data;

public class BlackjackRules
{
    public bool CanSplit(Hand hand)
    {
        return hand.cards.Count == 2 && CardValue(hand.cards[0])[0] == CardValue(hand.cards[1])[0];
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

    public bool HandCanContinue(List<Hand> handCheck)
    {
        foreach (var hand in handCheck)
        {
            if (!hand.isDone && !hand.isBust && !hand.isBlackjack)
            {
                return true;
            }
        }
        return false;
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

    public bool TryGetValidBet(string input, int money, out int betAmount)
    {
        if (int.TryParse(input, out int parsedNumber) &&
            parsedNumber > 0 &&
            parsedNumber <= money)
        {
            betAmount = parsedNumber;
            return true;
        }

        betAmount = 0;
        return false;
    }

}
