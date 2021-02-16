using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

public enum GameState {
    PLAY,
    PAUSE
}

public class CL_GameManager : MonoBehaviour
{
    [Header("GAME STATE")]
    public static GameState gameState;

    [Header("MENU")]
    public GameObject menu;

    [Header("UI REFERENCES")]
    public TextMeshProUGUI playStateTMP;
    public TextMeshProUGUI timestepTMP;
    public bool resetGridOnPreset { get; set; }
    public Toggle resetGridOnPresetToggle;
    [Space]
    public Slider bloomIntensity;
    public TextMeshProUGUI bloomIntensityValue;
    public Slider bloomThreshold;
    public TextMeshProUGUI bloomThresholdValue;
    public Slider chromaticAberration;
    public TextMeshProUGUI chromaticAberrationValue;
    public Slider vignetteIntensity;
    public TextMeshProUGUI vignetteIntensityValue;
    public Slider vignetteSmoothness;
    public TextMeshProUGUI vignetteSmoothnessValue;
    public Slider vignetteRoundness;
    public TextMeshProUGUI vignetteRoundnessValue;
    
    [Header("POST PROCESSING")]
    public PostProcessProfile pp_profile;
    private Bloom pp_bloom;
    private ChromaticAberration pp_chromaticAberration;
    private Vignette pp_vignette;

    [Header("OTHER REFERENCES")]
    public CL_Grid grid;

    private void Awake() {
        if (pp_profile.TryGetSettings<Bloom>(out pp_bloom)) {
            bloomIntensity.value = pp_bloom.intensity;
            bloomThreshold.value = pp_bloom.threshold;
            bloomIntensityValue.text = bloomIntensity.value.ToString("F2");
            bloomThresholdValue.text = bloomThreshold.value.ToString("F2");
        }
        if (pp_profile.TryGetSettings<ChromaticAberration>(out pp_chromaticAberration)) {
            chromaticAberration.value = pp_chromaticAberration.intensity;
            chromaticAberrationValue.text = chromaticAberration.value.ToString("F2");
        }
        if (pp_profile.TryGetSettings<Vignette>(out pp_vignette)) {
            vignetteIntensity.value = pp_vignette.intensity;
            vignetteSmoothness.value = pp_vignette.smoothness;
            vignetteRoundness.value = pp_vignette.roundness;
            vignetteIntensityValue.text = vignetteIntensity.value.ToString("F2");
            vignetteSmoothnessValue.text = vignetteSmoothness.value.ToString("F2");
            vignetteRoundnessValue.text = vignetteRoundness.value.ToString("F2");
        }
    }

    private void Start() {
        gameState = GameState.PAUSE;
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0.1f;

        playStateTMP.text = string.Format("{0}", gameState.ToString());
        timestepTMP.text = string.Format("Timestep: {0} fps", (1/Time.fixedDeltaTime).ToString("F0"));

        resetGridOnPreset = resetGridOnPresetToggle.isOn = true;
    }

    private void Update() {
        // Play/Pause Setup
        GameStateSetup();
        // FPS/Timestep Setup
        FPSSetup();
        // Preset Setup
        PresetSetup();
        // Grid Reset/Restart
        Restart();
        // Menu Toggle
        ToggleMenu();
    }

