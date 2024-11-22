import axios from 'axios';
import { variables } from './variables.js';
import jwtDecode from 'jwt-decode';

const API_URL = variables.CUSTOMER_API
const API_URL_AUTHEN = variables.AUTHEN_API
const API_URL_CART = variables.CART_API;
const API_URL_CART_ITEM = variables.CART_ITEM_API;

const getUsers = async () => {
  const response = await axios.get(API_URL);
  return response.data;
}

const getUser = async (userId) => {
  console.log('userId', userId);
  const response = await axios.get(`${API_URL}/${userId}`);
  return response.data;
}

const createUser = async (user) => {
  const response = await axios.post(API_URL, user);
  return response.data;
}

const updateUser = async (userId, user) => {
  const response = await axios.put(`${API_URL}/${userId}`, user);
  return response.data;
}

const deleteUser = async (userId) => {
  const response = await axios.delete(`${API_URL}/${userId}`);
  return response.data;
}

const login = async (loginData) => {
  try {
    const response = await axios.post(`${API_URL_AUTHEN}/login-customer`, loginData);
    if (response.data.token) {
      // Lấy userId từ response và lấy cartId của người dùng
      const userId = response.data.customer.customerID;

      // Kiểm tra xem session Storage có lưu cartItem hay ko?
      const data = sessionStorage.getItem('cartItems');
      if (data) {
        const cartItems = JSON.parse(data).map((item) => ({
          customerId: userId,
          productSizeID: item.productSizeID,
          quantity: item.quantity,
        }));
      
        try {
          axios.post(`${API_URL_CART_ITEM}/updateSession`, cartItems);
          
          // Xóa cartItems khỏi sessionStorage sau khi hoàn thành
          sessionStorage.removeItem('cartItems');
          console.log('Cart items have been successfully updated to the server.');
        } catch (error) {
          console.error('Error updating cart items to the server:', error);
        }
      } else {
        console.log('No cart items found in sessionStorage.');
      }
      
      return  userId; 
    }
    return { token: null };
  } catch (error) {
    if (error.response && error.response.status === 401) {
      alert('Invalid email or password');
    } else {
      alert('An error occurred');
    }
    return { token: null };
  }
};

export default {
  login,
  getUser,
  getUsers,
  createUser,
  updateUser,
  deleteUser
};