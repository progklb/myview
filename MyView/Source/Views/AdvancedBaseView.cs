using System;
using System.Threading.Tasks;

using CoreGraphics;

namespace MyView.Views
{
	/// <summary>
    /// Adds more advanced features on top of <see cref="BaseView"/>.
	/// </summary>
	public class AdvancedBaseView : BaseView
	{
        #region CONSTANTS
        protected readonly nfloat ANIM_IN_DURATION = 0.25f;
        protected readonly nfloat ANIM_OUT_DURATION = 0.25f;
        protected readonly nfloat ANIM_SCALE_TARGET = 1.1f;
        #endregion


		#region CONSTRUCTOR
        public AdvancedBaseView(IntPtr handle) : base(handle) { }
        #endregion


        #region INHERITED METHODS
        public override void AwakeFromNib()
        {
            // Override the base class animation durations
            AnimateInDuration = ANIM_IN_DURATION;
            AnimateOutDuration = ANIM_OUT_DURATION;

            base.AwakeFromNib();
        }

        public override void AnimateIn()
        {
            base.AnimateIn();

            // Scale downward
            Transform = CGAffineTransform.MakeScale(ANIM_SCALE_TARGET, ANIM_SCALE_TARGET);
            Animate(
                AnimateInDuration,
                () => Transform = CGAffineTransform.MakeIdentity()
            );
        }

        public override void AnimateOut()
        {
            base.AnimateOut();

            // Scale upward
            Transform = CGAffineTransform.MakeIdentity();
            Animate(
                AnimateOutDuration,
                () => Transform = CGAffineTransform.MakeScale(ANIM_SCALE_TARGET, ANIM_SCALE_TARGET)
            );
        }
		#endregion
		
		
		#region PUBLIC API
        public void AnimateOutAndRemove()
        {
            Animate(
                AnimateOutDuration,
                () => { Alpha = 0f; },
                () => { RemoveFromSuperview(); }
            );
        }

        public async void AnimateOutAndRemove(int delayMs, Action callback = null)
        {
            await Task.Delay(delayMs);
            AnimateOutAndRemove();

            callback?.Invoke();
        }
		#endregion
	}
}
