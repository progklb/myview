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
    	/// The amount of time in seconds before automatic dimming takes place.
    	public float DimTimeout { get; set; } = 5f;
    	#endregion
    	
    	
    	#region VARIABLE
    	/// If true, there is currently a timeout running for automatic dimming of this view.
    	private bool m_DimTimeoutRunning = false;
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
		
		public override void AnimateIn()
		{
			// Invalidate any timeout
			base.AnimateIn();
			m_DimTimeoutRunning = false;
		}
		
		public override void AnimateOut()
		{
			// Invalidate any timeout
			base.AnimateOut();
			m_DimTimeoutRunning = false;
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
        	AnimateAuthorChangeAsync(authorName, authorHandle).ConfigureAwait(false);
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
			
			m_DimTimeoutRunning = false;
		}
		
		/// <summary>
		/// Starts a timeout of length <see cref="DimTimeout"/> before dimming this view. 
		/// Note that this will be invalidated if any other animation is called on this object.
		/// </summary>
		public void StartDimTimeout(Action completionHandler)
		{
			StartDimTimeoutAsync(DimTimeout, completionHandler).ConfigureAwait(false);
		}
        #endregion
        
        
        #region HELPERS
        async Task StartDimTimeoutAsync(nfloat timeout, Action completionHandler = null)
        {	
        	// IMPROVE
        	// We enter into a loop here because its the easiest way of being able to 
        	// invalidate a timeout without have cancellation tokens or some other form of tracking task validity.
        	
        	m_DimTimeoutRunning = true;
        	var start = DateTime.Now.Ticks;
        	
        	do
        	{
        		await Task.Delay(50);
        	}
        	while (m_DimTimeoutRunning && (DateTime.Now.Ticks - start < timeout * TimeSpan.TicksPerSecond));
        	
        	// Check that the dim task is still valid.
        	if (m_DimTimeoutRunning)
        	{
        		AnimateDim();
        		
        		if (completionHandler != null)
        		{
        			completionHandler();
        		}
        	}
        }
        
        async Task AnimateAuthorChangeAsync(string authorName, string authorHandle)
        {
			FadeAuthor(ChangeAnimDuration, 0f);
			
			await Task.Delay((int)(ChangeAnimDuration * 1000));
			
			UILabelAuthorName.Text = authorName;
        	UILabelAuthorHandle.Text = string.Format(AUTHOR_HANDLE_FORMAT, authorHandle);
			FadeAuthor(ChangeAnimDuration, 1f);
        }
        
        void FadeAuthor(nfloat duration, nfloat targetAlpha)
        {
        	Animate(
				duration, 
				() => { 
					UILabelAuthorName.Alpha = targetAlpha;
					UILabelAuthorHandle.Alpha = targetAlpha;
				}
			);
        }
        #endregion
    }
}