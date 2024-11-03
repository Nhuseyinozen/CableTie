using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    GameObject selectedObje;
    GameObject selectedSocket;
    public bool thereIsMoment;

    [Header("LEVEL SETTINGS")]
    [SerializeField] private GameObject[] CollisionControlObjects;
    [SerializeField] private GameObject[] Sockets;
    [SerializeField] private int TargetSocketCount;
    [SerializeField] private List<bool> CollisionStatus;

    [Header("UI SETTINGS")]
    [SerializeField] private GameObject ControlPanel;
    [SerializeField] private TextMeshProUGUI ControlText;

    [Header("OTHER OBJECT")]
    [SerializeField] private GameObject[] lights;

    int CompletedCount;
    int CollisionObjectCount;
    LastSocket _LastSocket;






    void Start()
    {

        for (int i = 0; i < TargetSocketCount - 1; i++)
        {
            CollisionStatus.Add(false);
        }


    }



    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 200))
            {

                if (hit.collider != null)
                {
                    // LASTSOCKET
                    if (!thereIsMoment && selectedObje == null)
                    {
                        if (hit.collider.CompareTag("MaviFis") || hit.collider.CompareTag("KýrmýzýFis") || hit.collider.CompareTag("YesilFis"))
                        {
                            _LastSocket = hit.collider.GetComponent<LastSocket>();

                            _LastSocket.MovementProcedures("ChosenPosition", _LastSocket.existingSocket, _LastSocket.existingSocket.GetComponent<Socket>().movementPos);

                            // Soket seçildikten sonra tekrar seçilmemesi için kontrol ediyoruz.
                            selectedObje = hit.collider.gameObject;
                            selectedSocket = _LastSocket.existingSocket;
                            thereIsMoment = true;
                        }
                    }
                    // LASTSOCKET

                    //SOCKET 

                    if (hit.collider.CompareTag("Socket"))
                    {
                        if (selectedObje != null && !hit.collider.GetComponent<Socket>().fullness &&
                       selectedSocket != hit.collider.gameObject)
                        {

                            selectedSocket.GetComponent<Socket>().fullness = false;
                            Socket _Socket = hit.collider.GetComponent<Socket>();

                            _LastSocket.MovementProcedures("ChangePosition", hit.collider.gameObject, _Socket.movementPos);
                            _Socket.fullness = true;

                            selectedObje = null;
                            selectedSocket = null;
                            thereIsMoment = true;
                        }
                        else if (selectedSocket == hit.collider.gameObject)
                        {
                            _LastSocket.MovementProcedures("SocketReturn", hit.collider.gameObject);
                            selectedObje = null;
                            selectedSocket = null;
                            thereIsMoment = true;

                        }
                    }
                    //SOCKET
                }
            }

        }
    }

    public void SocketControl()
    {
        foreach (var item in Sockets)
        {
            if (item.GetComponent<LastSocket>().socketColor == item.GetComponent<LastSocket>().existingSocket.name)
            {
                CompletedCount++;
            }
        }

        if (CompletedCount == TargetSocketCount)
        {
            Debug.Log("Tüm soketler yerinde");

            foreach (var item in CollisionControlObjects)
            {
                item.SetActive(true);
            }

            StartCoroutine(CollisionObjectControl());

        }
        else
        {
            Debug.Log("Eþleþme tamamlandý.");
        }

        CompletedCount = 0;

    }


    public void CollisionControl(int CollisionIndex, bool status)
    {
        CollisionStatus[CollisionIndex] = status;

    }

    IEnumerator CollisionObjectControl()
    {

        Debug.Log("Kontrol ediliyor.");

        lights[0].SetActive(false);
        lights[1].SetActive(true);


        ControlPanel.SetActive(true);
        ControlText.text = "Being checked...";

        yield return new WaitForSeconds(4f);

        foreach (var item in CollisionStatus)
        {

            // Liste içibdeki obje durumlarý true olan yani çarpýþma olmayan objelerin sayýsýný alýr.
            if (item)
                CollisionObjectCount++;
        }

        if (CollisionObjectCount == CollisionStatus.Count)
        {
            Debug.Log("Kazandýnýz");

            ControlText.text = "YOU WIN !";

            lights[1].SetActive(false);
            lights[2].SetActive(true);
        }
        else
        {
            lights[1].SetActive(false);
            lights[0].SetActive(true);

            ControlText.text = "The cables are touching each other!";


            Debug.Log("Çarpma var");

            Invoke("PanelClose", 2f);

            foreach (var item in CollisionControlObjects)
            {
                item.SetActive(false);
            }

            
        }

        CollisionObjectCount = 0;

    }

    void PanelClose()
    {

        ControlPanel.SetActive(false);
    }
}
