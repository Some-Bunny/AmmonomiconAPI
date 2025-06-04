using BepInEx;
using Alexandria;
using Alexandria.ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.Collections;
using System.Reflection;
using AmmonomiconAPI.Code;
using static AmmonomiconAPI.Code.ExampleSetup;

namespace AmmonomiconAPI
{
    [BepInDependency(Alexandria.Alexandria.GUID)] 
    [BepInDependency(ETGModMainBehaviour.GUID)]
    [BepInPlugin(GUID, NAME, VERSION)]
    public class AmmonomiconAPIModule : BaseUnityPlugin
    {
        public const string GUID = "bobot_and_bunny.etg.ammonomiconapi";
        public const string NAME = "[[ AmmonomiconAPI ]]";
        public const string VERSION = "1.0.2";
        public const string TEXT_COLOR = "#FF0000";

        public void Start()
        {
            new Harmony(GUID).PatchAll();
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
        }

        public void GMStart(GameManager g)
        {
            StaticData.InitStaticData();
            AmmonomiconAPIHooks.Init();
            //UIBuilder.BuildBookmark("ExamplePrefix", "ExampleEnumNames", new ExampleCustomAmmonomiconPageController(), null, Assembly.GetExecutingAssembly());
            Log($"{NAME} v{VERSION} started successfully.", TEXT_COLOR);
        }

        public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }
    }
}
