using ArarGameLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.Effect
{
    //will be removed
    public abstract class EffectManager
    {
        public Sprite Sprite { get; set; }

        public bool IsActive { get; set; }

        private Action Task;
        private Action EndTask;
        private Action DrawingTask;

        public EffectManager(Sprite sprite)
        {
            Sprite = sprite;
        }

        public void Start()
        {
            IsActive = true;
        }

        public void End()
        {
            IsActive = false;
        }

        public void Update()
        {
            if (IsActive)
            {
                if (Task != null)
                    Task.Invoke();
            }
            else
            {
                if (EndTask != null)
                    EndTask.Invoke();
            }
        }

        public void Draw()
        {
            if (IsActive && DrawingTask != null)
                DrawingTask.Invoke();
        }

        public EffectManager SetTask(Action task)
        {
            Task = task;

            return this;
        }

        public EffectManager SetEndTask(Action endTask)
        {
            EndTask = endTask;

            return this;
        }

        public EffectManager SetDrawingTask(Action drawingTask)
        {
            DrawingTask = drawingTask;

            return this;
        }

        public static T Get<T>(List<EffectManager> effects) where T : EffectManager
        {
            return effects.FirstOrDefault(e => e is T) as T;
        }
    }
}
