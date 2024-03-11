using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PhotoCapture : MonoBehaviour
{
    private InputManager inputManager;
    private Texture2D screenCapture;

    [SerializeField] private GameObject polaroidPrefab;
    [SerializeField] private float polaroidPixelsPerUnit = 100f;

    [SerializeField] private Vector3 instantiatePosition = new Vector3(-0.2f, 2.6f, 0.4f);
    [SerializeField] private Vector3 instantiateRotation = new Vector3(0, 90, 25);

    PlayableDirector director;

    GameObject currentPhoto;

    [SerializeField] private PlayableAsset cameraInPlayable;
    [SerializeField] private PlayableAsset cameraOutPlayable;
    [SerializeField] private float offsetCameraHorizontal = 0;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        inputManager.playerInteractEvent += Interact;
        inputManager.playerLookInCameraEvent += LookInCamera;
        inputManager.playerUnLookInCameraEvent += LookOutCamera;
        screenCapture = new Texture2D(Screen.height, Screen.height, TextureFormat.RGB24, false);
        director = GetComponent<PlayableDirector>();
    }


    private void Interact()
    {
        if (!inputManager.IsLookingInCamera)
            return;

        if (currentPhoto != null) currentPhoto.SetActive(false);
        StartCoroutine(CapturePhoto());
    }

    private void LookInCamera()
    {
        director.playableAsset = cameraInPlayable;
        director.Play();
    }

    private void LookOutCamera()
    {
        director.playableAsset = cameraOutPlayable;
        director.Play();
    }

    IEnumerator CapturePhoto()
    {
        yield return new WaitForEndOfFrame();
        screenCapture.ReadPixels(new Rect(offsetCameraHorizontal, 0, Screen.height, Screen.height), 0, 0, false);
        screenCapture.Apply();

        InstantiatePhoto();
    }

    private void InstantiatePhoto()
    {
        GameObject photo = Instantiate(polaroidPrefab, transform);
        photo.transform.localPosition = instantiatePosition;
        photo.transform.localEulerAngles = instantiateRotation;
        photo.GetComponentInChildren<SpriteRenderer>().sprite = Sprite.Create(screenCapture, new Rect(0, 0, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), polaroidPixelsPerUnit);
        currentPhoto = photo;
    }
}