using Unity.Entities;

public class DeckBuilderHandAdjustEvent : IGenericEvent
{
    public Entity card;

    public DeckBuilderHandAdjustEvent(Entity card)
    {
        this.card = card;
    }
}
