using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TJ
{
    public class SaveDataManager : MonoBehaviour
    {
        // 슬롯 번호에 따른 경로 설정
        private string GetSaveFilePath(int slot)
        {
            return Application.persistentDataPath + $"/objectStates_slot{slot}.json";
        }

        // 슬롯에 맞게 모든 오브젝트 상태를 저장 (비활성화된 오브젝트도 포함)
        public void SaveGame(int slot)
        {
            List<ObjectState> allObjectStates = new List<ObjectState>();

            // 모든 루트 오브젝트를 가져옴
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (GameObject rootObj in rootObjects)
            {
                // 루트 오브젝트와 자식 오브젝트까지 모두 포함하여 상태 저장
                SaveObjectStateRecursive(rootObj.transform, allObjectStates);
            }

            // JSON으로 변환 후 슬롯 번호에 맞는 파일로 저장
            string json = JsonUtility.ToJson(new ObjectStateList { objectStates = allObjectStates }, true);
            File.WriteAllText(GetSaveFilePath(slot), json);
            Debug.Log($"모든 오브젝트 상태가 슬롯 {slot}에 저장되었습니다.");
        }

        // 재귀적으로 오브젝트 상태 저장 (비활성화된 오브젝트 포함)
        private void SaveObjectStateRecursive(Transform obj, List<ObjectState> allObjectStates)
        {
            ObjectState objectState = new ObjectState
            {
                objectName = obj.name,
                position = obj.position, // 월드 위치를 사용
                rotation = obj.eulerAngles,
                scale = obj.localScale,
                isActive = obj.gameObject.activeSelf // 활성화 상태 저장
            };

            allObjectStates.Add(objectState);

            // 자식 오브젝트들도 재귀적으로 저장
            foreach (Transform child in obj)
            {
                SaveObjectStateRecursive(child, allObjectStates);
            }
        }
    }

    [System.Serializable]
    public class ObjectStateList
    {
        public List<ObjectState> objectStates;
    }

    [System.Serializable]
    public class ObjectState
    {
        public string objectName;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public bool isActive;
    }
}
