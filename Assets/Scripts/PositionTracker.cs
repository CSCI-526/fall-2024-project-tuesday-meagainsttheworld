using UnityEngine;
using UnityEngine.SceneManagement;

public class PositionTracker : MonoBehaviour
{
    private readonly string waitTimeURL = "https://docs.google.com/forms/d/e/1FAIpQLSfxrUCyNT7e4dg9pYvrvIYtnsN4sZ2AjTkCSb_bqYKjv4Vlxg/formResponse";

    private struct PlayerTracker
    {
        public PlayerTracker(GameObject inputPlayer)
        {
            transform = inputPlayer.transform;
            name = inputPlayer.name;
            lastPosition = Vector3.one * 100;
            waitTime = 0;
        }

        public Transform transform;
        public string name;
        public Vector3 lastPosition;
        public float waitTime;
    }

    private PlayerTracker player1Tracker;
    private PlayerTracker player2Tracker;
    private readonly float waitLimit = 5;

    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players[0].name == "Player1")
        {
            player1Tracker = new PlayerTracker(players[0]);
            player2Tracker = new PlayerTracker(players[1]);
        }
        else
        {
            player1Tracker = new PlayerTracker(players[1]);
            player2Tracker = new PlayerTracker(players[0]);
        }
    }

    void Update()
    {
        // Check Player 1's position
        if (player1Tracker.transform.position == player1Tracker.lastPosition) player1Tracker.waitTime += Time.deltaTime;
        else
        {
            if (player1Tracker.waitTime > waitLimit) TrackWaitTime(player1Tracker);
            player1Tracker.waitTime = 0;
            player1Tracker.lastPosition = player1Tracker.transform.position;
        }

        // Check Player 2's position
        if (player2Tracker.transform.position == player2Tracker.lastPosition) player2Tracker.waitTime += Time.deltaTime;
        else
        {
            if (player2Tracker.waitTime > waitLimit) TrackWaitTime(player2Tracker);
            player2Tracker.waitTime = 0;
            player2Tracker.lastPosition = player2Tracker.transform.position;
        }
    }

    void OnDestroy()
    {
        if (player1Tracker.waitTime > waitLimit) TrackWaitTime(player1Tracker);
        if (player2Tracker.waitTime > waitLimit) TrackWaitTime(player2Tracker);
    }

    private void TrackWaitTime(PlayerTracker inputTracker)
    {
        Debug.Log(inputTracker.name + " waited at position " + inputTracker.lastPosition + " for " + inputTracker.waitTime + " seconds");

        WWWForm form = new();

        // Session ID
        form.AddField("entry.375346932", DataCollection.sessionID);
        // Current build name
        form.AddField("entry.684969437", DataCollection.buildNo);
        // Level
        form.AddField("entry.908662422", SceneManager.GetActiveScene().name);
        // Player ID
        form.AddField("entry.647556604", inputTracker.name);
        // Player X coordinate at idling location
        form.AddField("entry.435886054", inputTracker.lastPosition.x.ToString());
        // Player Y coordinate at idling location
        form.AddField("entry.1778632727", inputTracker.lastPosition.y.ToString());
        // Player idle wait time
        form.AddField("entry.2121814057", inputTracker.waitTime.ToString());

        DataCollection.Post(waitTimeURL, form);
    }
}