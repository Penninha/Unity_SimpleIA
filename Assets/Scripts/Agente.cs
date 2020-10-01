using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agente : MonoBehaviour
{
    public GameObject currentlyTarget;
    public GameObject box;

    public Material green;
    public Material yellow;

    private MeshRenderer rend;
    private bool canMove;
    private bool canPickBox;
    private bool goBackToOrigin;
    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        initialPos = transform.position;
        StartCoroutine(WaitForStart());
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            MoveToBox();
        }

        if(goBackToOrigin)
        {
            MoveToOrigin();
        }
    }

    void MoveToBox()
    {
        transform.LookAt(new Vector3(currentlyTarget.transform.position.x, transform.position.y, currentlyTarget.transform.position.z));
        transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(
                    currentlyTarget.transform.position.x,
                    initialPos.y,
                    currentlyTarget.transform.position.z
                ),
                10f * Time.deltaTime
            );
    }

    void MoveToOrigin()
    {
        transform.LookAt(initialPos);
        transform.position = Vector3.MoveTowards(
                transform.position,
                initialPos,
                10f * Time.deltaTime
            );

        if (transform.position == initialPos)
        {
            goBackToOrigin = false;
            rend.material = yellow;
            StartCoroutine(WaitForStart());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Contains("Plane"))
        {
            ReleaseBox();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.CompareTag("Cube") && canPickBox)
        {
            GoBackWithBox(other.gameObject);
        }
    }

    IEnumerator WaitForStart()
    {
        yield return new WaitForSeconds(3f);
        int randomBox = Spawner.boxSpawned.Count;

        if (randomBox > 0)
        {
            rend.material = green;
            FindBox();
        }
        else
        {
            StartCoroutine(WaitForStart());
        }
    }

    private void FindBox()
    {
        int randomBox = Spawner.boxSpawned.Count;
        if(randomBox > 0)
        {
            currentlyTarget = Spawner.boxSpawned[randomBox > 1 ? Random.Range(0, randomBox) : 0];
            GoToBox();
        }
    }

    private void GoToBox()
    {
        canMove = true;
        canPickBox = true;
    }

    private void GoBackWithBox(GameObject checker)
    {
        if(checker == currentlyTarget)
        {
            canMove = false;
            canPickBox = false;
            box = currentlyTarget;
            box.transform.parent = this.transform;

            if(box.name == "Red")
            {
                currentlyTarget = GameObject.FindGameObjectWithTag("RedPlane");            
            } else
            {
                currentlyTarget = GameObject.FindGameObjectWithTag("BluePlane");
            }

            canMove = true;
        }
    }

    private void ReleaseBox()
    {
        canMove = false;

        if(box)
        {
            box.transform.parent = null;
            Spawner.boxSpawned.Remove(box);
        }

        int randomBox = Spawner.boxSpawned.Count;
        if(randomBox > 0)
        {
            FindBox();
        }
        else
        {
            GoBackToOrigin();
        }
    }

    private void GoBackToOrigin()
    {
        goBackToOrigin = true;
    }
}
