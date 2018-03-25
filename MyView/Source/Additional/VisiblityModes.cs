namespace MyView.Additional
{
    public enum LocationVisibilityMode
    {
        /// Location will never be displayed
        Never = 0,
        /// Location will only be displayed when the footer is at full opacity (after animating in).
        Selected = 1,
        /// Location will always be displayed.
        Always = 2
    }

    public enum AuthorVisibilityMode
    {
        /// Item will only be displayed when the footer is at full opacity (after animating in).
        Selected = 0,
        /// Item will always be displayed.
        Always = 1
    }
}
