using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{
    public Image displayImage;
    public Sprite[] images; // Mảng chứa các Sprite (ảnh) cần hiển thị
    private int currentImageIndex = 0;

    void Start()
    {
        // Hiển thị ảnh đầu tiên khi game bắt đầu
        DisplayCurrentImage();
    }

    public void ShowNextImage()
    {
        currentImageIndex = (currentImageIndex + 1) % images.Length;
        DisplayCurrentImage();
    }

    public void ShowPreviousImage()
    {
        currentImageIndex = (currentImageIndex - 1 + images.Length) % images.Length;
        DisplayCurrentImage();
    }

    void DisplayCurrentImage()
    {
        displayImage.sprite = images[currentImageIndex];
    }
}
