using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Collections.Generic;

public class PlayerCamera : MonoBehaviour {
    [SerializeField]
    public List<Region> regions;
    public GameObject player;
    public Camera camera;

    public void Update() {
        foreach(var reg in regions){
            var playerPosition = player.transform.position;

            if (reg.Contains(playerPosition.x, playerPosition.y)) {
                camera.transform.position = new Vector3(reg.cameraX, reg.cameraY, camera.transform.position.z);
            }
        }
    }
}
