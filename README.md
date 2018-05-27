# Stone's Game
___
### 3Dゲームプロジェクト
自分の初めての３Dゲームです。Unityの勉強は一段落、今回は３D開発の流れを体験して見ます。

まずはゲームのタイプを決めます。自分は素晴らしい物語とカッコいいアクションがすきですから、以下の二つの要素を入れます。
- **RPG要素**
- **アクション要素**

#### RPG要素
RPGゲームをするとやはりストーリーですね。もちろん戦闘システムも重要なんですが、今回はActionで替わりますので、複雑の戦闘システムはとりあえす無視します。

ストーリーと言えば、台本が必要です。`mygame/Assets/Resource/Story/dialogue_story.md`このパスで台本があります。
> 追記: このストーリーファイルはただの見本です。会話しか記述されていません。良い台本にはその背景、時間などの重要事項も記述すべきです。

プログラムを台本を解析できろように、`XML`ファイルであるデータ形で保存します。保存パス：`mygame/Assets/Resource/Story/dialogue.xml`

もし台本が更新したら、その`XML`も更新します。
#### アクション要素
まずはプレイヤーが代表するキャラクターを動かしてみよう。その後アニメーションを増やして、キャラクターの動き種類を増やします。キャラクターのアニメーションは`mygame/Assets/Player/Animation`フォルダーに確認できます。

### プロジェクトの起動方法
まずプロジェクトをダンロードします。

Unity5で開きます。（自分のパソコン確認**Version: Unity 5.6.2f1 Personal(64bit)**）

Unity5プロジェクトに`Assets/Scenes/backyard`Sceneをダブルクリック開きます。

Unityゲームを実行します。

### ゲーム内容を説明
1. キャラクターアクションキーボード編
    1. `wsad`: キャラクターの移動
    1. `b`: 戦闘状態と正常状態のスイッチ。戦闘状態でしか使えないアクションがあります。
    1. `shift`: 押すと走ります、押さないと歩きます（動いてる場合）。正常状態と戦闘状態を含めます。
    1. `i`: 飛び蹴り、戦闘状態は普通の飛び蹴りです。正常状態で飛び蹴りすると、戦闘状態に入ります。
    1. `j`: 杖振り、近接攻撃。（戦闘状態で発動可能）
    1. `k`: 杖回り振り、近接攻撃。範囲はもっと広いです。（戦闘状態で発動可能）
        > 追記：この攻撃パタンは物理攻撃の上、魔法攻撃にしても多分いいアイディアですね。杖を振り回して、魔法の衝撃波（扇型）を放出しますとか。。。
    
    1. `l`: 視角ロック、視角ロックされたら、キャラクターの正面向き方向がロックされます。危険の時、後退する時前を警戒します。
    1. `m`: 魔法攻撃、威力絶大な単一方向魔法衝撃波。ただ魔法発動少々時間が必要です。（戦闘状態で発動可能）
    1. `f`: 近接魔法攻撃、近づいた敵を撃退することができます。（戦闘状態で発動可能）
    1. `q`: 攻撃躱す、後ろにジャンプ、攻撃を避ける。無敵時間があります。（戦闘状態で発動可能）
    1. `space`: 戦闘状態にジャンプすることができます。

2. NPC編    
    出場人物のNPCは2種類があります。モデルの名前`Archer`,`Mage`です。モデル保存位置は`Assets/Model/Charactors`です。

    メインキャラクターは登場人物と会話する事ができます。
    > 一応モデルにアニメーションも付いています。この後メインキャラクターになる可能性もあります。
3. モンスター編    
    `Assets/Model/Monsters`フォルダーにモンスターチームのモデルが保存してます。

    自動攻撃、自動パトロール機能実装されています。もしメインキャラクターはモンスターの感知範囲に入ると、キャラクターに攻撃をあたえます。もし感知範囲以外に逃げると、元の場所に戻ります。
4. マップ編
    > マップはざっくり作りました。五つの`Scene`を作りましたが、画面は雑過ぎで、見てはいられないです。ここで、開発を一旦止まって、画面の勉強を始めます。まずはUnityShaderを勉強すること。
### BUG表管理
`mygame/Assets/Resource/bugquestions.md`にBUGと不具合を管理します。

### ゲーム企画書
`mygame/Assets/Resources/游戏企划书.md`にゲームの完成度を記録します。

### ソースマージ記録
- **2018/05/04 merge the branch:game-resouece to master branch**
