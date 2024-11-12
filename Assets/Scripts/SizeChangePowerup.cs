using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SizeChangeType
{
    Growing,
    Shrinking
}

public class SizeChange : MonoBehaviour
{
    [SerializeField] private float effectDuration = 5f; // Public duration for how long the effect should last
    [SerializeField] private SizeChangeType type = SizeChangeType.Growing;
    [SerializeField] private bool regenerating = true;
    [SerializeField] private bool alternating = false;
    [SerializeField] private float alternatingInterval = 2.5f;
    private float alternatingTimer = 0;
    private readonly float sizeChangeValue = 2;
    private static PlayerStats defaultStats;
    private static PlayerStats bigStats;
    private static PlayerStats smallStats;
    private static Coroutine sizeChangeFunc;
    private static readonly Color regenColor = new(0, 0, 0, 0.5f);
    private readonly static string sizeChangingURL = "https://docs.google.com/forms/d/1URYzfOMw29EnC1lCHGKK_TI9W2LX6zDs24lghz9uj9k/formResponse";

    void Start()
    {
        if (!defaultStats) defaultStats = GameObject.FindWithTag("Player").GetComponent<PlayerController>().stats;
        if (!bigStats) bigStats = Resources.Load<PlayerStats>("Big Stats");
        if (!smallStats) smallStats = Resources.Load<PlayerStats>("Small Stats");

        if (gameObject.layer == 6)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if (gameObject.layer == 7)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
            transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.black;
        }

