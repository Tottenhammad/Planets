using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitController : MonoBehaviour
{
    public List<Segment> Splittees = new List<Segment>();
    public List<Segment> NextLoop = new List<Segment>();

    Coroutine splitter; 
    private void Start()
    {
        splitter = StartCoroutine(SplitUp());

    }
    IEnumerator SplitUp()
    {
        while (true)
        {
            if (Splittees.Count > 0)
            {

                foreach (Segment s in Splittees)
                {
                    s.CallSplit();
                    yield return new WaitUntil(() => s.splitting == false);
                    s.QueueChildrenForSplit();
                   // yield return new WaitForSecondsRealtime(0.1f);
                }

            }
            Splittees = new List<Segment>();
            foreach (Segment s in NextLoop)
            {
                Splittees.Add(s);
            }
            NextLoop = new List<Segment>();
            yield return new WaitForSecondsRealtime(0.00001f);
        }

    }

    public void RemoveAllOf(QuadTreePlanet planet)
    {
        StopCoroutine(splitter);
        foreach (Segment s in Splittees)
        {
            if (s.planet == planet)
                Splittees.Remove(s);
        }
        foreach (Segment s in NextLoop)
        {
            if (s.planet == planet)
                Splittees.Remove(s);
        }
        splitter = StartCoroutine(SplitUp());
    }
}
