using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // ȷ������� "Player" Tag
        {
            Debug.Log("�ӵ���������ң�");
            Destroy(gameObject); // �ӵ�����
            //other.GetComponent<PlayerHealth>()?.TakeDamage(10); // ���������
        }
    }

}
