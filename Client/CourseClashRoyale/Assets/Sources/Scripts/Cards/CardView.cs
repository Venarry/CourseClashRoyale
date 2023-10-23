using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _spawnPrice;

    public int Price { get; private set; }
    public int Level { get; private set; }

    public void SetMainImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public void SetSpawnPrice(int price)
    {
        _spawnPrice.text = price.ToString();
        Price = price;
    }

    public void SetLevel(int level)
    {
        Level = level;
    }
}
