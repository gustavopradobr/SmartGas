using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PumpAnimation : MonoBehaviour
{
    [SerializeField] private Transform[] position = new Transform[5];
    [SerializeField] private Transform pumpRoot;

    public void AnimatePump(bool forwardAnimation, float duration, Action? callback)
    {
        int index = forwardAnimation ? 1 : (position.Length-2);
        int increment = forwardAnimation ? 1 : -1;

        pumpRoot.DORotateQuaternion(position[index].transform.rotation, duration*0.25f);
        pumpRoot.DOMove(position[index].transform.position, duration * 0.25f).SetEase(Ease.Linear).OnComplete(() =>
        {
            index += increment;

            pumpRoot.DORotateQuaternion(position[index].transform.rotation, duration * 0.2f);
            pumpRoot.DOMove(position[index].transform.position, duration * 0.25f).SetEase(Ease.Linear).OnComplete(() =>
            {
                index += increment;

                pumpRoot.DORotateQuaternion(position[index].transform.rotation, duration * 0.25f);
                pumpRoot.DOMove(position[index].transform.position, duration * 0.25f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    index += increment;

                    pumpRoot.DORotateQuaternion(position[index].transform.rotation, duration * 0.25f);
                    pumpRoot.DOMove(position[index].transform.position, duration * 0.25f).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        callback?.Invoke();
                    });

                });

            });

        });
    }
}
