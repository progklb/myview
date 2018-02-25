using System;

using Foundation;
using UIKit;

using MyView.Adapters;
using MyView.Additional;
using MyView.Views;
using MyView.Unsplash;

namespace MyView.Screens
{
	/// <summary>
	/// The main screen of the app.
	/// </summary>
	public partial class MainViewController : UIViewController
	{
		#region CONSTANTS
		/// The text displayed as the header of the alert screen.
		private const string ALERT_HEADER = "Oh no!";
		/// The amount of time in milliseconds before automatically hiding the image interface.
		private const int IMAGE_INTERFACE_TIMEOUT = 3000;
		#endregion
		
		
		#region PROPERTIES
		/// The current mode that we are viewing the app in.
		public ApplicationModes CurrentMode { get; private set; }
		
		/// Returns an explicit focus target if one has been set. To trigger a focus on this target, first set it and then call <see cref="UpdateFocusIfNeeded"/>
		public override UIView PreferredFocusedView { get { return FocusTarget.TargetView ?? base.PreferredFocusedView; } }
		#endregion
        
        
		#region VARIABLES
		// Additional UI elements
		private CategorySelectView m_Select;
		private HeaderView m_Header;
		private FooterView m_Footer;
		private AlertView m_Alert;
		private LogoView m_Logo;
		
		/// Provides images for display on this page
        private SlideshowAdapter m_Slideshow;
        /// Keeps track of our last selected category.
        private SlideshowCategory m_LastSelectedCategory;

		/// A cache of the two image views for displaying images to the user
		private UIImageView[] m_ImageViews;
		/// The index of the currently display image view.
		private int m_CurrentImageIdx;
		#endregion


		#region CONSTRUCTOR
		public MainViewController(IntPtr handle) : base(handle) { }
		#endregion


		#region INHERITED METHODS
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			InitialiseInterface();
			InitialiseControls();
			InitialiseSlideshow();
			
			// We start at the category select screen. Note that we disable the
			// select recognizer so that it does not steal input from the category select view.
			CurrentMode = ApplicationModes.ImageView;
			SetSelectRecognizerEnabled(true);
			m_Footer.AnimateDim();
		}
		
		public override void ViewWillAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			
			// Start image provider
			m_Slideshow.Start();
			
			// Set up the UI to correspond to our current state
			m_LastSelectedCategory = m_Slideshow.CurrentCategory;
			//m_Select.SetCategoryText(m_Slideshow.CurrentCategory.DisplayName);
			SetHeader(m_Slideshow.CurrentCategory);
            
