using UnityEngine;

namespace Bonuses
{
    public class BonusReward : MonoBehaviour
    {
        private readonly string _increaseHPName = "IncreaseHP",
            _gunReadyName = "GunReady";
        
        [SerializeField] private float _extraHP;

        public void PrepareIncreaseHP()
        {
            PrepareBonus(_increaseHPName, _extraHP);
        }

        public void PrepareShotgun()
        {
            PrepareBonus(_gunReadyName, 2);
        }

        private void PrepareBonus(string name, float count)
        {
            PlayerPrefs.SetFloat(name, count);
            PlayerPrefs.Save();
        }
        
        private void PrepareBonus(string name, int count)
        {
            PlayerPrefs.SetInt(name, count);
            PlayerPrefs.Save();
        }
    }
}
