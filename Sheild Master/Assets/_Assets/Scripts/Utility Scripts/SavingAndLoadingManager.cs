using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace SheildMaster{

    public class SavingAndLoadingManager : MonoBehaviour{
        public static SavingAndLoadingManager instance {get;private set;}
        
        [SerializeField] private SaveData saveData;
        
        
        private void Awake(){
            if(instance == null){
                instance = this;
            }else{
                Destroy(instance);
            }
            DontDestroyOnLoad(instance);
            
            LoadGame();
            

        }
        [ContextMenu("SAVE GAME")]
        public void SaveGame(){
            saveData.playerData.Save();
            saveData.settingsSO.Save();
            for (int i = 0; i < saveData.itemSOArray.Length; i++){
                saveData.itemSOArray[i].Save();
            }
            for (int i = 0; i < saveData.levelDatas.Length; i++){
                saveData.levelDatas[i].Save();
            }
            
        }
        [ContextMenu("LOAD GAME")]
        public void LoadGame(){
            saveData.playerData.Load();
            saveData.settingsSO.Load();
            for (int i = 0; i < saveData.itemSOArray.Length; i++){
                saveData.itemSOArray[i].Load();
            }
            for (int i = 0; i < saveData.levelDatas.Length; i++){
                saveData.levelDatas[i].Load();
            }
            
        }
        private void OnApplicationPause(){
            SaveGame();
        }
        
        private void OnApplicationQuit(){
            SaveGame();
            
        }

    }
    [System.Serializable]
    public struct SaveData{
        public PlayerDataSO playerData;
        public ShopItemSO[] itemSOArray;
        public LevelDataSO[] levelDatas;
        public SettingsSO settingsSO;
        
    }

}