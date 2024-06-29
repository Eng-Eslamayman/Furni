namespace Furni.Web.Core.VeiwModels
{
	public class CustomArrivalProductViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public float Price { get; set; }
		public IList<string> ImageUrls { get; set; }
	}
}
