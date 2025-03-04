﻿using UnityEngine;
using Valve.VR;
using WindowsInput;
using Plugin.VRTRAKILL.Config;
using Plugin.VRTRAKILL.Config.Input;

namespace Plugin.VRTRAKILL.Input
{
    static class VRInputManager
    {
        private static InputSimulator InpSim => new InputSimulator();

        private static bool
            Jump = false,
            Dash = false,
            Slide = false;

        private static bool
            RHPrimaryFire = false,
            RHAltFire = false,
            ChangeWeaponVariation = false,
            OpenWeaponWheel = false;

        private static bool
            Punch = false,
            SwapHand = false,
            Whiplash = false;

        private static bool
            Slot0 = false,
            Slot1 = false, Slot2 = false, Slot3 = false,
            Slot4 = false, Slot5 = false, Slot6 = false,
            Slot7 = false, Slot8 = false, Slot9 = false;

        private static bool Escape;

        public static void Init()
        {
            // Movement
            SteamVR_Actions._default.Movement.AddOnUpdateListener(MovementH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Turn.AddOnUpdateListener(TurnH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Jump.AddOnUpdateListener(JumpH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slide.AddOnUpdateListener(SlideH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Dash.AddOnUpdateListener(DashH, SteamVR_Input_Sources.Any);

            // Weapons
            SteamVR_Actions._default.Shoot.AddOnUpdateListener(LHShootH, SteamVR_Input_Sources.LeftHand);
            SteamVR_Actions._default.AltShoot.AddOnUpdateListener(LHAltShootH, SteamVR_Input_Sources.LeftHand);
            SteamVR_Actions._default.Shoot.AddOnUpdateListener(RHShootH, SteamVR_Input_Sources.RightHand);
            SteamVR_Actions._default.AltShoot.AddOnUpdateListener(RHAltShootH, SteamVR_Input_Sources.RightHand);
            SteamVR_Actions._default.IterateWeapon.AddOnUpdateListener(IterateWeaponH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.ChangeWeaponVariation.AddOnUpdateListener(ChangeWeaponVariationH, SteamVR_Input_Sources.Any);
            // Weapon quick switch, open weapon wheel
            SteamVR_Actions._default.OpenWeaponWheel.AddOnUpdateListener(OpenWeaponWheelH, SteamVR_Input_Sources.Any);

            SteamVR_Actions._default.Slot0.AddOnUpdateListener(Slot0H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot1.AddOnUpdateListener(Slot1H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot2.AddOnUpdateListener(Slot2H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot2.AddOnUpdateListener(Slot2H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot3.AddOnUpdateListener(Slot3H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot4.AddOnUpdateListener(Slot4H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot5.AddOnUpdateListener(Slot5H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot6.AddOnUpdateListener(Slot6H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot7.AddOnUpdateListener(Slot7H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot8.AddOnUpdateListener(Slot8H, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Slot9.AddOnUpdateListener(Slot9H, SteamVR_Input_Sources.Any);

            SteamVR_Actions._default.SwapHand.AddOnUpdateListener(SwapHandH, SteamVR_Input_Sources.Any);
            SteamVR_Actions._default.Whiplash.AddOnUpdateListener(WhiplashH, SteamVR_Input_Sources.Any);

            SteamVR_Actions._default.Escape.AddOnUpdateListener(EscapeH, SteamVR_Input_Sources.Any);
        }

        private static void MovementH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        { VRInputVars.MoveVector = axis; }
        private static void TurnH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        {
            VRInputVars.TurnVector = axis;
            if (axis.x > 0 + Vars.Config.VRInputSettings.Deadzone) VRInputVars.TurnOffset += Vars.Config.VRInputSettings.SmoothTurningSpeed * Time.deltaTime;
            if (axis.x < 0 - Vars.Config.VRInputSettings.Deadzone) VRInputVars.TurnOffset -= Vars.Config.VRInputSettings.SmoothTurningSpeed * Time.deltaTime;
        }

        private static void JumpH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Jump) { Jump = newState; TriggerKey(ConfigMaster.Jump, Jump, !Jump); } }
        private static void SlideH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slide) { Slide = newState; TriggerKey(ConfigMaster.Slide, Slide, !Slide); } }
        private static void DashH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Dash) { Dash = newState; TriggerKey(ConfigMaster.Dash, Dash, !Dash); } }

        // Left controllers Shoot and AltShoot handle Punching and Hand swapping
        private static void LHShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != Punch)
            {
                Punch = newState;
                if (Vars.IsAMenu) LMBPress(Punch, !Punch);
                else InputManager.Instance.InputSource.Punch.Trigger(Punch, !Punch);
            }
        }
        private static void LHAltShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != SwapHand) { SwapHand = newState; TriggerKey(ConfigMaster.SwapHand, SwapHand, !SwapHand); } }

        // Right controllers Shoot and AltShoot handle Weapon shooting and Alternative fire
        private static void RHShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != RHPrimaryFire)
            {
                RHPrimaryFire = newState;
                if (Vars.IsSandboxArmActive) LMBPress(RHPrimaryFire, !RHPrimaryFire);
                else InputManager.Instance.InputSource.Fire1.Trigger(RHPrimaryFire, !RHPrimaryFire);
            }
        }
        private static void RHAltShootH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != RHAltFire)
            {
                RHAltFire = newState;
                if (Vars.IsSandboxArmActive) RMBPress(RHAltFire, !RHAltFire);
                else InputManager.Instance.InputSource.Fire2.Trigger(RHAltFire, !RHAltFire);
            }
        }

        private static void IterateWeaponH(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta)
        {
            if (Vars.IsWeaponWheelPresent) return;
            if (axis.y > 0 + Vars.Config.VRInputSettings.Deadzone * 1.5f) MouseScroll(-1);
            if (axis.y < 0 - Vars.Config.VRInputSettings.Deadzone * 1.5f) MouseScroll(1);
        }
        private static void ChangeWeaponVariationH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != ChangeWeaponVariation) { ChangeWeaponVariation = newState; TriggerKey(ConfigMaster.ChangeWeaponVariation, ChangeWeaponVariation, !ChangeWeaponVariation); } }
        // Handles quick swapping (ex. from pistol to railcannon, etc.) and weapon wheel
        private static void OpenWeaponWheelH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (newState != OpenWeaponWheel) { OpenWeaponWheel = newState; TriggerKey(ConfigMaster.LastWeaponUsed, OpenWeaponWheel, !OpenWeaponWheel); }
        }

        private static void SwapHandH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != SwapHand) { SwapHand = newState; TriggerKey(ConfigMaster.SwapHand, SwapHand, !SwapHand); } }
        private static void WhiplashH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Whiplash) { Whiplash = newState; TriggerKey(ConfigMaster.Whiplash, Whiplash, !Whiplash); } }

        private static void Slot0H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot0) { Slot0 = newState; TriggerKey(ConfigMaster.Slot0, Slot0, !Slot0); } }
        private static void Slot1H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot1) { Slot1 = newState; TriggerKey(ConfigMaster.Slot1, Slot1, !Slot1); } }
        private static void Slot2H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot2) { Slot2 = newState; TriggerKey(ConfigMaster.Slot2, Slot2, !Slot2); } }
        private static void Slot3H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot3) { Slot3 = newState; TriggerKey(ConfigMaster.Slot3, Slot3, !Slot3); } }
        private static void Slot4H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot4) { Slot4 = newState; TriggerKey(ConfigMaster.Slot4, Slot4, !Slot4); } }
        private static void Slot5H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot5) { Slot5 = newState; TriggerKey(ConfigMaster.Slot5, Slot5, !Slot5); } }
        private static void Slot6H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot6) { Slot6 = newState; TriggerKey(ConfigMaster.Slot6, Slot6, !Slot6); } }
        private static void Slot7H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot7) { Slot7 = newState; TriggerKey(ConfigMaster.Slot7, Slot7, !Slot7); } }
        private static void Slot8H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot8) { Slot8 = newState; TriggerKey(ConfigMaster.Slot8, Slot8, !Slot8); } }
        private static void Slot9H(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Slot9) { Slot9 = newState; TriggerKey(ConfigMaster.Slot9, Slot9, !Slot9); } }

        private static void EscapeH(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        { if (newState != Escape) { Escape = newState; TriggerKey(ConfigMaster.Escape, Escape, !Escape); } }

        // Simulate keyboard input
        private static void TriggerKey(WindowsInput.Native.VirtualKeyCode KeyCode, bool Started, bool Ended)
        {
            if (Started) InpSim.Keyboard.KeyDown(KeyCode);
            else if (Ended) InpSim.Keyboard.KeyUp(KeyCode);
        }

        // Simulate mouse input
        private static void LMBPress(bool Started, bool Ended)
        {
            if (Started) InpSim.Mouse.LeftButtonDown();
            else if (Ended) InpSim.Mouse.LeftButtonUp();
        }
        private static void RMBPress(bool Started, bool Ended)
        {
            if (Started) InpSim.Mouse.RightButtonDown();
            else if (Ended) InpSim.Mouse.RightButtonUp();
        }
        private static void MouseScroll(int Amount)
        {
            InpSim.Mouse.VerticalScroll(Amount);
        }

        // Unity.InputSystem input trigger
        public static void Trigger(this InputActionState state, bool started, bool cancelled)
        {
            if (started)
            {
                state.IsPressed = true;
                state.PerformedFrame = Time.frameCount;
                state.PerformedTime = Time.time;
            }
            else if (cancelled)
            {
                state.IsPressed = false;
                state.CanceledFrame = Time.frameCount;
            }
        }
    }
}