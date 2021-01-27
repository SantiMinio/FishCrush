using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T>
{
    public delegate T FactoryMethod();
    public delegate void ActivateOrDesactivate(T obj);

    FactoryMethod Factory;
    ActivateOrDesactivate Activate;
    ActivateOrDesactivate Desactivate;
    List<T> myObjs;
    bool isDynamic;

    public Pool(FactoryMethod _factory, ActivateOrDesactivate _activate, ActivateOrDesactivate _desactivate, int startAmmount = 3, bool _isDynamic = true)
    {
        isDynamic = _isDynamic;
        Factory = _factory;
        Activate = _activate;
        Desactivate = _desactivate;
        myObjs = new List<T>();
        for (int i = 0; i < startAmmount; i++)
        {
            var obj = Factory();
            Desactivate(obj);
            myObjs.Add(obj);
        }
    }

    public T GetObject()
    {
        T result;
        if(myObjs.Count == 0)
        {
            if (isDynamic) result = Factory();
            else return default(T);
        }
        else
        {
            result = myObjs[0];
            myObjs.Remove(result);
        }
        Activate(result);
        return result;
    }

    public void ReturnObject(T obj)
    {
        Desactivate(obj);
        myObjs.Add(obj);
    }
}
