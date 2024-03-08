using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameLogic;

[CreateAssetMenu()]
public class ComputerAccountSO : ScriptableObject {

    public string username;
    public string password;

    public Email[] emails;

    [System.Serializable]
    public class Email {
        public string topic;
        public string from;
        public string to;
        public string date;
        public GameLogic.GameStep executeStep;
        [TextArea] public string body;
    }
}
