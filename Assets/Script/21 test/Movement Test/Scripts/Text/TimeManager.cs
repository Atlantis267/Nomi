using UnityEngine;

public class TimeManager : MonoBehaviour {

	public float slowdownFactor = 0.05f;
	private float starttime;
	private float startfix;

    private void Start()
    {
		starttime = Time.timeScale;
		startfix = Time.fixedDeltaTime;
    }
    private void Update()
    {
		Time.timeScale = starttime;
		Time.fixedDeltaTime = startfix;
    }

    public void DoSlowmotion ()
	{
		Time.timeScale = slowdownFactor;
		Time.fixedDeltaTime = Time.timeScale * .02f;
	}

}
