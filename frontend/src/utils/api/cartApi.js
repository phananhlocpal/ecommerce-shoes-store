import axios from 'axios';
import { variables } from './variables.js';

const API_URL = variables.CART_API;

const getCarts = async () => {
  const response = await axios.get(API_URL);
  return response.data;
};

const getCartByUserId = async (userId) => {
  const response = await axios.get(`${API_URL}/${userId}`);
  return response.data;
};

const createCart = async (cart) => {
  const response = await axios.post(API_URL, cart);
  return response.data;
};

const updateCart = async (cartId, cart) => {
  const response = await axios.put(`${API_URL}/${cartId}`, cart);
  return response.data;
};

const deleteCart = async (cartId) => {
  const response = await axios.delete(`${API_URL}/${cartId}`);
  return response.data;
};

export default {
  getCarts,
  getCartByUserId,
  createCart,
  updateCart,
  deleteCart
};
