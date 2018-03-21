using System;

namespace MyView.Views
{
    /// <summary>
    /// Provides various options for the user to adjust app behaviour.
    /// </summary>
    public partial class OptionsView : AdvancedBaseView
    {
        #region CONSTRUCTOR
        public OptionsView(IntPtr handle) : base(handle) { }
        #endregion


        #region INHERITED METHODS
        public override void AnimateIn()
        {
            base.AnimateIn();

            UISegDisplay.ValueChanged += OnDisplayDurationChanged;
            UISegTransition.ValueChanged += OnTransitionDurationChanged;

            UIButtonPersistentDetails.PrimaryActionTriggered += OnPersistentDetailsChanged;
            UIButtonBlockPhoto.PrimaryActionTriggered += OnBlockPhoto;
            UIButtonBlockUser.PrimaryActionTriggered += OnBlockUser;
        }

        public override void AnimateOut()
        {
            base.AnimateOut();

            UISegDisplay.ValueChanged -= OnDisplayDurationChanged;
            UISegTransition.ValueChanged -= OnTransitionDurationChanged;

            UIButtonPersistentDetails.PrimaryActionTriggered -= OnPersistentDetailsChanged;
            UIButtonBlockPhoto.PrimaryActionTriggered -= OnBlockPhoto;
            UIButtonBlockUser.PrimaryActionTriggered -= OnBlockUser;
        }
        #endregion


        #region EVENT HANDLERS
        void OnDisplayDurationChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Display duration changed");
        }

        void OnTransitionDurationChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Transition duration changed");
        }

        void OnPersistentDetailsChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Persistent details changed");
        }

        void OnBlockPhoto(object sender, EventArgs e)
        {
            Console.WriteLine("Blocked photo");
        }

        void OnBlockUser(object sender, EventArgs e)
        {
            Console.WriteLine("Blocked user");
        }
        #endregion
    }
}