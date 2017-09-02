using System;
using System.Threading.Tasks;

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
		private const int IMAGE_INTERFACE_TIMEOUT = 5000;
		#endregion
		
		
		#region PROPERTIES
		/// The current mode that we are viewing the app in.
		public ApplicationModes CurrentMode { get; private set; }
		#endregion
        
        
		#region VARIABLES
		// Additional UI elements
		private CategorySelectView m_Select;
		private HeaderView m_Header;
		private FooterView m_Footer;
		private AlertView m_Alert;
		
		/// Provides images for display on this page
        private SlideshowAdapter m_Slideshow;
        /// Keeps track of our last selected category.
        private SlideshowCategory m_LastSelectedCategory;

		/// A cache of the two image views for displaying images to the user
		private UIImageView[] m_ImageViews;
		/// The index of the currently display image view.
		private int m_CurrentImageIdx;
		
		/// A reference to an active alert controller. This prevents multiple errors trying to create multiple controllers.
		private UIAlertController m_AlertController;
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
			
			m_Slideshow = new SlideshowAdapter();
			m_LastSelectedCategory = m_Slideshow.DefaultCategory;
			m_Select.SetCategoryText(m_Slideshow.DefaultCategory.DisplayName);
			
			// We start at the category select screen. Note that we disable the
			// select recognizer so that it does not steal input from the category select view.
			CurrentMode = ApplicationModes.CategorySelect;
			SetSelectRecognizerEnabled(false);
			m_Select.AnimateIn();
			m_Header.AnimateIn();
		}
		
		public override void ViewWillAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			
			// Start image provider
			m_Slideshow.Start();
            
            m_Slideshow.OnImageCycled += SetBackground;
            m_Slideshow.OnImageCycled += SetFooter;
            m_Slideshow.OnErrorThrown += ShowAlert;
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
			
			m_Alert = BaseView.CreateView<AlertView>(this.View);
			
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
		}
		#endregion
		
		
		#region EVENT HANDLERS
		void OnRemoteMenuClicked()
		{
			if (CurrentMode == ApplicationModes.ImageView)
			{
				CurrentMode = ApplicationModes.CategorySelect;
				SetSelectRecognizerEnabled(false);
				m_Footer.AnimateOut();
				m_Header.AnimateIn();
				m_Select.AnimateIn();
			}
			else if (CurrentMode == ApplicationModes.ImageDetails)
			{
				CurrentMode = ApplicationModes.CategorySelect;
				SetSelectRecognizerEnabled(false);
				m_Select.AnimateIn();
				m_Header.AnimateIn();
				m_Footer.AnimateOut();
			}
			else if (CurrentMode == ApplicationModes.CategorySelect)
			{
				m_Slideshow.SetSlideshowCategory(m_LastSelectedCategory);
			
				CurrentMode = ApplicationModes.ImageDetails;
				SetSelectRecognizerEnabled(true);
				SetHeader(m_Slideshow.CurrentCategory);
				m_Select.AnimateOut();
				m_Footer.AnimateIn();
				m_Footer.StartDimTimeout(OnFooterDim);
			}
		}
		
		void OnRemoteSelectClicked()
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
			CurrentMode = ApplicationModes.ImageDetails;
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
		}
		
		/// <summary>
		/// Sets the items to display on the header view.
		/// </summary>
		/// <param name="category">The category that has been selected.</param>
		void SetHeader(SlideshowCategory category)
		{
			m_Header.SetCategoryText(category.DisplayName);
		}
		
		/// <summary>
		/// Presents an alert to the user, indicating that something went wrong.
		/// </summary>
		/// <param name="message">Alert message to display.</param>
		void ShowAlert(string message)
		{
			//if (m_AlertController == null)
			//{
			//	m_AlertController = UIAlertController.Create(ALERT_HEADER, message, UIAlertControllerStyle.Alert);
			//	PresentViewController(m_AlertController, true, completionHandler: () => { m_AlertController = null; });
			//}
			
			m_Alert.SetText(ALERT_HEADER, message);
			m_Alert.AnimateIn();
		}
		#endregion
	}
}