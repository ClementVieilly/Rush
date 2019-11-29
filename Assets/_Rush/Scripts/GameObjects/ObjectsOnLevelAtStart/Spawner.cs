///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 30/10/2019 12:52
///-----------------------------------------------------------------

using System;
using System.Collections.Generic;
using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsOnLevelAtStart;
using Com.IsartDigital.Rush.GameObjects.ObjectsInstanciate;
using Com.IsartDigital.Rush.Manager;
using Pixelplacement;
using UnityEngine;

namespace Com.IsartDigital.Rush.GameObjects.ObjectsOnLevelAtStart
{
    public class Spawner : ObjectsOnLevelAtStartScript
    {
        [SerializeField] private GameObject cubePrefab;
        [SerializeField] private uint spawnFrequence = 4;
        [SerializeField] private uint spawnNumber;
        [SerializeField] private uint alias;
        [SerializeField] private ParticleSystem spawnParticle;
        [SerializeField] private ParticleSystemRenderer spawnParticleRenderer;
        [SerializeField] private AnimationCurve spawnAnim;
        private Material color;
        private int frequencyCounter;
        [SerializeField] private int startSpwan;
        private int spawnCounter = 0;
        private static List<Spawner> list = new List<Spawner>();

        public static void EmptySpawner() {
            Spawner lSpawn;
            for(int i = list.Count - 1; i >= 0; i--) {
                lSpawn = list[i];
                lSpawn.spawnCounter = 0;
                lSpawn.frequencyCounter = lSpawn.startSpwan;
            }
        }
        public static void PlaySpawnParticles() {
            for(int i = list.Count - 1; i >= 0; i--) {
                list[i].spawnParticle.Play();
            }
        }

        public static void InitAll() {
            Spawner lSpawn;

            for(int i = list.Count - 1; i >= 0; i--) {
                lSpawn = list[i];
                lSpawn.Init();
            }
        }
        public override void Init() {
            base.Init();
            list.Add(this);
            TimeManager.OnTick += TimeManager_OnTick;
            color = transform.GetChild(0).GetComponent<Renderer>().material;
            frequencyCounter = startSpwan;
            spawnParticleRenderer.material = color;
            spawnParticle = Instantiate(spawnParticle, transform);
        }

        private void TimeManager_OnTick() {
            spawnParticle.Stop();
            if(frequencyCounter > spawnFrequence && spawnCounter < spawnNumber) {
                frequencyCounter = 0;
                GameObject go;
                if(alias == 0) {

                    go = Instantiate(cubePrefab, transform.position + new Vector3(0, 2.5f, 0) - transform.forward, Quaternion.identity);
                    go.transform.localScale = Vector3.zero;
                    Tween.LocalScale(go.transform, Vector3.one * 0.8f, 0.2f / TimeManager.Speed, 0);

                    Tween.LocalPosition(go.transform, transform.position + new Vector3(0, 1f / 2.3f, 0), 0.2f / TimeManager.Speed, 0.2f / TimeManager.Speed, Tween.EaseInStrong);

                    Tween.LocalRotation(go.transform, Quaternion.AngleAxis(90f, transform.right), 0.15f / TimeManager.Speed, 0.4f / TimeManager.Speed);

                    Tween.LocalScale(go.transform, new Vector3(0.8f, 1.2f, 0.6f), 0.15f / TimeManager.Speed, 0.55f / TimeManager.Speed, spawnAnim);

                    Tween.LocalPosition(go.transform, transform.position + new Vector3(0, 1f / 2f, 0), 0.24f / TimeManager.Speed, 0.7f / TimeManager.Speed, Tween.EaseInStrong);

                    Tween.LocalScale(go.transform, Vector3.one * 0.8f, 0.05f / TimeManager.Speed, 0.94f / TimeManager.Speed, Tween.EaseIn);
                    Tween.LocalRotation(go.transform, transform.rotation, 0.01f / TimeManager.Speed, 0.99f / TimeManager.Speed, null, Tween.LoopType.None, null, go.GetComponent<CubeMove>().Init);
                }
                else {
                    go = Instantiate(cubePrefab, transform.position + new Vector3(0, 2.5f, 0), transform.rotation);
                    go.transform.localScale = Vector3.zero;
                    Tween.LocalScale(go.transform, Vector3.one * 0.8f, 0.5f / TimeManager.Speed, 0);

                    Tween.LocalPosition(go.transform, transform.position + new Vector3(0, 1f / 2.3f, 0), 0.5f / TimeManager.Speed, 0.5f / TimeManager.Speed, Tween.EaseInStrong, Tween.LoopType.None, null, go.GetComponent<CubeMove>().Init);
                }

                go.GetComponent<Renderer>().material = color;
                go.GetComponent<CubeMove>().alias = alias;

                spawnCounter++;
            }
            frequencyCounter++;
        }

        private void TweenCallBack() { }
        private void OnDestroy() {
            base.Destroy();
            TimeManager.OnTick -= TimeManager_OnTick;
            list.RemoveAt(list.IndexOf(this));
        }



    }
}