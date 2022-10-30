using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Linq;

public sealed class SongManager : MonoBehaviour
{
    private ChartData chartData;
    private float currentTime;

    private Input input;

    public GameObject notePrefab;
    public GameObject eyeButtonPrefab;

    private float speed;

    private float startTime;

    private SpriteRenderer[] eyeButtons;

    private InputAction[] keyActions;
    private InputActionPhase[] prevKeyActionPhase;

    [Space]
    public Sprite noteSpriteNormal;
    public Sprite noteSpriteHold;
    public Sprite noteSpriteHoldEnd;
    public Sprite noteSpriteDamage;
    public Sprite noteSpritePoison;
    public Sprite noteSpriteDeath;

    [Space]
    public Sprite eyeSpriteOpen;
    public Sprite eyeSpriteClosed;

    private int currentRequiredNote = 0;
    private Stack<Note> noteStack;
    private List<Note> notes;

    private Camera cam;
    private Transform camTransform;

    void Start()
    {
        cam = Camera.main;
        camTransform = cam.transform;

        Application.targetFrameRate = 120;

        LoadChartData();

        input = GameManager.Input;

        input._5K.Enable();
        keyActions = new InputAction[5];
        prevKeyActionPhase = new InputActionPhase[5];

        keyActions[0] = input._5K.Key0;
        keyActions[1] = input._5K.Key1;
        keyActions[2] = input._5K.Key2;
        keyActions[3] = input._5K.Key3;
        keyActions[4] = input._5K.Key4;

        CreateChartElements();

        startTime = Time.time + 3;
    }

    void LoadChartData()
    {
        ChartNote[] tempNotes = new ChartNote[short.MaxValue * 8];
        for(int i = 0; i < tempNotes.Length; i++)
        {
            tempNotes[i] = new()
            {
                time = i / 20f,
                type = NoteType.Normal,
                row = UnityEngine.Random.Range(0, 5),
            };
        }

        chartData = new()
        {
            name = "Test Chart",
            chartAuthor = "Sans",
            musicAuthor = "Papyrus",
            description = "No description given",
            bpm = 60,
            difficulty = 2,
            speed = 15,
            keyCount = 5,
            notes = tempNotes,
            preferedStackSize = 50,
        };

        speed = chartData.speed;
    }

    void CreateChartElements()
    {
        if (chartData.notes.Length == 0)
            return;

        eyeButtons = new SpriteRenderer[chartData.keyCount];
        foreach(int i in chartData.keyCount - 1)
        {
            eyeButtons[i] = Instantiate(eyeButtonPrefab, new Vector2(i * 1.5f - chartData.keyCount / 2 - 1, 0), Quaternion.identity).GetComponent<SpriteRenderer>();
        }

        noteStack = new(chartData.preferedStackSize);
        notes = new(chartData.preferedStackSize);
        for(int i = 0; i < chartData.preferedStackSize; i++)
        {
            GameObject currentNoteTemp = Instantiate(notePrefab);
            currentNoteTemp.SetActive(false);
            Note noteTemp = currentNoteTemp.GetComponent<Note>();
            noteStack.Push(noteTemp);
        }
    }

    void Update()
    {
        currentTime = Time.time - startTime;

        // C D
        // A B

        float aspect = cam.aspect;
        float orthographicSize = cam.orthographicSize;
        Vector2 cameraPosition = camTransform.position;
        Vector2 right = camTransform.right;
        Vector2 up = camTransform.up;

        Vector2 a = aspect * orthographicSize * -right - up * orthographicSize + cameraPosition;
        Vector2 b = aspect * orthographicSize *  right - up * orthographicSize + cameraPosition;
        Vector2 c = aspect * orthographicSize * -right + up * orthographicSize + cameraPosition;
        Vector2 d = aspect * orthographicSize *  right + up * orthographicSize + cameraPosition;

        Bounds cameraBounds = new();
        cameraBounds.Encapsulate(a);
        cameraBounds.Encapsulate(b);
        cameraBounds.Encapsulate(c);
        cameraBounds.Encapsulate(d);

        float topBorder = Mathf.Max(10, cameraBounds.max.y + 1);
        while(true)
        {
            ChartNote curChartNote = chartData.notes[currentRequiredNote];
            if((curChartNote.time - currentTime) * speed <= topBorder)
            {
                currentRequiredNote++;
                Note newNote = GetNewNoteFromStack(curChartNote.row);
                newNote.Initialise(this, curChartNote);
                notes.Add(newNote);
            }
            else
            {
                break;
            }
        }

        float bottomBorder = Mathf.Min(-2, cameraBounds.min.y - 1);
        for(int i = 0; i < notes.Count; i++)
        {
            Note note = notes[i];
            note.Poll(currentTime, speed);
            if(note.transform.position.y < bottomBorder)
            {
                PushNoteOnStack(note);
                notes.RemoveAt(i);
                i--;
            }
        }

        foreach (int i in keyActions.Length - 1)
        {
            InputActionPhase phase = keyActions[i].phase;

            if(phase == InputActionPhase.Performed)
            {
                eyeButtons[i].sprite = eyeSpriteClosed;

                bool hit = prevKeyActionPhase[i] != InputActionPhase.Performed;

                for (int j = 0; j < notes.Count; j++)
                {
                    Note note = notes[j];

                    if (note.data.row != i)
                        continue;

                    bool isHoldNote = note.data.type is NoteType.Hold or NoteType.HoldEnd;

                    float noteDelta = Mathf.Abs(note.data.time - currentTime);

                    NoteRating rating = NoteRatings.GetRating(noteDelta);
                    if (rating == NoteRating.Missed)
                        continue;

                    if (isHoldNote)
                        rating = NoteRating.Perfect;

                    if (hit ^ isHoldNote)
                    {
                        // Debug.Log(rating);
                        PushNoteOnStack(note);
                        notes.RemoveAt(j);
                        break;
                    }
                }
            }
            else
                eyeButtons[i].sprite = eyeSpriteOpen;

            prevKeyActionPhase[i] = phase;
        }
    }

    Note GetNewNoteFromStack(int rowParent)
    {
        if(noteStack.TryPop(out Note poppedNote))
        {
            poppedNote.transform.parent = eyeButtons[rowParent].transform;
            poppedNote.gameObject.SetActive(true);
            return poppedNote;
        }
        Debug.LogWarning("Hold on, a new note needed to be created!");
        GameObject newNote = Instantiate(notePrefab, eyeButtons[rowParent].transform);
        return newNote.GetComponent<Note>();
    }

    void PushNoteOnStack(Note note)
    {
        note.gameObject.SetActive(false);
        noteStack.Push(note);
    }
}
