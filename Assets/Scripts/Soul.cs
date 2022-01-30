using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Soul : MonoBehaviour
{
    public enum Soultype
    {
        Holy,
        Sinful,
        Mundane
    }

    public enum SinType
    {
        None,
        Lust,
        Gluttony,
        Greed,
        Sloth,
        Wrath,
        Envy,
        Pride
    }

    [SerializeField]
    Soultype soul;

    [SerializeField]
    SinType sin;

    public MeshRenderer mesh;

    int patience;

    int randomNumber;

    int randomAmountofSins;

    public Slider patienceSlider;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        patience = 100;
        patienceSlider.value = patience;
        mesh = GetComponent<MeshRenderer>();

        randomNumber = Random.Range(0,7);

        randomAmountofSins = Random.Range(1, 10);

        sin = (SinType)randomNumber;

        //Determines if the soul is sinful or not
        switch (sin)
        {
            case SinType.None:
                mesh.material.color = Color.blue;
                soul = Soultype.Holy;
                randomAmountofSins = 0;
                break;
            case SinType.Lust:
                mesh.material.color = Color.red;
                soul = Soultype.Sinful;
                break;
            case SinType.Gluttony:
                mesh.material.color = Color.red;
                soul = Soultype.Sinful;
                break;
            case SinType.Greed:
                mesh.material.color = Color.red;
                soul = Soultype.Sinful;
                break;
            case SinType.Sloth:
                mesh.material.color = Color.red;
                soul = Soultype.Sinful;
                break;
            case SinType.Wrath:
                mesh.material.color = Color.red;
                soul = Soultype.Sinful;
                break;
            case SinType.Envy:
                mesh.material.color = Color.red;
                soul = Soultype.Sinful;
                break;
            case SinType.Pride:
                mesh.material.color = Color.red;
                soul = Soultype.Sinful;
                break;
            default:
                soul = Soultype.Mundane;
                break;
        }

        Debug.Log("My sin is: " + sin + "\n I have commited the sin this many times: " + randomAmountofSins);
        Debug.Log("So my soul is " + soul);
        text.text = randomAmountofSins.ToString();
        StartCoroutine(losePatience());
    }

    IEnumerator losePatience()
    {
        while (patience >=0)
        {
            yield return new WaitForSeconds(2f);
            patience--;
            patienceSlider.value = patience;
            //Debug.Log("My current patience is: " + patience);
        }
    }
}
