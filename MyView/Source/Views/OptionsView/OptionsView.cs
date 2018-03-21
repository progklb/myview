using System;

namespace MyView.Views
{
    public partial class OptionsView : AdvancedBaseView
    {
        #region CONSTRUCTOR
        public OptionsView(IntPtr handle) : base(handle) { }
        #endregion


        #region INHERITED METHODS
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }
        #endregion
    }
}