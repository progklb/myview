using System;

namespace MyView.Views
{
    public partial class AlertView : BaseView
    {
    	#region CONSTRUCTOR
        public AlertView (IntPtr handle) : base (handle) { }
        #endregion
        
        
        #region INHERITED METHODS
		public override void AnimateIn()
		{
			base.AnimateIn();
		}
		
		public override void AnimateOut()
		{
			base.AnimateOut();
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
		
        /// <summary>
        /// Assigns the provided parameters to the fields on the alert view.
        /// </summary>
        /// <param name="title">Header text of the alert.</param>
        /// <param name="body">Body text of the alert.</param>
        public void SetText(string title, string body)
        {
        	UILabelTitle.Text = title;
        	UILabelBody.Text = body;
        }
        #endregion
    }
}