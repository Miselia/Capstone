using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Deck 
{
    // Start is called before the first frame update
    public Random rand;
    string filepath;
    Stack<int> cards;

    public Deck(string f)
    {
        cards = new Stack<int>();
        this.filepath = "Assets/Resources/"+f;
        rand = new Random();
        Debug.Log(filepath);
        buildDeck(filepath);

        // Cite this source: https://stackoverflow.com/questions/273313/randomize-a-listt by user "Shital Shah"
        int[] deck = cards.ToArray();
        //ShuffleAndStack(deck);
    }

    private void buildDeck(string file)
    {
        StreamReader reader = new StreamReader(file);
        bool flag = true;
        while(flag)
        {
            
            if ( reader.Peek() == -1) flag = false;
            else
            {
                string nextLine = reader.ReadLine();
                nextLine.Replace("\n", "");
                Debug.Log(nextLine);
                int tempInt = int.Parse(nextLine);
                Debug.Log(tempInt);
                cards.Push(tempInt);
            }
        }
    }

    private void ShuffleAndStack(int[] deck)
    {
        for(int i = deck.Length; i > 0; i--)
        {
            int temp = deck[i];
            // finish code from link
            deck[i] = 0;
        }
    }

    public int drawCard()
    {
        if (cards.Count != 0) return cards.Pop();
        else return 0;
    }

}
