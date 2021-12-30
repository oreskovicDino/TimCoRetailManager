namespace TRMDesktopUI.ViewModels
{
    using Caliburn.Micro;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using TRMDesktop.Library.Api;
    using TRMDesktop.Library.Helpers;
    using TRMDesktop.Library.Models;

    public class SalesViewModel : Screen
    {
        private int itemQuantity = 1;
        private IProductEndpoint productEndpoint;
        private IConfigHelper configHelper;
        private BindingList<ProductModel> product;
        private ProductModel selectedProduct;
        private BindingList<CartItemModel> cart = new BindingList<CartItemModel>();

        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper)
        {
            this.productEndpoint = productEndpoint;
            this.configHelper = configHelper;
        }

        public BindingList<ProductModel> Products
        {
            get { return product; }
            set
            {
                product = value;
                NotifyOfPropertyChange(() => Products);
            }
        }


        public ProductModel SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }


        public BindingList<CartItemModel> Cart
        {
            get { return cart; }
            set
            {
                cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var produtList = await productEndpoint.GetAll();

            Products = new BindingList<ProductModel>(produtList);
        }

        public int ItemQuantity
        {
            get { return itemQuantity; }
            set
            {
                itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal
        {
            get
            {
                return CalculateSubTotal().ToString("C");
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += (item.Product.RetailPrice * item.QuantityInCart);
            }
            return subTotal;
        }

        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = configHelper.GetTaxRate() / 100;
            foreach (var item in Cart)
            {
                if (item.Product.IsTaxable)
                {
                    taxAmount += (item.Product.RetailPrice * item.QuantityInCart * taxRate);
                }
            }
            return taxAmount;
        }

        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();
                return total.ToString("C");
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                //Make sure something is selected.
                //Make sure item is in stock.
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }

                return output;
            }
        }

        public void AddToCart()
        {

            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel cartItemModel = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(cartItemModel);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                //TODO: Make sure something is selected.

                return output;
            }
        }

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
        }

        public bool CanCheckout
        {
            get
            {
                bool output = false;

                //TODO: Make sure there is something in cart.

                return output;
            }
        }

        public void Checkout()
        {

        }

    }
}
