using System;
using System.Threading.Tasks;

using Foundation;
using UIKit;

using MyView.Additional;

namespace MyView.Views
{
	/// <summary>
	/// The bottom footer bar that is displayed as an overlay when viewing images to
	/// show image details and controls.
	/// </summary>
    public partial class FooterView : BaseView
    {
    	#region CONSTANTS
    	private const string AUTHOR_HANDLE_FORMAT = "@{0}";
    	#endregion
    	
    	
    	#region PROPERTIES
    	/// The target alpha value for <see cref="AnimateDim"/>.
    	public nfloat DimmedAlpha { get; set; } = 0.5f;
    	#endregion
    	
    	
    	#region CONSTTRUCTOR
        public FooterView(IntPtr handle) : base (handle) { }
        #endregion
        
        
        #region INHERITED METHODS
        public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			
			InsertGradient(UIViewGradient, Constants.Colors.BlackTransparent.CGColor, Constants.Colors.Black.CGColor);
			
			UILabelAuthorName.Text = 
			UILabelAuthorHandle.Text = string.Empty;
		}
        #endregion
        
        
        #region PUBLIC API
        /// <summary>
        /// Assigns the provided parameters to the display on the footer.
        /// </summary>
        /// <param name="authorName">Author name.</param>
        /// <param name="authorHandle">Author handle.</param>
        public void SetAuthorText(string authorName, string authorHandle)
        {
        	AnimateAuthorChange(authorName, authorHandle).ConfigureAwait(false);
        }
        
        /// <summary>
		/// Fades this view to a semi-transparent state.
		/// </summary>
		public virtual void AnimateDim()
		{
			Hidden = false;
			nfloat targetAlpha = DimmedAlpha;
			
			Animate(
				AnimateOutDuration,
				() => { Alpha = targetAlpha; }
			);
		}
        #endregion
        
        
        #region HELPERS
        async Task AnimateAuthorChange(string authorName, string authorHandle)
        {
			FadeAuthor(ChangeAnimDuration, 0f);
			
			await Task.Delay((int)(ChangeAnimDuration * 1000));
			
			UILabelAuthorName.Text = authorName;
        	UILabelAuthorHandle.Text = string.Format(AUTHOR_HANDLE_FORMAT, authorHandle);
			FadeAuthor(ChangeAnimDuration, 1f);
        }
        
        void FadeAuthor(nfloat duration, nfloat targetAlpha)
        {
        	Animate(duration, () => { 
					UILabelAuthorName.Alpha = targetAlpha;
					UILabelAuthorHandle.Alpha = targetAlpha;
				}
			);
        }
        #endregion
    }
}