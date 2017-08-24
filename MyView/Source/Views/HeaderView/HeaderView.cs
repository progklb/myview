using System;
using System.Threading.Tasks;

using Foundation;
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
    	#region CONSTANTS
    	public const string CATEGORY_FORMAT = "•  {0}";
    	#endregion
    	
    	
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
			
			InsertGradient(UIViewGradient, Constants.Colors.Black.CGColor, Constants.Colors.BlackTransparent.CGColor);
		}
		
		public override void AnimateIn()
		{
			UIViewGradient.Hidden = !ShowBackingGradient;
			base.AnimateIn();
		}
        #endregion
        
        
        #region PUBLIC API
        /// <summary>
        /// Assigns the provided parameter to the category field on the header.
        /// </summary>
        /// <param name="categoryText">Text to display.</param>
        public void SetCategoryText(string categoryText)
        {
        	AnimateCategoryChange(categoryText).ConfigureAwait(false);
        }
        #endregion
        
        
        #region HELPERS
        async Task AnimateCategoryChange(string categoryText)
        {
			FadeCategory(ChangeAnimDuration, 0f);
			
			await Task.Delay((int)(ChangeAnimDuration * 1000));
			
        	UILabelCategory.Text = string.Format(CATEGORY_FORMAT, categoryText);
			FadeCategory(ChangeAnimDuration, 1f);
        }
        
        void FadeCategory(nfloat duration, nfloat targetAlpha)
        {
        	Animate(duration, () => { 
					UILabelCategory.Alpha = targetAlpha;
				}
			);
        }
        #endregion
    }
}