using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    // JSON 배열을 파싱하기 위한 헬퍼 메서드
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"items\": " + json + "}";  // 배열을 객체로 감쌈
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.items;
    }

    // 배열을 감싸는 래퍼 클래스
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}
