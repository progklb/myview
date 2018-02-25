using System;
using System.Threading.Tasks;

namespace MyView.Views
{
	/// <summary>
	/// The bottom footer bar that is displayed as an overlay when viewing images to
	/// show image details and controls.
	/// </summary>
    public partial class FooterView : BaseView
    {
    	#region TYPES
    	public enum LocationVisibilityMode
    	{
    		/// Location will never be displayed
    		Never,
			/// Location will only be displayed when the footer is at full opacity (after animating in).
    		Selected,
			/// Location will always be displayed.
    		Always
    	}

		public enum AuthorVisibilityMode
		{
			/// Item will only be displayed when the footer is at full opacity (after animating in).
			Selected,
			/// Item will always be displayed.
			Always
		}
    	#endregion
    	
    	
    	#region CONSTANTS
    	private const string AUTHOR_HANDLE_FORMAT = "@{0}";
    	#endregion
    	
    	
    	#region PROPERTIES
    	/// Whether and how we should display the location text.
		public LocationVisibilityMode LocationVisibility { get; set; } = LocationVisibilityMode.Selected;
		/// How we should display the author text.
		public AuthorVisibilityMode AuthorVisibility { get; set; } = AuthorVisibilityMode.Selected;
    	
    	/// The target alpha value for <see cref="AnimateDim"/>.
    	public nfloat DimmedAlpha { get; set; } = 0.5f;
    	/// The amount of time in seconds before automatic dimming takes place.
    	public float DimTimeout { get; set; } = 5f;
    	#endregion
    	
    	
    	#region VARIABLE
    	/// If true, there is currently a timeout running for automatic dimming of this view.
    	private bool m_DimTimeoutRunning = false;
    	
    	/// True if the UI is currently in a dimmed state. False if fully transparent or opaque.
    	private bool m_Dimmed = false;
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
			UILabelAuthorHandle.Text = 
			UILabelLocationCity.Text =
			UILabelLocationCountry.Text = string.Empty;
		}
        #endregion
        
        
        #region ANIMATIONS
        public override void AnimateIn()
        {
            // Invalidate any timeout
            base.AnimateIn();
            m_DimTimeoutRunning = false;
            m_Dimmed = false;

            // Location fades differently to the rest of the UI.
            FadeLocation(AnimateOutDuration, 1f);
        }

        public override void AnimateOut()
        {
            // Invalidate any timeout
            base.AnimateOut();
            m_DimTimeoutRunning = false;
            m_Dimmed = false;
        }

        /// <summary>
        /// Fades this view to a semi-transparent state.
        /// </summary>
        public virtual void AnimateDim()
        {
            Hidden = false;
            nfloat targetAlpha = DimmedAlpha;

            m_Dimmed = true;

            Animate(
                AnimateOutDuration,
                () => { Alpha = targetAlpha; }
            );

            // Fully fade out the location
            if (LocationVisibility == LocationVisibilityMode.Selected)
            {
                FadeLocation(AnimateOutDuration, 0f);
            }

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

                completionHandler?.Invoke();
            }
        }
        #endregion


        #region AUTHOR
        /// <summary>
        /// Assigns the provided parameters to the display on the footer.
        /// </summary>
        /// <param name="authorName">Author name.</param>
        /// <param name="authorHandle">Author handle.</param>
        public void SetAuthorText(string authorName, string authorHandle)
        {
            AnimateAuthorChangeAsync(authorName, authorHandle).ConfigureAwait(false);
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
        
        
        #region LOCATION
        /// <summary>
        /// Assigns the provided parameters to the display on the footer.
        /// </summary>
        /// <param name="city">Name of the city.</param>
        /// <param name="country">Name of the country.</param>
        public void SetLocationText(string city, string country)
        {
            /*
                Location fades are different to the rest of the UI:
                When the footer goes into a dimmed state, the location should not be visible. In such cases we simply assign the text, not animating it because
                we don't want it to appear. If not dimmed however, we should animate the change normally.
            */

            if (LocationVisibility == LocationVisibilityMode.Never)
            {
                return;
            }

            var cityName = city ?? string.Empty;
            var countryName = country ?? string.Empty;

            if (LocationVisibility == LocationVisibilityMode.Always || (LocationVisibility == LocationVisibilityMode.Selected && !m_Dimmed))
            {
                AnimateLocationChangeAsync(cityName, countryName).ConfigureAwait(false);
            }
            else
            {
                UILabelLocationCity.Text = city;
                UILabelLocationCountry.Text = country;
            }
        }

        async Task AnimateLocationChangeAsync(string city, string country)
        {
			FadeLocation(ChangeAnimDuration, 0f);
			
			await Task.Delay((int)(ChangeAnimDuration * 1000));
			
			UILabelLocationCity.Text = city;
        	UILabelLocationCountry.Text = country;
			FadeLocation(ChangeAnimDuration, 1f);
        }
        
        void FadeLocation(nfloat duration, nfloat targetAlpha)
        {
        	Animate(
				duration, 
				() => { 
					UILabelLocationCity.Alpha = targetAlpha;
					UILabelLocationCountry.Alpha = targetAlpha;
				}
			);
        }
        #endregion
    }
}