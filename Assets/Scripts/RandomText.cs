using System.Collections;
using TMPro;
using UnityEngine;

public class RandomText : MonoBehaviour
{
    TextMeshProUGUI text;

    string[] phrases = new string[] {
        "It is all random ?",
        "Do anithing have a purpose ?",
        "Did I make it well ?"
    };

    string choosenPhrase;
    string showedText = "";

    [SerializeField] float delayValue = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        choosenPhrase = phrases[Random.Range(0, phrases.Length)];
        text = GetComponent<TextMeshProUGUI>();
        text.text = showedText;

        for (int i = 0; i < choosenPhrase.Length; i++)
        {
            StartCoroutine(UpdateText(i));
        }
    }

    IEnumerator UpdateText(int index){
        yield return new WaitForSeconds(delayValue * index);

        showedText += RandomChar();

        for(int i = 0; i < Random.Range(2,6); i++){
            char[] showedTextArrays = showedText.ToCharArray();
            showedTextArrays[index] = RandomChar();
            showedText = new string(showedTextArrays);
            yield return new WaitForSeconds(Random.Range(0.07f, delayValue));
        }

        char[] showedTextArray = showedText.ToCharArray();
        showedTextArray[index] = choosenPhrase[index];
        showedText = new string(showedTextArray);
    }

    char RandomChar(){
        return (char)Random.Range(32, 127);
    }

    void Update () {
        text.text = showedText;
    }
}
