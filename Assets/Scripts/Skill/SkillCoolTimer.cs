using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimer : MonoBehaviour
{
    private float time;
    private int remain = 0;
    private int interval = 0;
    private float elapsedTime = 0;
    private float intervalElapsed = 0;

    private bool updata = false;

    private Animation legacyAni;
    private Text count;

    public void Init()
    {
        legacyAni = GetComponent<Animation>();
        count = GetComponent<Text>();
        transform.localScale = Vector3.zero;
    }

    public void Execute(float targetTime, int interval = 1)
    {
        time = targetTime;
        remain = (int)targetTime;
        this.interval = interval;
        count.text = remain.ToString();
        count.transform.localScale = Vector3.zero;
        count.gameObject.SetActive(true);
        elapsedTime = 0;
        intervalElapsed = 0;
        updata = true;
    }

    private void Update()
    {
        if (updata == false)
            return;
        intervalElapsed += Time.deltaTime;
        intervalElapsed = Mathf.Clamp(intervalElapsed, 0, interval);

        if(intervalElapsed >= interval)
        {
            legacyAni.Play();
            intervalElapsed = 0;
            remain -= interval;
            count.text = remain.ToString();
        }
        elapsedTime += Time.deltaTime / time;
        if(elapsedTime >= 1.0f)
        {
            updata = false;
            elapsedTime = 0;
            count.gameObject.SetActive(false);
        }
    }
}
