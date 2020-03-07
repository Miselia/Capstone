using Assets.Resources;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Deck 
{
    // Start is called before the first frame update
    private int maxDeckSize = Constants.MaxDeckSize;
    private string filepath;
    private List<int> deck;
    int topOfDeck = 0;
    private string primaryFaction = "";
    private string secondaryFaction = "";


    public Deck(string f)
    {
        deck = new List<int>();
        this.filepath = "Assets/Resources/"+f;
        if (!buildDeck(filepath))
        {
            Debug.Log("Filepath not found");
            deck = null;
        }
        else
        {
            Shuffle();
            string output = " ";
            for (int i = 0; i < deck.Count; i++)
            {
                output += deck[i] + " ,";
            }
            Debug.Log(filepath + output);
            Debug.Log("Filepath loaded correctly");
        }
    }

    public Deck(string primary, string secondary)
    {
        deck = new List<int>();
        primaryFaction = primary;
        secondaryFaction = secondary;
    }
    public Deck()
    {
        deck = new List<int>();
        primaryFaction = "Fantasy";
        secondaryFaction = "Steampunk";
    }

    private bool buildDeck(string file)
    {
        StreamReader reader = new StreamReader(file);
        primaryFaction = reader.ReadLine().Replace("\n", "");
        secondaryFaction = reader.ReadLine().Replace("\n", "");
        bool flag = true;
        bool deckLimitReached = false;
        while(flag)
        {
            if ( reader.Peek() == -1) flag = false;
            else
            {
                string nextLine = reader.ReadLine();
                nextLine.Replace("\n", "");
                //Debug.Log(nextLine);
                int tempInt = int.Parse(nextLine);
                //Debug.Log(tempInt);
                if (AddCard(tempInt) == false)
                    deckLimitReached = true;
            }
        }
        if (deckLimitReached)
        {
            Debug.Log("Deck limit reached at Deck size = " + deck.Count);
            return false;
        }
        else
        {
            Debug.Log("Deck limit NOT reached. Deck size = " + deck.Count);
            return true;
        }
    }

    // Cite this source: https://stackoverflow.com/questions/273313/randomize-a-listt by user "Shital Shah"
    public void Shuffle()
    {
        for(int i = deck.Count; i > 0; i--)
        {
            // finish code from link
            topOfDeck = 0;
            int val = Random.Range(0, i);
            int temp = deck[0];
            deck[0] = deck[val];
            deck[val] = temp;
        }
    }

    public int DrawCard()
    {
        if (deck.Count > topOfDeck)
        {
            topOfDeck += 1;
            return deck[topOfDeck-1];
        }
        else return 0;
    }
    public string[] getFactions()
    {
        return new string[] { primaryFaction, secondaryFaction };
    }

    public bool AddCard(int cardID)
    {
        if (deck.Count < maxDeckSize)
        {
            deck.Add(cardID);
            return true;
        }
        else
            return false;
    }

    public bool RemoveCard(int cardID)
    {
        return deck.Remove(cardID);
    }

    public List<int> GetDeck()
    {
        return deck;
    }
}
