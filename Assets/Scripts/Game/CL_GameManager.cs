using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState {
    PLAY,
    PAUSE
}

public class CL_GameManager : MonoBehaviour 
{
    [Header("GAME STATE")]
    public static GameState gameState;

    [Header("UI REFERENCES")]
    public TextMeshProUGUI playStateTMP;
    public TextMeshProUGUI timestepTMP;

    [Header("OTHER REFERENCES")]
    public CL_Grid grid;

    private void Start() {
        gameState = GameState.PAUSE;
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0.1f;

        playStateTMP.text = string.Format("{0} (Press Numpad 5 to resume)", gameState.ToString());
        timestepTMP.text = string.Format("Timestep: {0} fps", (1/Time.fixedDeltaTime).ToString("F0"));
    }

    private void Update() {
        // PLAY/PAUSE State Input
        if (Input.GetKeyDown(KeyCode.Keypad5)) {
            PlayPauseSwitchStateDefinition();
        }

        // Adds 1 FPS
        if (Input.GetKeyDown(KeyCode.Keypad6)) {
            AddFPS(1);
        }
        // Adds 10 FPS
        if (Input.GetKeyDown(KeyCode.Keypad8)) {
            AddFPS(10);
        }
        // Removes 1 FPS
        if (Input.GetKeyDown(KeyCode.Keypad4)) {
            RemoveFPS(1);
        }
        // Removes 10 FPS
        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            RemoveFPS(10);
        }

        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            grid.ResetGrid();
        }
    }

    private void PlayPauseSwitchStateDefinition() {
        if (gameState == GameState.PLAY) { 
            gameState = GameState.PAUSE; 
            Time.timeScale = 0;
            playStateTMP.text = string.Format("{0} (Press Numpad 5 to resume)", gameState.ToString());
        }
        else { 
            gameState = GameState.PLAY;
            Time.timeScale = 1;
            playStateTMP.text = string.Format("{0} (Press Numpad 5 to pause)", gameState.ToString());
        }
    }

    private void AddFPS(float amount) {
        float f = (1 / Time.fixedDeltaTime) + amount;
        if (f > 60) { f = 60; }

        Time.fixedDeltaTime = 1 / f;
        timestepTMP.text = string.Format("Timestep: {0} fps", (1/Time.fixedDeltaTime).ToString("F0"));
    }

    private void RemoveFPS(float amount) {
        float f = (1 / Time.fixedDeltaTime) - amount;
        if (f < 1) { f = 1; }

        Time.fixedDeltaTime = 1 / f;
        timestepTMP.text = string.Format("Timestep: {0} fps", (1/Time.fixedDeltaTime).ToString("F0"));
    }
}