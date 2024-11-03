using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastSocket : MonoBehaviour
{
    public GameObject existingSocket;
    public string socketColor;
    [SerializeField] private GameManager gameManager;

    bool chosen;
    bool posChange;
    bool socketEntered;

    GameObject movementPos;
    GameObject socketItSelf;

    public void MovementProcedures(string process, GameObject Socket, GameObject TargetObje = null)
    {
        switch (process)
        {
            case "ChosenPosition":
                movementPos = TargetObje;
                chosen = true;
                break;

            case "ChangePosition":
                socketItSelf = Socket;
                movementPos = TargetObje;
                posChange = true;
                break;

            case "SocketReturn":
                socketItSelf = Socket;
                socketEntered = true;
                break;
        }
    }

    void Update()
    {
        if (chosen)
        {
            transform.position = Vector3.Lerp(transform.position, movementPos.transform.position, .1f);
            if (Vector3.Distance(transform.position, movementPos.transform.position) < .0010f)
            {
                chosen = false;

            }

        }

        if (posChange)
        {
            transform.position = Vector3.Lerp(transform.position, movementPos.transform.position, .1f);
            if (Vector3.Distance(transform.position, movementPos.transform.position) < .0010f)
            {
                posChange = false;
                socketEntered = true;

            }

        }
        if (socketEntered)
        {
            transform.position = Vector3.Lerp(transform.position, socketItSelf.transform.position, .1f);

            if (Vector3.Distance(transform.position, socketItSelf.transform.position) < .010f)
            {
                socketEntered = false;
                gameManager.thereIsMoment = false;
                existingSocket = socketItSelf;

                gameManager.SocketControl();
            }

        }

    }
}
