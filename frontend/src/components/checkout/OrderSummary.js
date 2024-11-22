import React, { useEffect, useState } from 'react';
import { PayPalScriptProvider, PayPalButtons } from '@paypal/react-paypal-js';
import { useCart } from "../../utils/hooks/useCart";
import { selectCurrentUser } from "../../store/reducers/userSlice";
import { useSelector } from 'react-redux';
import orderApi from '../../utils/api/orderApi';

function OrderSummary({ onPaymentComplete }) {
  const [usdTotal, setUsdTotal] = useState(null); // Store the converted total in USD
  const { subtotal, delivery, discount, defaultTotal, clearCart } = useCart();
  const currentUser = useSelector((selectCurrentUser));
  const buttonStyles = {
    layout: 'vertical',
    color: 'blue',
    label: 'checkout',
  };

  useEffect(() => {
    // Fetch exchange rate from a currency conversion API
    const fetchExchangeRate = async () => {
      try {
        const response = await fetch('https://api.exchangerate-api.com/v4/latest/VND');
        const data = await response.json();
        const vndToUsdRate = data.rates.USD; // Extract the USD rate from the API response
        const convertedTotal = defaultTotal * vndToUsdRate; // Convert VND to USD
        setUsdTotal(convertedTotal.toFixed(2)); // Update state with the USD total
      } catch (error) {
        console.error('Error fetching exchange rate:', error);
      }
    };

    fetchExchangeRate();
  }, [defaultTotal]);

  const onApprovePaypal = async (data, actions) => {
    const order = await actions.order.capture();
    console.log('Order details:', order);
    const email = order.payer.email_address;
    const transactionId = order.purchase_units[0].payments.captures[0].id;
    alert(`An order confirmation will be sent to email: ${email}. Transaction ID: ${transactionId}.`);

    // Prepare order data
    const orderData = {
      totalPrice: defaultTotal,
      transactionId: transactionId,
      customerID: currentUser.customerID,
    };

    console.log('Order data:', orderData);

    // Send order data to the backend
    try {
      const response = await orderApi.createOrderPaypal(orderData);
      console.log('Order saved successfully:', response.data);
      onPaymentComplete();
    } catch (error) {
      console.error('Error saving order:', error);
    }
  };

  if (usdTotal === null) {
    return <div>Loading...</div>; 
  }

  return (
    <div className='order-summary'>
      <div className="space-between">
        <p>Subtotal</p>
        <p>{subtotal}</p>
      </div>
      {discount > 0 && (
        <div className="space-between">
          <p>Discount</p>
          <p>-10%</p>
        </div>
      )}
      <div className="space-between">
        <p>Delivery</p>
        <p>{delivery}</p>
      </div>
      <div className="line"></div>
      <div className="space-between bold">
        <div>
          <p>Total</p>
        </div>
        <div className='items-end'>
          <p>{defaultTotal} VND</p>
          <p>{usdTotal} USD</p>
        </div>
      </div>
      <PayPalScriptProvider options={{ "client-id": "ASWQENE-qmdKB-AOzSTZFtuJfz8v26F7NxtFgpgAMvDGaeACJBuz6EOXju2d5KlXJ9h2QoJRM6XrpHi_", currency: "USD" }}>
        <PayPalButtons
          style={buttonStyles}
          createOrder={(data, actions) => {
            return actions.order.create({
              purchase_units: [
                {
                  amount: {
                    currency_code: "USD",
                    value: usdTotal, 
                  },
                },
              ],
            });
          }}
          onApprove={onApprovePaypal}
        />
      </PayPalScriptProvider>
    </div>
  );
}

export default OrderSummary;