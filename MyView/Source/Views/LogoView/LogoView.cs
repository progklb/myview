using System;

namespace MyView.Views
{
    public partial class LogoView : BaseView
    {
    	#region CONSTRUCTOR
        public LogoView (IntPtr handle) : base (handle) { }
    	#endregion
    	
    	
    	#region HELPER FUNCTIONS
    	public void AnimateOutAndRemove()
		{
			Animate(
				AnimateOutDuration,
				() => { Alpha = 0; },
				() => { RemoveFromSuperview(); }
			);
		}
    	#endregion
    }
}