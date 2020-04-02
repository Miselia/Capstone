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
        library["Default"].Add(new CardData(0, "Default", 0, 0, "None", "None", cardMaterialLibrary[0]));
        library["Horror"].Add(new CardData(1, "Blood Boil", 2, 3, "Speed+", "You just make daddy so angry", cardMaterialLibrary[1]));
        library["Fantasy"].Add(new CardData(2, "Fire Bolt", 3, 1, "", "3rd Level Spell", cardMaterialLibrary[2]));
        library["Sci-Fi"].Add(new CardData(3, "Plasma Bolt", 4, 4, "", "3rd Level Plasma", cardMaterialLibrary[3]));
        library["Steampunk"].Add(new CardData(4, "Red Coins", 5, 2, "", "Collect all 8 for a star!",cardMaterialLibrary[4]));
        library["Fantasy"].Add(new CardData(5, "Glimpse Into The Ether", 4, 1, "ManaRegen+","The Ether Glimpses back", cardMaterialLibrary[5]));
        library["Fantasy"].Add(new CardData(6, "Wandering Woodsprite", 4, 1, "Healing", "A helpful friend to all", cardMaterialLibrary[6]));
        library["Fantasy"].Add(new CardData(7, "Akashic Records", 7, 1, "Random Cards", "Randomness in all things", cardMaterialLibrary[7]));
        library["Fantasy"].Add(new CardData(8, "Well of Knowledge", 3, 1, "Next Heal/Damage doubled", "Knowledge is power", cardMaterialLibrary[8]));
        library["Fantasy"].Add(new CardData(9, "Magic Record", 5, 1, "Double cast next card", "I'll save this spell for later", cardMaterialLibrary[9]));
        library["Sci-Fi"].Add(new CardData(10, "Nano Barrier", 7, 4, "Create a barrier that can't be moved through", "No, you can't pass go", cardMaterialLibrary[10]));
        library["Sci-Fi"].Add(new CardData(11, "Gravity Well", 7, 4, "Well pulls projectiles in.", "Zucc", cardMaterialLibrary[11]));
        library["Sci-Fi"].Add(new CardData(12, "Electromagnetic Projectiles", 6, 4, "", "That's some pretty good ping", cardMaterialLibrary[12]));
        library["Sci-Fi"].Add(new CardData(13, "Overcharge", 4, 4, "Enhances speed of all projectiles", "Gotta go fast!", cardMaterialLibrary[13]));
        library["Steampunk"].Add(new CardData(14, "Filler", 4, 4, "0", "0", cardMaterialLibrary[14]));
        library["Steampunk"].Add(new CardData(15, "Filler", 4, 4, "0", "0", cardMaterialLibrary[15]));
        library["Steampunk"].Add(new CardData(16, "Filler", 4, 4, "0", "0", cardMaterialLibrary[16]));
        library["Steampunk"].Add(new CardData(17, "Filler", 4, 4, "0", "0", cardMaterialLibrary[17]));
        library["Sci-Fi"].Add(new CardData(18, "Chronosis", 6, 4, "Stops all projectiles currently on screen, then returns to the vector they were previously on.", "Stop, (In the name of love/Hammer time)", cardMaterialLibrary[18]));

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

