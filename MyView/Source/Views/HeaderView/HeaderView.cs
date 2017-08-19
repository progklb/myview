using Foundation;
using System;
using UIKit;
using CoreGraphics;

using MyView.Additional;

namespace MyView.Views
{
	/// <summary>
	/// The top header bar that is displayed as an overlay over the main screen.
	/// </summary>
    public partial class HeaderView : BaseView
    {
    	#region PROPERTIES
    	/// Whether or not the background gradient should be displayed. 
		/// Note that this does not take effect immediately and will only be considered when showing this view.
    	public bool ShowBackingGradient { get; set; }
    	#endregion
    	
    	
    	#region CONSTRUCTOR
        public HeaderView(IntPtr handle) : base (handle) { }
        #endregion
        
        
        #region INHERITED METHODS
        public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			
			InsertGradient(UIViewGradient, Colors.Black.CGColor, Colors.BlackTransparent.CGColor);
		}
		
		public override void AnimateIn()
		{
			UIViewGradient.Hidden = !ShowBackingGradient;
			base.AnimateIn();
		}
        #endregion
    }
}