        ChangePowerupSprite();
    }

    void Update()
    {
        if (alternating)
        {
            if (alternatingTimer > alternatingInterval)
            {
                type = type == SizeChangeType.Growing ? SizeChangeType.Shrinking : SizeChangeType.Growing;
                ChangePowerupSprite();
                alternatingTimer = 0;
            }
            else alternatingTimer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Grow powerup logic
        if (other.CompareTag("Player"))
        {   
            PlayerController mainPlayer = other.GetComponent<PlayerController>();
            PlayerController otherPlayer = mainPlayer.OtherPlayer;
            SendSizeChangingPills(mainPlayer, otherPlayer, true);
            
            // End any previous powerup effect and begin current powerup
            if (sizeChangeFunc != null) StopCoroutine(sizeChangeFunc);
            sizeChangeFunc = StartCoroutine(RevertSizesAfterTime(mainPlayer, otherPlayer));
            // Ziang Qin's code addon
            StartCoroutine(ConsumePowerup());
        }
    }

    private IEnumerator RevertSizesAfterTime(PlayerController mainPlayer, PlayerController otherPlayer)
    {
        // Make effects from any potential previous powerups are undone
        mainPlayer.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        otherPlayer.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        SetToDefaultSize(mainPlayer, otherPlayer);

        ChangeSizes(mainPlayer, otherPlayer);

        float timeSpent = effectDuration > 2 ? effectDuration - 1 : effectDuration * 0.8f;

        yield return new WaitForSeconds(timeSpent);
        float timeLeft = effectDuration - timeSpent;

        float interval = timeLeft / 10;

        bool colorToggle = true;

        while (timeLeft > 0)
        {
            if (colorToggle)
            {
                mainPlayer.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
                otherPlayer.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                mainPlayer.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
                otherPlayer.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            }
            colorToggle = !colorToggle;
            timeLeft -= interval;
            yield return new WaitForSeconds(interval);
        }

        SendSizeChangingPills(mainPlayer, otherPlayer, false);
        // Revert both players to their original sizes and stats
        SetToDefaultSize(mainPlayer, otherPlayer);
    }

    private IEnumerator ConsumePowerup()
    {
        // If regenerating: disable powerup collider + make it transparent and then reset
        // If not regenerating: disable powerup collider + make it invisible and then destroy
        GetComponent<Collider2D>().enabled = false;

        if (regenerating)
        {
            GetComponent<SpriteRenderer>().color -= regenColor;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color -= regenColor;
            transform.GetChild(1).GetComponent<SpriteRenderer>().color -= regenColor;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        }

        yield return new WaitForSeconds(effectDuration + 0.05f);

        if (regenerating)
        {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().color += regenColor;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color += regenColor;
            transform.GetChild(1).GetComponent<SpriteRenderer>().color += regenColor;
        }
        else Destroy(gameObject);
    }

    private void ChangeSizes(PlayerController mainPlayer, PlayerController otherPlayer)
    {
        float sizeChange;
        float massChange;

        PlayerStats mainStats;
        PlayerStats otherStats;

        // Change player sizes and stats in opposite ways
        if (type == SizeChangeType.Growing)
        {
            sizeChange = sizeChangeValue;
            massChange = 100;

            mainStats = bigStats;
            otherStats = smallStats;
        }
        else
        {
            sizeChange = 1 / sizeChangeValue;
            massChange = 0.01f;

            mainStats = smallStats;
            otherStats = bigStats;
        }

        mainPlayer.gameObject.transform.localScale *= sizeChange;
        // mainPlayer.PlayerTrail.widthMultiplier *= sizeChange;
        mainPlayer.transform.GetChild(0).localScale *= sizeChange;
        mainPlayer.PlayerRb.mass *= massChange;
        mainPlayer.stats = mainStats;

        otherPlayer.gameObject.transform.localScale /= sizeChange;
        // otherPlayer.PlayerTrail.widthMultiplier /= sizeChange;
        otherPlayer.transform.GetChild(0).localScale /= sizeChange;
        otherPlayer.PlayerRb.mass /= massChange;
        otherPlayer.stats = otherStats;
    }

    private void SetToDefaultSize(PlayerController mainPlayer, PlayerController otherPlayer)
    {
        mainPlayer.gameObject.transform.localScale = Vector3.one;
        // mainPlayer.PlayerTrail.widthMultiplier = 1;
        mainPlayer.transform.GetChild(0).localScale = Vector3.one;
        mainPlayer.PlayerRb.mass = 1;
        mainPlayer.stats = defaultStats;

        otherPlayer.gameObject.transform.localScale = Vector3.one;
        // otherPlayer.PlayerTrail.widthMultiplier = 1;
        otherPlayer.transform.GetChild(0).localScale = Vector3.one;
        otherPlayer.PlayerRb.mass = 1;
        otherPlayer.stats = defaultStats;
    }

    void OnValidate()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if (this == null) return;
                if (gameObject.layer == 0)
                {
                    GetComponent<SpriteRenderer>().color = Color.gray;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
                    transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.black;
                }
                if (gameObject.layer == 6)
                {
                    GetComponent<SpriteRenderer>().color = Color.black;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
                    transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.white;
                }
                else if (gameObject.layer == 7)
                {
                    GetComponent<SpriteRenderer>().color = Color.white;
                    transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
                    transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.black;
                }
                ChangePowerupSprite();
            };
        #endif
    }

    private void ChangePowerupSprite()
    {
        if (type == SizeChangeType.Growing)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    // Ziang Qin's new code
    public void SendSizeChangingPills(PlayerController mainPlayer, PlayerController otherPlayer, bool isStart)
    {
        WWWForm form = new();

        // Session ID
        form.AddField("entry.1750140478", DataCollection.SessionID);
        // Current build name
        form.AddField("entry.1446046744", DataCollection.buildNo);
        // Level
        form.AddField("entry.2145687275", SceneManager.GetActiveScene().name);
        // isExpanding
        form.AddField("entry.687210033", transform.position.ToString());
        // isStart
        form.AddField("entry.1696011394", isStart.ToString());
        // Player X coordinate at gravity toggle location
        form.AddField("entry.1884234040", mainPlayer.transform.position.x.ToString());
        // Player Y coordinate at gravity toggle location
        form.AddField("entry.849009397", mainPlayer.transform.position.y.ToString());

        DataCollection.Post(sizeChangingURL, form);

        WWWForm OtherPlayerform = new();

        // Session ID
        OtherPlayerform.AddField("entry.1750140478", DataCollection.SessionID);
        // Current build name
        OtherPlayerform.AddField("entry.1446046744", DataCollection.buildNo);
        // Level
        OtherPlayerform.AddField("entry.2145687275", SceneManager.GetActiveScene().name);
        // isExpanding
        OtherPlayerform.AddField("entry.687210033", transform.position.ToString());
        // isStart
        OtherPlayerform.AddField("entry.1696011394", isStart.ToString());
        // Player X coordinate at gravity toggle location
        OtherPlayerform.AddField("entry.1884234040", otherPlayer.transform.position.x.ToString());
        // Player Y coordinate at gravity toggle location
        OtherPlayerform.AddField("entry.849009397", otherPlayer.transform.position.y.ToString());

        DataCollection.Post(sizeChangingURL, OtherPlayerform);
    }
}
