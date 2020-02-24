using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardLibrary: MonoBehaviour
{
    [SerializeField] private List<Material> cardMaterialLibrary;
    private List<CardData> cardList;
    // Start is called before the first frame update
    void Awake()
    {
        cardList = new List<CardData>();
        //CardID, Name, manaCost, faction, traits, material
        //0 Unassigned, 1 Fantasy, 2 Steampunk, 3 Horror, 4 Sci-Fi
        cardList.Add(new CardData(0,"Default", 0, 0, "None", cardMaterialLibrary[0]));
        cardList.Add(new CardData(1, "Blood Boil", 2, 3, "Speed+", cardMaterialLibrary[1]));
        cardList.Add(new CardData(2, "Fire Bolt", 3, 1, "Fire", cardMaterialLibrary[2]));
        cardList.Add(new CardData(3, "Purple Shot", 4, 4, "", cardMaterialLibrary[3]));
        cardList.Add(new CardData(4, "Red Coin", 5, 2, "", cardMaterialLibrary[4]));

        Debug.Log(cardList[0].getID() + "," + cardList[1].getID() + "," + cardList[2].getID() + "," + cardList[3].getID() + "," + cardList[4].getID());
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */
    public List<CardData> GetListByID(int faction)
    {
        /*cardList = cardList.OrderBy(c => c.cardID).ToList(); 
        return cardList;*/
        return cardList.OrderBy(c => c.cardID).ToList();
    }
    public List<CardData> GetListByName(int faction)
    {
        /*cardList = cardList.OrderBy(c => c.cardName).ToList();
        //Debug.Log(cardList.Count);
        return cardList;*/
        return cardList.OrderBy(c => c.cardName).ToList();
    }
    public List<CardData> GetListByMana(int faction)
    {
        /*cardList = cardList.OrderBy(c => c.manaCost).ToList();
        return cardList;*/
        return cardList.OrderBy(c => c.manaCost).ToList();
    }

    public CardData GetCardData(int id)
    {
        return cardList[id];
    }
}
