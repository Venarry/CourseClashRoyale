using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegistrationView : MonoBehaviour
{
    private const int MinLoginLength = 3;

    [SerializeField] private TMP_InputField _login;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private TMP_InputField _conirmedPassword;
    [SerializeField] private Button _registrationButton;

    private void OnEnable()
    {
        _registrationButton.onClick.AddListener(TryRegistration);
    }

    private void OnDisable()
    {
        _registrationButton.onClick.RemoveListener(TryRegistration);
    }

    private void TryRegistration()
    {
        string login = _login.text;
        string password = _password.text;
        string confirmedPassword = _conirmedPassword.text;

        if (login.Length < MinLoginLength)
        {
            Debug.Log($"����� ������ ���� ������ {MinLoginLength} ��������");
            return;
        }

        if (password != confirmedPassword)
        {
            Debug.Log($"������ �� ���������");
            return;
        }

        Dictionary<string, string> data = new()
        {
            { "login", login },
            { "password", password },
        };

        WWWConnection
            .Post(URLProvider.RegistrationFile, data, OnRegistrationRequestSuccess);
    }

    private void OnRegistrationRequestSuccess(string callback)
    {
        if (string.IsNullOrEmpty(callback))
        {
            Debug.Log("����������� ������ �������. ������ �������������");
        }
        else
        {
            Debug.Log(callback);
        }
    }
}
