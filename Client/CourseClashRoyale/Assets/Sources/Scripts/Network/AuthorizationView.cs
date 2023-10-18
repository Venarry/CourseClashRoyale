using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AuthorizationView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _login;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private Button _authorizationButton;
    [SerializeField] private UserInfo _userInfo;

    private UserDataProvider _userDataProvider;

    public void Init(UserDataProvider userDataProvider)
    {
        _userDataProvider = userDataProvider;
    }

    private void OnEnable()
    {
        _authorizationButton.onClick.AddListener(TryAuthorize);
    }

    private void OnDisable()
    {
        _authorizationButton.onClick.RemoveListener(TryAuthorize);
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

    private void OnAuthorizationRequestSuccess(string callback)
    {
        if(int.TryParse(callback, out int id))
        {
            Dictionary<string, string> data = new()
            {
                { "tableName", "clashroyaledata" },
                { "id", id.ToString() },
            };

            WWWConnection.Post(
                URLProvider.DataProviderFile,
                data,
                OnGetDataRequestSuccess);

            _userInfo.Set(id);
        }
        else
        {
            Debug.Log("������ �����������");
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

        if (string.IsNullOrEmpty(data))
        {
            return;
        }

        UserData userData = JsonUtility.FromJson<UserData>(data);

        _userDataProvider.SetUserData(userData);
    }
}
