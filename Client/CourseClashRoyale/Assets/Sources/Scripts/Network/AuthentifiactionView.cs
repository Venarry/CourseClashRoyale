using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AuthentifiactionView : MonoBehaviour
{
    private const int MinLoginLength = 3;

    [SerializeField] private TMP_InputField _login;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private TMP_InputField _conirmedPassword;
    [SerializeField] private Button _authorizationButton;
    [SerializeField] private Button _registrationButton;

    private void OnEnable()
    {
        _authorizationButton.onClick.AddListener(TryAuthorize);
        _registrationButton.onClick.AddListener(TryRegistration);
    }

    private void OnDisable()
    {
        _authorizationButton.onClick.RemoveListener(TryAuthorize);
        _registrationButton.onClick.RemoveListener(TryRegistration);
    }

    private void TryAuthorize()
    {
        string login = _login.text;
        string password = _password.text;

        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        {
            return;
        }

        Dictionary<string, string> data = new()
        {
            { "login", login },
            { "password", password },
        };

        WWWConnection
            .Post(URLProvider.AuthificationFile, data, OnAuthorizationRequestSuccess, OnAuthorizationRequestError);
    }

    private void TryRegistration()
    {
        string login = _login.text;
        string password = _password.text;
        string confirmedPassword = _conirmedPassword.text;

        if(login.Length < MinLoginLength)
        {
            Debug.Log($"Логин должен быть больше {MinLoginLength} символов");
            return;
        }

        if(password != confirmedPassword)
        {
            Debug.Log($"Пароли не совпадают");
            return;
        }

        Dictionary<string, string> data = new()
        {
            { "login", login },
            { "password", password },
        };

        WWWConnection
            .Post(URLProvider.RegistrationFile, data, OnRegistrationRequestSuccess, OnAuthorizationRequestError);
    }

    private void OnRegistrationRequestSuccess(string callback)
    {
        if (string.IsNullOrEmpty(callback))
        {
            Debug.Log("Регистрация прошла успешно. Теперь авторизуйтесь");
        }
        else
        {
            Debug.Log(callback);
        }
    }

    private void OnAuthorizationRequestSuccess(string callback)
    {
        if(int.TryParse(callback, out int id))
        {
            Debug.Log(id);

            Dictionary<string, string> data = new()
            {
                { "game", "clash_royale_data" },
                { "id", id.ToString() },
            };

            WWWConnection.Post(
                URLProvider.DataProviderFile,
                data,
                OnGetDataRequestSuccess);
        }
        else
        {
            Debug.Log("Ошибка авторизации");
            return;
        }
    }

    private void OnAuthorizationRequestError(string error)
    {
        Debug.Log(error);
    }

    private void OnGetDataRequestSuccess(string data)
    {
        Debug.Log(data);
    }
}
