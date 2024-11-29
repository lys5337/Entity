using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public static class IntentDescriptionProvider
    {
        public static string GetIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            Debug.Log("적의 이름: " + enemy.nameUI);
            switch (enemy.nameUI)
            {
                /////0스테이지/////
                case "동굴의 작은 늑대":
                    return GetWolfIntentDescription(enemy, intentType);
                case "동굴의 작은 뱀":
                    return GetSnakeIntentDescription(enemy, intentType);
                case "동굴의 박쥐들":
                    return GetBatsIntentDescription(enemy, intentType);
                case "동굴의 움직이는 뼈":
                    return GetSkullIntentDescription(enemy, intentType);
                case "동굴의 사나운 늑대":
                    return GetFearWolfIntentDescription(enemy, intentType);
                case "동굴의 주인":
                    return GetBossBearIntentDescription(enemy, intentType);

                /////1스테이지/////
                case "고블린 궁수":
                    return GetGoblinArcherIntentDescription(enemy, intentType);
                case "고블린 검사":
                    return GetGoblinWarriorIntentDescription(enemy, intentType);
                case "마족 짐승":
                    return GetDemonBeastIntentDescription(enemy, intentType);
                case "고블린 선봉대":
                    return GetGoblinVanguardIntentDescription(enemy, intentType);
                case "트롤 전사":
                    return GetTrollWarriorIntentDescription(enemy, intentType);
                case "고블린 지휘관":
                    return GetGoblinCommanderIntentDescription(enemy, intentType);



                /////2스테이지/////
                //2-1스테이지//

                case "나태한 강도":
                    return GetLazyBanditIntentDescription(enemy, intentType);
                case "비열한 강도":
                    return GetVillainBanditIntentDescription(enemy, intentType);
                case "욕망의 강도":
                    return GetGreedyBanditIntentDescription(enemy, intentType);
                case "자만의 강도":
                    return GetProudBanditIntentDescription(enemy, intentType);
                case "전장에서 도망친 병사":
                    return GetDeserterIntentDescription(enemy, intentType);
                case "초원의 사자":
                    return GetLionIntentDescription(enemy, intentType);
                case "강도를 이끄는 자":
                    return GetBanditLeaderIntentDescription(enemy, intentType);

                
                //2-2스테이지//

                case "숲의 박쥐 때":
                    return GetBatKingIntentDescription(enemy, intentType);
                case "숲의 해골병사":
                    return GetSkeletonSoldierIntentDescription(enemy, intentType);
                case "숲의 파수꾼":
                    return GetSentryIntentDescription(enemy, intentType);
                case "숲의 지키는 첫번째 골렘":
                    return GetFirstGolemIntentDescription(enemy, intentType);
                case "숲을 지키는 두번째 골렘":
                    return GetSecondGolemIntentDescription(enemy, intentType);
                case "숲의 중앙을 수호하는 얼음 골렘":
                    return GetCentralGolemIntentDescription(enemy, intentType);
                case "침식의 숲의 주인":
                    return GetForestBossIntentDescription(enemy, intentType);

                /////3스테이지/////

                //3-1스테이지//

                case "겁먹은 병사":
                    return GetScaredSoldierIntentDescription(enemy, intentType);
                case "입구를 지키는 병사":
                    return GetEntranceGuardIntentDescription(enemy, intentType);
                case "순찰을 도는 병사":
                    return GetPatrolSoldierIntentDescription(enemy, intentType);
                case "잡힌 강도":
                    return GetCaughtBanditIntentDescription(enemy, intentType);
                case "경비견":
                    return GetGuardDogIntentDescription(enemy, intentType);
                case "흉학한 죄수":
                    return GetWickedPrisonerIntentDescription(enemy, intentType);
                case "교도관":
                    return GetJailerIntentDescription(enemy, intentType);
                case "간수장":
                    return GetWardenIntentDescription(enemy, intentType);


                //3-2스테이지//

                case "마족 돌격병":
                    return GetDemonWarriorIntentDescription(enemy, intentType);
                case "마족 마법사":
                    return GetDemonMageIntentDescription(enemy, intentType);
                case "마족 전사":
                    return GetDemonSoldierIntentDescription(enemy, intentType);
                case "마족 주술사":
                    return GetDemonShamanIntentDescription(enemy, intentType);
                case "마족 지원가":
                    return GetDemonSupportIntentDescription(enemy, intentType);
                case "마족 외팔 장군":
                    return GetDemonGeneralIntentDescription(enemy, intentType);
                case "마족에 넘어간 기사": 
                    return GetFallenKnightIntentDescription(enemy, intentType);
                case "침략 선봉 대장":
                    return GetVanguardLeaderIntentDescription(enemy, intentType);
                

                default:
                    return GetDefaultIntentDescription(intentType);
            }
        }

        ////0스테이지////
        private static string GetWolfIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "Impacts_Effect/CFXR2 Ground Hit";
                    return "물기";
                case EnemyAction.IntentType.Block:
                    return "웅크리기";
                case EnemyAction.IntentType.StrategicDebuff:
                    enemy.enemyEffectName = "Magic_Effect_addition/Buff_02aDown";
                    return "하울링";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "Impacts_Effect/CFXR2 Ground Hit";
                    return "날카로운 이빨";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetSnakeIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "Impacts_Effect/CFXR2 Ground Hit";
                    return "물기";
                case EnemyAction.IntentType.Block:
                    return "똬리 틀기";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "Impacts_Effect/CFXR2 Ground Hit";
                    return "독 엄니";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetBatsIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "Impacts_Effect/CFXR2 Ground Hit";
                    return "물기";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "Impacts_Effect/CFXR2 Ground Hit";
                    return "날개 치기";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetSkullIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "Impacts_Effect/CFXR2 Ground Hit";
                    return "부딛치기";
                case EnemyAction.IntentType.StrategicDebuff:
                    return "공포의 시선";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "Impacts_Effect/CFXR2 Ground Hit";
                    return "기분 나쁜 손길";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetFearWolfIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "Impacts_Effect/CFXR2 Ground Hit";
                    return "물기";
                case EnemyAction.IntentType.Block:
                    return "웅크리기";
                case EnemyAction.IntentType.StrategicDebuff:
                    enemy.enemyEffectName = "Magic_Effect_addition/Buff_02aDown";
                    return "하울링";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "날카로운 이빨";
                case EnemyAction.IntentType.EliteWolf_0Stage:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "잔인한 물기";
                case EnemyAction.IntentType.Waiting:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "...?";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetBossBearIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "내려 찍기";
                case EnemyAction.IntentType.Block:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "웅크리기";
                case EnemyAction.IntentType.StrategicBuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "발톱 정비";
                case EnemyAction.IntentType.StrategicDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "울부 짖기";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "물어 찢기";
                case EnemyAction.IntentType.BossBear_0Stage:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "흉포한 돌진";
                case EnemyAction.IntentType.Waiting:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "...?";
                default:
                    return "알 수 없음";
            }
        }
        //-----0스테이지----//

        ////1스테이지////

        private static string GetGoblinArcherIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.StrategicBuff:
                    return "사격 자세";
                case EnemyAction.IntentType.RangedAttack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "화살 발사";
                case EnemyAction.IntentType.Waiting:
                    return "독 화살 장전 중...";
                case EnemyAction.IntentType.RangedAttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "독 화살 발사";
                case EnemyAction.IntentType.Block:
                    return "수비 태세";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetGoblinWarriorIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "검 휘두르기";
                case EnemyAction.IntentType.Block:
                    return "방패 들기";
                case EnemyAction.IntentType.StrategicDebuff:
                    return "소리 지르기";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "오염된 칼날";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetDemonBeastIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "물기";
                case EnemyAction.IntentType.Block:
                    return "단단한 피부";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "날카로운 이빨";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetGoblinVanguardIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Waiting:
                    return "...";
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "돌진";
                case EnemyAction.IntentType.Block:
                    return "방어 자세";
                case EnemyAction.IntentType.SelfBuffPlayerDeuff:
                    return "공포의 주시";
                case EnemyAction.IntentType.StrategicBuff:
                    return "분노";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "깊은 열상";
                case EnemyAction.IntentType.SpecialAttack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit PLAIN (Cross)";
                    return "잔인한 가르기";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetTrollWarriorIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Waiting:
                    return "준비 중...";
                //case EnemyAction.IntentType.Waiting:
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "창 찌르기";
                case EnemyAction.IntentType.Block:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "방어 창술";
                case EnemyAction.IntentType.RangedAttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "창 던지기";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetGoblinCommanderIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //고블린 지휘관
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "기마 돌진";
                case EnemyAction.IntentType.Block:
                    return "방어 자세";
                case EnemyAction.IntentType.StrategicBuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "격노";
                case EnemyAction.IntentType.StrategicDebuff:
                    return "경멸의 눈빛";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "상처 가르기";
                case EnemyAction.IntentType.SpecialAttack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit PLAIN (Cross)";
                    return "잔인한 가르기";
                case EnemyAction.IntentType.Waiting:
                    return "심상치 않은 기운이 느껴진다...";
                default:
                    return "알 수 없음";
            }
        }


        //-----1스테이지----//

        ////2스테이지////

        //2-1스테이지//

        private static string GetLazyBanditIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Waiting:
                    return "...";
                case EnemyAction.IntentType.GoldStealAttack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "주머니 털기";
                case EnemyAction.IntentType.Block:
                    return "피하기";
                case EnemyAction.IntentType.StrategicBuff:
                    return "분노";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "약점 공격";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetVillainBanditIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Waiting:
                    return "...";
                case EnemyAction.IntentType.GoldStealAttack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "주머니 털기";
                case EnemyAction.IntentType.Block:
                    return "피하기";
                case EnemyAction.IntentType.StrategicBuff:
                    return "분노";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "약점 공격";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetGreedyBanditIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Waiting:
                    return "...";
                case EnemyAction.IntentType.GoldStealAttack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "주머니 털기";
                case EnemyAction.IntentType.Block:
                    return "피하기";
                case EnemyAction.IntentType.StrategicBuff:
                    return "분노";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "약점 공격";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetProudBanditIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Waiting:
                    return "...";
                case EnemyAction.IntentType.GoldStealAttack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "주머니 털기";
                case EnemyAction.IntentType.Block:
                    return "피하기";
                case EnemyAction.IntentType.StrategicBuff:
                    return "분노";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "약점 공격";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetDeserterIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Waiting:
                    return "...전의 상실";
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "베기";
                case EnemyAction.IntentType.Block:
                    return "막기";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetLionIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType) //자버프, 공격, 방어, 디버프, 디버프 공격
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "발톱 휘두르기";
                case EnemyAction.IntentType.Block:
                    return "피하기";
                case EnemyAction.IntentType.StrategicBuff:
                    return "표효";
                case EnemyAction.IntentType.StrategicDebuff:
                    return "경계";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "깨물어부수기";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetBanditLeaderIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)
        {
            switch (intentType) // 디버프, 공격, 막기, 자버프, 대기, 특수공격
            {
                case EnemyAction.IntentType.Waiting:
                    return "심상치 않은 기운이 느껴진다...";
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "깊게 베기";
                case EnemyAction.IntentType.Block:
                    return "막아내기";
                case EnemyAction.IntentType.StrategicDebuff:
                    return "경멸의 눈빛";
                case EnemyAction.IntentType.StrategicBuff:
                    return "공격 준비";
                case EnemyAction.IntentType.SpecialAttack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "치명상 공격";
                default:
                    return "알 수 없음";
            }
        }

        //2-2스테이지//

        private static string GetBatKingIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //박쥐 때
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "물기";
                case EnemyAction.IntentType.Block:
                    return "피하기";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetSkeletonSoldierIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)//해골병사 
        {
            switch (intentType) //자버프, 공격, 방어, 디버프, 디버프 공격
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "뼈 휘두르기";
                case EnemyAction.IntentType.Block:
                    return "막기";
                case EnemyAction.IntentType.StrategicDebuff:
                    return "비명 지르기";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetSentryIntentDescription(Enemy enemy, EnemyAction.IntentType intentType)//숲의 파수꾼
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Waiting:
                    return "...";   
                case EnemyAction.IntentType.RangedAttack:
                    enemy.enemyEffectName = "Fire_Effect/CFXR3 Hit Fire B (Air)";
                    return "화염 공격";
                case EnemyAction.IntentType.Block:
                    return "막아내기";
                case EnemyAction.IntentType.StrategicBuff:
                    return "공격 준비";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetFirstGolemIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //첫번째 골램
        {
            switch (intentType) // 디버프, 공격, 막기, 자버프, 대기, 특수공격
            {
                case EnemyAction.IntentType.Waiting:
                    return "...";
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "내려 찍기";
                case EnemyAction.IntentType.Block:
                    return "막아내기";
                case EnemyAction.IntentType.AttackDebuff:
                    return "숲의 저주";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetSecondGolemIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //두번재 골렘
        {
            switch (intentType) // 디버프, 공격, 막기, 자버프, 대기, 특수공격
            {
                case EnemyAction.IntentType.Waiting:
                    return "...";
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "내려 찍기";
                case EnemyAction.IntentType.Block:
                    return "막기";
                case EnemyAction.IntentType.AttackDebuff:
                    return "깊은 숲의 저주";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetCentralGolemIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //세번째 골렘
        {
            switch (intentType) // 디버프, 공격, 막기, 자버프, 대기, 특수공격
            {
                case EnemyAction.IntentType.Waiting:
                    return "...";
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "내려 찍기";
                case EnemyAction.IntentType.Block:
                    return "막아내기";
                case EnemyAction.IntentType.AttackDebuff:
                    return "저주 걸기";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetForestBossIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //숲의 주인
        {
            switch (intentType) // 디버프, 공격, 막기, 자버프, 대기, 특수공격
            {
                case EnemyAction.IntentType.Waiting:
                    return "심상치 않은 기운이 느껴진다...";
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "Nature_Effect/CFXR3 Hit Leaves A (Lit)";
                    return "나뭇잎 베기";
                case EnemyAction.IntentType.Block:
                    return "나무 방패";
                case EnemyAction.IntentType.StrategicBuff:
                    return "공격 준비";
                case EnemyAction.IntentType.RangedAttackDebuff:
                    enemy.enemyEffectName = "Nature_Effect/CFXR3 Shield Leaves A (Lit)";
                    return "자연의 분노";
                default:
                    return "알 수 없음";
            }
        }


        //-----2스테이지----//


        ////3스테이지////
        //3-1스테이지//

        private static string GetScaredSoldierIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //겁먹은 병사
        {
            switch (intentType) //공격 막기 공격디버프 자버프
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "무작정 공격";
                case EnemyAction.IntentType.Block:
                    return "막기";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "약점 공격";
                case EnemyAction.IntentType.StrategicBuff:
                    return "격노";
                    
                default:
                    return "알 수 없음";
            }
        }

        private static string GetEntranceGuardIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //입구를 지키는 병사
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "무작정 공격";
                case EnemyAction.IntentType.Block:
                    return "막기";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "약점 공격";
                case EnemyAction.IntentType.StrategicBuff:
                    return "격노";
                    
                default:
                    return "알 수 없음";
            }
        }

        private static string GetPatrolSoldierIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //순찰을 도는 병사
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "무작정 공격";
                case EnemyAction.IntentType.Block:
                    return "막기";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "약점 공격";
                case EnemyAction.IntentType.StrategicBuff:
                    return "격노";
                    
                default:
                    return "알 수 없음";
            }
        }

        private static string GetCaughtBanditIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //잡힌 강도
        {
            switch (intentType) //공격, 목조르기, 방어
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "사슬 휘두르기";
                case EnemyAction.IntentType.Block:
                    return "사슬로 막기";
                case EnemyAction.IntentType.StrategicBuff:
                    return "분노";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetGuardDogIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //경비견
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "물기";
                case EnemyAction.IntentType.Block:
                    return "피하기";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "약점 공격";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetWickedPrisonerIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //흉학한 죄수
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "사슬 공격";
                case EnemyAction.IntentType.Block:
                    return "사슬 수비";
                case EnemyAction.IntentType.StrategicBuff:
                    return "분노";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "무력화 시키기";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetJailerIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //교도관
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "창 찌르기";
                case EnemyAction.IntentType.Block:
                    return "수비 태세";
                case EnemyAction.IntentType.StrategicBuff:
                    return "격노";
                case EnemyAction.IntentType.Waiting:
                    return "...자세 잡는 중...";
                case EnemyAction.IntentType.RangedAttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "창 던지기";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetWardenIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //간수장
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "철퇴 휘두르기";
                case EnemyAction.IntentType.Block:
                    return "막기";
                case EnemyAction.IntentType.StrategicBuff:
                    return "극대노";
                case EnemyAction.IntentType.Waiting:
                    return "...분노가 느껴진다...";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "정의의 철퇴";
                default:
                    return "알 수 없음";
            }
        }

        //3-2스테이지//

        private static string GetDemonWarriorIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //마족 돌격병
        {
            switch (intentType) //공격 막기 공격디버프 자버프
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "세로 가르기";
                case EnemyAction.IntentType.Block:
                    return "막아내기";
                case EnemyAction.IntentType.AttackDebuff:
                    return "상처 공격";
                case EnemyAction.IntentType.StrategicBuff:
                    return "멸시하는 시선";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetDemonSoldierIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //마족 전사
        {
            switch (intentType) //공격 막기 공격디버프 자버프
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "가로 베기";
                case EnemyAction.IntentType.Block:
                    return "공격 막기";
                case EnemyAction.IntentType.StrategicDebuff:
                    return "무시하는 시선";
                case EnemyAction.IntentType.AttackDebuff:
                    return "무릎 베기";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetDemonMageIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //마족 마법사
        {
            switch (intentType) //공격 막기 공격디버프 자버프
            {
                case EnemyAction.IntentType.Waiting:
                    return "마력 모으는 중";
                case EnemyAction.IntentType.StrategicBuff:
                    return "마력 증폭";
                case EnemyAction.IntentType.RangedAttack:
                    enemy.enemyEffectName = "Fire_Effect/CFXR3 Hit Fire B (Air)";
                    return "파이어볼";
                case EnemyAction.IntentType.Block:
                    return "막기";
                case EnemyAction.IntentType.SpecialAttack:
                    enemy.enemyEffectName = "Impacts_Effect/CFXR Impact Glowing HDR (Blue)";
                    return "방전";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetDemonShamanIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //마족 주술사
        {
            switch (intentType) //공격 막기 공격디버프 자버프
            {
                case EnemyAction.IntentType.Waiting:
                    return "마력 충전";
                case EnemyAction.IntentType.StrategicBuff:
                    return "마력 증폭";
                case EnemyAction.IntentType.RangedAttack:
                    enemy.enemyEffectName = "Fire_Effect/CFXR3 Hit Fire B (Air)";
                    return "화상";
                case EnemyAction.IntentType.Block:
                    return "수비 자세";
                case EnemyAction.IntentType.SpecialAttack:
                    enemy.enemyEffectName = "Impacts_Effect/CFXR Impact Glowing HDR (Blue)";
                    return "뇌격";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetDemonSupportIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //마족 지원가
        {
            switch (intentType) //공격 막기 공격디버프 자버프
            {
                case EnemyAction.IntentType.Waiting:
                    return "마력 모으는 중";
                case EnemyAction.IntentType.StrategicBuff:
                    return "마력 증폭";
                case EnemyAction.IntentType.RangedAttack:
                    enemy.enemyEffectName = "Fire_Effect/CFXR3 Hit Fire B (Air)";
                    return "파이어볼";
                case EnemyAction.IntentType.Block:
                    return "막기";
                case EnemyAction.IntentType.SpecialAttack:
                    enemy.enemyEffectName = "Impacts_Effect/CFXR Impact Glowing HDR (Blue)";
                    return "뇌전의 구슬";
                default:
                    return "알 수 없음";
            }
        }

        private static string GetDemonGeneralIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //마족 외팔 장군
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "복부 찌르기";
                case EnemyAction.IntentType.AttackBuff:
                    return "분노";
                case EnemyAction.IntentType.AttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "무릎 찌르기";
                case EnemyAction.IntentType.Block:
                    return "공격 방어";
                case EnemyAction.IntentType.Waiting:
                    return "심상치 않은 기운이 느껴진다...";
                case EnemyAction.IntentType.RangedAttackDebuff:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "투창";

                default:
                    return "알 수 없음";
            }
        }

        private static string GetFallenKnightIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //타락한 기사
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Waiting:
                    return "...";
                //대기
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "깊숙한 찌르기";
                //대기
                case EnemyAction.IntentType.Block:
                    return "부동 자세";
                case EnemyAction.IntentType.AttackDebuff:
                    return "늑골 분쇄";

                default:
                    return "알 수 없음";
            }
        }

        private static string GetVanguardLeaderIntentDescription(Enemy enemy, EnemyAction.IntentType intentType) //선봉대장
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Waiting:
                    return "심상치 않은 기운이 느껴진다...";
                case EnemyAction.IntentType.StrategicBuff:
                    return "누적되는 분노";
                case EnemyAction.IntentType.Attack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "돌진 참수";
                case EnemyAction.IntentType.Block:
                    return "수비 자세"; 
                case EnemyAction.IntentType.SelfBuffPlayerDeuff:
                    return "영혼 흡수";
                //수비자세//
                //대기//
                case EnemyAction.IntentType.SpecialAttack:
                    enemy.enemyEffectName = "SwordTrail_Effect/CFXR4 Sword Hit FIRE (Cross)";
                    return "산산 조각내기";
                
                
                default:
                    return "알 수 없음";
            }
        }





        //-----3스테이지----//


        private static string GetDefaultIntentDescription(EnemyAction.IntentType intentType)
        {
            switch (intentType)
            {
                case EnemyAction.IntentType.Attack:
                    return "?";
                case EnemyAction.IntentType.Block:
                    return "?";
                case EnemyAction.IntentType.StrategicBuff:
                    return "?";
                case EnemyAction.IntentType.StrategicDebuff:
                    return "?";
                case EnemyAction.IntentType.AttackDebuff:
                    return "?";
                default:
                    return "알 수 없음";
            }
        }
    }
}
