using System;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace MyView
{
	/// <summary>
	/// The main screen of the app.
	/// </summary>
	public partial class MainViewController : UIViewController
	{
		#region VARIABLES
		private CategorySelectView m_Select;
		private HeaderView m_Header;
		private FooterView m_Footer;

		private bool m_SecondBackgroundActive;
		
		
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
			
			UIImageBackground1.Alpha = 1f;
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
			UIImageView oldImage, newImage;

			if (!m_SecondBackgroundActive)
			{
				oldImage = UIImageBackground1;
				newImage = UIImageBackground2;
			}
			else
			{
				oldImage = UIImageBackground2;
				newImage = UIImageBackground1;
			}

			UIView.Animate(
				1000, 
				() => { oldImage.Alpha = 0f; }
			);
			UIView.Animate(1000, 
				() => { newImage.Alpha = 1f; }
			);

			m_SecondBackgroundActive = !m_SecondBackgroundActive;
		}
		#endregion


		#region DEBUG
		async Task Cycle()
		{
			for (;;)
			{
				await Task.Delay(2000);
				ShowImageInterface(true);

				await Task.Delay(2000);
				ShowImageInterface(false);
				
				await Task.Delay(2000);
				ShowSelectionInterface(true);
				
				await Task.Delay(2000);
				ShowSelectionInterface(false);
				
				await Task.Delay(2000);
				SetBackground();
			}
		}
		#endregion
	}
}