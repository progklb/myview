using Foundation;
using System;
using UIKit;
using CoreGraphics;

namespace MyView
{
	/// <summary>
	/// The top header bar that is displayed as an overlay over the main screen.
	/// </summary>
    public partial class HeaderView : BaseView
    {
    	#region CONSTTRUCTOR
        public HeaderView(IntPtr handle) : base (handle) { }
        #endregion
        
        
        #region INHERITED METHODS
        public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			
			InsertGradient(UIViewGradient, Colors.Black.CGColor, Colors.BlackTransparent.CGColor);
		}
        #endregion
    }
}