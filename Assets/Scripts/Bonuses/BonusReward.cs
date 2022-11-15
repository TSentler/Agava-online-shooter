using UnityEngine;

namespace Bonuses
{
    public class BonusReward : MonoBehaviour
    {
        private readonly string _increaseHPName = "IncreaseHP",
            _gunReadyName = "GunReady";
        
        [SerializeField] private float _extraHP;

        private RevardedVideo _revardedVideo;

        private void Awake()
        {
            _revardedVideo = FindObjectOfType<RevardedVideo>();
        }

        public void PrepareIncreaseHP()
        {
            _revardedVideo.OnRevardedVideoButtonClick("HP");
            //Debug.Log(_revardedVideo.IsRewarded);

            //if (_revardedVideo.IsRewarded == true)
            //{
            //    PrepareBonus(_increaseHPName, _extraHP);
            //}       
        }

        public void PrepareRifle()
        {
            _revardedVideo.OnRevardedVideoButtonClick("Rifle");
            //Debug.Log(_revardedVideo.IsRewarded);

            //if (_revardedVideo.IsRewarded == true)
            //{
            //    PrepareBonus(_gunReadyName, 1);
            //}
        }
        
        public void PrepareShotgun()
        {
            _revardedVideo.OnRevardedVideoButtonClick("Shotgun");
            //Debug.Log(_revardedVideo.IsRewarded);

            //if (_revardedVideo.IsRewarded == true)
            //{
            //    PrepareBonus(_gunReadyName, 2);
            //}
        }

        public void PrepareBonus(string name, float count)
        {
            PlayerPrefs.SetFloat(name, count);
            PlayerPrefs.Save();
        }
        
        public void PrepareBonus(string name, int count)
        {
            PlayerPrefs.SetInt(name, count);
            PlayerPrefs.Save();
        }
    }
}
