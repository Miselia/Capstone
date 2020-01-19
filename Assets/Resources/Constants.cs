namespace Assets.Resources
{
    public static class Constants
    {
        // When Making New Constants, Look to See if they fit in \\
        // An appropriate Category. If not, make a new comment header \\
        // And start a new section describing the general use of the new constants \\

        // For example, this is the header for the variables related to the Game space itself \\
        public static readonly int PlayerBoundaryOffset = 10;
        public static readonly int PlayerBoundarySize = 13;
        public static readonly int PlayerMaximumHealth = 3;
        public static readonly float PlayerRadius = 0.5f;

        // Entities Don't really need constants since their constants are only used internally \\
        // However, if we decide we need them we can un-comment and then add them to Entity classes \\
        //public static readonly float PlayerBoundaryScale = 10.125f;
        //public static readonly float UniversalCircleEntityScale = 2.35f;
    }
}