            SetBackground(new UnsplashImage(UIImage.FromFile(Constants.Images.StartUpPhoto).AsJPEG().ToArray()));
		}
		
		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			
			m_Slideshow.Stop();
		}
		#endregion
		
		
		#region SETUP
		/// <summary>
		/// Initialises the interface for the first time.
		/// </summary>
		void InitialiseInterface()
		{				
			m_Select = BaseView.CreateView<CategorySelectView>(this.View);
			m_Select.AddItemSelectedCallback(OnCategoryItemSelected);
            m_Select.AddItemFocusedCallback(OnCategoryItemFocused);
			
			m_Header = BaseView.CreateView<HeaderView>(this.View);
			m_Header.ShowBackingGradient = true;
			
			m_Footer = BaseView.CreateView<FooterView>(this.View);
			m_Logo = BaseView.CreateView<LogoView>(this.View, null, false);
			
			m_ImageViews = new UIImageView[] { UIImageBackground1, UIImageBackground2 };
			
			UIImageBackground1.Alpha = 0f;
			UIImageBackground2.Alpha = 0f;
		}
		
		/// <summary>
		/// Initialises gesture recognizers for capturing input.
		/// </summary>
		void InitialiseControls()
		{	
			var menuRecognizer = new UITapGestureRecognizer(OnRemoteMenuClicked);
			menuRecognizer.AllowedPressTypes = new NSNumber[] { NSNumber.FromInt64((Int64)UIPressType.Menu) };
			View.AddGestureRecognizer(menuRecognizer);
			
			var selectRecognizer = new UITapGestureRecognizer(OnRemoteSelectClicked);
			selectRecognizer.AllowedPressTypes = new NSNumber[] { NSNumber.FromInt64((Int64)UIPressType.Select) };
			View.AddGestureRecognizer(selectRecognizer);

			var touchRecognizer = new UITapGestureRecognizer(OnRemoteTouched);
			touchRecognizer.AllowedPressTypes = new NSNumber[] { };
			touchRecognizer.AllowedTouchTypes = new NSNumber[] { NSNumber.FromInt64((Int64)UITouchType.Indirect) };
			View.AddGestureRecognizer(touchRecognizer);
		}
		
		/// <summary>
		/// Initialises the slideshow adapter and subscribes this instance to its events.
		/// </summary>
		void InitialiseSlideshow()
		{
			m_Slideshow = new SlideshowAdapter();
			
			m_Slideshow.OnImageCycled += SetBackground;
            m_Slideshow.OnImageCycled += SetFooter;
            
            // Display alert on error. Successful downloads hide the alert.
            m_Slideshow.OnErrorThrown += ShowAlert;
            m_Slideshow.OnImageCycled += HideAlert;
                        
            // Start up logic
            m_Slideshow.OnImageCycled += HideStartUpLogo;
            
			m_Slideshow.SetSlideshowCategory(m_Slideshow.DefaultCategory);
		}
		#endregion
		
		
		#region EVENT HANDLERS - CONTROLS
		void OnRemoteMenuClicked()
		{
			if (CurrentMode == ApplicationModes.ImageView || CurrentMode == ApplicationModes.ImageDetails)
			{
				OnShowCategorySelect();
			}
			else if (CurrentMode == ApplicationModes.CategorySelect)
			{
				// Close menu, setting our original category.
				OnCategoryItemSelected(m_LastSelectedCategory);
			}
		}

		void OnRemoteSelectClicked()
		{
			if (CurrentMode == ApplicationModes.ImageView || CurrentMode == ApplicationModes.ImageDetails)
			{
				OnShowCategorySelect();
			}
		}

		void OnRemoteTouched()
		{
			if (CurrentMode == ApplicationModes.ImageView)
			{
				CurrentMode = ApplicationModes.ImageDetails;
				m_Header.AnimateIn();
				m_Footer.AnimateIn();
				m_Footer.StartDimTimeout(OnFooterDim);
			}
			else if (CurrentMode == ApplicationModes.ImageDetails)
			{
				CurrentMode = ApplicationModes.ImageView;
				m_Header.AnimateOut();
				m_Footer.AnimateDim();
			}
		}
		#endregion


		#region EVENT HANDLERS - OTHER
		void OnShowCategorySelect()
		{
			CurrentMode = ApplicationModes.CategorySelect;
			SetSelectRecognizerEnabled(false);
			SetHeader(null);
			m_Select.AnimateIn();
			m_Header.AnimateIn();
			m_Footer.AnimateOut();
		}

		void OnCategoryItemSelected(SlideshowCategory category)
		{
			m_Slideshow.SetSlideshowCategory(category);
			m_LastSelectedCategory = category;
			
			CurrentMode = ApplicationModes.ImageDetails;
			SetSelectRecognizerEnabled(true);
			SetHeader(m_Slideshow.CurrentCategory);
			m_Select.AnimateOut();
			m_Footer.AnimateIn();
			m_Footer.StartDimTimeout(OnFooterDim);
		}
		
		void OnCategoryItemFocused(SlideshowCategory category)
		{
			m_Slideshow.SetSlideshowCategory(category);
		}
		
		void OnFooterDim()
		{
			if (CurrentMode != ApplicationModes.CategorySelect)
			{
				m_Header.AnimateOut();
				CurrentMode = ApplicationModes.ImageView;
			}
		}
		#endregion
		
		
		#region CONTROLS
		/// <summary>
		/// Sets the enabled state of the gesture recognizer for the remote's select button.
		/// </summary>
		/// <param name="enabled">If set to <c>true</c> enabled.</param>
		void SetSelectRecognizerEnabled(bool enabled)
		{
			View.GestureRecognizers[1].Enabled = enabled;
		}
		#endregion


		#region USER INTERFACE
		/// <summary>
		/// Sets the provided image as the displayed image. This intiates a transition from the current image to the new image.
		/// </summary>
		/// <param name="image">Image to display.</param>
		void SetBackground(UnsplashImage image)
		{	
			// Fade out old view
			UIView.Animate(
				m_Slideshow.TransitionDuration / 1000f,
				() => { m_ImageViews[m_CurrentImageIdx].Alpha = 0f; }
			);
			
			// Increment to next view
            m_CurrentImageIdx = (++m_CurrentImageIdx % m_ImageViews.Length);
			
			// Fade in new view
			UIView.Animate(
				m_Slideshow.TransitionDuration / 1000f, 
				() => { m_ImageViews[m_CurrentImageIdx].Alpha = 1f; }
			);
			
			// Assign new image
			var data = NSData.FromArray(image.custom.imageData);
			m_ImageViews[m_CurrentImageIdx].Image = UIImage.LoadFromData(data);
		}
		
		/// <summary>
		/// Sets the items to display on the footer view according to the provided image.
		/// </summary>
		/// <param name="image">Image that should be represented.</param>
		void SetFooter(UnsplashImage image)
		{
			m_Footer.SetAuthorText(image.user.name, image.user.username);
			m_Footer.SetLocationText(image.location.city, image.location.country);
		}
		
		/// <summary>
		/// Sets the items to display on the header view.
		/// </summary>
		/// <param name="category">The category that has been selected.</param>
		void SetHeader(SlideshowCategory category)
		{
			m_Header.SetCategoryText(category?.DisplayName);
		}
		
		/// <summary>
		/// Presents an alert to the user, indicating that something went wrong.
		/// </summary>
		/// <param name="message">Alert message to display.</param>
		void ShowAlert(string message)
		{
			if (m_Alert == null)
			{
				m_Alert = BaseView.CreateView<AlertView>(this.View);
				m_Alert.AnimateIn();
			}
			
			m_Alert.SetText(ALERT_HEADER, message);
		}
		
		/// <summary>
		/// Removes the alert from view and destroys it.
		/// </summary>
		/// <param name="image">Image.</param>
		void HideAlert(UnsplashImage image)
		{
			if (m_Alert != null)
			{
				m_Alert.AnimateOutAndRemove();
				m_Alert = null;
			}
		}
		
		/// <summary>
		/// Animates out the starting logo.
		/// </summary>
		void HideStartUpLogo(UnsplashImage image)
		{
			m_Slideshow.OnImageCycled -= HideStartUpLogo;
			
			m_Logo.AnimateOutAndRemove();
			
			// Only tell the header to display if we are in the correct mode to do so.
			if (CurrentMode == ApplicationModes.CategorySelect || CurrentMode == ApplicationModes.ImageDetails)
			{
				m_Header.AnimateIn();
			}
		}
		#endregion
	}
}