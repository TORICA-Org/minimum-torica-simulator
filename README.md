# Minimum TORICA Simulator
TORICA Simulatorの最小構成版です．

この`README`が制作手順書です．
とにかく作ってみよう！

## 0. Unityで3Dゲームを作るには？
<details>
<summary>Content</summary>

3Dゲームを作るのは，本来とても難しいことです．
作りたいゲームの要素を盛り込むまでに描写処理や物理演算など，
ゲームの前提となる処理がとても複雑だからです．

そこで登場するのが，「ゲームエンジン」と呼ばれるものです．
ゲームの前提となる処理が最初から組み込まれており，
開発者はゲーム要素の設計に集中することができます．

Unityは最も有名なゲームエンジンのひとつです．
簡単な制御なら，プログラミング言語に触れずとも実装できるほど機能が豊富です．
複雑な処理は，C#と呼ばれるプログラミング言語を介して実装します．

[Unity (ゲームエンジン) - Wikipedia](https://ja.wikipedia.org/wiki/Unity_(%E3%82%B2%E3%83%BC%E3%83%A0%E3%82%A8%E3%83%B3%E3%82%B8%E3%83%B3))

</details>

## 1. Unityの開発環境を構築する
<details>
<summary>Content</summary>

Unityで開発を行うためには，適切なバージョンの"Unity Editor"をインストールする必要があります．
この手順書では，"Unity 6"の安定版（LTS）を使用します．（2026/04/09現在: Unity 6.3 LTS）

以下に示すブログに従って，Unity Editorのインストールまでしてみましょう．

[\[Unity入門\] 1. Unity Editorのインストールをしよう！ #初心者 - Qiita](https://qiita.com/tugamecreate/items/7d87931161979b6cb4fa)
[【初心者向け】Unityエディタのインストール方法 - 渋谷ほととぎす通信](https://shibuya24.info/entry/unity-install-unityeditor)

</details>


## 2. 新しくプロジェクトを作成する
<details>
<summary>Content</summary>

Unity Hubを起動し，"New Project"から新規プロジェクトを作成しましょう．
今回は3Dのゲームを作るので，"Universal 3D"というプロジェクトテンプレートを使います．
基本的に保存形式はUnityアカウントに紐づくクラウドとローカルのストレージを併用しますが，
ローカルのみを選択することもできます．

[【2026年最新】Windowsで始めるUnity！導入から初期設定までの完全ガイド！ | TechChance! 公式ブログ](https://techchance.jp/blog/2025/11/08/how_to_introduce_unity_for_windows/)
[\[Unity\]最初のプロジェクトテンプレートはどれがいいのか？〜URP・HDRP・Built-inの違い〜[初心者向け] | えきふるゲームラボ](https://ekifurulab.com/unitytemplate/)

</details>


## 2. Unity Editorを知る
<details>
<summary>Content</summary>

まずは，Unity Editorに慣れましょう！

[【初心者向け】Unityエディタの基本操作を解説！ | Unity Learning Materials](https://learning.unity3d.jp/10734/)

[初心者が覚えておくべき11のUnityエディタ機能【Unityの使い方】 - 渋谷ほととぎす通信](https://shibuya24.info/entry/unity-operation)

</details>


## 3. 水面を作る
<details>
<summary>Content</summary>

簡単なものから作っていきましょう．まずは水面です．
ヒエラルキーの左上にある＋マークから，`3D Object` > `Plane`を作成します．

![add-plane](img/add-plane.png)


これで，1辺が10mの平面が作成されました．
ヒエラルキーで`Plane`が選択された状態では，
画面右側のインスペクターが表示されているはずです．

![added-plane](img/added-plane.png)


ここで，この平面の位置と大きさを次のように変更します．
`Scale`を変更し，ここではXZ軸方向への大きさを200倍にしています．
これによって，1辺が2kmの平面ができました．

![scale-plane](img/scale-plane.png)

</details>


## 3. 飛行機の3Dモデルを取り込む
<details>
<summary>Content</summary>

見た目から入っていきましょう！

あなたがこれをブラウザ上のGitHubから見ているのであれば，
`<> Code` > `Download ZIP`で必要なファイルを取得できます．

展開したZIPファイルには，`obj`という名前のフォルダーがあるはずです．
その中にある`ARG-1.obj`という名前のファイルを，
Unity Editor下部の`Project` > `Assets`フォルダー内にドラッグ＆ドロップしてください．

![import-obj](img/import-obj.png)


すると，飛行機らしきシルエットのサムネイルがでるはずです．

![imported-obj](img/imported-obj.png)


`ARG-1`をクリックしてインスペクターを表示させ，`Scale Factor`を`0.001`に変更します．
これは，Objファイルがmm（ミリメートル）単位で記録している寸法値を，Unity上でm（メートル）単位で解釈するためです．

![scale-obj](img/scale-obj.png)


`ARG-1`をヒエラルキーにドラッグ＆ドロップし，シーンに追加します．

![add-obj](img/add-obj.png)


`ARG-1`を右クリックし，`Prefab` > `Unpack Completely`で完全に展開します．

![unpack-completely-obj](img/unpack-completely-obj.png)


ここからの操作が複雑です．
`ARG-1`には`Wing`，`fin`，`tail`が含まれますが，これらを組み替えて，
`Wing`に`fin`，`tail`が含まれるようにします．
空になった`ARG-1`は削除し，`Wing`の名前を`Aircraft`としましょう．

![edit-hierarchy-1](img/edit-hierarchy-1.png)
![edit-hierarchy-2](img/edit-hierarchy-2.png)
![edit-hierarchy-3](img/edit-hierarchy-3.png)
![edit-hierarchy-4](img/edit-hierarchy-4.png)


`Aircraft`をクリックし，インスペクターを表示させます．
`Position`を次のように変更します．
鳥人間コンテストにおいてはプラットフォームの高さがおよそ10m，
そこにパイロットの腰の高さが加わるので，およそ11mの高さに設置しました．
また，助走の10mだけ後退させました．

![position-aircraft](img/position-aircraft.png)


これで設置は完了です．

</details>


## 4. カメラを設定する
<details>
<summary>Content</summary>

もとからある`Main Camera`を`Aircraft`の子オブジェクトにします．
こうすることで，`Aircraft`の動きに`Main Camera`が追従するようになります．

![hierarchy-camera](img/hierarchy-camera.png)


`Main Camera`のインスペクターで，`Transform`を次のように設定します．
`Aircraft`との相対的な位置と回転を設定しています．

![transform-camera](img/transform-camera.png)


画面下部のバーのボタンから，カメラのプレビューを確認することができます．

![camera-preview](img/camera-preview.png)


上部にある再生ボタンから，`Play mode`に切り替えることができます．

![enter-play-mode](img/enter-play-mode.png)
![in-play-mode](img/in-play-mode.png)


カメラに正しく写っていればOKです！

</details>

## 5. 物理演算を適用する
<details>
<summary>Content</summary>

ついに`Aircraft`を動かすときがきました！

`Aircraft`のインスペクターを表示させ，`Add Component` > `Rigidbody`で`Rigidbody`コンポーネントを追加します．

![add-rigidbody](img/add-rigidbody.png)
![added-rigidbody](img/added-rigidbody.png)


`Use Gravity`にチェックが入っていることを確認します．
そうしたら，上部の再生ボタンを押してみましょう！
どうなりましたか？

</details>

## 6. 衝突判定を適用する
<details>
<summary>Content</summary>

`5.`では，物理演算を適用したことにより重力が働き，下へ落下していきました．
しかし，先ほど作成した平面にあたってもすり抜けてしまっています．

これでは面白くありません．衝突を判定する「コライダー」を設定しましょう．

まず，`Plane`のインスペクターで，もとからある`Mesh Collider`を削除します．

![remove-mesh-collider](img/remove-mesh-collider.png)


代わりに，処理がより簡単な`Box Collider`を追加します．

![added-box-collider](img/added-box-collider.png)


次に，`Aircraft`にもコライダーを設定します．
このとき，`Convex`にチェックを入れておきます（凹面が存在するため）．

![added-mesh-collider](img/added-mesh-collider.png)

そうしたら，もう一度上部の再生ボタンを押してみましょう！
どうなりましたか？

</details>

## 
<details>
<summary>Content</summary>
</details>



## 
<details>
<summary>Content</summary>
</details>

