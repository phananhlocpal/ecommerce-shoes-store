import { useSelector } from 'react-redux';
import { selectCurrentUser } from './userSlice';

export const useIsAuthenticated = () => {
  const currentUser = useSelector(selectCurrentUser);
  return currentUser !== null;
};
