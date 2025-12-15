using System;
using AccessibilityMod.Services;
using MelonLoader;
using UnityEngine;

namespace AccessibilityMod.Core
{
    public class AccessibilityMod : MelonMod
    {
        public static MelonLogger.Instance Logger { get; private set; }
        public static AccessibilityMod Instance { get; private set; }

        private static bool _isInitialized = false;

        public override void OnInitializeMelon()
        {
            Instance = this;
            Logger = LoggerInstance;
            Logger.Msg("Phoenix Wright Accessibility Mod initializing...");
        }

        private void InitializeAccessibility()
        {
            if (_isInitialized)
                return;

            try
            {
                // Initialize localization first (needed by other services)
                LocalizationService.Initialize();

                GameObject managerObject = new GameObject("AccessibilityMod_CoroutineRunner");
                managerObject.AddComponent<CoroutineRunner>();

                Logger.Msg("Accessibility systems initialized successfully");
                _isInitialized = true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Failed to initialize accessibility systems: {ex.Message}");
            }
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            Logger.Msg($"Scene loaded: {sceneName} (Index: {buildIndex})");

            // Initialize on first scene load when Unity is ready
            InitializeAccessibility();
        }

        public override void OnUpdate()
        {
            try
            {
                // Update navigators to detect mode changes
                UpdateNavigators();

                InputManager.ProcessInput();
            }
            catch (Exception ex)
            {
                Logger.Error($"Error in OnUpdate: {ex.Message}");
            }
        }

        private void UpdateNavigators()
        {
            PointingNavigator.Update();
            LuminolNavigator.Update();
            VasePuzzleNavigator.Update();
            FingerprintNavigator.Update();
            VideoTapeNavigator.Update();
            VaseShowNavigator.Update();
            DyingMessageNavigator.Update();
            BugSweeperNavigator.Update();
        }

        public override void OnDeinitializeMelon()
        {
            if (CoroutineRunner.Instance != null)
            {
                UnityEngine.Object.Destroy(CoroutineRunner.Instance.gameObject);
            }

            Logger.Msg("Phoenix Wright Accessibility Mod deinitialized.");
        }
    }
}
