using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    // JSON �迭�� �Ľ��ϱ� ���� ���� �޼���
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"items\": " + json + "}";  // �迭�� ��ü�� ����
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.items;
    }

    // �迭�� ���δ� ���� Ŭ����
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}
