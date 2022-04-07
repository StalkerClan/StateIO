using UnityEngine;

public class InversePendulum : MonoBehaviour
{
    public Vector3 startRotation = new Vector3(0f, 0f, 150f);
    public Vector3 endRotation = new Vector3(0f, 0f, 30f);
    public float desiredDuration = 2f;
    public float elapsedTime;
    public float percent;
    private int mutiply;
    public bool stopped;


    private void Start()
    {
        transform.eulerAngles = startRotation;
    }

    private void Update()
    {
        PendulumEffect();
    }

    private void CheckMultiply()
    {
        if ((percent >= 0 && percent <= 0.2) ||
            (percent >= 0.8 && percent <= 1))
        {
            mutiply = 2;
        } 
        else if ((percent >= 0.2 && percent <= 0.4) ||
                (percent >= 0.6 && percent <= 0.8))
        {
            mutiply = 3;
        }
        else if (percent >= 0.4 && percent <= 0.6) 
        {
            mutiply = 4;
        }
    }

    public int GetMultiply()
    {
        stopped = true;
        CheckMultiply();
        return mutiply;
    }

    public void PendulumEffect()
    {
        if (stopped)
        {
            return;
        }

        percent = elapsedTime / desiredDuration;
        elapsedTime += Time.deltaTime;
        transform.eulerAngles = Vector3.Lerp(startRotation, endRotation, percent);

        if (percent >= 1)
        {
            percent = 0;
            elapsedTime = 0;
            Vector3 temp = startRotation;
            startRotation = endRotation;
            endRotation = temp;
        }
    }
}
