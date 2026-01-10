using UnityEngine;

public class HTPPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pages; // assign your 6 image GameObjects in the inspector

    private int currentIndex = 0;

    void Start()
    {
        if (pages == null || pages.Length == 0)
            return;

        // Ensure only one page is active at start
        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
                pages[i].SetActive(i == currentIndex);
        }
    }

    // Call this from your Next button OnClick
    public void NextPage()
    {
        if (pages == null || pages.Length == 0)
            return;

        pages[currentIndex]?.SetActive(false);
        currentIndex = (currentIndex + 1) % pages.Length; // wraps around to the first page
        pages[currentIndex]?.SetActive(true);
    }

    // Call this from your Back button OnClick
    public void PrevPage()
    {
        if (pages == null || pages.Length == 0)
            return;

        pages[currentIndex]?.SetActive(false);
        currentIndex = (currentIndex - 1 + pages.Length) % pages.Length; // wraps around to the last page
        pages[currentIndex]?.SetActive(true);
    }
}
