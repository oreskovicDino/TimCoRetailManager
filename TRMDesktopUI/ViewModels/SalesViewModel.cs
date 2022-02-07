namespace TRMDesktopUI.ViewModels
{
    using AutoMapper;
    using Caliburn.Micro;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using TRMDesktop.Library.Api;
    using TRMDesktop.Library.Helpers;
    using TRMDesktop.Library.Models;
    using TRMDesktopUI.Models;

    public class SalesViewModel : Screen
    {
        private int itemQuantity = 1;
        private IProductEndpoint productEndpoint;
        private IConfigHelper configHelper;
        private ISaleEndpoint saleEndpoint;
        private IMapper mapper;
        private StatusInfoViewModel status;
        private IWindowManager window;
        private BindingList<ProductDisplayModel> product;
        private ProductDisplayModel selectedProduct;
        private CartItemDisplayModel selectedCartItem;
        private BindingList<CartItemDisplayModel> cart = new BindingList<CartItemDisplayModel>();

        public SalesViewModel(
            IProductEndpoint productEndpoint,
            IConfigHelper configHelper,
            ISaleEndpoint saleEndpoint,
            IMapper mapper,
            StatusInfoViewModel status,
            IWindowManager window
            )
        {
            this.productEndpoint = productEndpoint;
            this.configHelper = configHelper;
            this.saleEndpoint = saleEndpoint;
            this.mapper = mapper;
            this.status = status;
            this.window = window;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLopcation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message == "Unauthorized")
                {
                    status.UpdateMessage("Unauthorize Access!", "You do not have permission to interact wiht the Sale Form.");
                    window.ShowDialog(status, null, settings);
                }
                else
                {
                    status.UpdateMessage("Fatal Exception!", ex.Message);
                    window.ShowDialog(status, null, settings);
                }
                TryClose();
            }
        }

        public BindingList<ProductDisplayModel> Products
        {
            get { return product; }
            set
            {
                product = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public ProductDisplayModel SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public CartItemDisplayModel SelectedCartItem
        {
            get { return selectedCartItem; }
            set
            {
                selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }

        private async Task ResetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            await LoadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
        }

        public BindingList<CartItemDisplayModel> Cart
        {
            get { return cart; }
            set
            {
                cart = value;
                NotifyOfPropertyChange(() => Cart);
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

        public bool CanCheckout
        {
            get
            {
                bool output = false;

                if (Cart.Count > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
                {
                    output = true;
                }
                return output;
            }
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

        public string Tax
        {
            get
            {
                return CalculateTax().ToString("C");
            }
        }

        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();
                return total.ToString("C");
            }
        }

        private async Task LoadProducts()
        {
            var produtList = await productEndpoint.GetAll();
            var products = mapper.Map<List<ProductDisplayModel>>(produtList);
            Products = new BindingList<ProductDisplayModel>(products);
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

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = configHelper.GetTaxRate() / 100;

            taxAmount = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);
            return taxAmount;
        }

        public void AddToCart()
        {

            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                CartItemDisplayModel CartItemDisplayModel = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(CartItemDisplayModel);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
        }

        public void RemoveFromCart()
        {


            SelectedCartItem.Product.QuantityInStock += 1;
            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckout);
            NotifyOfPropertyChange(() => CanAddToCart);
        }

        public async Task Checkout()
        {
            SaleModel sale = new SaleModel();
            foreach (var item in Cart)
            {
                //Add exception hadeling 
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });

            }

            await saleEndpoint.PostSale(sale);

            await ResetSalesViewModel();
        }

    }
}
