using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ArarGameLibrary.ScreenManagement
{
    public interface IScreen : IBaseObject,IXna
    {
        IScreen PreviousScreen { get; set; }

        IScreen NextScreen { get; set; }

        ScreenState ScreenState { get; set; }

        void CheckWhetherIsReady();

        void DisableThenAddNew(IScreen newScreen);

        IScreen Activate();

        IScreen Freeze(IScreen newScreen);

        IScreen SetPreviousScreen(IScreen previousScreen);

        IScreen SetNextScreen(IScreen nextScreen);
    }

    public enum ScreenState
    {
        Active,
        Inactive,
        Frozen,
        Preparing
    }

    public abstract class Screen : BaseObject,IScreen, IDisposable
    {
        private IntPtr nativeResource = Marshal.AllocHGlobal(100);

        public IScreen PreviousScreen { get; set; }

        public IScreen NextScreen { get; set; }

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

        public virtual void LoadContent(Texture2D texture = null) { }

        public virtual void UnloadContent()
        {
            Dispose();
        }

        public virtual void Update(GameTime gameTime = null)
        {
            if (ScreenState == ScreenState.Frozen)
                return;

            if (ScreenState == ScreenState.Preparing)
                CheckWhetherIsReady();
        }

        public virtual void Draw(SpriteBatch spriteBatch = null) { }

        public void DisableThenAddNew(IScreen newScreen)
        {
            ScreenState = ScreenState.Inactive;

            ScreenManager.Add(newScreen);
        }

        public IScreen Freeze(IScreen newScreen)
        {
            ScreenState = ScreenState.Frozen;

            newScreen.SetPreviousScreen(this);

            ScreenManager.Add(newScreen);

            return this;
        }

        public IScreen Activate()
        {
            ScreenState = ScreenManagement.ScreenState.Active;

            return this;
        }

        //public abstract void HandleInput();

        //public abstract void ExitScreen();

        public IScreen SetPreviousScreen(IScreen previousScreen)
        {
            PreviousScreen = previousScreen;

            return this;
        }

        public IScreen SetNextScreen(IScreen nextScreen)
        {
            NextScreen = nextScreen;

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
