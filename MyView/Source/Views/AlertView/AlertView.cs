using System;

namespace MyView.Views
{
    public partial class AlertView : AdvancedBaseView
    {
    	#region CONSTRUCTOR
        public AlertView (IntPtr handle) : base (handle) { }
        #endregion
        
        
        #region PUBLIC API
        /// <summary>
        /// Assigns the provided parameters to the fields on the alert view.
        /// </summary>
        /// <param name="title">Header text of the alert.</param>
        /// <param name="body">Body text of the alert.</param>
        public void SetText(string title, string body)
        {
        	UILabelTitle.Text = title;
        	UILabelBody.Text = body;
        }
        #endregion
    }
}