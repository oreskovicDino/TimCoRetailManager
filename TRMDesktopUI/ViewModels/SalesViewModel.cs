namespace TRMDesktopUI.ViewModels
{
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TRMDesktop.Library.Api;
    using TRMDesktop.Library.Models;

    public class SalesViewModel: Screen
    {
        private BindingList<ProductModel> product;

        public BindingList<ProductModel> Products
        {
            get { return product; }
            set
            { 
                product = value; 
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel selectedProduct;

        public ProductModel SelectedProduct
        {
            get { return selectedProduct; }
            set {
                selectedProduct = value;
                NotifyOfPropertyChange(()=> SelectedProduct);
                NotifyOfPropertyChange(()=> CanAddToCart);
            }
        }


        private BindingList<CartItemModel> cart = new BindingList<CartItemModel>();

        public BindingList<CartItemModel> Cart
        {
            get { return cart; }
            set
            {
                cart = value; 
                NotifyOfPropertyChange(() => Cart);
            }
        }
                
        private int itemQuantity = 1;

        private IProductEndpoint productEndpoint;

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            this.productEndpoint = productEndpoint;
            
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
            set { 
                itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal { 
            get {
                decimal subTotal = 0;
                foreach (var item in Cart)
                {
                    subTotal += (item.Product.RetailPrice * item.QuantityInCart);
                }
                return subTotal.ToString("C");
            }
        }

        public string Tax { get { return "$0.00"; } }

        public string Total { get { return "$0.00"; } }

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
