using Foundation;
using System;
using UIKit;

namespace MyView.Views
{
	/// <summary>
	/// The bottom footer bar that is displayed as an overlay allowing the user to
	/// select an image category to view.
	/// </summary>
    public partial class CategorySelectView : BaseView
    {
    	#region CONSTTRUCTOR
        public CategorySelectView(IntPtr handle) : base (handle) { }
        #endregion
        
        
        #region INHERITED METHODS
        //public override void AwakeFromNib()
		//{
		//	base.AwakeFromNib();
			
			//UICollectionCategories.RegisterClassForCell(typeof(ImageCell), new NSString(ImageCell.CellIdentifier));
			//UICollectionCategories.Source = new UICollectionViewSource();
		//}
        #endregion
    }
}