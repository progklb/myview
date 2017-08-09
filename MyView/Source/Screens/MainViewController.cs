using System;
using System.Threading.Tasks;
using UIKit;

namespace MyView
{
	/// <summary>
	/// The main screen of the app.
	/// </summary>
	public partial class MainViewController : UIViewController
	{
		#region PROPERTIES
        public int TransitionDuration { get; set; } = 2000;
		#endregion
        
        
		#region VARIABLES
		private CategorySelectView m_Select;
		private HeaderView m_Header;
		private FooterView m_Footer;

		private UIImageView[] m_ImageViews;
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

			Cycle();
		}
		#endregion


		#region HELPERS
		/// <summary>
		/// Initialises the interface for the first time.
		/// </summary>
		void InitialiseInterface()
		{
			m_Select = BaseView.CreateView<CategorySelectView>(this.View);
			m_Header = BaseView.CreateView<HeaderView>(this.View);
			m_Footer = BaseView.CreateView<FooterView>(this.View);
			
			m_ImageViews = new UIImageView[] { UIImageBackground1, UIImageBackground2 };
			
			UIImageBackground1.Alpha = 0f;
			UIImageBackground2.Alpha = 0f;
		}

		/// <summary>
		/// Shows or hides the category selection interface.
		/// </summary>
		/// <param name="show">If set to <c>true</c> show.</param>
		void ShowSelectionInterface(bool show)
		{
			if (show)
			{
				m_Select.AnimateIn();
			}
			else
			{
				m_Select.AnimateOut();
			}
		}

		/// <summary>
		/// Shows or hides the image view interface.
		/// </summary>
		/// <param name="show">If set to <c>true</c> show.</param>
		void ShowImageInterface(bool show)
		{
			if (show)
			{
				m_Header.AnimateIn();
				m_Footer.AnimateIn();
			}
			else
			{
				m_Header.AnimateOut();
				m_Footer.AnimateOut();
			}
		}

		void SetBackground()
		{	
			// Fade out old view		
			UIView.Animate(
				TransitionDuration, 
				() => { m_ImageViews[m_CurrentImageIdx].Alpha = 0f; }
			);
			
			// Increment to next view
            m_CurrentImageIdx = (++m_CurrentImageIdx % m_ImageViews.Length);
			
			// Fade in new view
			UIView.Animate(
				TransitionDuration, 
				() => { m_ImageViews[m_CurrentImageIdx].Alpha = 1f; }
			);
		}
		#endregion


		#region DEBUG
		async Task Cycle()
		{
			await Task.Delay(3000);
			ShowImageInterface(true);
			await Task.Delay(3000);
			ShowImageInterface(false);
			
			await Task.Delay(3000);
			ShowSelectionInterface(true);
			await Task.Delay(3000);
			ShowSelectionInterface(false);
				
			for (;;)
			{
				await Task.Delay(2000);
				SetBackground();
			}
		}
		#endregion
	}
}