using PuzzleMeWindowsProject.Manager;
using PuzzleMeWindowsProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleMeWindowsProject.ScreenManagement
{
    public enum ScreenState
    {
        Active,
        Inactive,
        Frozen,
        Preparing
    }

    public abstract class Screen : BaseObject,IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public Screen PreviousScreen { get; set; }

        public ScreenState ScreenState { get; set; }

        private double TimeToActive { get; set; }

        //private bool IsInitialized { get; set; }

        private bool IsLoaded { get; set; }

        public Screen()
        {
            ScreenState = ScreenState.Preparing;

            Initialize();

            IsLoaded = Load();
        }

        ~Screen()
        {
            Dispose(false);
        }

        public void CheckWhetherIsReady()
        {
            TimeToActive += Global.GameTime.ElapsedGameTime.TotalSeconds;

            if (TimeToActive > 0.2 && IsLoaded)
            {
                InputManager.IsActive = true;

                ScreenState = ScreenState.Active;
            }
            else
            {
                InputManager.IsActive = false;
            }
        }

        public abstract void Initialize();

        public abstract bool Load();

        public virtual void UnloadContent()
        {
            Dispose();
        }

        public virtual void Update()
        {
            if (ScreenState == ScreenState.Frozen)
                return;

            if(ScreenState ==  ScreenState.Preparing)
                CheckWhetherIsReady();
        }

        public abstract void Draw();

        public void DisableThenAddNew(Screen newScreen)
        {
            ScreenState = ScreenState.Inactive;

            ScreenManager.Add(newScreen);
        }

        public Screen Freeze(Screen newScreen)
        {
            ScreenState = ScreenState.Frozen;

            newScreen.SetPreviousScreen(this);

            ScreenManager.Add(newScreen);

            return this;
        }

        public Screen Activate()
        {
            ScreenState = ScreenManagement.ScreenState.Active;

            return this;
        }

        //public abstract void HandleInput();

        //public abstract void ExitScreen();

        public Screen SetPreviousScreen(Screen previousScreen)
        {
            PreviousScreen = previousScreen;

            return this;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // free native resources if there are any.
            if (nativeResource != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(nativeResource);
                nativeResource = IntPtr.Zero;
            }
        }
    }
}
