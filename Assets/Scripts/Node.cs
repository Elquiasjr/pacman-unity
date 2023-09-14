using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public List<Vector2> availableDirections { get; private set;}

    private void Start(){
        this.availableDirections = new List<Vector2>();
        
        CheckAvaliableDirection(Vector2.up);
        CheckAvaliableDirection(Vector2.down);
        CheckAvaliableDirection(Vector2.left);
        CheckAvaliableDirection(Vector2.right);

    }

    private void CheckAvaliableDirection(Vector2 direction){
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, direction, 1.0f, this.obstacleLayer);
        if(hit.collider == null){
            this.availableDirections.Add(direction);
        }
    }
}
