using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public Button yourButton; // Kéo và thả Button từ Scene vào đây trong Inspector
    public AudioClip clickSound; // Đặt âm thanh bạn muốn phát khi bấm vào Button

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Lấy AudioSource từ GameObject này
        audioSource.clip = clickSound; // Gán AudioClip cần phát vào AudioSource

        Button[] buttons = FindObjectsOfType<Button>(); // Tìm tất cả các Button trong Scene

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => PlayClickSound()); // Gắn hàm PlayClickSound vào sự kiện click của từng Button
        }
    }

    void PlayClickSound()
    {
        audioSource.Play(); // Phát âm thanh khi Button được bấm
    }
}
