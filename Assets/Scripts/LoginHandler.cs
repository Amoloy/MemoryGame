using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginHandler : MonoBehaviour
{
    public TextMeshProUGUI Input;
    public UserController UserController;

    public void Authorise()
    {
        UserController.Authorise(Input.text);
    }
}
