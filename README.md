# Minimum TORICA Simulator
TORICA Simulatorの最小構成版です．

この`README`が制作手順書です．
とにかく作ってみよう！

---
<details>
<summary>
  
## 0. Unityとは？

</summary>

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

---

<details>
<summary>

## 1. Unityの開発環境を構築する

</summary>

Unityで開発を行うためには，適切なバージョンの"Unity Editor"をインストールする必要があります．
この手順書では，"Unity 6"の安定版（LTS）を使用します．（2026/04/09現在: Unity 6.3 LTS）

以下に示すブログに従って，Unity Editorのインストールまでしてみましょう．

[\[Unity入門\] 1. Unity Editorのインストールをしよう！ #初心者 - Qiita](https://qiita.com/tugamecreate/items/7d87931161979b6cb4fa)

[【初心者向け】Unityエディタのインストール方法 - 渋谷ほととぎす通信](https://shibuya24.info/entry/unity-install-unityeditor)

</details>

---

<details>
<summary>

## 2. 新しくプロジェクトを作成する

</summary>

Unity Hubを起動し，"New Project"から新規プロジェクトを作成しましょう．
今回は3Dのゲームを作るので，"Universal 3D"というプロジェクトテンプレートを使います．
基本的に保存形式はUnityアカウントに紐づくクラウドとローカルのストレージを併用しますが，
ローカルのみを選択することもできます．

[【2026年最新】Windowsで始めるUnity！導入から初期設定までの完全ガイド！ | TechChance! 公式ブログ](https://techchance.jp/blog/2025/11/08/how_to_introduce_unity_for_windows/)

[\[Unity\]最初のプロジェクトテンプレートはどれがいいのか？〜URP・HDRP・Built-inの違い〜[初心者向け] | えきふるゲームラボ](https://ekifurulab.com/unitytemplate/)

![create-project](img/create-project.png)

</details>

---

<details>
<summary>

## 3. Unity Editorを知る

</summary>

まずは，Unity Editorに慣れましょう！

[【初心者向け】Unityエディタの基本操作を解説！ | Unity Learning Materials](https://learning.unity3d.jp/10734/)

[初心者が覚えておくべき11のUnityエディタ機能【Unityの使い方】 - 渋谷ほととぎす通信](https://shibuya24.info/entry/unity-operation)

</details>

---

<details>
<summary>

## 4. 水面を作る

</summary>

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

---

<details>
<summary>

## 5. 飛行機の3Dモデルを取り込む

</summary>

見た目から入っていきましょう！

あなたがこれをブラウザ上のGitHubから見ているのであれば，
`<> Code` > `Download ZIP`で必要なファイルを取得できます．

![download-zip](img/download-zip.png)


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

---

<details>
<summary>

## 6. カメラを設定する

</summary>

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

---

<details>
<summary>

## 7. 物理演算を適用する

</summary>

ついに`Aircraft`を動かすときがきました！

`Aircraft`のインスペクターを表示させ，`Add Component` > `Rigidbody`で`Rigidbody`コンポーネントを追加します．

![add-rigidbody](img/add-rigidbody.png)
![added-rigidbody](img/added-rigidbody.png)


`Use Gravity`にチェックが入っていることを確認します．
そうしたら，上部の再生ボタンを押してみましょう！
どうなりましたか？

</details>

---

<details>
<summary>

## 8. 衝突判定を適用する

</summary>

物理演算を適用したことにより重力が働き，下へ落下していったはずです！
しかし，先ほど作成した平面に当たってもすり抜けてしまっています．

これでは面白くありません．衝突を判定する「コライダー」を設定しましょう．

まず，`Plane`のインスペクターで，もとからある`Mesh Collider`を削除します．

![remove-mesh-collider](img/remove-mesh-collider.png)


代わりに，処理がより単純な`Box Collider`を追加します．

![added-box-collider](img/added-box-collider.png)


次は，`Aircraft`に`Mesh Collider`を設定します．
このとき，`Convex`にチェックを入れておきます（凹面が存在するため）．

![added-mesh-collider](img/added-mesh-collider.png)

そうしたら，もう一度上部の再生ボタンを押してみましょう！
どうなりましたか？

</details>

---

<details>
<summary>

## 9. 空力計算を実装する

</summary>

コライダーによる衝突判定により，地面に落ちた際の挙動がリアルに再現されたはずです！

ここでついに，フライトシミュレーターの肝である空力計算を実装したいと思います．

...しかしながら，あまりにも難解過ぎるので，
元九州大学鳥人間チーム全体設計の[いーそー](https://x.com/mtk_birdman)様制作の
[BR Simulator for Glider](https://mtkbirdman.com/birdmanrallysim-for-glider)に含まれる空力計算用スクリプト
`AerodynamicCalculator.cs`を利用させていただきます．

先ほどダウンロードしたZIPファイルに含まれる`base` > `AerodynamicCalculator.cs`は，
この手順書における`Aircraft`にアタッチするだけで上手く動作するように改変したものです．

まず，`Assets`フォルダの中で右クリックし，`Create` > `Folder`で新たにフォルダを作成します．
フォルダの名前は`Scripts`としてください．

![mkdir](img/mkdir.png)
![rename-dir](img/rename-dir.png)


`base`フォルダを`Scripts`フォルダにドラッグ＆ドロップします．

![copy-base](img/copy-base.png)
![base-on-project](img/base-on-project.png)


C#スクリプトは，ヒエラルキー上のゲームオブジェクトにドラッグ＆ドロップすることでアタッチすることができます．

`Aircraft`に`AerodynamicCalculator`を

`tail`に`Elevator`を

`fin`に`Rudder`を

それぞれアタッチします．

![attach-scripts](img/attach-scripts.png)


`Aircraft`のインスペクターを開きます．

![aircraft-inspector](img/aircraft-inspector.png)


`AerodynamicCalculator`にある`Action Prop`の"︙（縦三点リーダー）"をクリックし，
`Use Reference`を選択します．

![action-prop-use-reference](img/action-prop-use-reference.png)


右端の"◎（二重丸）"をクリックし，"Player/Move"をアタッチします．

![select-input-action-reference](img/select-input-action-reference.png)
![aircraft-inspector-final](img/aircraft-inspector-final.png)


これで，機体の挙動は鳥人間コンテスト滑空機部門出場機を再現したものになったはずです！
上部の再生ボタンを押した後，スペースキーを押すことでフライトがスタートします！
WASDキーもしくは矢印キーでラダーとエレベーターを操作できます．

</details>

---

<details>
<summary>

## 10. プラットフォームを作成する

</summary>

プラットフォームはあったほうが良いでしょう！

ヒエラルキー上で，`3D Object` > `Cube`を作成します．

![create-cube](img/create-cube.png)


インスペクターで，`Cube`の`Transform`を次のように設定します．

![pratform-transform](img/pratform-transform.png)

</details>

---

<details>
<summary>

## 11. ヒンジジョイントを使う

</summary>

いーそー様のブログにある，
[【Unity】フライトシミュレーターを作る：新機体の実装 | 垂直尾翼と水平尾翼のオブジェクトの設定を行う](https://mtkbirdman.com/unity-implementation-new-model#toc9)
に従って設定してみましょう．

これにて，かなりそれっぽい簡易フライトシミュレーターが完成したと思います！！！

</details>

---

<details>
<summary>

## 12. ここからの発展

</summary>

C#への理解を深めるうえで，言語は違いますが
[C++入門 AtCoder Programming Guide for beginners (APG4b) - AtCoder](https://atcoder.jp/contests/apg4b)
の第1章は一通りやることをおすすめします．

- カメラの複数配置
- 水面のマテリアル作成
- UI Toolkitを使ったUIの作成
などなど...

</details>

---
