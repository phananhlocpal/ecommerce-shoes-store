import axios from 'axios';
import { variables } from './variables.js';

const API_URL = variables.CART_ITEM_API;

const getCartItems = async () => {
  const response = await axios.get(API_URL);
  return response.data;
};

const getCartItemsByCartId = async (cartId) => {
  const response = await axios.get(`${API_URL}/${cartId}`);
  return response.data;
};

const addCartItem = async (cartItem) => {
  const response = await axios.post(API_URL, cartItem);
  return response.data;
};

const updateCartItem = async (cartItemId, cartItem) => {
  const response = await axios.put(`${API_URL}/${cartItemId}`, cartItem);
  return response.data;
};

const deleteCartItem = async (cartItemId) => {
  const response = await axios.delete(`${API_URL}/${cartItemId}`);
  return response.data;
};

export default {
  getCartItems,
  getCartItemsByCartId,
  addCartItem,
  updateCartItem,
  deleteCartItem
};