    private void GameStateSetup() {
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            if (gameState == GameState.PLAY) { 
                gameState = GameState.PAUSE; 
                Time.timeScale = 0;
                playStateTMP.text = string.Format("{0}", gameState.ToString());
            }
            else { 
                gameState = GameState.PLAY;
                Time.timeScale = 1;
                playStateTMP.text = string.Format("{0}", gameState.ToString());
            }
        }
    }

    private void FPSSetup() {
        // Adds 1 FPS
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            AddFPS(1);
        }
        // Adds 10 FPS
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            AddFPS(10);
        }
        // Removes 1 FPS
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            RemoveFPS(1);
        }
        // Removes 10 FPS
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            RemoveFPS(10);
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

    private void Restart() {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            // We make sure we pause the game before resetting the grid
            gameState = GameState.PAUSE;
            Time.timeScale = 0;
            playStateTMP.text = string.Format("{0}", gameState.ToString());

            grid.ResetGrid();
        }
    }

    private void PresetSetup() {
        // Gosper's Glider Gun
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            UsePreset(1);
        }
        // Simkin Glider Gun
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            UsePreset(2);
        }
        // Pulsar
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            UsePreset(3);
        }
        // Penta-decathlon
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            UsePreset(4);
        }
        // R-pentomino
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            UsePreset(5);
        }
        // Acorn
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            UsePreset(6);
        }
    }

    private void UsePreset(int index) {
        // We first reset the grid (if the toggle is active)
        if (resetGridOnPreset) { 
            // We make sure we pause the game before resetting the grid
            gameState = GameState.PAUSE;
            Time.timeScale = 0;
            playStateTMP.text = string.Format("{0}", gameState.ToString());

            grid.ResetGrid();
        }

        // We hide the menu on preset key down
        if (menu.activeSelf) { menu.SetActive(false); }

        switch (index) {
            // Gosper's Glider Gun
            case 1:
                grid.ActivateCell(50,43);
                grid.ActivateCell(50,42);
                grid.ActivateCell(51,43);
                grid.ActivateCell(51,42);
                grid.ActivateCell(60,43);
                grid.ActivateCell(60,42);
                grid.ActivateCell(60,41);
                grid.ActivateCell(61,44);
                grid.ActivateCell(61,40);
                grid.ActivateCell(62,45);
                grid.ActivateCell(62,39);
                grid.ActivateCell(63,45);
                grid.ActivateCell(63,39);
                grid.ActivateCell(64,42);
                grid.ActivateCell(65,44);
                grid.ActivateCell(65,40);
                grid.ActivateCell(66,43);
                grid.ActivateCell(66,42);
                grid.ActivateCell(66,41);
                grid.ActivateCell(67,42);
                grid.ActivateCell(70,45);
                grid.ActivateCell(70,44);
                grid.ActivateCell(70,43);
                grid.ActivateCell(71,45);
                grid.ActivateCell(71,44);
                grid.ActivateCell(71,43);
                grid.ActivateCell(72,46);
                grid.ActivateCell(72,42);
                grid.ActivateCell(74,46);
                grid.ActivateCell(74,47);
                grid.ActivateCell(74,42);
                grid.ActivateCell(74,41);
                grid.ActivateCell(84,45);
                grid.ActivateCell(84,44);
                grid.ActivateCell(85,45);
                grid.ActivateCell(85,44);
                break;
                
            // Simkin Glider Gun
            case 2:
                grid.ActivateCell(48,49);
                grid.ActivateCell(48,48);
                grid.ActivateCell(49,49);
                grid.ActivateCell(49,48);
                grid.ActivateCell(52,46);
                grid.ActivateCell(52,45);
                grid.ActivateCell(53,46);
                grid.ActivateCell(53,45);
                grid.ActivateCell(55,49);
                grid.ActivateCell(55,48);
                grid.ActivateCell(56,49);
                grid.ActivateCell(56,48);
                grid.ActivateCell(68,32);
                grid.ActivateCell(68,31);
                grid.ActivateCell(69,39);
                grid.ActivateCell(69,38);
                grid.ActivateCell(69,37);
                grid.ActivateCell(69,32);
                grid.ActivateCell(69,30);
                grid.ActivateCell(70,40);
                grid.ActivateCell(70,37);
                grid.ActivateCell(70,30);
                grid.ActivateCell(71,40);
                grid.ActivateCell(71,37);
                grid.ActivateCell(71,30);
                grid.ActivateCell(71,29);
                grid.ActivateCell(73,40);
                grid.ActivateCell(74,40);
                grid.ActivateCell(74,36);
                grid.ActivateCell(75,39);
                grid.ActivateCell(75,37);
                grid.ActivateCell(76,38);
                grid.ActivateCell(79,38);
                grid.ActivateCell(79,37);
                grid.ActivateCell(80,38);
                grid.ActivateCell(80,37);
                break;
            // Pulsar
            case 3:
                grid.ActivateCell(67,44);
                grid.ActivateCell(67,43);
                grid.ActivateCell(67,42);
                grid.ActivateCell(67,38);
                grid.ActivateCell(67,37);
                grid.ActivateCell(67,36);
                grid.ActivateCell(69,46);
                grid.ActivateCell(69,41);
                grid.ActivateCell(69,39);
                grid.ActivateCell(69,34);
                grid.ActivateCell(70,46);
                grid.ActivateCell(70,41);
                grid.ActivateCell(70,39);
                grid.ActivateCell(70,34);
                grid.ActivateCell(71,46);
                grid.ActivateCell(71,41);
                grid.ActivateCell(71,39);
                grid.ActivateCell(71,34);
                grid.ActivateCell(72,44);
                grid.ActivateCell(72,43);
                grid.ActivateCell(72,42);
                grid.ActivateCell(72,38);
                grid.ActivateCell(72,37);
                grid.ActivateCell(72,36);
                grid.ActivateCell(74,44);
                grid.ActivateCell(74,43);
                grid.ActivateCell(74,42);
                grid.ActivateCell(74,38);
                grid.ActivateCell(74,37);
                grid.ActivateCell(74,36);
                grid.ActivateCell(75,46);
                grid.ActivateCell(75,41);
                grid.ActivateCell(75,39);
                grid.ActivateCell(75,34);
                grid.ActivateCell(76,46);
                grid.ActivateCell(76,41);
                grid.ActivateCell(76,39);
                grid.ActivateCell(76,34);
                grid.ActivateCell(77,46);
                grid.ActivateCell(77,41);
                grid.ActivateCell(77,39);
                grid.ActivateCell(77,34);
                grid.ActivateCell(79,44);
                grid.ActivateCell(79,43);
                grid.ActivateCell(79,42);
                grid.ActivateCell(79,38);
                grid.ActivateCell(79,37);
                grid.ActivateCell(79,36);
                break;
            // Penta-decathlon    
            case 4:
                grid.ActivateCell(72,44);
                grid.ActivateCell(72,42);
                grid.ActivateCell(72,39);
                grid.ActivateCell(72,37);
                grid.ActivateCell(73,48);
                grid.ActivateCell(73,47);
                grid.ActivateCell(73,45);
                grid.ActivateCell(73,42);
                grid.ActivateCell(73,39);
                grid.ActivateCell(73,36);
                grid.ActivateCell(73,34);
                grid.ActivateCell(73,33);
                grid.ActivateCell(74,44);
                grid.ActivateCell(74,42);
                grid.ActivateCell(74,39);
                grid.ActivateCell(74,37);
                break;
            // R-pentomino
            case 5:
                grid.ActivateCell(72,40);
                grid.ActivateCell(73,41);
                grid.ActivateCell(73,40);
                grid.ActivateCell(73,39);
                grid.ActivateCell(74,41);
                break;
            // Acorn
            case 6:
                grid.ActivateCell(70,40);
                grid.ActivateCell(71,42);
                grid.ActivateCell(71,40);
                grid.ActivateCell(73,41);
                grid.ActivateCell(74,40);
                grid.ActivateCell(75,40);
                grid.ActivateCell(76,40);
                break;
        }
    }

    private void ToggleMenu() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(!menu.activeSelf);
        }
    }

    public void SetBloomIntensity() {
        bloomIntensityValue.text = bloomIntensity.value.ToString("F2");
        pp_bloom.intensity.Override(bloomIntensity.value);
    }
    public void SetBloomThreshold() {
        bloomThresholdValue.text = bloomThreshold.value.ToString("F2");
        pp_bloom.threshold.Override(bloomThreshold.value);
    } 
    public void SetChromaticAberration() {
        chromaticAberrationValue.text = chromaticAberration.value.ToString("F2");
        pp_chromaticAberration.intensity.Override(chromaticAberration.value);
    }
    public void SetVignetteIntensity() {
        vignetteIntensityValue.text = vignetteIntensity.value.ToString("F2");
        pp_vignette.intensity.Override(vignetteIntensity.value);
    }
    public void SetVignetteSmoothness() {
        vignetteSmoothnessValue.text = vignetteSmoothness.value.ToString("F2");
        pp_vignette.smoothness.Override(vignetteSmoothness.value);
    }
    public void SetVignetteRoundness() {
        vignetteRoundnessValue.text = vignetteRoundness.value.ToString("F2");
        pp_vignette.roundness.Override(vignetteRoundness.value);
    }

    public void Exit() {
        // We first unload to release memory taken by the level
        Application.Unload();
        // Then, we quit the simulation (exit the application)
        Application.Quit();
    }
}