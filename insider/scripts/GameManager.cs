using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace aki_lua87.UdonScripts.insider
{
    public class GameManager : UdonSharpBehaviour
    {
        // Debug
        [SerializeField] private Text Debug1;
        [SerializeField] private Text Debug2;
        [SerializeField] private Text Debug3;

        [SerializeField] private GameObject initPosObject;
        [SerializeField] private GameObject startObject;
        [SerializeField] private GameObject insiderObject;
        [SerializeField] private Text insiderObjectOdaiText;
        [SerializeField] private GameObject masterObject;
        [SerializeField] private Text masterObjectOdaiText;
        [SerializeField] private GameObject[] siminObject = new GameObject[5];
        [SerializeField] private GameObject[] playObject = new GameObject[4];
        public int GameState = 0; // 0->初期 1->人数選択 2->役職カードセット 3->ゲーム開始 4->Anser 5->thinkingTime 6->犯人当て 7->GameEnd 
        
        [UdonSynced]
        private string currentOdai = "不明なエラー(0)"; // お題はプレイヤー間で同期 値のセットはゲーム開始プレイヤーが実施
        // Tips: if (!Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) Networking.SetOwner(Networking.LocalPlayer, this.gameObject);

        private string[] odais = { 
            "ハンバーグ","駅弁", "カレーライス", "麦茶", "日本酒", "しじみ", "海苔" 
            , "定規", "はさみ", "ペン", "ノート", "付箋", "糊", "カッター" 
            , "のこぎり", "金槌", "斧", "鎌", "軍手", "ねじ" 
            , "道路", "標識", "信号機", "トンネル", "橋", "横断歩道" 
            , "神社", "役所", "小学校", "コンビニ", "発電所", "スーパー" 
            , "トウモロコシ", "ピーナッツ", "枝豆", "レントゲン", "天気予報", "ニュース"
            , "リンゴ", "サイコロ", "ボードゲーム", "トランプ", "オセロ", "将棋"
            , "壁", "椅子", "ベッド", "天井", "屋上", "地下" 
            , "Tシャツ", "めがね", "靴下", "ズボン", "パーカー", "パンツ"
            , "散歩", "ベビーカー", "ミイラ", "狐", "猫", "犬"
            , "潜水艦", "タクシー", "ヘリコプター", "ロケット", "消防車", "自動車"
            , "くすり", "テニス", "チョコレート", "作文", "歴史", "地球温暖化"
            , "VRChat", "インターネット", "タイムマシン", "金メダル", "ダンボール", "妖精"
            , "火星", "月", "火", "水", "空気", "食欲"
            , "台所", "風呂", "トイレ", "鏡", "庭", "プラネタリウム"
            , "山", "森", "島", "沼", "川", "海"
            , "弁護士", "国会議員", "教師(先生)", "警察官", "タクシー", "農家"
            , "貴族", "人間", "鳥", "天使", "吸血鬼", "ドラゴン","カニ"
            // , "", "", "", "", "", "" コピペ用
        };

        private Vector3[] initPos = new Vector3[8];

        private uint randomState = 54321;
        void Start()
        {
            randomState = (uint)System.DateTime.Now.Millisecond;
            Vector3 shift = initPosObject.transform.position;
            for(var i = 0; i<initPos.Length; i++)
            {
                initPos[i] = shift;
                shift.z -= 0.07f;
            }
            init();
        }

        private void initPosShuffle()
        {
            for (int i = 0; i < initPos.Length; i++) 
            {
                Vector3 tmp = initPos[i];
                int randomIndex = xorRandom(initPos.Length);
                initPos[i] = initPos[randomIndex];
                initPos[randomIndex] = tmp;
            }
		}

        // ゲームリセット(リセットボタンから呼び出し)
        public void init()
        {
            // Playオブジェクトを非表示
            DisactivePlayObject();
            // 開始ボタンをアクティブ化
            ActiveStartObject();

            // 札をシャッフル
            initPosShuffle();

            for(var i = 0; i<siminObject.Length; i++)
            {
                siminObject[i].transform.position = initPos[i];
                siminObject[i].transform.rotation = Quaternion.Euler(180, 0, 0);
            }
            Debug1.text = "デバッグウインドウ(仮)";
            insiderObject.transform.position = initPos[6];
            insiderObject.transform.rotation = Quaternion.Euler(180, 0, 0);
            Debug2.text = xorRandom(100).ToString();
            masterObject.transform.position = initPos[7];
            masterObject.transform.rotation = Quaternion.Euler(180, 0, 0);
            Debug3.text = fetchRandOdai();
            // 札を非アクティブ化
            for(var i = 0; i<siminObject.Length; i++)
            {
                siminObject[i].SetActive(false);
            }
            insiderObject.SetActive(false);
            masterObject.SetActive(false);
            // 札のロール公開もリセット -> お題はロールとセットで見える
            // ゲーム進行を0に
            GameState = 0;
        }

        // インサイダーゲームボタン
        public void GameInit()
        {
            GameState = 1;

            // お題を変数に代入(Syncのため代入だけ)
            setOdai();

            // 人数選択オブジェクトをアクティブ
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ActivePlayObject");

            // スタート用のスイッチをDisable
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "DisactiveStartObject");
        } 

        // インサイダーゲームボタン(から人数を引数に呼び出し)
        private void gameStart(int player)
        {
            GameState = 2;

            // 人数選択オブジェクトを非アクティブ
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "DisactivePlayObject");

            // 市民の数だけ市民札をActive化
            var siminCount = player - 2;
            for(var i = 0;i<siminCount;i++)
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ActiveSimin"+i.ToString());
            }
            // 役職札をアクティブ
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ActiveMaster");
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ActiveInsider");

            // お題を札にセット
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WriteOdai");
        }

        private void setOdai()
        {
            if (!Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
            currentOdai = fetchRandOdai();
        }

        private string fetchRandOdai()
        {
            int inum = xorRandom(odais.Length);
            return odais[inum];
        }

        public void ActivePlayObject()
        {
            for(var i = 0; i<playObject.Length; i++)
            {
                playObject[i].SetActive(true);
            }
        }

        public void DisactivePlayObject()
        {
            for(var i = 0; i<playObject.Length; i++)
            {
                playObject[i].SetActive(false);
            }
        }

        public void DisactiveStartObject()
        {
            startObject.SetActive(false);
        }
        public void ActiveStartObject()
        {
            startObject.SetActive(true);
        }

        // オブジェクト状態同期用
        public void WriteOdai()
        {
            // この関数動いてないときない？
            Debug2.text = "WriteOdai()" + currentOdai;

            // インサイダーとマスターの札にお題を書き込み
            this.insiderObjectOdaiText.text = currentOdai;
            this.masterObjectOdaiText.text = currentOdai;
        }
        public void ActiveMaster()
        {
            masterObject.SetActive(true);
        }
        public void ActiveInsider()
        {
            insiderObject.SetActive(true);
        }
        public void ActiveSimin0()
        {
            siminObject[0].SetActive(true);
        }
        public void ActiveSimin1()
        {
            siminObject[1].SetActive(true);
        }
        public void ActiveSimin2()
        {
            siminObject[2].SetActive(true);
        }
        public void ActiveSimin3()
        {
            siminObject[3].SetActive(true);
        }
        public void ActiveSimin4()
        {
            siminObject[4].SetActive(true);
        }
        public void ActiveSimin5()
        {
            siminObject[5].SetActive(true);
        }
        

        // インタラクト出来るオブジェクトから呼び出し↓
        public void StartPlayer4()
        {
            gameStart(4);
        }
        public void StartPlayer5()
        {
            gameStart(5);
        }
        public void StartPlayer6()
        {
            gameStart(6);
        }
        public void StartPlayer7()
        {
            gameStart(7);
        }
        public void StartPlayer8()
        {
            gameStart(8);
        }
        public void Reset()
        {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "init");
        }
        private int xorRandom(int n)
        {
            uint y = randomState;
            y = y ^ (y << 13);
            y = y ^ (y >> 17);
            y = y ^ (y << 5);
            randomState = y;
            double u = (randomState-1)/4294967295.0;
            int v = (int)(u*n);
            if(v == n) v = n-1;
            return v;
        }
    }
}