using ArarGameLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Manager
{
    public abstract class EventManager
    {
        public Sprite Sprite { get; set; }

        private bool IsContinuous { get; set; }

        private Action Task;

        private Action DrawingTask;

        private bool IsInvoked { get; set; }

        private bool IsDrawable { get; set; }

        private bool IsActive { get; set; }

        public Func<bool> WhenToInvoke { get; set; }

        public EventManager(Sprite sprite, bool isContinuous = false)
        {
            Sprite = sprite;

            IsContinuous = isContinuous;

            IsActive = true;
        }

        public virtual void Update()
        {
            if (!IsActive)
                return;

            if (IsContinuous)
                Task.Invoke();
            else if (!IsInvoked)
                {
                    Task.Invoke();

                    IsInvoked = true;
                }
        }

        public virtual void Draw()
        {
            if (!IsActive)
                return;

            if (IsDrawable && DrawingTask != null)
                DrawingTask.Invoke();
        }

        public EventManager SetTask(Action task)
        {
            Task = task;

            return this;
        }

        public EventManager SetDrawingTask(Action drawingTask)
        {
            DrawingTask = drawingTask;

            return this;
        }

        public EventManager SetContinuous(bool enable)
        {
            IsContinuous = enable;

            return this;
        }

        public EventManager SetDrawable(bool enable)
        {
            IsDrawable = enable;

            return this;
        }

        public EventManager SetWhenToInvoke(Func<bool> whenToInvoke)
        {
            WhenToInvoke = whenToInvoke;

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

