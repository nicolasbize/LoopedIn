using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILibrarianNote : MonoBehaviour
{

    public Transform noteTransform;
    [TextArea] public string thought;

    void Start()
    {
        Player.Instance.OnStateChange += Player_OnStateChange;
        noteTransform.gameObject.SetActive(false);
        Player.Instance.OnLibrarianNotePickup += Player_OnLibrarianNotePickup;
    }

    private void Player_OnLibrarianNotePickup(object sender, System.EventArgs e) {
        noteTransform.gameObject.SetActive(true);
        Player.Instance.StartThinking(thought, Player.State.Moving);
    }

    private void Player_OnStateChange(object sender, System.EventArgs e) {
        // we get here after thinking out loud, but without having closed the clue
        if (Player.Instance.GetState() == Player.State.Moving) {
            noteTransform.gameObject.SetActive(false);
        }
    }

}
