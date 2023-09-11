using System;
using UnityEngine;

[RequireComponent(typeof(ProgressBar))]
public class HealthView : MonoBehaviour
{
    [SerializeField] private LookAtRotator _lookAtRotator;
    [SerializeField] private ProgressBar _progressBar;
    private HealthPresenter _healthPresenter;
    private bool _isInitialized;
    private bool _healthBarCanHide;

    public event Action HealthOver;

    public void Init(HealthPresenter healthPresenter,
        Transform progressBarTarget,
        bool barCanHide,
        Color color)
    {
        gameObject.SetActive(false);

        _healthPresenter = healthPresenter;
        _lookAtRotator.SetTarget(progressBarTarget, isInvert: false);
        _healthBarCanHide = barCanHide;
        _progressBar.SetColor(color);
        OnHealthChange();
        _isInitialized = true;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _healthPresenter.Enable();
        _healthPresenter.HealthChanged += OnHealthChange;
        _healthPresenter.HealthOver += OnHealthOver;
    }

    private void OnDisable()
    {
        if (_isInitialized == false)
            return;

        _healthPresenter.Disable();
        _healthPresenter.HealthChanged -= OnHealthChange;
        _healthPresenter.HealthOver -= OnHealthOver;
    }

    public void SetHealthBarVisibleState(bool state)
    {
        _healthBarCanHide = state;
        OnHealthChange();
    }

    public void TakeDamage(int value)
    {
        _healthPresenter.TakeDamage(value);
    }

    private void OnHealthChange()
    {
        //Debug.Log(_healthPresenter.Health);
        _progressBar.SetValue(_healthPresenter.HealthNormalized, _healthBarCanHide);
    }

    private void OnHealthOver()
    {
        HealthOver?.Invoke();
        
    }
}
