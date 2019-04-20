using PuzzleMeWindowsProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.Effect
{
    public abstract class EffectManager
    {
        public Sprite Sprite { get; set; }

        private bool IsActive { get; set; }

        private Action Task;
        private Action EndTask;

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
                Task.Invoke();
            }
            else
            {
                EndTask.Invoke();
            }
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
    }
}
