///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 22/10/2019 18:54
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts.GameObjects.ObjectsOnLevelAtStart;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Rush.GameObjects.ObjectsOnLevelAtStart {
	public class Teleport : ObjectsOnLevelAtStartScript {
        [SerializeField] public Transform pair;
        [SerializeField] private ParticleSystem particles;
        public static List<Transform> transformList = new List<Transform>();
        public static List<Teleport> list = new List<Teleport>(); 

        public static void DestroyAll() {
            Teleport lTeleport;
            for(int i = 0; i < list.Count; i++) {
                lTeleport = list[i];
                lTeleport.Destroy(); 
            }
        }

        public static void InitAll() {
            Teleport lTeleport;
            for(int i = 0; i < transformList.Count; i++) {
                lTeleport = list[i];
                lTeleport.Init();
                lTeleport.InitParticle(); 
            }
        }
        private void InitParticle() {
           
        }
        private void Awake() {
            list.Add(this);
            transformList.Add(transform);
            particles = Instantiate(particles, transform);
        }
        public override void Destroy() {
            list.RemoveAt(list.IndexOf(this));
            transformList.RemoveAt(transformList.IndexOf(transform));
            base.Destroy();
        }
        
    }
}