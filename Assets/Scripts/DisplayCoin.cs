using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCoin : MonoBehaviour
{
    float timer;
    int value;

    [SerializeField] GameObject goalObject;

    Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 2)//1second keep,2seconds move
        {
            gameObject.transform.position = Vector3.Lerp(initialPosition, goalObject.transform.position, (timer - 2) / 1.5f);
        }
        if (timer > 3.5f)
        {
            GameManager.Instance.AddMoney(value);
            Destroy(gameObject);
        }
    }

    public void Occur(Vector3 basePosition, int value, GameObject goalObject)
    {
        this.goalObject = goalObject;
        this.value = value;
        float tmpRadian = Random.Range(0, Mathf.PI * 2);
        gameObject.transform.position = basePosition + new Vector3(Mathf.Cos(tmpRadian) * 50, Mathf.Sin(tmpRadian) * 50);
        initialPosition = gameObject.transform.position;
    }
}
