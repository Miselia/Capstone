using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Deck 
{
    // Start is called before the first frame update
    string filepath;
    Stack<int> cards;

    public Deck(string f)
    {
        cards = new Stack<int>();
        this.filepath = "Assets/Resources/"+f;
        Debug.Log(filepath);
        buildDeck(filepath);
        
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
    public int drawCard()
    {
        if (cards.Count != 0) return cards.Pop();
        else return 0;
    }

}
