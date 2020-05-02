using Assets.Resources;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Deck 
{
    // ValidDecks is a folder that serves to hold all valid decks
    // This makes deck loading from the Lobby very easy as it will only display decks that are valid
    // This means that each deck, valid or invalid, will be stored in the Assets/Resources directory,
    // while ValidDecks will also be stored in the ValidDecks folder (meaning we store the same file twice)

    // Start is called before the first frame update
    private int maxDeckSize = Constants.MaxDeckSize;
    private int maxCopiesPerDeck = Constants.MaxCopiesPerDeck;
    private string filepath;
    private List<int> deck;
    int topOfDeck = 0;
    private string primaryFaction = "";
    private string secondaryFaction = "";
    private string deckName = "";


    public Deck(string f)
    {
        deck = new List<int>();
        deckName = f;
        this.filepath = "Assets/Resources/"+deckName + ".txt";
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

    public Deck(string primary, string secondary, string name)
    {
        deck = new List<int>();
        deckName = name;
        this.filepath = "Assets/Resources/" + deckName + ".txt";
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
        while (flag)
        {
            if (reader.Peek() == -1) flag = false;
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
    public void SaveDeck()
    {
        StreamWriter writer = new StreamWriter((filepath), false);
        writer.WriteLine(primaryFaction);
        writer.WriteLine(secondaryFaction);
        for (int i = 0; i < deck.Count; i++) { 
            writer.WriteLine(deck[i]);
        }
        writer.Close();

        // If deck is valid, write to "ValidDecks" folder
        if (CheckDeckIsValid())
        {
            filepath = "Assets/Resources/ValidDecks/" + deckName + ".txt";
            StreamWriter validWriter = new StreamWriter((filepath), false);
            validWriter.WriteLine(primaryFaction);
            validWriter.WriteLine(secondaryFaction);
            for (int i = 0; i < deck.Count; i++)
            {
                validWriter.WriteLine(deck[i]);
            }
            validWriter.Close();
        }
    }

    public bool CheckDeckIsValid()
    {
        // This is an O(n^2) operation, it could be better, but it is what is it
        bool ret = false;
        if (deck.Count == maxDeckSize)
        {
            ret = true;
            foreach (int card in deck)
            {
                if (CountCopiesInDeck(card) > maxCopiesPerDeck)
                    ret = false;
            }
        }
        return ret;
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
        if (deck.Count < maxDeckSize && CountCopiesInDeck(cardID) < maxCopiesPerDeck )
        {
            deck.Add(cardID);
            return true;
        }
        else
            return false;
    }

    private int CountCopiesInDeck(int compare)
    {
        int ret = 0;
        foreach(int i in deck)
        {
            if (i == compare)
                ret += 1;
        }
        return ret;
    }

    public bool RemoveCard(int cardID)
    {
        return deck.Remove(cardID);
    }

    public List<int> GetDeck()
    {
        return deck;
    }
    public int GetPrimary()
    {
        switch (primaryFaction)
        {
            case "Fantasy":
                return 1;
            case "Steampunk":
                return 2;
            case "Sci-Fi":
                return 3;
            case "Horror":
                return 4;


        }
        return 0;
    }
    public string GetPrimaryString()
    {
        return primaryFaction;
    }
    public string GetSecondary()
    {
        return secondaryFaction;
    }
}
