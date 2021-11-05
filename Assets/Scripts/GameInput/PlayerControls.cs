// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""c03614c4-10c5-4083-85bb-1a2d018eb38d"",
            ""actions"": [
                {
                    ""name"": ""Horizontal"",
                    ""type"": ""Value"",
                    ""id"": ""bd573db1-8406-4ae1-8364-ba2ba9d7e140"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Vertical"",
                    ""type"": ""Button"",
                    ""id"": ""98a3a23c-e5f9-44af-af93-27b0323fe021"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""7bc5293b-0d7d-4c5c-875f-be265e50cd94"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4e0c5962-fcbc-48e0-826d-94ccd05f46a9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""397d1e76-d0b9-4ddc-b8f6-185195eb4715"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""Value"",
                    ""id"": ""e072129e-d28f-45dc-b034-be6923adedf2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MenuMovement"",
                    ""type"": ""Value"",
                    ""id"": ""8ce0e394-af37-4b26-8b7e-ebd0fdc0518e"",
                    ""expectedControlType"": ""Dpad"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MenuInteract"",
                    ""type"": ""Button"",
                    ""id"": ""2478ce14-5b57-47ee-85ab-159c139370e8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PauseL"",
                    ""type"": ""Button"",
                    ""id"": ""46c3b3ab-4557-4e85-886d-51ab520e42ed"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""1c00ebf7-d511-4797-994a-56a2a856da3c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MenuInteractMouse"",
                    ""type"": ""Button"",
                    ""id"": ""7d172ee7-1469-42f5-9c63-3ea688e56b9f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpecialMove"",
                    ""type"": ""Button"",
                    ""id"": ""2515f7ee-30fa-4c26-af47-c5ba12e8a81b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QuickSave"",
                    ""type"": ""Button"",
                    ""id"": ""54522605-db22-473a-a8d6-493ea1547620"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""QuickLoad"",
                    ""type"": ""Button"",
                    ""id"": ""3e831130-94da-4b33-9c26-a2bb6ada2908"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchAbility"",
                    ""type"": ""Button"",
                    ""id"": ""d2455f5c-bada-48a5-90e9-bc38362cd026"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ViewStats"",
                    ""type"": ""Button"",
                    ""id"": ""7d5b3d93-c329-461f-8fbb-fe163cc3e18e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""UseItem"",
                    ""type"": ""Button"",
                    ""id"": ""f7b46160-d3e1-4acc-9483-19f92f397fe7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DialoguesBack"",
                    ""type"": ""Button"",
                    ""id"": ""98e841e6-d02d-420a-8379-54cf96f7e253"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Horizontal"",
                    ""id"": ""405e175f-191a-42de-bc3b-9e67989010aa"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""8b3e47a3-9b2f-4baa-8ee1-e826bf88d6d3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""f2e3ddba-8dd3-400a-a1b6-0822ae9f1db0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Horizontal Joystick"",
                    ""id"": ""db87d491-1c1f-4369-b818-0fdd1fc83916"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""32bbf8ee-9764-4420-b541-850e0a3ba876"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""9d241c37-db79-40e1-8dd5-a0ad3caca712"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Horizontal DPad"",
                    ""id"": ""5f619de4-033a-42b1-a832-3df6684cd763"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""6235a76f-4692-48da-9a5c-eca8e96ca63d"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""753b9cf3-16ff-49d8-8451-c4903770e6a8"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Horizontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Vertical"",
                    ""id"": ""97bdd8d1-2fe2-4a68-bf42-64f6ef8e233a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""739d584a-f041-480d-85ec-283a35b4d432"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""4bb80c0c-e384-4577-af58-c521d02873c7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Vertical Joystick"",
                    ""id"": ""73f1d216-75d1-4112-8bdc-39bf3ea12316"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""342c34a4-aae5-4588-8a77-9710957426c8"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8d2134be-80a7-4686-9d20-911b848733e1"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Vertical DPad"",
                    ""id"": ""9d153cbf-1083-4998-afb5-e773986bbd20"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ea47055c-d44d-410f-99dd-5c55d0933353"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""5d9741da-16d4-4e1c-b681-24a99a3b6633"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Vertical"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""489adac4-3998-46eb-8248-813063badf88"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d7c5a4d4-5a45-4734-b0dc-33fa73da2afa"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e1cd06c-2893-475a-82c1-eb87e8da5f21"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1530185-c606-43b9-81dd-8872bc859517"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": ""MultiTap(tapTime=0.15,tapDelay=0.25,pressPoint=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""791ecd9e-9c7a-475a-a815-a6e8bf274b13"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b0fce9b-c33c-4c6e-894b-3790bfbbc165"",
                    ""path"": ""<DualShockGamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40f8e94f-ff79-4162-8759-035f3a2c6fe2"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""MultiTap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abbd7ef7-eacc-4968-97cf-fc80ff356373"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": ""MultiTap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2abfb5a1-e876-4463-aee0-88fc9631d03b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d47bd19-d01d-4dd8-976e-6983375cdc4c"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""CameraMove"",
                    ""id"": ""51291d18-ccec-4a55-8a80-51cb2755c301"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""aa456360-7fa0-48cf-8776-1b28fede4e69"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e416938f-e32e-4678-9d95-8cb3359fd25f"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1448b05a-c498-4f79-8667-a825ac99b6f2"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""090f9865-5a1f-446d-b2c8-05c115016397"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""DPad"",
                    ""id"": ""bcb9bd5d-8391-4a90-b467-bd69a2c4bf52"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""293143f4-9ac2-4864-9d61-cef1b174d852"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""27b71077-ceed-4649-805b-ae1fdbc09677"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""10f888ad-5c06-430c-8b59-28d66e26c09e"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e960cf05-efb0-43ae-a286-a40f1df90421"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Joystick"",
                    ""id"": ""140af464-429f-4d1c-a66e-8761efc31489"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6652a038-7c33-40b1-8671-f5797826778c"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d2b24631-447f-41a8-a8c0-60f2d1330c14"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""27975d01-3764-43eb-bf30-8be38ff194ef"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""223b37a1-d10e-433f-80ec-e676384b03ec"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""cad26ce2-f508-4145-b283-cff82298c8e5"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuInteract"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""de7206e7-1ac3-4db1-974a-ef5d952d0b98"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20d1e04f-6e66-4e2f-b657-64176069ecc6"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseL"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""78076907-119f-45cc-8755-582be6dd25ed"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c327cb9a-6292-4122-92bb-496f642bbee2"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34b54d77-5419-4ba7-9680-2cd678d5b58f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuInteractMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89095ac8-7acb-4a38-bbd1-2084dbd624e4"",
                    ""path"": ""<DualShockGamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpecialMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b61fd912-ccd2-42bc-afb5-90d11d52ba26"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpecialMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""44fc1253-5bb7-4045-ba17-f73fbba1739a"",
                    ""path"": ""<Keyboard>/f5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickSave"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4491e4b4-a761-435f-a202-7b57571b8938"",
                    ""path"": ""<Keyboard>/f9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""QuickLoad"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""334483ac-9b1d-4d8b-81a7-ab69d85bf591"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""324723f1-3a53-4076-af46-538c93b936ab"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fa80a2d4-85e9-4203-85cf-1888a43ee1cb"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e3fbacc-8892-4f44-b2e1-8986d793ae2b"",
                    ""path"": ""<Keyboard>/alt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ViewStats"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a80307a-e0cf-4b8e-bf0d-d66b6f74d2d2"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3106177-55d4-410a-867c-27c0b8576ec0"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e8fc0a7c-fb33-4aa8-b5f0-24e2f17950be"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DialoguesBack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Horizontal = m_Gameplay.FindAction("Horizontal", throwIfNotFound: true);
        m_Gameplay_Vertical = m_Gameplay.FindAction("Vertical", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Roll = m_Gameplay.FindAction("Roll", throwIfNotFound: true);
        m_Gameplay_Attack = m_Gameplay.FindAction("Attack", throwIfNotFound: true);
        m_Gameplay_Camera = m_Gameplay.FindAction("Camera", throwIfNotFound: true);
        m_Gameplay_MenuMovement = m_Gameplay.FindAction("MenuMovement", throwIfNotFound: true);
        m_Gameplay_MenuInteract = m_Gameplay.FindAction("MenuInteract", throwIfNotFound: true);
        m_Gameplay_PauseL = m_Gameplay.FindAction("PauseL", throwIfNotFound: true);
        m_Gameplay_Interact = m_Gameplay.FindAction("Interact", throwIfNotFound: true);
        m_Gameplay_MenuInteractMouse = m_Gameplay.FindAction("MenuInteractMouse", throwIfNotFound: true);
        m_Gameplay_SpecialMove = m_Gameplay.FindAction("SpecialMove", throwIfNotFound: true);
        m_Gameplay_QuickSave = m_Gameplay.FindAction("QuickSave", throwIfNotFound: true);
        m_Gameplay_QuickLoad = m_Gameplay.FindAction("QuickLoad", throwIfNotFound: true);
        m_Gameplay_SwitchAbility = m_Gameplay.FindAction("SwitchAbility", throwIfNotFound: true);
        m_Gameplay_ViewStats = m_Gameplay.FindAction("ViewStats", throwIfNotFound: true);
        m_Gameplay_UseItem = m_Gameplay.FindAction("UseItem", throwIfNotFound: true);
        m_Gameplay_DialoguesBack = m_Gameplay.FindAction("DialoguesBack", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Horizontal;
    private readonly InputAction m_Gameplay_Vertical;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Roll;
    private readonly InputAction m_Gameplay_Attack;
    private readonly InputAction m_Gameplay_Camera;
    private readonly InputAction m_Gameplay_MenuMovement;
    private readonly InputAction m_Gameplay_MenuInteract;
    private readonly InputAction m_Gameplay_PauseL;
    private readonly InputAction m_Gameplay_Interact;
    private readonly InputAction m_Gameplay_MenuInteractMouse;
    private readonly InputAction m_Gameplay_SpecialMove;
    private readonly InputAction m_Gameplay_QuickSave;
    private readonly InputAction m_Gameplay_QuickLoad;
    private readonly InputAction m_Gameplay_SwitchAbility;
    private readonly InputAction m_Gameplay_ViewStats;
    private readonly InputAction m_Gameplay_UseItem;
    private readonly InputAction m_Gameplay_DialoguesBack;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Horizontal => m_Wrapper.m_Gameplay_Horizontal;
        public InputAction @Vertical => m_Wrapper.m_Gameplay_Vertical;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Roll => m_Wrapper.m_Gameplay_Roll;
        public InputAction @Attack => m_Wrapper.m_Gameplay_Attack;
        public InputAction @Camera => m_Wrapper.m_Gameplay_Camera;
        public InputAction @MenuMovement => m_Wrapper.m_Gameplay_MenuMovement;
        public InputAction @MenuInteract => m_Wrapper.m_Gameplay_MenuInteract;
        public InputAction @PauseL => m_Wrapper.m_Gameplay_PauseL;
        public InputAction @Interact => m_Wrapper.m_Gameplay_Interact;
        public InputAction @MenuInteractMouse => m_Wrapper.m_Gameplay_MenuInteractMouse;
        public InputAction @SpecialMove => m_Wrapper.m_Gameplay_SpecialMove;
        public InputAction @QuickSave => m_Wrapper.m_Gameplay_QuickSave;
        public InputAction @QuickLoad => m_Wrapper.m_Gameplay_QuickLoad;
        public InputAction @SwitchAbility => m_Wrapper.m_Gameplay_SwitchAbility;
        public InputAction @ViewStats => m_Wrapper.m_Gameplay_ViewStats;
        public InputAction @UseItem => m_Wrapper.m_Gameplay_UseItem;
        public InputAction @DialoguesBack => m_Wrapper.m_Gameplay_DialoguesBack;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Horizontal.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHorizontal;
                @Horizontal.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHorizontal;
                @Horizontal.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnHorizontal;
                @Vertical.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnVertical;
                @Vertical.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnVertical;
                @Vertical.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnVertical;
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Roll.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRoll;
                @Attack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Camera.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCamera;
                @MenuMovement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMenuMovement;
                @MenuMovement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMenuMovement;
                @MenuMovement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMenuMovement;
                @MenuInteract.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMenuInteract;
                @MenuInteract.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMenuInteract;
                @MenuInteract.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMenuInteract;
                @PauseL.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPauseL;
                @PauseL.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPauseL;
                @PauseL.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPauseL;
                @Interact.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @MenuInteractMouse.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMenuInteractMouse;
                @MenuInteractMouse.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMenuInteractMouse;
                @MenuInteractMouse.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMenuInteractMouse;
                @SpecialMove.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpecialMove;
                @SpecialMove.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpecialMove;
                @SpecialMove.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSpecialMove;
                @QuickSave.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickSave;
                @QuickSave.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickSave;
                @QuickSave.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickSave;
                @QuickLoad.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickLoad;
                @QuickLoad.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickLoad;
                @QuickLoad.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnQuickLoad;
                @SwitchAbility.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchAbility;
                @SwitchAbility.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchAbility;
                @SwitchAbility.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchAbility;
                @ViewStats.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnViewStats;
                @ViewStats.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnViewStats;
                @ViewStats.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnViewStats;
                @UseItem.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUseItem;
                @UseItem.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUseItem;
                @UseItem.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnUseItem;
                @DialoguesBack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDialoguesBack;
                @DialoguesBack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDialoguesBack;
                @DialoguesBack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDialoguesBack;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Horizontal.started += instance.OnHorizontal;
                @Horizontal.performed += instance.OnHorizontal;
                @Horizontal.canceled += instance.OnHorizontal;
                @Vertical.started += instance.OnVertical;
                @Vertical.performed += instance.OnVertical;
                @Vertical.canceled += instance.OnVertical;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
                @MenuMovement.started += instance.OnMenuMovement;
                @MenuMovement.performed += instance.OnMenuMovement;
                @MenuMovement.canceled += instance.OnMenuMovement;
                @MenuInteract.started += instance.OnMenuInteract;
                @MenuInteract.performed += instance.OnMenuInteract;
                @MenuInteract.canceled += instance.OnMenuInteract;
                @PauseL.started += instance.OnPauseL;
                @PauseL.performed += instance.OnPauseL;
                @PauseL.canceled += instance.OnPauseL;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @MenuInteractMouse.started += instance.OnMenuInteractMouse;
                @MenuInteractMouse.performed += instance.OnMenuInteractMouse;
                @MenuInteractMouse.canceled += instance.OnMenuInteractMouse;
                @SpecialMove.started += instance.OnSpecialMove;
                @SpecialMove.performed += instance.OnSpecialMove;
                @SpecialMove.canceled += instance.OnSpecialMove;
                @QuickSave.started += instance.OnQuickSave;
                @QuickSave.performed += instance.OnQuickSave;
                @QuickSave.canceled += instance.OnQuickSave;
                @QuickLoad.started += instance.OnQuickLoad;
                @QuickLoad.performed += instance.OnQuickLoad;
                @QuickLoad.canceled += instance.OnQuickLoad;
                @SwitchAbility.started += instance.OnSwitchAbility;
                @SwitchAbility.performed += instance.OnSwitchAbility;
                @SwitchAbility.canceled += instance.OnSwitchAbility;
                @ViewStats.started += instance.OnViewStats;
                @ViewStats.performed += instance.OnViewStats;
                @ViewStats.canceled += instance.OnViewStats;
                @UseItem.started += instance.OnUseItem;
                @UseItem.performed += instance.OnUseItem;
                @UseItem.canceled += instance.OnUseItem;
                @DialoguesBack.started += instance.OnDialoguesBack;
                @DialoguesBack.performed += instance.OnDialoguesBack;
                @DialoguesBack.canceled += instance.OnDialoguesBack;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnHorizontal(InputAction.CallbackContext context);
        void OnVertical(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnMenuMovement(InputAction.CallbackContext context);
        void OnMenuInteract(InputAction.CallbackContext context);
        void OnPauseL(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnMenuInteractMouse(InputAction.CallbackContext context);
        void OnSpecialMove(InputAction.CallbackContext context);
        void OnQuickSave(InputAction.CallbackContext context);
        void OnQuickLoad(InputAction.CallbackContext context);
        void OnSwitchAbility(InputAction.CallbackContext context);
        void OnViewStats(InputAction.CallbackContext context);
        void OnUseItem(InputAction.CallbackContext context);
        void OnDialoguesBack(InputAction.CallbackContext context);
    }
}
