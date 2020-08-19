using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MidiPlayer : MonoBehaviour
{

    public string filename;

    public Sampler sampler;
    public KeyboardSpawner keyboard;

    private List<NoteInfo> notes;
    private int onIndex = 0;
    private float startTime;

    private Key[] keys;

    // Start is called before the first frame update
    void Start()
    {
        MidiFile midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + filename + ".mid");

        TempoMap tempoMap = midiFile.GetTempoMap();

        Debug.Log("Read midi");

        // source: https://answers.unity.com/questions/1248104/parse-a-midi-file.html
        var programChanges = new Dictionary<FourBitNumber, Dictionary<long, SevenBitNumber>>();
        foreach (var timedEvent in midiFile.GetTimedEvents())
        {
            var programChangeEvent = timedEvent.Event as ProgramChangeEvent;
            if (programChangeEvent == null)
                continue;

            var channel = programChangeEvent.Channel;

            Dictionary<long, SevenBitNumber> changes;
            if (!programChanges.TryGetValue(channel, out changes))
                programChanges.Add(channel, changes = new Dictionary<long, SevenBitNumber>());

            changes[timedEvent.Time] = programChangeEvent.ProgramNumber;
        }

        notes = midiFile.GetNotes().Select(
            n => new NoteInfo
            {
                programChange = GetProgramNumber(n.Channel, n.Time, programChanges),
                time = n.TimeAs<MetricTimeSpan>(tempoMap).TotalMicroseconds / 1000000f,
                length = n.LengthAs<MetricTimeSpan>(tempoMap).TotalMicroseconds / 1000000f,
                velocity = n.Velocity / 127f,
                noteNumber = n.NoteNumber
            }).ToList();


        //remove percussion
        List<NoteInfo> temp = new List<NoteInfo>();

        foreach (NoteInfo ni in notes)
         {
            // Debug.Log(ni.time + " " + ni.length + " " + ni.noteNumber);
            if (ni.programChange >= 9 && ni.programChange <= 16)
                continue;

            temp.Add(ni);
         }

        notes = temp;

        startTime = Time.time;
    }
    private static int? GetProgramNumber(FourBitNumber channel, long time, Dictionary<FourBitNumber, Dictionary<long, SevenBitNumber>> programChanges)
    {
        Dictionary<long, SevenBitNumber> changes;
        if (!programChanges.TryGetValue(channel, out changes))
            return null;

        var times = changes.Keys.Where(t => t <= time).ToArray();
        return times.Any()
            ? (int?)changes[times.Max()]
            : null;
    }

    // Update is called once per frame
    void Update()
    {
        // init keys
        if(keys == null || keys.Length != keyboard.transform.childCount)
        {
            keys = keyboard.GetComponentsInChildren<Key>();
        }

        if (onIndex >= notes.Count)
            return;

        float t = Time.time - startTime;

        while(onIndex < notes.Count && notes[onIndex].time < t)
        {

            StartCoroutine(PlayNote(notes[onIndex].noteNumber - 24, notes[onIndex].length, notes[onIndex].velocity));

            onIndex++;

        }
    }

    IEnumerator PlayNote(int n, float s, float v)
    {

        // Debug.Log("Playing note: " + n + " for " + s + "seconds");

        sampler.StartNote(n, v);

        // search for key
        foreach(Key k in keys)
        {
            if (k.note != n)
                continue;

            k.SimulateNote(v);
            break;
        }

        yield return new WaitForSeconds(s);

        sampler.EndNote(n);
    }

    class NoteInfo
    {
        public int? programChange { get; set; }
        public float time { get; set;  }
        public float length { get; set; }
        public float velocity { get; set; }
        public int noteNumber { get; set; }
    }

    class NoteComparer : IComparer<NoteInfo>
    {
        public int Compare(NoteInfo x, NoteInfo y)
        {

            if (x.time < y.time)
                return -1;
            else if (y.time < x.time)
                return 1;
            else
                return 0;

        }
    }
}
