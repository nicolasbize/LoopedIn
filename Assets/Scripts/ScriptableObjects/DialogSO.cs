using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DialogSO : ScriptableObject
{
    public string greeting;
    public Branch[] branches;

    [System.Serializable]
    public class Branch {
        public string question;
        public string response;
        public Branch[] branches;
    }

}
