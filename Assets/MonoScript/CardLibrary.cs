using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardLibrary: MonoBehaviour
{
    [SerializeField] private List<Material> cardMaterialLibrary;
    private Dictionary<string, List<CardData>> library;
    // Start is called before the first frame update
    void Awake()
    {
        library = new Dictionary<string, List<CardData>>();
        library.Add("Default", new List<CardData>());
        library.Add("Fantasy", new List<CardData>());
        library.Add("Steampunk", new List<CardData>());
        library.Add("Horror", new List<CardData>());
        library.Add("Sci-Fi", new List<CardData>());

        //CardID, Name, manaCost, faction, traits, material
        //0 Unassigned, 1 Fantasy, 2 Steampunk, 3 Horror, 4 Sci-Fi
        library["Default"].Add(new CardData(0, "Default", 0, 0, "None", cardMaterialLibrary[0]));
        library["Horror"].Add(new CardData(1, "Blood Boil", 2, 3, "Speed+", cardMaterialLibrary[1]));
        library["Fantasy"].Add(new CardData(2, "Fire Bolt", 3, 1, "Fire", cardMaterialLibrary[2]));
        library["Sci-Fi"].Add(new CardData(3, "Purple Shot", 4, 4, "", cardMaterialLibrary[3]));
        library["Steampunk"].Add(new CardData(4, "Red Coin", 5, 2, "", cardMaterialLibrary[4]));

        //Debug.Log(cardList[0].getID() + "," + cardList[1].getID() + "," + cardList[2].getID() + "," + cardList[3].getID() + "," + cardList[4].getID());
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */
    public List<CardData> GetListByID(string faction)
    {
        return library[faction].OrderBy(c => c.cardID).ToList();
    }
    public List<CardData> GetAllByID()
    {
        List<CardData> cardLibrary = new List<CardData>();
        cardLibrary = GetListByID("Default");
        cardLibrary.AddRange(GetListByID("Fantasy"));
        cardLibrary.AddRange(GetListByID("Steampunk"));
        cardLibrary.AddRange(GetListByID("Horror"));
        cardLibrary.AddRange(GetListByID("Sci-Fi"));
        return cardLibrary.OrderBy(c => c.cardID).ToList();
    }
    public List<CardData> GetListByName(string faction)
    {
        return library[faction].OrderBy(c => c.cardName).ToList();
    }
    public List<CardData> GetListByMana(string faction)
    {
        return library[faction].OrderBy(c => c.manaCost).ToList();
    }
    /*
    public CardData GetCardData(int id)
    {
        return cardList[id];
    }
    */
}

