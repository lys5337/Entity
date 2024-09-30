using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TJ
{
    public class LoadDataManager : MonoBehaviour
    {
        // 슬롯 번호에 따른 파일 경로를 가져오는 함수
        private string GetSaveFilePath(int slot)
        {
            return Application.persistentDataPath + $"/objectStates_slot{slot}.json";
        }

        // 슬롯에 맞게 오브젝트 상태를 불러오기
        public void LoadGame(int slot)
        {
            string saveFilePath = GetSaveFilePath(slot);

            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                ObjectStateList objectStateList = JsonUtility.FromJson<ObjectStateList>(json);

                foreach (ObjectState state in objectStateList.objectStates)
                {
                    GameObject obj = GameObject.Find(state.objectName);
                    if (obj != null)
                    {
                        // 오브젝트 상태 복원
                        Transform objTransform = obj.transform;
                        objTransform.position = state.position; // 월드 위치 사용
                        objTransform.eulerAngles = state.rotation;
                        objTransform.localScale = state.scale; // 월드 스케일 적용
                        obj.SetActive(state.isActive); // 활성화/비활성화 상태 복원
                    }
                }

                Debug.Log($"모든 오브젝트 상태가 슬롯 {slot}에서 불러와졌습니다.");
            }
            else
            {
                Debug.LogWarning($"슬롯 {slot}에 저장된 오브젝트 상태가 없습니다.");
            }
        }

    }
}
