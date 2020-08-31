using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;

public class Sampler : MonoBehaviour
{

    public string sampleName = "piano";
    public bool prefix = true;

    public GameObject notePrefab;

    public int maxSimultaneousNotes = 13;

    public LanternLauncher launcher;

    private List<Note> notes;

    private float interval = 0.5f, acc;
    private int current = 33;

    private float semiFactor;
    private float volume = 1, fadeStep = 0.05f, fadeInterval = 0.01f;

    private static string[] noteNames = {
        "C",
        "Db",
        "D",
        "Eb",
        "E",
        "F",
        "Gb",
        "G",
        "Ab",
        "A",
        "Bb",
        "B",
    };

    private List<NoteDestroy> playingNotes;

    private bool sustain = false;



    private static Regex noteParser= new Regex(@"(?<letters>[a-zA-Z]+)(?<numbers>[0-9]+)", RegexOptions.Compiled);


    // Start is called before the first frame update
    void Start()
    {
        acc = interval;

        playingNotes = new List<NoteDestroy>();

        LoadSamples();

        semiFactor = (float)Math.Pow(2, 1 / 12f);
    }

    public void SetInstrument(string s)
    {
        prefix = true;
    }

    public void SetInstrument(string s, bool p)
    {
        sampleName = s;
        prefix = p;
        LoadSamples();
    }

    private void LoadSamples()
    {

        notes = new List<Note>();

        // load all available notes
        for (int i = 0; i < 12 * 8; i++)
        {
            string path = "Audio/" + sampleName + "/" + GetSampleName(i);

            AudioClip temp = Resources.Load<AudioClip>(path);

            if (temp == null)
                continue;

            Debug.Log("Loaded: " + path);

            notes.Add(new Note(i, temp));
        }

    }

    private string GetSampleName(int n)
    {
        string result = prefix ? sampleName + "_" : "";

        return result + NumberToNote(n);
    }

    public static int NoteToNumber(string note)
    {
        // Find matches.
        MatchCollection matches = noteParser.Matches(note);

        foreach (Match match in matches)
        {
            GroupCollection groups = match.Groups;

            // find number in scal
            int a = 0;
            for(; a<noteNames.Length; a++)
            {
                if (groups["letters"].Value.ToLower().Equals(noteNames[a].ToLower()))
                    break;
            }

            if (a == noteNames.Length)
            {
                Debug.LogWarning("Unparseable note passed.");
                return 0;
            }

            return (int.Parse(groups["numbers"].Value) - 1) * 12 + a;
        }
        
        Debug.LogWarning("Unparseable note passed.");
        
        return 0;
    }

    public static string NumberToNote(int n)
    {
        // n going from 0 = c1
        return noteNames[n % noteNames.Length] + (n / noteNames.Length + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (acc >= interval)
        {
            acc -= interval;

            //PlayNote(current);

            current++;
        }

        if (Input.GetKeyDown("up"))
            //PlayNote(44);
             PlayNote(UnityEngine.Random.Range(20, 70));

        acc += Time.deltaTime;
    }

    public void PlayNote(int n)
    {
        StartNote(n);

        StartCoroutine(WaitForNote(n, 1));
    }

    IEnumerator WaitForNote(int n, float s)
    {
        yield return new WaitForSeconds(s);

        EndNote(n);
    }

    public void StartNote(int n)
    {
        StartNote(n, 1);
    }

    public void StartNote(int n, float velocity)
    {
        // find closest note
        Note closest = null;
        int distance = int.MaxValue,
            sign = 0;

        foreach (Note note in notes)
        {
            int s = n - note.note,
                d = Math.Abs(s);

            s = Math.Sign(s);

            if (d < distance)
            {
                distance = d;
                sign = s;
                closest = note;
            }
        }

        if (closest == null)
        {
            Debug.LogError("No closest note found!");
            return;
        }

        // instantiate note
        GameObject go = Instantiate(notePrefab, transform);
        NoteDestroy nd = go.GetComponent<NoteDestroy>();
        
        nd.startTime = Time.time;
        nd.note = n;

        AudioSource source = go.GetComponent<AudioSource>();

        source.clip = closest.clip;

        float factor = sign < 0 ? 1 / semiFactor : semiFactor;

        factor = (float)Math.Pow(factor, distance);

        source.pitch = factor;
        source.volume = volume * velocity;

        source.Play();

        Debug.Log("Playing note " + n + " with pitch " + factor + " and velocity " + velocity);

        if(!sustain)
            playingNotes.Add(nd);

        launcher.Launch(n);

        // if over limit, find oldest child and remove
        for (int i = 0; transform.childCount - i > maxSimultaneousNotes; i++)
        {
            // assume chronological order for now
            /*GameObject oldest = null;
            float oldestTime = float.MaxValue;

            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject temp = transform.GetChild(i).gameObject;

                if (temp.GetComponent<NoteDestroy>().startTime < oldestTime)
                {
                    oldest = temp;
                    oldestTime = temp.GetComponent<NoteDestroy>().startTime;
                }
            }

            Debug.Log("Too many notes " + oldest);

            // destroy oldest
            Destroy(oldest);*/

            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void EndNote(int n)
    {
        // find note to end
        for (int i = 0; i < playingNotes.Count; i++) {
            if (playingNotes[i] == null)
            {
                playingNotes.RemoveAt(i);
                i--;
                continue;
            }

            if (playingNotes[i].note != n)
                continue;

            StartCoroutine(FadeNote(playingNotes[i]));

            playingNotes.RemoveAt(i);
            break;
        }
    }

    public void SetSustain(bool s)
    {
        sustain = s;
    }

    private IEnumerator FadeNote(NoteDestroy nd)
    {
        AudioSource source = nd.gameObject.GetComponent<AudioSource>();

        while (source != null && source.volume > 0)
        {
            source.volume -= fadeStep;
            yield return new WaitForSeconds(fadeInterval);
        }

        if (nd != null)
        {
            Destroy(nd.gameObject);
        }
    }

    class Note
    {
        public int note;
        public AudioClip clip;

        public Note(int note, AudioClip clip)
        {
            this.note = note;
            this.clip = clip;
        }
    }
}
