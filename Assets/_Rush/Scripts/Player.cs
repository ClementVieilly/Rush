///-----------------------------------------------------------------
/// Author : Clément VIEILLY
/// Date : 06/11/2019 09:42
///-----------------------------------------------------------------

using Com.IsartDigital.Assets._Rush.Scripts;
using Com.IsartDigital.Rush.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.IsartDigital.Rush {
	public class Player : MonoBehaviour {
        public static  List<Inventory> inventory = new List<Inventory>();
        protected GameObject currentTile;
        protected Transform currentPosition; 
        protected int index = 0;
       [SerializeField] protected Camera cam;
        protected bool hitSomething;
        protected RaycastHit hitInfo; 
       private void Start () {
            currentTile = inventory[0].Tile;
            
            //currentTile =  Instantiate(currentTile); 
            //ControllerManager.OnMouse0Down += ControllerManager_OnMouse0Down; 
		}

       /* private void ControllerManager_OnMouse0Down(float axeX, float axeY) {

            if(hitInfo.collider.gameObject.layer == 8) {
                inventory.Add(hitInfo.collider.gameObject);
                currentTile = hitInfo.collider.gameObject;
                currentTile.layer = 2; 
                return;
            }
            currentTile.transform.position = hitInfo.collider.gameObject.transform.position + Vector3.up / 2;
            inventory.Remove(currentTile); 
           
            currentTile.layer = 8;
           
            currentTile = null;
            currentTile = Instantiate(inventory[0]);

             
        }

        private void Update () {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            hitSomething = Physics.Raycast(ray, out hitInfo, 30);
            currentTile.layer = 2;
            if(inventory.Count != 0 && hitSomething) {

                if(hitInfo.collider.gameObject.layer == 8) return; 
                currentTile.transform.position = hitInfo.collider.gameObject.transform.position + Vector3.up / 2;
                currentTile.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;

            }

        }*/


	}
}