using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public ChartNote data;
    private SongManager songManager;

    public void Initialise(SongManager songManager, ChartNote noteData)
    {
        data = noteData;
        this.songManager = songManager;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.sprite = data.type switch
        {
            NoteType.Hold => songManager.noteSpriteHold,
            NoteType.HoldEnd => songManager.noteSpriteHoldEnd,
            NoteType.Damage => songManager.noteSpriteDamage,
            NoteType.Poison => songManager.noteSpritePoison,
            NoteType.Death => songManager.noteSpriteDeath,
            _ => songManager.noteSpriteNormal,
        };
    }

    public void Poll(float currentTime, float speed)
    {
        float deltaTime = data.time - currentTime;
        transform.localPosition = new Vector2(0, deltaTime * speed);
        transform.localRotation = Quaternion.identity;
    }
}
