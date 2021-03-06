﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SteelArm : Weapon {

    const float punchStartDelay = 0.1F, punchEndDelay = 0.25F;
    const int damage = 100;
    float punchPeriodStartTime;
    enum PunchStatus { Start, End, Ready };
    PunchStatus punchStatus;
    int enemiesLayer = 1 << 8;
    Quaternion normalRotation, punchRotation, blockRotation;
    NetworkIdentity parentIdentity;
    void Start () {
        normalRotation =  weapon.transform.localRotation;
        punchRotation = Quaternion.LookRotation(Vector3.down + weapon.transform.forward);
        blockRotation = Quaternion.LookRotation(Vector3.down + Vector3.left);
        parentIdentity = gameObject.GetComponent<NetworkIdentity>();
    }

    void Update() {
        if (isBlock)
        {
            weapon.transform.localRotation = Quaternion.Lerp(weapon.transform.localRotation, blockRotation, Time.deltaTime * 15);
        }
        else if (isEnabled || !(punchStatus == PunchStatus.Ready))
        {
            switch (punchStatus)
            {
                case PunchStatus.Ready:
                    if (isEnabled)
                    punchStatus = PunchStatus.Start;
                    break;
            case PunchStatus.Start:
                    weapon.transform.localRotation = Quaternion.Lerp(weapon.transform.localRotation, punchRotation, Time.deltaTime * 15);
                    if (Time.time - punchPeriodStartTime > punchStartDelay)
                    {
                        punchPeriodStartTime = Time.time;
                        punchStatus = PunchStatus.End;
                        if (parentIdentity.isServer)
                        {
                            Punch();
                        }
                    }
                break;
            case PunchStatus.End:
                    weapon.transform.localRotation = Quaternion.Lerp(weapon.transform.localRotation, normalRotation, Time.deltaTime * 15);
                    if (Time.time - punchPeriodStartTime > punchEndDelay)
                    {
                        punchPeriodStartTime = Time.time;
                        punchStatus = PunchStatus.Ready;
                    }
                break;
            }
        }
        else
        {
            punchPeriodStartTime = Time.time;
            weapon.transform.localRotation = Quaternion.Lerp(weapon.transform.localRotation, normalRotation, Time.deltaTime * 20);
        }
	}

    void Punch()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, 5, enemiesLayer);

        foreach (Collider col in cols)
        {
            if (col.GetComponent<Health>() != null && col.transform != gameObject.transform)
            {
                if (Vector3.Angle(transform.forward, col.transform.position - transform.position) < 120)
                col.GetComponent<Health>().Punch(damage, transform.position, transform);
            }
        }
    }
}
