import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { addToCart, removeFromCart, updateQuantity, calculateSubtotal, getTotal, applyDiscount, clearCart, loadCart } from '../../store/reducers/cartSlice';
import { formatPrice } from './useUtil';
import { selectCurrentUser } from '../../store/reducers/userSlice';

export const useCart = () => {
  const dispatch = useDispatch();
  const items = useSelector(state => state.cart.items);
  const subtotal = useSelector(state => state.cart.subtotal);
  const delivery = useSelector(state => state.cart.delivery);
  const discount = useSelector(state => state.cart.discount);
  const total = useSelector(state => state.cart.total);
  const quantity = items.reduce((acc, item) => acc + item.quantity, 0);

  console.log("items", items);

  const currentUser = useSelector(selectCurrentUser);

  useEffect(() => {
    dispatch(loadCart(currentUser));  
  }, [dispatch, currentUser]);

  useEffect(() => {
    dispatch(calculateSubtotal());
  }, [items, dispatch, quantity]);

  useEffect(() => {
    dispatch(getTotal());
  }, [subtotal, delivery, dispatch, discount]);

  const addToCartHandler = (item) => {
    dispatch(addToCart({ cartItemID: item.cartItemID,  product: item.product, productPrice: item.productPrice, productSizeID: item.productSizeID, productSize: item.productSize, quantity: item.quantity, currentUser: currentUser }));
  };
  
  const removeFromCartHandler = (itemId, size) => {
    const item = items.find((item) => item.product.productID === itemId && item.productSize === size); 
    console.log(item)
    dispatch(removeFromCart({ cartItemID: item.cartItemID, productID: itemId, productSize: size, currentUser: currentUser }));
  };
  
  const updateQuantityHandler = (updatedItem) => {
    if (updatedItem.quantity === 0) {
      const item = items.find((item) => item.product.productID === updatedItem.product.productID && item.productSize === updatedItem.productSize);
      if (item) {
        dispatch(removeFromCart({ cartItemID: updatedItem.cartItemID, productID: updatedItem.product.productID, productSize: updatedItem.productSize, currentUser: currentUser}));
      }
    } else if (updatedItem.quantity > 0) {
      dispatch(updateQuantity({ cartItemID: updatedItem.cartItemID, product: updatedItem.product, productPrice: updatedItem.productPrice, productSizeID: updatedItem.productSizeID, productSize: updatedItem.productSize, quantity: updatedItem.quantity, currentUser: currentUser }));
    }
  };

  const clearCartHandler = () => {
    dispatch(clearCart({currentUser}));
  };

  const applyDiscountHandler = (discountCode) => {
    const discount = 0.1;
    if(discountCode.toLowerCase() === "10off"){
      dispatch(applyDiscount({ discount }));
    } else{
      alert("Wrong discount")
    }
  }; 

  return { 
    addToCart: addToCartHandler, 
    removeFromCart: removeFromCartHandler, 
    updateQuantity: updateQuantityHandler, 
    clearCart: clearCartHandler, 
    applyDiscount: applyDiscountHandler,
    items, 
    defaultSubtotal: subtotal,
    defaultTotal: total, 
    subtotal: formatPrice(subtotal), 
    delivery: formatPrice(delivery), 
    total: formatPrice(total), 
    quantity,
    discount
  };
};



