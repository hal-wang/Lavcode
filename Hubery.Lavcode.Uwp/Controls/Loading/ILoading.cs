namespace Hubery.Lavcode.Uwp.Controls.Loading
{
    public interface ILoading
    {
        public bool Clickable { get; set; }
        public double BackgroundOpacity { get; set; }
        public double PaneOpacity { get; set; }
        public double Size { get; set; }
        public string LoadingStr { get; set; }
    }
}
