using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefynModules.EventCore.Monobehaviors;
using System;
using DefynModules.EventCore.Managers;
using Photon.Pun;

public class GameAreaController : EventHandledMonoBehavior
{
    public ContainerController[] row0;
    public ContainerController[] row1;
    public ContainerController[] row2;

    EnumSelection localPlayerSelection = EnumSelection.Cross;
    EnumSelection enemyPlayerSelection = EnumSelection.Cross;

    public Sprite circle;
    public Sprite cross;
    Vector2Int lastMoveId;

    private Sprite localPlayerSprite;
    private Sprite enemyPlayerSprite;

    bool isFirstPlayerLastRound;

    private bool isTurn;

    public override void SubscribeEvents()
    {
        EventManager.AddListener<WinnerCheckEvent>(HandleOnWinnerCheckEvent);
        EventManager.AddListener<GameStartEvent>(HandleOnGameStartEvent);
        EventManager.AddListener<NetworkEvent>(HandleOnNetworkEvent);
        EventManager.AddListener<GameResetEvent>(HandleOnGameResetEvent);
    }

    private void HandleOnGameResetEvent(GameResetEvent e)
    {
        if (e.launch)
        {
            Reset();
            isTurn = false;
            isFirstPlayerLastRound = false;
        }
    }

    private void HandleOnNetworkEvent(NetworkEvent e)
    {
        if(e.enumNetworkEvent == EnumNetworkEvent.GameStart)
        {
            if (GameManager.GetIfLocalPlayerTurn())
            {
                isFirstPlayerLastRound = true;
                isTurn = true;
                localPlayerSelection = EnumSelection.Circle;
                enemyPlayerSelection = EnumSelection.Cross;
                localPlayerSprite = circle;
                enemyPlayerSprite = cross;
                Toast.MakeText(EnumToast.Debug, "Your Turn!");
            }
            else
            {
                isTurn = false;
                localPlayerSelection = EnumSelection.Cross;
                enemyPlayerSelection = EnumSelection.Circle;
                localPlayerSprite = cross;
                enemyPlayerSprite = circle;
            }
        }
        if(e.enumNetworkEvent == EnumNetworkEvent.Move)
        {
            MoveNetworkData moveNetworkData = e.networkEventData as MoveNetworkData;
            switch (moveNetworkData.movePos.x)
            {
                case 0:
                    row0[moveNetworkData.movePos.y].SetEnemyState(enemyPlayerSelection , enemyPlayerSprite);
                    break;
                case 1:
                    row1[moveNetworkData.movePos.y].SetEnemyState(enemyPlayerSelection, enemyPlayerSprite);
                    break;
                case 2:
                    row2[moveNetworkData.movePos.y].SetEnemyState(enemyPlayerSelection, enemyPlayerSprite);
                    break;
            }
            isTurn = true;
            Toast.MakeText(EnumToast.Debug, "Your Turn!");
        }
        if (e.enumNetworkEvent == EnumNetworkEvent.GameOver)
        {
            GameOverNetworkData gameOverNetworkData = e.networkEventData as GameOverNetworkData;
            switch (gameOverNetworkData.movePos.x)
            {
                case 0:
                    row0[gameOverNetworkData.movePos.y].SetSpriteOnly(enemyPlayerSelection, enemyPlayerSprite);
                    break;
                case 1:
                    row1[gameOverNetworkData.movePos.y].SetSpriteOnly(enemyPlayerSelection, enemyPlayerSprite);
                    break;
                case 2:
                    row2[gameOverNetworkData.movePos.y].SetSpriteOnly(enemyPlayerSelection, enemyPlayerSprite);
                    break;
            }

            Toast.MakeText(EnumToast.Debug, "Try next time!");
            AudioManager.PlaySFX(EnumAudioId.Lose);
            isFirstPlayerLastRound = true;
            LeanTween.delayedCall(2f, () => 
            {
            Reset();
            Toast.MakeText(EnumToast.Debug, "Your turn!");
            isTurn = true;
            });
        }
        if (e.enumNetworkEvent == EnumNetworkEvent.Draw)
        {
            GameDrawNetworkData gameDrawNetworkData = e.networkEventData as GameDrawNetworkData;
            switch (gameDrawNetworkData.movePos.x)
            {
                case 0:
                    row0[gameDrawNetworkData.movePos.y].SetSpriteOnly(enemyPlayerSelection, enemyPlayerSprite);
                    break;
                case 1:
                    row1[gameDrawNetworkData.movePos.y].SetSpriteOnly(enemyPlayerSelection, enemyPlayerSprite);
                    break;
                case 2:
                    row2[gameDrawNetworkData.movePos.y].SetSpriteOnly(enemyPlayerSelection, enemyPlayerSprite);
                    break;
            }
            Toast.MakeText(EnumToast.Debug, "Draw!");
            AudioManager.PlaySFX(EnumAudioId.Lose);
            LeanTween.delayedCall(2f, () => {
                Reset();
                Toast.MakeText(EnumToast.Debug, "Your turn!");
                if (!isFirstPlayerLastRound)
                    isTurn = true;
            });
            
        }
    }

