namespace GleyDailyRewards
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class DayButtonScript : MonoBehaviour
    {
        public AudioClip rewardSound;
        public TextMeshProUGUI dayText;
        public Image rewardImage;
        public TextMeshProUGUI rewardValue;

        public GameObject availableEffect;
        public Image dayBg;
        public Sprite claimedSprite;
        public Sprite currentSprite;
        public Sprite availableSprite;
        public Sprite lockedSprite;

        private Button button;
        Sprite rewardSprite;
        int dayNumber;
        int reward;


        /// <summary>
        /// Setup each day button
        /// </summary>
        /// <param name="dayNumber">current day number</param>
        /// <param name="rewardSprite">button sprite</param>
        /// <param name="rewardValue">reward value</param>
        /// <param name="currentDay">current active day</param>
        /// <param name="timeExpired">true if timer expired</param>
        public void Initialize(int dayNumber, Sprite rewardSprite, int rewardValue, int currentDay, bool timeExpired)
        {
            dayText.text = dayNumber.ToString();
            rewardImage.sprite = rewardSprite;
            this.rewardValue.text = rewardValue.ToString();
            this.dayNumber = dayNumber;
            this.rewardSprite = rewardSprite;
            reward = rewardValue;
            button = GetComponent<Button>();

            Refresh(currentDay,timeExpired);
        }


        /// <summary>
        /// Refresh button if required
        /// </summary>
        /// <param name="currentDay"></param>
        /// <param name="timeExpired"></param>
        public void Refresh(int currentDay,bool timeExpired)
        {
            if (dayNumber - 1 < currentDay)
            {
                dayBg.sprite = claimedSprite;
                dayText.gameObject.SetActive(false);
                availableEffect.SetActive(false);
            }

            if (dayNumber - 1 == currentDay)
            {
                if (timeExpired == true)
                {
                    dayBg.sprite = availableSprite;
                    rewardImage.color = Color.white;
                    button.interactable = true;
                    availableEffect.SetActive(true);
                }
                else
                {
                    dayBg.sprite = currentSprite;
                    rewardImage.color = Color.grey;
                }
                dayText.gameObject.SetActive(true);
            }

            if (dayNumber - 1 > currentDay)
            {
                availableEffect.SetActive(false);
                dayBg.sprite = lockedSprite;
                dayText.gameObject.SetActive(true);
                rewardImage.color = Color.grey;
                button.interactable = false;
            }
        }


        /// <summary>
        /// Called when a day button is clicked
        /// </summary>
        public void ButtonClicked()
        {
            if (dayBg.sprite == availableSprite)
            {
                CalendarManager.Instance.ButtonClick(dayNumber, reward, rewardSprite);
                GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(rewardSound);
            }
        }
    }
}
