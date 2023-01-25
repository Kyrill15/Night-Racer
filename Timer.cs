using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI checkpointText;
    [SerializeField] private TextMeshProUGUI checkpointText2;
    [SerializeField] private TextMeshProUGUI checkpointText3;

    [SerializeField] private TextMeshProUGUI totalLapTime;
    [SerializeField] private TextMeshProUGUI totalLapTimetwo;
    [SerializeField] private TextMeshProUGUI totalLapTimethree;

    [SerializeField] private GameObject lap1;
    [SerializeField] private GameObject checkpointObjectlap1;
    [SerializeField] private GameObject lapEndLap1;
    [SerializeField] private GameObject lap2;
    [SerializeField] private GameObject checkpointObjectlap2;
    [SerializeField] private GameObject lapEndLap2;
    [SerializeField] private GameObject lap3;
    [SerializeField] private GameObject checkpointObjectlap3;
    [SerializeField] private GameObject lapEndLap3;

    private bool isTriggered = false;

    private float waitTime = 5.0f;
    private float time;
    private float sixtySeconds;

    private void Start()
    {
        lapEndLap1.SetActive(false);
        lap2.SetActive(false);
        lapEndLap2.SetActive(false);
        checkpointObjectlap2.SetActive(false);
        lapEndLap2.SetActive(false);
        lap3.SetActive(false);
        checkpointObjectlap3.SetActive(false);
        lapEndLap3.SetActive(false);
    }

    private void Update()
    {
        if (isTriggered == true)
        {
            time += Time.deltaTime;
            timerText.text = sixtySeconds.ToString("Time : 0") + time.ToString(" : 0.00");
            if (time > waitTime)
            {
                time = time - waitTime;
                sixtySeconds++;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isTriggered = true;
        if (other.gameObject.tag == "Checkpoint")
        {
            lapEndLap1.SetActive(true);
            checkpointText.text = sixtySeconds.ToString("Checkpoint : 0") + time.ToString(" : 0.00");
            checkpointObjectlap1.SetActive(false);
            lap1.SetActive(false);
        }
        else if (other.gameObject.tag == "LapEnd")
        {
            lapEndLap1.SetActive(false);
            lap2.SetActive(true);
            totalLapTime.text = sixtySeconds.ToString("Lap 1 : 0") + time.ToString(" : 0.00");
            checkpointObjectlap2.SetActive(true);
        }
        else if (other.gameObject.tag == "Checkpoint2")
        {
            lapEndLap2.SetActive(false);
            checkpointText2.text = sixtySeconds.ToString("Checkpoint : 0") + time.ToString(" : 0.00");
            checkpointObjectlap2.SetActive(false);
            lap2.SetActive(false);
            lapEndLap2.SetActive(true);
        }
        else if (other.gameObject.tag == "LapEnd2")
        {
            lapEndLap2.SetActive(false);
            totalLapTimetwo.text = sixtySeconds.ToString("Lap 2 : 0") + time.ToString(" : 0.00");
            lap3.SetActive(true);
            checkpointObjectlap3.SetActive(true);
        }
        else if (other.gameObject.tag == "Checkpoint3")
        {
            checkpointText3.text = sixtySeconds.ToString("Checkpoint : 0") + time.ToString(" : 0.00");
            checkpointObjectlap3.SetActive(false);
            checkpointObjectlap3.SetActive(true);
            lapEndLap3.SetActive(true);
            lap3.SetActive(false);
        }
        else if (other.gameObject.tag == "LapEnd3")
        {
            lapEndLap3.SetActive(false);
            checkpointObjectlap3.SetActive(false);
            totalLapTimethree.text = sixtySeconds.ToString("Lap 3 : 0") + time.ToString(" : 0.00");
        }
    }
}
