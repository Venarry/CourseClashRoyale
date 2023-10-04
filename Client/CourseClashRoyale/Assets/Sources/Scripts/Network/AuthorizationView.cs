using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AuthorizationView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _login;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private Button _authorizationButton;

    private void OnEnable()
    {
        _authorizationButton.onClick.AddListener(TryAuthorize);
    }

    private void OnDisable()
    {
        _authorizationButton.onClick.RemoveListener(TryAuthorize);
    }

    public void TryAuthorize()
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

        WWWConnection.Instance
            .Post(URLProvider.AuthificationFile, data, OnRequestSuccess, OnRequestError);
    }

    private void OnRequestSuccess(string callback)
    {
        Debug.Log(callback);


    }

    private void OnRequestError(string error)
    {
        Debug.Log(error);
    }
}
