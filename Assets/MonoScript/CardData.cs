using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData
{

    public int cardID;
    public string cardName;
    public float manaCost;
    private int faction; //0 Unassigned, 1 Fantasy, 2 Steampunk, 3 Horror, 4 Sci-Fi
    private string traits;
    private string flavor;
    private Material material;
    public CardData(int cardID, string cardName, float manaCost, int faction, string traits, string flavor, Material material)
    {
        this.cardID = cardID;
        this.cardName = cardName;
        this.manaCost = manaCost;
        this.faction = faction;
        this.traits = traits;
        this.flavor = flavor;
        this.material = material;
    }
    public int getID()
    {
        return cardID;
    }

    public string getName()
    {
        return cardName;
    }
    public float getCost()
    {
        return manaCost;
    }
    public int getFaction()
    {
        return faction;
    }
    public string getTraits()
    {
        return traits;
    }
    public string getFlavor()
    {
        return flavor;
    }
    public Material getMaterial()
    {
        return material;
    }


}
