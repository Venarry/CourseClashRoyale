using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _spawnPrice;

    public void SetMainImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public void SetSpawnPrice(int price)
    {
        _spawnPrice.text = price.ToString();
    }
}
