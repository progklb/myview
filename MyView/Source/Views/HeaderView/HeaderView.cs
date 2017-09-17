using System;
using System.Threading.Tasks;

namespace MyView.Views
{
	/// <summary>
	/// The top header bar that is displayed as an overlay over the main screen.
	/// </summary>
    public partial class HeaderView : BaseView
    {
    	#region CONSTANTS
    	public const string CATEGORY_FORMAT = "•  {0}";
    	public const int ANIMATION_Y_MOVEMENT = 20;
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
			UILabelCategory.Text = string.Empty;
			UILabelCategory.Alpha = 0f;
        	UILabelCategory.Hidden = true;
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
        /// If a null parameter is provided, the category label will be hidden.
        /// </summary>
        /// <param name="categoryText">Text to display.</param>
        public void SetCategoryText(string categoryText)
        {
        	if (categoryText != null)
        	{
	        	AnimateCategoryChangeAsync(categoryText).ConfigureAwait(false);
        	}
        	else
        	{
        		HideCategoryLabel();
        	}
        }
        #endregion
        
        
        #region HELPERS
        /// <summary>
        /// Fades out the category text, changes it, and fades it back in.
        /// If
        /// </summary>
        /// <returns>The category change async.</returns>
        /// <param name="categoryText">Category text to set.</param>
        async Task AnimateCategoryChangeAsync(string categoryText)
        {
        	if (!UILabelCategory.Hidden)
        	{
				FadeCategoryLabel(ChangeAnimDuration, 0f);
				await Task.Delay((int)(ChangeAnimDuration * 1000));
        	}
			
			UILabelCategory.Hidden = false;
        	UILabelCategory.Text = string.Format(CATEGORY_FORMAT, categoryText);
			FadeCategoryLabel(ChangeAnimDuration, 1f);
        }
        
        /// <summary>
        /// Immediately hides the category label and sets the hidden flag so that we know it is hidden.
        /// </summary>
        void HideCategoryLabel()
        {
        	UILabelCategory.Alpha = 0f;
        	UILabelCategory.Hidden = true;
        }
        
        /// <summary>
        /// Fades the category text to the alpha specified.
        /// </summary>
        /// <param name="duration">Duration.</param>
        /// <param name="targetAlpha">Target alpha.</param>
        void FadeCategoryLabel(nfloat duration, nfloat targetAlpha)
        {
        	Animate(
				duration, 
				() => { UILabelCategory.Alpha = targetAlpha; }
			);
        }
        
        /// <summary>
        /// Triggers an "ease-in" animation. Offsets the view by the animation Y change, and then set the target position in the animation.
        /// </summary>
        void AnimateEaseIn()
        {
			var currentFrame = Frame;
			currentFrame.Y -= ANIMATION_Y_MOVEMENT;
			Frame = currentFrame;
			
			Animate(
				AnimateInDuration,
				() => {
						var targetFrame = Frame;
						targetFrame.Y += ANIMATION_Y_MOVEMENT;
						Frame = targetFrame;
				}
			);
        }
        #endregion
    }
}