namespace CameraDiplomat.DTO
{
	public class ProductViewModel
	{
		public bool NameCorrect { get; set; }
		public bool QualityCorrect { get; set; }
		public bool CodeCorrect { get; set; }
		public bool TextCorrect { get; set; }

		public string Name { get; set; }
		public string Quality { get; set; }
		public string QRCode { get; set; }
		public string Text { get; set; }

		public ProductViewModel (bool productNameCorrect, bool productQualityCorrect, bool productCodeCorrect,bool productTextCorrect,
								string productName, string productQuality, string productQRCode, string productText)
		{
			this.NameCorrect = productNameCorrect;
			this.QualityCorrect = productQualityCorrect;
			this.CodeCorrect = productCodeCorrect;
			this.TextCorrect = productTextCorrect;
			this.Name = productName;
			this.Quality = productQuality;
			this.QRCode = productQRCode;
			this.Text = productText;
		}
	}
}
