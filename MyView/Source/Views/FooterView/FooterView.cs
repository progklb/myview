using Foundation;
using System;
using UIKit;

namespace MyView
{
	/// <summary>
	/// The bottom footer bar that is displayed as an overlay when viewing images to
	/// show image details and controls.
	/// </summary>
    public partial class FooterView : BaseView
    {
    	#region CONSTTRUCTOR
        public FooterView(IntPtr handle) : base (handle) { }
        #endregion
        
        
        #region INHERITED METHODS
        public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			
			InsertGradient(UIViewGradient, Colors.BlackTransparent.CGColor, Colors.Black.CGColor);
		}
        #endregion
    }
}