    private void HandleOnGameStartEvent(GameStartEvent e)
    {
    }

    public override void UnsubscribeEvents()
    {
        EventManager.RemoveListener<WinnerCheckEvent>(HandleOnWinnerCheckEvent);
    }

    private void HandleOnWinnerCheckEvent(WinnerCheckEvent e)
    {
        if(e.launch)
         CheckForWinner();
    }

    

    private void Reset()
    {
        for(int i = 0; i<3; i++)
        {
            row0[i].Reset();
        }
        for (int i = 0; i < 3; i++)
        {
            row1[i].Reset();
        }
        for (int i = 0; i < 3; i++)
        {
            row2[i].Reset();
        }
    }



    void CheckForWinner()
    {
        // Y 
        for (int i = 0; i < 3; i++)
        {
            if (row0[i].selection == row1[i].selection && row0[i].selection == row2[i].selection && row0[i].selection != EnumSelection.NotMarked)
            {
                GameOver();
                return;
            }
        }
        if (row0[0].selection == row0[1].selection && row0[0].selection == row0[2].selection && row0[0].selection != EnumSelection.NotMarked)
        {
            GameOver();
            return;
        }
        if (row1[0].selection == row1[1].selection && row1[0].selection == row1[2].selection && row1[0].selection != EnumSelection.NotMarked)
        {
            GameOver();
            return;
        }
        if (row2[0].selection == row2[1].selection && row2[0].selection == row2[2].selection && row2[0].selection != EnumSelection.NotMarked)
        {
            GameOver();
            return;
        }
        if (row0[0].selection == row1[1].selection && row0[0].selection == row2[2].selection && row0[0].selection != EnumSelection.NotMarked)
        {
            GameOver();
            return;
        }
        if (row0[2].selection == row1[1].selection && row0[2].selection == row2[0].selection && row0[2].selection != EnumSelection.NotMarked)
        {
            GameOver();
            return;
        }

        if (Array.Exists(row0, containerController => containerController.selection == EnumSelection.NotMarked)
            || Array.Exists(row1, containerController => containerController.selection == EnumSelection.NotMarked)
            || Array.Exists(row2, containerController => containerController.selection == EnumSelection.NotMarked)
            )
        {
            ContinueGame();
        }
        else
        {
            object[] objects = { lastMoveId.x, lastMoveId.y };
            Toast.MakeText(EnumToast.Debug, "Draw!");
            AudioManager.PlaySFX(EnumAudioId.Lose);
            NetworkingManager.RaiseNetworkEvent(EnumNetworkEvent.Draw, objects);
            LeanTween.delayedCall(2f, () =>
            {
                Reset();
                if (!isFirstPlayerLastRound)
                    isTurn = true;
                Toast.MakeText(EnumToast.Debug, "Your turn!");
            });
        }
    }

    void GameOver()
    {
        isFirstPlayerLastRound = false;
        object[] data = { lastMoveId.x , lastMoveId.y };
        AudioManager.PlaySFX(EnumAudioId.Win);
        Toast.MakeText(EnumToast.Debug, "You Won!");
        NetworkingManager.RaiseNetworkEvent(EnumNetworkEvent.GameOver, data);
        LeanTween.delayedCall(2f, () =>
        {
            Reset();
            isTurn = false;
            Toast.MakeText(EnumToast.Debug, "Opponents turn!");
        });
        
    }

    void ContinueGame()
    {
        object[] data = { lastMoveId.x, lastMoveId.y };
        NetworkingManager.RaiseNetworkEvent(EnumNetworkEvent.Move, data);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isTurn)
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition) , Vector2.zero);
            if (hit.collider != null)
            {
                ContainerController containerController = hit.transform.gameObject.GetComponent<ContainerController>();
                lastMoveId = containerController.id;
                containerController.SetState(localPlayerSelection, localPlayerSprite);
                isTurn = false;
            }
        }
    }
}
