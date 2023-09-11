using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject _parent;
    [SerializeField] private Image _background;
    [SerializeField] private Image _progress;

    private void Awake()
    {
        SetVisiable(true);
    }

    public void SetValue(float progress, bool canHide = false)
    {
        progress = Mathf.Clamp(progress, 0, 1);

        if (canHide == true)
        {
            RefreshVisibleState(progress);
        }

        _progress.fillAmount = progress;
    }

    public void SetCanVisibleState(bool canHide)
    {
        if (canHide == true)
        {
            RefreshVisibleState(_progress.fillAmount);
        }
        else
        {
            SetVisiable(true);
        }
    }

    private void RefreshVisibleState(float value)
    {
        if (value >= 1)
            SetVisiable(false);
        else
            SetVisiable(true);
    }

    public void SetColor(Color color)
    {
        _progress.color = color;
    }

    private void SetVisiable(bool state)
    {
        //_progress.enabled = state;
        //_background.enabled = state;
        _parent.SetActive(state);
    }
}
