namespace Furni.Web.Core.VeiwModels
{
	public class ShoppingCartViewModel
	{
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public Order Order { get; set; }
    }
}
