namespace SABA_CH.Global
{
    public class Setting
    {
        public int FontSize { get; set; }
        public string Fontfamily { get; set; }
        public string RowColor { get; set; }
        public Setting(int FontSize, string Fontfamily, string RowColor)
        {
            this.FontSize = FontSize;
            this.Fontfamily = Fontfamily;
            this.RowColor = RowColor;
        }
    }
}
