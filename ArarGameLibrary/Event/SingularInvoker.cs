using ArarGameLibrary.Manager;
using ArarGameLibrary.Model;
using System;

namespace ArarGameLibrary.Event
{
    //Events.Add(new SingularInvoker(this,
    //whenToInvoke: () =>
    //            {
    //                return MaxRowsCount <= RowsCountToShow;
    //            },
    //            success: () =>
    //            {
    //                Bar.SetVisible(false);
    //            },
    //            fail: () => 
    //            {
    //                Bar.SetVisible(true);
    //            }));
    public class SingularInvoker : EventManager
    {
        bool IsPositive { get; set; }

        Action Success { get; set; }

        Action Fail { get; set; }

        public SingularInvoker(Sprite sprite, Func<bool> whenToInvoke, Action success, Action fail)
            : base(sprite)
        {
            SetWhenToInvoke(whenToInvoke);

            SetSuccessAction(success);

            SetFailAction(fail);

            SetTask(() =>
            {
                if (IsPositive)
                {
                    if (Success != null)
                    {
                        Success();

                        SetDrawable(true);
                    }
                }
                else
                {
                    if (Fail != null)
                    {
                        Fail();

                        SetDrawable(false);
                    }
                }
            });
        }

        public override void Update()
        {
            if (WhenToInvoke() != IsPositive)
                Reset();

            IsPositive = WhenToInvoke();

            base.Update();
        }


        public SingularInvoker SetSuccessAction(Action successAction)
        {
            Success = successAction;

            return this;
        }

        public SingularInvoker SetFailAction(Action failAction)
        {
            Fail = failAction;

            return this;
        }
    }
}
