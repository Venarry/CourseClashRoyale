using UnityEngine;
using UnityEngine.EventSystems;

public class GameCardHandler : MonoBehaviour, IPointerClickHandler
{
    private Card _card;
    private GameDeckView _gameDeckView;

    public void Init(Card card, GameDeckView gameDeckView)
    {
        _card = card;
        _gameDeckView = gameDeckView;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _gameDeckView.SetActiveCard(_card);
    }
}
