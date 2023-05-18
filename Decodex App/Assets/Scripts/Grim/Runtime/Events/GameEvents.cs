using Grim.Events;
using Grim.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Grim.Events
{
    /// <summary>
    /// Taken (mostly) from Acciaio :*
    /// </summary>
    public sealed class GameEvents
    {
        private static GameEvents _instance;
        public static GameEvents Current
        {
            get
            {
                if (_instance == null)
                    _instance = new();
                return _instance;
            }
        }
        public bool IsRunning => true;
        private readonly Dictionary<string, PubSubBoard> _boards = new();
        public PubSubBoard Global = new();

        public PubSubBoard this[string boardName] => GetOrCreate(boardName);

        private PubSubBoard GetOrCreate(string boardName)
        {
            if (!_boards.ContainsKey(boardName)) _boards.Add(boardName, new());
            return _boards[boardName];
        }

        public void Subscribe<T>(string eventName, Action<T> subscription)
            => Global.Subscribe(eventName, subscription);

        public void Subscribe<T>(string eventName, RefAction<T> subscription)
            => Global.Subscribe(eventName, subscription);

        public void Subscribe(string eventName, Action subscription)
            => Global.Subscribe(eventName, subscription);

        public bool Unsubscribe<T>(string eventName, Action<T> subscription)
            => Global.Unsubscribe(eventName, subscription);

        public bool Unsubscribe<T>(string eventName, RefAction<T> subscription)
            => Global.Unsubscribe(eventName, subscription);

        public void Unsubscribe(string eventName, Action subscription)
            => Global.Unsubscribe(eventName, subscription);

        public void Trigger<T>(string eventName, T args)
        {
            if (!IsRunning)
            {
                Debug.LogError($"[Pub-Sub System] System shut down, won't trigger the event '{eventName}'.");
                return;
            }
            Global.Trigger(eventName, args);
        }

        public void Trigger<T>(string eventName, ref T args)
        {
            if (!IsRunning)
            {
                Debug.LogError($"[Pub-Sub System] System shut down, won't trigger the event '{eventName}'.");
                return;
            }
            Global.Trigger(eventName, ref args);
        }

        public void Trigger(string eventName)
        {
            if (!IsRunning)
            {
                Debug.LogError($"[Pub-Sub System] System shut down, won't trigger the event '{eventName}'.");
                return;
            }
            Global.Trigger(eventName);
        }

        public void Subscribe<T>(string board, string eventName, Action<T> subscription)
            => GetOrCreate(board).Subscribe(eventName, subscription);

        public void Subscribe<T>(string board, string eventName, RefAction<T> subscription)
            => GetOrCreate(board).Subscribe(eventName, subscription);

        public void Subscribe(string board, string eventName, Action subscription)
            => GetOrCreate(board).Subscribe(eventName, subscription);

        public bool Unsubscribe<T>(string board, string eventName, Action<T> subscription)
            => GetOrCreate(board).Unsubscribe(eventName, subscription);

        public bool Unsubscribe<T>(string board, string eventName, RefAction<T> subscription)
            => GetOrCreate(board).Unsubscribe(eventName, subscription);

        public void Unsubscribe(string board, string eventName, Action subscription)
            => GetOrCreate(board).Unsubscribe(eventName, subscription);

        public void Trigger<T>(string board, string eventName, T args)
        {
            if (!IsRunning)
            {
                Debug.LogError($"[Pub-Sub System] System shut down, won't trigger the event '{eventName}'.");
                return;
            }
            GetOrCreate(board).Trigger(eventName, args);
        }

        public void Trigger<T>(string board, string eventName, ref T args)
        {
            if (!IsRunning)
            {
                Debug.LogError($"[Pub-Sub System] System shut down, won't trigger the event '{eventName}'.");
                return;
            }
            GetOrCreate(board).Trigger(eventName, ref args);
        }

        public void Trigger(string board, string eventName)
        {
            if (!IsRunning)
            {
                Debug.LogError($"[Pub-Sub System] System shut down, won't trigger the event '{eventName}'.");
                return;
            }
            GetOrCreate(board).Trigger(eventName);
        }
    }
}
