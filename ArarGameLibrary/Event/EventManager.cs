using ArarGameLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Event
{
    public abstract class EventManager
    {
        public Sprite Sprite { get; set; }

        private bool IsContinuous { get; set; }

        private Action Task;

        private bool IsInvoked { get; set; }

        private bool IsActive { get; set; }

        public EventManager(Sprite sprite, bool isContinuous = false)
        {
            Sprite = sprite;

            IsActive = IsContinuous = isContinuous;
        }

        public virtual void Update()
        {
            if (!IsActive)
                return;

            if (IsContinuous)
                Task.Invoke();
            else
                if (!IsInvoked)
                {
                    Task.Invoke();

                    IsInvoked = true;
                }
        }

        public EventManager SetTask(Action task)
        {
            Task = task;

            return this;
        }

        public EventManager Reset()
        {
            IsInvoked = false;

            return this;
        }

        public void Start()
        {
            IsActive = true;
        }

        public void End()
        {
            IsActive = false;
        }

        public static T Get<T>(List<EventManager> events) where T : EventManager
        {
            return events.FirstOrDefault(e => e is T) as T;
        }
    }
}
